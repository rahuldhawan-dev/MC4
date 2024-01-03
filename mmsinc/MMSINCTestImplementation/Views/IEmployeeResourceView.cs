using MMSINC.Interface;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Views
{
    public interface IEmployeeResourceView : IResourceView<Employee>
    {
        #region Properties

        IEmployeeDetailView EmployeeDetailView { get; }

        #endregion
    }
}
