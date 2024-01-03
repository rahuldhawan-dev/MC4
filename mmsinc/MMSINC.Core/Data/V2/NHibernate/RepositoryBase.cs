using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;

namespace MMSINC.Data.V2.NHibernate
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        #region Properties

        public ISession Session { get; }

        protected IQueryable<T> Linq => Session.Query<T>();

        #endregion

        #region Constructors

        public RepositoryBase(ISession session)
        {
            Session = session;
        }

        #endregion

        #region Exposed Methods

        public T Find(object id)
        {
            return Session.Get<T>(id);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return Linq.Where(predicate);
        }

        public T Insert(T entity)
        {
            Session.Save(entity);
            return entity;
        }

        public T Update(T entity)
        {
            // passing the full type name here to make it so that proxy objects
            // that make their way here get saved as the objects they really are
            Session.SaveOrUpdate(typeof(T).FullName, entity);
            return entity;
        }

        public IQueryable<T> GetAll()
        {
            return Linq;
        }

        #endregion
    }
}
