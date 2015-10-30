using MvcSeed.Component.Util;
using System;

namespace MvcSeed.Component.WeiXin.Helpers
{
    public class WeiXinUtility
    {
        /// <summary>
        /// 获取accessToken，首先尝试从cache中获取，若没有则调用接口获取最新的accessToken并存入cache中。
        /// 待优化：添加accessToken被动刷新机制
        /// </summary>
        /// <param name="appId">appId</param>
        /// <param name="appSecret">appSecret</param>
        /// <param name="myCache">myCache</param>
        /// <returns></returns>
        public static string GetAccessToken(string appId, string appSecret, ICache myCache)
        {
            //设置accessToken cache过期时间为1小时(微信官方accessToken有效期为2h)
            var expiry = DateTime.Now.AddHours(1);
            var accessToken = myCache.Get("WeiXinAT_" + appId, () => WeiXinHelper.GetAccessToken(appId, appSecret), expiry);
            return accessToken;
        }
    }
}
