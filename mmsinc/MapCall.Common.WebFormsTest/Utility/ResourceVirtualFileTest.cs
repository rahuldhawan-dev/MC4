using System.IO;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.Common.WebFormsTest.Utility
{
    /// <summary>
    /// Summary description for VirtualFileTest
    /// </summary>
    [TestClass]
    public class ResourceVirtualFileTest
    {
        #region Fields

        private Mock<IResourceManager> _mockManager = new Mock<IResourceManager>();
        private const string EXPECTED_VIRTUALPATH = "/some/dir/file.txt";
        private const string EXPECTED_RESOURCENAME = "MapCall.Common.some.dir.file.txt";

        #endregion

        [TestInitialize]
        public void ResourceVirtualFileTestInitialize()
        {
            _mockManager.Setup(x => x.ApplicationVirtualPathRoot).Returns("/");
            _mockManager.Setup(x => x.ConvertPathToResourceName(EXPECTED_VIRTUALPATH))
                        .Returns("MapCall.Common.some.dir.file.txt");
        }

        private ResourceVirtualFile Initialize()
        {
            return new TestResourceVirtualFile(EXPECTED_VIRTUALPATH, _mockManager.Object);
        }

        [TestMethod]
        public void TestConstructorSetsResourceFileNameProperty()
        {
            var target = Initialize();
            Assert.AreEqual(target.ResourceName, EXPECTED_RESOURCENAME);
        }

        [TestMethod]
        public void TestOpenMethodReturnsStream()
        {
            var target = Initialize();
            var expectedStream = new Mock<Stream>();
            _mockManager.Setup(x => x.GetStreamByResourceFile(target)).Returns(expectedStream.Object);

            var result = target.Open();

            Assert.AreSame(expectedStream.Object, result);
        }
    }

    public class TestResourceVirtualFile : ResourceVirtualFile
    {
        public TestResourceVirtualFile(string virtualPath, IResourceManager owningManager)
            : base(virtualPath, owningManager) { }

        //protected override string ConvertVirtualPathToResourceName()
        //{
        //    var path = VirtualPath;
        //    path = path.Replace("~", "");
        //    path = path.Replace("/", ".");
        //    path = ResourceManager.AssemblyName + path;

        //    return path;
        //}
    }
}
