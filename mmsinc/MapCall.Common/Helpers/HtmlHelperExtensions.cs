using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;

namespace MapCall.Common.Helpers.ClassExtensions
{
    public static class HtmlHelperExtensions
    {
        public static bool CurrentUserCanDo(this HtmlHelper helper, User currentUser, RoleModules module,
            RoleActions action = RoleActions.Read, OperatingCenter opCenter = null)
        {
            return
                currentUser.GetCachedMatchingRoles(module, action, opCenter)
                           .CanAccessRole;
        }
    }
}
