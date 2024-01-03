using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using StructureMap;

namespace Contractors.Helpers
{
    public static class LinkHelper
    {
        #region Constants

        public const string LIST_ITEM_FORMAT = "<li>{0}</li>";

        #endregion

        #region Private Members

        internal static bool CurrentUserIsAdmin
        {
            get
            {
                return DependencyResolver.Current.GetService<IAuthenticationService<ContractorUser>>().CurrentUserIsAdmin;
            }
        }

        #endregion

        public static MvcHtmlString AdminActionMenuItem(this HtmlHelper helper, string linkText, string actionName, string controllerName)
        {
            return CurrentUserIsAdmin
                       ? new MvcHtmlString(String.Format(LIST_ITEM_FORMAT, helper.ActionLink(linkText, actionName, controllerName)))
                       : new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString AdminActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName)
        {
            return CurrentUserIsAdmin
                    ? helper.ActionLink(linkText, actionName, controllerName)
                    : new MvcHtmlString(string.Empty);
        }
    }
}
