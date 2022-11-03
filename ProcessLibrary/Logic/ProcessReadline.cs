namespace ProcessCommunication.ProcessLibrary.Logic;

/// <summary>
/// The process read line class
/// </summary>
public sealed class ProcessReadline
{
    private readonly ILogger logger;

    /// <summary>
    /// Create a new instance of ProcessReadline
    /// </summary>
    public ProcessReadline(ILogger logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// The read line method
    /// </summary>
    /// <param name="client">The client</param>
    /// <param name="address">The address</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>The received line</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public string Readline(
        IProcessTcpClient client,
        string address ,
        CancellationToken token)
    {
        logger.Log($"Waiting for command {address}");
        var networkStream = client.GetStream();
        token.ThrowIfCancellationRequested();
        var streamReader = new StreamReader(networkStream);
        var result = streamReader.ReadLine();
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException($"Can not read line form {nameof(client)}");
        }
        return result;

    }
}
