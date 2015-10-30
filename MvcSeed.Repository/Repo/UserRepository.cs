using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcSeed.Component.Data;
using MvcSeed.Repository.Entity;

namespace MvcSeed.Repository.Repo
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(IUnitOfWork uw) : base(uw) { }
    }
}
