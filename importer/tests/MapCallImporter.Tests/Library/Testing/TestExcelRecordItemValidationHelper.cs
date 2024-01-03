using System.Data;
using MapCallImporter.Common;
using MapCallImporter.Validation;
using MMSINC.Data;

namespace MapCallImporter.Library.Testing
{
    // the validation helper uses find instead of load so we can test reference validity,
    // but we need to throw DataException like the import helper does
    public class TestExcelRecordItemValidationHelper<TEntity, TViewModel, TExcelRecord> : ExcelRecordItemValidationHelper<TEntity, TViewModel, TExcelRecord>
        where TEntity : class, new()
        where TViewModel : ViewModel<TEntity>
        where TExcelRecord : ExcelRecordBase<TEntity, TViewModel, TExcelRecord>
    {
        #region Constructors

        public TestExcelRecordItemValidationHelper() : base(null) { }

        #endregion

        #region Private Methods

        protected override void OnFailedRequirement(string message)
        {
            throw new DataException(message);
        }

        #endregion
    }
}