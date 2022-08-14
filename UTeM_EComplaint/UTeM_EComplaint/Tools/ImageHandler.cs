using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UTeM_EComplaint.Tools
{
    internal class ImageHandler
    {
        public static ImageSource LoadBase64(string base64)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                ImageSource image = ImageSource.FromStream(() => new MemoryStream(bytes));
                return image;
            }
            catch 
            {
                throw;
            }
        }
       

        public static async Task<string> ConvertToBase64Async(Stream stream)
        {
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, (int)stream.Length);
            string imageBase64 = Convert.ToBase64String(bytes);

            stream.Seek(0, SeekOrigin.Begin);

            return imageBase64;
        }
    }
}
