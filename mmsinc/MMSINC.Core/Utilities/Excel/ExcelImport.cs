using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MMSINC.ClassExtensions.TypeExtensions;
using OfficeOpenXml;
using StructureMap;

namespace MMSINC.Utilities.Excel
{
    public class ExcelImport<T> : IDisposable
    {
        #region Constants

        // Seriously, if the header can't be found by the tenth row then someone
        // has made a terrible excel file(more terrible than other excel files).
        private const int MAX_ROWS_TO_SCAN_FOR_HEADERS = 10;

        #endregion

        #region Fields

        // ReSharper disable once NotAccessedField.Local
        private static readonly List<PropertyInfo> _properties = typeof(T).GetProperties().ToList();

        private bool _isDisposed;
        private int _headerRowIndex;

        #endregion

        #region Properties

        public ExcelPackage Package { get; protected set; }
        public MemoryStream PackageMemoryStream { get; }

        public virtual IContainer Container { get; set; }

        protected virtual int MaxRowScan => MAX_ROWS_TO_SCAN_FOR_HEADERS;

        #endregion

        #region Constructors

        public ExcelImport(IContainer container, byte[] fileData)
        {
            Container = container;
            if (fileData != null)
            {
                PackageMemoryStream = new MemoryStream(fileData);
                Package = new ExcelPackage(PackageMemoryStream);
            }
        }

        #endregion

        #region Private Methods

        private ExcelWorksheet GetWorkSheet(string tableName = null)
        {
            var sheets = Package.Workbook.Worksheets;
            ExcelWorksheet returnSheet = null;

            if (string.IsNullOrWhiteSpace(tableName))
            {
                // Some of the excel files NJAW send us do not have predictable sheet names. 
                // In this case we use the first sheet.
                returnSheet = sheets.FirstOrDefault();
            }
            else
            {
                returnSheet = sheets.SingleOrDefault(x => x.Name == tableName);
            }

            if (returnSheet == null)
            {
                throw new InvalidOperationException("A sheet with the name '" + tableName + "' could not be found.");
            }

            return returnSheet;
        }

        private static object ConvertValueToType(object value, Type type)
        {
            // if (value == DBNull.Value)
            if (value == null)
            {
                return null;
            }

            if (type.IsNullable())
            {
                type = Nullable.GetUnderlyingType(type);

                if (value is string s && string.IsNullOrWhiteSpace(s))
                {
                    return null;
                }
            }

            if (type == typeof(string))
            {
                return value.ToString();
            }

            object converted = null;
            if (value is double d && type == typeof(DateTime))
            {
                converted = DateTime.FromOADate(d);
            }
            else
            {
                if (value is string s)
                {
                    if (double.TryParse(s, out var dvalue))
                    {
                        value = dvalue;
                    }
                }

                converted = Convert.ChangeType(value, type);
            }

            if (converted is string)
            {
                // Columns that aren't left-justified always end up with random whitespace
                // at the beginning and end.
                converted = converted.ToString().Trim();
            }

            return converted;
        }

        private static string FormatColumnName(string actualColumnName)
        {
            var foo = typeof(T).Name;

            return new[] {
                " ", ".", "#", "/", "'", "(", ")", "-", "?", "%", "&"
            }.Aggregate(actualColumnName?.Trim(), (current, toReplace) => current.Replace(toReplace, string.Empty));
        }

