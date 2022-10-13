namespace ProcessCommunication.ProcessLibrary.Logic
{
    public abstract class ProcessCommunicationBase
    {
        protected const int MAX_RETRIES = 10;

        /// <summary>
        /// The internal logger
        /// </summary>
        protected ILogger Logger { get; }
        
        /// <summary>
        /// The serializer helper
        /// </summary>
        protected SerializerHelper SerializerHelper { get; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        protected string IpAddress { get; }

        /// <summary>
        /// Gets or sets the port
        /// </summary>
        protected int Port { get; }

        /// <summary>
        /// Create a new instance of ProcessCommunicationBase
        /// </summary>
        protected ProcessCommunicationBase(NotNull<ILogger> logger, NotEmptyOrWhiteSpace ipAddress, int port)
        {
            SerializerHelper = new SerializerHelper();
            this.Logger = logger.Value;
            IpAddress = ipAddress.Value;
            Port = port;
        }
    }
}

