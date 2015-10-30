using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcSeed.Component.Data;
using MySql.Data.MySqlClient;

namespace MvcSeed.Repository
{
    public class DefaultUnitOfWork : IUnitOfWork
    {
        public IDbConnection Connection { get; private set; }

        private readonly Stack<IDbTransaction> transactions;

        public DefaultUnitOfWork(string connectionString)
        {
            this.Connection = new MySqlConnection(connectionString);
            this.transactions = new Stack<IDbTransaction>();
        }

        private void OpenConnection()
        {
            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }
        }

        private void CloseConnection()
        {
            if (this.Connection.State != ConnectionState.Closed)
            {
                this.Connection.Close();
            }
        }

        public void BeginTran()
        {
            OpenConnection();
            this.transactions.Push(this.Connection.BeginTransaction());
        }

        public void Commit()
        {
            if (this.transactions.Any())
            {
                OpenConnection();
                this.transactions.Pop().Commit();
                CloseConnection();
            }
        }

        public void Rollback()
        {
            if (this.transactions.Any())
            {
                OpenConnection();
                this.transactions.Pop().Rollback();
                CloseConnection();
            }
        }

        public IDbTransaction GetLastTransaction()
        {
            return this.transactions.LastOrDefault();
        }

        public void Dispose()
        {
            CloseConnection();
            this.Connection.Dispose();
        }
    }
}
