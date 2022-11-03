namespace ProcessCommunication.ProcessLibrary.Logic;

/// <summary>
/// The process manager class
/// </summary>
public sealed class ProcessServer : ProcessCommunicationBase, IDisposable
{
    private readonly TcpListener server;
    private volatile bool isDisposed;


    /// <summary>
    /// Gets the us started indicting if started or not
    /// </summary>
    public bool IsStarted { get; private set; }

    /// <summary>
    /// Create a new instance of ProcessManager
    /// </summary>
    public ProcessServer(
        ILogger logger, 
        ISerializerHelper serializerHelper, 
        string ipAddress, 
        int port): base(logger, serializerHelper, ipAddress, port)
    {
        var ipPAddress = IPAddress.Parse(ipAddress);
        server = new TcpListener(ipPAddress, port);
        IsStarted = false;
    }

    /// <summary>
    /// Destructor for instance of ProcessManager
    /// </summary>
    ~ProcessServer()
    {
        Dispose(false);
    }

    /// <summary>
    /// Start the process communication 
    /// </summary>
    /// <param name="processCommandHandlerCreator">The function to create the process command handler</param>
    /// <param name="token">The cancellation token </param>
    public void Start(Func<IProcessServerCommunicationHandler> processCommandHandlerCreator, CancellationToken token)
    {
        Logger.Log(
            $"Try to start server with IpAddress <{IpAddress}> and port number " +
            $"<{Port.ToString(CultureInfo.InvariantCulture)}>");
        try
        {
            server.Start();
            //ToDo send message that server is started UDP??
            _ = Task.Factory.StartNew(() => HandleReceivedCommands(processCommandHandlerCreator, token), TaskCreationOptions.LongRunning);
            IsStarted = true;
            Logger.Log(
                $"Started server with IpAddress <{IpAddress}>" +
                $" and port number <{Port.ToString(CultureInfo.InvariantCulture)}>");
        }
        catch (Exception exception)
        {
            IsStarted = false;
            Logger.LogException(
                "Exception when try to start server" +
                $" IpAddress <{IpAddress}> and port number " +
                $"<{Port.ToString(CultureInfo.InvariantCulture)}>", exception);
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

        isDisposed = true;
    }


    private void HandleReceivedCommands(Func<IProcessServerCommunicationHandler> funcProcessCommandHandler, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(Thread.CurrentThread.Name))
        {
            Thread.CurrentThread.Name = $"{nameof(ProcessServer)}| {nameof(HandleReceivedCommands)}";
        }

        while (!token.IsCancellationRequested)
        {
            var client = server.AcceptTcpClient();
            //To to send that new clinet is accepet UDP??
            _ = Task.Factory.StartNew(() => DoCommunication(client, funcProcessCommandHandler, token), TaskCreationOptions.LongRunning);
        }
    }

    private void DoCommunication(TcpClient client, Func<IProcessServerCommunicationHandler> funcProcessCommandHandler, CancellationToken token)
    {
        var address = "Unknown";
        if (client.Client.RemoteEndPoint is IPEndPoint endPoint)
        {
            address = endPoint.Address.ToString();
        }
        Logger.Log($"Start communication with {address}");
        var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        try
        {
            var processReadline = new ProcessReadline(Logger);
            var canContinue = CanContinue(client, token);
            while (canContinue)
            {
                try
                {
                    var processTcpClient = new ProcessTcpClient(client);
                    var result = processReadline.Readline(processTcpClient, address, cts.Token);
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        continue;
                    }
                    _ = Task.Factory.StartNew(() => SendResponse(client, result, funcProcessCommandHandler, cts.Token), token);
                    canContinue = CanContinue(client, token);
                }
                catch (Exception exception)
                {
                    Logger.LogException($"Exception during communication with {address}", exception);
                    canContinue = false;
                }
            }
        }
        finally
        {
            //ToDo how to dispose cts an Tcp client
            Logger.Log($"Finished communication with {address}");
            cts.Cancel();
        }
    }


    private static void SendResponse(
        TcpClient tcpClient, 
        string processCommand, 
        Func<IProcessServerCommunicationHandler> funcProcessCommandHandler, 
        CancellationToken token)
    {
        var processHandler = funcProcessCommandHandler.Invoke();
        var client = new ProcessTcpClient(tcpClient);
        var processClient = client;
        processHandler.HandelCommand(processClient, processCommand, token);
    }
}
