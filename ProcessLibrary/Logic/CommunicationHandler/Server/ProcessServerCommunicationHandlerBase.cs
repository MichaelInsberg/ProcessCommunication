namespace ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Server
{
    public abstract class ProcessServerCommunicationHandlerBase : ProcessCommunicationHandlerBase, IProcessServerCommunicationHandler
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
        
        protected abstract void HandelCommandInternal(
            NotNull<IProcessTcpClient> processTcpClient,
            NotNull<ProcessDataBase> command,
            CancellationToken token);
    }
}

