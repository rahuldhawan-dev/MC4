using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class EmbeddedVirtualPathProviderTest
    {
        #region Fields

        private EmbeddedVirtualPathProvider _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new EmbeddedVirtualPathProvider();
            _target.RegisterAssembly(typeof(MMSINC.Testing.Utilities.ImageLoader).Assembly);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestFileExistsReturnsTrueForVirtualPath()
        {
            var expected = "~/Embed/MMSINC.Testing.Utilities.Images.test.png";
            Assert.IsTrue(_target.FileExists(expected));
            Assert.IsTrue(_target.FileExists(expected.ToUpper()), "FileExists must be case insensitive.");
        }

        //[TestMethod]
        //public void TestFileExistsReturnsTrueForNonVirtualPath()
        //{
        //    var expected = "/Embed/MMSINC.Testing.Utilities.Images.test.png";
        //    Assert.IsTrue(_target.FileExists(expected));
        //    Assert.IsTrue(_target.FileExists(expected.ToUpper()), "FileExists must be case insensitive.");
        //}

        [TestMethod]
        public void TestFileExistReturnsFalseIfFileDoesNotExistAsEmbeddedResource()
        {
            Assert.IsFalse(_target.FileExists("~/Embed/IDoNot.exist"));
        }

        [TestMethod]
        public void TestGetFileReturnsCorrectFile()
        {
            var expected = ImageLoader.GetArbitraryPng200By150();
            var file = _target.GetFile("~/Embed/MMSINC.Testing.Utilities.Images.test.png");
            using (var stream = file.Open())
            {
                MyAssert.StreamsAreEqual(expected, stream);
            }
        }

        #endregion
    }
}
