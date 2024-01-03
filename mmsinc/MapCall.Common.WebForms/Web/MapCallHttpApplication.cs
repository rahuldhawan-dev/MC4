using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Common;
using MapCall.Common.Configuration;
using MapCall.Common.Utility;
using MMSINC.Utilities.StructureMap;
using StructureMap;

namespace MapCall.Common.Web
{
    /// <summary>
    /// Replacement for Global.asax
    /// </summary>
    public abstract class MapCallHttpApplication : HttpApplicationBase
    {
        #region Consts

        internal const string CONFIGURATION_RESOURCE_NAME = "MapCall.Common.Resources.resourceconfig.xml";

        #endregion

        #region Fields

        private IResourceConfiguration _siteConfiguration;
        private IResourceManager _resourceManager;
        protected IContainer _container;

        #endregion

        #region Properties

        public static bool IsVisualStudioDevServer
        {
            get
            {
#if DEBUG
                var requestUrl = GetCurrentRequestUrl();
                return requestUrl != null && requestUrl.Authority.StartsWith("localhost:",
                    StringComparison.OrdinalIgnoreCase);
#endif
                return false;
            }
        }

        public static bool IsLocalhostMapCall
        {
            get
            {
#if DEBUG
                var requestUrl = GetCurrentRequestUrl();
                return requestUrl != null && requestUrl.AbsolutePath.StartsWith("/mapcall/",
                    StringComparison.OrdinalIgnoreCase);
#endif
                return false;
            }
        }

        public static bool IsStagingServer
        {
            get
            {
                var requestUrl = GetCurrentRequestUrl();
                return requestUrl != null && requestUrl.Authority.Contains("mapcall.info",
                    StringComparison.OrdinalIgnoreCase);
            }
        }

        public static MapCallHttpApplication Instance { get; private set; }

        public IResourceConfiguration SiteConfiguration
        {
            get
            {
                if (_siteConfiguration == null)
                {
                    _siteConfiguration = GetResourceConfiguration();
                    _siteConfiguration.ConfigurationResourceName = CONFIGURATION_RESOURCE_NAME;
                }

                return _siteConfiguration;
            }
        }

        public IResourceManager ResourceManager
        {
            get
            {
                if (_resourceManager == null)
                {
                    _resourceManager = CreateResourceManager();
                    _resourceManager.InitializeConfiguration(SiteConfiguration);
                }

                return _resourceManager;
            }
        }

        #endregion

        #region Constructor

        public MapCallHttpApplication()
        {
            Instance = this;
        }

        #endregion

        #region Public Methods

        private bool CurrentRequestAllowsAnonymousAccess()
        {
            var context = DependencyResolver.Current.GetService<HttpContextBase>();

            // SkipAuthorization will be set to true by FormsAuthorization in
            // certain cases, like if a url is for WebResources.axd or if the
            // url is for the login page.
            if (context.SkipAuthorization)
            {
                return true;
            }

            // UrlAuthorizationModule is what determines if a page can be accessed
            // based on the authorization sections of web.config and a specified user.
            var anonUser = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            if (UrlAuthorizationModule.CheckUrlAccessForPrincipal(context.Request.Path, anonUser,
                context.Request.HttpMethod))
            {
                return true;
            }

            return false;
        }

        // NOTE: Don't move this to a further base class that WebForms and MVC share. MVC
        //       does not need this because the AuthorizationFilters do a better job at everything.
        // This is one of those auto-wired events for ASP.
        // ReSharper disable once UnusedMember.Local
        public void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
#if DEBUG
            // Why is this here and why is this in a DEBUG check? Because otherwise
            // you can never log in to 271 when running locally. Logging into MapCall
            // creates an AuthenticationLog in the mapcall database, then 271 can't authenticate
            // because it tries to find the same AuthenticationLog in the mapcall_workorders
            // database.
            if (GetResourceConfiguration().Site == Utility.Site.WorkOrders)
            {
                return;
            }
#endif

            // This needs to happen in PostAuthenticateRequest because it's at this point that
            // HttpContext.Current.User has an object.

            // Don't do authorization processing on things that allow anonymous access. We don't need it
            // running on pages that load a lot of images, especially since the response for images
            // should be getting cached anyway.
            if (!CurrentRequestAllowsAnonymousAccess())
            {
                if (!DependencyResolver.Current.GetService<IAuthenticationService>().CurrentUserIsAuthenticated)
                {
                    LogOutUser();
                }
            }
        }

        /// <summary>
        /// Logs a user out, marks their auth cookie as logged out so it can no longer be used.
        /// If redirectToLoginPage is true it will redirect the login page truthfully.
        /// </summary>~
        /// <param name="redirectToLoginPage"></param>
        /// <param name="includeReturnUrl">Set to true if the current request url should be included 
        /// as a return url</param>
        public virtual void LogOutUser(bool redirectToLoginPage = true, bool includeReturnUrl = true)
        {
            DependencyResolver.Current.GetService<IAuthenticationService>().SignOut();

            // TODO: Move the .NET Session kill to SignOut method.
            var cur = HttpContext.Current;
            if (cur.Session != null)
            {
                cur.Session.Abandon();
                cur.Session.Clear();
            }
        }

        protected override void OnApplication_Start()
        {
            base.OnApplication_Start();

            _container = CommonDependencies.Register(RegisterDependencies);
            ResourceHandler.SetResourceManager(ResourceManager);
            // VPP registration needs to be done before any requests occur. 
            GetHostingEnvironment().RegisterVirtualPathProvider(CreateResourceVirtualPathProvider());
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }

        public static Uri GetCurrentRequestUrl()
        {
            // sometimes "the request isn't ready yet", so we just return null
            try
            {
                return HttpContext.Current.Request.Url;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Override to create a site-specific ResourceConfiguration.
        /// </summary>
        /// <returns></returns>
        public abstract IResourceConfiguration GetResourceConfiguration();

        /// <summary>
        /// Override to register site-specific dependencies
        /// </summary>
        /// <param name="i"></param>
        public virtual void RegisterDependencies(ConfigurationExpression i) { }

        public virtual IResourceManager CreateResourceManager()
        {
            return new ResourceManager();
        }

        public virtual ResourceVirtualPathProvider CreateResourceVirtualPathProvider()
        {
            var rvpp = new ResourceVirtualPathProvider(this.ResourceManager);
            return rvpp;
        }

        public virtual IHostingEnvironment GetHostingEnvironment()
        {
            return new HostingEnvironmentWrapper();
        }

        #endregion
    }
}
