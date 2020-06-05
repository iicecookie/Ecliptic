using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecliptic.Database
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        DbContext _context;
        DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public void Add(TEntity item)
        {
            if (item == null) return;
            _dbSet.Add(item);
            _context.SaveChanges();
        }

        public void Add(List<TEntity> items)
        {
            if (items == null) return;
            for (int i = 0, c = 0; i < items.Count; i++)
            {
                _dbSet.Add(items[i]);
                if (c == 50) { c = 0; _context.SaveChanges(); }
            }
            _context.SaveChanges();
        }


        public IEnumerable<TEntity> LoadAll()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<TEntity> LoadAll(Func<TEntity, bool> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }

       
        public TEntity FindById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(TEntity item)
        {
            _dbSet.Remove(item);
            _context.SaveChanges();
        }
    }
}
