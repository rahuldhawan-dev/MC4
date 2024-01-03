using StructureMap;

namespace MMSINC.Authentication
{
    /// <summary>
    /// Single filter that should be used for dealing with authorization for api access rather than mvc.
    /// Currently supports HTTP Basic authentication.
    /// </summary>
    public class ApiAuthorizationFilter : AuthorizationFilterBase
    {
        #region Constructors

        public ApiAuthorizationFilter(IContainer container)
        {
            // NOTE: The order of these matter! 
            _authorizers.Add(container.GetInstance<AnonymousAuthorizer>());
            _authorizers.Add(container.GetInstance<HttpAuthenticationAuthorizer>());
            _authorizers.Add(container.GetInstance<AdminAuthorizer>());
        }

        #endregion
    }
}
