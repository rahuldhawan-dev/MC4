using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using GlobExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Testing.Utilities
{
    public class MvcProjectFileTester
    {
        #region Private Members

        private readonly string _projectFilePath;
        private readonly XPathNavigator _projectFileNavigator;

        #endregion

        #region Constructors

        public MvcProjectFileTester(string projectFilePath)
        {
            _projectFilePath = projectFilePath;
            
            if (!File.Exists(_projectFilePath))
            {
                throw new FileNotFoundException($"Could not find file '{_projectFilePath}'");
            }

            var doc = new XPathDocument(_projectFilePath);
            _projectFileNavigator = doc.CreateNavigator();
        }

        #endregion

        #region Private Methods

        private IEnumerable<string> GetAllViewFileNames(string basePath)
        {
            return Glob.Files(basePath, "**/*.cshtml");
        }

        private string GetProjectPath()
        {
            return Path.GetDirectoryName(_projectFilePath);
        }

        #endregion

        #region Exposed Methods

        public void AssertAllViewFilesAtPathAreContentFilesInTheProject(
            string projectPath = null)
        {
            var viewFiles = GetAllViewFileNames(projectPath ?? GetProjectPath());
            var registeredViewFiles = GetAllRegisteredViewFileNames();

            var unregisteredViews = new List<string>();

            foreach (var file in viewFiles)
            {
                if (!registeredViewFiles.Contains(file))
                {
                    unregisteredViews.Add(file);
                }
            }

            Assert.AreEqual(
                0,
                unregisteredViews.Count,
                "The following view files were not found to be registered in project file " +
                $"'{_projectFilePath}':{Environment.NewLine}" +
                string.Join(Environment.NewLine, unregisteredViews));

            var unviewedRegistrations = new List<string>();

            foreach (var file in registeredViewFiles)
            {
                if (!viewFiles.Contains(file))
                {
                    unviewedRegistrations.Add(file);
                }
            }

            Assert.AreEqual(
                0,
                unviewedRegistrations.Count,
                $"The following view files were registered in project file '{_projectFilePath}' but not " +
                $"found to exist:{Environment.NewLine}" +
                string.Join(Environment.NewLine, unviewedRegistrations));
        }

        private IEnumerable<string> GetAllRegisteredViewFileNames()
        {
            var namespaceManager = new XmlNamespaceManager(_projectFileNavigator.NameTable);
            namespaceManager.AddNamespace("msb", "http://schemas.microsoft.com/developer/msbuild/2003");

            var nodes = _projectFileNavigator
               .Select("//msb:Project/msb:ItemGroup/msb:Content", namespaceManager);

            foreach (XPathNavigator node in nodes)
            {
                var include = node.GetAttribute("Include", "");
                if (include.EndsWith(".cshtml"))
                {
                    yield return include;
                }
            }
        }

        #endregion
    }
}
