using System;
using MMSINC.Interface;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Views
{
    public interface IEmployeeDetailView : IDetailView<Employee>
    {
        #region Properties

        IChildResourceView<Employee> EmployeeChildResourceView { get; }

        #endregion

        #region Events

        event EventHandler MenuEmployeeClicked,
                           MenuEmployeesClicked,
                           MenuTerritoriesClicked,
                           MenuOrdersClicked,
                           MenuItemClicked;

        #endregion

        #region Exposed Methods

        void ToggleControl(string control, bool visible);
        void ToggleControl(string control, bool visible, bool callHideControls);

        #endregion
    }
}
