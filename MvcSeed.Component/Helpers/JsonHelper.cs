using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcSeed.Component.Helpers
{
    public static class JsonHelper
    {
        public static T JsonDeserializer<T>(string jsonString)
        {
            try
            {
                //将"yyyy-MM-ddTHH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式
                string p = @"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}";
                var matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
                var reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);

                var jsonSerializer = new DataContractJsonSerializer(typeof(T));
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    return (T)jsonSerializer.ReadObject(ms);
                }
            }
            catch
            {
                return default(T);
            }
        }

        public static T JsonDeserializer<T>(Stream jsonStream)
        {
            try
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(T));
                return (T)jsonSerializer.ReadObject(jsonStream);
            }
            catch
            {
                return default(T);
            }
        }

        public static string JsonSerializer<T>(T t)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                jsonSerializer.WriteObject(ms, t);
                string jsonString = Encoding.UTF8.GetString(ms.ToArray());
                return jsonString;
            }
        }

        private static string ConvertJsonDateToDateString(Match m)
        {
            var dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            string result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        private static string ConvertDateStringToJsonDate(Match m)
        {
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            string result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }
    }
}
