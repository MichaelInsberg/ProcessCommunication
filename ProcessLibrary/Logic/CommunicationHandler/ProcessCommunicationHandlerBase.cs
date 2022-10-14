namespace ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler
{
    /// <summary>
    /// The process communication handler base class
    /// </summary>
    public abstract class ProcessCommunicationHandlerBase 
    {
        /// <summary>
        /// Gets or sets the serializer helper
        /// </summary>
        protected SerializerHelper SerializerHelper { get; }

        /// <summary>
        /// Create a new instance of ProcessCommunicationHandlerBase
        /// </summary>
        protected ProcessCommunicationHandlerBase()
        {
            SerializerHelper = new SerializerHelper();
        }
        /// <summary>
        /// The get registered types method
        /// </summary>
        /// <returns>The registered types enumerable</returns>
        protected abstract NotNull<IEnumerable<Type>> GetRegisteredTypes();

        /// <summary>
        /// The get command method
        /// </summary>
        /// <param name="commandValue">The received command</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The </returns>
        protected ProcessDataBase? GetCommand(string commandValue, CancellationToken cancellationToken)
        {
            var registerCommandTypes = GetRegisteredTypes().Value;
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

