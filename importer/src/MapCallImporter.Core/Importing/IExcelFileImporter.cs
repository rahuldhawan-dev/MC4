using MapCallImporter.Common;

namespace MapCallImporter.Importing
{
    public interface IExcelFileImporter : IExcelFileHandler
    {
        #region Abstract Methods

        ExcelFileValidationAndMappingResult Handle(ExcelFileValidationAndMappingResult lastResult);

        #endregion
    }
}