using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Views;

namespace MMSINCTestImplementation.Presenters
{
    public class EmployeeResourcePresenter : ResourcePresenter<Employee>
    {
        #region Private Members

        private readonly IEmployeeResourceView _view;

        #endregion

        #region Properties

        public IEmployeeResourceView EmployeeResourceView
        {
            get { return _view; }
        }

        #endregion

        #region Constructors

        // NOTE: LEAVE THIS CONSTRUCTOR IN!!! YOU HAVE BEEN WARNED!
        public EmployeeResourcePresenter(IChildResourceView<Employee> view, IRepository<Employee> repository)
            : base(view, repository) { }

        public EmployeeResourcePresenter(IEmployeeResourceView view, IRepository<Employee> repository)
            : base(view, repository)
        {
            _view = view;
        }

        #endregion

        #region Event Passthroughs

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if (EmployeeResourceView != null)
            {
                EmployeeResourceView.EmployeeDetailView.MenuItemClicked +=
                    DetailView_MenuItemClicked;
                foreach (var child in EmployeeResourceView.EmployeeDetailView.ChildResourceViews)
                {
                    child.ChildEvent += ChildResourceView_ChildEvent;
                }
            }
        }

        #endregion

        #region Event Handlers

        #region ChildResourceViews

        protected void ChildResourceView_ChildEvent(object sender, EventArgs e)
        {
            SetListViewData();
            SetRepositoryDataKeyFromListViewDataKey();
        }

        #endregion

        #region DetailView

        protected override void DetailView_DiscardChangesClicked(object sender, EventArgs e)
        {
            base.DetailView_DiscardChangesClicked(sender, e);
            if (DetailView.CurrentMode == DetailViewMode.Insert && SearchView != null && ListView != null)
            {
                ListView.SetListData(
                    Repository.GetFilteredSortedData(SearchView.GenerateExpression(),
                        ListView.SqlSortExpression));
            }
        }

        protected void DetailView_MenuItemClicked(object sender, EventArgs e)
        {
            SetListViewData();
            SetRepositoryDataKeyFromListViewDataKey();
        }

        #endregion

        #region ListView

        protected override void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            EmployeeResourceView.EmployeeDetailView.ToggleControl(
                EmployeeDetailPresenter.Controls.DETAIL, true, true);
            base.ListView_SelectedIndexChanged(sender, e);
        }

        protected override void ListView_CreateClicked(object sender, EventArgs e)
        {
            EmployeeResourceView.EmployeeDetailView.ToggleControl(
                EmployeeDetailPresenter.Controls.MENU, false, false);
            EmployeeResourceView.EmployeeDetailView.ToggleControl(
                EmployeeDetailPresenter.Controls.DETAIL, true, true);
            base.ListView_CreateClicked(sender, e);
        }

        #endregion

        #endregion
    }
}
