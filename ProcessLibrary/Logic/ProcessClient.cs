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

        /// <summary>
        /// Gets the is connected
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Create a new instance of ProcessClient
        /// </summary>
        public ProcessClient(
            ILogger logger,             
            ISerializerHelper serializerHelper, 
            string ipAddress, 
            int port): base(logger, serializerHelper,ipAddress, port)
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

        /// <summary>
        /// The connect method
        /// </summary>
        /// <param name="token"></param>
        /// <param name="progressResponseHandler"></param>
        public void Connect(Func<IProgressClientResponseHandler> progressResponseHandler, CancellationToken token)
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
                Logger.LogException($"Try to start server with IpAddress <{IpAddress}> and port number <{Port}>", exception);
                throw;
            }
        }

        private void ReceivedCommands(Func<IProgressClientResponseHandler> progessResponseHandler, CancellationToken token)
        {
            var canContinue = CanContinue(client, token);
            var processReadline = new ProcessReadline(Logger);
            while (canContinue)
            {
                try
                {
                    var processTcpClient = new ProcessTcpClient(client);
                    var result = processReadline.Readline(processTcpClient, IpAddress, token);
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        continue;
                    }
                    _ = Task.Factory.StartNew(() => HandleResponse(progessResponseHandler, result), token);
                    canContinue = CanContinue(client, token);
                }
                catch (Exception exception)
                {
                    IsConnected = false;
                    Logger.LogException($"Exception during communication with {IpAddress}", exception);
                    canContinue = false;
                }
            }

            Logger.Log($"Finished communication with {IpAddress}");
        }


        private void HandleResponse(Func<IProgressClientResponseHandler> progressResponseHandler, string result)
        {
            try
            {
                var processHandler = progressResponseHandler.Invoke();
                processHandler.HandleResponse(result);
            }
            catch (Exception exception)
            {
                Logger.LogException($"Exception in {nameof(HandleResponse)}",exception);
            }
        }

        /// <summary>
        /// The write command async method
        /// </summary>
        /// <param name="command"></param>
        public async Task WriteCommandAsync(CommandBase command)
        {
            try
            {
                var networkStream = client.GetStream();
                var streamWriter = new StreamWriter(networkStream);
                var stringValue = SerializerHelper.Serialize(command);
                await streamWriter.WriteLineAsync(stringValue).ConfigureAwait(false);
                await streamWriter.FlushAsync().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Logger.LogException($"Exception during communication with {IpAddress}", exception);
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
