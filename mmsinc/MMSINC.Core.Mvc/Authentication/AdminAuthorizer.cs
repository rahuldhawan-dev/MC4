using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace MMSINC.Authentication
{
    public class AdminAuthorizer : MvcAuthorizer
    {
        public override void Authorize(AuthorizationArgs authArgs)
        {
            if (GetAttributes<RequiresAdminAttribute>(authArgs.Context).Any())
            {
                // We can skip any other authorizers when the RequiresAdminAttribute exists.
                authArgs.SkipAdditionalAuthorizing = true;

                if (!AuthenticationService.CurrentUserIsAdmin)
                {
                    authArgs.Context.Result = new RedirectResult(ClassExtensions.ControllerExtensions.Urls.FORBIDDEN);
                }
            }
        }

        public AdminAuthorizer(IContainer container) : base(container) { }
    }
}