        private ExcelColumnHeaderResult GetColumnNamesAndIndex(ExcelWorksheet worksheet)
        {
            var result = new ExcelColumnHeaderResult();
            var expectedColumnNames = _properties.Select(x => x.Name).ToArray();
            var i = 0;

            foreach (var row in GetRowValues(worksheet))
            {
                if (i++ == MaxRowScan)
                {
                    break;
                }

                var expectedColumnNamesExceptThoseThatExist =
                    expectedColumnNames.Except(row.Select(x => FormatColumnName(x.Text))).ToList();
                var isHeaderColumn = !expectedColumnNamesExceptThoseThatExist.Any();
                if (!isHeaderColumn)
                {
                    var hasSomeButNotAllOfTheHeaders =
                        expectedColumnNamesExceptThoseThatExist.Count < expectedColumnNames.Count();
                    if (hasSomeButNotAllOfTheHeaders)
                    {
                        // Stop scanning for header rows. This indicates we've found a row with some of the
                        // expected column names but not all of them. If we're getting a file that has an
                        // invalid header row followed by a valid header row then that file needs to be recreated
                        // in a manner that makes sense. -Ross 3/15/2018
                        var missingColumns = string.Join(", ", expectedColumnNamesExceptThoseThatExist);
                        result.ValidationResult =
                            new ValidationResult(
                                $"Unable to find the following columns in the header row: {missingColumns}.");
                        break;
                    }
                }
                else
                {
                    // Setting this row index here kinda smells. -Ross 8/4/2016
                    _headerRowIndex = row.Start.Row;
                    var dict = new Dictionary<string, ExcelColumnWithIndex>();

                    foreach (var cell in row)
                    {
                        var columnName = FormatColumnName(cell.Text);

                        if (dict.ContainsKey(columnName))
                        {
                            Debug.Print("A column has already been mapped with the name: " +
                                        columnName);
                        }
                        else
                        {
                            dict.Add(columnName,
                                new ExcelColumnWithIndex(worksheet.Column(cell.Columns), cell.Start.Column));
                        }
                    }

                    result.ColumnsByName = dict;
                    break; // Break out so we aren't continuing to scan more rows
                }
            }

            if (result.ColumnsByName == null && result.ValidationResult == null)
            {
                result.ValidationResult =
                    new ValidationResult("Unable to find a row that includes all the expected column headers.");
            }

            return result;
        }

        /// <summary>
        /// Returns an ExcelRange for each row that has at least one non-null value.
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="startingRow"></param>
        /// <returns></returns>
        private IEnumerable<ExcelRange> GetRowValues(ExcelWorksheet worksheet, int? startingRow = null)
        {
            var startRow = startingRow.HasValue ? startingRow.Value : worksheet.Dimension.Start.Row;
            var endRow = worksheet.Dimension.End.Row + 1;
            var startColumn = worksheet.Dimension.Start.Column;
            var endColumn = worksheet.Dimension.End.Column;

            for (var i = startRow; i < endRow; i++)
            {
                // We need to ignore rows that have no values in them because NJAW likes to give us
                // all sorts of randomly formatted files.

                var values = worksheet.Cells[i, startColumn, i, endColumn];
                if (values.Any()) // If this is false then the row is empty of any values.
                {
                    yield return values;
                }
            }
        }

        protected virtual T GetInstance()
        {
            return Container.GetInstance<T>();
        }

        #endregion

        #region Public Methods

        public bool ColumnHeadersMatch(string tableName = null)
        {
            var worksheet = GetWorkSheet(tableName);
            var columnsByFormattedName = GetColumnNamesAndIndex(worksheet);

            if (columnsByFormattedName.ValidationResult != null)
            {
                // Validation error occurred, so column headers didn't match.
                return false;
            }

            var columnNames = columnsByFormattedName
                             .ColumnsByName.Select(c => c.Key).Where(x => x != string.Empty).ToList();
            var propertyNames = _properties.Select(p => p.Name).ToArray();

            return columnNames.Count() == propertyNames.Length && columnNames.All(col => propertyNames.Contains(col));
        }

        public virtual void Dispose()
        {
            try
            {
                if (!_isDisposed)
                {
                    Package.Dispose();
                    PackageMemoryStream.Dispose();
                }
            }
            finally
            {
                _isDisposed = true;
            }
        }

        #region GetItems/TryGetItems

        public IEnumerable<T> GetItems(string tableName = null)
        {
            var result = TryGetImport(tableName);
            if (result.ValidationResults.Any())
            {
                throw new InvalidOperationException(result.ValidationResults.First().ErrorMessage);
            }

            foreach (var item in result.Results)
            {
                if (item.ValidationResults.Any())
                {
                    throw new InvalidOperationException(item.ValidationResults.First().ErrorMessage);
                }
            }

            return result.Results.Select(x => x.ConvertedItem);
        }

