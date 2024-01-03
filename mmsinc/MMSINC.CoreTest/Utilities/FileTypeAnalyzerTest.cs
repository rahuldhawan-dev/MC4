using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class FileTypeAnalyzerTest
    {
        private static byte[] CreateBytes(string hex)
        {
            var bytes = new List<byte>();

            foreach (var h in hex.Split('-'))
            {
                bytes.Add(byte.Parse(h, System.Globalization.NumberStyles.HexNumber));
            }

            return bytes.ToArray();
        }

        [TestMethod]
        public void TestCanDetectPdfs()
        {
            var file = CreateBytes("25-50-44-46");
            var result = FileTypeAnalyzer.GetFileType(file);
            Assert.AreEqual(FileTypes.Pdf, result);
        }

        [TestMethod]
        public void TestCanDetectBmps()
        {
            var file = CreateBytes("42-4D");
            var result = FileTypeAnalyzer.GetFileType(file);
            Assert.AreEqual(FileTypes.Bmp, result);
        }

        [TestMethod]
        public void TestCanDetectJpegs()
        {
            var file = CreateBytes("FF-D8-FF");
            var result = FileTypeAnalyzer.GetFileType(file);
            Assert.AreEqual(FileTypes.Jpeg, result);
        }

        [TestMethod]
        public void TestCanDetectPngs()
        {
            var file = CreateBytes("89-50-4E-47-0D-0A-1A-0A");
            var result = FileTypeAnalyzer.GetFileType(file);
            Assert.AreEqual(FileTypes.Png, result);
        }

        [TestMethod]
        public void TestCanDetectBothTypesOfTiffs()
        {
            var file = CreateBytes("49-49-2A-00");
            var result = FileTypeAnalyzer.GetFileType(file);
            Assert.AreEqual(FileTypes.Tiff, result);

            file = CreateBytes("4D-4D-00-2A");
            result = FileTypeAnalyzer.GetFileType(file);
            Assert.AreEqual(FileTypes.Tiff, result);
        }

        [TestMethod]
        public void TestCanDetectExcel2007Files()
        {
            var file = TestFiles.GetExcel2007File();
            var result = FileTypeAnalyzer.GetFileType(file);
            Assert.AreEqual(FileTypes.Xlsx, result);
        }

        [TestMethod]
        public void TestCanDetectExcel2007FilesAsCreatedByEPPlusLibrary()
        {
            var file = TestFiles.GetExcel2007FileCreatedByEPPlus();
            var result = FileTypeAnalyzer.GetFileType(file);
            Assert.AreEqual(FileTypes.Xlsx, result);
        }
    }
}
