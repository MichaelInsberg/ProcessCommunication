namespace ProcessCommunication.ProcessLibrary.Logic
{
    public sealed class ProcessWriteCommand
    {
        
        public void WriteCommad(
            NotNull<IProcessTcpClient> processClient, 
            NotNull<CommandBase> command,
            NotNull<ISerializerHelper> serializerHelper)
        {
            var networkStream = processClient.Value.GetStream();
            // todo using (resource)
            var streamWriter = new StreamWriter(networkStream);
            var stringValue = serializerHelper.Value.Serialize(new NotNull<object>(command.Value));
            streamWriter.WriteLine(stringValue);
            streamWriter.Flush();
        }
    }
}

