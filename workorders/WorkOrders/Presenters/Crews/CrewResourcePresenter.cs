using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;

namespace WorkOrders.Presenters.Crews
{
    public class CrewResourcePresenter : WorkOrdersAdminResourcePresenter<Crew>
    {
        #region Constructors

        public CrewResourcePresenter(IResourceView view, IRepository<Crew> repository)
            : base(view, repository)
        {
        }

        #endregion

        #region Event Handlers

        protected override void View_LoadComplete(object sender, EventArgs e)
        {
            if (ListView != null && ListView.Visible)
            {
                SetListViewData();
                ListView.DataBind();
            }
        }

        #endregion
    }
}
