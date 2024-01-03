using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace MMSINC.Testing.Utilities
{
    /// <summary>
    /// Stupid class for combining multiple VirtualPathProviders because the way that comes
    /// with ASP is all marked internal.
    /// </summary>
    public class CompositeVirtualPathProvider : VirtualPathProvider
    {
        #region Fields

        private readonly Dictionary<string, VirtualPathProvider> _filesByProvider;

        #endregion

        #region Properties

        public List<VirtualPathProvider> Providers { get; private set; }

        #endregion

        #region Constructor

        public CompositeVirtualPathProvider()
        {
            Providers = new List<VirtualPathProvider>();
            _filesByProvider = new Dictionary<string, VirtualPathProvider>();
        }

        #endregion

        public override bool FileExists(string virtualPath)
        {
            if (_filesByProvider.ContainsKey(virtualPath))
            {
                return true;
            }

            foreach (var p in Providers)
            {
                if (p.FileExists(virtualPath))
                {
                    _filesByProvider.Add(virtualPath, p);
                    return true;
                }
            }

            return base.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (_filesByProvider.ContainsKey(virtualPath))
            {
                return _filesByProvider[virtualPath].GetFile(virtualPath);
            }

            return base.GetFile(virtualPath);
        }
    }
}
