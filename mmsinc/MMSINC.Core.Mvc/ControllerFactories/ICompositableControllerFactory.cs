using System;
using System.Web.Mvc;
using System.Web.Routing;
using JetBrains.Annotations;

namespace MMSINC.ControllerFactories
{
    /// <summary>
    /// Interface for representing a ControllerFactory that can be used by a CompositeControllerFactory
    /// </summary>
    public interface ICompositableControllerFactory : IControllerFactory
    {
        /// <summary>
        /// Returns true if this factory can handle a route for the given controller name.
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        bool CanHandleController(RequestContext requestContext, [AspMvcController] string controllerName);

        Type TryGetControllerType(RequestContext requestContext, [AspMvcController] string controllerName);
    }
}
