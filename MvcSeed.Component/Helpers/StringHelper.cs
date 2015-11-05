using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSeed.Component.Helpers
{
    public class StringHelper
    {
        public static byte[] HexStringToBytes(string hex)
        {
            if (hex.Length == 0)
            {
                return new byte[] { 0 };
            }

            if (hex.Length % 2 == 1)
            {
                hex = "0" + hex;
            }

            var result = new byte[hex.Length / 2];

            for (var i = 0; i < hex.Length / 2; i++)
            {
                result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }

            return result;
        }

        public static string BytesToHexString(byte[] input)
        {
            var hexString = new StringBuilder(64);

            foreach (byte t in input)
            {
                hexString.Append(String.Format("{0:X2}", t));
            }
            return hexString.ToString();
        }

        public static string BytesToDecString(byte[] input)
        {
            var decString = new StringBuilder(64);

            for (var i = 0; i < input.Length; i++)
            {
                decString.Append(String.Format(i == 0 ? "{0:D3}" : "-{0:D3}", input[i]));
            }
            return decString.ToString();
        }

        // Bytes are string
        public static string ASCIIBytesToString(byte[] input)
        {
            var enc = new ASCIIEncoding();
            return enc.GetString(input);
        }

        public static string UTF16BytesToString(byte[] input)
        {
            var enc = new UnicodeEncoding();
            return enc.GetString(input);
        }

        public static string UTF8BytesToString(byte[] input)
        {
            var enc = new UTF8Encoding();
            return enc.GetString(input);
        }

        public static string GB2312BytesToString(byte[] input)
        {
            Encoding GB2312 = Encoding.GetEncoding("GB2312");
            return GB2312.GetString(input);
        }

        public static string ToBase64(byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        public static byte[] FromBase64(string base64)
        {
            return Convert.FromBase64String(base64);
        }
    }
}
