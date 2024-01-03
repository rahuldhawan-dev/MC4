using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.ClassExtensions.StringExtensions;

namespace MMSINC.ClassExtensions
{
    public enum ManyToManyRouteAction
    {
        Add,
        Remove,
        Index
    }

    public static class RouteCollectionExtensions
    {
        #region Extension Methods

        /// <summary>
        /// Returns RouteData for an arbitrary Uri.
        /// </summary>
        public static RouteData GetRouteData(this RouteCollection routeCollection, Uri uri)
        {
            using (var sw = new StringWriter())
            {
                var httpContext = new HttpContext(new HttpRequest(null, uri.GetAbsoluteUriWithoutQuery(), uri.Query),
                    new HttpResponse(sw));
                return routeCollection.GetRouteData(new HttpContextWrapper(httpContext));
            }
        }

        public static string GetManyToManyRouteName(ManyToManyRouteAction action, string parentModel, string childModel)
        {
            return String.Format("{0}{1}{2}", action, parentModel, childModel);
        }

        public static RouteCollection AddManyToManyRoutes(this RouteCollection routes, string parentModel,
            string childModel, string[] nameSpaces, bool add = true, bool delete = true, string area = "",
            bool index = true)
        {
            if (add)
            {
                routes.AddChildishRoute(ManyToManyRouteAction.Add, parentModel, childModel, nameSpaces, area);
            }

            if (delete)
            {
                routes.AddChildishRoute(ManyToManyRouteAction.Remove, parentModel, childModel, nameSpaces, area);
            }

            if (index)
            {
                // index doesn't have an id
                routes.AddChildishRouteWithoutModelId(ManyToManyRouteAction.Index, parentModel, childModel, nameSpaces,
                    area);
            }

            return routes;
        }

        private static void AddChildishRoute(this RouteCollection routes, ManyToManyRouteAction action,
            string parentModel, string childModel, string[] nameSpaces, string area)
        {
            var childIdParam = String.Format("{0}Id", childModel.ToCamelCase());

            var defaults =
                new RouteValueDictionary(
                    new {controller = parentModel, action = String.Format("{0}{1}", action, childModel)});

            defaults.Add(childIdParam, UrlParameter.Optional);

            var routeName = GetManyToManyRouteName(action, parentModel, childModel);
            var route = routes.MapRoute(routeName,
                String.Format("{0}{1}/{{id}}/{2}/{3}/{{{4}}}",
                    (!String.IsNullOrEmpty(area)) ? area + "/" : string.Empty,
                    parentModel, childModel.Pluralize(), action, childIdParam),
                defaults, nameSpaces);

            if (!String.IsNullOrWhiteSpace(area))
            {
                route.DataTokens["area"] = area;
            }
        }

        private static void AddChildishRouteWithoutModelId(this RouteCollection routes, ManyToManyRouteAction action,
            string parentModel, string childModel, string[] nameSpaces, string area)
        {
            var defaults =
                new RouteValueDictionary(
                    new {controller = parentModel, action = childModel.Pluralize()});

            var routeName = GetManyToManyRouteName(action, parentModel, childModel);
            var route = routes.MapRoute(routeName,
                String.Format("{0}{1}/{{id}}/{2}", (!String.IsNullOrEmpty(area)) ? area + "/" : string.Empty,
                    parentModel, childModel.Pluralize()),
                defaults, nameSpaces);

            if (!String.IsNullOrWhiteSpace(area))
            {
                route.DataTokens["area"] = area;
            }
        }

        #endregion
    }
}
