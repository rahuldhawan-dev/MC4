using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace MMSINC.Testing.Utilities
{
    /// <summary>
    /// A VirtualPathProvider implementation that maps a virtual path to a 
    /// static file location. Only really useful for tests that need to use
    /// virtual paths but can't use HostingEnvironment.VirtualPathProvider(which
    /// has a version of a VPP that acts kinda like this).
    /// </summary>
    public class StaticVirtualPathProvider : VirtualPathProvider
    {
        #region Properties

        /// <summary>
        /// The local directory that files should exist in.
        /// </summary>
        public string AbsoluteRoot { get; set; }

        #endregion

        #region Public Methods

        public string ToAbsolutePath(string virtualPath)
        {
            if (string.IsNullOrWhiteSpace(AbsoluteRoot))
            {
                throw new Exception("AbsoluteRoot property must be set to the location the static files reside in.");
            }

            var path = virtualPath.Replace("~/", "").Replace("/", "\\");
            path = AbsoluteRoot + path;
            return path;
        }

        public override bool FileExists(string virtualPath)
        {
            //  Console.WriteLine("File Exists? " + virtualPath);
            var absolute = ToAbsolutePath(virtualPath);
            return File.Exists(absolute) || base.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            return new StaticVirtualFile(virtualPath, ToAbsolutePath(virtualPath));
            //return base.GetFile(virtualPath);
        }

        #endregion

        private class StaticVirtualFile : VirtualFile
        {
            private string _absolutePath;

            public StaticVirtualFile(string virtualPath, string absolutePath) : base(virtualPath)
            {
                _absolutePath = absolutePath;
            }

            public override Stream Open()
            {
                return File.Open(_absolutePath, FileMode.Open);
            }
        }
    }
}
