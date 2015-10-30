using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.HttpUtility;
using System.Collections.Generic;

namespace MvcSeed.Component.WeiXin.Helpers
{
    public class WeiXinHelper
    {
        public static OAuthAccessToken GetOAuthAccessToken(string appid, string secret, string code, string grant_type = "authorization_code")
        {
            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}",
                                    appid, secret, code, grant_type);

            var accessToken = Get.GetJson<OAuthAccessToken>(url);
            return accessToken;
        }

        public static OAuthUserInfo GetOAuthUserInfo(string accessToken, string openid)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", accessToken, openid);

            var oauthUserInfo = Get.GetJson<OAuthUserInfo>(url);

            return oauthUserInfo;
        }

        public static string GetAccessToken(string appId, string appSecret)
        {
            var accessToken = CommonApi.GetToken(appId, appSecret);
            return accessToken.access_token;
        }

        public static List<string> GetFollowersIds(string accessToken)
        {
            var openIds = new List<string>();

            var firstFollowersEntity = GetFollowersEntity(accessToken, string.Empty);

            if (firstFollowersEntity != null && firstFollowersEntity.count > 0)
            {
                var followersEntity = firstFollowersEntity;
                while (followersEntity.count > 0)
                {
                    openIds.AddRange(followersEntity.data.openid);
                    var temp = GetFollowersEntity(accessToken, followersEntity.next_openid);

                    followersEntity = temp;
                }
            }

            return openIds;
        }

        public static FollowersEntity GetFollowersEntity(string accessToken, string next_openid)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}", accessToken, next_openid);

            var followersEntity = Get.GetJson<FollowersEntity>(url);

            return followersEntity;
        }

        public static WeixinUserResult GetWeixinUser(string accessToken, string openid)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}", accessToken, openid);

            var user = Get.GetJson<WeixinUserResult>(url);

            return user;
        }

        public static List<WeixinUserResult> GetFollowers(string accessToken)
        {
            var openids = GetFollowersIds(accessToken);
            var users = new List<WeixinUserResult>();

            foreach (var openid in openids)
            {
                var user = GetWeixinUser(accessToken, openid);
                users.Add(user);
            }

            return users;
        }

        public static SendTemplateMessageResult SendTemplateMessage(string accessToken, string openId, string templateId, string url, object data)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
            var msgData = new TempleteModel()
            {
                touser = openId,
                template_id = templateId,
                url = url,
                data = data
            };
            return CommonJsonSend.Send<SendTemplateMessageResult>(accessToken, urlFormat, msgData);
        }
    }

    /// <summary>
    /// same as RefreshAccessToken
    /// </summary>
    public class OAuthAccessToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }

    public class OAuthUserInfo
    {
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public string[] privilege { get; set; }
    }

    public class FollowersEntity
    {
        public long total { get; set; }
        public long count { get; set; }
        public Data data { get; set; }

        public string next_openid { get; set; }

        public class Data
        {
            public string[] openid { get; set; }
        }
    }

    public class WeixinUserResult
    {
        public int subscribe { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public long subscribe_time { get; set; }
    }
}
