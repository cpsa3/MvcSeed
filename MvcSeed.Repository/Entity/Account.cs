using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSeed.Repository.Entity
{
    public class Account
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
