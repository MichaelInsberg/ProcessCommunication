using System.Collections.Concurrent;
using System.Text;
using ProcessCommunication.ProcessLibrary.DataClasses;

namespace ProcessCommunication.ProcessLibrary.Logic;

/// <summary>
/// The process manager class
/// </summary>
public sealed class ProcessManager : IDisposable
{
    private volatile bool isDisposed;
    private readonly TcpListener server;
    private readonly ILogger logger;
    private readonly SerializerHelper serializerHelper;
    private readonly ConcurrentDictionary<TcpClient, TcpClientItem> connecedClients;

    /// <summary>
    /// Gets or sets the IP address
    /// </summary>
    public string IpAddress { get; }

    /// <summary>
    /// Gets or sets the port
    /// </summary>
    public int Port { get; }

    /// <summary>
    /// Gets the us started indicting if started or not
    /// </summary>
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
        connecedClients = new ConcurrentDictionary<TcpClient, TcpClientItem>();
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
    public void Start(CancellationToken token)
    {
        logger.Log(new NotEmptyOrWhiteSpace($"Try to start server with IpAddress <{IpAddress}> and port number <{Port}>"));
        try
        {
            server.Start();
            _ = Task.Factory.StartNew(() => HandleReceivedCommands(token), TaskCreationOptions.LongRunning);
            _ = Task.Factory.StartNew(() => StartAcceptTcpClients(token), TaskCreationOptions.LongRunning);
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

        while (connecedClients.IsEmpty)
        {
            var item = connecedClients.FirstOrDefault();
            var tcpClient = item.Key;
            if (tcpClient == null)
            {
                continue;
            }
            RemoveClient(tcpClient, new CancellationToken());
        }

        isDisposed = true;
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
            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
            var tcpClientItem = new TcpClientItem(new NotNull<TcpClient>(client), cancellationTokenSource);
            connecedClients.AddOrUpdate(client, tcpClientItem, (_, item) => item);
            _ = Task.Factory.StartNew(() => DoCommunication(client, cancellationTokenSource.Token), TaskCreationOptions.LongRunning);
        }
    }


    private void DoCommunication(TcpClient tcpClient, CancellationToken token)
    {
        var address = "Unknown";
        if (tcpClient.Client.RemoteEndPoint is IPEndPoint endPoint)
        {
            address = endPoint.Address.ToString();
        }
        var canContinue = !token.IsCancellationRequested && tcpClient.Connected;
        while (canContinue)
        {
            try
            {
                var networkStream = tcpClient.GetStream();
                var streamReader = new StreamReader(networkStream, Encoding.Unicode);
                var result = streamReader.ReadLine();
                var obj = serializerHelper.DeSerialize<StartServer>(new NotEmptyOrWhiteSpace(result));
                //ToDo enque the commannd in commad queue
                logger.Log(new NotEmptyOrWhiteSpace($"type received {obj.GetType()}"));
                canContinue = !token.IsCancellationRequested && tcpClient.Connected;
            }
            catch (Exception exception)
            {
                logger.LogException(new NotEmptyOrWhiteSpace($"Exception during communication with {address}"), exception);
                canContinue = false;
            }
        }

        RemoveClient(tcpClient, token);
    }

    private void RemoveClient(TcpClient tcpClient, CancellationToken token)
    {
        if (!connecedClients.TryRemove(tcpClient, out var item))
        {
            Task.Delay(1, token).Wait(token);
        }
        item?.Dispose();
    }

    private void HandleReceivedCommands(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
        }
    }
}
