using System.Linq;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.HttpUtility;
using System.Collections.Generic;

namespace MvcSeed.Component.WeiXin.Helpers
{
    using System;

    using MvcSeed.Component.Helpers;
    using MvcSeed.Component.Util;

    using Senparc.Weixin.MP.Entities;

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

        public static CustomWxJsonResult<WeixinUserResult> GetWeixinUserHasRetry(string appId, string appSecret, ICache cache, string openid)
        {
            try
            {
                return WeiXinUtility.Do(accessToken =>
                {
                    var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}", accessToken, openid);

                    var user = Get.GetJson<WeixinUserResult>(url);

                    var result = new CustomWxJsonResult<WeixinUserResult> { Data = user };
                    return result;
                }, appId, appSecret, cache);
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetWeixinUserHasRetry error：" + ex.ToString());
                throw;
            }
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
            try
            {
                const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
                var msgData = new TempleteModel
                {
                    touser = openId,
                    template_id = templateId,
                    url = url,
                    data = data
                };
                return CommonJsonSend.Send<SendTemplateMessageResult>(accessToken, urlFormat, msgData);
            }
            catch (ErrorJsonResultException ex)
            {
                return new SendTemplateMessageResult
                {
                    errcode = ex.JsonResult.errcode,
                    errmsg = ex.JsonResult.errmsg,
                    P2PData = ex.JsonResult.P2PData
                };
            }
        }

        public static SendTemplateMessageResult SendTemplateMessageHasRetry(string appId, string appSecret, ICache cache, string openId, string templateId, string url, object data)
        {
            try
            {
                return WeiXinUtility.Do(accessToken =>
                {
                    const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
                    var msgData = new TempleteModel
                    {
                        touser = openId,
                        template_id = templateId,
                        url = url,
                        data = data
                    };
                    return CommonJsonSend.Send<SendTemplateMessageResult>(accessToken, urlFormat, msgData);
                }, appId, appSecret, cache);
            }
            catch (ErrorJsonResultException ex)
            {
                return new SendTemplateMessageResult
                {
                    errcode = ex.JsonResult.errcode,
                    errmsg = ex.JsonResult.errmsg,
                    P2PData = ex.JsonResult.P2PData
                };
            }
        }

        #region QrCode

        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="accessToken">accessToken</param>
        /// <param name="expireSeconds">该二维码有效时间，以秒为单位。 最大不超过1800。0时为永久二维码</param>
        /// <param name="sceneId">场景值ID，临时二维码时为32位整型，永久二维码时最大值为1000</param>
        /// <returns></returns>
        public static CreateQrCodeResult CreateQrTicket(string accessToken, int expireSeconds, int sceneId)
        {
            return QrCode.Create(accessToken, expireSeconds, sceneId);
        }

        /// <summary>
        /// 获取下载二维码的地址
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static string GetShowQrCodeUrl(string ticket)
        {
            return QrCode.GetShowQrCodeUrl(ticket);
        }

        /// <summary>
        /// 用字符串类型创建二维码
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="sceneStr">场景值ID（字符串形式的ID），字符串类型，长度限制为1到64，仅永久二维码支持此字段</param>
        /// <returns></returns>
        public static CreateQrCodeResult CreateQrTicketByStr(string accessToken, string sceneStr)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            var data = new
            {
                action_name = "QR_LIMIT_STR_SCENE",
                action_info = new
                {
                    scene = new
                    {
                        scene_str = sceneStr
                    }
                }
            };
            return CommonJsonSend.Send<CreateQrCodeResult>(accessToken, urlFormat, data);
        }

        #endregion

    }

    public class CustomWxJsonResult<T> : WxJsonResult
    {
        public T Data { get; set; }
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
