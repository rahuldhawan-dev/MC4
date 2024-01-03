using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Presenters
{
    public class EmployeeResourceRPCPresenter : ResourceRPCPresenter<Employee>
    {
        #region Constructors

        public EmployeeResourceRPCPresenter(IResourceRPCView<Employee> view, IRepository<Employee> repository)
            : base(view, repository) { }

        #endregion
    }
}
