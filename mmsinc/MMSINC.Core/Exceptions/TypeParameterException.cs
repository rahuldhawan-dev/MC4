using System;

namespace MMSINC.Exceptions
{
    public class TypeParameterException : Exception
    {
        #region Constructors

        public TypeParameterException(string message)
            : base(message) { }

        #endregion
    }
}
