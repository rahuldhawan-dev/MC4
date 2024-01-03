// ReSharper disable CheckNamespace

using System;
using MMSINC.ClassExtensions.TypeExtensions;

namespace MMSINC.Utilities
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Default implementation of IValueConverter. No conversion actually occurs. 
    /// The value passed in is the value passed out.
    /// </summary>
    public class DefaultValueConverter : IValueConverter
    {
        #region Private Methods

        private static bool TypeIsAssignable(Type one, Type two)
        {
            if (one.IsAssignableFrom(two))
            {
                return true;
            }

            if (!one.IsNullable() && !two.IsNullable())
            {
                return false;
            }

            // We need to special case nullables. Reflection sets
            // null values on value types to default(T). It won't
            // throw an exception.

            var oneNullable = (one.IsNullable() ? one.GetGenericArguments()[0] : one);
            var twoNullable = (two.IsNullable() ? two.GetGenericArguments()[0] : two);
            return TypeIsAssignable(oneNullable, twoNullable);
        }

        #endregion

        #region Public Methods

        public virtual bool CanConvert(Type typeToConvert, Type typeToConvertTo)
        {
            return TypeIsAssignable(typeToConvert, typeToConvertTo);
        }

        public virtual object ToPrimary(object secondaryValue)
        {
            return secondaryValue;
        }

        public virtual object ToSecondary(object primaryValue)
        {
            return primaryValue;
        }

        #endregion
    }
}
