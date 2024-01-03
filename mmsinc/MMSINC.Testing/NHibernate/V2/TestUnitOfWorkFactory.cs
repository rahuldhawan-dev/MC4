using MMSINC.Data.V2;
using MMSINC.Data.V2.NHibernate;
using StructureMap;

namespace MMSINC.Testing.NHibernate.V2
{
    public class TestUnitOfWorkFactory : UnitOfWorkFactory
    {
        #region Constructors

        public TestUnitOfWorkFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        protected override IUnitOfWork BuildInstance<TInstance>()
        {
            return new IndisposableUnitOfWorkWrapper(base.BuildInstance<TInstance>());
        }

        #endregion
    }
}
