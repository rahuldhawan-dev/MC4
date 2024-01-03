using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Presenters
{
    public class OrderChildResourcePresenter : ChildResourcePresenter<Order>
    {
        #region Constructors

        public OrderChildResourcePresenter(IChildResourceView<Order> view, IRepository<Order> repository)
            : base(view, repository) { }

        #endregion

        #region Event Handlers

        protected override void ListView_CreateClicked(object sender, EventArgs e)
        {
            ChildResourceView.OnChildEvent(e);
            if (DetailView != null)
            {
                DetailView.ShowEntity(new Order());
                DetailView.SetViewMode(DetailViewMode.Insert);
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
                    ord => ord.Employee == ParentEntity,
                    ListView.SqlSortExpression));
        }

        #endregion
    }
}
