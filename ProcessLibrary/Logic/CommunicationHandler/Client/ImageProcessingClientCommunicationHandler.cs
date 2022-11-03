namespace ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Client
{
    public class ImageProcessingClientCommunicationHandler: ProcessCommunicationHandlerBase, IProgressClientResponseHandler
    {
        /// <inheritdoc />
        protected override IEnumerable<Type> GetRegisteredTypes()
        {
            var enumerable = new List<Type>
            {
                typeof(ResponseImageProcessingConvertImage),
            };
            return enumerable;
        }

        /// <inheritdoc />
        public void HandleResponse(string command)
        {
            var receivedCommand = GetCommand(command, CancellationToken.None);

            logger.Log($"Response is {receivedCommand}");
        }

        public ImageProcessingClientCommunicationHandler(ILogger logger) : base(logger)
        {
        }
    }
}
