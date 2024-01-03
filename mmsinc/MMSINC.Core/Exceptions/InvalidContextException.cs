using System;

namespace MMSINC.Exceptions
{
    public class InvalidContextException : Exception
    {
        #region Constructors

        public InvalidContextException(string message)
            : base(message) { }

        #endregion
    }
}
