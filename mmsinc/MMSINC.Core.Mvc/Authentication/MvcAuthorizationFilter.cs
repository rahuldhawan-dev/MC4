using StructureMap;

namespace MMSINC.Authentication
{
    /// <summary>
    /// The single filter that should be used for dealing with authorization.
    /// </summary>
    public class MvcAuthorizationFilter : AuthorizationFilterBase
    {
        #region Constructor

        public MvcAuthorizationFilter(IContainer container)
        {
            // NOTE: The order of these matter! 
            _authorizers.Add(container.GetInstance<AnonymousAuthorizer>());
            _authorizers.Add(container.GetInstance<FormsAuthenticationAuthorizer>());
            _authorizers.Add(container.GetInstance<AdminAuthorizer>());
        }

        #endregion
    }
}
