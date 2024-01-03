using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace MMSINC.ControllerFactories
{
    public class CompositeControllerFactory : IControllerFactory
    {
        #region Properties

        public IList<ICompositableControllerFactory> Factories { get; private set; }

        #endregion

        #region Constructors

        public CompositeControllerFactory()
        {
            Factories = new Collection<ICompositableControllerFactory>();
        }

        #endregion

        #region Private Methods

        private IControllerFactory GetControllerFactory(RequestContext requestContext, string controllerName)
        {
            return Factories.FirstOrDefault(x => x.CanHandleController(requestContext, controllerName));
        }

        // This signature is here to match DefaultControllerFactory and because RouteContext needs it.
        protected Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            foreach (var f in Factories)
            {
                var type = f.TryGetControllerType(requestContext, controllerName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        #endregion

        #region Public Methods

        [DebuggerStepThrough]
        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            var factory = GetControllerFactory(requestContext, controllerName);
            if (factory != null)
            {
                return factory.CreateController(requestContext, controllerName);
            }

            // Need to throw a 404 since that's what DefaultControllerFactory does and also ensures
            // we get the pretty 404 pages.
            var err = string.Format("Unable to find a registered ControllerFactory that can handle '{0}'.",
                controllerName);
            throw new HttpException(404, err);
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            // DefaultFactoryController returns SessionStateBehavior.Default when no controller type is
            // found, so mimic that behavior. 
            var factory = GetControllerFactory(requestContext, controllerName);
            return (factory != null
                ? factory.GetControllerSessionBehavior(requestContext, controllerName)
                : SessionStateBehavior.Default);
        }

        public void ReleaseController(IController controller)
        {
            // This could be better, but I don't want to cache all the controllers with their factories
            // until we need it. The DefaultControllerFactory just disposes the controller and nothing
            // else, so repeating that here.

            var disp = controller as IDisposable;
            if (disp != null)
            {
                disp.Dispose();
            }
        }

        #endregion
    }
}
