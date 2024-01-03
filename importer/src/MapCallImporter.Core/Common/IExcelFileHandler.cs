using MapCallImporter.Library;
using OfficeOpenXml;

namespace MapCallImporter.Common
{
    public interface IExcelFileHandler
    {
        #region Abstract Methods

        bool CanHandle(ExcelPackage package, ref ExcelFileProcessingResult openResult);
        void AddIssue(string issue);

        #endregion

        #region Events/Delegates

        event ProgressChangedEventHandler ProgressChanged;

        #endregion
    }
}