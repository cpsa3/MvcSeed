using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSeed.Repository.Entity
{
    public class User
    {
        public long Id { get; set; }

        public long OrgId { get; set; }

        public string UserName { get; set; }

        public string UserPw { get; set; }

        public string SchoolLimit { get; set; }

        public long Level { get; set; }

        public string Authority { get; set; }

        public string CardId { get; set; }

        public string UserSex { get; set; }

        public string UserTel { get; set; }

        public bool Enable { get; set; }

        public DateTime EnableTimeStart { get; set; }

        public DateTime EnableTimeEnd { get; set; }

        public string EnableWeekDays { get; set; }
    }
}
