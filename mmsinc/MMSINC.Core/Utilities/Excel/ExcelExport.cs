using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using OfficeOpenXml;

namespace MMSINC.Utilities.Excel
{
    ///<summary>Exports a collection of tables to an Excel spreadsheet.</summary>
    public class ExcelExport
    {
        #region Properties

        ///<summary>Gets the sheets which will be exported by this instance.</summary>
        public Collection<IExcelSheet> Sheets { get; }

        #endregion

        #region Constructors

        ///<summary>Creates a new ExcelExport instance.</summary>
        public ExcelExport()
        {
            Sheets = new Collection<IExcelSheet>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds a new worksheet to the export with the given data and optional arguments.
        /// </summary>
        /// <typeparam name="TRow"></typeparam>
        /// <param name="items">The data/rows to export to the worksheet.</param>
        /// <param name="args">Optional arguments for how the worksheet should be created.</param>
        /// <returns></returns>
        public ExcelExport AddSheet<TRow>(IEnumerable<TRow> items, ExcelExportSheetArgs args = null)
        {
            args = args ?? new ExcelExportSheetArgs();

            if (items is IEnumerable<IDictionary<string, object>> dictItems)
            {
                Sheets.Add(new DictionarySheet(args.SheetName, dictItems, args.ExportableProperties, args.Header));
            }
            else
            {
                Sheets.Add(new TypedSheet<TRow>(args.SheetName, items, args.ExportableProperties, args.Header));
            }

            return this;
        }

        ///<summary>Exports all of the added sheets to an Excel file.</summary>
        ///<param name="fileName">The filename to export to.  The file type is inferred from the extension.</param>
        public void ExportTo(string fileName, bool autoFit = true)
        {
            ExportTo(fileName, ExcelUtility.GetDBType(Path.GetExtension(fileName)), autoFit);
        }

        ///<summary>Exports all of the added sheets to an Excel file.</summary>
        public void ExportTo(string fileName, ExcelFormat format, bool autoFit = true)
        {
            // NOTE: If this is taking a long time to export, make sure you aren't
            //       lazyloading a ton of entities. That significantly slows down
            //       exports with large numbers of items.
            var fileInfo = new FileInfo(fileName);
            using (var package = new ExcelPackage(fileInfo))
            {
                foreach (var sheet in Sheets)
                {
                    sheet.Export(package, autoFit);
                }

                package.Save();
            }
        }

        /// <summary>
        /// Exports the Excel file directly to a stream.
        /// </summary>
        /// <param name="outputStream"></param>
        public void ExportTo(Stream outputStream)
        {
            using (var package = new ExcelPackage())
            {
                foreach (var sheet in Sheets)
                {
                    sheet.Export(package);
                }

                package.SaveAs(outputStream);
            }
        }

        #endregion
    }
}
