using MapCallImporter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Library.Excel;
using MMSINC.Data;
using IUnitOfWorkFactory = MMSINC.Data.V2.IUnitOfWorkFactory;

namespace MapCallImporter.Importing
{
    public class ExcelFileImporter<TEntity, TViewModel, TExcelModel> : ExcelFileHandlerBase<TExcelModel>, IExcelFileImporter
        where TEntity : class, new()
        where TViewModel : ViewModel<TEntity>
        where TExcelModel : ExcelRecordBase<TEntity, TViewModel, TExcelModel>
    {
        #region Constants

        public const int BATCH_SIZE = 25,
            MAX_PROGRESS_PERCENTAGE = 5;

        #endregion

        #region Constructors

        public ExcelFileImporter(IUnitOfWorkFactory uowFactory, IExcelImportFactory excelImportFactory) : base(uowFactory, excelImportFactory) {}

        #endregion

        #region Exposed Methods

        public virtual ExcelFileValidationAndMappingResult Handle(ExcelFileValidationAndMappingResult lastResult)
        {
            _issues = new List<string>();
            var entities = ((IEnumerable<TEntity>)lastResult.Entities).ToArray();
            var total = entities.Count();

            using (var uow = UnitOfWorkFactory.Build())
            {
                uow.Container.GetInstance<TExcelModel>().PreImport(entities);

                foreach ((int index, TEntity entity) in entities.GetWithIndex(2))
                {
                    UpdateProgress((index - 1).Scale(total, 100),
                        $"Importing {typeof(TEntity).Name} at row {index}...");

                    try
                    {
                        uow.Container.GetInstance<TExcelModel>().InsertEntity(uow, entity);
                    }
                    catch (Exception e)
                    {
                        AddIssue($"{typeof(TEntity).Name} at row {index} could not properly be saved: {e}");
                        uow.Rollback();
                        return new ExcelFileValidationAndMappingResult(ExcelFileProcessingResult.InvalidFileContents,
                            null, _issues);
                    }

                    if (index % 25 == 0)
                    {
                        uow.Flush();
                    }
                }

                uow.Commit();
                return new ExcelFileValidationAndMappingResult(ExcelFileProcessingResult.FileValid, typeof(TExcelModel),
                    entities: lastResult.Entities);
            }
        }

        #endregion
    }
}
