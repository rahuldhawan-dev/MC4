using System;
using System.Linq;
using System.Linq.Expressions;

namespace MMSINC.Data.V2
{
    public interface IRepository<T>
    {
        #region Abstract Methods

        T Insert(T entity);

        /// <summary>
        /// Performs a lookup and will return null if the entity was not found.
        /// </summary>
        T Find(object id);

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();

        T Update(T valve);

        #endregion
    }
}
