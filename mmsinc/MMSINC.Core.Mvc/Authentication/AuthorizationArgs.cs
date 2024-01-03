using System.Web.Mvc;

namespace MMSINC.Authentication
{
    public class AuthorizationArgs
    {
        #region Properties

        /// <summary>
        /// Gets the authorization context instance being used on an authorization filter.
        /// </summary>
        public AuthorizationContext Context { get; private set; }

        /// <summary>
        /// This says that authorization should be skipped entirely. This should only
        /// ever be set to true because anonymous access is allowed. Default value is false.
        /// 
        /// NOTE: This is specifically needed for output caching. The server is allowed
        /// to cache things when SkipAuthorizingEntirely is true!
        /// </summary>
        public bool SkipAuthorizingEntirely { get; set; }

        /// <summary>
        /// This tells the MvcAuthorizationFilter to skip running any
        /// additional authorizers after the current one completes. The
        /// only thing that should set this to true is the AdminAuthorizer.
        /// </summary>
        public bool SkipAdditionalAuthorizing { get; set; }

        #endregion

        #region Constructor

        public AuthorizationArgs(AuthorizationContext context)
        {
            Context = context;
            SkipAuthorizingEntirely = false;
        }

        #endregion
    }
}
