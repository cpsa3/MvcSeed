using MvcSeed.Component.Util;
using System;

namespace MvcSeed.Component.WeiXin.Helpers
{
    using MvcSeed.Component.Helpers;

    using Senparc.Weixin.MP;
    using Senparc.Weixin.MP.Entities;

    public class WeiXinUtility
    {
        /// <summary>
        /// 获取accessToken，首先尝试从cache中获取，若没有则调用接口获取最新的accessToken并存入cache中。
        /// 待优化：添加accessToken被动刷新机制
        /// </summary>
        /// <param name="appId">appId</param>
        /// <param name="appSecret">appSecret</param>
        /// <param name="myCache">myCache</param>
        /// <param name="isRefreshForse">是否获取新的accessToken</param>
        /// <returns></returns>
        public static string GetAccessToken(string appId, string appSecret, ICache myCache, bool isRefreshForse = false)
        {
            //设置accessToken cache过期时间为10min(微信官方accessToken有效期为2h)
            var expiry = DateTime.Now.AddMinutes(10);
            var accessToken = myCache.Get("WeiXinAT_" + appId, () => WeiXinHelper.GetAccessToken(appId, appSecret), expiry, isRefreshForse, true);
            
            LogHelper.Error("accessToken:" + accessToken);
            return accessToken;
        }

        /// <summary>
        /// 使用AccessToken进行操作时，如果遇到AccessToken错误的情况，重新获取AccessToken一次，并重试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun">第一个参数为accessToken</param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="cache"></param>
        /// <param name="retryIfFaild"></param>
        /// <returns></returns>
        public static T Do<T>(Func<string, T> fun, string appId, string appSecret, ICache cache, bool retryIfFaild = true) where T : WxJsonResult
        {
            T result = null;

            try
            {
                var accessToken = WeiXinUtility.GetAccessToken(appId, appSecret, cache);
                result = fun(accessToken);
            }
            catch (ErrorJsonResultException ex)
            {
                if (retryIfFaild && ex.JsonResult.errcode == ReturnCode.验证失败)
                {
                    var accessTokenNew = WeiXinUtility.GetAccessToken(appId, appSecret, cache, true);
                    result = fun(accessTokenNew);
                }
                else
                {
                    throw;
                }
            }

            return result;
        }
    }
}
