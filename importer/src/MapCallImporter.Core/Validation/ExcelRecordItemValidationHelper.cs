using System.Collections.Generic;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using MMSINC.Data;
using IUnitOfWork = MMSINC.Data.V2.IUnitOfWork;

namespace MapCallImporter.Validation
{
    /// <summary>
    /// Adds an issue to the calling validator's collection when any requirements fail.
    /// </summary>
    public class ExcelRecordItemValidationHelper<TEntity, TViewModel, TExcelRecord> : ExcelRecordItemHelperBase<TEntity>
        where TEntity : class, new()
        where TViewModel : ViewModel<TEntity>
        where TExcelRecord : ExcelRecordBase<TEntity, TViewModel, TExcelRecord>
    {
        #region Private Members

        protected readonly ExcelFileValidator<TEntity, TViewModel, TExcelRecord> _validator;

        #endregion

        #region Public Properties

        public override IEnumerable<string> LastErrors => _validator?.LastIssues;

        #endregion

        #region Constructors

        public ExcelRecordItemValidationHelper(ExcelFileValidator<TEntity, TViewModel, TExcelRecord> validator)
        {
            _validator = validator;
        }

        #endregion

        #region Private Methods

        protected override void OnFailedRequirement(string message)
        {
            _validator.AddIssue(message);
        }

        protected override TRef LoadEntityRef<TRef>(int id, IUnitOfWork uow)
        {
            return uow.Find<TRef>(id);
        }

        #endregion
    }
}