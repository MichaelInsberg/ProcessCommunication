namespace ProcessCommunication.ProcessLibrary.Logic;

/// <summary>
/// The process write command class
/// </summary>
public sealed class ProcessWriteCommand
{
    /// <summary>
    /// The write command method
    /// </summary>
    /// <param name="processClient">The progress client</param>
    /// <param name="command">The command</param>
    /// <param name="serializerHelper">The serializer helper</param>
    public void WriteCommad(
        NotNull<IProcessTcpClient> processClient, 
        NotNull<ProcessDataBase> command,
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
