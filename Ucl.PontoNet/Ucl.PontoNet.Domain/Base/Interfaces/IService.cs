using System;
using System.Collections.Generic;

namespace Ucl.PontoNet.Domain.Base.Interfaces
{
    public interface IService<TEntity> : IDisposable where TEntity : class
    {
        void Delete(TEntity obj);

        IEnumerable<TEntity> GetAll();

        TEntity GetById(Guid id);

        TEntity Insert(TEntity obj);

        TEntity Update(TEntity obj);
    }
}