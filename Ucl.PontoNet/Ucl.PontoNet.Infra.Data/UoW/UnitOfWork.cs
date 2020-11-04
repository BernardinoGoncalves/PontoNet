using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data;
using System.Data.Common;
using Ucl.PontoNet.Domain.Base.Interfaces;

namespace Ucl.PontoNet.Infra.Data.UoW
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public IDbConnection Connection { get; set; }
        public DbContext Context { get; set; }
        public Guid Id { get; set; }

        public IDbTransaction Transaction { get; set; }
        // public IDbContextTransaction ContextTransaction { get; set; }
        public bool ValidateEntity { get; set; }

        public UnitOfWork(IDbConnection connection, DbContext context)
        {
            this.Context = context;
            this.ValidateEntity = true;
            this.Context.Database.OpenConnection();
            this.Connection = this.Context.Database.GetDbConnection();
        }

        public void BeginTransaction()
        {
            var transactionContext = this.Context.Database.BeginTransaction();
            Transaction = (transactionContext as IInfrastructure<DbTransaction>).Instance;
        }


        public void Commit(bool dispose = true)
        {
            Transaction.Commit();

            if (dispose)
            {
                Dispose();
            }

        }


        public void Dispose()
        {
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                Connection?.Dispose();
                Connection = null;
            }

            if (Transaction != null)
            {
                Transaction?.Dispose();

                Transaction = null;
            }

            if (Context != null)
            {
                Context?.Dispose();

                Context = null;
            }
        }

        public void Open()
        {
            Connection.Open();
        }

        public void Rollback(bool dispose = true)
        {
            Transaction.Rollback();


            if (dispose)
            {
                Dispose();
            }
        }


    }
}