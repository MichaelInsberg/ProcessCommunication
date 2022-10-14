using ProcessCommunication.ProcessLibrary.DataClasses.Response;

namespace ProcessCommunication.ProcessLibrary.Logic.CommunicatationHandler
{
    public sealed class ProcessServerClientCommunicationHandler : ProcessCommunicationHandlerBase
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
                var processWriteCommand = new ProcessWriteCommand();
                processWriteCommand.WriteCommad(processClient, command, new NotNull<ISerializerHelper>(SerializerHelper));
            }
        }
    }
}

