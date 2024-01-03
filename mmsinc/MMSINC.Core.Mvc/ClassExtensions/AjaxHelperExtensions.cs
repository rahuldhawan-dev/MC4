using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using JetBrains.Annotations;
using MMSINC.Authentication;
using MMSINC.Helpers;
using MMSINC.Metadata;

namespace MMSINC.ClassExtensions
{
    public static class AjaxHelperExtensions
    {
        #region SecureActionLink

        public static MvcHtmlString SecureActionLink<TUser>(this AjaxHelper helper,
            ISecureFormTokenService tokenService, TUser currentUser, string linkText, [AspMvcAction] string actionName,
            [AspMvcController] string controllerName, object routeValues = null, AjaxOptions ajaxOptions = null,
            object htmlAttributes = null)
            where TUser : IAdministratedUser
        {
            var routeValuesLocal = routeValues as IDictionary<string, object> ?? new RouteValueDictionary(routeValues);
            var htmlAttributesLocal = htmlAttributes as IDictionary<string, object> ??
                                      new RouteValueDictionary(htmlAttributes);

            routeValuesLocal.Add("controller", controllerName);

            var token = tokenService.CreateTokenWithRouteValues(actionName, controllerName, currentUser.Id,
                routeValuesLocal);

            routeValuesLocal.Add(FormBuilder.SECURE_FORM_HIDDEN_FIELD_NAME, token);
            return helper.ActionLink(linkText, actionName,
                new RouteValueDictionary(routeValuesLocal), ajaxOptions,
                htmlAttributesLocal);
        }

        #endregion
    }
}
