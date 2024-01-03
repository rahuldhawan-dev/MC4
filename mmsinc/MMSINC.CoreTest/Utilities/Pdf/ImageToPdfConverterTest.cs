using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities.Pdf;

namespace MMSINC.CoreTest.Utilities.Pdf
{
    // [DeploymentItem("evointernal.dat")] // Take this out and EvoPdf tests will start crashing.
    [TestClass] // It may not be true anymore as of Visual Studio 2013.
    public class ImageToPdfConverterTest
    {
        #region Fields

        private ImageToPdfConverter _target;
        private byte[] _multiPageTiff = ImageLoader.GetMultiPageTiff();

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new ImageToPdfConverter();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestLicenseIsNotNullOrEmptyOrSomeOtherWrongValue()
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(_target.License));
        }

        [TestMethod]
        public void TestRenderDoesNotCrashWhenCreatingPagesFromMultiPageTiffs()
        {
            MyAssert.DoesNotThrow(() => _target.Render(new[] {_multiPageTiff}));
        }

        [TestMethod]
        public void TestRenderSetsLicenseOnDocumentObject()
        {
            var result = _target.RenderToDocument(new[] {_multiPageTiff});
            Assert.AreEqual(_target.License, result.LicenseKey);
        }

        [TestMethod]
        public void TestRenderSplitsMultiPageTiffsIntoSeparatePages()
        {
            var result = _target.RenderToDocument(new[] {_multiPageTiff});
            // PdfPage object does not give any way to access the elements that were
            // added to it. This is all that can be tested.
            Assert.AreEqual(2, result.Pages.Count);
        }

        #endregion
    }
}
