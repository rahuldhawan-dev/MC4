using System.Reflection;
using MMSINC.ClassExtensions.ReflectionExtensions;

namespace MMSINC.Testing.Utilities
{
    /// <summary>
    /// Static class with static methods to get static files for tests.
    /// This is useful for testing file uploads of different types.
    /// </summary>
    public static class TestFiles
    {
        private static Assembly _assembly = typeof(TestFiles).Assembly;

        /// <summary>
        /// Returns an Excel 2007 file. This is still the standard xlsx format in 2023. 
        /// </summary>
        /// <returns></returns>
        public static byte[] GetExcel2007File()
        {
            return _assembly.LoadEmbeddedFile("MMSINC.Testing.Utilities.Excel.excel2007.xlsx");
        }

        /// <summary>
        /// Returns an Excel 2007 file created by EPPlus. This is still a perfectly functional xlsx file, but
        /// something about how EPPlus makes files makes it not identical to what Excel itself saves.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetExcel2007FileCreatedByEPPlus()
        {
            return _assembly.LoadEmbeddedFile("MMSINC.Testing.Utilities.Excel.excel2007epplus.xlsx");
        }

        /// <summary>
        /// Returns a valid, single-page tiff file.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetSinglePageTiffFile()
        {
            return _assembly.LoadEmbeddedFile("MMSINC.Testing.Utilities.Images.test.tif");
        }
    }
}
