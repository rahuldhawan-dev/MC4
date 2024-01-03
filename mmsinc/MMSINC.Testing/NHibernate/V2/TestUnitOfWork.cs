using MMSINC.Data.V2.NHibernate;
using NHibernate;
using StructureMap;

namespace MMSINC.Testing.NHibernate.V2
{
    public class TestUnitOfWork : UnitOfWork
    {
        #region Constructors

        public TestUnitOfWork(IContainer container, ISession session) : base(container, session) { }

        #endregion

        #region Private Methods

        protected override void DisposeSession() { }

        #endregion
    }
}
