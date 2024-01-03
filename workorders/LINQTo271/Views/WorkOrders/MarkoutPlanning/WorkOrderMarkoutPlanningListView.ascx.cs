using System;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace LINQTo271.Views.WorkOrders.MarkoutPlanning
{
    public partial class WorkOrderMarkoutPlanningListView : WorkOrderListView, IWorkOrderMarkoutPlanningListView
    {
        #region Properties

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.PrePlanning; }
        }

        public int? OperatingCenterID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Private Methods

        protected void OnSaveCommand(GridViewCommandEventArgs e)
        {
            var row = ((WebControl)e.CommandSource).Parent.Parent;
            var workOrderID = Int32
                .Parse(((Label)row.FindControl("lblOrderNumber")).Text);
            var dateNeeded = DateTime
                .Parse(((TextBox)row.FindControl("txtDateNeeded")).Text);
            var markoutTypeID = Int32
                .Parse(((DropDownList)row.FindControl("ddlMarkoutType"))
                        .SelectedValue);
            var markoutNote = ((TextBox)row.FindControl("txtMarkoutNote")).Text;

            if (SaveClicked != null)
            {
                SaveClicked(this,
                    new MarkoutPlanningEventArgs(workOrderID, dateNeeded,
                        markoutTypeID, markoutNote));
            }
        }

        #endregion

        #region Event Handlers

        protected void gvWorkOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "save")
            {
                OnSaveCommand(e);
            }
        }

        #endregion

        #region Events

        public event EventHandler<MarkoutPlanningEventArgs> SaveClicked;

        #endregion
    }
}
