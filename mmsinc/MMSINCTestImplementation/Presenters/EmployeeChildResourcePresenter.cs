using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Presenters
{
    public class EmployeeChildResourcePresenter : ChildResourcePresenter<Employee>
    {
        #region Constructors

        public EmployeeChildResourcePresenter(IChildResourceView<Employee> view, IRepository<Employee> repository)
            : base(view, repository) { }

        #endregion

        #region Event Handlers

        protected override void ListView_CreateClicked(object sender, EventArgs e)
        {
            ChildResourceView.OnChildEvent(e);
            if (DetailView != null)
            {
                DetailView.ShowEntity(new Employee {ReportsTo = (Employee)ParentEntity});
                DetailView.SetViewMode(DetailViewMode.Insert);
                DetailView.DataBind();
            }

            if (View != null)
                View.SetViewMode(ResourceViewMode.Detail);
        }

        #endregion

        #region Exposed Methods

        public override void FilterListViews()
        {
            ListView.SetListData(
                Repository.GetFilteredSortedData(
                    emp => emp.ReportsTo == ParentEntity,
                    ListView.SqlSortExpression));
        }

        #endregion
    }
}
