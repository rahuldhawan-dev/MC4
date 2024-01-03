using System;
using System.Diagnostics;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MMSINC.Testing.NHibernate
{
    public class InMemoryDatabaseTestWithUnitOfWork<TEntity, TRepository> : InMemoryDatabaseTest<TEntity, TRepository>
        where TRepository : class, IRepository<TEntity>
        where TEntity : class
    {
        #region Exposed Methods

        [DebuggerStepThrough]
        public void WithUnitOfWork(Action<IUnitOfWork> fn)
        {
            using (var uow = Container.GetInstance<TestUnitOfWorkFactory>().Build())
            {
                fn(uow);
            }
        }

        #endregion
    }

    public class TestUnitOfWorkFactory : UnitOfWorkFactory
    {
        #region Constructors

        private TestUnitOfWorkFactory(IContainer container, ISessionFactory sessionFactory) : base(container,
            sessionFactory) { }

        public TestUnitOfWorkFactory(IContainer container, ISession session) : this(container,
            new DummySessionFactory(session)) { }

        #endregion

        #region Private Methods

        protected override IUnitOfWork BuildInstance<TInstance>()
        {
            return new IndisposableUnitOfWorkWrapper(base.BuildInstance<TInstance>());
        }

        #endregion
    }
}
