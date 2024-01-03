using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Exceptions;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.ObjectExtensions
    // ReSharper restore CheckNamespace
{
    public static class ObjectExtensions
    {
        #region Constants

        public const BindingFlags PUBLIC_INSTANCE =
                                      BindingFlags.Instance |
                                      BindingFlags.Public,
                                  INSTANCE_METHOD =
                                      BindingFlags.Instance |
                                      BindingFlags.NonPublic |
                                      BindingFlags.FlattenHierarchy,
                                  STATIC_METHOD =
                                      BindingFlags.Static |
                                      BindingFlags.NonPublic |
                                      BindingFlags.FlattenHierarchy;

        #endregion

        #region Extension Methods

        /// <summary>
        /// Returns a dynamic version of the object passed in. You lose
        /// compile-time safety, so keep that in mind!
        /// </summary>
        public static dynamic AsDynamic(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(
                    "Dynamic null object? I don't think so!");
            }

            return obj;
        }

        public static object GetHiddenPropertyValueByName(this object obj, string propertyName)
        {
            if (obj == null)
                return null;

            var prop = obj.GetType().GetProperty(propertyName, INSTANCE_METHOD);

            if (prop == null)
                throw new PropertyNotFoundException(obj.GetType(), propertyName, false);

            return prop.GetValue(obj, null);
        }

        /// <summary>
        /// Gets the value of the exposed (public) property of the given object
        /// by its name.  If propertyName contains one or more dots, this
        /// method will recur down through a chain of properties.  If any part
        /// of propertyName relates to a property that doesn't exist or isn't
        /// exposed, an ArgumentOutOfRange exception will be thrown.
        /// </summary>
        /// <returns>The value of the specified property.</returns>
        public static object GetPropertyValueByName(this object obj, string propertyName)
        {
            if (obj == null)
                return null;

            if (propertyName.Contains("."))
            {
                string curProp;
                propertyName.SplitPropertyNameOnDot(out curProp, out propertyName);

                return
                    obj.InnerGetPropertyValueByName(curProp).GetPropertyValueByName(propertyName);
            }

            return obj.InnerGetPropertyValueByName(propertyName);
        }

        public static object GetFieldValueByName(this object obj, string fieldName)
        {
            var field = obj.GetType().GetField(fieldName);

            if (field == null)
            {
                throw new PropertyNotFoundException(obj.GetType(), fieldName + " (actually a field)");
            }

            return field.GetValue(obj);
        }

        public static object GetFormattedPropertyValueByName(this object obj, string propertyName)
        {
            if (obj == null)
                return null;

            if (propertyName.Contains("."))
            {
                string curProp;
                propertyName.SplitPropertyNameOnDot(out curProp, out propertyName);

                return
                    obj.InnerGetPropertyValueByName(curProp).GetPropertyValueByName(propertyName);
            }

            return obj.InnerGetFormattedPropertyValueByName(propertyName);
        }

        /// <summary>
        /// Invokes the non-public instance method with the given name on the
        /// given object, using the given parameters.
        /// </summary>
        /// <returns>The return value from invoking the specified method.</returns>
        public static object InvokeInstanceMethod(this object obj, string methodName, params object[] parameters)
        {
            var mi = obj.GetType().GetMethod(methodName, INSTANCE_METHOD);

            if (mi == null)
                throw new ArgumentOutOfRangeException("methodName", methodName,
                    "Could not find private method on target object.");

            return mi.Invoke(obj, parameters);
        }

        /// <summary>
        /// Throws an ArgumentNullException if the given object is null.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameterName"></param>
        [DebuggerStepThrough]
        public static object ThrowIfNull(this object obj, string parameterName, string message = null)
        {
            if (obj == null)
            {
                throw (String.IsNullOrWhiteSpace(message))
                    ? new ArgumentNullException(parameterName)
                    : new ArgumentNullException(parameterName, message);
            }

            return obj;
        }

        public static TObject ThrowIfNull<TObject>(this TObject obj, string parameterName, string message = null)
            where TObject : class
        {
            if (obj == null)
            {
                throw (String.IsNullOrWhiteSpace(message))
                    ? new ArgumentNullException(parameterName)
                    : new ArgumentNullException(parameterName, message);
            }

            return obj;
        }

        /// <summary>
        /// Sets the specified property on the specified object to the
        /// specified value, regardless of whether the specified
        /// property has a public setter.  Throws a PropertyNotFoundException
        /// if the specified object is not found to have a public property
        /// with the given name.  Note that this will not be able to find a
        /// private setter inherited from a parent class because of how private
        /// modifies access in inherited classes.
        /// </summary>
        public static void SetPropertyValueByName(this object obj, string propertyName, object value)
        {
            PropertyInfo prop;
            if (!obj.HasPublicProperty(propertyName, out prop))
                throw new PropertyNotFoundException(obj.GetType(), propertyName);

            try
            {
                prop.SetValue(obj, value, null);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(
                    $"Error setting property '{propertyName}' on type '{obj.GetType()}' to value '{value}'", e);
            }
        }

        /// <summary>
        /// Sets the specified property on the specified object to the
        /// specified value.  If an exposed property with the given name is not
        /// found, a PropertyNotFoundException will be thrown.  If the property
        /// is found but has no public setter, a SetterNotFound exception will
        /// be thrown.
        /// </summary>
        public static void SetPublicPropertyValueByName(this object obj, string propertyName, object value)
        {
            PropertyInfo prop;
            MethodInfo setter;

            if (!obj.HasPublicProperty(propertyName, out prop))
                throw new PropertyNotFoundException(obj.GetType(), propertyName);
            if (!obj.HasPublicSetter(prop, out setter)) throw new SetterNotFoundException(obj.GetType(), propertyName);

            setter.Invoke(obj, new[] {
                value
            });
        }

        /// <summary>
        /// Returns a boolean value indicating whether or not the given object
        /// has an exposed Field with the given name.
        /// </summary>
        public static bool HasPublicField(this object obj, string fieldName)
        {
            FieldInfo prop;
            return obj.HasPublicField(fieldName, out prop);
        }

        /// <summary>
        /// Returns a boolean value indicating whether or not the given object
        /// has an exposed field with the given name.  If found, `prop' will
        /// be set to a FieldInfo representing that field.
        /// </summary>
        public static bool HasPublicField(this object obj, string fieldName, out FieldInfo prop)
        {
            prop = obj.GetType().GetField(fieldName, PUBLIC_INSTANCE);

            return prop != null;
        }

        /// <summary>
        /// Returns a boolean value indicating whether or not the given object
        /// has an exposed property with the given name.
        /// </summary>
        public static bool HasPublicProperty(this object obj, string propertyName)
        {
            PropertyInfo prop;
            return obj.HasPublicProperty(propertyName, out prop);
        }

        /// <summary>
        /// Returns a boolean value indicating whether or not the given object
        /// has an exposed property with the given name.  If found, `prop' will
        /// be set to a PropertyInfo representing that property.
        /// </summary>
        public static bool HasPublicProperty(this object obj, string propertyName, out PropertyInfo prop)
        {
            prop = obj.GetType().GetProperty(propertyName, PUBLIC_INSTANCE);

            return prop != null;
        }

        /// <summary>
        /// Returns a boolean value indicating whether or not the given object
        /// has an exposed property with an exposed setter with the given name.
        /// </summary>
        public static bool HasPublicSetter(this object obj, string propertyName)
        {
            MethodInfo setter;
            return obj.HasPublicSetter(propertyName, out setter);
        }

        /// <summary>
        /// Returns a boolean value indicating whether or not the given object
        /// has an exposed property with an exposed setter with the given name.
        /// If found, the given `setter' will be set to a MethodInfo
        /// representing that setter.
        /// </summary>
        public static bool HasPublicSetter(this object obj, string propertyName, out MethodInfo setter)
        {
            PropertyInfo prop;
            setter = null;

            if (!obj.HasPublicProperty(propertyName, out prop)) return false;

            setter = prop.GetSetMethod();

            return setter != null;
        }

        /// <summary>
        /// Returns a boolean value indicating whether or not the given object
        /// has an exposed property with an exposed setter represented by the
        /// given PropertyInfo.  If found, the given `setter' will be set to a
        /// MethodInfo representing that setter.
        /// </summary>
        public static bool HasPublicSetter(this object obj, PropertyInfo prop, out MethodInfo setter)
        {
            setter = prop.GetSetMethod();

            return setter != null;
        }

        /// <summary>
        /// Retrieves a collection of PropertyInfo objects representing the
        /// publicly readable properties on the given object.  Useful for
        /// enumerating the gettable properties on an anonymous object.
        /// </summary>
        public static IEnumerable<PropertyInfo> GetAllPublicGetters(this object obj)
        {
            return obj.GetType()
                      .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                      .Where(pi => pi.CanRead);
        }

        public static IEnumerable<TThing> Duplicate<TThing>(this TThing obj, int copies = 1)
        {
            for (var i = -1; i < copies; ++i)
            {
                yield return obj;
            }
        }

        public static object CloneWithoutNulls(this object obj)
        {
            var ret = new ExpandoObject();
            var casted = (ICollection<KeyValuePair<string, object>>)ret;

            foreach ((PropertyInfo property, object value) in obj
                                                             .GetType()
                                                             .GetPublicPropertiesAndValues(
                                                                  obj))
            {
                if (value != null)
                {
                    casted.Add(new KeyValuePair<string, object>(property.Name, value));
                }
            }

            return ret;
        }

        #endregion

        #region Private Static Methods

        private static object InnerGetPropertyValueByName(this object obj, string propertyName)
        {
            PropertyInfo prop;
            if (!obj.HasPublicProperty(propertyName, out prop))
                throw new ArgumentOutOfRangeException("propertyName",
                    String.Format(
                        "Exposed instance property {0} was not found for object type {1}.",
                        propertyName, obj.GetType()));

            return prop.GetValue(obj, null);
        }

        private static object InnerGetFormattedPropertyValueByName(this object obj, string propertyName)
        {
            PropertyInfo prop;
            if (!obj.HasPublicProperty(propertyName, out prop))
                throw new ArgumentOutOfRangeException("propertyName",
                    String.Format(
                        "Exposed instance property {0} was not found for object type {1}.",
                        propertyName, obj.GetType()));

            var attr = prop.GetCustomAttributes<DisplayFormatAttribute>().FirstOrDefault();
            var value = prop.GetValue(obj, null);

            return (attr != null && !String.IsNullOrWhiteSpace(attr.DataFormatString))
                ? String.Format(attr.DataFormatString, value)
                : value;
        }

        private static void SplitPropertyNameOnDot(this string descriptor, out string propertyName, out string rest)
        {
            var idx = descriptor.IndexOf(".");
            propertyName = descriptor.Substring(0, idx);
            rest = descriptor.Substring(idx + 1, descriptor.Length - idx - 1);
        }

        #endregion
    }
}
