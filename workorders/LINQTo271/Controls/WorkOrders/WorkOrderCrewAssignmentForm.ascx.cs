using System;
using System.Drawing;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Utilities;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderCrewAssignmentForm : WorkOrderDetailControlBase
    {
        #region Constants

        public static readonly Color COMPLETED_ORDER_ROW_COLOR = Color.LightGreen;

        #endregion

        #region Private Members

        protected CrewAssignmentRepository _crewAssignmentRepository;

        #endregion

        #region Properties

        protected CrewAssignmentRepository CrewAssignmentRepository
        {
            get
            {
                if (_crewAssignmentRepository == null)
                    _crewAssignmentRepository =
                        DependencyResolver.Current.GetService<CrewAssignmentRepository>();
                return _crewAssignmentRepository;
            }
        }

        #endregion

        #region Private Methods

        protected override void SetDataSource(int workOrderID)
        {
            odsCrewAssignments.SelectParameters["WorkOrderID"].DefaultValue =
                workOrderID.ToString();
        }

        private void OnAssignmentCommand(CommandEventArgs args, float employeesOnJob)
        {
            var assignment = CrewAssignmentRepository.Get(args.CommandArgument);
            assignment.DateEnded = DateTime.Now;
            assignment.EmployeesOnJob = employeesOnJob;
            CrewAssignmentRepository.UpdateCurrentEntity(assignment);
            CrewAssignmentRepository.UpdateSAPWorkOrder(assignment.WorkOrder);
            gvCrewAssignments.DataBind();
        }

        protected virtual void DisplayStartDate(Control row, DateTime date)
        {
            var lbl = (Label)row.FindControl("lblStartDate");
            if (lbl != null)
            {
                lbl.Visible = true;
                lbl.Text = string.Format(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE,date);
            }
        }

        protected virtual void DisplayEndDate(Control row, DateTime date)
        {
            var lbl = (Label)row.FindControl("lblEndDate");
            var lbEnd = row.FindControl("lbEnd");
            if (lbEnd != null)
            {
                lbEnd.Visible = false;
            }
            if (lbl != null)
            {
                lbl.Visible = true;
                lbl.Text = string.Format(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE,date);
            }
        }

        protected virtual void DisplayEmployeesOnJob(Control row, float employeesOnJob)
        {
            row.FindControl("txtEmployeesOnJob").Visible = false;
            var lbl = (Label)row.FindControl("lblEmployeesOnJob");
            if (lbl != null)
            {
                lbl.Visible = true;
                lbl.Text = employeesOnJob.ToString();
            }
        }

        protected virtual void DisplayNotApplicable(Control row)
        {
            row.FindControl("lbEnd").Visible = false;
            row.FindControl("lblEndNotApplicable").Visible = true;
            row.FindControl("lblStartDate").Visible = false;
            row.FindControl("lblStartNotApplicable").Visible = true;
            DisplayEmployeesNotApplicable(row);
        }

        protected virtual void DisplayEmployeesNotApplicable(Control row)
        {
            row.FindControl("lblEmployeesNotApplicable").Visible = true;
            row.FindControl("txtEmployeesOnJob").Visible = false;
            row.FindControl("lblEmployeesOnJob").Visible = false;
        }

        #endregion

        #region Event Handlers

        protected override void Page_Prerender(object sender, EventArgs e)
        {
            base.Page_Prerender(sender, e);

            gvCrewAssignments.AutoGenerateEditButton =
                (ParentView.Phase == WorkOrderPhase.General && CurrentMvpMode == DetailViewMode.Edit);
        }

        protected void gvCrewAssignments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var txtEmployeesOnJob =
                ((Control)e.CommandSource).Parent.Parent.FindControl(
                    "txtEmployeesOnJob") as MvpTextBox;

            if (txtEmployeesOnJob != null)
            {
                OnAssignmentCommand(e, float.Parse(txtEmployeesOnJob.Text));
            }
        }

        // TODO:
        // this doesn't take into consideration the CurrentMode ('Edit', 'Insert', 'ReadOnly')
        // it may need to at some point.
        protected void gvCrewAssignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow || e.Row.DataItem == null || (CurrentMvpMode == DetailViewMode.Edit && (e.Row.RowState == DataControlRowState.Edit || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate))))
                return;
            var assignment = (CrewAssignment)e.Row.DataItem;
            if (assignment.DateStarted != null)
            {
                DisplayStartDate(e.Row, assignment.DateStarted.Value);
                if (assignment.DateEnded != null)
                {
                    DisplayEndDate(e.Row, assignment.DateEnded.Value);
                    if (assignment.EmployeesOnJob.HasValue)
                    {
                        DisplayEmployeesOnJob(e.Row,
                            assignment.EmployeesOnJob.Value);
                    }
                    else
                    {
                        DisplayEmployeesNotApplicable(e.Row);
                    }
                }
            }
            else
            {
                DisplayNotApplicable(e.Row);
            }

            // If the underlying work order has a completed date, show the row as colored.
            if (assignment.WorkOrder.DateCompleted != null)
            {
                e.Row.BackColor = COMPLETED_ORDER_ROW_COLOR;
            }
        }

        #endregion
    }
}