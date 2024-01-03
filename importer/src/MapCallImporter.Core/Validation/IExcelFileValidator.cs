using MapCallImporter.Common;

namespace MapCallImporter.Validation
{
    public interface IExcelFileValidator : IExcelFileHandler
    {
        #region Abstract Methods

        ExcelFileValidationAndMappingResult Handle(byte[] fileBytes);

        #endregion
    }
}