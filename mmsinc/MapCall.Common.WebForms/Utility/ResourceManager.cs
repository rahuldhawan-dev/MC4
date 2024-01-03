using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using MMSINC.ClassExtensions.ReflectionExtensions;

namespace MapCall.Common.Utility
{
    public class ResourceManager : IResourceManager
    {
        #region Fields

        private string _appPathRoot;

        // Also leaving these static, because unless we start inheriting from this
        // to use in other assemblies, these will always be the same regardless
        // of instance. Assembly absolutely needs to be kept as a one-time initializer
        // since Assembly.GetExecutingAssembly() can really slow things down if it has
        // to repeatedly get it. 
        private static readonly Assembly _assembly;
        private static readonly string _baseNamespace;

        // Stores a case-insensitive lookup for a resource name. Returns the case-sensitive version.
        private readonly Dictionary<string, string> _resourceNames =
            CreateDictionary<string>();

        // Since ASP.Net doesn't care about case-sensitive names, neither do I.
        private readonly Dictionary<string, ResourceVirtualDirectory> _virtualDirectories
            = CreateDictionary<ResourceVirtualDirectory>();

        private readonly Dictionary<string, ResourceVirtualFile> _resourceFilesByAbsolutePath
            = CreateDictionary<ResourceVirtualFile>();

        private readonly Dictionary<string, ResourceVirtualFile> _resourceFilesByName
            = CreateDictionary<ResourceVirtualFile>();

        private readonly Dictionary<string, ResourceVirtualDirectory> _resourceDirectoriesByAbsolutePath
            = CreateDictionary<ResourceVirtualDirectory>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Application's virtual path root directory name. Setter is really 
        /// only for testing purposes.
        /// </summary>
        public string ApplicationVirtualPathRoot
        {
            get
            {
                if (_appPathRoot == null)
                {
                    return HostingEnvironment.ApplicationVirtualPath;
                }

                return _appPathRoot;
            }
            set { _appPathRoot = value; }
        }

        public Assembly Assembly
        {
            get { return _assembly; }
        }

        public string BaseNamespace
        {
            get { return _baseNamespace; }
        }

        public IEnumerable<string> ResourceNames
        {
            get { return _resourceNames.Keys; }
        }

        public IResourceConfiguration SiteConfiguration { get; private set; }

        /// <summary>
        /// Returns all of the embedded resource files. 
        /// </summary>
        public IEnumerable<ResourceVirtualFile> AllFiles
        {
            get { return _resourceFilesByName.Values; }
        }

        #endregion

        #region Constructors

        static ResourceManager()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _baseNamespace = _assembly.GetShortestNamespace();
        }

        public ResourceManager()
        {
            BuildResourceNamesDictionary();
        }

        #endregion

        #region Private Methods

        protected virtual IEnumerable<string> GetUsableResourceNames()
        {
            return _assembly.GetManifestResourceNames();
        }

        internal void BuildResourceNamesDictionary()
        {
            var names = GetUsableResourceNames();

            foreach (var name in names)
            {
                _resourceNames.Add(name, name);
                // Debug.Print("Actually Here: " + r);
            }
        }

