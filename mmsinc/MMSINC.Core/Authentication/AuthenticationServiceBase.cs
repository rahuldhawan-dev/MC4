using System;
using System.Security.Principal;
using System.Web;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Exceptions;
using MMSINC.Utilities;
using StructureMap;

// TODO: TokenAuthentication is kinda shoved in here where it doesn't belong. 
//       It'd be nice if the MvcAuthorizers could be used to actually create the AuthenticationService
//       instance, but then validation wouldn't work for non-MVC projects. -Ross 7/22/2015 

// TODO: TokenAuthentication also does not work with AuthenticationLog stuff because there
//       isn't a cookie involved. A cookie gets set but it's not persisted between API requests.
//       -Ross 7/22/2015

namespace MMSINC.Authentication
{
    public abstract class AuthenticationServiceBase
    {
        #region Exposed Static Methods

        /// <summary>
        /// Throws an AuthenticationException if the given string is not a
        /// valid email address.
        /// </summary>
        public static void ThrowIfInvalidEmail(string email)
        {
            if (!email.IsValidEmail())
            {
                throw AuthenticationException.InvalidOrBadlyFormattedEmail(email);
            }
        }

        #endregion
    }

    /// <summary>
    /// Service capable of validating user email addresses and passwords.
    /// </summary>
    public abstract class AuthenticationServiceBase<TEntity, TAuthenticationLog> : AuthenticationServiceBase,
        IAuthenticationService<TEntity>
        where TEntity : class, IAdministratedUser
        where TAuthenticationLog : class, IAuthenticationLog<TEntity>, new()
    {
        #region Constants

        // All sites originally used 120 minutes, which was an MTV show, so let's keep that going.
        private static readonly TimeSpan _expirationTimeOut = TimeSpan.FromHours(2);

        #endregion

        #region Private Members

        private TEntity _currentUser;

        /// <summary>
        /// To explain this variable even further: This is the value that's set on
        /// HttpContext.Current.User.IsAuthenticated by FormsAuthentication. This
        /// happens before AuthenticationService ever gets called.
        /// </summary>
        private bool _principalIsAuthenticatedByFormsAuthentication;

        private readonly IAuthenticationCookieFactory _cookieFactory;
        private readonly IContainer _container;
        private bool? _cookieIsAuthenticated;
        private bool _isTokenAuthenticated;

        #endregion

        #region Properties

        private IAuthenticationRepository<TEntity> Repository =>
            _container.GetInstance<IAuthenticationRepository<TEntity>>();

        private IAuthenticationLogRepository<TAuthenticationLog, TEntity> LogRepository =>
            _container.GetInstance<IAuthenticationLogRepository<TAuthenticationLog, TEntity>>();

        /// <summary>
        /// Gets/sets the wrapper used for dealing with FormsAuthentication.
        /// </summary>
        internal IFormsAuthenticator FormsAuthenticator { get; set; }

        /// <summary>
        /// Represents the data in the parsed Identity.Name value. It is not validated! 
        /// THIS CAN NOT BE NULL.
        /// </summary>
        protected IAuthenticationCookie AuthenticationCookie { get; set; }

        private bool AuthenticationCookieIsValid
        {
            get
            {
                // IsValidlyFormatted is true if the Id is not-null and greater
                // than 0, and if the UserName is not-null and a valid email
                // address(by default). It does not determine whether the
                // values are actually valid in the database.
                if (!AuthenticationCookie.IsValidlyFormatted)
                {
                    return false;
                }

                // We need to cache this to prevent repeated database calls during a single request.
                // CurrentUserIsAuthenticated is used in a lot of places, this would potentially
                // result in hundreds of calls(ie RoleService checks CurrentUserIsAuthenticated, and RoleService
                // is called multiple times just by the menu renderer alone)
                if (!_cookieIsAuthenticated.HasValue)
                {
                    _cookieIsAuthenticated = IsCookieValidForRequest();
                }

                return _cookieIsAuthenticated.Value;
            }
        }

        /// <summary>
        /// Boolean value indicating that the current user is authenticated,
        /// and is an administrator by whatever methods are used to denote
        /// rank.
        /// </summary>
        public bool CurrentUserIsAdmin
        {
            get { return CurrentUserIsAuthenticated && CurrentUser.IsAdmin; }
        }

        /// <summary>
        /// Boolean value indicating whether the current user is authenticated
        /// via the forms authentication system.
        /// </summary>
        public bool CurrentUserIsAuthenticated
        {
            get
            {
                // IsAuthenticated will always be false after SignIn is called. 
                if (!_principalIsAuthenticatedByFormsAuthentication)
                {
                    return false;
                }

                // Need to cut out early because token authenticated users will
                // not have cookies.
                if (_isTokenAuthenticated)
                {
                    return true;
                }

                if (!AuthenticationCookieIsValid)
                {
                    return false;
                }

                // ReSharper disable PossibleInvalidOperationException
                // Null check is done by AuthenticationCookie implementation.
                // TODO: Check if this is cached properly by NHibernate.
                var user = Repository.Find(AuthenticationCookie.Id.Value);
                // ReSharper restore PossibleInvalidOperationException
                return
                    user != null &&
                    user.UniqueName.Equals(AuthenticationCookie.UserName,
                        StringComparison.InvariantCultureIgnoreCase) && user.HasAccess;
            }
        }

        /// <summary>
        /// Typed object representing the currently logged in user.
        /// </summary>
        public TEntity CurrentUser
        {
            get
            {
                if (!CurrentUserIsAuthenticated)
                {
                    throw new InvalidOperationException("Current user is not authenticated.");
                }

                return EnsureCurrentUser();
            }
        }

        /// <summary>
        /// Returns a unique identifier for the current user. Throws if current user is not authenticated
        /// </summary>
        public string CurrentUserIdentifier
        {
            get { return CurrentUser.UniqueName; }
        }

        public int CurrentUserId
        {
            get { return CurrentUser.Id; }
        }

        #endregion

        #region Constructors

        protected AuthenticationServiceBase(IContainer container, IPrincipal principal,
            IAuthenticationCookieFactory cookieFactory)
        {
            _container = container;
            _cookieFactory = cookieFactory;
            // The Identity.IsAuthenticated value will never ever change during the life of 
            // a request, including when SignIn is called. This is only useful during initialization.
            _principalIsAuthenticatedByFormsAuthentication = principal?.Identity.IsAuthenticated ?? false;
            FormsAuthenticator = container.GetInstance<FormsAuthenticator>();
            if (principal?.Identity != null)
            {
                AuthenticationCookie = _cookieFactory.CreateCookie(principal.Identity.Name);
            }
        }

        #endregion

        #region Private Methods

        protected TEntity EnsureCurrentUser()
        {
            var ident = AuthenticationCookie.Id.Value;
            if (_currentUser == null || _currentUser.Id != ident)
            {
                SetCurrentUser(Repository.Find(ident), _isTokenAuthenticated);
            }

            return _currentUser;
        }

        /// <summary>
        /// Resets all of the current user and authentication details for a given user. 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isTokenAuthenticated">Set to true if the user was authenticated via TokenValidationAttribute.</param>
        private void SetCurrentUser(TEntity user, bool isTokenAuthenticated)
        {
            if (user != null)
            {
                // Set AuthCookie first so that in case it throws an exception we don't end up
                // with the service instance being in an invalid state.
                AuthenticationCookie = _cookieFactory.CreateCookie(user.Id, user.UniqueName);
                _currentUser = user;
                _principalIsAuthenticatedByFormsAuthentication = true;
                _isTokenAuthenticated = isTokenAuthenticated;
                // NOTE: Don't set _cookieIsAuthenticated when setting the user to a valid user.
                //       It needs to be set later on.
            }
            else
            {
                AuthenticationCookie = _cookieFactory.CreateEmptyCookie();
                _currentUser = null;
                _principalIsAuthenticatedByFormsAuthentication = false;
                _cookieIsAuthenticated = false;
                _isTokenAuthenticated = false;
            }
        }

        private bool IsCookieValidForRequest()
        {
            // If a cookie is sent to the server and the cookie has expired, somewhere
            // before this event is ever fired, the FormsAuthenticationModule *removes*
            // the authentication cookie from the Request.Cookies collection. That is
            // stupid.

            var context = _container.GetInstance<HttpContextBase>();

            // If this is not returning a value when you expect it to, make sure that it's
            // being set in HttpApplicationBase.OnBeginRequest.
            var cookie = FormsAuthenticator.GetCurrentRequestAuthenticationCookie();

            if (string.IsNullOrWhiteSpace(cookie))
            {
                // Do not do a logout call as there's nothing the server can do
                // without the cookie value being there. Also it would just end
                // in a redirect loop.
                return false;
            }

            var log = LogRepository.FindActiveLogByCookie(cookie);

            // Log can be null for a number of reasons:
            //  1. User has pre-existing cookie from before rollout of this code. 
            //  2. User has set their own cookie value somehow.
            //  3. User has a valid cookie, but its LoggedOutAt value is set.
            if (log == null)
            {
                return false;
            }

            // And even though the repository is checking this before
            // returning value, an extra safeguard is okay.
            if (log.LoggedOutAt.HasValue)
            {
                return false;
            }

            // Reasons this could happen:
            // 1. Someone actually stole the cookie
            // 2. User changed networks(might have switched wifi or changed to mobile or something)
            // NOTE: This check is useless on .info because the firewall messes with the request IP address. 
            //       .info will always return ::1, same as localhost on dev machines.
            if (log.IpAddress != context.Request.UserHostAddress)
            {
                return false;
            }

            // User cookie has expired. 
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            if (log.ExpiresAt <= now)
            {
                return false;
            }

            // FormsAuthentication did it this way, so are we. FormsAuth would only update the
            // cookie if the current time was halfway or less towards the expiration time. 
            //
            // TODO: Find a better place to do this. End request may work.
            var halfOfTimeout = TimeSpan.FromMilliseconds(_expirationTimeOut.TotalMilliseconds / 2);
            var halfWayPoint = log.ExpiresAt.Subtract(halfOfTimeout);

            if (halfWayPoint <= now)
            {
                log.ExpiresAt = now.Add(_expirationTimeOut);
                LogRepository.Save(log);
            }

            return true;
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Returns a UserLoginAttemptStatus value indicating whether or not
        /// the given email address and password are valid.
        /// </summary>
        public UserLoginAttemptStatus ValidateUser(string email, string password)
        {
            return Repository.ValidateUser(email, password);
        }

        /// <summary>
        /// Signs in the current request as the specified user through Forms Authentication. Remember to call ValidateUser first!
        /// </summary>
        /// <remarks>
        /// NOTE: There's no specific support here for calling SignIn when a user is already
        ///       authenticated. It just happens to work.
        /// </remarks>
        /// <param name="userId"></param>
        /// <param name="isTokenAuthenticated">Set to true if authentication was done via 
        /// the TokenValidationAttribute. Permits is the only place that uses this.</param>
        public void SignIn(int userId, bool isTokenAuthenticated)
        {
            var user = Repository.Find(userId);
            SetCurrentUser(user, isTokenAuthenticated);
            var cookieValue = FormsAuthenticator.SetAuthCookie(AuthenticationCookie);
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();

            var authLog = new TAuthenticationLog();
            // NOTE: UserHostAddress becomes useless when the server is running behind a 
            //       proxy or NAT or whatever because it will report the IP address of the
            //       router/firewall instead for every single request. ex: .info does this
            authLog.IpAddress = _container.GetInstance<HttpContextBase>().Request.UserHostAddress;
            authLog.User = user;
            authLog.LoggedInAt = now;
            authLog.ExpiresAt = now.Add(_expirationTimeOut);
            // update the User's LastLoggedInAt
            user.LastLoggedInAt = now;
            Repository.Save(user);
            LogRepository.Save(authLog, cookieValue);
        }

        /// <summary>
        /// Marks the current request authentication cookie as invalid so that it can no longer be used. 
        /// </summary>
        public void SignOut()
        {
            // Set the cookie to logged out in the database before anything else. If anything in the future causes
            // the code after that to crash, the cookie will still have been invalidated.
            var encryptedCookie = FormsAuthenticator.GetCurrentRequestAuthenticationCookie();
            // The cookie will be null if there is no cookie, but an empty string if the key is present without a value.
            //if(encryptedCookie != null)
            if (!string.IsNullOrWhiteSpace(encryptedCookie))
            {
                // A log can potentially be null when we release this as none of the current cookies that a user
                // has will exist in the database.
                var log = LogRepository.FindActiveLogByCookie(encryptedCookie);

                // Don't overwrite the LoggedOutAt value if one already exists, that would lead
                // to bad data.
                if (log != null && !log.LoggedOutAt.HasValue)
                {
                    log.LoggedOutAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                    LogRepository.Save(log);
                }
            }

            FormsAuthenticator.SignOut();
            SetCurrentUser(null, false);

            // TODO: Clear session in a non-hack way somehow.

            // Do not redirect! Redirecting is handled by the object that called SignOut. 
        }

        public int GetUserId(string email)
        {
            return Repository.GetUser(email).Id;
        }

        #endregion
    }

    /// <summary>
    /// Service capable of validating user email addresses and passwords.
    /// </summary>
    public interface IAuthenticationService<out TEntity> : IAuthenticationService where TEntity : IAdministratedUser
    {
        #region Abstract Properties

        /// <summary>
        /// Typed object representing the currently logged in user.
        /// </summary>
        TEntity CurrentUser { get; }

        #endregion
    }

    /// <summary>
    /// Service capable of validating user email addresses and passwords.
    /// </summary>
    public interface IAuthenticationService
    {
        #region Abstract Properties

        /// <summary>
        /// Boolean value indicating that the current user is an administrator
        /// by whatever methods are used to denote rank.
        /// </summary>
        bool CurrentUserIsAdmin { get; }

        /// <summary>
        /// Boolean value indicating whether the current user is authenticated
        /// via whatever authentication system is in place.
        /// </summary>
        bool CurrentUserIsAuthenticated { get; }

        /// <summary>
        /// Returns a unique value that represents the current user. So like... their username.
        /// </summary>
        string CurrentUserIdentifier { get; }

        int CurrentUserId { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a UserLoginAttemptStatus value indicating whether or not
        /// the given email address and password are valid.
        /// </summary>
        UserLoginAttemptStatus ValidateUser(string email, string password);

        /// <summary>
        /// Sign the current user in using their userId.
        /// </summary>
        void SignIn(int userId, bool isTokenAuthenticated);

        /// <summary>
        /// Sign the current user out.
        /// </summary>
        void SignOut();

        int GetUserId(string email);

        #endregion
    }

    public enum UserLoginAttemptStatus
    {
        /// <summary>
        /// User does not exist.
        /// </summary>
        UnknownUser = -1,

        /// <summary>
        /// User exists but the wrong password was supplied.
        /// </summary>
        BadPassword = -2,

        /// <summary>
        /// Bad email supplied for user.
        /// </summary>
        InvalidEmail = -3,

        /// <summary>
        /// Indicates that a user's login attempt succeeded.
        /// </summary>
        Success = 1,

        /// <summary>
        /// User's account is disabled.
        /// </summary>
        AccessDisabled,

        /// <summary>
        /// User successfully logged in but their current password does not meet 
        /// the current password security requirements.
        /// </summary>
        SuccessRequiresPasswordChange
    }
}
