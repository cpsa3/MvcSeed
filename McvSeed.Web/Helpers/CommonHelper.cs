using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MvcSeed.Web.Security;
using MvcSeed.Business.Util;
using MvcSeed.Component.Util;
using Microsoft.Practices.Unity;
using MvcSeed.Component.WeiXin.Helpers;
using MvcSeed.Repository.Entity;

namespace MvcSeed.Web.Helpers
{
    public class CommonHelper
    {
        public static string WeiXinToken = ConfigurationManager.AppSettings["weixinToken"];
        public static string AppId = ConfigurationManager.AppSettings["appId"];
        public static string AppSecret = ConfigurationManager.AppSettings["appSecret"];
        public static string TemplateId = ConfigurationManager.AppSettings["templateId"];
        public static string TemplateBaseUrl = ConfigurationManager.AppSettings["templateBaseUrl"];
        private static readonly ICache cache = Bootstrapper.Instance.UnityContainer.Resolve<ICache>();


        public static bool IsWeiXinBrowser()
        {
            return HttpContext.Current.Request.UserAgent != null && HttpContext.Current.Request.UserAgent.Contains("MicroMessenger");
        }

        public static CurrentUser InitializationCurrentUser(User user, Org org)
        {
            var currentUser = CurrentContext.GetCurrentUser();

            currentUser.UserId = user.Id;
            currentUser.UserName = user.UserName;

            CurrentContext.SetUser(currentUser);

            return currentUser;
        }

        public static string GetAccessToken(string appId, string appSecret)
        {
            return WeiXinUtility.GetAccessToken(appId, appSecret, cache);
        }

        public static string GetAccessToken()
        {
            return WeiXinUtility.GetAccessToken(AppId, AppSecret, cache);
        }
    }
}