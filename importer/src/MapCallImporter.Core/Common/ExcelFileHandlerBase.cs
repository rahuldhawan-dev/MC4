using System.Collections.Generic;
using System.Runtime.InteropServices;
using MapCallImporter.Library;
using MapCallImporter.Library.Excel;
using MMSINC.Data.V2;
using OfficeOpenXml;

namespace MapCallImporter.Common
{
    public abstract class ExcelFileHandlerBase<TExcelRecord> : ExcelFileHandlerBase
    {
        #region Properties

        public virtual IUnitOfWorkFactory UnitOfWorkFactory { get; }
        public virtual IExcelImportFactory ExcelImportFactory { get; }

        #endregion

        #region Constructors

        public ExcelFileHandlerBase(IUnitOfWorkFactory uowFactory, IExcelImportFactory excelFactory)
        {
            UnitOfWorkFactory = uowFactory;
            ExcelImportFactory = excelFactory;
        }

        #endregion

        #region Exposed Methods

        public override void AddIssue(string issue)
        {
            _issues = _issues ?? new List<string>();
            _issues.Add(issue);
        }

        public override bool CanHandle(ExcelPackage package, ref ExcelFileProcessingResult openResult)
        {
            return ExcelImportFactory.Build<TExcelRecord>(package).ColumnHeadersMatch();
        }

        #endregion
    }

    public abstract class ExcelFileHandlerBase : IExcelFileHandler
    {
        #region Private Members

        protected IList<string> _issues;

        #endregion

        #region Properties

        public virtual IEnumerable<string> LastIssues => _issues;

        #endregion

        #region Private Methods

        protected void InvokeProgressChanged(decimal progress, string status)
        {
            ProgressChanged?.Invoke(this, new ProgressAndStatusChangedArgs(progress, status));
        }

        protected virtual void UpdateProgress(decimal currentProgress, string status)
        {
            InvokeProgressChanged(currentProgress, status);
        }

        #endregion

        #region Abstract Methods

        public abstract bool CanHandle(ExcelPackage package, ref ExcelFileProcessingResult openResult);
        public abstract void AddIssue(string issue);

        #endregion

        #region Events/Delegates

        public event ProgressChangedEventHandler ProgressChanged;

        #endregion
    }
}
