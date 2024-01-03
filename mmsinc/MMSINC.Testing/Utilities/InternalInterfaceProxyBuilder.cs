using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MMSINC.Testing.Utilities
{
    /// <summary>
    /// Creates a dynamically generated type that inherits one type and also
    /// implements an internal interface from another assembly(assuming the assembly has
    /// at least one InternalsVisisbleToAttribute).
    /// This can then create new instances of that type.
    /// </summary>
    /// <typeparam name="T">T has to be a type that effectively implements the entire internal interface</typeparam>
    /// <remarks>
    /// 
    /// REUSE THE SAME INSTANCE. This is potentially memory/performance heavy. Also
    /// making two proxybuilder instances of the same internal interface will not result in type
    /// equality between the two generated types.
    /// 
    /// </remarks>
    public class InternalInterfaceProxyBuilder<T>
    {
        #region Properties

        public Type ProxyType { get; private set; }

        #endregion

        #region Constructor

        public InternalInterfaceProxyBuilder(Assembly assemblyContainingInternalInterfaceType,
            string fullInterfaceTypeName)
        {
            var internalsVisibleTo =
                assemblyContainingInternalInterfaceType.GetCustomAttributes(typeof(InternalsVisibleToAttribute), true)
                                                       .First() as InternalsVisibleToAttribute;
            var internalsVisibleToAssemblyName = internalsVisibleTo.AssemblyName.Split(',').First();

            var publicKeyString = internalsVisibleTo.AssemblyName.Split("=".ToCharArray())[1];
            var publicKey = ToBytes(publicKeyString);

            // Create a fake System.Web.Mvc.Test assembly with the public key token set
            var assemblyName = new AssemblyName {Name = internalsVisibleToAssemblyName};
            assemblyName.SetPublicKey(publicKey);

            // Get the domain of our current thread to host the new fake assembly
            var domain = Thread.GetDomain();
            var assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(internalsVisibleToAssemblyName,
                internalsVisibleToAssemblyName + ".dll");
            domain.TypeResolve += (o, e) => moduleBuilder.Assembly;

            // Create a new type that inherits from the provided generic and implements the IBuildManager interface
            var interfaceType = assemblyContainingInternalInterfaceType.GetType(fullInterfaceTypeName, true);
            var typeName = "MagicalRossProxy" + interfaceType.Name;
            var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.NotPublic | TypeAttributes.Class,
                typeof(T), new[] {interfaceType});
            ProxyType = typeBuilder.CreateType();
        }

        #endregion

        #region Private Methods

        private static byte[] ToBytes(string str)
        {
            var bytes = new List<Byte>();

            while (str.Length > 0)
            {
                var bstr = str.Substring(0, 2);
                bytes.Add(Convert.ToByte(bstr, 16));
                str = str.Substring(2);
            }

            return bytes.ToArray();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new instance of the proxy type.
        /// </summary>
        /// <returns></returns>
        public T CreateInstance()
        {
            return (T)Activator.CreateInstance(ProxyType);
        }

        #endregion
    }
}
