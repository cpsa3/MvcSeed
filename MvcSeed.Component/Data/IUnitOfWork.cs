using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MvcSeed.Component.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }

        void BeginTran();

        void Commit();

        void Rollback();

        IDbTransaction GetLastTransaction();
    }
}
