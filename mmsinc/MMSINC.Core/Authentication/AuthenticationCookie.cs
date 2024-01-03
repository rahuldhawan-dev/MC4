using System;
using MMSINC.ClassExtensions.StringExtensions;
using StructureMap;

namespace MMSINC.Authentication
{
    public interface IAuthenticationCookie
    {
        #region Properties

        /// <summary>
        /// Returns the numerical id that represents the user in the database.
        /// </summary>
        int? Id { get; }

        /// <summary>
        /// Returns the username portion of the cookie. This can be any non-null/empty/whitespace string.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Gets whether the original cookie value is valid. This does not validate anything against 
        /// the database or checks that users actually exist or anything. This only checks that the 
        /// expected parts for the cookie all exist.
        /// </summary>
        bool IsValidlyFormatted { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Formats the Id and UserName into a usable string that the FormsAuthenticator can use.
        /// </summary>
        /// <returns></returns>
        string ToCookieString();

        #endregion
    }

    /// <summary>
    /// Represents the FormsAuthentication's FormsIdentity.Name information. 
    /// </summary>
    public class AuthenticationCookie : IAuthenticationCookie
    {
        #region Consts

        internal const string FORMAT = "{0}" + SEPARATOR + "{1}",
                              SEPARATOR = "; ";

        #endregion

        #region Properties

        /// <summary>
        /// Returns the original cookie value used to create this instance, but only if the constructor
        /// that accepts the original cookie is used. This is for debugging purposes.
        /// </summary>
        public string OriginalCookie { get; private set; }

        /// <summary>
        /// Returns the numerical id that represents the user in the database.
        /// </summary>
        public int? Id { get; protected set; }

        /// <summary>
        /// Returns the username portion of the cookie. This can be any non-null/empty/whitespace string.
        /// </summary>
        public string UserName { get; protected set; }

        /// <summary>
        /// Gets whether the original cookie value is valid. This does not validate anything against 
        /// the database or checks that users actually exist or anything. This only checks that the 
        /// expected parts for the cookie all exist.
        /// </summary>
        public virtual bool IsValidlyFormatted
        {
            get
            {
                return (Id.GetValueOrDefault() > 0 && !string.IsNullOrWhiteSpace(UserName) && UserName.IsValidEmail());
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an empty, invalid AuthenticationCookie instance.
        /// </summary>
        public AuthenticationCookie() : this(null, null) { }

        /// <summary>
        /// Creates an AuthenticationCookie by parsing the original cookie value into its individual parts. 
        /// Check the IsValidlyFormatted property to determine the outcome.
        /// </summary>
        /// <param name="cookie"></param>
        public AuthenticationCookie(IContainer container, string cookie)
        {
            OriginalCookie = cookie;
            ParseAndSetValues(container, cookie);
        }

        /// <summary>
        /// Creates an AuthenticationCookie for the supplied id and username. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        public AuthenticationCookie(int id, string username)
        {
            username.ThrowIfNullOrWhiteSpace("UserName parameter must be set when using this constructor.");
            Id = id;
            UserName = username;
        }

        #endregion

        #region Private Methods

        protected virtual void ParseAndSetValues(IContainer container, string rawCookieValue)
        {
            // Cookies must come in the format of "#{id}; #{username}", ex: "1; someuser" without the quotes.
            const int ID_INDEX = 0;
            const int USERNAME_INDEX = 1;
            const int EXPECTED_STRING_SPLIT_LENGTH = 2;

            if (!string.IsNullOrWhiteSpace(rawCookieValue))
            {
                var deets = rawCookieValue.Split(new[] {SEPARATOR}, StringSplitOptions.RemoveEmptyEntries);
                if (deets.Length == EXPECTED_STRING_SPLIT_LENGTH)
                {
                    UserName = deets[USERNAME_INDEX];
                    int id;

                    if (int.TryParse(deets[ID_INDEX], out id))
                    {
                        Id = id;
                    }
                }
            }
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Formats the Id and UserName into a usable string that the FormsAuthenticator can use.
        /// </summary>
        /// <returns></returns>
        public virtual string ToCookieString()
        {
            if (!IsValidlyFormatted)
            {
                throw new InvalidOperationException(
                    "AuthenticationCookies can not be formatted when they are not valid.");
            }

            return string.Format(FORMAT, Id, UserName);
        }

        #endregion
    }
}
