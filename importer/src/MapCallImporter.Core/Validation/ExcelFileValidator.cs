using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Library.Excel;
using MMSINC.Data;
using MMSINC.Utilities.Excel;
using OfficeOpenXml;
using IUnitOfWorkFactory = MMSINC.Data.V2.IUnitOfWorkFactory;

namespace MapCallImporter.Validation
{
    public class ExcelFileValidator<TEntity, TViewModel, TExcelRecord> : ExcelFileHandlerBase<TExcelRecord>, IExcelFileValidator
        where TEntity : class, new()
        where TViewModel : ViewModel<TEntity>
        where TExcelRecord : ExcelRecordBase<TEntity, TViewModel, TExcelRecord>
    {
        #region Constructors

        public ExcelFileValidator(IUnitOfWorkFactory uowFactory, IExcelImportFactory excelImportFactory) : base(uowFactory, excelImportFactory) { }

        #endregion

        #region Private Methods

        protected virtual ExcelFileValidationAndMappingResult HandleContent(ExcelImport<TExcelRecord> import)
        {
            var entityName = typeof(TEntity).Name;
            UpdateProgress(0, $"Beginning validation of {entityName} file...");
            var entities = new List<TEntity>();

            using (var uow = UnitOfWorkFactory.BuildMemoized())
            {
                List<ItemWithIndex<TExcelRecord>> items;

                try
                {
                    var unindexedItems = import.GetItems();
                    items = unindexedItems.GetWithIndex(2).ToList();
                }
                catch (Exception e)
                {
                    _issues.Add(e.ToString());
                    return new ExcelFileValidationAndMappingResult(ExcelFileProcessingResult.InvalidFileContents,
                        typeof(TExcelRecord), LastIssues);
                }

                var helper = new ExcelRecordItemValidationHelper<TEntity, TViewModel, TExcelRecord>(this);

                foreach ((int index, TExcelRecord item) in items)
                {
                    UpdateProgress((index - 1) * 100 / (decimal)items.Count, $"Validating {entityName} at row {index}...");

                    entities.Add(item.MapAndValidate(uow, index, helper));

                    if (index % 25 == 0)
                    {
                       uow.Flush();
                    }
                }

                // no sense in saving any changes during validation
               uow.Rollback();
            }

            return new ExcelFileValidationAndMappingResult(LastIssues.Any()
                ? ExcelFileProcessingResult.InvalidFileContents
                : ExcelFileProcessingResult.FileValid, typeof(TExcelRecord), LastIssues, entities);
        }

        #endregion

        #region Exposed Methods

        public virtual ExcelFileValidationAndMappingResult Handle(byte[] fileBytes)
        {
            _issues = new List<string>();
            using (var stream = new MemoryStream(fileBytes))
            using (var package = new ExcelPackage(stream))
            {
                return HandleContent(ExcelImportFactory.Build<TExcelRecord>(package));
            }
        }

        #endregion
    }
}
