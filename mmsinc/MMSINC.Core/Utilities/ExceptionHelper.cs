using System;
using JetBrains.Annotations;

namespace MMSINC.Utilities
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// Returns a new TException instance with the message formatted. No String.Format crap here!
        /// </summary>
        [StringFormatMethod("messageFormat")]
        public static TException Format<TException>(string messageFormat, params object[] formatParams)
            where TException : Exception
        {
            var errMessage = string.Format(messageFormat, formatParams);
            return (TException)Activator.CreateInstance(typeof(TException), new object[] {errMessage});
        }
    }
}
