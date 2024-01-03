using MapCallImporter.Common;
using MapCallImporter.Library;
using System.Diagnostics;
using System.Linq;

namespace MapCallImporter.Importing
{
    public class ExcelFileImportingService : ExcelFileHandlerServiceBase<ExcelFileImporterFinderService>
    {
        #region Constructors

        public ExcelFileImportingService(ExcelFileImporterFinderService finderService) : base(finderService) {}

        #endregion

        #region Exposed Methods

        public virtual TimedExcelFileMappingResult Handle(ExcelFileValidationAndMappingResult lastResult)
        {
            var stopwatch = Stopwatch.StartNew();
            var handler = (IExcelFileImporter)FinderService.GetHandler(lastResult);
            OnProgressChanged(this, new ProgressAndStatusChangedArgs(0, "Using cached mapped entities, skipping file read."));
            
            handler.ProgressChanged += OnProgressChanged;

            var result = handler.Handle(lastResult);

            _issues = result.Issues.ToList();
            stopwatch.Stop();

            return new TimedExcelFileMappingResult(result, stopwatch.ElapsedMilliseconds);
        }

        #endregion
    }
}
