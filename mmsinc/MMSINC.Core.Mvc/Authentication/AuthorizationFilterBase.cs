using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MMSINC.Authentication
{
    public abstract class AuthorizationFilterBase : IAuthorizationFilter
    {
        #region Fields

        protected readonly List<MvcAuthorizer> _authorizers = new List<MvcAuthorizer>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of authorizers used when performing authorization. Authorizers
        /// are ran in the order they appear in the list until one fails.
        /// </summary>
        public IList<MvcAuthorizer> Authorizors
        {
            get { return _authorizers; }
        }

        /// <summary>
        /// For unit testing only.
        /// </summary>
        internal bool LastAuthorizationPassed { get; set; }

        #endregion

        #region Private Methods

        private void EnsureNotChildAction(AuthorizationContext filterContext)
        {
            // The following code + comment are from 
            // https://github.com/ASP-NET-MVC/aspnetwebstack/blob/master/src/System.Web.Mvc/AuthorizeAttribute.cs
            // - Ross 10/13/2014
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                // If a child action cache block is active, we need to fail immediately, even if authorization
                // would have succeeded. The reason is that there's no way to hook a callback to rerun
                // authorization before the fragment is served from the cache, so we can't guarantee that this
                // filter will be re-run on subsequent requests.
                throw new InvalidOperationException("Cannot use within a child action cache.");
            }
        }

        #endregion

        #region Public Methods

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            // Some of this needs to mimic how AuthorizeAttribute works, because it does some
            // extra stuff in relation to output caching if a user is authorized.

            LastAuthorizationPassed = false;

            EnsureNotChildAction(filterContext);

            var authArgs = new AuthorizationArgs(filterContext);
            foreach (var auth in Authorizors)
            {
                if (auth.IsEnabled)
                {
                    auth.Authorize(authArgs);
                    if (authArgs.SkipAuthorizingEntirely)
                    {
                        // This is essentially saying "Yes, this is an anonymous resource"
                        // and the output caching can cache to the server.
                        LastAuthorizationPassed = true;
                        return;
                    }
                    else
                    {
                        // We want to prevent any server caching of stuff that requires
                        // authorization. Server output cache doesn't know anything about
                        // users changing and what not, so a different user could potentially
                        // be given a cache of the wrong thing.
                        filterContext.HttpContext.Response.Cache.SetNoServerCaching();
                    }

                    if (authArgs.SkipAdditionalAuthorizing && filterContext.Result == null)
                    {
                        // The rest of the method needs to run, unlike the other spots that return early.
                        break;
                    }

                    if (filterContext.Result != null)
                    {
                        // The authorizer has failed at authorizing, so don't run any more.
                        LastAuthorizationPassed = false;
                        return; // Return early because the code at the bottom should not be ran
                    }
                }
            }

            LastAuthorizationPassed = true;
        }

        #endregion
    }
}
