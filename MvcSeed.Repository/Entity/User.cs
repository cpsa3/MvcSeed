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
        public string UserName { get; set; }
        public bool Enable { get; set; }
        public DateTime CreatedTime { get; set; }
        public string AvatarUrl { get; set; }
    }
}
