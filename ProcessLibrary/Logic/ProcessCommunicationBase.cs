namespace ProcessCommunication.ProcessLibrary.Logic;

/// <summary>
/// The process communication base class
/// </summary>
public abstract class ProcessCommunicationBase
{
    private readonly ISerializerHelper serializerHelper;

    /// <summary>
    /// The internal logger
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// The serializer helper
    /// </summary>
    protected ISerializerHelper SerializerHelper => serializerHelper;


    /// <summary>
    /// Gets or sets the IP address
    /// </summary>
    protected string IpAddress { get; }

    /// <summary>
    /// Gets or sets the port
    /// </summary>
    protected int Port { get; }

    /// <summary>
    /// Create a new instance of ProcessCommunicationBase
    /// </summary>
    protected ProcessCommunicationBase(
        NotNull<ILogger> logger, 
        NotNull<ISerializerHelper> serializerHelper, 
        NotEmptyOrWhiteSpace ipAddress, 
        int port)
    {
        this.serializerHelper = serializerHelper.Value;
        this.Logger = logger.Value;
        IpAddress = ipAddress.Value;
        Port = port;
    }

    /// <summary>
    /// The can continue method
    /// </summary>
    /// <param name="tcpClient">The client</param>
    /// <param name="token">The cancellation token</param>
    /// <returns></returns>
    protected static bool CanContinue(TcpClient tcpClient, CancellationToken token)
    {
        return !token.IsCancellationRequested && tcpClient.Connected;
    }

}
