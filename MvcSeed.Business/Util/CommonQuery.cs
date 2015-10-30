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

        public Org GetOrgByEname(string ename)
        {
            var sqlParams = new Dictionary<string, object>();
            var query = new StringBuilder();

            query.Append(@"SELECT * 
                          FROM Org
                          WHERE ename = @Ename 
                          limit 1;");
            sqlParams.Add("@Ename", ename);

            var orgs = QueryManage.GetList<Org>(query.ToString(), sqlParams);

            return orgs.FirstOrDefault();
        }

        public Org GetOrg(long id)
        {
            throw new NotImplementedException();
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

        public WeixinAccount GetWeixinAccount(string openId)
        {
            const string query = @"SELECT 
                                *
                            FROM
                                weixinaccount
                            WHERE
                                WeiXinID = @OpenId
                            ORDER BY Id DESC";
            return QueryManage.GetList<WeixinAccount>(query, new { OpenId = openId }).FirstOrDefault();
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
    }
}
