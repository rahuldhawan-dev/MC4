using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DataAnnotationsExtensions.ClientValidation;
using MMSINC.Authentication;
using MMSINC.Common;
using MMSINC.Configuration;
using MMSINC.ControllerFactories;
using MMSINC.Data.NHibernate;
using MMSINC.Helpers;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.StructureMap;
using MMSINC.Validation;
using MMSINC.ClassExtensions.StringExtensions;
using NHibernate;
using StructureMap;
using StructureMap.Web.Pipeline;

namespace MMSINC
{
    public abstract class MvcApplication : HttpApplicationBase
    {
        #region Constants

        public const string DEFAULT_CONNECTION_STRING = "Main";

        #endregion

        #region Private Members

        private static DatabaseConfiguration _dbConfig;

        #endregion

        #region Properties

        public static StructureMapServiceLocator ServiceLocator { get; protected set; }

        // This is used by _BaseLayout.cshtml for page titles. 
        public static string ApplicationDisplayName => ConfigurationManager.AppSettings["ApplicationDisplayName"];

        public static bool IsInTestMode { get; set; }

        /// <summary>
        /// When true, the application will allow all requests to be processed 
        /// while IsInTestMode == true. This is for regression testing only. This
        /// won't do anyhing at all if the TestRequestProcessingFilter is not included
        /// in the filters list. 
        /// </summary>
        public static bool AllowTestRequests { get; set; }

#if DEBUG

        private static readonly HashSet<string> _regressionTestFlags = new HashSet<string>();

        /// <summary>
        /// A collection of flags that can be set during regression tests. ANY CALL TO THIS MUST BE WRAPPED
        /// WITH IF DEBUG STATEMENTS! 
        /// statements.
        /// </summary>
        /// <remarks>
        /// 
        /// This is for regression testing only. There are no thread safety guarantees for accessing this
        /// so production code/release builds should never touch this. This is being doubly enforced by
        /// requiring that you check IsInTestMode prior to using the RegressionTestFlags property so that
        /// RegressionTestFlag logic doesn't creep into unit tests.
        /// 
        /// </remarks>
        public static HashSet<string> RegressionTestFlags
        {
            get
            {
                if (!IsInTestMode)
                {
                    throw new InvalidOperationException(
                        "You should not be attempting to access RegressionTestFlags when IsInTestMode is false.");
                }

                return _regressionTestFlags;
            }
        }

#endif

        #endregion

        #region Private Methods

        private void InitializeBundles(BundleCollection bundles, EmbeddedVirtualPathProvider embeddedPathProvider)
        {
            // If this is false, all the embedded scripts fail. Yay!
            // Need a handler still for these files when in debug mode. 
            BundleTable.EnableOptimizations = true;
            HostingEnvironment.RegisterVirtualPathProvider(embeddedPathProvider);

            // This FileSetOrderList is some stupid thing that automatically sets the order of
            // your included files. That means if you have file names that somehow fit into 
            // the internal "pattern", then MVC will change the order of the files around, causing
            // stylesheets and scripts to be loaded in the wrong order. I have no idea why they 
            // thought that was a good idea.
            bundles.FileSetOrderList.Clear();
            RegisterBundles(bundles, embeddedPathProvider);
        }

