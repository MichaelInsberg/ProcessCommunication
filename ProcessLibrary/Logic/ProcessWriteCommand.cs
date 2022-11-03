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
        IProcessTcpClient processClient, 
        ProcessDataBase command,
        ISerializerHelper serializerHelper)
    {
        var networkStream = processClient.GetStream();
        // todo using (resource)
        var streamWriter = new StreamWriter(networkStream);
        var stringValue = serializerHelper.Serialize(command);
        streamWriter.WriteLine(stringValue);
        streamWriter.Flush();
    }
}
