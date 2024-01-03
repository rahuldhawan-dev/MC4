using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class ImageHelperTest
    {
        #region Consts

        public const int TEST_IMAGE_WIDTH = 200;
        public const int TEST_IMAGE_HEIGHT = 150;

        #endregion

        #region Fields

        // These are being stored as IEnumerable's to force copying to array
        // during tests. 
        private static IEnumerable<byte> _png;
        private static IEnumerable<byte> _bmp;
        private static IEnumerable<byte> _jpg;
        private static IEnumerable<byte> _tif;
        private static IEnumerable<byte> _tifBadDpi;
        private ImageHelper _target;

        #endregion

        #region Setup/Teardown

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _png = ImageLoader.GetArbitraryPng200By150();
            _bmp = ImageLoader.GetArbitraryBitmap200By150();
            _jpg = ImageLoader.GetArbitraryJpg200By150();
            _tif = ImageLoader.GetArbitraryTif200By150();
            _tifBadDpi = ImageLoader.GetBadDpiTiff();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new ImageHelper(_png.ToArray());
        }

        [TestCleanup]
        public void CleanupTest()
        {
            if (_target != null)
            {
                _target.Dispose();
                _target = null;
            }
        }

        #endregion

        #region Constructor

        private void TestCanRead(IEnumerable<byte> data)
        {
            MyAssert.DoesNotThrow(() => new ImageHelper(data.ToArray()));
        }

        [TestMethod]
        public void TestConstructorCanReadPngFiles()
        {
            TestCanRead(_png);
        }

        [TestMethod]
        public void TestConstructorCanReadJpgFiles()
        {
            TestCanRead(_jpg);
        }

        [TestMethod]
        public void TestConstructorCanReadBmpFiles()
        {
            TestCanRead(_bmp);
        }

        [TestMethod]
        public void TestConstructorCanReadTifFiles()
        {
            TestCanRead(_tif);
        }

        [TestMethod]
        public void TestConstructorThrowsErrorWhenGivenAnUnsupportedFile()
        {
            var uselessFile = new ASCIIEncoding().GetBytes("oh no I'm a string");
            MyAssert.Throws<InvalidOperationException>(() => new ImageHelper(uselessFile),
                "The data passed to the ImageHelper is not a valid image.");
        }

        #endregion

        [TestMethod]
        public void TestHeightIsCorrect()
        {
            Assert.AreEqual(TEST_IMAGE_HEIGHT, _target.Height);
        }

        [TestMethod]
        public void TestWidthIsCorrect()
        {
            Assert.AreEqual(TEST_IMAGE_WIDTH, _target.Width);
        }

        #region Dpi

        [TestMethod]
        public void TestDpiDefaultsTo96IfTiffHasBadDpiValue()
        {
            var bytes = _tifBadDpi.ToArray();
            using (var ms = new MemoryStream(bytes))
            {
                // Making sure the embedded image is the expected one.
                var image = System.Drawing.Image.FromStream(ms);
                Assert.IsTrue(image.HorizontalResolution < 1,
                    "The HorizontalResolution should be a 0.1111112345... type value");
                Assert.IsTrue(image.HorizontalResolution > 0,
                    "The HorizontalResolution should be a 0.1111112345... type value");
            }

            using (var target = new ImageHelper(bytes))
            {
                Assert.AreEqual(96, target.Dpi);
            }
        }

        #endregion

        #region ImageFormat

        [TestMethod]
        public void TestCorrectImageFormatIsSetToBmpForBmps()
        {
            using (var target = new ImageHelper(_bmp.ToArray()))
            {
                Assert.AreEqual("bmp", target.Format.FormatName);
            }
        }

        [TestMethod]
        public void TestCorrectImageFormatIsSetToPngForPngs()
        {
            using (var target = new ImageHelper(_png.ToArray()))
            {
                Assert.AreEqual("png", target.Format.FormatName);
            }
        }

        [TestMethod]
        public void TestCorrectImageFormatIsSetToJpgForJpgs()
        {
            using (var target = new ImageHelper(_jpg.ToArray()))
            {
                Assert.AreEqual("jpeg", target.Format.FormatName);
            }
        }

        [TestMethod]
        public void TestCorrectImageFormatIsSetToTiffForTiffs()
        {
            using (var target = new ImageHelper(_tif.ToArray()))
            {
                Assert.AreEqual("tiff", target.Format.FormatName);
            }
        }

        #endregion

        #region Resize

        [TestMethod]
        public void TestResizeResizesToGivenValues()
        {
            var result = _target.Resize(32, 44); // arbitrary numbers

            using (var helper = new ImageHelper(result))
            {
                Assert.AreEqual(32, helper.Width);
                Assert.AreEqual(44, helper.Height);
            }
        }

        [TestMethod]
        public void TestResizeThrowsExceptionIfWidthLessThanZero()
        {
            MyAssert.Throws<InvalidOperationException>(() => _target.Resize(-1, 0));
        }

        [TestMethod]
        public void TestResizeThrowsExceptionIfHeightLessThanZero()
        {
            MyAssert.Throws<InvalidOperationException>(() => _target.Resize(0, -1));
        }

        #endregion

        #region ResizeProportional

        [TestMethod]
        public void TestResizeProportionalResizesProportionally()
        {
            var maxWidth = 400;
            var maxHeight = 600;
            var ratio = Math.Min((maxWidth / TEST_IMAGE_WIDTH), (maxHeight / TEST_IMAGE_HEIGHT));
            var expectedWidth = Convert.ToInt32(TEST_IMAGE_WIDTH * ratio);
            var expectedHeight = Convert.ToInt32(TEST_IMAGE_HEIGHT * ratio);

            using (var helper = new ImageHelper(_target.ResizeProportional(maxWidth, maxHeight)))
            {
                Assert.AreEqual(expectedWidth, helper.Width);
                Assert.AreEqual(expectedHeight, helper.Height);
            }
        }

        #endregion

        #region ScaleToFit

        [TestMethod]
        public void TestScaleToFitReturnsExpectedDimensionWhenActualSizeIsSmallerThanMaxSize()
        {
            float actualWidth = 200;
            float actualHeight = 300;
            float maxWidth = 450;
            float maxHeight = 600;
            float expectedWidth = 400;
            float expectedHeight = 600;

            var result = ImageHelper.GetScaledDimensions(actualWidth, actualHeight, maxWidth, maxHeight);
            Assert.AreEqual(expectedWidth, result.Width);
            Assert.AreEqual(expectedHeight, result.Height);
        }

        [TestMethod]
        public void TestScaleToFitReturnsExpectedDimensionWhenActualSizeIsLargerThanMaxSize()
        {
            float actualWidth = 400;
            float actualHeight = 600;
            float maxWidth = 250;
            float maxHeight = 300;
            float expectedWidth = 200;
            float expectedHeight = 300;

            var result = ImageHelper.GetScaledDimensions(actualWidth, actualHeight, maxWidth, maxHeight);
            Assert.AreEqual(expectedWidth, result.Width);
            Assert.AreEqual(expectedHeight, result.Height);
        }

        #endregion
    }
}
