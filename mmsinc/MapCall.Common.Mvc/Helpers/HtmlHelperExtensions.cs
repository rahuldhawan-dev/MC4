using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;

namespace MapCall.Common.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static bool CurrentUserCanEdit(this HtmlHelper that, IRoleService roleService, User currentUser,
            IThingWithOperatingCenter model, RequestContext context = null)
        {
            context = context ?? HttpContext.Current.Request.RequestContext;

            var area = context.RouteData.Values["Area"];
            var requiredRoles = roleService
               .GetRequiredRolesForRoute(context, "Edit",
                    context.RouteData.Values["Controller"].ToString(),
                    area == null ? null : area.ToString());

            return currentUser.IsAdmin ||
                   requiredRoles.All(
                       role => currentUser.GetCachedMatchingRoles(role.Module, role.Action, model.OperatingCenter)
                                          .CanAccessRole);
        }

        public static bool CurrentUserCanDestroy(this HtmlHelper that, IRoleService roleService, User currentUser,
            IThingWithOperatingCenter model, RequestContext context = null)
        {
            context = context ?? HttpContext.Current.Request.RequestContext;

            var area = context.RouteData.Values["Area"];
            var requiredRoles = roleService
               .GetRequiredRolesForRoute(context, "Destroy",
                    context.RouteData.Values["Controller"].ToString(),
                    area == null ? null : area.ToString());

            return currentUser.IsAdmin ||
                   requiredRoles.All(
                       role => currentUser.GetCachedMatchingRoles(role.Module, role.Action, model.OperatingCenter)
                                          .CanAccessRole);
        }
    }
}
