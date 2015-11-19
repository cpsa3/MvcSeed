using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcSeed.Repository.Entity;
using MvcSeed.Repository.Repo;

namespace MvcSeed.Business.Util
{
    public class CommonQuery
    {
        public QueryManage QueryManage { get; private set; }

        public CommonQuery(QueryManage queryManage)
        {
            this.QueryManage = queryManage;
        }

        public User GetUserByAccount(long orgId, string userName, string userPw)
        {
            string query = @"SELECT *
                              FROM user
                              WHERE userName = @UserName
	                                AND orgId = @OrgId
	                                AND userPw = @UserPw";
            return QueryManage.GetList<User>(query, new
            {
                UserName = userName,
                OrgId = orgId,
                UserPw = userPw
            }).FirstOrDefault();
        }

        public User GetUserByWeixin(long orgId, string userName, string userPw)
        {
            var query = @"SELECT user.*
                            FROM user
                            WHERE orgid = @OrgId
	                            AND username = @UserName
	                            AND userpw = @UserPw
	                            AND enable = 1;";

            return QueryManage.GetList<User>(query, new
            {
                OrgId = orgId,
                UserName = userName,
                UserPw = userPw
            }).FirstOrDefault();
        }

        public OAuthAccount GetWechartAccount(string openId)
        {
            const string query = @"SELECT 
                                    *
                                FROM
                                    oauthaccount
                                WHERE
                                    OAuthCode = @OpenId
                                AND Source = @Source
                                ORDER BY Id DESC";
            return QueryManage.GetList<OAuthAccount>(query, new { OAuthCode = openId, Source = OAuthSource.Wechart }).FirstOrDefault();
        }

        public User GetUser(long userId)
        {
            const string query = @"SELECT 
                                    *
                                FROM
                                    user
                                WHERE
                                    Id = @UserId
                                AND Enable = 1
                                ORDER BY Id DESC";
            return QueryManage.GetList<User>(query, new { UserId = userId }).FirstOrDefault();
        }
    }
}
