using ProcessCommunication.ProcessLibrary.DataClasses;

namespace ProcessCommunication.ProcessLibrary.Logic;

/// <summary>
/// The process manager class
/// </summary>
public class ProcessManager
{
    private readonly TcpListener server;
    private readonly ILogger logger;
    private readonly SerializerHelper serializerHelper;

    /// <summary>
    /// Gets or sets the IP address
    /// </summary>
    public string IpAddress { get; }

    /// <summary>
    /// Gets or sets the port
    /// </summary>
    public int Port { get; }

    public bool IsStarted { get; private set; }

    /// <summary>
    /// Create a new instance of ProcessManager
    /// </summary>
    public ProcessManager(NotNull<ILogger> logger, NotEmptyOrWhiteSpace ipAddress, int port)
    {
        serializerHelper = new SerializerHelper();
        this.logger = logger.Value;
        IpAddress = ipAddress.Value;
        Port = port;
        var ipPAddress = IPAddress.Parse(ipAddress.Value);
        server = new TcpListener(ipPAddress, port);
        IsStarted = false;
    }

    /// <summary>
    /// Start the process communication 
    /// </summary>
    /// <param name="token">The cancellation token</param>
    public void Start(CancellationToken token)
    {
        logger.Log(new NotEmptyOrWhiteSpace($"Try to start server with IpAddress <{IpAddress}> and port number <{Port}>"));
        try
        {
            server.Start();
            _ =  Task.Factory.StartNew(() => StartReceivingCommands(token),TaskCreationOptions.LongRunning);
            _ =  Task.Factory.StartNew(() => StartAcceptTcpClients(token), TaskCreationOptions.LongRunning);
            IsStarted = true;
        }
        catch (Exception e)
        {
            IsStarted = false;
            logger.Log(new NotEmptyOrWhiteSpace($"Try to start server with IpAddress <{IpAddress}> and port number <{Port}>"));
            Console.WriteLine(e);
            throw;
        }
    }

    private void StartAcceptTcpClients(CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(Thread.CurrentThread.Name))
        {
            Thread.CurrentThread.Name = $"{nameof(ProcessManager)}| {nameof(StartAcceptTcpClients)}";
        }
        while (!token.IsCancellationRequested)
        {
            var client = server.AcceptTcpClient();
            _ = Task.Factory.StartNew(() => DoCommunication(client, token), TaskCreationOptions.LongRunning);
        }
    }

    private void DoCommunication(TcpClient tcpClient, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(Thread.CurrentThread.Name))
        {
            var address = "Unknown";
            if (tcpClient.Client.RemoteEndPoint is IPEndPoint endPoint)
            {
                address = endPoint.Address.ToString();
            }
            Thread.CurrentThread.Name = $"{nameof(ProcessManager)}| {nameof(DoCommunication)} | {address}";
        }
        while (!token.IsCancellationRequested)
        {
            var networkStream = tcpClient.GetStream();
            var streamReader = new StreamReader(networkStream, System.Text.Encoding.Unicode);
            var result = streamReader.ReadLine();
            var obj = serializerHelper.DeSerialize<StartServer>(new NotEmptyOrWhiteSpace(result));
            logger.Log(new NotEmptyOrWhiteSpace($"type received {obj.GetType()}"));
        }
    }

    private void StartReceivingCommands(CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(Thread.CurrentThread.Name))
        {
            Thread.CurrentThread.Name = $"{nameof(ProcessManager)}| {nameof(StartAcceptTcpClients)}";
        }
        while (!token.IsCancellationRequested)
        {
        }
    }
}
