using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using McvSeed.Web.Helpers;
using MvcSeed.Business.Util;
using MvcSeed.Component.Data;
using MvcSeed.Component.WeiXin.Helpers;
using Microsoft.Practices.Unity;
using MvcSeed.Repository.Entity;
using MvcSeed.Repository.Repo;

namespace McvSeed.Web.Security
{
    public class WeiXinAuthFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            string AppId = CommonHelper.AppId;
            string AppSecret = CommonHelper.AppSecret;

            var currentUser = CurrentContext.GetCurrentUser();

            //判断是否来自微信浏览器
            if ((!CommonHelper.IsWeiXinBrowser()) || (currentUser.UserId != 0 && !string.IsNullOrEmpty(currentUser.OpenId)))
            {
                return;
            }

            if (!string.IsNullOrEmpty(currentUser.OpenId))
            {
                //正常情况不需要再次请求绑定
                //CreateCurrentUser(currentUser.OpenId);
                return;
            }
            else
            {
                //从微信菜单首次跳转至移动端网页时会请求openid
                var wechartCb = filterContext.HttpContext.Request["wechartCb"];
                var code = filterContext.HttpContext.Request["code"];

                //根据code参数判断是否为微信网页授权的回调
                if (!string.IsNullOrEmpty(code))
                {
                    var oauthAT = WeiXinHelper.GetOAuthAccessToken(AppId, AppSecret, code);
                    string openId = oauthAT.openid;

                    currentUser.OpenId = openId;
                    CurrentContext.SetUser(currentUser);

                    CreateCurrentUser(openId);
                }
                else if (string.IsNullOrEmpty(wechartCb))
                {
                    if (filterContext.HttpContext.Request.Url != null)
                    {
                        var url =
                            string.Format(
                                "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect",
                                AppId, GetRedirectUrl(filterContext.HttpContext.Request.Url.AbsoluteUri), "Authorize");
                        filterContext.Result = new RedirectResult(url);
                    }
                }
            }
        }

        /// <summary>
        /// 判断当前微信账号是否已绑定，若绑定则生成currentUser
        /// </summary>
        /// <param name="openId">openId</param>
        /// <returns></returns>
        private bool CreateCurrentUser(string openId)
        {
            using (var unitOfWork = Bootstrapper.Instance.UnityContainer.Resolve<IUnitOfWork>("ReadUnitOfWork"))
            {
                var queryManage = new QueryManage(unitOfWork);
                var commonQuery = new CommonQuery(queryManage);

                var weixinAccount = commonQuery.GetWeixinAccount(openId);

                if (weixinAccount == null)
                {
                    return false;
                }

                var user = commonQuery.GetUserByWeixin(weixinAccount.OrgId, weixinAccount.UserName,
                    weixinAccount.Password);

                if (user != null)
                {
                    var org = commonQuery.GetOrg(user.OrgId);
                    if (org != null)
                    {
                        CommonHelper.InitializationCurrentUser(user, org);
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// 添加自定义参数，用于识别是否来自微信的回调
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetRedirectUrl(string url)
        {
            var redirectUrl = url.Contains("?") ?
                url + "&wechartCb=get" :
                url + "?wechartCb=get";
            return redirectUrl;
        }
    }
}