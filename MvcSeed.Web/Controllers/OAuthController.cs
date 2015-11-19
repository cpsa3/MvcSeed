using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MvcSeed.Business.Util;
using MvcSeed.Component.Github.Entities;
using MvcSeed.Component.Github.Helpers;
using MvcSeed.Component.Helpers;
using MvcSeed.Repository.Entity;
using MvcSeed.Repository.Repo;
using MvcSeed.Web.Helpers;
using MvcSeed.Web.Models;
using System.Web.Mvc;
using MvcSeed.Web.Security;

namespace MvcSeed.Web.Controllers
{
    public class OAuthController : BaseController
    {
        /// <summary>
        /// 第三方授权回调
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AuthorizeCallback(AuthorizeDto dto)
        {
            //TODO 校验state,防止跨站请求伪造攻击

            var accessToken = await GithubHelper.GetAccessTokenAsync(new AccessTokenRequest
            {
                client_id = CommonHelper.ClientId,
                client_secret = CommonHelper.ClientSecret,
                code = dto.code
            });

            var githubUser = await GithubHelper.GetGithubUserAsync(accessToken);

            var user = GetOAuthUser(githubUser);

            if (user != null)
            {
                var currentUser = CurrentContext.GetCurrentUser();
                currentUser.UserId = user.Id;
                currentUser.UserName = user.UserName;
                CurrentContext.SetUser(currentUser);

                return RedirectToAction("Index", "Home");
            }

            return Content("用户已经被禁用，请联系管理员");

        }

        /// <summary>
        /// 根据githubUser.id获取userId，若首次访问，则新建OAuthAccount记录和User记录
        /// </summary>
        /// <param name="githubUser"></param>
        /// <returns></returns>
        private User GetOAuthUser(GithubUserInfoResult githubUser)
        {
            using (UnitOfWork)
            {
                User user = null;
                var queryManage = new QueryManage(UnitOfWork);
                var commonQuery = new CommonQuery(queryManage);
                var userRepo = new UserRepository(UnitOfWork);
                var oauthAccountRepo = new BaseRepository<OAuthAccount>(UnitOfWork);

                const string query = @"SELECT *
                                        FROM oauthaccount
                                        WHERE OAuthCode = @OAuthCode
	                                        AND Source = @Source";
                var oauthAccount = queryManage.GetList<OAuthAccount>(query, new { OAuthCode = githubUser.id, Source = OAuthSource.Github })
                    .FirstOrDefault();

                if (oauthAccount == null)
                {
                    user = new User
                    {
                        UserName = githubUser.login,
                        Enable = true,
                        CreatedTime = DateTime.Now
                    };
                    user.Id = userRepo.CreateWithIdentity(user);

                    oauthAccountRepo.Create(new OAuthAccount
                    {
                        OAuthCode = githubUser.id.ToString(CultureInfo.InvariantCulture),
                        Source = OAuthSource.Github,
                        UserId = user.Id,
                        CreatedTime = DateTime.Now
                    });
                }
                else
                {
                    user = commonQuery.GetUser(oauthAccount.UserId);
                }

                return user;
            }
        }
    }
}