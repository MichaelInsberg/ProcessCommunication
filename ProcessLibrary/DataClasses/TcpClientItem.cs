using System.Collections.Concurrent;

namespace ProcessCommunication.ProcessLibrary.DataClasses;

/// <summary>
/// The TCP client item class
/// </summary>
public sealed class TcpClientItem : IDisposable
{
    private volatile bool isDisposed;
    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly TcpClient client;
    private readonly ConcurrentQueue<string> commandQueue;

    /// <summary>
    /// Gets the command queue
    /// </summary>
    public ConcurrentQueue<string> CommandQueue => commandQueue;

    /// <summary>
    /// Create a new instance of TcpClientItem
    /// </summary>
    public TcpClientItem(TcpClient client, CancellationTokenSource cancellationTokenSource)
    {
        this.cancellationTokenSource = cancellationTokenSource;
        this.client = client;
        commandQueue = new ConcurrentQueue<string>();
    }

    /// <summary>
    /// Destructor for instance of TcpClientItem
    /// </summary>
    ~TcpClientItem()
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
        
        cancellationTokenSource.Dispose();
        
        client.Dispose();
        
        isDisposed = true;
    }
}
