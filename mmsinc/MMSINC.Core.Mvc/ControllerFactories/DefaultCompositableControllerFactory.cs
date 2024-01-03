using System;
using System.Web.Mvc;
using System.Web.Routing;
using JetBrains.Annotations;
using StructureMap;

namespace MMSINC.ControllerFactories
{
    public class DefaultCompositableControllerFactory : DefaultControllerFactory, ICompositableControllerFactory
    {
        private readonly IContainer _container;

        public DefaultCompositableControllerFactory(IContainer container)
        {
            _container = container;
        }

        #region Private Methods

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return (IController)_container.GetInstance(controllerType);
        }

        #endregion

        #region Exposed Methods

        public bool CanHandleController(RequestContext requestContext, [AspMvcController] string controllerName)
        {
            return TryGetControllerType(requestContext, controllerName) != null;
        }

        public Type TryGetControllerType(RequestContext requestContext, string controllerName)
        {
            return GetControllerType(requestContext, controllerName);
        }

        #endregion
    }
}
