using MMSINC.Utilities.Excel;
using OfficeOpenXml;

namespace MapCallImporter.Library.Excel
{
    public interface IExcelImportFactory
    {
        ExcelImport<TEntity> Build<TEntity>(ExcelPackage package);
    }
}