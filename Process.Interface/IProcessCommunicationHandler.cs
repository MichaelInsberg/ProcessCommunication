using Process.Interface.DataClasses;

namespace Process.Interface;

// public ProcessCommandHandlerCreator = Func<IProcessCommandHandler>

/// <summary>
/// The process command handler interface
/// </summary>
public interface IProcessCommunicationHandler
{
    void HandelCommand(NotNull<IProcessTcpClient> processClient, NotEmptyOrWhiteSpace command, CancellationToken token);
}
