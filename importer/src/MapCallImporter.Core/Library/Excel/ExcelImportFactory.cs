using System;
using MMSINC.Utilities.Excel;
using OfficeOpenXml;
using StructureMap;

namespace MapCallImporter.Library.Excel
{
    public class ExcelImportFactory : FactoryBase, IExcelImportFactory
    {
        #region Constructors

        public ExcelImportFactory(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public ExcelImport<TEntity> Build<TEntity>(ExcelPackage package)
        {
            return (ExcelImport<TEntity>)_container.With(package).GetInstance(typeof(FasterExcelImport<TEntity>));
        }

        #endregion
    }
}