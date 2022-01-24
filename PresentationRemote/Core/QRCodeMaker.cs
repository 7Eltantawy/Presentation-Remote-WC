using QRCoder;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace PresentationRemote.Core
{
    public static class QRCodeMaker
    {
        public static BitmapImage GenerateQRImage(this string text)
        {
            QRCodeGenerator qrGenerator = new ();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return Convert(qrCodeImage);
        }
        public static BitmapImage Convert(this Bitmap src)
        {
            MemoryStream ms = new ();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new ();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}
