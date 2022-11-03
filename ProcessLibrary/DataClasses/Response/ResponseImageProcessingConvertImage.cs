using System.Drawing;
using System.Drawing.Imaging;

namespace ProcessCommunication.ProcessLibrary.DataClasses.Response;

public class ResponseImageProcessingConvertImage : ResponseBase
{
    public List<byte> BitmapData { get; set; }

    public ResponseImageProcessingConvertImage()
    {
        BitmapData = new List<byte>();
    }
    public void SetBitmap(Bitmap bitmap)
    {
        using var output = new MemoryStream();
        bitmap.Save(output, ImageFormat.Jpeg);

        BitmapData.Clear();
        BitmapData.AddRange(output.ToArray());
    }
}
