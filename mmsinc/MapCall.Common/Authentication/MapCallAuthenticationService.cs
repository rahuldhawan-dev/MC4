using System.Security.Principal;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using StructureMap;

namespace MapCall.Common.Authentication
{
    public class MapCallAuthenticationService : AuthenticationServiceBase<User, AuthenticationLog>
    {
        #region Constructors

        public MapCallAuthenticationService(IContainer container, IPrincipal principal,
            IAuthenticationCookieFactory cookieFactory) : base(container, principal, cookieFactory) { }

        #endregion
    }
}
