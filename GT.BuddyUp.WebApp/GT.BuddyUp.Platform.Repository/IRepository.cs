using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace GT.BuddyUp.Platform.Repository
{
    public interface IRepository<T> where T : class
    {
        int GetCount(Expression<Func<T, bool>> filter = null);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includes = "", int? skip = null, int? take = null);
        IEnumerable<T> Get(string filter, object[] filterValues = null, string orderBy = null, string includes = "", int? skip = null, int? take = null);
        T GetByID(object id);
        void Add(T entity);
        void Update(T entityToUpdate);
        bool Delete(object id);
        bool Delete(T entityToDelete);
        bool Delete(IEnumerable<T> entities);
        IRepository<T> Use(IUnitOfWork uow);
        bool Exists(Expression<Func<T, bool>> condition);
    }
}
