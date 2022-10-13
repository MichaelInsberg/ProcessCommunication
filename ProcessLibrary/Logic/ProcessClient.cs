namespace ProcessCommunication.ProcessLibrary.Logic
{
    /// <summary>
    /// The process client class
    /// </summary>
    public sealed class ProcessClient : ProcessCommunicationBase, IDisposable
    {
        private volatile bool isDisposed;
        private readonly TcpClient client;
        private readonly IPAddress ipPAddress;

        public bool IsConnected { get; private set; }

        /// <summary>
        /// Create a new instance of ProcessClient
        /// </summary>
        public ProcessClient(NotNull<ILogger> logger, NotEmptyOrWhiteSpace ipAddress, int port): base(logger, ipAddress, port)
        {
            client = new TcpClient();
            ipPAddress = IPAddress.Parse(IpAddress);
            IsConnected = false;
        }

        /// <summary>
        /// Destructor for instance of ProcessClient
        /// </summary>
        ~ProcessClient()
        {
            Dispose(disposing: false);
        }

        public void Connect(CancellationToken token, Func<IProgessResponseHandler> progressResponseHandler)
        {
            try
            {
                client.Connect(ipPAddress, Port);
                IsConnected = client.Connected;
                _ = Task.Factory.StartNew(() => ReceivedCommands(progressResponseHandler, token), TaskCreationOptions.LongRunning);
            }
            catch (Exception exception)
            {
                IsConnected = false;
                Logger.LogException(new NotEmptyOrWhiteSpace($"Try to start server with IpAddress <{IpAddress}> and port number <{Port}>"), exception);
                throw;
            }
        }

        private void ReceivedCommands(Func<IProgessResponseHandler> progessResponseHandler, CancellationToken token)
        {
            var canContinue = !token.IsCancellationRequested && client.Connected;
            var processReadline = new ProcessReadline(new NotNull<ILogger>(Logger));
            while (canContinue)
            {
                try
                {
                    var processTcpClient = new ProcessTcpClient(new NotNull<TcpClient>(client));
                    var result = processReadline.Readline(processTcpClient, new NotEmptyOrWhiteSpace(IpAddress), token);
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        continue;
                    }
                    Logger.Log(new NotEmptyOrWhiteSpace($"Receive command {result}"));
                    _ = Task.Factory.StartNew(() => HandleResponse(progessResponseHandler, result), token);
                    canContinue = !token.IsCancellationRequested && client.Connected;
                }
                catch (Exception exception)
                {
                    IsConnected = false;
                    Logger.LogException(new NotEmptyOrWhiteSpace($"Exception during communication with {IpAddress}"), exception);
                    canContinue = false;
                }
            }

            Logger.Log(new NotEmptyOrWhiteSpace($"Finished communication with {IpAddress}"));
        }

        private void HandleResponse(Func<IProgessResponseHandler> progessResponseHandler, string result)
        {
            try
            {
                var processHandler = progessResponseHandler.Invoke();
                processHandler.HandleResponse(new NotEmptyOrWhiteSpace(result));
            }
            catch (Exception exception)
            {
                Logger.LogException(new NotEmptyOrWhiteSpace($"Excpetion in {nameof(HandleResponse)}"),exception);
            }
        }

        /// <summary>
        /// The write command async method
        /// </summary>
        /// <param name="startServer"></param>
        public async Task WriteCommandAsync(CommandBase startServer)
        {
            try
            {
                var networkStream = client.GetStream();
                var streamWriter = new StreamWriter(networkStream);
                var stringValue = SerializerHelper.Serialize(new NotNull<object>(startServer));
                await streamWriter.WriteLineAsync(stringValue).ConfigureAwait(false);
                await streamWriter.FlushAsync().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Logger.LogException(new NotEmptyOrWhiteSpace($"Exception during communication with {IpAddress}"), exception);
                IsConnected = false;
                throw;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern
        /// </summary>
        /// <param name="disposing">is currently disposing</param>
        private void Dispose(bool disposing)
        {
            if (!disposing || isDisposed)
            {
                return;
            }

            if (client.Connected)
            {
                client.Close();
                client.Dispose();
            }

            isDisposed = true;
        }
    }
}
