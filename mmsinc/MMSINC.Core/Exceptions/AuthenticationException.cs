using System;

namespace MMSINC.Exceptions
{
    public class AuthenticationException : Exception
    {
        #region Constants

        public struct StringFormats
        {
            public const string INVALID_EMAIL = "Invalid email address: '{0}'",
                                USER_ALREADY_EXISTS =
                                    "User already exists for email: '{0}'",
                                USER_DOES_NOT_EXIST =
                                    "User '{0}' does not exist";
        }

        #endregion

        #region Properties

        public string Email { get; private set; }
        public string MessageFormat { get; private set; }

        #endregion

        #region Constructors

        private AuthenticationException()
            : base()
        {
            throw new InvalidOperationException();
        }

        private AuthenticationException(string message)
            : base(message)
        {
            throw new InvalidOperationException();
        }

        private AuthenticationException(string email, string format)
            : base(string.Format(format, email)) { }

        #endregion

        #region Common Exceptions

        public static AuthenticationException InvalidOrBadlyFormattedEmail(
            string email)
        {
            return new AuthenticationException(email,
                StringFormats.INVALID_EMAIL);
        }

        public static AuthenticationException UserAlreadyExists(string email)
        {
            return new AuthenticationException(email,
                StringFormats.USER_ALREADY_EXISTS);
        }

        public static AuthenticationException UserDoesNotExist(string email)
        {
            return new AuthenticationException(email,
                StringFormats.USER_DOES_NOT_EXIST);
        }

        #endregion
    }
}
