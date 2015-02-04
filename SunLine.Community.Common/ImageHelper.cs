using System;
using System.Drawing;
using System.IO;

namespace SunLine.Community.Common
{
    public static class ImageHelper
    {
        public static Stream GetThumbnail(Stream imageStream, int width)
        {
            imageStream.Seek(0, SeekOrigin.Begin);
            Image image = Image.FromStream(imageStream);

            if(image.Width <= width)
            {
                imageStream.Seek(0, SeekOrigin.Begin);
                return imageStream;
            }

            float ratio = image.Width / (float) width;
            int newHeight = (int)(image.Height / ratio);

            Image thumbnail = image.GetThumbnailImage(width, newHeight, ()=>false, IntPtr.Zero);

            MemoryStream ms = new MemoryStream();
            thumbnail.Save(ms, image.RawFormat);

            return ms;
        }
    }
}

