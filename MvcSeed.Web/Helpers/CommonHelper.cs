﻿namespace MvcSeed.Web.Helpers
{
    using System.Configuration;
    using System.Web;

    using Microsoft.Practices.Unity;

    using MvcSeed.Business.Util;
    using MvcSeed.Component.Util;
    using MvcSeed.Component.WeiXin.Helpers;
    using MvcSeed.Repository.Entity;
    using MvcSeed.Web.Security;

    public class CommonHelper
    {
        #region Wechart

        public static string WeiXinToken = ConfigurationManager.AppSettings["weixinToken"];
        public static string AppId = ConfigurationManager.AppSettings["appId"];
        public static string AppSecret = ConfigurationManager.AppSettings["appSecret"];
        public static string TemplateId = ConfigurationManager.AppSettings["templateId"];
        public static string TemplateBaseUrl = ConfigurationManager.AppSettings["templateBaseUrl"];

        #endregion

        #region Github

        public static string ClientId = ConfigurationManager.AppSettings["ClientId"];
        public static string ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];

        #endregion

        private static readonly ICache cache = Bootstrapper.Instance.UnityContainer.Resolve<ICache>();


        public static bool IsWeiXinBrowser()
        {
            return HttpContext.Current.Request.UserAgent != null && HttpContext.Current.Request.UserAgent.Contains("MicroMessenger");
        }

        public static CurrentUser InitializationCurrentUser(User user, OAuthSource source)
        {
            var currentUser = CurrentContext.GetCurrentUser();

            currentUser.UserId = user.Id;
            currentUser.UserName = user.UserName;
            currentUser.Source = source;

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