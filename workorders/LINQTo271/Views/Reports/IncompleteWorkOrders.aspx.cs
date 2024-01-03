using System;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Controls.GridViewHelper;
using WorkOrders.Model;

namespace LINQTo271.Views.Reports
{
    public partial class IncompleteWorkOrders : WorkOrdersReport
    {
        #region Event Handlers

        protected void gvSearchResults_DataBinding(object sender, EventArgs e)
        {
            ((MvpGridView)sender).DataSource =
                WorkOrderRepository.GetIncompleteWorkOrdersByWorkDescriptionID(
                    Int32.Parse(ddlWorkDescription.SelectedValue));
            var helper = new GridViewHelper((GridView)gvSearchResults);

            helper.RegisterGroup("OperatingCenter", true, true);
            helper.RegisterSummary("Longitude", "Total: {0}", SummaryOperation.Count, "OperatingCenter");
            helper.ApplyGroupSort();
        }
        
        protected void gvSearchResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void gvSearchResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            
        }

        #endregion
    }
}
