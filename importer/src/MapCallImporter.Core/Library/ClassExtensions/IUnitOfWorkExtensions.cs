using System;
using System.Linq;
using System.Linq.Expressions;
using MMSINC.Data.V2;

// ReSharper disable once CheckNamespace
namespace MapCallImporter.Library.ClassExtensions
{
    // ReSharper disable once InconsistentNaming
    public static class IUnitOfWorkExtensions
    {
        #region Exposed Methods

        public static T Find<T>(this IUnitOfWork that, object id)
        {
            return that.GetRepository<T>().Find(id);
        }

        public static IQueryable<T> Where<T>(this IUnitOfWork that, Expression<Func<T, bool>> predicate)
        {
            return that.GetRepository<T>().Where(predicate);
        }

        public static T Insert<T>(this IUnitOfWork that, T entity)
        {
            return that.GetRepository<T>().Insert(entity);
        }

        public static T Update<T>(this IUnitOfWork that, T entity)
        {
            return that.GetRepository<T>().Update(entity);
        }

        #endregion
    }
}
