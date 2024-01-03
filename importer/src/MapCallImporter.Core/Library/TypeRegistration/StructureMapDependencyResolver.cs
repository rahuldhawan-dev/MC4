using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using StructureMap;

namespace MapCallImporter.Library.TypeRegistration
{
    public class StructureMapDependencyResolver : ServiceLocatorImplBase
    {
        #region Private Members

        protected readonly IContainer _container;

        #endregion

        #region Constructors

        public StructureMapDependencyResolver(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Private Methods

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return key == null ? _container.GetInstance(serviceType) : _container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType).Cast<object>();
        }

        #endregion
    }
}
