using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.ControllerFactories;
using MMSINC.Metadata;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities.StructureMap;
using StructureMap;

namespace MMSINC.Testing
{
    public class FakeMvcApplicationTester : MvcApplicationTester<FakeMvcApplication>
    {
        public FakeMvcApplicationTester(IContainer container, bool initializeApplicationInstance = true) : base(
            container, initializeApplicationInstance) { }

        protected override void InitializeControllerFactory(CompositeControllerFactory actual,
            FakeCompositeControllerFactory factory)
        {
            base.InitializeControllerFactory(actual, factory);

            factory.RegisterController("Crud", new FakeCrudController());
        }
    }

    /// <summary>
    /// Sooner or later, this will become a class that will be able to test and simulate 
    /// the whole life of an MvcApplication. Requests, routes, all that jazz. 
    /// 
    /// ENSURE THIS GETS DISPOSED!
    /// </summary>
    /// <typeparam name="TApp">This should be the type of HttpApplication in the site's Global.asax.cs file.</typeparam>
    /// <remarks>
    /// 
    /// It'd be neat if this could somehow interact with BuildManager and have it compile
    /// views, but I'm not sure that's possible. However, what could maybe be possible,
    /// is using IIS Hostable Web Core and host an in-process web server for tests which
    /// would be pretty spiffy. That's outside the current goal for this class though.
    /// 
    /// TODO: Merge the MvcAssert methods that test for validation to work with this.
    /// </remarks>
    public class MvcApplicationTester<TApp> : IDisposable where TApp : MvcApplication, new()
    {
        #region Fields

        private bool _isDisposed,
                     _isInitialized;

        private readonly MvcApplicationAssemblyCache _typeCache =
            MvcApplicationAssemblyCache.GetAssemblyCacheForType(typeof(TApp));

        private readonly StaticPropertyReplacer<RouteTable, RouteCollection> _routeTableReplacer;
        private readonly FakeFilterProviders _filterProviders;
        private readonly StaticPropertyReplacer<ModelValidatorProviderCollection> _modelValidatorProvidersReplacer;
        private readonly StaticPropertyReplacer<ModelBinderProviderCollection> _modelBinderProviderReplacer;
        private readonly StaticPropertyReplacer<ModelBinderDictionary> _modelBindersReplacer;
        private readonly FakeModelMetadataProviders _modelMetadataProviderReplacer;
        private readonly StaticPropertyReplacer<ValueProviderFactoryCollection> _valueProviderFactoryReplacer;
        private readonly StaticPropertyReplacer<ViewEngineCollection> _viewEngineCollectionReplacer;
        private readonly List<IStaticPropertyReplacer> _disposableReplacers = new List<IStaticPropertyReplacer>();
        private ViewEngineBase _viewEngine;
        private readonly StaticPropertyReplacer<ModelFormatterProvider> _formatterProviderReplacer;
        protected readonly IContainer _container;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the instance being used by this tester.
        /// </summary>
        public TApp ApplicationInstance { get; private set; }

        public FakeBuildManager BuildManager { get; private set; }
        public FakeCompositeControllerFactory ControllerFactory { get; private set; }

        public FakeFilterProviders Filters
        {
            get { return _filterProviders; }
        }

        public ModelBinderProviderCollection ModelBinderProviders
        {
            get { return _modelBinderProviderReplacer.ReplacementInstance; }
        }

        public ModelBinderDictionary ModelBinders
        {
            get { return _modelBindersReplacer.ReplacementInstance; }
        }

        public ModelFormatterProvider ModelFormatterProvider
        {
            get { return _formatterProviderReplacer.ReplacementInstance; }
        }

        public ModelMetadataProvider ModelMetadataProvider
        {
            get { return _modelMetadataProviderReplacer.ReplacementInstance; }
        }

        public ModelValidatorProviderCollection ValidatorProviders
        {
            get { return _modelValidatorProvidersReplacer.ReplacementInstance; }
        }

        public ValueProviderFactoryCollection ValueProviderFactories
        {
            get { return _valueProviderFactoryReplacer.ReplacementInstance; }
        }

        public ViewEngineCollection ViewEngines
        {
            get { return _viewEngineCollectionReplacer.ReplacementInstance; }
        }

        /// <summary>
        /// Can be set by tests at any time to override the default FakeViewEngine.
        /// FilesystemViewEngineBase can be overridden in your test project to search against
        /// the codebase on-disk for views.
        /// </summary>
        public ViewEngineBase ViewEngine
        {
            get => _viewEngine;
            set
            {
                _viewEngineCollectionReplacer.ReplacementInstance.Clear();
                _viewEngineCollectionReplacer.ReplacementInstance.Add(_viewEngine = value);
            }
        }

