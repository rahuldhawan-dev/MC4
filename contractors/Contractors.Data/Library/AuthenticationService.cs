using System.Security.Principal;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using StructureMap;

namespace Contractors.Data.Library
{
    public class AuthenticationService : MMSINC.Authentication.AuthenticationServiceBase<ContractorUser, ContractorsAuthenticationLog>
    {
        #region Constructors

        public AuthenticationService(IContainer container, IPrincipal principal, IAuthenticationCookieFactory cookieFactory) : base(container, principal, cookieFactory) { }

        #endregion
    }
}
