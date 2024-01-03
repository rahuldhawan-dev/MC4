using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MMSINC.ClassExtensions.ObjectExtensions;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.MemberInfoExtensions
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Extension methods for all Reflection types. Type, PropertyInfo, MethodInfo, etc all derive from MemberInfo.
    /// </summary>
    public static class MemberInfoExtensions
    {
        #region Extension Methods

        /// <summary>
        /// Generic version of GetCustomAttributes that also makes the inherit argument work properly.
        /// </summary>
        [Obsolete("Use the one in the core library instead")]
        public static IEnumerable<TAttr> GetCustomAttributes<TAttr>(this MemberInfo member, bool inherit = false)
            where TAttr : Attribute
        {
            member.ThrowIfNull("member");

            // Using Attribute.GetCustomAttributes vs member.GetCustomAttributes due to the latter ignoring
            // the inherit argument for some types(like PropertyInfo).
            // http://connect.microsoft.com/VisualStudio/feedback/details/337105/runtimepropertyinfo-getcustomattributes-ignores-the-inherit-attribute
            return Attribute.GetCustomAttributes(member, inherit).OfType<TAttr>();
        }

        public static bool HasAttribute<TAttribute>(this MemberInfo member, bool inherit = false)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(member, inherit).Any();
        }

        // TODO: Fix me. I fail if passed a base type for a property that has an attribut ethat inherits from the base type.
        // I do not have time to look into what may or may not break by fixing this. -Ross 8/24/2020
        public static bool HasAttribute(this MemberInfo member, Type attributeType, bool inherit = false)
        {
            return GetCustomAttributes<Attribute>(member, inherit)
               .Any(x => x.GetType().IsAssignableFrom(attributeType));
        }

        #endregion
    }
}