        /// <summary>
        /// Gets the RouteCollection used for everything this tester does. If you have something
        /// that's using RouteTable.Routes then you're doing something wrong.
        /// </summary>
        public RouteCollection Routes
        {
            get { return _routeTableReplacer.ReplacementInstance; }
        }

        #endregion

        #region Constructor

        /// <param name="initializeApplicationInstance">Set to true if the ApplicationInstance should have all of its initializers called in the constructor.</param>
        public MvcApplicationTester(IContainer container, bool initializeApplicationInstance = true)
        {
            _container = container;
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));

            ApplicationInstance = _container.GetInstance<TApp>();

            _routeTableReplacer = new StaticPropertyReplacer<RouteTable, RouteCollection>("_instance");
            _disposableReplacers.Add(_routeTableReplacer);

            _filterProviders = new FakeFilterProviders();
            _disposableReplacers.Add(_filterProviders);

            _modelBinderProviderReplacer =
                new StaticPropertyReplacer<ModelBinderProviderCollection>(typeof(ModelBinderProviders),
                    "_binderProviders");
            _disposableReplacers.Add(_modelBinderProviderReplacer);

            _modelBindersReplacer = new StaticPropertyReplacer<ModelBinderDictionary>(typeof(ModelBinders), "_binders");
            _disposableReplacers.Add(_modelBindersReplacer);

            _modelMetadataProviderReplacer = new FakeModelMetadataProviders();
            _disposableReplacers.Add(_modelMetadataProviderReplacer);

            _modelValidatorProvidersReplacer =
                new StaticPropertyReplacer<ModelValidatorProviderCollection>(typeof(ModelValidatorProviders),
                    "_providers");
            _disposableReplacers.Add(_modelValidatorProvidersReplacer);

            _valueProviderFactoryReplacer =
                new StaticPropertyReplacer<ValueProviderFactoryCollection>(typeof(ValueProviderFactories),
                    "_factories");
            _disposableReplacers.Add(_valueProviderFactoryReplacer);

            _viewEngineCollectionReplacer =
                new StaticPropertyReplacer<ViewEngineCollection>(typeof(ViewEngines), "_engines");
            ViewEngine = new FakeViewEngine();
            _disposableReplacers.Add(_viewEngineCollectionReplacer);

            ControllerFactory = new FakeCompositeControllerFactory();
            _disposableReplacers.Add(ControllerFactory);

            _formatterProviderReplacer =
                new StaticPropertyReplacer<ModelFormatterProvider>(typeof(ModelFormatterProviders), "Current");
            _disposableReplacers.Add(_formatterProviderReplacer);

