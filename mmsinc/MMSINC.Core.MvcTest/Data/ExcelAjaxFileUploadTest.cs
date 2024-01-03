using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Data;
using MMSINC.Utilities.Excel;
using StructureMap;

namespace MMSINC.Core.MvcTest.Data
{
    [TestClass]
    public class ExcelAjaxFileUploadTest
    {
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
        }

        #region Tests

        [TestMethod]
        public void TestValidationReturnsEarlyForConversionErrors()
        {
            var fileData = GetType().Assembly.LoadEmbeddedFile("MMSINC.Core.MvcTest.Data.excel2007.xlsx");
            var target = new ExcelAjaxFileUpload<ExcelModelForBadSheet>(_container);
            target.FileUpload = new MMSINC.Metadata.AjaxFileUpload();
            target.FileUpload.BinaryData = fileData;
            target.SheetName = "BadSheet";
            var results = target.Validate(null).ToList();

            Assert.AreEqual(1, results.Count,
                "There are two rows in the file, there should be only one validation error.");
            Assert.AreEqual("Row[3]: Property \"IntColumn\": Unable to convert \"R\" to type System.Int32.",
                results.Single().ErrorMessage);
        }

        [TestMethod]
        public void TestNormalValidationIsRanIfThereAreNoConversionErrors()
        {
            var fileData = GetType().Assembly.LoadEmbeddedFile("MMSINC.Core.MvcTest.Data.excel2007.xlsx");
            var target = new ExcelAjaxFileUpload<ExcelModelForBadSheet>(_container);
            target.FileUpload = new MMSINC.Metadata.AjaxFileUpload();
            target.FileUpload.BinaryData = fileData;
            target.SheetName = "RequiredSheet";
            var results = target.Validate(null).ToList();

            Assert.AreEqual(1, results.Count,
                "There are two rows in the file, there should be only one validation error.");
            Assert.AreEqual("Row[2]: The RequiredInt field is required.", results.Single().ErrorMessage);
        }

        [TestMethod]
        public void TestAnyOtherValidatorBesidesRequiredValidatorRuns()
        {
            var fileData = GetType().Assembly.LoadEmbeddedFile("MMSINC.Core.MvcTest.Data.excel2007.xlsx");
            var target = new ExcelAjaxFileUpload<ExcelModelForBadSheet>(_container);
            target.FileUpload = new MMSINC.Metadata.AjaxFileUpload();
            target.FileUpload.BinaryData = fileData;
            target.SheetName = "RangeSheet";
            var results = target.Validate(null).ToList();

            Assert.AreEqual(1, results.Count,
                "There are two rows in the file, there should be only one validation error.");
            Assert.AreEqual("Row[2]: The field RangeInt must be between 1 and 10.", results.Single().ErrorMessage);
        }

        #endregion

        #region Test classes

        private class ExcelModelForBadSheet
        {
            public int IntColumn { get; set; }

            [Required]
            public int? RequiredInt { get; set; }

            [Range(1, 10)]
            public int RangeInt { get; set; }
        }

        #endregion
    }
}
