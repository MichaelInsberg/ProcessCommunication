using System.Drawing;

namespace ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Server;

public class ImageProcessingServerCommunicationHandler : ProcessCommunicationHandlerBase, IProcessServerCommunicationHandler
{
    /// <inheritdoc />
    public void HandelCommand(IProcessTcpClient processClient, string command, CancellationToken token)
    {
        var receivedCommand = GetCommand(command, token);
        if (receivedCommand is null)
        {
            logger.Log($"Received command {receivedCommand}");
            //Send unknow command
        }
        logger.Log($"Received command {receivedCommand}");
        HandelCommandInternal(processClient, receivedCommand, token);
    }

    /// <inheritdoc />
    protected override IEnumerable<Type> GetRegisteredTypes()
    {
        var enumerable = new List<Type>
        {
            typeof(CommandConvertImage),
        };
        return enumerable;
    }

    protected void HandelCommandInternal(IProcessTcpClient processTcpClient, ProcessDataBase command, CancellationToken token)
    {
        var processWriteCommand = new ProcessWriteCommand();
        
        if (command is CommandConvertImage commandConvertImage)
        {
            using var memoryStream = new MemoryStream(commandConvertImage.BitmapData.ToArray());
            using var bitmap = new Bitmap(memoryStream);
            var response = new ResponseImageProcessingConvertImage();
            response.SetBitmap(bitmap);
            processWriteCommand.WriteCommad(processTcpClient, response, SerializerHelper);
            bitmap.Save("ReceiveImage.jpg");
        }
    }

    public ImageProcessingServerCommunicationHandler(ILogger logger) : base(logger)
    {
    }
}
