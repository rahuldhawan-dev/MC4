using System.Collections.Generic;
using MapCallImporter.Library;

namespace MapCallImporter.Common
{
    public abstract class ExcelFileHandlerServiceBase<TFinderService> : ExcelFileHandlerServiceBase
        where TFinderService : ExcelFileHandlerFinderServiceBase
    {
        #region Properties

        public TFinderService FinderService { get; }

        #endregion

        #region Constructors

        public ExcelFileHandlerServiceBase(TFinderService finderService)
        {
            FinderService = finderService;
        }

        #endregion
    }

    public abstract class ExcelFileHandlerServiceBase
    {
        #region Private Members

        protected IList<string> _issues;

        #endregion

        #region Properties

        public virtual IEnumerable<string> LastIssues => _issues;

        #endregion

        #region Event Handlers

        protected void OnProgressChanged(object sender, ProgressAndStatusChangedArgs e)
        {
            ProgressChanged?.Invoke(sender, e);
        }

        #endregion

        #region Events/Delegates

        public event ProgressChangedEventHandler ProgressChanged;

        #endregion
    }
}
