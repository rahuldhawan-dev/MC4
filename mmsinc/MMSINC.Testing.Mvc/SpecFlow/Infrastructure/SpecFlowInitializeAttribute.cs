using System;
using System.Reflection;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;

namespace MMSINC.Testing.SpecFlow.Infrastructure
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class SpecFlowInitializeAttribute : Attribute
    {
        /// <summary>
        /// Assembly attribute that tells MMSINC.Testing to run a static method on a class before running SpecFlow tests.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        public SpecFlowInitializeAttribute(Type type, string methodName)
        {
            type.ThrowIfNull("type");
            methodName.ThrowIfNullOrWhiteSpace("method");

            var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
            if (method == null)
            {
                throw new TypeAccessException(string.Format("Can't find method '{0}' on type '{1}'", methodName,
                    type.FullName));
            }

            Type = type;
            Method = method;
        }

        #region Properties

        public Type Type { get; private set; }
        public MethodInfo Method { get; private set; }

        #endregion
    }
}
