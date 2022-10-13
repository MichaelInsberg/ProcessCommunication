using System.Diagnostics;

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
            WriteMessage(logMessage.Value);
            if (exception is null)
            {
                return;
            }

            var message = exception.Message;
            WriteMessage($"Exception message: {message}");
            var innerException = exception.InnerException;
            while (innerException is not null)
            {
                message = innerException.Message;
                WriteMessage($"Inner exception message: {message}");
                innerException = innerException.InnerException;
            }

        }

        private static void WriteMessage(string message)
        {
            var value = 
                $"{Environment.NewLine}"+
                "----------------------------------------------------------------------------------------------------------------------"+
                $"{Environment.NewLine}"+
                $"Thread Id:<{Thread.CurrentThread.ManagedThreadId}> Datetime:<{DateTime.Now.ToString(CultureInfo.InvariantCulture)}>" +
                $"{Environment.NewLine}" +
                $"{message}" +
                $"{Environment.NewLine}"+
                "----------------------------------------------------------------------------------------------------------------------";
            Trace.WriteLine(value);
            Console.WriteLine(value);
        }
    }
}
