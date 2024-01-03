using MMSINC.Authentication;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities.Users;
using StructureMap;

namespace MapCall.Common.Metadata
{
    public class RequiresUserAdminAuthorizer : MvcAuthorizer
    {
        public override void Authorize(AuthorizationArgs authArgs)
        {
            if (GetAttributes<RequiresUserAdminAttribute>(authArgs.Context).Any())
            {
                var authServ = (IAuthenticationService<User>)AuthenticationService;
                
                if (!authServ.CurrentUser.IsAdmin && !authServ.CurrentUser.IsUserAdmin)
                {
                    authArgs.Context.Result = new RedirectResult(MMSINC.ClassExtensions.ControllerExtensions.Urls.FORBIDDEN);
                }
            }
        }

        public RequiresUserAdminAuthorizer(IContainer container) : base(container) { }
    }
}
