using MMSINC.Data;
using NHibernate;
using StructureMap;

namespace MMSINC.Data.NHibernate
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        #region Private Members

        protected readonly IContainer _container;
        protected readonly ISessionFactory _sessionFactory;

        #endregion

        #region Constructors

        public UnitOfWorkFactory(IContainer container, ISessionFactory sessionFactory)
        {
            _container = container;
            _sessionFactory = sessionFactory;
        }

        #endregion

        #region Private Methods

        protected virtual IUnitOfWork BuildInstance<TInstance>()
            where TInstance : UnitOfWork
        {
            return _container.GetNestedContainer()
                             .With(typeof(ISession), _sessionFactory.OpenSession())
                             .GetInstance<TInstance>();
        }

        #endregion

        #region Exposed Methods

        public virtual IUnitOfWork Build()
        {
            return BuildInstance<UnitOfWork>();
        }

        public virtual IUnitOfWork BuildMemoized()
        {
            return BuildInstance<MemoizedUnitOfWork>();
        }

        #endregion
    }

    public class MemoizedUnitOfWork : UnitOfWork
    {
        #region Constructors

        public MemoizedUnitOfWork(IContainer container, ISession session) : base(container,
            new MemoizedSessionWrapper(session)) { }

        #endregion
    }
}
