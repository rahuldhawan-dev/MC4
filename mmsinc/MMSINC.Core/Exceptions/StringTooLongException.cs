using System;

namespace MMSINC.Exceptions
{
    public class StringTooLongException : DomainLogicException
    {
        #region Constants

        private const string MESSAGE_FORMAT =
            "Cannot set {0} to a string longer than {1} chars.";

        #endregion

        #region Constructors

        public StringTooLongException(string valueName, int maxLength)
            : base(GenerateMessage(valueName, maxLength)) { }

        #endregion

        #region Private Static Methods

        private static string GenerateMessage(string valueName, int maxLength)
        {
            return String.Format(MESSAGE_FORMAT, valueName, maxLength);
        }

        #endregion
    }
}
