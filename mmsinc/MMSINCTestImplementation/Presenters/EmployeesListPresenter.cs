using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Presenters
{
    public class EmployeesListPresenter : ListPresenter<Employee>
    {
        #region Constructors

        public EmployeesListPresenter(IListView<Employee> view)
            : base(view) { }

        #endregion
    }
}
