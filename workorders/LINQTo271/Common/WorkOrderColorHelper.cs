using System.Drawing;
using System.Web.UI.WebControls;
using MapCall.Common.Model.Entities;
using WorkOrder = WorkOrders.Model.WorkOrder;

namespace LINQTo271.Common
{
    public static class WorkOrderColorHelper
    {
        public static readonly Color PAST_ASSIGNMENT_ROW_COLOR = Color.FromArgb(255, 253, 185), 
                                     FUTURE_ASSIGNMENT_ROW_COLOR = Color.FromArgb(187, 241, 255),
                                     CANCELLED = Color.FromArgb(255, 224, 141),
                                     COMPLETED = Color.FromArgb(187, 255, 188);

        public static void ApplyColors(GridViewRow row)
        {
            if (row.RowType != DataControlRowType.DataRow ||
                row.DataItem == null)
            {
                return;
            }
            var wo = (WorkOrder)row.DataItem;
            
            switch (wo.Status)
            {
                case WorkOrderStatus.Cancelled:
                    row.BackColor = CANCELLED;
                    break;

                case WorkOrderStatus.Completed:
                    row.BackColor = COMPLETED;
                    break;

                case WorkOrderStatus.ScheduledPreviously:
                    row.BackColor = PAST_ASSIGNMENT_ROW_COLOR;
                    break;

                case WorkOrderStatus.ScheduledCurrently:
                    row.BackColor = FUTURE_ASSIGNMENT_ROW_COLOR;
                    break;
            }
        }
    }
}