        private void InitializeRoutes(RouteCollection routes)
        {
            // When finding a controller to match a route, MVC defaults to first looking in the
            // namespaces supplied to the route and then looks outside of those namespaces if it
            // can't find a controller. This is dumb and allows for all area controllers to be 
            // accessed without the area in the url. To fix this, each route has to explicitly have
            // the UseNamespaceFallback data token added and set to false. This makes MVC *only* look
            // in the specified namespaces for a matching controller. Note that routes registered
            // with AreaRegistration automatically have this set to false. 
            const string USE_NAMESPACE_FALLBACK = "UseNamespaceFallback";
            RegisterRoutes(routes);
            foreach (var r in routes.OfType<Route>())
            {
                // MVC has two different private classes called IgnoreRouteInternal
                // that have null DataToken dictionaries. They don't need to be set 
                // as far as I can tell. -Ross 3/26/2015

                if (r.DataTokens != null && !r.DataTokens.ContainsKey(USE_NAMESPACE_FALLBACK))
                {
                    r.DataTokens[USE_NAMESPACE_FALLBACK] = false;
                }
            }
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Intended to be overridden in inheriting classes for any additional
        /// code that needs to run in Application_BeginRequest.
        /// </summary>
        public virtual void OnApplication_BeginRequest() { }

        /// <summary>
        /// Intended to be overridden in inheriting classes for any aditional
        /// code that needs to run in Application_EndRequest.
        /// </summary>
        public virtual void OnApplication_EndRequest() { }

        /// <summary>
        /// Override if the default ModelMetadataProvider should be replaced with something else.
        /// </summary>
        public virtual ModelMetadataProvider CreateModelMetadataProvider()
        {
            return new CustomModelMetadataProvider();
        }

        /// <summary>
        /// Override to register any specific Meta
        /// </summary>
        public virtual void RegisterMetadata(ModelMetadataProvider metaDataProvider)
        {
            // noop
        }

        public virtual void RegisterModelFormatters(ModelFormatterProvider provider)
        {
            // This is a backwards compatability thing with the old Boolean display template.
            // The template, for whatever reason, was defaulting to using a Yes/No format.
            var boolDisplayFormat = new BoolFormatAttribute("Yes", "No", "n/a");
            provider.RegisterFormatter(typeof(bool), boolDisplayFormat);
            provider.RegisterFormatter(typeof(bool?), boolDisplayFormat);

            var boolEditorFormat = new BoolFormatAttribute("Yes", "No", ControlHelper.DEFAULT_EMPTY_TEXT_FOR_DROPDOWNS);
            provider.RegisterEditorFormatter(typeof(bool?), boolEditorFormat);
        }

        public virtual void RegisterValidatorProviders(ModelValidatorProviderCollection providers, IContainer container)
        {
            // These are added by default when you call ModelBinderProviders.Providers, but they're
            // being cleared out during unit testing somewhere when they shouldn't be.
            if (!providers.OfType<DataAnnotationsModelValidatorProvider>().Any())
            {
                providers.Add(new DataAnnotationsModelValidatorProvider());
            }

            if (!providers.OfType<ClientDataTypeModelValidatorProvider>().Any())
            {
                providers.Add(new ClientDataTypeModelValidatorProvider());
            }

            providers.Add(new DateTimeModelValidatorProvider());
            providers.Add(new FileUploadValidatorProvider());
            providers.Add(new ClientSideIntegerValidatorProvider());

            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAddressAttribute),
                typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(NumericStringAttribute),
                typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(typeof(EntityMustExistAttribute),
                (metadata, context, attribute) => new EntityMustExistAttributeAdapter(metadata, context,
                    attribute as EntityMustExistAttribute, container));
            DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(typeof(RequiredWhenAttribute),
                (metadata, context, attribute) => new RequiredWhenAttributeAdapter(metadata, context,
                    attribute as RequiredWhenAttribute, container));
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();
        }

        public virtual void RegisterModelBinderProviders(ModelBinderProviderCollection providers)
        {
            providers.Add(new EnumModelBinderProvider());
        }

        public virtual void RegisterModelBinders(IContainer container, ModelBinderDictionary binders)
        {
            binders.DefaultBinder = container.GetInstance<StructureMapModelBinder>();
            // This is so all of our MVC apps trim the strings for any string model property during binding.
            binders.Add(typeof(string), new StringModelBinder());
            binders.Add(typeof(List<string>), new StringListModelBinder());
            binders.Add(typeof(int[]), new Int32ArrayModelBinder());
        }

        public virtual void RegisterBundles(BundleCollection bundles, EmbeddedVirtualPathProvider embeddedPathProvider)
        {
            // noop
        }

        public virtual void RegisterValueProviderFactories(ValueProviderFactoryCollection factoryProviderCollection,
            IContainer container)
        {
            // noop
        }

        public virtual IControllerFactory CreateControllerFactory(IContainer container)
        {
            var factory = new CompositeControllerFactory();
            factory.Factories.Add(container.GetInstance<DefaultCompositableControllerFactory>());
            return factory;
        }

        #endregion

        #region Abstract Methods

        public abstract IContainer InitializeContainer();
        public abstract void RegisterGlobalFilters(GlobalFilterCollection filters, IContainer container);
        public abstract void RegisterRoutes(RouteCollection routes);

        #endregion

        #region Application Events

