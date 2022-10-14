namespace ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler
{
    public abstract class ProcessCommunicationHandlerBase 
    {
        private IEnumerable<Type> registerCommandTypes;
        protected SerializerHelper SerializerHelper { get; }

        public ProcessCommunicationHandlerBase()
        {
            SerializerHelper = new SerializerHelper();
        }
        protected abstract NotNull<IEnumerable<Type>> GetRegistedTypes();

        protected ProcessDataBase? GetCommand(string commandValue, CancellationToken cancellationToken)
        {
            registerCommandTypes = GetRegistedTypes().Value;
            ProcessDataBase? result = null; 
            foreach (var registerCommand in registerCommandTypes)
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }
                    result = (ProcessDataBase)SerializerHelper.DeSerialize(new NotEmptyOrWhiteSpace(commandValue), registerCommand);
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

