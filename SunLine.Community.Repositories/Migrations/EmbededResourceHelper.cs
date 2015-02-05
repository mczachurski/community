using System;
using System.IO;
using System.Reflection;

namespace SunLine.Community.Repositories.Migrations
{
    public static class EmbededResourceHelper
    {
        public static string ReadFromEmbededResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string result;
            Stream stream = null;
            try
            {
                stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    throw new ArgumentException(string.Format("Resource {0} not exists.", resourceName));
                }

                using (var reader = new StreamReader(stream))
                {
                    stream = null;
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }


            return result;
        }
    }
}

