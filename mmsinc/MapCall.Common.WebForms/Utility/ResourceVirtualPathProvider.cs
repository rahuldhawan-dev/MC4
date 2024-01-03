using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.Common.Utility
{
    // Allow me to introduce the WORST DOCUMENTED CLASSES IN ALL OF ASP.NET!
    // UGH: This DOES NOT WORK when a page has a ScriptManager with EnablePageMethods = true.
    // See http://connect.microsoft.com/VisualStudio/feedback/details/362269/httpexception-when-serving-a-page-with-a-scriptmanager-using-a-virtual-path-provider

    // TODO: Aggregate all VirtualFiles/Directories into one dictionary so we don't have to repeatedly do recursive file searches.

    public class ResourceVirtualPathProvider : VirtualPathProvider
    {
        #region Properties

        public IResourceManager ResourceManager { get; private set; }

        #endregion

        #region Constructors

        public ResourceVirtualPathProvider(IResourceManager manager)
        {
            ResourceManager = manager.ThrowIfNull(nameof(manager));
        }

        #endregion

        #region Public Methods

        public override bool DirectoryExists(string virtualDir)
        {
            var dirExists = (ResourceManager.ResourceDirectoryExists(virtualDir) || base.DirectoryExists(virtualDir));

            //     Debug.Print("Directory Exists({0}): {1}", dirExists, virtualDir);
            return dirExists;
        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            //     Debug.Print("GetDirectory: " + virtualDir);

            if (ResourceManager.ResourceDirectoryExists(virtualDir))
            {
                return ResourceManager.GetResourceDirectory(virtualDir);
            }

            return base.GetDirectory(virtualDir);
        }

        public override bool FileExists(string virtualPath)
        {
            var fileExists = (ResourceManager.ResourceFileExists(virtualPath) || base.FileExists(virtualPath));
            //     Debug.Print("File Exists({0}): {1}", fileExists, virtualPath);
            return fileExists;
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            //    Debug.Print("GetFile: {0}", virtualPath);
            if (ResourceManager.ResourceFileExists(virtualPath))
            {
                return ResourceManager.GetResourceFile(virtualPath);
            }

            return base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies,
            DateTime utcStart)
        {
            // Note that this has nothing to do with client-side caching. It has to do with ASP.Net's internal
            // caching system. 

            // By default, VirtualPathProvider returns null when calling the base unless there's
            // more than one VirtualPathProvider registered. Since there's going to be atleast two, we need
            // to determine if which virtualPaths are our embedded resources and return null for them.
            // Returning a CacheDependency for an embedded resource does not work. Ever. 

            // MapPathBasedVirtualPathProvider does this, so we're following suit. 
            if (virtualPathDependencies == null)
            {
                return null;
            }

            // Embedded files can't be cached. 
            if (ResourceManager.ResourceFileExists(virtualPath) || ResourceManager.ResourceDirectoryExists(virtualPath))
            {
                return null;
            }

            // This should return something if a file exists and there's another VirtualPathProvider running. So when
            // the site's actually running this should normally return something because MapPathBasedVirtualPathProvider
            // is registered by default. 
            return base.GetCacheDependency(virtualPath, RemoveManagedVirtualPathDependencies(virtualPathDependencies),
                utcStart);
        }

        protected virtual IEnumerable<string> RemoveManagedVirtualPathDependencies(IEnumerable virtualPathDependencies)
        {
            // Now need to filter out our embedded resources from the virtual dependencies list.
            // I'm not entirely sure if this is the right thing to do, but there's very little
            // useful documentation on how you're supposed to use a custom VirtualPathProvider.
            var newDepends = new List<string>();

            foreach (string vpd in virtualPathDependencies)
            {
                var ok = VirtualPathUtility.ToAbsolute(vpd);
                if (!ResourceManager.ResourceFileExists(ok) && !ResourceManager.ResourceDirectoryExists(ok))
                {
                    newDepends.Add(ok);
                }
            }

            return newDepends;
        }

        #endregion
    }
}