        protected override void OnApplication_Start()
        {
            base.OnApplication_Start();
            AreaRegistration.RegisterAllAreas();
            var container = InitializeContainer();
            RegisterGlobalFilters(GlobalFilters.Filters, container);
            InitializeRoutes(RouteTable.Routes);
            ServiceLocator = new StructureMapServiceLocator(container);

            ControllerBuilder.Current.SetControllerFactory(CreateControllerFactory(container));
            // Need to override the default with our own implementation.
            RegisterValidatorProviders(ModelValidatorProviders.Providers, container);
            ModelMetadataProviders.Current = CreateModelMetadataProvider();
            RegisterMetadata(ModelMetadataProviders.Current);
            RegisterModelFormatters(ModelFormatterProviders.Current);
            RegisterModelBinderProviders(ModelBinderProviders.BinderProviders);
            RegisterModelBinders(container, ModelBinders.Binders);
            RegisterValueProviderFactories(ValueProviderFactories.Factories, container);
            InitializeBundles(BundleTable.Bundles, new EmbeddedVirtualPathProvider());
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        protected void Application_BeginRequest()
        {
            OnApplication_BeginRequest();
        }

        protected void Application_EndRequest()
        {
            if (!IsInTestMode)
            {
                HttpContextLifecycle.DisposeAndClearAll();
            }
            else
            {
                // So this is really a hack. We don't want the session to get flushed/cleared
                // when the browser starts sending off requests for stylesheets/images/static
                // content. There's not a lot that can be done to do this 100% effectively
                // without making a huge mess of production code(introducing new httphandlers
                // and junk). 
                //
                // This is a bandaid though.
                if (Request.RequestContext.RouteData.Route != null
                    && !Request.RawUrl.Contains("content", StringComparison.InvariantCultureIgnoreCase)
                    && !Request.RawUrl.Contains("scripts", StringComparison.InvariantCultureIgnoreCase)
                    && !Request.RawUrl.Contains("resource", StringComparison.InvariantCultureIgnoreCase))
                {
                    var session = (ISession)ServiceLocator.GetService(typeof(ISession));

                    if (session.IsOpen)
                    {
                        session.Flush();
                        session.Clear();
                    }
                }
            }

            // If one of our error filters has caught and handled something,
            // then Context.Error should be null. Otherwise, we're getting a
            // 404 from deep in the bowels of ASP or IIS that isn't going
            // through the MVC pipeline all the way and we need to handle it.
            //
            // We're essentially doing what the httpErrors section of web.config
            // does but correctly. 
            //
            // This dumb solution found at http://stackoverflow.com/a/9026907/152168
            if (Context.Response.StatusCode == 404 && Context.Error != null)
            {
                // We need to do a Response.Clear to remove the default 404 html junk.
                Response.Clear();
                var rd = new RouteData();
                rd.Values["controller"] = "Error";
                rd.Values["action"] = "NotFound";

                // If this throws an exception, we're now screwed and have a blank screen 
                // without any useful information at all. Also, this rewrapping of the
                // the HttpContext with a new RequestContext isn't very useful because
                // HttpContext.Current.Request.RequestContext will still refer to the old one.
                HandleUnhandledError404s(new RequestContext(new System.Web.HttpContextWrapper(Context), rd),
                    (IContainer)ServiceLocator.GetService(typeof(IContainer)));
            }

            OnApplication_EndRequest();
        }

        /// <summary>
        /// Override for when a custom error needs to be displayed that isn't able to be handled
        /// by a global filter. ie: returning 404s for missing controllers/actions.
        /// </summary>
        /// <param name="requestContext"></param>
        protected virtual void HandleUnhandledError404s(RequestContext requestContext, IContainer container)
        {
            // noop.
        }

        #endregion
    }

    public abstract class MvcApplication<TAssemblyOf, TUser> : MvcApplication
        where TUser : class, IAdministratedUser
    {
        #region Properties

        public IContainer Container { get; protected set; }
        public abstract DependencyRegistrar<TAssemblyOf, TUser> DependencyRegistrar { get; }

        #endregion

        #region Exposed Methods

        public sealed override IContainer InitializeContainer()
        {
            Container = DependencyRegistrar.EnsureDependenciesRegistered();
            DependencyResolver
               .SetResolver(new StructureMapDependencyResolver(Container));
            return Container;
        }

        #endregion
    }
}
