namespace ProcessCommunication.ProcessLibrary.Logic;

/// <summary>
/// The process manager class
/// </summary>
public sealed class ProcessManager : ProcessCommunicationBase, IDisposable
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
    public ProcessManager(NotNull<ILogger> logger, NotEmptyOrWhiteSpace ipAddress, int port): base(logger, ipAddress, port)
    {
        var ipPAddress = IPAddress.Parse(ipAddress.Value);
        server = new TcpListener(ipPAddress, port);
        IsStarted = false;
    }

    /// <summary>
    /// Destructor for instance of ProcessManager
    /// </summary>
    ~ProcessManager()
    {
        Dispose(false);
    }

    /// <summary>
    /// Start the process communication 
    /// </summary>
    /// <param name="token">The cancellation token</param>
    /// <param name="processCommandHandlerCreator">The function to create the process command handler</param>
    public void Start(CancellationToken token, Func<IProcessCommandHandler> processCommandHandlerCreator)
    {
        Logger.Log(new NotEmptyOrWhiteSpace(
            $"Try to start server with IpAddress <{IpAddress}> and port number " +
            $"<{Port.ToString(CultureInfo.InvariantCulture)}>"));
        try
        {
            server.Start();
            _ = Task.Factory.StartNew(() => HandleReceivedCommands(processCommandHandlerCreator, token), TaskCreationOptions.LongRunning);
            IsStarted = true;
            Logger.Log(new NotEmptyOrWhiteSpace(
                $"Started server with IpAddress <{IpAddress}>" +
                $" and port number <{Port.ToString(CultureInfo.InvariantCulture)}>"));
        }
        catch (Exception exception)
        {
            IsStarted = false;
            Logger.LogException(new NotEmptyOrWhiteSpace(
                "Exception when try to start server" +
                $" IpAddress <{IpAddress}> and port number " +
                $"<{Port.ToString(CultureInfo.InvariantCulture)}>"), exception);
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


    private void HandleReceivedCommands(Func<IProcessCommandHandler> funcProcessCommandHandler, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(Thread.CurrentThread.Name))
        {
            Thread.CurrentThread.Name = $"{nameof(ProcessManager)}| {nameof(HandleReceivedCommands)}";
        }

        while (!token.IsCancellationRequested)
        {
            var client = server.AcceptTcpClient();
            _ = Task.Factory.StartNew(() => DoCommunication(client, funcProcessCommandHandler, token), TaskCreationOptions.LongRunning);
        }
    }

    private void DoCommunication(TcpClient tcpClient, Func<IProcessCommandHandler> funcProcessCommandHandler, CancellationToken token)
    {
        var address = "Unknown";
        if (tcpClient.Client.RemoteEndPoint is IPEndPoint endPoint)
        {
            address = endPoint.Address.ToString();
        }
        Logger.Log(new NotEmptyOrWhiteSpace($"Start communication with {address}"));
        var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        try
        {
            var canContinue = !token.IsCancellationRequested && tcpClient.Connected;
            while (canContinue)
            {
                try
                {
                    Logger.Log(new NotEmptyOrWhiteSpace($"Waiting for command {address}"));
                    var networkStream = tcpClient.GetStream();
                    var streamReader = new StreamReader(networkStream);
                    var result = streamReader.ReadLine();
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        continue;
                    }
                    Logger.Log(new NotEmptyOrWhiteSpace($"Receive command {result}"));
                    _ = Task.Factory.StartNew(() => SendResponse(tcpClient, result, funcProcessCommandHandler, cts.Token), token);
                    canContinue = !token.IsCancellationRequested && tcpClient.Connected;
                }
                catch (Exception exception)
                {
                    Logger.LogException(new NotEmptyOrWhiteSpace($"Exception during communication with {address}"), exception);
                    canContinue = false;
                }
            }
        }
        finally
        {
            //ToDo how to dispose cts an Tcp client
            Logger.Log(new NotEmptyOrWhiteSpace($"Finished communication with {address}"));
            cts.Cancel();
        }
    }

    private static void SendResponse(
        TcpClient tcpClient, 
        string processCommand, 
        Func<IProcessCommandHandler> funcProcessCommandHandler, 
        CancellationToken token)
    {
        var processHandler = funcProcessCommandHandler.Invoke();
        var client = new ProcessTcpTcpClient(new NotNull<TcpClient>(tcpClient));
        var processClient = new NotNull<TcpClient>(tcpClient);
        processHandler.HandelCommand(processClient, new NotEmptyOrWhiteSpace(processCommand), token);
    }
}
