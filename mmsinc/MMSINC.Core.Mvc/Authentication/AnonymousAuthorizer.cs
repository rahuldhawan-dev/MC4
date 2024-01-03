using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace MMSINC.Authentication
{
    public sealed class AnonymousAuthorizer : MvcAuthorizer
    {
        public override void Authorize(AuthorizationArgs authArgs)
        {
            if (GetAttributes<AllowAnonymousAttribute>(authArgs.Context).Any())
            {
                // No need for the other filters to run.
                authArgs.SkipAuthorizingEntirely = true;
            }
        }

        public AnonymousAuthorizer(IContainer container) : base(container) { }
    }
}
