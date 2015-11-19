using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcSeed.Component.Github.Entities;
using MvcSeed.Component.Helpers;
using MvcSeed.Component.Util;

namespace MvcSeed.Component.Github.Helpers
{
    public class GithubHelper
    {
        public static async Task<string> GetAccessTokenAsync(AccessTokenRequest accessTokenRequest)
        {
            //TODO 异常处理&缓存机制
            const string urlFormat = "https://github.com/login/oauth/access_token";
            var result = await HttpHelper.PostAsync<AccessTokenResult, AccessTokenRequest>(
                urlFormat,
                accessTokenRequest,
                headers: new Dictionary<string, string> { { "Accept", "application/json" } });
            return result.access_token;
        }

        public static async Task<GithubUserInfoResult> GetGithubUserAsync(string accessToken)
        {
            const string urlFormat = "https://api.github.com/user?access_token={0}";

            //userAgent不能为空，github api会抛异常“The server committed a protocol violation. Section=ResponseStatusLine”
            var result = await HttpHelper.GetAsync<GithubUserInfoResult>(
                string.Format(urlFormat, accessToken),
                userAgent: "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36");
            return result;
        }

        public static string GetAccessToken(AccessTokenRequest accessTokenRequest)
        {
            //TODO 异常处理&缓存机制
            const string urlFormat = "https://github.com/login/oauth/access_token";
            var result = HttpHelper.Post<AccessTokenResult, AccessTokenRequest>(
                urlFormat,
                accessTokenRequest,
                headers: new Dictionary<string, string> { { "Accept", "application/json" } });
            return result.access_token;
        }

        public static GithubUserInfoResult GetGithubUser(string accessToken)
        {
            const string urlFormat = "https://api.github.com/user?access_token={0}";

            //userAgent不能为空，github api会抛异常“The server committed a protocol violation. Section=ResponseStatusLine”
            var result = HttpHelper.Get<GithubUserInfoResult>(
                string.Format(urlFormat, accessToken),
                userAgent: "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36");
            return result;
        }
    }
}
