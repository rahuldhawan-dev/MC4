using System;
using System.Linq;
using System.Text.RegularExpressions;
using MMSINC.Authentication;

namespace MapCall.Common.Authentication
{
    public class ContractorUserCredentialPolicy : ICredentialPolicy
    {
        #region Consts

        private const int MIN_PASSWORD_LENGTH = 8;
        public const int MAXIMUM_FAILED_LOGIN_ATTEMPT_COUNT = 6;

        public const string REQUIREMENTS_MESSAGE =
            "Passwords must be at least 8 characters in length and must meet at least 3 of the 4 requirements: At least one upper case character, at least one lower case character, at least one number, or atleast one special character.";

        #endregion

        #region Fields

        private static readonly Func<string, bool>[] _characterRequirements;

        #endregion

        #region Properties

        // bug 4077: Maximum failed attempts is 6.
        public int MaximumFailedLoginAttemptCount => MAXIMUM_FAILED_LOGIN_ATTEMPT_COUNT;

        #endregion

        #region Constructors

        static ContractorUserCredentialPolicy()
        {
            _characterRequirements = new Func<string, bool>[] {
                HasAtLeastOneLowercaseCharacter,
                HasAtLeastOneUppercaseCharacter,
                HasAtLeastOneNumberCharacter,
                HasAtLeastOneSpecialCharacter
            };
        }

        #endregion

        #region Private Methods

        private static bool HasAtLeastOneLowercaseCharacter(string password)
        {
            return Regex.IsMatch(password, "^(?=.*[a-z])");
        }

        private static bool HasAtLeastOneUppercaseCharacter(string password)
        {
            return Regex.IsMatch(password, "^(?=.*[A-Z])");
        }

        private static bool HasAtLeastOneNumberCharacter(string password)
        {
            return Regex.IsMatch(password, "^(?=.*[0-9])");
        }

        private static bool HasAtLeastOneSpecialCharacter(string password)
        {
            return Regex.IsMatch(password, "^(?=.*[!@#$%^&*().,;:'+=_`~/<>\\-\"{}\\[\\]|\\?])");
        }

        #endregion

        #region Public Methods

        public bool PasswordMeetsRequirement(string password)
        {
            if (password == null)
            {
                return false;
            }

            // Bug 4077
            // Password must be at least 8 characters in length
            if (password.Length < MIN_PASSWORD_LENGTH)
            {
                return false;
            }

            // Bug 4077
            // Password must have at least 3 out of 4 of the following: upper, lower, number, special character
            var matchedRequirements = _characterRequirements.Count(x => x(password));
            return matchedRequirements >= 3;
        }

        #endregion
    }
}
