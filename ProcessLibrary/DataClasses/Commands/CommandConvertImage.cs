using System.Drawing;
using System.Drawing.Imaging;

namespace ProcessCommunication.ProcessLibrary.DataClasses.Commands
{
    public class CommandConvertImage : CommandBase
    {
        public List<byte> BitmapData { get; set; }

        public CommandConvertImage()
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
}
