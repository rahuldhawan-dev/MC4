using System;
using MMSINC.Common;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Views;

namespace MMSINCTestImplementation.Presenters
{
    public class EmployeeDetailPresenter : DetailPresenter<Employee>
    {
        #region Constants

        public struct Controls
        {
            public const string DETAIL = "pnlDetailsView";
            public const string C_EMPLOYEES = "crvEmployees";
            public const string C_TERRITORIES = "crvEmployeeTerritories";
            public const string C_ORDERS = "crvOrders";
            public const string MENU = "mnuEmployee";
        }

        #endregion

        #region Private Members

        private readonly IEmployeeDetailView _view;

        #endregion

        #region Properties

        public IEmployeeDetailView EmployeeDetailView
        {
            get { return _view; }
        }

        #endregion

        #region Constructors

        // NOTE: LEAVE THIS CONSTRUCTOR IN!!! YOU HAVE BEEN WARNED!
        public EmployeeDetailPresenter(IDetailView<Employee> view)
            : base(view) { }

        public EmployeeDetailPresenter(IEmployeeDetailView view)
            : base(view)
        {
            _view = view;
        }

        #endregion

        #region Event Passthroughs

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if (EmployeeDetailView != null)
            {
                EmployeeDetailView.MenuEmployeeClicked += View_MenuEmployeeClicked;
                EmployeeDetailView.MenuEmployeesClicked += View_MenuEmployeesClicked;
                EmployeeDetailView.MenuTerritoriesClicked +=
                    View_MenuTerritoriesClicked;
                EmployeeDetailView.MenuOrdersClicked += View_MenuOrdersClicked;
            }
        }

        #endregion

        #region Event Handlers

        protected virtual void View_MenuEmployeeClicked(object sender, EventArgs e)
        {
            EmployeeDetailView.ToggleControl(Controls.DETAIL, true, true);
        }

        protected virtual void View_MenuEmployeesClicked(object sender, EventArgs e)
        {
            EmployeeDetailView.ToggleControl(Controls.C_EMPLOYEES, true, true);
        }

        protected virtual void View_MenuTerritoriesClicked(object sender, EventArgs e)
        {
            EmployeeDetailView.ToggleControl(Controls.C_TERRITORIES, true, true);
        }

        protected virtual void View_MenuOrdersClicked(object sender, EventArgs e)
        {
            EmployeeDetailView.ToggleControl(Controls.C_ORDERS, true, true);
        }

        protected override void View_Inserting(object sender, EntityEventArgs<Employee> e)
        {
            //Check here just in case we are in a child detailview.
            if (EmployeeDetailView != null)
                EmployeeDetailView.ToggleControl(Controls.MENU, true, false);
            base.View_Inserting(sender, e);
        }

        #endregion
    }
}
