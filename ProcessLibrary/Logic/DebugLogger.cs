namespace ProcessCommunication.ProcessLibrary.Logic
{
    /// <summary>
    /// The debug logger class
    /// </summary>
    public sealed class DebugLogger : ILogger
    {
        /// <inheritdoc />
        public void Log(NotEmptyOrWhiteSpace logMessage)
        {
            LogException(logMessage, null);
        }

        /// <inheritdoc />
        public void LogException(NotEmptyOrWhiteSpace logMessage, Exception? exception)
        {
            Console.WriteLine(logMessage.Value);
            if (exception is null)
            {
                return;
            }

            var message = exception.Message;
            Console.WriteLine(message);
            var innerException = exception.InnerException;
            while (innerException is not null)
            {
                message = innerException.Message;
                Console.WriteLine(message);
                innerException = innerException.InnerException;
            }

        }
    }
}
