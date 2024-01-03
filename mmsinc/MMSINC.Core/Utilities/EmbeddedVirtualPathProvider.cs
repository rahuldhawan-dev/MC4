using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace MMSINC.Utilities
{
    /// <summary>
    /// Simple VirtualPathProvider that registers assemblies that have embedded files in them
    /// and then returns their file streams.
    /// </summary>
    public class EmbeddedVirtualPathProvider : VirtualPathProvider
    {
        #region Consts

        private const string EMBEDDED_VIRTUAL_ROOT = "~/Embed/";

        #endregion

        #region Fields

        private readonly HashSet<Assembly> _registeredAssemblies = new HashSet<Assembly>();

        private readonly Dictionary<string, Assembly> _registeredResources =
            new Dictionary<string, Assembly>(StringComparer.InvariantCultureIgnoreCase);

        #endregion

        #region Private Methods

        private string RemoveVirtualRoot(string vpp)
        {
            if (string.IsNullOrWhiteSpace(vpp))
            {
                return vpp;
            }

            // Need to call this to ensure we have "~/Blah" urls and not urls
            // that include virtual directores(ie /Modules/Mvc/).
            var fixedVpp = VirtualPathUtility.ToAppRelative(vpp);

            if (fixedVpp.StartsWith(EMBEDDED_VIRTUAL_ROOT, StringComparison.InvariantCultureIgnoreCase))
            {
                return fixedVpp.Substring(EMBEDDED_VIRTUAL_ROOT.Length);
            }

            return vpp;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers an assembly's embedded resources with this instance.
        /// </summary>
        /// <param name="ass"></param>
        public void RegisterAssembly(Assembly ass)
        {
            if (!_registeredAssemblies.Contains(ass))
            {
                _registeredAssemblies.Add(ass);

                foreach (var resourceName in ass.GetManifestResourceNames())
                {
                    _registeredResources.Add(resourceName, ass);
                }
            }
        }

        public override bool FileExists(string virtualPath)
        {
            return IsEmbeddedFile(virtualPath) || base.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsEmbeddedFile(virtualPath))
            {
                var resourceName = RemoveVirtualRoot(virtualPath);
                var ass = _registeredResources[resourceName];
                return new EmbeddedFile(virtualPath, ass, resourceName);
            }
            else
            {
                return base.GetFile(virtualPath);
            }
        }

        // Dear Ross, When you look at this screen, add MMSINC.Core.Mvc.Content.
        // Then create a BetterBundleBase that keeps track of Includes.
        // Then make a BetterStyleBundle and BetterScriptBundle.
        // Make sure to do that concatenation thing ScriptBundle uses

        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath,
            System.Collections.IEnumerable virtualPathDependencies, System.DateTime utcStart)
        {
            if (IsEmbeddedFile(virtualPath) || virtualPathDependencies == null)
            {
                return null;
            }

            var newDepends = new List<string>();

            // Need to remove the embedded paths because they can't be cached and errors get thrown. See the resource virtual path provider from the WebForms project.
            foreach (string vpd in virtualPathDependencies)
            {
                if (!IsEmbeddedFile(vpd))
                {
                    newDepends.Add(vpd);
                }
            }

            return base.GetCacheDependency(virtualPath, newDepends, utcStart);
        }

        public bool IsEmbeddedFile(string vpp)
        {
            vpp = RemoveVirtualRoot(vpp);
            return _registeredResources.ContainsKey(vpp);
        }

        #endregion

        private class EmbeddedFile : VirtualFile
        {
            private readonly Assembly _assembly;
            private readonly string _manifestName;

            public EmbeddedFile(string virtualPath, Assembly ass, string manifestName)
                : base(virtualPath)
            {
                _assembly = ass;
                _manifestName = manifestName;
            }

            public override Stream Open()
            {
                // I have no idea how this stream is used, whether a StreamReader
                // uses it and disposes it or what. So to be safe, always return 
                // a new stream.
                return _assembly.GetManifestResourceStream(_manifestName);
            }
        }
    }
}
