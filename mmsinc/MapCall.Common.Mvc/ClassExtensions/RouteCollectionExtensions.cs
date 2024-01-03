using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.ClassExtensions.StringExtensions;

// ReSharper disable CheckNamespace
namespace MapCall.Common.ClassExtensions.RouteCollectionExtensions
    // ReSharper restore CheckNamespace
{
    public static class RouteCollectionExtensions
    {
        #region Constants

        public static readonly string[] DEFAULT_ROUTE_NAMES = new[] {
            "Default", "default"
        };

        public const string DEFAULT_ROUTE_ALREADY_EXISTS =
                                "The resources route must be added before the Default route.  Please move this route registration above the Default route registration in the Application class.",
                            ROUTE_NAME = "Resources",
                            ROUTE_URL = "Content/{action}/{file}";

        public const string ROUTE_CONTROLLER = ROUTE_NAME;

        #endregion

        #region Extension Methods

        public static Route AddResourcesRoute(this RouteCollection routes, string routeName = null)
        {
            routeName = routeName ?? ROUTE_NAME;
            if (DEFAULT_ROUTE_NAMES.Any(n => routes[n] != null))
            {
                throw new ConfigurationErrorsException(DEFAULT_ROUTE_ALREADY_EXISTS);
            }

            return routes.MapRoute(routeName, ROUTE_URL,
                new {controller = ROUTE_CONTROLLER});
        }

        #endregion
    }
}
