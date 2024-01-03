using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using MMSINC.Testing.MSTest.TestExtensions;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.Common.WebFormsTest.Utility
{
    /// <summary>
    /// Summary description for ResourceVirtualPathProviderTest
    /// </summary>
    [TestClass]
    public class ResourceVirtualPathProviderTest
    {
        #region Fields

        private Mock<IResourceManager> _mockManager = new Mock<IResourceManager>();
        private Mock<IResourceConfiguration> _mockResourceConfig = new Mock<IResourceConfiguration>();

        #endregion

        [TestInitialize]
        public void ResourceVirtualPathProviderTestInitialize()
        {
            _mockManager.Setup(x => x.ApplicationVirtualPathRoot).Returns("/");
            _mockManager.Setup(x => x.SiteConfiguration).Returns(_mockResourceConfig.Object);

            var listOfResourceNames = new List<string>();
            listOfResourceNames.Add("MapCall.Common.Resources.scripts.choochoo.train");

            _mockManager.Setup(x => x.ResourceNames).Returns(listOfResourceNames);

            _mockManager.Setup(x =>
                             x.ConvertPathToResourceName("/resources/"))
                        .Returns("MapCall.Common.Resources");

            _mockManager.Setup(x =>
                             x.ConvertPathToResourceName("/resources/scripts/"))
                        .Returns("MapCall.Common.Resources.scripts");

            _mockManager.Setup(x => x.ResourceDirectoryExists("/resources/scripts/")).Returns(true);
            _mockManager.Setup(x => x.ResourceFileExists("/resources/scripts/choochoo.train")).Returns(true);
            _mockManager.Setup(x => x.ResourceDirectoryExists("/resources/thingymajiggers/")).Returns(false);
            _mockManager.Setup(x => x.ResourceFileExists("/resources/scripts/bad.file")).Returns(false);

            //   _mockManager.Setup(x => x.GetResourceDirectory("/resources/scripts/")).Returns(_mockVDir.Object);
        }

        private TestResourceVirtualPathProvider Initialize()
        {
            return new TestResourceVirtualPathProvider(_mockManager.Object);
        }

        #region Constructor tests

        [TestMethod]
        public void TestConstructorThrowsExceptionOnNullArgument()
        {
            MyAssert.Throws(() => new ResourceVirtualPathProvider(null));
        }

        [TestMethod]
        public void TestConstructorSetsResourceManagerProperty()
        {
            var target = Initialize();
            Assert.AreSame(_mockManager.Object, target.ResourceManager);
        }

        #endregion

        #region DirectoryExists tests

        [TestMethod]
        public void TestDirectoryExistsReturnsTrueForValidVirtualPath()
        {
            var target = Initialize();
            var result = target.DirectoryExists("/resources/scripts/");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDirectoryExistsReturnsFalseForInvalidVirtualPath()
        {
            var target = Initialize();
            var result = target.DirectoryExists("/resources/thingymajiggers/");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestDirectoryExistsReturnsFalseWhenGivenValidFilePath()
        {
            var target = Initialize();
            var result = target.DirectoryExists("/resources/scripts/choochoo.train");
            Assert.IsFalse(result);
        }

        #endregion

        #region FileExists tests

        [TestMethod]
        public void TestFileExistsReturnsTrueForValidVirtualPath()
        {
            var target = Initialize();
            var result = target.FileExists("/resources/scripts/choochoo.train");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestFileExistsReturnsFalseForInvalidVirtualPath()
        {
            var target = Initialize();
            var result = target.FileExists("/resources/scripts/bad.file");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestFileExistsReturnsFalseWhenPassedADirectoryPath()
        {
            var target = Initialize();
            var result = target.FileExists("/resources/scripts/");
            Assert.IsFalse(result);
        }

        #endregion

        #region GetCacheDependency tests

        [TestMethod]
        public void TestGetCacheDependencyReturnsNullWhenDependenciesArgumentIsNull()
        {
            var target = Initialize();
            var result = target.GetCacheDependency("/some/path", null, DateTime.UtcNow);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetCacheDependencyReturnsNullForFileWhenResourceManagerFileExistsIsTrue()
        {
            var expectedVirtualPath = "/some/path/file.ext";

            _mockManager.Setup(x => x.ResourceFileExists(expectedVirtualPath)).Returns(true);

            var target = Initialize();
            var result =
                target.GetCacheDependency(expectedVirtualPath, new Mock<IEnumerable>().Object, DateTime.UtcNow);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetCacheDependencyReturnsNUllForDirectoryWhenResourceManagerDirectoryExistsIsTrue()
        {
            var expectedVirtualPath = "/some/path/";

            _mockManager.Setup(x => x.ResourceDirectoryExists(expectedVirtualPath)).Returns(true);

            var target = Initialize();
            var result =
                target.GetCacheDependency(expectedVirtualPath, new Mock<IEnumerable>().Object, DateTime.UtcNow);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetCacheDependencyReturnsNullWhenGivenValidPath()
        {
            var target = Initialize();
            var result =
                target.GetCacheDependency("/resources/scripts/", new Mock<IEnumerable>().Object, DateTime.UtcNow);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetCacheDependencyReturnsNullWhenCallingTheBaseMethodAndThereIsNoPreviousProvider()
        {
            var target = Initialize();
            Assert.IsNull(target.GetPrevious());

            var result = target.GetCacheDependency("/some/path/to/something/", new List<string>(), DateTime.UtcNow);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestRemoveManagedVirtualPathDependenciesDoesAsItsNameSays()
        {
            var d1 = "/something/yeah/";
            var d2 = "/somethingelse/ok/eakgaoekae";
            var d3 = "/resources/scripts/";

            var dependencies = new List<string>(new[] {d1, d2, d3});

            _mockManager.Setup(x => x.ResourceDirectoryExists(d1)).Returns(true);
            _mockManager.Setup(x => x.ResourceDirectoryExists(d2)).Returns(false);
            _mockManager.Setup(x => x.ResourceFileExists(d2)).Returns(false);

            var target = Initialize();
            var result = target.TestRemoveManagedVirtualPathDependencies(dependencies);

            Assert.IsFalse(result.Contains(d1));
            Assert.IsTrue(result.Contains(d2));
            Assert.IsFalse(result.Contains(d3));
        }

        #endregion

        [TestMethod]
        public void TestGetDirectoryReturnsResourceVirtualDirectoryForValidPath()
        {
            var expectedDir = new TestResourceVirtualDirectory("eagaega", null, _mockManager.Object);
            var target = Initialize();

            _mockManager.Setup(x => x.GetResourceDirectory("/resources/scripts/")).Returns(
                expectedDir);

            var dir = target.GetDirectory("/resources/scripts/");

            Assert.AreSame(expectedDir, dir);
        }

        [TestMethod]
        public void TestGetFileReturnsResourceVirtualFileForValidPath()
        {
            var expectedFile =
                new TestResourceVirtualFile("/resources/scripts/jquery.js", _mockManager.Object);
            var target = Initialize();

            _mockManager.Setup(x => x.ResourceFileExists(expectedFile.VirtualPath)).Returns(true);
            _mockManager.Setup(x => x.GetResourceFile(expectedFile.VirtualPath)).Returns(expectedFile);

            var file = target.GetFile(expectedFile.VirtualPath);

            Assert.AreSame(expectedFile, file);
        }

        [TestMethod]
        public void TestGetDirectoryReturnsNullForInvalidPath()
        {
            var target = Initialize();
            Assert.IsNull(target.GetDirectory("/some/path/"));
        }

        [TestMethod]
        public void TestGetResourceFileReturnsNullForInvalidPath()
        {
            var target = Initialize();
            Assert.IsNull(target.GetFile("/some/file.txt"));
        }
    }

    public class TestResourceVirtualPathProvider : ResourceVirtualPathProvider
    {
        #region Constructors

        public TestResourceVirtualPathProvider(IResourceManager manager)
            : base(manager) { }

        #endregion

        #region Methods

        public VirtualPathProvider GetPrevious()
        {
            return Previous;
        }

        public IEnumerable<string> TestRemoveManagedVirtualPathDependencies(IEnumerable depends)
        {
            return RemoveManagedVirtualPathDependencies(depends);
        }

        #endregion
    }
}
