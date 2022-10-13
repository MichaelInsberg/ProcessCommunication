using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using ProcessCommunication.ProcessLibrary.DataClasses;
using ProcessCommunication.ProcessLibrary.DataClasses.Commands;
using ProcessCommunication.ProcessLibrary.DataClasses.Response;

namespace ProcessCommunication.ProcessLibrary.Logic;

/// <summary>
/// The process manager class
/// </summary>
public sealed class ProcessManager : ProcessCommunicationBase, IDisposable
{
    private readonly TcpListener server;
    private volatile bool isDisposed;
    private readonly ConcurrentDictionary<TcpClient, TcpClientItem> connectedClients;


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
        connectedClients = new ConcurrentDictionary<TcpClient, TcpClientItem>();
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
        Logger.Log(new NotEmptyOrWhiteSpace(
            $"Try to start server with IpAddress <{IpAddress}> and port number " +
            $"<{Port.ToString(CultureInfo.InvariantCulture)}>"));
        try
        {
            server.Start();
            _ = Task.Factory.StartNew(() => ReceivedCommands(token), TaskCreationOptions.LongRunning);
            _ = Task.Factory.StartNew(() => HandleRecievedCommnads(token), TaskCreationOptions.LongRunning);
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

        while (connectedClients.IsEmpty)
        {
            var item = connectedClients.FirstOrDefault();
            var tcpClient = item.Key;
            if (tcpClient == null)
            {
                continue;
            }
            RemoveClient(tcpClient, new CancellationToken());
        }

        isDisposed = true;
    }


    private void HandleRecievedCommnads(CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(Thread.CurrentThread.Name))
        {
            Thread.CurrentThread.Name = $"{nameof(ProcessManager)}| {nameof(HandleRecievedCommnads)}";
        }

        while (!token.IsCancellationRequested)
        {
            var client = server.AcceptTcpClient();
            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
            var tcpClientItem = new TcpClientItem(new NotNull<TcpClient>(client), cancellationTokenSource);
            _ = connectedClients.AddOrUpdate(client, tcpClientItem, (_, item) => item);
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
        Logger.Log(new NotEmptyOrWhiteSpace($"Start communication with {address}"));
        var canContinue = !token.IsCancellationRequested && tcpClient.Connected;
        while (canContinue)
        {
            try
            {
                Logger.Log(new NotEmptyOrWhiteSpace($"Waiting for command {address}"));
                var networkStream = tcpClient.GetStream();
                var streamReader = new StreamReader(networkStream, Encoding.Unicode);
                var result = streamReader.ReadLine();
                if (string.IsNullOrWhiteSpace(result))
                {
                    continue;
                }
                var item = TryToGetTcpClientItem(tcpClient, token);
                item.CommandQueue.Enqueue(result);   
                Logger.Log(new NotEmptyOrWhiteSpace($"Receive command {result}"));
                canContinue = !token.IsCancellationRequested && tcpClient.Connected;
            }
            catch (Exception exception)
            {
                Logger.LogException(new NotEmptyOrWhiteSpace($"Exception during communication with {address}"), exception);
                canContinue = false;
            }
        }

        RemoveClient(tcpClient, token);
        Logger.Log(new NotEmptyOrWhiteSpace($"Finished communication with {address}"));
    }

    private TcpClientItem TryToGetTcpClientItem(TcpClient tcpClient, CancellationToken token)
    {
        TcpClientItem item;
        var counter = 0;
        while (!connectedClients.TryGetValue(tcpClient, out item))
        {
            Task.Delay(1, token).Wait(token);
            counter++;
            if (counter >= MAX_RETRIES)
            {
                throw new InvalidOperationException($"Max retry reached in {nameof(TryToGetTcpClientItem)}");
            }
        }

        return item;
    }

    private void RemoveClient(TcpClient tcpClient, CancellationToken token)
    {
        TcpClientItem item;
        var counter = 0;
        while (!connectedClients.TryRemove(tcpClient, out item))
        {
            Task.Delay(1, token).Wait(token);
            counter++;
            if (counter >= MAX_RETRIES)
            {
                throw new InvalidOperationException($"Max retry reached in {nameof(RemoveClient)}");
            }
        }
        item?.Dispose();
    }

    private void ReceivedCommands(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (connectedClients.IsEmpty)
            {
                Task.Delay(1, token).Wait(token);
                continue;
            }
            try
            {
                //This throw an exception is a clint ist added or remove but that does not carw
                foreach ((var client, var clientItem) in connectedClients)
                {
                    if (clientItem.CommandQueue.IsEmpty)
                    {
                        continue;
                    }

                    if (!clientItem.CommandQueue.TryDequeue(out var result))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        continue;
                    }
                    
                    
                    //ToDo Hier vesuchen der string in igrend ein Object zu deserialiseiren
                    var obj = SerializerHelper.DeSerialize<CommandStartServer>(new NotEmptyOrWhiteSpace(result));
                    //Todo und dann hier das Response command senden
                    var networkStream = client.GetStream();
                    var streamWriter = new StreamWriter(networkStream, Encoding.Unicode);
                    var responseStartServer = new ResponseStartServer { IpAddress = IpAddress, IsStarted = true, SerialNumber = "Wurst" };
                    var stringValue = SerializerHelper.Serialize(new NotNull<object>(responseStartServer));
                    streamWriter.WriteLineAsync(stringValue).ConfigureAwait(false);
                    streamWriter.FlushAsync().ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                Logger.LogException(new NotEmptyOrWhiteSpace($"Exception during communication with {nameof(ReceivedCommands)}"), exception);
            }
        }
    }
}
