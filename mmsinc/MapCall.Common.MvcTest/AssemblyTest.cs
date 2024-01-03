using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using MapCall.Common.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Testing;

namespace MapCall.Common.MvcTest
{
    // [DeploymentItem(@"x86\SQLite.Interop.dll", "x86")]
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    [TestClass]
    public class AssemblyTest
    {
        #region Fields

        private XPathNavigator _target;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            // Can't use the Project constructor to do this because apparently it automatically adds itself to this
            // ProjectCollection.GlobalProjectCollection collection(collection). It throws an exception after it's already
            // been added once. ;
            var currentPath = Path.GetFullPath(".");
            var path =
                currentPath
                    .ReplaceRegex(@"\\TestResults\\[^\\]+\\Out", string.Empty)
                    .Replace(@"\everything", @"\mmsinc")
                + @"\MapCall.Common.Mvc\MapCall.Common.Mvc.csproj";

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(
                    string.Format("Could not find file '{0}'. Current path is '{1}'.", path, currentPath), path);
            }

            var doc = new XPathDocument(path);
            _target = doc.CreateNavigator();
        }

        #endregion

        #region Private Methods

        private void AssertCorrectBuildAction(Func<string, bool> predicate, string itemType)
        {
            var namespaceManager = new XmlNamespaceManager(_target.NameTable);
            namespaceManager.AddNamespace("msb", "http://schemas.microsoft.com/developer/msbuild/2003");

            var nodes = _target
                .Select("//msb:Project/msb:ItemGroup/msb:None | " +
                        "//msb:Project/msb:ItemGroup/msb:Compile | " +
                        "//msb:Project/msb:ItemGroup/msb:EmbeddedResource", namespaceManager);
            var fails = new List<string>();
            
            foreach (XPathNavigator node in nodes)
            {
                var include = node.GetAttribute("Include", "");
                if (predicate(include) && node.Name != itemType)
                {
                    fails.Add(include);
                }
            }

            if (fails.Any())
            {
                Assert.Fail("The following files do not have the expected build action of '{0}': {1}", itemType,
                    string.Join(", ", fails));
            }
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSharedViewsHaveBuildActionSetToNone()
        {
            AssertCorrectBuildAction(include => include.EndsWith(".cshtml"), "None");
        }

        [TestMethod]
        public void TestGeneratedCodeForSharedViewsHaveBuildActionSetToCompile()
        {
            AssertCorrectBuildAction(include => include.EndsWith(".generated.cs"), "Compile");
        }

        [TestMethod]
        public void TestContentFilesAllHaveTheirBuildActionSetToEmbeddedResource()
        {
            AssertCorrectBuildAction(include => include.StartsWith(@"Content\"), "EmbeddedResource");
        }

        [TestMethod]
        public void TestControllers()
        {
            TestLibrary.TestAllControllersInheritFromCustomControllerBase(typeof(ErrorController).Assembly);
        }

        #endregion
    }
}
