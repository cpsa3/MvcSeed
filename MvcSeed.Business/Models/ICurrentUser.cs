using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSeed.Business.Models
{
    public interface ICurrentUser
    {
        long UserId { get; set; }
        string UserName { get; set; }
    }
}
