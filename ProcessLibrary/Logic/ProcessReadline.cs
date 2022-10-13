namespace ProcessCommunication.ProcessLibrary.Logic
{
    public sealed class ProcessReadline
    {
        private readonly ILogger logger;

        public ProcessReadline(NotNull<ILogger> logger)
        {
            this.logger = logger.Value;
        }

        public string Readline(
            IProcessTcpClient client,
            NotEmptyOrWhiteSpace address ,
            CancellationToken token)
        {
            logger.Log(new NotEmptyOrWhiteSpace($"Waiting for command {address}"));
            var networkStream = client.GetStream();
            var streamReader = new StreamReader(networkStream);
            var result = streamReader.ReadLine();
            if (string.IsNullOrWhiteSpace(result))
            {
                throw new InvalidOperationException($"Can not read line form {nameof(client)}");
            }
            return result;

        }
    }
}

