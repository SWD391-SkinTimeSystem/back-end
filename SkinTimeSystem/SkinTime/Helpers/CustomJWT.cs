using SimpleBase;
using System.IO.Compression;
using System.Text;

namespace SkinTime.Helpers
{
    public class CustomJWT
    {
        public static string CompressAndEncode(string jwtToken)
        {
            byte[] compressedData = Compress(Encoding.UTF8.GetBytes(jwtToken));
            return Base58.Bitcoin.Encode(compressedData);
        }

        private static byte[] Compress(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                using (var gzip = new GZipStream(ms, CompressionMode.Compress))
                {
                    gzip.Write(data, 0, data.Length);
                }
                return ms.ToArray();
            }
        }
        public static string DecodeAndDecompress(string encodedToken)
        {
            byte[] decodedBytes = Base58.Bitcoin.Decode(encodedToken);
            return Encoding.UTF8.GetString(Decompress(decodedBytes));
        }

        private static byte[] Decompress(byte[] compressedData)
        {
            using (var ms = new MemoryStream(compressedData))
            using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
            using (var output = new MemoryStream())
            {
                gzip.CopyTo(output);
                return output.ToArray();
            }
        }

    }
}
