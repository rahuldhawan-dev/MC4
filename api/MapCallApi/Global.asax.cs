using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities.Users;
using MapCallApi.Configuration;
using MMSINC.Authentication;
using MMSINC.Configuration;
using MMSINC.ErrorHandlers;
using StructureMap;
using DependencyRegistrar = MapCallApi.Configuration.DependencyRegistrar;

namespace MapCallApi
{
    public class MvcApplication : MapCallMvcApplication<User, User>
    {
        #region Private Members

        #region Fields

        private DependencyRegistrar _dependencyRegistrar;

        #endregion

        #endregion

        #region Properties

        public override DependencyRegistrar<User, User> DependencyRegistrar
        {
            get
            {
                return _dependencyRegistrar ??
                    (_dependencyRegistrar = new DependencyRegistrar());
            }
        }

        #endregion

        #region Exposed Methods

        protected override void OnApplication_Start()
        {
            base.OnApplication_Start();
            log4net.Config.XmlConfigurator.Configure();
        }
        
        public override void RegisterGlobalFilters(GlobalFilterCollection filters, IContainer container)
        {
            if (IsProduction || IsStaging)
            {
                //filters.Add(new RequireHttpsAttribute());
                filters.Add(container.GetInstance<ErrorEmailer>());
            }

            var auth = container.GetInstance<ApiAuthorizationFilter>();
            // This should place RoleAuthorizor after the normal anon/authenticated/admin checks
            var role = container.GetInstance<RoleAuthorizer>();
            role.ReturnErrorsAsViews = false;
            auth.Authorizors.Add(role);
            auth.Authorizors.Add(container.GetInstance<UserMustHaveProfileAuthorizer>());
            
            filters.Add(auth);
            filters.Add(container.GetInstance<TrySkipIisCustomErrorsFilter>());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AjaxErrorHandlerAttribute {
                ShowExplicitErrorMessages = !IsProduction
            });
        }

        public override void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("proxies/{resource}.ashx/{*pathInfo}");

            DoHorribleHackToRegisterMvcAttributeRoutes(routes);
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new [] {"MapCallApi.Controllers"}
            );
        }

        /// <summary>
        /// This exists because calling routes.MapMvcAttributeRoutes() by itself does not work
        /// with our unit test system. Internally, it attempts to find an instance of DefaultControllerFactory
        /// in order to enumerate all of the controller types. It can't find that because we swap that out
        /// as part of MvcApplication. So it then creates an instance of DFC and tries to enumerate the controller
        /// types, but an error is thrown from BuildManager about trying to access things during the pre-init stage.
        ///
        /// This method bypasses some of what MapMvcAttributeRoutes does internally and jumps straight to
        /// one of the internal methods that accepts a pre-defined list of controller types. This is a huge hack
        /// and it's likely to break during an upgrade. I also don't know what impact it may have elsewhere so we
        /// probably shouldn't use this anywhere outside of this project.
        /// </summary>
        /// <param name="routes"></param>
        private void DoHorribleHackToRegisterMvcAttributeRoutes(RouteCollection routes)
        {
            if (!IsInTestMode)
            {
                routes.MapMvcAttributeRoutes();
                return;
            }

            var controllerTypes = (from type in this.GetType().Assembly.GetExportedTypes()
                                   where
                                       type != null && type.IsPublic
                                                    && type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                                                    && !type.IsAbstract && typeof(IController).IsAssignableFrom(type)
                                   select type).ToList();

            var attributeRoutingAssembly = typeof(RouteCollectionAttributeRoutingExtensions).Assembly;
            var attributeRoutingMapperType =
                attributeRoutingAssembly.GetType("System.Web.Mvc.Routing.AttributeRoutingMapper");

            var mapAttributeRoutesMethod = attributeRoutingMapperType.GetMethod(
                "MapAttributeRoutes",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(RouteCollection), typeof(IEnumerable<Type>) },
                null);

            mapAttributeRoutesMethod.Invoke(null, new object[] { routes, controllerTypes });
        }

        #endregion
    }
}
