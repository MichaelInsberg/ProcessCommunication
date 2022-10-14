namespace ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Client
{
    /// <summary>
    /// The process server client communication handler class
    /// </summary>
    public sealed class ProcessServerClientCommunicationHandler : ProcessCommunicationHandlerBase, IProgressClientResponseHandler
    {
        /// <inheritdoc />
        protected override NotNull<IEnumerable<Type>> GetRegisteredTypes()
        {
            var enumerable = new List<Type>
            {
                typeof(ResponseStartServer),
            };
            return new NotNull<IEnumerable<Type>>(enumerable);
        }

        /// <inheritdoc />
        public void HandleResponse(NotEmptyOrWhiteSpace command)
        {
            var receivedCommand = GetCommand(command.Value, CancellationToken.None);

            Console.WriteLine($"Response is {receivedCommand}");
        }
    }
}

