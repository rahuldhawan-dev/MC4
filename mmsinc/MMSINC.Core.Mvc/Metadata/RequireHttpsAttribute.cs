using System;
using System.Web.Mvc;

namespace MMSINC.Metadata
{
    public class MyRequireHttpsAttribute : RequireHttpsAttribute
    {
        #region Constants

        public const string LOAD_BALANCER_HEADER = "Upgrade-Insecure-Requests";

        #endregion

        #region Exposed Methods

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            if (filterContext.HttpContext.Request.IsSecureConnection)
            {
                return;
            }

            if (string.Equals(filterContext.HttpContext.Request.Headers[LOAD_BALANCER_HEADER], "1"))
            {
                return;
            }

            if (filterContext.HttpContext.Request.IsLocal)
            {
                return;
            }

            HandleNonHttpsRequest(filterContext);
        }

        #endregion
    }
}
