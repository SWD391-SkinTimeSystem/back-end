

using System.Text;

namespace SkinTime.Helpers
{
        public class Base32Encoder
        {
            private static readonly char[] Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();
            private static readonly byte[] Base32Lookup = new byte[128];

            static Base32Encoder()
            {
                for (int i = 0; i < Base32Lookup.Length; i++) Base32Lookup[i] = 0xFF;
                for (int i = 0; i < Base32Chars.Length; i++) Base32Lookup[Base32Chars[i]] = (byte)i;
            }

            public static string Encode(string input)
            {
                byte[] data = Encoding.UTF8.GetBytes(input);
                int outputLength = ((data.Length * 8) + 4) / 5;
                char[] result = new char[outputLength];

                int buffer = data[0];
                int next = 1, bitsLeft = 8, index = 0;

                while (index < result.Length)
                {
                    if (bitsLeft < 5)
                    {
                        if (next < data.Length)
                        {
                            buffer <<= 8;
                            buffer |= data[next++] & 0xFF;
                            bitsLeft += 8;
                        }
                        else
                        {
                            int pad = 5 - bitsLeft;
                            buffer <<= pad;
                            bitsLeft += pad;
                        }
                    }

                    int val = (buffer >> (bitsLeft - 5)) & 0x1F;
                    bitsLeft -= 5;
                    result[index++] = Base32Chars[val];
                }

                return new string(result);
            }

            public static string Decode(string base32)
            {
                int buffer = 0, bitsLeft = 0, index = 0;
                byte[] result = new byte[base32.Length * 5 / 8];

                foreach (char c in base32)
                {
                    if (c >= 128 || Base32Lookup[c] == 0xFF) throw new FormatException("Invalid Base32 character.");

                    buffer <<= 5;
                    buffer |= Base32Lookup[c] & 0x1F;
                    bitsLeft += 5;

                    if (bitsLeft >= 8)
                    {
                        result[index++] = (byte)(buffer >> (bitsLeft - 8));
                        bitsLeft -= 8;
                    }
                }

                return Encoding.UTF8.GetString(result, 0, index);
            }
        }
    }

