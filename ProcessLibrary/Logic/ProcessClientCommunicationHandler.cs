﻿using ProcessCommunication.ProcessLibrary.DataClasses.Response;

namespace ProcessCommunication.ProcessLibrary.Logic
{
    public sealed class ProcessClientCommunicationHandler : ProcessCommunicationHandler
    {
        protected override NotNull<IEnumerable<Type>> GetRegistedTypes()
        {
            var enumerable = new List<Type>
            {
                typeof(ResponseStartServer),
            };
            return new NotNull<IEnumerable<Type>>(enumerable);
        }

        protected override void HandelCommandInternal(NotNull<IProcessTcpClient> processClient, NotNull<CommandBase> command, CancellationToken token)
        {
            var commandType = command.Value.GetType();
            if (commandType == typeof(ResponseStartServer))
            {
                var networkStream = processClient.Value.GetStream();
                var streamWriter = new StreamWriter(networkStream);
                var responseStartServer = new ResponseStartServer { IpAddress = "Test", IsStarted = true, SerialNumber = "Wurst" };
                var stringValue = SerializerHelper.Serialize(new NotNull<object>(responseStartServer));
                streamWriter.WriteLine(stringValue);
                streamWriter.Flush();
            }
        }
    }
}
