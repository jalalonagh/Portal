using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;
using System.Text;

namespace JO.Shared.Classes.Captcha
{
    public static class Captcha
    {
        const string FONTFAMILY = "Arial";

        public static string CreateCode(int digitCount = 5)
        {
            char[] chars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder sb = new();
            Random random = new();
            for (int i = 0; i < digitCount; i++)
            {
                int count = random.Next(chars.Length);
                sb.Append(chars[count]);
            }

            return sb.ToString();
        }

        public static Color BackgroundColor { get; set; } = Color.Wheat;

        [SupportedOSPlatform("windows")]
        public static MemoryStream BuildImage(string captchaCode, int imageHeight, int imageWidth, int fontSize, int distortion = 18, ImageFormat imgFormat = null!)
        {
            int newX, newY;
            Random random = new();
            MemoryStream memoryStream = new();
            using (Bitmap captchaImage = new(imageWidth, imageHeight, PixelFormat.Format64bppArgb))
            using (Bitmap cache = new(imageWidth, imageHeight, PixelFormat.Format64bppArgb))
            using (Graphics g = Graphics.FromImage(captchaImage))
            using (Font txtFont = new(FONTFAMILY, fontSize, FontStyle.Italic))
            {
                g.Clear(BackgroundColor);
                g.DrawString(captchaCode, txtFont, Brushes.Gray, new PointF(0, 0));

                for (int i = 0; i < 8; i++)
                {
                    int startX = random.Next(imageWidth);
                    int startY = random.Next(imageHeight);
                    int endX = random.Next(imageWidth);
                    int endY = random.Next(imageHeight);

                    g.DrawLine(Pens.Gray, startX, startY, endX, endY);
                }

                for (int y = 0; y < imageHeight; y++)
                {
                    for (int x = 0; x < imageWidth; x++)
                    {
                        newX = (int)(x + (distortion * Math.Sin(Math.PI * y / 64.0)));
                        newY = (int)(y + (distortion * Math.Cos(Math.PI * x / 64.0)));
                        if (newX < 0 || newX >= imageWidth)
                        {
                            newX = 0;
                        }
                        if (newY < 0 || newY >= imageHeight)
                        {
                            newY = 0;
                        }
                        cache.SetPixel(x, y, captchaImage.GetPixel(newX, newY));
                    }
                }

                if (imgFormat == null)
                {
                    imgFormat = ImageFormat.Jpeg;
                }

                cache.Save(memoryStream, imgFormat);
                memoryStream.Position = 0;

                return memoryStream;
            }
        }
    }
}
