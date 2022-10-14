namespace ProcessCommunication.ProcessLibrary.Logic.CommunicatationHandler
{
    public abstract class ProcessCommunicationHandlerBase : IProcessServerCommunicationHandler 
    {
        private IEnumerable<Type> registerCommandTypes;
        protected SerializerHelper SerializerHelper { get; }

        public ProcessCommunicationHandlerBase()
        {
            SerializerHelper = new SerializerHelper();
        }

        /// <inheritdoc />
        public void HandelCommand(NotNull<IProcessTcpClient> processClient, NotEmptyOrWhiteSpace command, CancellationToken token)
        {
            registerCommandTypes = GetRegistedTypes().Value;
            var receivedCommand = GetCommand(command.Value, token);
            if (receivedCommand is null)
            {
                //Send unknow command
            }
            HandelCommandInternal(processClient, new NotNull<CommandBase>(receivedCommand), token);
        }

        protected abstract NotNull<IEnumerable<Type>> GetRegistedTypes();

        protected abstract void HandelCommandInternal(
            NotNull<IProcessTcpClient> processTcpClient,
            NotNull<CommandBase> command,
            CancellationToken token);

        private CommandBase? GetCommand(string commandValue, CancellationToken cancellationToken)
        {
            CommandBase? result = null; 
            foreach (var registerCommand in registerCommandTypes)
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }
                    result = (CommandBase)SerializerHelper.DeSerialize(new NotEmptyOrWhiteSpace(commandValue), registerCommand);
                }
                catch
                {
                    //Do nothing
                }
            }

            return result;
        }
    }
}

