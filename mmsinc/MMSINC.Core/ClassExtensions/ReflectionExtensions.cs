using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.ReflectionExtensions
    // ReSharper restore CheckNamespace
{
    public static class MethodInfoExtensions
    {
        #region Extension Methods

        [Obsolete("Use the one in MemberInfoExtensions instead")]
        public static bool HasAttribute<TAttribute>(this MethodInfo method, bool inherit = false)
            where TAttribute : Attribute
        {
            // This is not forwarded to the MemberInfoExtension because there could be slight differences.
            method.ThrowIfNull("method");
            return HasAttribute(method, typeof(TAttribute), inherit);
        }

        [Obsolete("Use the one in MemberInfoExtensions instead")]
        public static bool HasAttribute(this MethodInfo method, Type attributeType, bool inherit = false)
        {
            // This is not forwarded to the MemberInfoExtension because there could be slight differences.
            method.ThrowIfNull("method");
            return method.GetCustomAttributes(attributeType, inherit).Length > 0;
        }

        #endregion
    }

    public static class TypeExtensions
    {
        #region Extension Methods

        [Obsolete("Use MemberInfoExtensions for this.")]
        public static bool HasAttribute<TAttribute>(this Type cls, bool inherit = false)
            where TAttribute : Attribute
        {
            return HasAttribute(cls, typeof(TAttribute), inherit);
        }

        [Obsolete("Use MemberInfoExtensions for this.")]
        public static bool HasAttribute(this Type cls, Type attributeType, bool inherit = false)
        {
            return cls.GetCustomAttributes(attributeType, inherit).Length > 0;
        }

        public static bool ImplementsRawGeneric(this Type toCheck, Type generic)
        {
            return toCheck.GetInterfaces().Any(i =>
                i.IsGenericType ? i.GetGenericTypeDefinition() == generic : i.ImplementsRawGeneric(generic));
        }

        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
        {
            while (toCheck != typeof(Object) && toCheck != null)
            {
                var cur = toCheck.IsGenericType
                    ? toCheck.GetGenericTypeDefinition()
                    : toCheck;
                if (generic == cur)
                {
                    return true;
                }

                // if BaseType is null, we have an interface
                toCheck = toCheck.BaseType;
            }

            return false;
        }

        public static bool HasPublicGetter(this Type toCheck, string propertyName, Type returnType)
        {
            return toCheck.GetProperty(propertyName, returnType) != null;
        }

        public static bool TryGetPublicGetter(this Type toCheck, string propertyName, out PropertyInfo prop)
        {
            prop = toCheck.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            return prop != null;
        }

        public static IEnumerable<PropertyInfo> GetAllPropertiesWithAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return type
                  .GetProperties(BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public)
                  .Where(p => p.HasAttribute<TAttribute>());
        }

        public static IEnumerable<(string Name, int Value)> GetConstantValues(this Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static)
                       .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                       .Select<FieldInfo, (string Name, int Value)>(fi =>
                            (fi.Name, (int)fi.GetValue(null)))
                       .OrderByDescending(c => c.Value);
        }

        #endregion
    }

    public static class AssemblyExtensions
    {
        #region Extension Methods

        /// <summary>
        /// Retrieve all classes (Types) from the given assembly that pass the
        /// given predicate.
        /// </summary>
        public static IEnumerable<Type> GetClassesByCondition(this Assembly assembly, Func<Type, bool> predicate)
        {
            return assembly
                  .GetTypes()
                  .Where(t => !t.IsInterface && !t.IsValueType && predicate(t));
        }

        /// <summary>
        /// Returns the byte array of an embedded resource by combining the type's namespace
        /// with the filename. Because that's how GetManifestResourceStream works.
        /// </summary>
        public static byte[] LoadEmbeddedFile(this Assembly ass, string fullFileName)
        {
            using (var s = ass.GetManifestResourceStream(fullFileName))
            {
                if (s == null)
                {
                    throw new NullReferenceException("Couldn't find embedded file: " + fullFileName);
                }

                using (var reader = new BinaryReader(s))
                {
                    return reader.ReadBytes((int)s.Length);
                }
            }
        }

        /// <summary>
        /// Returns the byte array of an embedded resource by combining the location
        /// and fileName.
        /// </summary>
        public static byte[] LoadEmbeddedFile(this Assembly ass, string location, string fileName)
        {
            return ass.LoadEmbeddedFile(location + "." + fileName);
        }

        /// <summary>
        /// Returns the byte array of an embedded resource by combining the type's namespace
        /// with the filename. Because that's how GetManifestResourceStream works.
        /// </summary>
        public static byte[] LoadEmbeddedFile(this Type type, string fileName)
        {
            return type.Assembly.LoadEmbeddedFile(type.Namespace, fileName);
        }

        public static string GetShortestNamespace(this Assembly ass)
        {
            return ass
                  .GetTypes().Select(t => t.Namespace).Distinct()
                  .Aggregate((string)null, (shortest, current) => {
                       if (String.IsNullOrEmpty(shortest))
                       {
                           return current;
                       }

                       return shortest.Length > current.Length ? current : shortest;
                   });
        }

        #endregion
    }
}
