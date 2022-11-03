﻿namespace ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Server;

/// <summary>
/// The process server communication handler class
/// </summary>
public sealed class ProcessServerCommunicationHandler : ProcessCommunicationHandlerBase, IProcessServerCommunicationHandler
{
    /// <inheritdoc />
    public void HandelCommand(NotNull<IProcessTcpClient> processClient, NotEmptyOrWhiteSpace command, CancellationToken token)
    {
        var receivedCommand = GetCommand(command.Value, token);
        if (receivedCommand is null)
        {
            //Send unknow command
        }
        HandelCommandInternal(processClient, new NotNull<ProcessDataBase>(receivedCommand), token);
    }

    /// <inheritdoc />
    protected override NotNull<IEnumerable<Type>> GetRegisteredTypes()
    {
        var enumerable = new List<Type>
        {
            typeof(CommandStartServer),
        };
        return new NotNull<IEnumerable<Type>>(enumerable);
    }

    protected void HandelCommandInternal(NotNull<IProcessTcpClient> processTcpClient, NotNull<ProcessDataBase> command, CancellationToken token)
    {
        var commandType = command.Value.GetType();
        var processWriteCommand = new ProcessWriteCommand();
        if (commandType == typeof(CommandStartServer))
        {
            processWriteCommand.WriteCommad(processTcpClient, command, new NotNull<ISerializerHelper>(SerializerHelper));
        }
    }

    public ProcessServerCommunicationHandler(ILogger logger) : base(logger)
    {
    }
}
