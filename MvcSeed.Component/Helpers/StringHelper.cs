using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace MvcSeed.Component.Helpers
{
    public class StringHelper
    {

        public static string BytesToHexString(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "").ToLower();
        }

        public static byte[] HexStringToBytes(string hexString)
        {
            return SoapHexBinary.Parse(hexString).Value;
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
