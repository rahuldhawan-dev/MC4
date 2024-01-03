using System;
using System.Collections.Generic;

namespace MMSINC.Utilities.ObjectMapping
{
    public sealed class ObjectMapperException : Exception
    {
        #region Constructor

        private ObjectMapperException(string message, Exception innerException = null) :
            base(message, innerException) { }

        #endregion

        #region Public Methods

        public static ObjectMapperException UnableToMapProperty(string propertyName, Exception innerException)
        {
            const string format = "Unable to map property '{0}'. See inner exception for more info.";
            return new ObjectMapperException(string.Format(format, propertyName), innerException);
        }

        public static ObjectMapperException MultipleSettersFound(IEnumerable<string> primaryPropNames)
        {
            const string format =
                "Multiple properties({0}) are trying to map a value to the same property on a secondary object. This is not allowed.";

            var primes = string.Join(", ", primaryPropNames);
            return new ObjectMapperException(string.Format(format, primes));
        }

        #endregion
    }
}
