using System.Buffers.Text;
using System.IO.Compression;
using System.Text;
using SimpleBase; 


namespace SkinTime.Helpers
{
    public static class CustomEncoder
    {
        public static string Encode(string input)
        {
            byte[] compressedData = Compress(Encoding.UTF8.GetBytes(input));
            string encoded = Base58.Bitcoin.Encode(compressedData);

            // Đảm bảo độ dài nằm trong khoảng [10, 255]
            int length = Math.Max(10, Math.Min(encoded.Length, 255));

            return encoded.Substring(0, length);
        }

        // Giải mã: Base58 -> Giải nén GZip
        public static string Decode(string encodedInput)
        {
            byte[] decodedBytes = Base58.Bitcoin.Decode(encodedInput);
            byte[] decompressedData = Decompress(decodedBytes);

            return Encoding.UTF8.GetString(decompressedData);
        }

        // Hàm nén bằng GZip
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

        // Hàm giải nén bằng GZip
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
