using System;
using System.IO;

namespace ArtScanner.Utils.Helpers
{
    public static class StreamHelpers
    {
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
