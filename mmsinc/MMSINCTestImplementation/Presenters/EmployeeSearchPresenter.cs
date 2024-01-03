using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Presenters
{
    public class EmployeeSearchPresenter : SearchPresenter<Employee>
    {
        #region Constructors

        public EmployeeSearchPresenter(ISearchView<Employee> view)
            : base(view) { }

        #endregion
    }
}
