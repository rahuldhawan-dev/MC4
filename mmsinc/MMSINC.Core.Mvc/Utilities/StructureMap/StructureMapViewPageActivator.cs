using System;
using System.Web.Mvc;
using StructureMap;

namespace MMSINC.Utilities.StructureMap
{
    public class StructureMapViewPageActivator : IViewPageActivator
    {
        #region Private Members

        protected readonly IContainer _container;

        #endregion

        #region Constructors

        public StructureMapViewPageActivator(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public object Create(ControllerContext controllerContext, Type type)
        {
            var ret = _container.GetInstance(type) ?? Activator.CreateInstance(type);
            return ret;
        }

        #endregion
    }
}
