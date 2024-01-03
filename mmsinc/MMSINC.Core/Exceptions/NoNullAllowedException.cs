using System;

namespace MMSINC.Exceptions
{
    public class NoNullAllowedException : DomainLogicException
    {
        #region Constants

        private const string MESSAGE_FORMAT =
            "Attempt to set {0} to NULL when not allowed.";

        #endregion

        #region Constructors

        public NoNullAllowedException(string valueName)
            : base(GenerateMessage(valueName)) { }

        #endregion

        #region Private Static Methods

        private static string GenerateMessage(string valueName)
        {
            return String.Format(MESSAGE_FORMAT, valueName);
        }

        #endregion
    }
}
