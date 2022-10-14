namespace ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Server;

/// <summary>
/// The process server communication handler class
/// </summary>
public sealed class ProcessServerCommunicationHandler : ProcessServerCommunicationHandlerBase
{
    /// <inheritdoc />
    protected override NotNull<IEnumerable<Type>> GetRegisteredTypes()
    {
        var enumerable = new List<Type>
        {
            typeof(CommandStartServer),
        };
        return new NotNull<IEnumerable<Type>>(enumerable);
    }

    /// <inheritdoc />
    protected override void HandelCommandInternal(NotNull<IProcessTcpClient> processTcpClient, NotNull<ProcessDataBase> command, CancellationToken token)
    {
        var commandType = command.Value.GetType();
        var processWriteCommand = new ProcessWriteCommand();
        if (commandType == typeof(CommandStartServer))
        {
            processWriteCommand.WriteCommad(processTcpClient, command, new NotNull<ISerializerHelper>(SerializerHelper));
        }
    }
}
