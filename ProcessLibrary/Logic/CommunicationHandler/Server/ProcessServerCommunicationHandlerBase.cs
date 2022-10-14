namespace ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Server
{
    /// <summary>
    /// The process server communication handler base class
    /// </summary>
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

        /// <summary>
        /// The internal handle command method
        /// </summary>
        /// <param name="processClient">The process client to send a response</param>
        /// <param name="command">The receive command</param>
        /// <param name="token">The cancellation token</param>
        protected abstract void HandelCommandInternal(
            NotNull<IProcessTcpClient> processClient,
            NotNull<ProcessDataBase> command,
            CancellationToken token);
    }
}

