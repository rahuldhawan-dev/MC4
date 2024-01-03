using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Web.Mvc;
using MMSINC.Testing.Utilities;

namespace MMSINC.Testing
{
    public class FakeBuildManager
    {
        #region Fields

        private static readonly InternalInterfaceProxyBuilder<FakeBuildManager> _proxy =
            new InternalInterfaceProxyBuilder<FakeBuildManager>(Assembly.GetAssembly(typeof(Controller)),
                "System.Web.Mvc.IBuildManager");

        public Collection<Assembly> ReferencedAssemblies = new Collection<Assembly>();

        #endregion

        #region Public static methods

        public static FakeBuildManager CreateMock()
        {
            return _proxy.CreateInstance();
        }

        #endregion

        #region IBuildManager implementation

        public virtual bool FileExists(string virtualPath)
        {
            throw new NotImplementedException();
        }

        public virtual Type GetCompiledType(string virtualPath)
        {
            throw new NotImplementedException();
        }

        public virtual ICollection GetReferencedAssemblies()
        {
            return ReferencedAssemblies;
        }

        public virtual Stream ReadCachedFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public virtual Stream CreateCachedFile(string fileName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
