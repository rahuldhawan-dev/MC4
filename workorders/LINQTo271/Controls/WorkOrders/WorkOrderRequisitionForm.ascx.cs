using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using WorkOrders;
using WorkOrders.Model;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderRequisitionForm : WorkOrderDetailControlBase, IWorkOrderRequisitionForm
    {
        #region Constants

        public struct RequisitionsParameterNames
        {
            public const string WORK_ORDER_ID = "workOrderID",
                                REQUISITION_TYPE_ID = "requisitionTypeID",
                                SAP_REQUISITION_NUMBER = "sapRequisitionNumber",
                                CREATOR_ID = "creatorID";
        }

        public struct ControlIDs
        {
            public const string REQUISITION_GRIDVIEW = "gvRequisitions",
                                REQUISITION_ERROR_LABEL = "lblRequisitionError",
                                SAP_REQUISITION_NUMBER_TEXTBOX = "txtSAPRequisitionNumber",
                                REQUISITION_TYPE_DROPDOWNLIST = "ddlRequisitionType",
                                UPDATE_PANEL = "upRequisitions",
                                ROW_EDIT_LINKBUTTON = "lbEdit",
                                ROW_DELETE_LINKBUTTON = "lbDelete";
        }

        #endregion
        
        #region Private Members

        protected IGridView gvRequisitions;
        protected ITextBox _txtSAPRequisitionNumber;
        protected IDropDownList _ddlRequisitionType;
        protected ILabel _requisitionError;
        protected IObjectDataSource odsRequisitions;
        protected IUpdatePanel upRequisitions;

        #endregion

        #region Properties

        public ILabel RequisitionError
        {
            get
            {
                if (_requisitionError == null)
                    _requisitionError = upRequisitions.FindIControl<ILabel>(ControlIDs.REQUISITION_ERROR_LABEL);
                return _requisitionError;
            }
        }

        public string RequisitionTypeID
        {
            get
            {
                if (_ddlRequisitionType == null)
                    _ddlRequisitionType = gvRequisitions.IFooterRow.FindIControl<IDropDownList>(
                        ControlIDs.REQUISITION_TYPE_DROPDOWNLIST);
                return _ddlRequisitionType.SelectedValue;
            }
        }

        public string SAPRequisitionNumber
        {
            get
            {
                if (_txtSAPRequisitionNumber == null)
                    _txtSAPRequisitionNumber = gvRequisitions.IFooterRow.FindIControl<ITextBox>(
                        ControlIDs.SAP_REQUISITION_NUMBER_TEXTBOX);
                return _txtSAPRequisitionNumber.Text;
            }
        }
        
        #endregion

        #region Private Methods

        protected override void SetDataSource(int workOrderID)
        {
            odsRequisitions.SelectParameters["WorkOrderID"].DefaultValue =
                workOrderID.ToString();
        }

        private static void ToggleEditControlsInRow(IGridViewRow row, bool visible)
        {
            var lbEdit =
                row.FindIControl<ILinkButton>(ControlIDs.ROW_EDIT_LINKBUTTON);
            var lbDelete =
                row.FindIControl<ILinkButton>(ControlIDs.ROW_DELETE_LINKBUTTON);

            if (lbEdit != null && lbDelete != null)
            {
                lbEdit.Visible = lbDelete.Visible = visible;
            }
        }

        private void ToggleEditAndInsertControls(bool visible)
        {
            if (gvRequisitions.IFooterRow != null)
            {
                gvRequisitions.IFooterRow.Visible = visible;
            }

            foreach (var row in gvRequisitions.IRows)
            {
                ToggleEditControlsInRow(row, visible);
            }
        }

        #endregion
        
        #region Event Handlers

        protected void lbInsert_Click(object sender, EventArgs e)
        {
            odsRequisitions.Insert();
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
            //noop
        }

        protected void odsRequisitions_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.InputParameters[RequisitionsParameterNames.WORK_ORDER_ID] = WorkOrderID;
            e.InputParameters[RequisitionsParameterNames.REQUISITION_TYPE_ID] = RequisitionTypeID;
            e.InputParameters[RequisitionsParameterNames.SAP_REQUISITION_NUMBER] = SAPRequisitionNumber;
            e.InputParameters[RequisitionsParameterNames.CREATOR_ID] = SecurityService.GetEmployeeID();
        }

        protected override void Page_Prerender(object sender, EventArgs e)
        {
            base.Page_Prerender(sender, e);

            ToggleEditAndInsertControls(CurrentMvpMode != DetailViewMode.ReadOnly);
        }
        
        #endregion
    }

    public interface IWorkOrderRequisitionForm : IWorkOrderDetailControl
    {
    }
}