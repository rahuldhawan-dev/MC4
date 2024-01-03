using System;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Customers;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Utility;
using MapCall.Common.Utility.Notifications;
using MapCall.Common.Utility.Permissions;
using MapCall.Common.Web;
using MMSINC.Authentication.OAuth2;
using MMSINC.Data.NHibernate;
using MMSINC.Interface;
using MMSINC.Utilities;
using MMSINC.Utilities.Permissions;
using RazorEngine.Templating;
using StructureMap;
using Role = MapCall.Common.Model.Entities.Role;

namespace MapCall
{
    public class Global : MapCallHttpApplication
    {
        #region Private Methods

        void Application_EndRequest(Object sender, EventArgs e)
        {
            // clean up after StructureMap
            //ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();

            // THIS IS ABSOLUTELY NEEDED FOR THAT VIEWONE GARBAGE TO WORK
            // AND PROBABLY SOME OTHER JAVA STUFF.
            if (Response.Cookies.Count > 0)
            {
                // NOTE: leave this dirty, dirty loop in place.  It needs to happen this way, or
                // the auth ticket will get nulled
                foreach (string s in Response.Cookies.AllKeys)
                {
                    if (s == FormsAuthentication.FormsCookieName)
                    {
                        Response.Cookies[FormsAuthentication.FormsCookieName].HttpOnly = false;
                    }
                }
            }
        }

        protected override void OnApplication_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            var log = log4net.LogManager.GetLogger("MapCall Application");

            log.Info("MapCall application starting");

            RouteTable.Routes.MapPageRoute(
                "authorization-callback",
                "authorization-code/callback",
                "~/OAuth2Consume.aspx");

            base.OnApplication_Start();
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Logs a user out, marks their auth cookie as logged out so it can no longer be used.
        /// If redirectToLoginPage is true it will redirect the login page truthfully.
        /// </summary>
        /// <param name="redirectToLoginPage"></param>
        public override void LogOutUser(bool redirectToLoginPage = true, bool includeReturnUrl = true)
        {
            base.LogOutUser(redirectToLoginPage);

            if (redirectToLoginPage)
            {
                var url = "~/login.aspx";
                if (includeReturnUrl)
                {
                    url = url + "?ReturnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl);
                }

                HttpContext.Current.Response.Redirect(url);
            }
        }

        public override IResourceConfiguration GetResourceConfiguration()
        {
            return new ResourceConfiguration {
                Site = Common.Utility.Site.MapCall
            };
        }

        public override void RegisterDependencies(ConfigurationExpression i)
        {
            RegisterModel(i);
            RegisterUtilities(i);
        }

        private void RegisterUtilities(ConfigurationExpression i)
        {
            i.For<IDateTimeProvider>().Use<DateTimeProvider>();
            i.For<IRoleManager>().Use(() => RoleManager.Current);
            i.For<IHttpContext>().Use(ctx =>
                new MMSINC.Common.HttpContextWrapper(ctx.GetInstance<IContainer>(), HttpContext.Current));
            i.For<IServer>().Use(ctx => ctx.GetInstance<IHttpContext>().Server);
            i.For<INotificationService>().Use<NotificationService>();
            i.For<INotifier>().Use<RazorNotifier>();
            i.For<ITemplateService>().Use(_ => new TemplateService()).Singleton();
            i.For<IPermissionsObjectFactory>().Use<PermissionsObjectFactory>();
            i.For<IHttpClientFactory>().Use<HttpClientFactory>();
            i.For<OAuth2Config>().Use(_ => OAuth2Config.Load());
            i.For<IOAuth2AuthenticationHelper>().Use<OAuth2AuthenticationHelper>().Singleton();
            i.For<IOAuth2TokenValidator>().Use<OAuth2TokenValidator>();
        }

        private void RegisterModel(ConfigurationExpression i)
        {
            i.For<INotificationConfigurationRepository>().Use<NotificationConfigurationRepository>();
            i.For<IH2OSurveyRepository>().Use<H2OSurveyRepository>();
            i.For<IUserRepository>().Use<UserRepository>();
            i.For<IRepository<User>>().Use(ctx => ctx.GetInstance<IUserRepository>());
            i.For<IRepository<Employee>>().Use<EmployeeRepository>();
            i.For<IGISLayerUpdateRepository>().Use<GISLayerUpdateRepository>();
            i.For<IRepository<AggregateRole>>().Use<RepositoryBase<AggregateRole>>();
        }

        #endregion
    }
}