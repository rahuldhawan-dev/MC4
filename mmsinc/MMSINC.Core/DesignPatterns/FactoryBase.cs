using StructureMap;

namespace MMSINC.DesignPatterns
{
    public abstract class FactoryBase
    {
        #region Private Members

        protected readonly IContainer _container;

        #endregion

        #region Constructors

        public FactoryBase(IContainer container)
        {
            _container = container;
        }

        #endregion
    }
}
