using System.Reflection;
using MMSINC.ClassExtensions.ReflectionExtensions;

namespace MMSINC.Testing.Utilities
{
    /// <summary>
    /// Class for getting embedded file streams used in various tests. 
    /// </summary>
    public static class ImageLoader
    {
        #region Const

        private const string ROOT_IMAGE_NAMESPACE = "MMSINC.Testing.Utilities.Images";

        public const int TEST_IMAGE_WIDTH = 200;
        public const int TEST_IMAGE_HEIGHT = 150;

        #endregion

        #region Fields

        private static readonly Assembly _assembly = typeof(ImageLoader).Assembly;

        #endregion

        #region Private methods

        private static byte[] LoadFile(string fileName)
        {
            return _assembly.LoadEmbeddedFile(ROOT_IMAGE_NAMESPACE, fileName);
        }

        #endregion

        #region Exposed methods

        /// <summary>
        /// Returns a blank(all white) PNG that's 850x1100 pixels, 100 DPI. Roughly 250 bytes.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetBlank850By1100()
        {
            return LoadFile("blank-850x1100.png");
        }

        /// <summary>
        /// Returns a blank(all grey) PNG that's 850x1100 pixels, 100 DPI. Roughly 250 bytes.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetBlankGrey850By1100()
        {
            return LoadFile("grey-850x1100.png");
        }

        /// <summary>
        /// Returns a black/white striped PNG that's 850x1100 pixels, 100 DPI. Roughly 25k. More useful for regression testing
        /// </summary>
        /// <returns></returns>
        public static byte[] GetStripes850By1100()
        {
            return LoadFile("stripes-850x1100.png");
        }

        /// <summary>
        /// Returns a Tiff with less than 1 values for DPI. About 150k. 
        /// </summary>
        /// <returns></returns>
        public static byte[] GetBadDpiTiff()
        {
            return LoadFile("bad-dpi.tiff");
        }

        /// <summary>
        /// Returns a tiff file that has two images/pages in it.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetMultiPageTiff()
        {
            return LoadFile("multipage.tif");
        }

        #region Arbitrary test images used for ImageHelperTest

        /// <summary>
        /// Returns an arbitrary 200x150 BMP. Roughly 90k. Only use to test that we have bitmap support.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetArbitraryBitmap200By150()
        {
            return LoadFile("test.bmp");
        }

        /// <summary>
        /// Returns an arbitrary 200x150 JPG. Roughly 15k.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetArbitraryJpg200By150()
        {
            return LoadFile("test.jpg");
        }

        /// <summary>
        /// Returns an arbitrary 200x150 PNG. Roughly 3k. Smallest of the test files!
        /// </summary>
        /// <returns></returns>
        public static byte[] GetArbitraryPng200By150()
        {
            return LoadFile("test.png");
        }

        /// <summary>
        /// Returns an arbitrary 200x150 PNG. Roughly 7k. 
        /// </summary>
        /// <returns></returns>
        public static byte[] GetArbitraryTif200By150()
        {
            return LoadFile("test.tif");
        }

        #endregion

        #endregion
    }
}