            if (initializeApplicationInstance)
            {
                Init();
            }
        }

        #endregion

        #region Private Methods

        private void DisposeStaticReplacers()
        {
            foreach (var replacer in _disposableReplacers)
            {
                replacer.Dispose();
            }
        }

        protected virtual void InitializeControllerFactory(CompositeControllerFactory actualCompositeControllerFactory,
            FakeCompositeControllerFactory factory)
        {
            factory.InjectActualControllerFactory(actualCompositeControllerFactory);

            // TODO: Figure out how to get ControllerFactory to play nice
            // with ObjectFactory. It works fine during testing but not 
            // when the site is actually running.

            // TODO: Have FakeControllerFactory either use ObjectFactory
            //       to get controller instances or let it take a Func<>
            //       object to simulate the creation anyway.
            //       Cause this won't work with any parameterless constructor
            //       or things like SecureController
            foreach (var controllerType in _typeCache.ControllerTypes)
            {
                var name = (controllerType.Name.EndsWith("Controller")
                    ? controllerType.Name.Replace("Controller", "")
                    : controllerType.Name);

                // We want to lazily initialize the controller here because otherwise ObjectFactory will be creating
                // any dependency injected constructor objects too. Having this be lazy shaves off a couple minutes of
                // total test run time.
                var lazyInstance =
                    new Lazy<ControllerBase>(() => (ControllerBase)_container.GetInstance(controllerType));
                factory.RegisterControllerForNamespaceLazily(controllerType.Namespace, name, lazyInstance);

                // We also need to register for "default" namespaces
                if (controllerType.Namespace != null)
                {
                    factory.RegisterControllerForNamespaceLazily(string.Empty, name, lazyInstance);
                }
            }
        }

        private void InitializeAreas()
        {
            // Retrieves all the AreaRegistration types that are in the same assembly
            // as the application. This will blow up if the area is in a different assembly
            // for whatever reason.

            var areaRegistrations =
                typeof(TApp).Assembly.GetTypes().Where(x => typeof(AreaRegistration).IsAssignableFrom(x));

            foreach (var arType in areaRegistrations)
            {
                if (arType == typeof(FakeAreaRegistration))
                {
                    continue;
                }

                var ar = (AreaRegistration)Activator.CreateInstance(arType);
                var context = new AreaRegistrationContext(ar.AreaName, Routes);
                ar.RegisterArea(context);
            }
        }

        #endregion

        #region Public Methods

        #region Init/Cleanup

        /// <summary>
        /// Set any properties needed on the MvcApplication instance. Called
        /// before Init() is called.
        /// </summary>
        /// <param name="instance"></param>
        protected virtual void InitializeApplicationInstance(TApp instance)
        { /* noop */
        }

        /// <summary>
        /// Calls all the initialization methods on the ApplicationInstance. 
        /// Call this AFTER configuring anything needed on the ApplicationInstance.
        /// Call this BEFORE adding extra test routes/areas/whatever that are test-specific.
        /// </summary>
        public void Init()
        {
            if (_isInitialized)
            {
                throw new Exception("Can not initialize MvcApplicationTester twice.");
            }

            InitializeApplicationInstance(ApplicationInstance);

            try
            {
                // Call all methods that initialize actual ApplicationInstance stuff first.
                ApplicationInstance.RegisterGlobalFilters(Filters.GlobalFilters, _container);

                // Areas need to be initialized prior to other routes so the route order is correct.
                InitializeAreas();
                ApplicationInstance.RegisterRoutes(Routes);
                // TODO: Initialize Container
                var ccf = (CompositeControllerFactory)ApplicationInstance.CreateControllerFactory(_container);
                InitializeControllerFactory(ccf, ControllerFactory);
                ApplicationInstance.RegisterValidatorProviders(ValidatorProviders, _container);

                _modelMetadataProviderReplacer.ReplacementInstance =
                    ApplicationInstance.CreateModelMetadataProvider() ?? new EmptyModelMetadataProvider();
                ApplicationInstance.RegisterMetadata(ModelMetadataProvider);
                ApplicationInstance.RegisterModelFormatters(ModelFormatterProvider);
                ApplicationInstance.RegisterModelBinderProviders(ModelBinderProviders);
                ApplicationInstance.RegisterModelBinders(_container, ModelBinders);
                ApplicationInstance.RegisterValueProviderFactories(ValueProviderFactories, _container);
                // Then have the static replacers get initialized. 
                foreach (var initer in _disposableReplacers)
                {
                    initer.Init();
                }
            }
            catch (Exception)
            {
                // Need to ensure we clean up changes to static fields/props on common classes
                // if we end up erroring out.
                Dispose();
                throw;
            }
            finally
            {
                _isInitialized = true;
            }
        }

        /// <summary>
        /// Cleans up all the static property replacements. MUST CALL
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            try
            {
                if (ApplicationInstance != null)
                {
                    ApplicationInstance.Dispose();
                }

                DisposeStaticReplacers();
            }
            finally
            {
                _isDisposed = true;
            }
        }

        #endregion

        /// <summary>
        /// Creates an Area and registers some default routes for it.
        /// </summary>
        /// <param name="areaName"></param>
        public void RegisterArea(string areaName)
        {
            RegisterArea(areaName, reg => reg.MapRoute(areaName + "Default",
                areaName + "/{controller}/{action}/{id}",
                new {
                    id = UrlParameter.Optional,
                    namespaces = new[] {areaName + "Namespace"}
                }));
        }

        public void RegisterArea(string areaName, Action<AreaRegistrationContext> registration)
        {
            var areaReg = new FakeAreaRegistration();
            areaReg.RegisterAreaAction = registration;
            areaReg.SetAreaName(areaName);
            var routes = new RouteCollection();
            areaReg.CreateContextAndRegister(routes);

            // Reverse so we can insert them in the order they were created.
            foreach (var route in routes.Reverse())
            {
                Routes.Insert(0, route);
            }
        }

        #region CreateRequestHandler

        public FakeMvcHttpHandler CreateRequestHandler()
        {
            // ReSharper disable RedundantArgumentDefaultValue
            return new FakeMvcHttpHandler(_container, routes: Routes);
            // ReSharper restore RedundantArgumentDefaultValue
        }

        /// <summary>
        /// Initializes this FakeMvcHandler instance to use a specific url. Use this when using the empty constructor.
        /// STRONGLY RECOMMENDED TO USE NAMED PARAMETERS CAUSE THERE'S A LOT OF STRINGS
        /// </summary>
        /// <param name="hostName">www.domain.com</param>
        /// <param name="appPath">/somesubdir/</param>
        /// <param name="requestPath">~/some/controller/action</param>
        /// <param name="httpMethod">GET/POST</param>
        /// <param name="urlProtocol">HTTP/HTTPS</param>
        /// <param name="port">Wine</param>
        /// <param name="throwIfRouteDataIsMissing">Set this to false if you're gonna test calling Process/ProcessRequest on a route that will not exist.</param>
        public FakeMvcHttpHandler CreateRequestHandler(string requestPath, string hostName = "localhost",
            string appPath = "/",
            string httpMethod = "GET", string urlProtocol = "HTTP", int port = -1,
            bool throwIfRouteDataIsMissing = true)
        {
            requestPath.ThrowIfNullOrWhiteSpace("requestPath");
            // ReSharper disable RedundantArgumentName
            return new FakeMvcHttpHandler(_container, hostName: hostName,
                appPath: appPath,
                requestPath: requestPath,
                httpMethod: httpMethod,
                urlProtocol: urlProtocol,
                port: port,
                throwIfRouteDataIsMissing: throwIfRouteDataIsMissing,
                routes: Routes);
            // ReSharper restore RedundantArgumentName
        }

        #endregion

        #endregion
    }

    internal class MvcApplicationAssemblyCache
    {
        #region Fields

        // ReSharper disable RedundantNameQualifier
        private static readonly Type _controllerType = typeof(System.Web.Mvc.IController),
                                     _httpApplicationType = typeof(System.Web.HttpApplication);
        // ReSharper restore RedundantNameQualifier

        private static readonly Dictionary<Assembly, MvcApplicationAssemblyCache> _assemblyCache =
            new Dictionary<Assembly, MvcApplicationAssemblyCache>();

        #endregion

        #region Properties

        public Assembly PrimaryAssembly { get; private set; }
        public IEnumerable<Assembly> ReferencedAssemblies { get; private set; }
        public IEnumerable<Type> ControllerTypes { get; private set; }

        #endregion

        #region Constructor

        private MvcApplicationAssemblyCache(Assembly assemblyOfMvcApplicationType)
        {
            PrimaryAssembly = assemblyOfMvcApplicationType;
            var allAssemblies = new HashSet<Assembly>();
            allAssemblies.Add(
                PrimaryAssembly); // Add the testing assembly since it'll have controllers in it most likely.
            foreach (var refAss in PrimaryAssembly.GetReferencedAssemblies())
            {
                var name = refAss.FullName;
                // No reason to waste time scanning all of the referenced .NET library.
                if (!name.StartsWith("System") && !name.StartsWith("mscorlib"))
                {
                    // Console.WriteLine("Loading assembly: {0}", refAss.FullName);
                    allAssemblies.Add(Assembly.Load(refAss));
                }
            }

            ControllerTypes = allAssemblies
                             .SelectMany(GetTypesInAssembly)
                             .Where(IsValidControllerType).ToArray();
        }

        #endregion

        #region Private Methods

        private static Type[] GetTypesInAssembly(Assembly ass)
        {
            try
            {
                return ass.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // This exception likes to get thrown a lot.
                if (!ex.LoaderExceptions.Any())
                {
                    throw;
                }

                var err =
                    string.Format(
                        "Unable to load all types from assembly {0} because one or more types is causing an issue. " +
                        "Check the inner exception's LoaderExceptions for more details. " +
                        "Here's the first one: {1}", ass.FullName, ex.LoaderExceptions.First().Message);
                throw new Exception(err, ex);
            }
        }

        private static bool IsValidControllerType(Type type)
        {
            return _controllerType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface &&
                   !type.IsGenericTypeDefinition;
        }

        private static void ThrowIfNotHttpApplication(Type type)
        {
            if (!_httpApplicationType.IsAssignableFrom(type))
            {
                throw new InvalidOperationException(
                    "The type used with MvcApplicationAssemblyCache must inherit from HttpApplication.");
            }
        }

        #endregion

        #region Public Methods

        public static MvcApplicationAssemblyCache GetAssemblyCacheForType(Type mvcAppType)
        {
            ThrowIfNotHttpApplication(mvcAppType);
            var ass = mvcAppType.Assembly;
            lock (_assemblyCache)
            {
                if (!_assemblyCache.ContainsKey(ass))
                {
                    _assemblyCache.Add(ass, new MvcApplicationAssemblyCache(ass));
                }
            }

            return _assemblyCache[ass];
        }

        #endregion
    }
}
