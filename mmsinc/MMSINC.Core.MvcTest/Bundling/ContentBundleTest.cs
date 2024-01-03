using System;
using System.Linq;
using MMSINC.Bundling;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Bundling
{
    [TestClass]
    public class ContentBundleTest
    {
        [TestMethod]
        public void TestConstructorPassesVirtualPathToBaseConstructor()
        {
            var result = new ContentBundle("~/virtual");
            Assert.AreEqual("~/virtual", result.Path);
        }

        [TestMethod]
        public void TestConstructorPassesCdnPathtoBaseConstructor()
        {
            var result = new ContentBundle("~/virtual");
            Assert.IsNull(result.CdnPath);

            result = new ContentBundle("~/virtual", null);
            Assert.IsNull(result.CdnPath);

            result = new ContentBundle("~/virtual", "cdn thing");
            Assert.AreEqual("cdn thing", result.CdnPath);
        }

        [TestMethod]
        public void TestIncludeWithSingleFileAddsPathToIncludedFilesList()
        {
            var target = new ContentBundle("~/virtual");
            target.Include("~/some/file.css");
            Assert.AreEqual("~/some/file.css", target.IncludedFiles.Single());
        }

        [TestMethod]
        public void TestIncludeWithParamsArgumentAddsAllPathsToIncludedFilesList()
        {
            var target = new ContentBundle("~/virtual");
            target.Include("~/some/file.css", "~/somethingelse");
            var paths = target.IncludedFiles.ToArray();
            Assert.AreEqual("~/some/file.css", paths[0]);
            Assert.AreEqual("~/somethingelse", paths[1]);
        }

        [TestMethod]
        public void TestIncludeDirectoryThrowsNotImplementedException()
        {
            var target = new ContentBundle("~/virtual");
            MyAssert.Throws<NotImplementedException>(() => target.IncludeDirectory("virt", "pattern"));
            MyAssert.Throws<NotImplementedException>(() => target.IncludeDirectory("virt", "pattern", true));
        }
    }
}
