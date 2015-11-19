using System.Threading.Tasks;
using MvcSeed.Component.Github.Entities;
using MvcSeed.Component.Github.Helpers;
using MvcSeed.Component.Helpers;
using MvcSeed.Repository.Entity;
using MvcSeed.Web.Helpers;
using MvcSeed.Web.Models;
using System.Web.Mvc;

namespace MvcSeed.Web.Controllers
{
    public class OAuthController : BaseController
    {
        /// <summary>
        /// 第三方授权回调
        /// </summary>
        /// <returns></returns>
        public ActionResult AuthorizeCallback(AuthorizeDto dto)
        {
            //TODO 校验state,防止跨站请求伪造攻击

            var accessToken = GithubHelper.GetAccessToken(new AccessTokenRequest
            {
                client_id = CommonHelper.ClientId,
                client_secret = CommonHelper.ClientSecret,
                code = dto.code
            });

            var githubUser = GithubHelper.GetGithubUser(accessToken);

            //TODO 根据githubUser.id获取userId，若首次访问，则新建OAuthAccount记录和User记录


            return Json(githubUser, JsonRequestBehavior.AllowGet);
        }

        private void GetOAuthUser(long oauthId)
        {

        }
    }
}