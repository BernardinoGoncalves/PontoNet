using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Ucl.PontoNet.Domain.Base;
using Ucl.PontoNet.Domain.Base.Interfaces;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace Ucl.PontoNet.Infra.Data.Base
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {

        public IDbConnection Connection { get; set; }
        protected IUnitOfWork Uow { get; set; }

        public Repository(IUnitOfWork uow)
        {
            this.Uow = uow;
            Connection = uow.Connection;
        }

        public virtual void Delete(Guid id)
        {
            var obj = Activator.CreateInstance<TEntity>();
            obj.Id = id;
            this.Uow.Context.Set<TEntity>().Remove(obj);
            this.Uow.Context.SaveChanges();
        }
        public virtual void Delete(TEntity obj)
        {
            this.Uow.Context.Set<TEntity>().Remove(obj);
            this.Uow.Context.SaveChanges();
        }
        public virtual async void DeleteAsync(TEntity obj)
        {
            this.Uow.Context.Set<TEntity>().Remove(obj);
            await this.Uow.Context.SaveChangesAsync();

        }
        public virtual void DeleteRange(ICollection<TEntity> t)
        {
            this.Uow.Context.Set<TEntity>().RemoveRange(t);
            this.Uow.Context.SaveChanges();
        }

        public virtual TEntity GetById(Guid id)
        {
            return this.Uow.Context.Set<TEntity>().FirstOrDefault(o => o.Id == id);
        }
        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await this.Uow.Context.Set<TEntity>().FirstOrDefaultAsync(o => o.Id == id);
        }

        public virtual TEntity Insert(TEntity obj)
        {
            this.Uow.Context.Set<TEntity>().Add(obj);
            this.Uow.Context.SaveChanges();
            return obj;
        }
        public virtual async Task<TEntity> InsertAsync(TEntity obj)
        {
            this.Uow.Context.Set<TEntity>().Add(obj);
            await this.Uow.Context.SaveChangesAsync();
            return obj;
        }

        public virtual TEntity Update(TEntity obj)
        {
            this.Uow.Context.Set<TEntity>().Update(obj);
            this.Uow.Context.SaveChanges();
            return obj;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity obj)
        {
            this.Uow.Context.Set<TEntity>().Update(obj);
            await this.Uow.Context.SaveChangesAsync();
            return obj;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Connection.GetAll<TEntity>((IDbTransaction)Uow.Transaction);
        }
        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> match, params string[] includeProperties)
        {
            var result = this.Uow.Context.Set<TEntity>().AsQueryable();

            if (includeProperties != null)
            {
                result = includeProperties.Aggregate(result, (current, property) => current.Include(property));
            }

            return result.Where(match);
        }
        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> match, IFilterParameters parameters, string[] includeFilterFields, params string[] includeProperties)
        {
            var result = this.Uow.Context.Set<TEntity>().Where(match);
            if (includeProperties != null)
            {
                result = includeProperties.Aggregate(result, (current, property) => current.Include(property));
            }

            if (!string.IsNullOrWhiteSpace(parameters.filter))
            {
                result = result.ApplyFilter(parameters.filter, includeFilterFields);
            }

            if (!string.IsNullOrWhiteSpace(parameters.sort))
            {
                result = result.OrderBy(parameters.sort);
            }

            return result;
        }

        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> match, params string[] includeProperties)
        {
            var result = this.Uow.Context.Set<TEntity>().AsQueryable();
            if (includeProperties != null)
            {
                result = includeProperties.Aggregate(result, (current, property) => current.Include(property));
            }

            return await result.SingleOrDefaultAsync(match);
        }

        public virtual IQueryable<TEntity> FilterAll(Expression<Func<TEntity, bool>> match, string filter, string[] includeFilterFields, params string[] includeProperties)
        {
            var result = this.Uow.Context.Set<TEntity>().AsQueryable();
            result = includeProperties.Aggregate(result, (current, property) => current.Include(property));

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.ApplyFilter(filter, includeFilterFields);
            }

            return result.Where(match);
        }

        public virtual IQueryable<TEntity> Set => this.Uow.Context.Set<TEntity>();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}