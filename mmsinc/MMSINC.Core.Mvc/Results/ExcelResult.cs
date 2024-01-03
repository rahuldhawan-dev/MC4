using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using MMSINC.Utilities.Excel;

namespace MMSINC.Results
{
    // This is based off of this https://gist.github.com/SLaks/3044898
    // But cleaned up a bit and removed some methods we aren't using.

    public class ExcelResult : FilePathResult, ITemporaryFileResult
    {
        #region Fields

        private int _sheetCount = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ExcelExport instance used by this result.
        /// </summary>
        public ExcelExport Exporter { get; private set; }

        ///<summary>Gets the Excel format that will be sent to the client.</summary>
        public ExcelFormat Format { get; private set; }

        #region Testing related properties

        public bool HasExecuted { get; private set; }

        /// <summary>
        /// Set this to true if you don't want the file to be deleted after executing.
        /// This should only be used by ExcelResultTester.
        /// </summary>
        [Obsolete("If the DeleteTemporaryFileFilter works properly, this can be removed.")]
        public bool IsInTestMode { get; set; }

        public bool Autofit { get; set; } = true;

        #endregion

        #endregion

        #region Constructors

        ///<summary>Creates an ExcelResult that sends an Excel spreadsheet with the specified filename and format.</summary>
        public ExcelResult(string name) //OleDb fails if the extension doesn't match...
            : base(GetFileName(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            Exporter = new ExcelExport();
            FileDownloadName = name + ".xlsx";
        }

        public ExcelResult() : this("export") { }

        #endregion

        #region Private Methods

        private static string GetFileName()
        {
            const string extension = "xlsx";
            var path = MMSINC.Utilities.FileIO.GetRandomTemporaryFileName();

            // NOTE: The library that exports to excel generates an invalid file if
            //       the extension isn't correct. It doesn't throw an error, but you
            //       get an error when opening the file in Excel.
            path = Path.ChangeExtension(path, extension);
            return path;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new worksheet to the excel export with the given data used for rows.
        /// </summary>
        /// <typeparam name="TRow"></typeparam>
        /// <param name="items">The data to export.</param>
        /// <param name="args">Optional parameters for how the worksheet should be created.</param>
        /// <returns></returns>
        public ExcelResult AddSheet<TRow>(IEnumerable<TRow> items, ExcelExportSheetArgs args = null)
        {
            args = args ?? new ExcelExportSheetArgs();
            if (string.IsNullOrWhiteSpace(args.SheetName))
            {
                _sheetCount++;
                args.SheetName = "Sheet" + _sheetCount;
            }

            Exporter.AddSheet(items, args);
            return this;
        }

        ///<summary>Creates the Excel file and sends it to the client.</summary>
        public override void ExecuteResult(ControllerContext context)
        {
            if (!HasExecuted)
            {
                Exporter.ExportTo(FileName, Format, Autofit);
                base.ExecuteResult(context);
            }

            HasExecuted = true;
        }

        protected override void WriteFile(System.Web.HttpResponseBase response)
        {
            base.WriteFile(response);
            // The base method calls response.TransmitFile. Something later in the ASP
            // pipeline does the actual sending of the file, but it's after this WriteFile
            // method has completed. Flushing the response forces the file to be sent to the 
            // client so we can properly delete the temporary file we've created.

            if (response.IsClientConnected)
            {
                // Only flush if the client is still connected. Otherwise we just get useless
                // "The remote host closed the connection" error emails. This mostly gets triggered
                // when someone double clicks an export link, or they're impatient and hit it
                // a second time before the first request completes.
                response.Flush();
            }

            // ExcelResultTester relies on this to prevent the file from being deleted.
            if (!IsInTestMode)
            {
                //// Make sure to delete the file or else the temp directory gets littered with 
                //// files and that causes errors.
                //File.Delete(FileName);
            }
        }

        public void DeleteTemporaryFiles()
        {
            // Make sure to delete the file or else the temp directory gets littered with 
            // files and that causes errors. This method gets called by the DeleteTemporaryFileFilter
            File.Delete(FileName);
        }

        #endregion
    }
}
