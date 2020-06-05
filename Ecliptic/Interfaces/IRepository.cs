using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ecliptic.Database
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity FindById(int id);
        
        void Add(List<TEntity> items);
        void Add(TEntity item);
        
      
        IEnumerable<TEntity> LoadAll();
        IEnumerable<TEntity> LoadAll(Func<TEntity, bool> predicate);
        
        void Remove(TEntity item);
        void Update(TEntity item);
    }
}