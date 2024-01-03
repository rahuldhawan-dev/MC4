using System.Security.Principal;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MapCall.Common.Model.Entities.Users;
using StructureMap;

namespace MapCallMVC.Configuration
{
    public class AuthenticationService : AuthenticationServiceBase<User, AuthenticationLog>
    {
        public AuthenticationService(IContainer container, IPrincipal principal, IAuthenticationCookieFactory cookieFactory) : base(container, principal, cookieFactory) { }
    }
}