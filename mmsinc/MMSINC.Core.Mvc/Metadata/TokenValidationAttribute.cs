using System;
using System.Web.Mvc;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Exists so that LogonAuthorizeAttribute can allow for controller actions
    /// that use Token Validation instead of logins and passwords.  The actual
    /// validation functionality will need to happen in an inherited class in
    /// the site project.
    /// 
    /// NOTE: This is not an AuthorizeAttribute because it works with AuthenticationAuthorizer.
    /// Do not make this an AuthorizeAttribute or else it will get executed twice.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class TokenValidationAttribute : Attribute
    {
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            // noop
        }
    }
}
