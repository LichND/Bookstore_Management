using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BookStore_Management
{
    public class ImageEncoder
    {
        public static string ImageFilter { get; } = "Image File (*.jpg;*.bmp;*.png)|*.jpg;*.bmp;*.png";
        public static string Image2StringHex(BitmapImage image)
        {
            try
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    return BitConverter.ToString(ms.ToArray()).Replace("-", "");
                }
            }
            catch
            {
                return "";
            }
        }
        public static BitmapImage StringHex2Image(string image)
        {
            if (image is null || image == "")
                return null;

            byte[] data = Enumerable.Range(0, image.Length / 2).Select(x => Convert.ToByte(image.Substring(x * 2, 2), 16)).ToArray();

            using (var ms = new MemoryStream(data))
            {
                var ret = new BitmapImage();
                ret.BeginInit();
                ret.CacheOption = BitmapCacheOption.OnLoad; // here
                ret.StreamSource = ms;
                ret.EndInit();
                return ret;
            }
        }
    }
}
