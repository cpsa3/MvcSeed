using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSeed.Repository.Entity
{
    public class WeixinAccount
    {
        public long Id { get; set; }
        public long OrgId { get; set; }
        public string WeixinId { get; set; }
        public string SysFolder { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime BoundDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string Crmver { get; set; }
    }
}
