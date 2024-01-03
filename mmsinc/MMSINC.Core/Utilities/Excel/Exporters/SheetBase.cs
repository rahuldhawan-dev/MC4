using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using NHibernate.Mapping;
using OfficeOpenXml;
using OfficeOpenXml.Style;

// ReSharper disable CheckNamespace
namespace MMSINC.Utilities.Excel
    // ReSharper restore CheckNamespace
{
    ///<summary>Stores a single exportable worksheet.</summary>
    public interface IExcelSheet
    {
        #region Properties

        ///<summary>Gets the name of the worksheet.</summary>
        string Name { get; }

        #endregion

        void Export(ExcelPackage excelPackage, bool autoFit = true);
    }

    public interface IExcelSheet<TRow> : IExcelSheet
    {
        ///<summary>Gets a collection of objects containing the data to show in each row.</summary>
        IEnumerable<TRow> Items { get; }

        ///<summary>Gets the columns to put in the sheet.</summary>
        IEnumerable<ColumnInfo> Columns { get; }
    }

    ///<summary>A reusable base class that exports data provided by a derived class to an excel spreadsheet.</summary>
    ///<typeparam name="TRow">The type of the objects used by the implementation to represent each row.</typeparam>
    public abstract class SheetBase<TRow> : IExcelSheet<TRow>
    {
        #region Properties

        ///<summary>Gets the name of the exported worksheet.</summary>
        public string Name { get; private set; }

        ///<summary>Gets a collection of objects containing the data to show in each row.</summary>
        public IEnumerable<TRow> Items { get; protected set; }

        ///<summary>Gets the columns to put in the sheet.</summary>
        public IEnumerable<ColumnInfo> Columns { get; protected set; }

        ///<summary>Text to add as the top row of the excel file</summary>
        public string Header { get; protected set; }

        #endregion

        #region Constructors

        ///<summary>Creates a SheetBase instance.</summary>
        protected SheetBase(string name)
        {
            Name = name;
        }

        #endregion

        #region Private Methods

        ///<summary>Gets the column values for a specific row object.  (in the order returned by GetColumns())</summary>
        protected abstract IEnumerable<object> GetValues(TRow row);

        #endregion

        public void Export(ExcelPackage excelPackage, bool autoFit = true)
        {
            // Without a format, Excel will display dates as raw doubles.
            const string dateFormat = "M/d/yyyy h:mm:ss AM/PM", shortDateFormat = "M/d/yyyy";
            // NOTE: Excel is 1-based rather than 0-based.
            var workSheet = excelPackage.Workbook.Worksheets.Add(Name);
            var colIndex = 1;

            // Add headers
            foreach (var col in Columns)
            {
                var cell = workSheet.Cells[1, colIndex];
                cell.Value = col.ColumnName;
                cell.Style.Font.Bold = true;
                colIndex++;
            }

            // Start at 2 since the first row is the header row.
            var rowCount = 2;

            foreach (var row in Items)
            {
                colIndex = 1;
                foreach (var value in GetValues(row))
                {
                    var cell = workSheet.Cells[rowCount, colIndex];
                    cell.Value = value;
                    if (value is DateTime)
                    {
                        cell.Style.Numberformat.Format =
                            value.ToString().EndsWith("12:00:00 AM") ? shortDateFormat : dateFormat;
                    }

                    colIndex++;
                }

                rowCount++;
            }

            if (!String.IsNullOrWhiteSpace(Header))
            {
                workSheet.InsertRow(1, 1);
                workSheet.InsertRow(1, 1);
                var cells = workSheet.Cells[1, 1, 1, Columns.Count()];
                cells.Merge = true;
                cells.Value = Header;
            }

            // Dimension will be null if the sheet does not have any cells.
            if (autoFit && workSheet.Dimension != null)
            {
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
            }
        }
    }

    ///<summary>Stores information about a column in an exported sheet.</summary>
    public class ColumnInfo
    {
        ///<summary>Gets the name of the column as displayed in the column header.</summary>
        public string ColumnName { get; protected set; }

        protected ColumnInfo() { }

        ///<summary>Creates a new ColumnInfo instance.</summary>
        public ColumnInfo(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
