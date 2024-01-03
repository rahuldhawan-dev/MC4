using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderScheduleOfValuesForm : WorkOrderDetailControlBase, IWorkOrderScheduleOfValuesForm
    {
        #region Constants

        public struct ParameterNames
        {
            public const string WORK_ORDER_ID = "workOrderID",
                SCHEDULE_OF_VALUE_ID = "ScheduleOfValueID", 
                TOTAL = "Total",
                IS_OVERTIME = "IsOvertime",
                OTHER_DESCRIPTION = "OtherDescription";
        }

        public struct ControlIDs
        {
            public const string 
                EDIT_LINK = "lbEdit",
                DELETE_LINK = "lbDelete",
                SCHEDULE_OF_VALUE = "ddlScheduleOfValue",
                TOTAL = "txtTotal",
                OVERTIME = "chkIsOvertime",
                OTHER_DESCRIPTION = "txtOtherDescription";
        }

        #endregion

        #region Private Members

        private int _scheduleOfValueID;
        private decimal _total;
        private bool _isOvertime;
        private string _otherDescription;
        
        #endregion

        #region Properties

        public int ScheduleOfValueID
        {
            get
            {
                if (_scheduleOfValueID == 0 &&
                    gvScheduleOfValues.IFooterRow.FindIControl<IDropDownList>(ControlIDs.SCHEDULE_OF_VALUE).SelectedValue != "")
                    _scheduleOfValueID = int.Parse(gvScheduleOfValues.IFooterRow.FindIControl<IDropDownList>(ControlIDs.SCHEDULE_OF_VALUE).SelectedValue);
                return _scheduleOfValueID;
            }
        }

        public decimal Total
        {
            get
            {
                if (_total== 0)
                    _total = decimal.Parse(gvScheduleOfValues.IFooterRow.FindIControl<ITextBox>(ControlIDs.TOTAL).Text);
                return _total;
            }
        }

        public string OtherDescription
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_otherDescription))
                    _otherDescription = gvScheduleOfValues.IFooterRow.FindIControl<ITextBox>(ControlIDs.OTHER_DESCRIPTION).Text;
                return _otherDescription;
            }
        }

        public bool IsOvertime
        {
            get
            {
                return gvScheduleOfValues.IFooterRow.FindIControl<ICheckBox>(ControlIDs.OVERTIME).Checked;
            }
        }
        
        #endregion

        #region Private Methods

        private static void ToggleEditControlsInRow(IGridViewRow row, bool visible)
        {
            var lbEdit =
                row.FindIControl<ILinkButton>(ControlIDs.EDIT_LINK);
            var lbDelete =
                row.FindIControl<ILinkButton>(ControlIDs.DELETE_LINK);

            if (lbEdit != null && lbDelete != null)
            {
                lbEdit.Visible = lbDelete.Visible = visible;
            }
        }

        private void ToggleEditAndInsertControls(bool visible)
        {
            if (gvScheduleOfValues.IFooterRow != null)
            {
                gvScheduleOfValues.IFooterRow.Visible = visible;
            }

            foreach (var row in gvScheduleOfValues.IRows)
            {
                ToggleEditControlsInRow(row, visible);
            }
        }
        
        protected override void SetDataSource(int workOrderID)
        {
            odsWorkOrdersScheduleOfValues.SelectParameters["WorkOrderID"].DefaultValue = workOrderID.ToString();
        }

        #endregion

        #region Event Handlers
        
        protected override void Page_Prerender(object sender, EventArgs e)
        {
            base.Page_Prerender(sender, e);
            ScriptManager.RegisterClientScriptBlock(gvScheduleOfValues, gvScheduleOfValues.GetType(), "scheduleOfValue" + UniqueID, "WorkOrderScheduleOfValuesForm.handleUpdatePanelCallback();", true);
            ToggleEditAndInsertControls(CurrentMvpMode != DetailViewMode.ReadOnly);
        }

        protected void lbScheduleOfValueInsert_Click(object sender, EventArgs e)
        {
            if (IPage.IsValid)
            {
                odsWorkOrdersScheduleOfValues.Insert();
            }
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
            
        }

        protected void odsWorkOrdersScheduleOfValues_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.InputParameters[ParameterNames.WORK_ORDER_ID] = WorkOrderID;
            e.InputParameters[ParameterNames.SCHEDULE_OF_VALUE_ID] = ScheduleOfValueID;
            e.InputParameters[ParameterNames.TOTAL] = Total;
            e.InputParameters[ParameterNames.IS_OVERTIME] = IsOvertime;
            e.InputParameters[ParameterNames.OTHER_DESCRIPTION] = OtherDescription;
        }

        protected void odsWorkOrdersScheduleOfValues_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var scheduleOfValueID = (e.InputParameters[ParameterNames.SCHEDULE_OF_VALUE_ID] ?? string.Empty).ToString();

            if (!String.IsNullOrEmpty(scheduleOfValueID))
                e.InputParameters[ParameterNames.SCHEDULE_OF_VALUE_ID] =
                    new Regex(":::.*").Replace(scheduleOfValueID, String.Empty);
        }

        #endregion

    }

    public interface IWorkOrderScheduleOfValuesForm : IWorkOrderDetailControl { }
}