using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCallActiveMQListener.Tests
{
    public class TestUnitOfWorkFactory : UnitOfWorkFactory
    {
        #region Constructors

        private TestUnitOfWorkFactory(IContainer container, ISessionFactory sessionFactory) : base(container, sessionFactory) { }
        public TestUnitOfWorkFactory(IContainer container, ISession session) : this(container, new DummySessionFactory(session)) { }

        #endregion

        #region Private Methods

        protected override IUnitOfWork BuildInstance<TInstance>()
        {
            return new IndisposableUnitOfWorkWrapper(base.BuildInstance<TInstance>());
        }

        #endregion
    }
}