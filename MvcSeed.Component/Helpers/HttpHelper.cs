using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MvcSeed.Component.Helpers
{
    public static class HttpHelper
    {
        #region 同步方法

        public static T Get<T>(string url, string userAgent = null)
        {
            return Task.Run(() => GetAsync<T>(url, userAgent)).Result;
        }

        public static T Post<T>(string url, string jsonString, string contentType = "Application/Json", Dictionary<string, string> headers = null)
        {
            return Task.Run(() => PostAsync<T>(url, jsonString, contentType, headers)).Result;
        }

        public static TResult Post<TResult, TSource>(string url, TSource TEntity, string contentType = "Application/Json", Dictionary<string, string> headers = null)
        {
            return Task.Run(() => PostAsync<TResult, TSource>(url, TEntity, contentType, headers)).Result;
        }

        #endregion

        #region 异步方法

        public static async Task<T> GetAsync<T>(string url, string userAgent = null)
        {
            var httpClient = new HttpClient();

            if (userAgent != null)
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            }

            var jsonString = await httpClient.GetStringAsync(url);
            var result = JsonHelper.JsonDeserializer<T>(jsonString);
            return result;
        }

        public static async Task<T> PostAsync<T>(string url, string jsonString, string contentType = "Application/Json", Dictionary<string, string> headers = null)
        {
            var httpClient = new HttpClient();

            var content = new StringContent(jsonString, Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            var response = await httpClient.PostAsync(url, content);
            var result = JsonHelper.JsonDeserializer<T>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public static async Task<TResult> PostAsync<TResult, TSource>(string url, TSource TEntity, string contentType = "Application/Json", Dictionary<string, string> headers = null)
        {
            var jsonString = JsonHelper.JsonSerializer(TEntity);
            return await PostAsync<TResult>(url, jsonString, contentType, headers);
        }

        #endregion

        #region 扩展方法

        /// <summary>
        /// 封装System.Web.HttpUtility.HtmlEncode
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string html)
        {
            return System.Web.HttpUtility.HtmlEncode(html);
        }

        /// <summary>
        /// 封装System.Web.HttpUtility.HtmlDecode
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string html)
        {
            return System.Web.HttpUtility.HtmlDecode(html);
        }

        /// <summary>
        /// 封装System.Web.HttpUtility.UrlEncode
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(this string url)
        {
            return System.Web.HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// 封装System.Web.HttpUtility.UrlDecode
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDecode(this string url)
        {
            return System.Web.HttpUtility.UrlDecode(url);
        }

        #endregion
    }
}
