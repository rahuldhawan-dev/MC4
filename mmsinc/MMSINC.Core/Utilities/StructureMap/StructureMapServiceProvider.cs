using System;
using StructureMap;

namespace MMSINC.Utilities.StructureMap
{
    public class StructureMapServiceProvider : IServiceProvider
    {
        #region Private Members

        private readonly IContainer _container;

        #endregion

        #region Constructors

        public StructureMapServiceProvider(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public object GetService(Type serviceType)
        {
            return _container.GetInstance(serviceType);
        }

        #endregion
    }
}
