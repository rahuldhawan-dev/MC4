using MapCallImporter.Common;
using MapCallImporter.Importing;
using MapCallImporter.Validation;

namespace MapCallScheduler.JobHelpers.AssetUploadProcessor
{
    public class AssetUploadFileHandler : IAssetUploadFileHandler
    {
        #region Private Members

        private readonly ExcelFileValidationService _validationService;
        private readonly ExcelFileImportingService _importingService;

        #endregion

        #region Constructors

        public AssetUploadFileHandler(ExcelFileValidationService validationService,
            ExcelFileImportingService importingService)
        {
            _validationService = validationService;
            _importingService = importingService;
        }

        #endregion

        #region Exposed Methods

        public TimedExcelFileMappingResult Handle(string fileName)
        {
            var validationResult = _validationService.Handle(fileName);

            if (validationResult.Result != ExcelFileProcessingResult.FileValid)
            {
                return validationResult;
            }

            return _importingService.Handle(validationResult);
        }

        #endregion
    }
}