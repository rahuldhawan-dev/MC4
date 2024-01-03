using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;
using JetBrains.Annotations;
using MMSINC.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using StructureMap;

namespace MMSINC.Testing
{
    // TODO: This could potentially be converted to an actual ExcelReader class if we ever need it.
    /// <summary>
    /// Helper class for reading back excel files exported by an ExcelResult object. 
    /// MAKE SURE TO DISPOSE THIS! Disposing deletes the created file so it doesn't waste space.
    /// </summary>
    public class ExcelResultTester : IDisposable
    {
        #region Fields

        private bool _isDisposed;
        private ExcelPackage _package;
        private string _firstSheetName;
        private int _rowsToSkip;
        private readonly IContainer _container;

        #endregion

        #region Properties

        public ExcelResult ExcelResult { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="executeResult">Set to true if the result needs to be executed. Default is false.</param>
        /// <param name="skipRows">The row that the data starts at, not 0 based. Defaults to 0</param>
        public ExcelResultTester(IContainer container, ExcelResult result, bool executeResult = false, int skipRows = 0)
        {
            _container = container;
            _rowsToSkip = skipRows;
            ExcelResult = result;
            ExcelResult.IsInTestMode = true;
            ExcelResult.Autofit = false;
            if (executeResult)
            {
                var pipeline = new FakeMvcHttpHandler(_container);
                var controller = pipeline.CreateAndInitializeController<FakeController>();
                result.ExecuteResult(controller.ControllerContext);
            }
        }

        #endregion

        #region Private Methods

        private string GetFirstSheetName()
        {
            if (_firstSheetName == null)
            {
                _firstSheetName = GetSheetNames().First();
            }

            return _firstSheetName;
        }

        private static DateTime StripMillisecondsFromDateTime(DateTime date)
        {
            if (date.Millisecond != 0)
            {
                return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            }

            return date;
        }

        /// <summary>
        /// Converts a value to the crappy version Excel would expect.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object PrepareValueForComparison(object value)
        {
            if (value != null)
            {
                if (value is int || value is float)
                {
                    // Excel 2003 uses doubles for ints and floats
                    return Convert.ToDouble(value);
                }

                if (value is DateTime)
                {
                    return StripMillisecondsFromDateTime((DateTime)value);
                }

                if (value is bool)
                {
                    return value;
                }

                if (value.GetType().IsClass)
                {
                    return value.ToString();
                }
            }

            // Otherwise return unchanged.
            return value;
        }

        #endregion

        #region Public Methods

        public void CopyFileTo(string location)
        {
            System.IO.File.Copy(ExcelResult.FileName, location, true);
        }

        public ExcelPackage GetExcelPackage()
        {
            if (!ExcelResult.HasExecuted)
            {
                throw new Exception("The result has not executed yet, so there's nothing to connect to.");
            }

            if (_package == null)
            {
                _package = new ExcelPackage(new FileInfo(ExcelResult.FileName));
            }

            return _package;
        }

        /// <summary>
        /// Returns the number of rows in a sheet. This does not count the header row.
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public int GetRowCount(string sheetName)
        {
            var sheet = GetWorksheet(sheetName);
            if (sheet.Dimension == null)
            {
                // Dimension will be null when there aren't any cells.
                return 0;
            }

            return sheet.Dimension.Rows - 1; // -1 to ignore the header row.
        }

        public IEnumerable<string> GetSheetNames()
        {
            return GetExcelPackage().Workbook.Worksheets.Select(sheet => sheet.Name);
        }

        public Dictionary<string, int> GetColumnNamesAndIndex(string sheetName)
        {
            var worksheet = GetWorksheet(sheetName);
            var startColumn = worksheet.Dimension.Start.Column;
            var endColumn = worksheet.Dimension.End.Column + 1;

            var dict = new Dictionary<string, int>();
            for (var i = startColumn; i < endColumn; i++)
            {
                dict.Add((string)worksheet.Cells[1 + _rowsToSkip, i].Value, i);
            }

            return dict;
        }

        /// <summary>
        /// Returns a DataTable for the given Excel sheet. This returns a cached instance.
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public ExcelWorksheet GetWorksheet(string sheetName)
        {
            var worksheet = GetExcelPackage().Workbook.Worksheets.SingleOrDefault(x => x.Name == sheetName);
            if (worksheet == null)
            {
                var allSheetNames = string.Join(", ", GetSheetNames());
                throw new Exception(
                    $"Unable to locate worksheet with name \"{sheetName}\". Did find the following sheets: {allSheetNames}");
            }

            return worksheet;
        }

        // This may need to be modified to offset the rowIndex by 1.
        public ExcelRow GetSheetRowAtIndex(string sheet, int rowIndex)
        {
            var worksheet = GetWorksheet(sheet);
            return worksheet.Row(rowIndex);
        }

        public T GetValue<T>(string columnName, int rowIndex)
        {
            return GetValue<T>(GetFirstSheetName(), columnName, rowIndex);
        }

        public T GetValue<T>(string sheetName, string columnName, int rowIndex)
        {
            // EPPlus isn't zero-based and we're assuming the first row is actually column names.
            // So add 2 to the given rowIndex.
            // And account for the startingRow
            var actualRowIndex = rowIndex + 2 + _rowsToSkip;
            var worksheet = GetWorksheet(sheetName);
            var columns = GetColumnNamesAndIndex(sheetName);
            var columnIndex = columns[columnName];

            return (T)worksheet.Cells[actualRowIndex, columnIndex].Value;
        }

        #region Assertions

        private void AssertEquality(object expectedValue, string sheet, string columnName, int rowIndex = 0,
            string message = null, params object[] formatParams)
        {
            var realExpectedValue = PrepareValueForComparison(expectedValue);
            var resultValue = GetValue<object>(sheet, columnName, rowIndex);

            if (realExpectedValue == null)
            {
                // Depending on the column type, EPPlus reads null values as either nulls or empty strings.
                if (resultValue as string == string.Empty)
                {
                    resultValue = null;
                }

                resultValue.Should().BeNull();
            }
            else if (realExpectedValue is decimal realExpectedDecimalValue)
            {
                if (resultValue is double resultDoubleValue)
                {
                    // Excel is really shitty and even if you have a column as a DECIMAL it
                    // gets returned as a DOUBLE. This somehow works at fixing the problem.`
                    resultValue = Convert.ToDecimal(resultDoubleValue);
                }

                ((decimal)resultValue).Should().Be(realExpectedDecimalValue);
            }
            else if (realExpectedValue is long realExpectedLongValue)
            {
                resultValue = Convert.ToInt64(resultValue);

                ((long)resultValue).Should().Be(realExpectedLongValue);
            }
            else if (realExpectedValue is DateTime realExpectedDateTimeValue)
            {
                if (resultValue is double resultDoubleValue)
                {
                    // Excel's date format doesn't handle milliseconds so we need to strip that out.
                    var date = DateTime.FromOADate(resultDoubleValue);
                    date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                    resultValue = date;
                }

                ((DateTime)resultValue).Should().BeCloseTo(realExpectedDateTimeValue, 1.Seconds());
            }
        }

        /// <summary>
        /// Asserts that the given value is equal to the value for a specific cell in the Excel sheet.
        /// </summary>
        [StringFormatMethod("message")]
        public void AreEqual(object expectedValue, string sheet, string columnName, int rowIndex = 0,
            string message = null, params object[] formatParams)
        {
            if (expectedValue == null)
            {
                // This is to prevent writing sloppy tests where we're checking that all the values are there but not all 
                // of the values have been set. 
                throw new InvalidOperationException("Use IsNull for testing null values. Column name: " + columnName);
            }

            AssertEquality(expectedValue, sheet, columnName, rowIndex, message, formatParams);
        }

        /// <summary>
        /// Asserts that the given value is equal to the value for a specific cell in the first Excel sheet found.
        /// </summary>
        [StringFormatMethod("message")]
        public void AreEqual(object expectedValue, string columnName, int rowIndex = 0, string message = null,
            params object[] formatParams)
        {
            AreEqual(expectedValue, GetFirstSheetName(), columnName, rowIndex, message, formatParams);
        }

        /// <summary>
        /// Asserts that the only sheet in the excel file has a column by a given name.
        /// </summary>
        public void ContainsColumn(string columnName)
        {
            ContainsColumn(GetFirstSheetName(), columnName);
        }

        /// <summary>
        /// Asserts that the matching sheet has a column by a given name.
        /// </summary>
        public void ContainsColumn(string sheetName, string columnName)
        {
            var names = GetColumnNamesAndIndex(sheetName);
            if (!names.ContainsKey(columnName))
            {
                var columns = string.Join(", ", names.Keys);
                Assert.Fail("Unable to find column '{0}' in sheet '{1}'. Did find these columns: {2}", columnName,
                    sheetName, columns);
            }
        }

        /// <summary>
        /// Asserts that the only sheet in the excel file does not have a column by a given name.
        /// </summary>
        public void DoesNotContainColumn(string columnName)
        {
            DoesNotContainColumn(GetFirstSheetName(), columnName);
        }

        /// <summary>
        /// Asserts that the named sheet in the excel file does not have a column by a given name.
        /// </summary>
        public void DoesNotContainColumn(string sheetName, string columnName)
        {
            if (GetColumnNamesAndIndex(sheetName).ContainsKey(columnName))
            {
                Assert.Fail("Unexpectedly found column '{0}' in sheet '{1}'.", columnName, sheetName);
            }
        }

        /// <summary>
        /// Asserts that the given value is null for a specific cell in the Excel sheet.
        /// </summary>
        [StringFormatMethod("message")]
        public void IsNull(string sheet, string columnName, int rowIndex = 0, string message = null,
            params object[] formatParams)
        {
            AssertEquality(null, sheet, columnName, rowIndex, message, formatParams);
        }

        /// <summary>
        /// Asserts that the given value is null for a specific cell in the first Excel sheet found.
        /// </summary>
        [StringFormatMethod("message")]
        public void IsNull(string columnName, int rowIndex = 0, string message = null, params object[] formatParams)
        {
            IsNull(GetFirstSheetName(), columnName, rowIndex, message, formatParams);
        }

        #endregion

        public void Dispose()
        {
            try
            {
                if (!_isDisposed)
                {
                    _package?.Dispose();
                    File.Delete(ExcelResult.FileName);
                }
            }
            finally
            {
                _isDisposed = true;
            }
        }

        #endregion
    }
}
