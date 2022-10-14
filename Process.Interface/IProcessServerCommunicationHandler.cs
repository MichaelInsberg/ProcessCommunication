using Process.Interface.DataClasses;

namespace Process.Interface;

/// <summary>
/// The process command handler interface
/// </summary>
public interface IProcessServerCommunicationHandler
{
    /// <summary>
    /// The handle command method
    /// </summary>
    /// <param name="processClient">The process client to send a response</param>
    /// <param name="command">The receive command</param>
    /// <param name="token">The cancellation token</param>
    void HandelCommand(NotNull<IProcessTcpClient> processClient, NotEmptyOrWhiteSpace command, CancellationToken token);
}