        /// <summary>
        /// Attempts to convert all items in the Excel worksheet to the required type. Any
        /// conversion errors will be returned as ValidationResults.
        /// </summary>
        /// <param name="validationResults"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ExcelImportResult<T> TryGetImport(string tableName = null)
        {
            var importValidationResults = new List<ValidationResult>();
            var importRowResults = new List<ExcelImportRowResult<T>>();
            var importResult = new ExcelImportResult<T> {
                ValidationResults = importValidationResults,
                Results = importRowResults
            };
            var worksheet = GetWorkSheet(tableName);

            // If this fails, get validation result and return ImportResult
            var columnsByFormattedName = GetColumnNamesAndIndex(worksheet);
            if (columnsByFormattedName.ValidationResult != null)
            {
                importValidationResults.Add(columnsByFormattedName.ValidationResult);
                return importResult;
            }

            //var propColumns = new Dictionary<PropertyInfo, ExcelColumnWithIndex>();
            var propColumns = _properties.ToDictionary(x => x, x => columnsByFormattedName.ColumnsByName[x.Name]);

            // Using row index, rather than zero-based item count, because these validation
            // errors are more for the user uploading the file. They won't know that 0 means 2.
            var curRowIndex = _headerRowIndex + 1;
            foreach (var row in GetRowValues(worksheet, _headerRowIndex + 1))
            {
                var result = new ExcelImportRowResult<T>();
                result.Row = curRowIndex;
                result.ConvertedItem = Container.GetInstance<T>();
                var validationResults = new List<ValidationResult>();
                result.ValidationResults = validationResults;

                foreach (var prop in propColumns)
                {
                    var value = worksheet.Cells[row.Start.Row, prop.Value.Index].Value; //row[column];

                    object converted = null;

                    try
                    {
                        converted = ConvertValueToType(value, prop.Key.PropertyType);
                    }
                    catch (Exception)
                    {
                        validationResults.Add(new ValidationResult(
                            $"Row[{curRowIndex}]: Property \"{prop.Key.Name}\": Unable to convert \"{value}\" to type {prop.Key.PropertyType}.",
                            new[] {prop.Key.Name}));
                    }

                    prop.Key.SetValue(result.ConvertedItem, converted, null);
                }

                importRowResults.Add(result);
                curRowIndex++;
            }

            return importResult;
        }

        #endregion

        #endregion

        #region Helper classes

        private class ExcelColumnWithIndex
        {
            #region Properties

            public ExcelColumn Column { get; private set; }
            public int Index { get; private set; }

            #endregion

            #region Constructors

            public ExcelColumnWithIndex(ExcelColumn column, int index)
            {
                Column = column;
                Index = index;
            }

            #endregion
        }

        private class ExcelColumnHeaderResult
        {
            public ValidationResult ValidationResult { get; set; }
            public Dictionary<string, ExcelColumnWithIndex> ColumnsByName { get; set; }
        }

        #endregion
    }

    public sealed class ExcelImportResult<T>
    {
        /// <summary>
        /// Returns any validation errors that may have occurred prior to importing the results. This 
        /// includes missing column headers and similar errors.
        /// 
        /// This does not include validation errors for each individual row.
        /// </summary>
        public IEnumerable<ValidationResult> ValidationResults { get; set; }

        /// <summary>
        /// The results for each individual row in the results.
        /// </summary>
        public IEnumerable<ExcelImportRowResult<T>> Results { get; set; }
    }

    public sealed class ExcelImportRowResult<T>
    {
        #region Properties

        /// <summary>
        /// The object created from an Excel row. This item is not guaranteed to be
        /// valid, so check the ValidationResults property first.
        /// </summary>
        public T ConvertedItem { get; set; }

        /// <summary>
        /// Returns any validation errors that occurred while converting the Excel row
        /// to the expected type.
        /// </summary>
        public IEnumerable<ValidationResult> ValidationResults { get; set; }

        /// <summary>
        /// The row number in the excel file that this result refers to.
        /// </summary>
        public int Row { get; set; }

        #endregion
    }
}
