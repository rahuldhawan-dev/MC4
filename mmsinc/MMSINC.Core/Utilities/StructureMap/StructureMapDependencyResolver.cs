using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace MMSINC.Utilities.StructureMap
{
    /// <summary>
    /// Bridges between a StructureMap IContainer and the Mvc framework's DependencyResolver,
    /// for when constructor injection is not possible (such as for ViewModels).
    /// </summary>
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        #region Private Members

        private readonly IContainer _container;

        #endregion

        #region Constructors

        public StructureMapDependencyResolver(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public object GetService(Type serviceType)
        {
            return (serviceType.IsAbstract || serviceType.IsInterface)
                ? _container.TryGetInstance(serviceType)
                : _container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances<object>()
                             .Where(s => s.GetType() == serviceType);
        }

        #endregion
    }
}
