using System;
using System.Text;
using System.Text.RegularExpressions;
using MMSINC.ClassExtensions.StringExtensions;

namespace MMSINC.Utilities
{
    public static class PhoneNumberFormatter
    {
        #region Consts

        public const string DEFAULT_FORMAT = "({0}) {1}-{2} {3}";

        #endregion

        #region Private Methods

        private static string GetExtension(string phoneNumber)
        {
            if (phoneNumber.Length <= 10)
            {
                return null;
            }

            return phoneNumber.Substring(10, phoneNumber.Length - 10);
        }

        private static string FormatClean(string phoneNumber, string format)
        {
            var areaCode = phoneNumber.Substring(0, 3);
            var exchange = phoneNumber.Substring(3, 3);
            var line = phoneNumber.Substring(6, 4);
            var ext = GetExtension(phoneNumber);
            var sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(format))
            {
                sb.Append(areaCode);
                sb.Append(exchange);
                sb.Append(line);
                if (ext != null)
                {
                    sb.Append("x").Append(ext);
                }
            }
            else
            {
                var formattedExt = string.Empty;
                if (ext != null)
                {
                    formattedExt = "x" + ext;
                }

                sb.AppendFormat(format, areaCode, exchange, line, formattedExt);
            }

            return sb.ToString().Trim();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Takes an arbitrary string and formats it into a pretty phone number. 
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="format">Optional string format. {0} = area code, {1} = exchange, {2} = line number, {3} = extension. Set to null for no formatting.</param>
        /// <returns></returns>
        public static string Format(string phoneNumber, string format = DEFAULT_FORMAT)
        {
            phoneNumber.ThrowIfNullOrWhiteSpace(phoneNumber);

            var modifiedPhone = phoneNumber.Trim();
            modifiedPhone = Regex.Replace(modifiedPhone, "[^0-9]", "");

            if (modifiedPhone.StartsWith("1"))
            {
                modifiedPhone = phoneNumber.Substring(1, phoneNumber.Length - 1);
            }

            if (modifiedPhone.Length < 10)
            {
                throw ExceptionHelper.Format<ArgumentException>(
                    "The value '{0}' can not be formatted as a phone number.", phoneNumber);
            }

            return FormatClean(modifiedPhone, format);
        }

        /// <summary>
        /// Attempts to format a numerical string into a valid phone number.
        /// Returns null if the string is invalid.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="format">Optional string format. {0} = area code, {1} = exchange, {2} = line number, {3} = extension. Set to null for no formatting.</param>
        /// <returns></returns>
        public static string TryFormat(string phoneNumber, string format = DEFAULT_FORMAT)
        {
            // Format throws an exception for null phone numbers. Rather than eating an 
            // exception(which is a performance hit), just check that it's null here. Two
            // null checks is nothing compared to throwing an exception.

            if (phoneNumber == null)
            {
                return null;
            }

            try
            {
                return Format(phoneNumber);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
