using System.Security.Principal;
using System.Web.Mvc;
using MMSINC.Authentication;

namespace MapCall.Common.ClassExtensions
{
    public static class UserExtensions
    {
        #region Fields

        //public static readonly IEnumerable<string> ADMIN_USER_NAMES
        //    = new HashSet<string>(new[] {
        //        "kevinkirwan",
        //        "dougthorn",
        //        "keanek",
        //        "mcadmin"
        //    }, StringComparer.OrdinalIgnoreCase);

        #endregion

        #region Extension Methods

        public static bool IsAdmin(this IPrincipal user)
        {
            return DependencyResolver.Current.GetService<IAuthenticationService>().CurrentUserIsAdmin;

            //return (ADMIN_USER_NAMES.Contains(user.Identity.Name));
        }

        #endregion
    }
}
