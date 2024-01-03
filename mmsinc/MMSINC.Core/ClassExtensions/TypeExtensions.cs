using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MMSINC.ClassExtensions.IEnumerableExtensions;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.TypeExtensions
    // ReSharper restore CheckNamespace
{
    public static class TypeExtensions
    {
        #region Constants

        public static readonly BindingFlags PUBLIC_INSTANCE_PROPERTY =
            (BindingFlags.Public | BindingFlags.Instance);

        #endregion

        #region Private Members

        private static Type _nullableType;

        #endregion

        #region Properties

        public static Type NullableType
        {
            get { return _nullableType ?? (_nullableType = typeof(Nullable<>)); }
        }

        #endregion

        /// <summary>
        /// Extension method to indicate whether or not a given Type is Nullable<T>.
        /// </summary>
        /// <returns>
        /// A bool indicating whether or not the given Type is Nullable<T>.
        /// </returns>
        public static bool IsNullable(this Type t)
        {
            return t.IsGenericType &&
                   t.GetGenericTypeDefinition() == NullableType;
        }

        private static object SafelyGet(object instance, PropertyInfo prop, Func<object> onError)
        {
            try
            {
                return prop.GetValue(instance, null);
            }
            catch
            {
                return onError();
            }
        }

        private static object SafelyGetRef(object instance, PropertyInfo prop)
        {
            return SafelyGet(instance, prop, () => null);
        }

        private static object SafelyGetVal(object instance, PropertyInfo prop)
        {
            return SafelyGet(instance, prop, () => Activator.CreateInstance(prop.PropertyType));
        }

        /// <summary>
        /// Return all public properties and their values, regardless of whether any values are null.
        /// </summary>
        public static IEnumerable<(PropertyInfo Property, object Value)> GetPublicPropertiesAndValues(this Type t,
            object instance)
        {
            foreach (var prop in instance.GetType().GetProperties(PUBLIC_INSTANCE_PROPERTY))
            {
                if ((prop.PropertyType.IsValueType &&
                     prop.PropertyType.IsNullable()) ||
                    !prop.PropertyType.IsValueType)
                {
                    var value = SafelyGetRef(instance, prop);
                    yield return (prop, value);
                }
                else if (prop.PropertyType.IsValueType)
                {
                    yield return (prop, SafelyGetVal(instance, prop));
                }
            }
        }

        public static IEnumerable<PropertyInfo> GetProperties(this Type t, BindingFlags flags,
            Func<PropertyInfo, bool> filterFn)
        {
            return t.GetProperties(flags).Where(filterFn);
        }

        public static ConstructorInfo GetGreediestConstructor(this Type t)
        {
            return t.GetConstructors().OrderByDescending(c => c.GetParameters().Count()).First();
        }

        public static bool HasPropertyNamed(this Type t, string name)
        {
            return t.GetProperties().Any(p => p.Name == name);
        }

        public static bool HasFieldNamed(this Type t, string name)
        {
            return t.GetFields().Any(p => p.Name == name);
        }

        public static bool HasParameterlessConstructor(this Type t)
        {
            return (t.GetConstructor(Type.EmptyTypes) != null);
        }

        public static bool IsOrIsSubclassOf(this Type left, Type right)
        {
            return left == right || left.IsSubclassOf(right);
        }

        public static bool IsOrIsSubclassOf<TOther>(this Type left)
        {
            return left.IsOrIsSubclassOf(typeof(TOther));
        }

        public static bool Implements<TInterface>(this Type type)
        {
            return type.Implements(typeof(TInterface));
        }

        public static bool Implements(this Type type, Type interfaceType)
        {
            return type.GetInterface(interfaceType.FullName) != null;
        }

        public static string GetFullName(this Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }

            var sb = new StringBuilder();

            sb.Append(type.Name.Substring(0, type.Name.LastIndexOf("`")));
            sb.Append(type.GetGenericArguments().Aggregate("<", (a, t) => a + (a == "<" ? "" : ",") + t.GetFullName()));
            sb.Append(">");

            return sb.ToString();
        }

        public static PropertyInfo GetPropertyFromInterface(this Type interfaceType, string name)
        {
            var prop = interfaceType.GetProperty(name);

            if (prop != null)
            {
                return prop;
            }

            foreach (var parent in interfaceType.GetInterfaces())
            {
                prop = GetPropertyFromInterface(parent, name);

                if (prop != null)
                {
                    return prop;
                }
            }

            return null;
        }
    }

    public static class TypeArrayExtensions
    {
        public static string TypeArrayToString(this Type[] types)
        {
            return String.Join(", ", types.Map<Type, string>(t => t.Name));
        }
    }
}
