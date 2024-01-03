using System;
using System.Collections.Generic;
using System.Web.Optimization;

namespace MMSINC.Bundling
{
    /// <summary>
    /// Base class for bundling files together for MVC bundling. This is primarily
    /// for testing because bundles silently ignore included files that can't be found.
    /// </summary>
    public class ContentBundle : Bundle
    {
        #region Fields

        private readonly HashSet<string> _includedFiles = new HashSet<string>();

        #endregion

        #region Properties

        /// <summary>
        /// Returns the virtual paths for files that were included through the Include method.
        /// This includes all the files that were silently ignored by the Include method.
        /// 
        /// NOTE: This will fail at finding files if you include them through wildcards. 
        /// </summary>
        public IEnumerable<string> IncludedFiles
        {
            get { return _includedFiles; }
        }

        #endregion

        #region Constructor

        public ContentBundle(string virtualPath, string cdnPath = null) : base(virtualPath, cdnPath) { }

        #endregion

        #region Public Methods

        public override Bundle Include(params string[] virtualPaths)
        {
            foreach (var vp in virtualPaths)
            {
                Include(vp);
            }

            return this;
        }

        public override Bundle Include(string virtualPath, params IItemTransform[] transforms)
        {
            base.Include(virtualPath, transforms);
            _includedFiles.Add(virtualPath);
            return this;
        }

        public override Bundle IncludeDirectory(string directoryVirtualPath, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public override Bundle IncludeDirectory(string directoryVirtualPath, string searchPattern,
            bool searchSubdirectories)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
