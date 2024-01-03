using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Xml;

namespace MapCall.Common.Utility
{
    public class ResourceVirtualDirectory : VirtualDirectory
    {
        #region Fields

        private readonly Dictionary<string, ResourceVirtualDirectory> _directories =
            new Dictionary<string, ResourceVirtualDirectory>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, ResourceVirtualFile> _files =
            new Dictionary<string, ResourceVirtualFile>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, VirtualFileBase> _children =
            new Dictionary<string, VirtualFileBase>(StringComparer
               .OrdinalIgnoreCase); // This is supposed to be both dirs and files.

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ResourceManager that this VirtualFile belongs to. 
        /// </summary>
        public IResourceManager ResourceManager { get; private set; }

        public override IEnumerable Directories
        {
            get { return _directories.Values; }
        }

        public override IEnumerable Files
        {
            get { return _files.Values; }
        }

        public override IEnumerable Children
        {
            get { return _children.Values; }
        }

        #endregion

        #region Constructors

        public ResourceVirtualDirectory(string virtualPath, XmlNode node, IResourceManager owningManager)
            : base(virtualPath)
        {
            ResourceManager = owningManager;
            Initialize(node);
        }

        #endregion

        #region Private Methods

        protected virtual void Initialize(XmlNode node)
        {
            InitializeChildren(node);
            InitializeFiles();
        }

        protected virtual void InitializeChildren(XmlNode node)
        {
            // Debug.Print("New Dir: " + virtualPath);
            foreach (XmlNode child in node.ChildNodes)
            {
                var nodeName = child.Name;
                if (nodeName == "dir")
                {
                    var childDir = CreateChildDirectory(child);
                    _directories.Add(childDir.VirtualPath, childDir);
                    _children.Add(childDir.VirtualPath, childDir);
                }
            }
        }

        protected virtual ResourceVirtualDirectory CreateChildDirectory(XmlNode childNode)
        {
            var dirName = childNode.Attributes["name"].Value;
            var childDirPath = PathHelper.CombineDirectory(VirtualPath, dirName);

            return new ResourceVirtualDirectory(childDirPath, childNode, ResourceManager);
        }

        protected virtual void InitializeFiles()
        {
            var resourceDir = ResourceManager.ConvertPathToResourceName(VirtualPath);
            foreach (var r in ResourceManager.ResourceNames)
            {
                if (r.StartsWith(resourceDir, StringComparison.OrdinalIgnoreCase))
                {
                    var children = r.Substring(resourceDir.Length, (r.Length - (resourceDir.Length)));
                    //       Debug.Print(children);

                    var c = children.Split('.');
                    var possibleChildDirName = c.First().Replace('_', ' ');
                    var childDirPath = PathHelper.CombineDirectory(VirtualPath, possibleChildDirName);

                    //     Debug.Print("Directories Contains Key({0}) : {1}", _directories.ContainsKey(childDirPath), childDirPath);

                    if (!_directories.ContainsKey(childDirPath))
                    {
                        var filePath = PathHelper.CombineFile(VirtualPath, children);

                        if (FindFile(filePath) == null)
                        {
                            var file = new ResourceVirtualFile(filePath, ResourceManager);

                            _files.Add(file.VirtualPath, file);
                            _children.Add(file.VirtualPath, file);
                        }
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        public ResourceVirtualFile FindFile(string virtualFilePath)
        {
            if (_files.ContainsKey(virtualFilePath))
            {
                return _files[virtualFilePath];
            }

            foreach (var dir in _directories)
            {
                var file = dir.Value.FindFile(virtualFilePath);
                if (file != null)
                {
                    return file;
                }
            }

            return null;
        }

        /// <summary>
        /// Method called to gather all files and sub-directory files together.
        /// </summary>
        /// <remarks>
        /// 
        /// This method's used by ResourceConfiguration to cache all the files by keys.
        /// 
        /// </remarks>
        public void GetAllFiles(IList<ResourceVirtualFile> fileList)
        {
            foreach (var dir in _directories.Values)
            {
                dir.GetAllFiles(fileList);
            }

            foreach (var file in _files.Values)
            {
                fileList.Add(file);
            }
        }

        public void GetAllChildDirectories(IList<ResourceVirtualDirectory> dirList)
        {
            foreach (var dir in _directories.Values)
            {
                dir.GetAllChildDirectories(dirList);
                dirList.Add(dir);
            }
        }

        #endregion
    }
}
