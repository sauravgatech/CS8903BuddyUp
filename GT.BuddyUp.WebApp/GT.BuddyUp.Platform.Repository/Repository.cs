using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Data;

namespace GT.BuddyUp.Platform.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbContext _context;
        private DbSet<T> _dbSet;

        public virtual IRepository<T> Use(IUnitOfWork uow)
        {
            _context = uow.Context;
            _dbSet = _context.Set<T>();
            return this;
        }

        #region Allow a mechanism to generate dynamic queries (see ProductLookup.cs)
        //If this route is ever used, then IRepository.cs and any unit test will have to stub these out accordingly
        //public virtual IQueryable<T> GetContext()
        //{
        //    // Stub version for unit test would just be: return new List<T>().AsQueryable();
        //    return _dbSet;
        //}

        //public virtual int RunDynamicCount(IQueryable<T> query)
        //{
        //    return query.Count();
        //}

        //public virtual IEnumerable<T> RunDynamicQuery(IQueryable<T> query, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includes = "", int? skip = null, int? take = null)
        //{
        //    if (orderBy != null)
        //    {
        //        query = orderBy(query);

        //        if (skip.HasValue)
        //        {
        //            query = query.Skip(skip.Value);
        //        }

        //        if (take.HasValue)
        //        {
        //            query = query.Take(take.Value);
        //        }
        //    }

        //    return query.ToList();
        //}
        #endregion

        public virtual int GetCount(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includes = "", int? skip = null, int? take = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includes.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (orderBy != null && skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (orderBy != null && take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.ToList();
        }


        public virtual IEnumerable<T> Get(string filter, object[] filterValues = null, string orderBy = "", string includes = "", int? skip = null, int? take = null)
        {
            IQueryable<T> query = _dbSet;

            if (!String.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(filter, filterValues);
            }

            foreach (var includeProperty in includes.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (!String.IsNullOrWhiteSpace(orderBy))
            {
                query = query.OrderBy(orderBy);
            }

            if (!String.IsNullOrWhiteSpace(orderBy) && skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (!String.IsNullOrWhiteSpace(orderBy) && take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.ToList();
        }

        public virtual T GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual bool Delete(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            return Delete(entityToDelete);
        }

        public virtual bool Delete(T entityToDelete)
        {
            if (null == entityToDelete) return false;
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
            return true;
        }

        public virtual bool Delete(IEnumerable<T> entities)
        {
            if (null == entities) return false;
            _dbSet.RemoveRange(entities);
            return true;
        }

        public virtual void Update(T entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual bool Exists(Expression<Func<T, bool>> condition)
        {
            IQueryable<T> query = _dbSet;
            return query.Any(condition);
        }

    }
}
