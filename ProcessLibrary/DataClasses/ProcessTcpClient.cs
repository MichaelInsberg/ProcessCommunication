namespace ProcessCommunication.ProcessLibrary.DataClasses;

/// <summary>
/// The process client class
/// </summary>
public sealed class ProcessTcpClient : IProcessTcpClient
{
    private volatile bool isDisposed;
    private readonly TcpClient client;

    /// <summary>
    /// Create a new instance of ProcessClient
    /// </summary>
    public ProcessTcpClient(TcpClient client)
    {
        this.client = client;
    }

    /// <summary>
    /// Destructor for instance of ProcessClient
    /// </summary>
    ~ProcessTcpClient()
    {
        Dispose(disposing: false);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(disposing: true);
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

        client.Dispose();

        isDisposed = true;
    }


    /// <inheritdoc />
    public NetworkStream GetStream()
    {
        return client.GetStream();
    }
}
