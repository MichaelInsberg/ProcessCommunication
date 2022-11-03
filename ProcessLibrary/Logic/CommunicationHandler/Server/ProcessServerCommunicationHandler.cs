namespace ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Server;

/// <summary>
/// The process server communication handler class
/// </summary>
public sealed class ProcessServerCommunicationHandler : ProcessCommunicationHandlerBase, IProcessServerCommunicationHandler
{
    /// <inheritdoc />
    public void HandelCommand(IProcessTcpClient processClient, string command, CancellationToken token)
    {
        var receivedCommand = GetCommand(command, token);
        if (receivedCommand is null)
        {
            logger.Log($"Received command {receivedCommand}");
            //Send unknow command
        }
        logger.Log($"Received command {receivedCommand}");

        HandelCommandInternal(processClient, receivedCommand, token);
    }

    /// <inheritdoc />
    protected override IEnumerable<Type> GetRegisteredTypes()
    {
        var enumerable = new List<Type>
        {
            typeof(CommandStartServer),
        };
        return enumerable;
    }

    protected void HandelCommandInternal(IProcessTcpClient processTcpClient, ProcessDataBase command, CancellationToken token)
    {
        var processWriteCommand = new ProcessWriteCommand();
        if (command is CommandStartServer)
        {
            //ToDo create response
            processWriteCommand.WriteCommad(processTcpClient, command, SerializerHelper);
        }
    }

    public ProcessServerCommunicationHandler(ILogger logger) : base(logger)
    {
    }
}
