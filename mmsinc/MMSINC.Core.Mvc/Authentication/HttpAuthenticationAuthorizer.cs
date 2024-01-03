using System;
using System.Text;
using System.Web.Mvc;
using StructureMap;

namespace MMSINC.Authentication
{
    /// <summary>
    /// Authorizer that checks that a user has provided http basic authorization headers
    /// in order to log in.
    /// </summary>
    public class HttpAuthenticationAuthorizer : MvcAuthorizer
    {
        #region Properties

        protected IMembershipHelper Membership => _container.GetInstance<IMembershipHelper>();

        #endregion

        #region Constructors

        public HttpAuthenticationAuthorizer(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Authorize(AuthorizationArgs authArgs)
        {
            var context = authArgs.Context;
            var req = context.HttpContext.Request;
            var auth = req.Headers["Authorization"];
            if (!string.IsNullOrEmpty(auth))
            {
                var cred = Encoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6)))
                                   .Split(':');
                var user = new {Name = cred[0], Pass = cred[1]};
                if (Membership.ValidateUser(user.Name, user.Pass))
                {
                    var userId = AuthenticationService.GetUserId(user.Name);
                    AuthenticationService.SignIn(userId, true);
                    return;
                }
            }

            context.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            context.HttpContext.Response.AddHeader("WWW-Authenticate", "Basic realm=\"Mapcall\"");
            context.Result = new HttpUnauthorizedResult();
        }

        #endregion
    }
}
