using System.Diagnostics;
using System.IO;
using System.Web.Hosting;

namespace MapCall.Common.Utility
{
    [DebuggerDisplay("{ResourceName}")]
    public class ResourceVirtualFile : VirtualFile
    {
        #region Fields

        private readonly string _resourceName;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the name of this file as it exists in embedded resource land.
        /// </summary>
        public string ResourceName
        {
            get { return _resourceName; }
        }

        /// <summary>
        /// Gets the ResourceManager that this VirtualFile belongs to. 
        /// </summary>
        public IResourceManager ResourceManager { get; private set; }

        #endregion

        #region Constructors

        public ResourceVirtualFile(string virtualPath, IResourceManager owningManager)
            : base(virtualPath)
        {
            ResourceManager = owningManager;
            _resourceName = ResourceManager.ConvertPathToResourceName(VirtualPath);
        }

        #endregion

        #region Public Methods

        public override Stream Open()
        {
            return ResourceManager.GetStreamByResourceFile(this);
        }

        #endregion
    }
}
