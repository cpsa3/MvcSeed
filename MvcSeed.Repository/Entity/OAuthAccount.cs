using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSeed.Repository.Entity
{
    /// <summary>
    /// 第三方授权登录
    /// </summary>
    public class OAuthAccount
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string OAuthCode { get; set; }
        public OAuthSource Source { get; set; }
        public DateTime CreatedTime { get; set; }
    }

    public enum OAuthSource
    {
        Wechart = 1,
        Github = 2,
        Weibo = 3
    }
}
