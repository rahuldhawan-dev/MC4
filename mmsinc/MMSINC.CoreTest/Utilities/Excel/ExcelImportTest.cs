using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities.Excel;
using StructureMap;

namespace MMSINC.CoreTest.Utilities.Excel
{
    [TestClass]
    public class ExcelImportTest
    {
        #region Fields

        private ExcelImport<ExcelModel> _target;
        private string _filePath;
        private IContainer _container;
        private byte[] _testBinary;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _testBinary = TestFiles.GetExcel2007File();
            _container = new Container();

            _target = _container.With(_testBinary).GetInstance<ExcelImport<ExcelModel>>();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _target.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestImportCanImportStringFields()
        {
            var result = _target.GetItems("Sheet1").ToArray();
            Assert.AreEqual("a", result[0].StringColumn);
            Assert.AreEqual("b", result[1].StringColumn);
            Assert.AreEqual("c", result[2].StringColumn);
        }

        [TestMethod]
        public void TestImportDoesNotTruncateLeadingZerosFromStringFieldsContainingNumbersWithLeadingZeros()
        {
            var result = _target.GetItems("Sheet1").ToArray();
            Assert.AreEqual("01", result[3].StringColumn);
        }

        [TestMethod]
        public void TestImportCanImportIntegerFields()
        {
            var result = _target.GetItems("Sheet1").ToArray();
            Assert.AreEqual(1, result[0].IntColumn);
            Assert.AreEqual(2, result[1].IntColumn);
            Assert.AreEqual(3, result[2].IntColumn);
        }

        [TestMethod]
        public void TestImportCanImportIntegerFieldsFromADifferentWorksheet()
        {
            var result = _target.GetItems("Sheet2").ToArray();
            Assert.AreEqual(3, result[0].IntColumn);
            Assert.AreEqual(1, result[1].IntColumn);
            Assert.AreEqual(2, result[2].IntColumn);
        }

        [TestMethod]
        public void TestGetItemsCanImportDateFields()
        {
            var result = _target.GetItems("Sheet1").ToArray();
            Assert.AreEqual(new DateTime(2014, 7, 15), result[0].DateColumn);
            Assert.AreEqual(new DateTime(2014, 7, 16), result[1].DateColumn);
            Assert.AreEqual(new DateTime(2014, 7, 17), result[2].DateColumn);
        }

        [TestMethod]
        public void TestGetItemsCanImportNullableFields()
        {
            var result = _target.GetItems("Sheet1").ToArray();
            Assert.AreEqual(1, result[0].NullIntColumn);
            Assert.IsNull(result[1].NullIntColumn);
            Assert.AreEqual(3, result[2].NullIntColumn);
        }

        [TestMethod]
        public void TestGetItemsCanImportColumnsWithSpacesInItsName()
        {
            var result = _target.GetItems("Sheet1").ToArray();
            Assert.AreEqual("yeah", result[0].ColumnWithSpaceInName);
            Assert.AreEqual("ok", result[1].ColumnWithSpaceInName);
            Assert.AreEqual("sure", result[2].ColumnWithSpaceInName);
        }

        [TestMethod]
        public void TestGetItemsCanDealWithWhiteSpaceInColumnNameDueToCenteredText()
        {
            var result = _target.GetItems("Sheet1").ToArray();
            Assert.AreEqual("a", result[0].CenteredColumnName);
            Assert.AreEqual("b", result[1].CenteredColumnName);
            Assert.AreEqual("c", result[2].CenteredColumnName);
        }

        [TestMethod]
        public void TestGetItemsTrimsStringValuesToRemoveWhiteSpace()
        {
            var result = _target.GetItems("Sheet1").ToArray();
            Assert.AreEqual("I should be trimmed", result[0].TrimColumn);
        }

        [TestMethod]
        public void
            TestGetItemsDefaultsToUsingTheFirstMatchingColumnIfMoreThanOneColumnSharesTheSameColumnNameBecauseSAPIsTheWorst()
        {
            var result = _target.GetItems("Sheet1").ToArray();
            Assert.AreEqual("yes", result[0].SameColumnName);
        }

        [TestMethod]
        public void TestTryGetImportReturnsValidationResultsWithErrors()
        {
            var target = _container
                        .With(_testBinary)
                        .GetInstance<ExcelImport<ExcelModelForBadSheet>>();
            var results = target.TryGetImport("BadSheet").Results.ToList();

            // The first IntColumn has a value of 1
            // The second IntColumn has a value of R which is not valid
            Assert.IsFalse(results[0].ValidationResults.Any());
            Assert.AreEqual("Row[3]: Property \"IntColumn\": Unable to convert \"R\" to type System.Int32.",
                results[1].ValidationResults.Single().ErrorMessage);
        }

        [TestMethod]
        public void TestTryGetImportReturnsValidationErrorIfSheetIsMissingHeaderRow()
        {
            var target = _container
                        .With(_testBinary)
                        .GetInstance<ExcelImport<ExcelModelForBadSheet>>();
            var results = target.TryGetImport("NoHeaderSheet");

            Assert.AreEqual("Unable to find a row that includes all the expected column headers.",
                results.ValidationResults.Single().ErrorMessage);
        }

        [TestMethod]
        public void TestTryGetImportReturnsValidationErrorForMissingColumnsInHeaderRow()
        {
            var target = _container
                        .With(_testBinary)
                        .GetInstance<ExcelImport<ExcelModelForSheetWithoutAllTheColumns>>();
            var results = target.TryGetImport("MissingHeaderSheet").ValidationResults.ToList();

            // The NoHeaderSheet sheet should be missing two columns.
            Assert.AreEqual("Unable to find the following columns in the header row: SomeOtherColumn.",
                results[0].ErrorMessage);
        }

        #endregion

        #region Test classes

        private class ExcelModel
        {
            public int IntColumn { get; set; }
            public string StringColumn { get; set; }
            public string ColumnWithSpaceInName { get; set; }
            public string CenteredColumnName { get; set; }
            public DateTime DateColumn { get; set; }
            public int? NullIntColumn { get; set; }
            public string TrimColumn { get; set; }
            public string SameColumnName { get; set; }
        }

        private class ExcelModelForBadSheet
        {
            public int IntColumn { get; set; }
        }

        private class ExcelModelForSheetWithoutAllTheColumns
        {
            public int IntColumn { get; set; }
            public int SomeOtherColumn { get; set; }
        }

        #endregion
    }
}
