namespace ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Client
{
    public class ImageProcessingClientCommunicationHandler: ProcessCommunicationHandlerBase, IProgressClientResponseHandler
    {
        /// <inheritdoc />
        protected override NotNull<IEnumerable<Type>> GetRegisteredTypes()
        {
            var enumerable = new List<Type>
            {
                typeof(ResponseImageProcessingConvertImage),
            };
            return new NotNull<IEnumerable<Type>>(enumerable);
        }

        /// <inheritdoc />
        public void HandleResponse(NotEmptyOrWhiteSpace command)
        {
            var receivedCommand = GetCommand(command.Value, CancellationToken.None);

            logger.Log(new NotEmptyOrWhiteSpace($"Response is {receivedCommand}"));
        }

        public ImageProcessingClientCommunicationHandler(ILogger logger) : base(logger)
        {
        }
    }
}
