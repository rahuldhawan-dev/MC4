using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using MMSINC.ControllerFactories;
using MMSINC.Testing.Utilities;

namespace MMSINC.Testing
{
    /// <summary>
    /// For use with MvcApplicationTester. Probably shouldn't use it outside of that.
    /// </summary>
    public class FakeCompositeControllerFactory : ICompositableControllerFactory, IStaticPropertyReplacer
    {
        #region Fields

        private ControllerBuilder _controllerBuilder;
        private bool _isInited;
        private readonly Dictionary<string, Dictionary<string, Lazy<ControllerBase>>> _controllersByNamespace;
        private CompositeControllerFactory _actualControllerFactory;

        private static readonly PropertyInfo _controllerFactoryBuildManagerProperty =
            typeof(DefaultControllerFactory).GetProperty("BuildManager",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        #endregion

        #region Properties

        /// <summary>
        /// Set to true if this should try to simulate how DefaultControllerFactory reacts to things.
        /// </summary>
        public bool ActLikeDefaultControllerFactory { get; set; }

        public IControllerFactory PreviousControllerFactory { get; private set; }

        #endregion

        #region Constructor

        public FakeCompositeControllerFactory()
        {
            _controllersByNamespace = new Dictionary<string, Dictionary<string, Lazy<ControllerBase>>>();
            _controllersByNamespace.Add(string.Empty, new Dictionary<string, Lazy<ControllerBase>>());
        }

        #endregion

        #region Exposed Methods

        public void InjectActualControllerFactory(CompositeControllerFactory actualFactory)
        {
            if (_isInited)
            {
                throw new Exception("Can not inject actual factory when this has already been initialized.");
            }

            _actualControllerFactory = actualFactory;
        }

        /// <summary>
        /// Clears all the registered controllers
        /// </summary>
        public void Clear()
        {
            _controllersByNamespace.Clear();

            // This line can die when the Controllers property dies.
            _controllersByNamespace.Add(string.Empty, new Dictionary<string, Lazy<ControllerBase>>());
        }

        /// <summary>
        /// Registers a controller that only works when a route allows a specific namespace. 
        /// This namespace can be any string for this factory. Use in conjunction with
        /// ActActLikeDefaultControllerFactory = true.
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="controllerName"></param>
        /// <param name="controller"></param>
        public void RegisterControllerForNamespace(string nameSpace, string controllerName, ControllerBase controller)
        {
            RegisterControllerForNamespaceLazily(nameSpace, controllerName, new Lazy<ControllerBase>(() => controller));
        }

        /// <summary>
        /// Registers a controller that only works when a route allows a specific namespace. 
        /// This namespace can be any string for this factory. Use in conjunction with
        /// ActActLikeDefaultControllerFactory = true. Also use this for lazy instantiating
        /// of controllers(which means use this in MvcApplicationTester so dependency injection
        /// doesn't cause everything to go slow when all the controllers for an app get created).
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="controllerName"></param>
        /// <param name="controller"></param>
        public void RegisterControllerForNamespaceLazily(string nameSpace, string controllerName,
            Lazy<ControllerBase> controller)
        {
            if (!_controllersByNamespace.ContainsKey(nameSpace))
            {
                _controllersByNamespace.Add(nameSpace, new Dictionary<string, Lazy<ControllerBase>>());
            }

            _controllersByNamespace[nameSpace][controllerName] = controller;
        }

        public void RegisterController(string controllerName, ControllerBase controller)
        {
            // We need one registration for when "UseDefaultNamespace" comes up in the RouteData.DataTokens
            // which will then look for string.Empty I think. Who knows.
            // Also we need one with the actual namespace.

            var lazyController = new Lazy<ControllerBase>(() => controller);
            RegisterControllerForNamespaceLazily(string.Empty, controllerName, lazyController);
            RegisterControllerForNamespaceLazily(controller.GetType().Namespace, controllerName, lazyController);
        }

        public void RegisterController(ControllerBase controller)
        {
            var cName = controller.GetType().Name;
            if (cName.EndsWith("Controller"))
            {
                cName = cName.Replace("Controller", "");
            }

            RegisterController(cName, controller);
        }

        /// <summary>
        /// Sets ControllerBuilder.Current's ControllerFactory with this instance.
        /// Call Reset() to return the previous ControllerFactory to ControllerBuilder.Current.
        /// </summary>
        public void Init()
        {
            if (!_isInited)
            {
                _controllerBuilder = ControllerBuilder.Current;
                PreviousControllerFactory = _controllerBuilder.GetControllerFactory();
                _controllerBuilder.SetControllerFactory(_actualControllerFactory);
                //_actualControllerFactory.Factories.Add(this);
                _actualControllerFactory.Factories.Insert(0, this);

                foreach (var factory in _actualControllerFactory.Factories.OfType<DefaultControllerFactory>())
                {
                    var mockBM = FakeBuildManager.CreateMock();
                    _controllerFactoryBuildManagerProperty.SetValue(factory, mockBM, null);
                }

                _isInited = true;
            }
        }

        public void Dispose()
        {
            if (_isInited)
            {
                _controllerBuilder.SetControllerFactory(PreviousControllerFactory);
                _controllerBuilder = null;
                _actualControllerFactory.Factories.Remove(this);
                foreach (var factory in _actualControllerFactory.Factories.OfType<DefaultControllerFactory>())
                {
                    // Need to kill the BuildManagerWrapper instance. The getter on the property
                    // will create a new one if it needs to.
                    _controllerFactoryBuildManagerProperty.SetValue(factory, null, null);
                }

                _isInited = false;
            }
        }

        private Dictionary<string, Lazy<ControllerBase>> GetRegistrationDictionary(RequestContext requestContext)
        {
            if (!ActLikeDefaultControllerFactory)
            {
                return _controllersByNamespace[string.Empty];
            }

            // MVC does a lot more trickery here, but basically, if there's a route that only allows
            // for specific namespaces, then MVC will return the controller type if it exists within
            // those namespaces. If there's another DataToken of UseNamespaceFallback = true(or possibly
            // anything at all except false, it only checks if the not-strongly-typed-object returned does not equal false)
            // then it will try to use the Default route to find a controller. 
            //
            // This is setup to ignore the UseNamespaceFallback part at the moment. 

            var namespaces = (IEnumerable<string>)requestContext.RouteData.DataTokens["namespaces"];
            if (namespaces != null)
            {
                foreach (var ns in namespaces)
                {
                    if (_controllersByNamespace.ContainsKey(ns))
                    {
                        return _controllersByNamespace[ns];
                    }
                }

                throw new Exception("Unable to create controller for namespace");
            }
            else
            {
                return _controllersByNamespace[string.Empty];
            }
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            var controllerReg = GetRegistrationDictionary(requestContext);
            if (controllerReg.ContainsKey(controllerName))
            {
                return controllerReg[controllerName].Value;
            }

            // To simulate what the default ControllerFactory does
            if (!controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
            {
                return CreateController(requestContext, controllerName + "Controller");
            }

            throw new Exception(
                string.Format("FakeControllerFactory doesn't have a controller registered for the name '{0}'.",
                    controllerName));
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            // To mimic what DefaultControllerFactory does, sorta, throw if the controller doesn't exist, otherwise return default behavior.
            // We'll wanna change this if CreateController ever does anything more crafty than what it does now.
            CreateController(requestContext, controllerName);
            return SessionStateBehavior.Default;
        }

        public void ReleaseController(IController controller)
        {
            if (controller is IDisposable)
            {
                ((IDisposable)controller).Dispose();
            }
        }

        public bool CanHandleController(RequestContext requestContext, string controllerName)
        {
            var controllerReg = GetRegistrationDictionary(requestContext);
            if (controllerReg.ContainsKey(controllerName))
            {
                return true;
            }

            // To simulate what the default ControllerFactory does
            if (!controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
            {
                var fullControllerName = controllerName + "Controller";
                return CanHandleController(requestContext, fullControllerName);
            }

            return false;
        }

        public Type TryGetControllerType(RequestContext requestContext, string controllerName)
        {
            var controllerReg = GetRegistrationDictionary(requestContext);
            if (controllerReg.ContainsKey(controllerName))
            {
                return controllerReg[controllerName].GetType();
            }

            // To simulate what the default ControllerFactory does
            if (!controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
            {
                var fullControllerName = controllerName + "Controller";
                return TryGetControllerType(requestContext, fullControllerName);
            }

            return null;
        }

        #endregion
    }
}
