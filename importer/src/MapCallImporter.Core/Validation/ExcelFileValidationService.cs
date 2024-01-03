using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using MapCallImporter.Common;
using MapCallImporter.Library;

namespace MapCallImporter.Validation
{
    /// <summary>
    /// Service which handles validation of Excel files, regardless of contents.  A specific
    /// ExcelFileValidatorBase implementation will be selected based on the column headers of
    /// the file, which will then be used to validate the contents of the file.
    /// </summary>
    public class ExcelFileValidationService : ExcelFileHandlerServiceBase<ExcelFileValidatorFinderService>
    {
        #region Constructors

        public ExcelFileValidationService(ExcelFileValidatorFinderService finderService) : base(finderService) { }

        #endregion

        public event EventHandler FileOpen; 

        #region Exposed Methods

        public virtual TimedExcelFileMappingResult Handle(string filePath)
        {
            IExcelFileValidator handler;
            byte[] fileBytes;
            var stopwatch = Stopwatch.StartNew();

            TimedExcelFileMappingResult Fail(ExcelFileProcessingResult result, string message = null)
            {
                stopwatch.Stop();
                return new TimedExcelFileMappingResult(result, null, stopwatch.ElapsedMilliseconds,
                    string.IsNullOrWhiteSpace(message) ? null : new[] {message});
            }

            try
            {
                OnProgressChanged(this, new ProgressAndStatusChangedArgs(0, $"Opening file '{filePath}'..."));
                handler = (IExcelFileValidator)FinderService.GetHandler(fileBytes = File.ReadAllBytes(filePath));
                OnProgressChanged(this, new ProgressAndStatusChangedArgs(0, "Done!"));
                FileOpen?.Invoke(this, EventArgs.Empty);
            }
            catch (COMException e) when (e.Message.StartsWith("A disk error occurred during a write operation."))
            {
                return Fail(ExcelFileProcessingResult.InvalidFileType);
            }
            catch (IOException e) when (e.Message == $"The process cannot access the file '{filePath}' because it is being used by another process.")
            {
                return Fail(ExcelFileProcessingResult.FileAlreadyOpen);
            }
            catch (Exception e)
            {
                return Fail(ExcelFileProcessingResult.OtherError, e.ToString());
            }

            if (handler == null)
            {
                return Fail(ExcelFileProcessingResult.CouldNotDetermineContentType);
            }

            handler.ProgressChanged += OnProgressChanged;

            try
            {
                var res = handler.Handle(fileBytes);
                _issues = res.Issues.ToList();
                stopwatch.Stop();
                return new TimedExcelFileMappingResult(res, stopwatch.ElapsedMilliseconds);

            }
            catch (Exception e)
            {
                return Fail(ExcelFileProcessingResult.OtherError, e.ToString());
            }
        }

        #endregion
    }
}
