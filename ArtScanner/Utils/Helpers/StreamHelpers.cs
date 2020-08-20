using System;
using System.IO;

namespace ArtScanner.Utils.Helpers
{
    public static class StreamHelpers
    {
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static byte[] GetByteArrayFromFilePath(string filepath)
        {
            var bytes = default(byte[]);

            using (var streamReader = new StreamReader(filepath))
            {

                using (var memstream = new MemoryStream())
                {
                    streamReader.BaseStream.CopyTo(memstream);
                    bytes = memstream.ToArray();
                }
            }

            return bytes;
        }
    }
}
