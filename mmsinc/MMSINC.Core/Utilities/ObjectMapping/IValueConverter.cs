// ReSharper disable CheckNamespace

using System;

namespace MMSINC.Utilities
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Interface for converting a value during an ObjectMapper property mapping operation.
    /// </summary>
    public interface IValueConverter
    {
        /// <summary>
        /// Returns true if this converter can convert from one type to the other.
        /// </summary>
        /// <returns></returns>
        bool CanConvert(Type typeToConvert, Type typeToConvertTo);

        /// <summary>
        /// Converts a secondary object's value to be usable by a primary object.
        /// </summary>
        /// <param name="secondaryValue"></param>
        /// <returns></returns>
        object ToPrimary(object secondaryValue);

        /// <summary>
        /// Converts a primary object's value to be usable by a secondary object. 
        /// </summary>
        /// <param name="primaryValue"></param>
        /// <returns></returns>
        object ToSecondary(object primaryValue);
    }
}