        internal void CacheResources()
        {
            var allFiles = new List<ResourceVirtualFile>();
            var allDirs = new List<ResourceVirtualDirectory>();

            foreach (var rootDir in _virtualDirectories.Values)
            {
                rootDir.GetAllFiles(allFiles);
                rootDir.GetAllChildDirectories(allDirs);
                allDirs.Add(rootDir);
            }

            foreach (var dir in allDirs)
            {
                _resourceDirectoriesByAbsolutePath.Add(dir.VirtualPath, dir);
            }

            foreach (var file in allFiles)
            {
                _resourceFilesByAbsolutePath.Add(file.VirtualPath, file);
                _resourceFilesByName.Add(file.ResourceName, file);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads the embedded configuration file and parses out all its goodies. 
        /// </summary>
        public void InitializeConfiguration(IResourceConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            SiteConfiguration = config;

            InitializeResources();
        }

        public void InitializeResources()
        {
            var configDoc = GetResourceConfigDocument();
            var config = GetVirtualDirectoryNodes(configDoc);

            var appRoot = ApplicationVirtualPathRoot;

            foreach (XmlNode rootDir in config)
            {
                var rootPath = PathHelper.CombineDirectory(appRoot, rootDir.Attributes["name"].Value);
                var resources = new ResourceVirtualDirectory(rootPath, rootDir, this);
                _virtualDirectories.Add(resources.VirtualPath, resources);
            }

            CacheResources();
        }

        public virtual XmlDocument GetResourceConfigDocument()
        {
            return GetXmlResource(SiteConfiguration.ConfigurationResourceName);
        }

        public virtual XmlNodeList GetVirtualDirectoryNodes(XmlDocument configDoc)
        {
            if (configDoc == null)
            {
                throw new NullReferenceException("configDoc");
            }

            var config = configDoc.SelectNodes("gofigure/virtualDirectories/dir");

            if (config.Count == 0)
            {
                throw new NullReferenceException("VirtualDirectories not found in resourceconfig.xml");
            }

            return config;
        }

        /// <summary>
        /// Returns true if there's a VirtualDirectory that matches the virtual path.
        /// </summary>
        public bool ResourceDirectoryExists(string absolutePath)
        {
            return _resourceDirectoriesByAbsolutePath.ContainsKey(absolutePath);
        }

        /// <summary>
        /// Returns the ResourceVirtualDirectory that matches the virtual path if it exists. Otherwise it returns null. 
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public ResourceVirtualDirectory GetResourceDirectory(string absolutePath)
        {
            return (ResourceDirectoryExists(absolutePath) ? _resourceDirectoriesByAbsolutePath[absolutePath] : null);
        }

        /// <summary>
        /// Returns true if there's an embedded resource that matches the given virtual path.
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public bool ResourceFileExists(string absolutePath)
        {
            return _resourceFilesByAbsolutePath.ContainsKey(absolutePath);
        }

        /// <summary>
        /// Returns the ResourceVirtualFile for the given virtual path. Returns null if file doesn't exist.
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public ResourceVirtualFile GetResourceFile(string absolutePath)
        {
            return (ResourceFileExists(absolutePath) ? _resourceFilesByAbsolutePath[absolutePath] : null);
        }

        #region Helper Methods

        public static Dictionary<string, T> CreateDictionary<T>()
        {
            return new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        }

        internal virtual XmlDocument GetXmlResource(string resourceName)
        {
            var s = GetStreamByResourceName(resourceName);

            if (s == null)
            {
                throw new FileNotFoundException("Resource not found: " + resourceName);
            }

            var doc = new XmlDocument();
            using (s)
            {
                doc.Load(CreateXmlReader(s));
            }

            return doc;
        }

        internal static XmlReader CreateXmlReader(Stream stream)
        {
            var settings = new XmlReaderSettings();

            // To make life easier when parsing. 
            settings.IgnoreComments = true;

            return XmlReader.Create(stream, settings);
        }

        public string ConvertPathToResourceName(string path)
        {
            // This is required based on the app location. 
            // Ex: Running MapCall locally means all virtual paths are
            // initially sent with "/mapcall/" at the beginning. VirtualPathUtility.ToAppRelative
            // removes that automatically. 

            path = VirtualPathUtility.ToAppRelative(path, ApplicationVirtualPathRoot);
            var dirFile = SplitPathDirectoryAndFile(path);

            var dir = dirFile.Item1;
            dir = dir.Replace("~", "")
                     .Replace(" ", "_") // Virtual dirs get _ for spaces. Virtual files keep spaces!
                     .Replace("/", ".");

            var resourcePath = BaseNamespace + dir + dirFile.Item2;

            if (_resourceNames.ContainsKey(resourcePath))
            {
                return _resourceNames[resourcePath];
            }

            return resourcePath;
        }

        /// <summary>
        /// Takes an app relative path(directory or directory + filename) and splits them up into
        /// directory and filename. This is to make life easier with formatting the relative path
        /// the proper resource path due to the stupid naming scheme .NET uses..
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static Tuple<string, string> SplitPathDirectoryAndFile(string path)
        {
            // That +1 is there so I don't have to -1 three times.
            var lastSlashIndex = path.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1;

            // Check if slash is the last character in the string, in which case all we
            // have is a directory.
            if (lastSlashIndex == (path.Length))
            {
                return new Tuple<string, string>(path, string.Empty);
            }

            var dir = path.Substring(0, lastSlashIndex);
            var file = path.Substring(lastSlashIndex, (path.Length - lastSlashIndex));

            return new Tuple<string, string>(dir, file);
        }

        public bool HasResource(string resourcePath)
        {
            var embedPath = ConvertPathToResourceName(resourcePath);
            var exists = _resourceNames.ContainsKey(embedPath);

            //Debug.Print("HasResource({0}): {1}", exists, resourcePath);
            return exists;
        }

        public virtual Stream GetStreamByResourceName(string resourceName)
        {
            return _assembly.GetManifestResourceStream(resourceName);
        }

        /// <summary>
        /// Gets a stream for a given VirtualPath. 
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public Stream GetStreamByVirtualPath(string resourcePath)
        {
            resourcePath = ConvertPathToResourceName(resourcePath);
            return GetStreamByResourceName(resourcePath);
        }

        public Stream GetStreamByResourceFile(ResourceVirtualFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            return GetStreamByResourceName(file.ResourceName);
        }

        #endregion

        #endregion
    }
}
