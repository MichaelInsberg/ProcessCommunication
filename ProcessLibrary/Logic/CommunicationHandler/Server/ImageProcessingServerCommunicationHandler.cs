using System.Drawing;

namespace ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Server;

public class ImageProcessingServerCommunicationHandler : ProcessCommunicationHandlerBase, IProcessServerCommunicationHandler
{
    /// <inheritdoc />
    public void HandelCommand(NotNull<IProcessTcpClient> processClient, NotEmptyOrWhiteSpace command, CancellationToken token)
    {
        var receivedCommand = GetCommand(command.Value, token);
        if (receivedCommand is null)
        {
            //Send unknow command
        }
        HandelCommandInternal(processClient, new NotNull<ProcessDataBase>(receivedCommand), token);
    }

    /// <inheritdoc />
    protected override NotNull<IEnumerable<Type>> GetRegisteredTypes()
    {
        var enumerable = new List<Type>
        {
            typeof(CommandConvertImage),
        };
        return new NotNull<IEnumerable<Type>>(enumerable);
    }

    protected void HandelCommandInternal(NotNull<IProcessTcpClient> processTcpClient, NotNull<ProcessDataBase> command, CancellationToken token)
    {
        var processWriteCommand = new ProcessWriteCommand();
        
        if (command.Value is CommandConvertImage commandConvertImage)
        {
            using var memoryStream = new MemoryStream(commandConvertImage.BitmapData.ToArray());
            using var bitmap = new Bitmap(memoryStream);
            var response = new ResponseImageProcessingConvertImage();
            response.SetBitmap(bitmap);
            processWriteCommand.WriteCommad(processTcpClient, new NotNull<ProcessDataBase>(response), new NotNull<ISerializerHelper>(SerializerHelper));
            bitmap.Save("ReceiveImage.jpg");
        }
    }

    public ImageProcessingServerCommunicationHandler(ILogger logger) : base(logger)
    {
    }
}
