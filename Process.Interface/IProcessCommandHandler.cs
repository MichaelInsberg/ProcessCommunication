using Process.Interface.DataClasses;

namespace Process.Interface;

// public ProcessCommandHandlerCreator = Func<IProcessCommandHandler>

/// <summary>
/// The process command handler interface
/// </summary>
public interface IProcessCommandHandler
{
    void HandelCommand(NotNull<IProcessTcpClient> processClient, NotEmptyOrWhiteSpace command, CancellationToken token);
}
