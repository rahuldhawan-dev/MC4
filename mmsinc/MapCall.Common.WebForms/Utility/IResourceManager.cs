using System.Collections.Generic;
using System.IO;

namespace MapCall.Common.Utility
{
    public interface IResourceManager
    {
        #region Properties

        IResourceConfiguration SiteConfiguration { get; }

        /// <summary>
        /// Returns all of the embedded resource files. 
        /// </summary>
        IEnumerable<ResourceVirtualFile> AllFiles { get; }

        string ApplicationVirtualPathRoot { get; set; }

        IEnumerable<string> ResourceNames { get; }

        #endregion

        #region Methods

        #region Utility

        string ConvertPathToResourceName(string path);
        Stream GetStreamByResourceName(string resourceName);
        Stream GetStreamByResourceFile(ResourceVirtualFile file);
        Stream GetStreamByVirtualPath(string resourcePath);
        bool HasResource(string resourcePath);

        #endregion

        /// <summary>
        /// Reads the embedded configuration file and parses out all its goodies. 
        /// </summary>
        void InitializeConfiguration(IResourceConfiguration config);

        void InitializeResources();

        /// <summary>
        /// Returns true if there's a VirtualDirectory that matches the virtual path.
        /// </summary>
        bool ResourceDirectoryExists(string absolutePath);

        /// <summary>
        /// Returns the ResourceVirtualDirectory that matches the virtual path if it exists. Otherwise it returns null. 
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        ResourceVirtualDirectory GetResourceDirectory(string absolutePath);

        /// <summary>
        /// Returns true if there's an embedded resource that matches the given virtual path.
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        bool ResourceFileExists(string absolutePath);

        /// <summary>
        /// Returns the ResourceVirtualFile for the given virtual path. Returns null if file doesn't exist.
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        ResourceVirtualFile GetResourceFile(string absolutePath);

        #endregion
    }
}
