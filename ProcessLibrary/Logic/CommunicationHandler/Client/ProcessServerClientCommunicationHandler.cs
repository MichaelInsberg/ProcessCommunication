namespace ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Client
{
    /// <summary>
    /// The process server client communication handler class
    /// </summary>
    public sealed class ProcessServerClientCommunicationHandler : ProcessCommunicationHandlerBase, IProgressClientResponseHandler
    {
        /// <inheritdoc />
        protected override IEnumerable<Type> GetRegisteredTypes()
        {
            var enumerable = new List<Type>
            {
                typeof(ResponseStartServer),
            };
            return enumerable;
        }

        /// <inheritdoc />
        public void HandleResponse(string command)
        {
            var receivedCommand = GetCommand(command, CancellationToken.None);

            logger.Log($"Response is {receivedCommand}");
        }

        public ProcessServerClientCommunicationHandler(ILogger logger) : base(logger)
        {
        }
    }
}

