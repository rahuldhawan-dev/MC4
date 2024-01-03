using System;
using System.ComponentModel;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.EnumExtensions
    // ReSharper restore CheckNamespace
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns all the values in an enum in a generic array.
        /// </summary>
        /// <remarks>
        /// I'm returning this as an array(instead of an IEnumerable) to keep it compatible
        /// with the way Enum.GetValues returns stuff.
        /// </remarks>
        public static T[] GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static string DescriptionAttr<T>(this T source)
        {
            var fi = source.GetType().GetField(source.ToString());
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
    }
}
