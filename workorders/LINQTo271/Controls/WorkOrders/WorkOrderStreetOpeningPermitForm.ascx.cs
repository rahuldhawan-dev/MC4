using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Interface;
using Permits.Data.Client.Repositories;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderStreetOpeningPermitForm : WorkOrderDetailControlBase, IWorkOrderStreetOpeningPermitForm
    {
        #region Constants

        public struct StreetOpeningPermitsParameterNames
        {
            public const string WORK_ORDER_ID = "workOrderID",
                                STREETOPENINGPERMIT_NUMBER =
                                    "streetOpeningPermitNumber",
                                DATE_REQUESTED = "dateRequested",
                                DATE_ISSUED = "dateIssued",
                                EXPIRATION_DATE = "expirationDate",
                                NOTES = "notes";
        }

        public struct ControlIDs
        {
            public const string EDIT_LINK = "lbSOPEdit",
                                DELETE_LINK = "lbSOPDelete",
                                UPDATE_LINK = "lbSOPSave",
                                CANCEL_LINK = "lbSOPCancel",
                                INSERT_LINK = "lbInsert",
                                DATE_ISSUED = "ccDateIssued",
                                EXPIRATION_DATE = "ccExpirationDate",
                                NOTES = "txtNotes",
                                DATE_REQUESTED = "ccDateRequested",
                                STREETOPENINGPERMITNUMBER = "txtStreetOpeningPermitNumber",
                                PERMIT_ID = "lblPermitID", 
                                PERMIT_LINK = "lbPermit",
                                IS_PAID_FOR = "chkIsPaidFor";
        }

        public const string CREATE_PERMIT_FORMAT_URL = "~/Views/WorkOrders/PermitProcessing/PermitProcessingResourceViewRPCPage.aspx?cmd=view&arg={0}",
                            CREATE_PERMIT_FORMAT_URL_WITH_ID = "~/Views/WorkOrders/PermitProcessing/PermitProcessingResourceViewRPCPage.aspx?cmd=view&arg={0}&permitId={1}";

        #endregion

        #region Control Declarations

        protected string _txtStreetOpeningPermitNumber,
                           _txtDateRequested,
                           _txtDateIssued,
                           _txtExpirationDate,
                           _txtNotes;
        
        protected IGridView gvStreetOpeningPermits;
        protected IObjectDataSource odsStreetOpeningPermits;
        protected IButton btnCreatePermit;
        protected ILinkButton lbPermit;

        #endregion

        #region Properties

        public string StreetOpeningPermitNumber
        {
            get
            {
                if (_txtStreetOpeningPermitNumber == null)
                    _txtStreetOpeningPermitNumber = gvStreetOpeningPermits.IFooterRow.FindIControl<ITextBox>(ControlIDs.STREETOPENINGPERMITNUMBER).Text;
                return _txtStreetOpeningPermitNumber;
            }
        }

        public string DateRequested
        {
            get
            {
                if (_txtDateRequested == null)
                    _txtDateRequested = gvStreetOpeningPermits.IFooterRow.FindIControl<ITextBox>(ControlIDs.DATE_REQUESTED).Text;
                return _txtDateRequested;
            }
        }

        public string DateIssued
        {
            get
            {
                if (_txtDateIssued == null)
                {
                    _txtDateIssued = gvStreetOpeningPermits.IFooterRow.FindIControl<ITextBox>(ControlIDs.DATE_ISSUED).Text;
                }
               return _txtDateIssued;
            }
        }

        public string ExpirationDate
        {
            get
            {
                if (_txtExpirationDate == null)
                    _txtExpirationDate = gvStreetOpeningPermits.IFooterRow.FindIControl<ITextBox>(ControlIDs.EXPIRATION_DATE).Text;
                return _txtExpirationDate;
            }
        }

        public string Notes
        {
            get
            {
                if (_txtNotes == null)
                    _txtNotes = gvStreetOpeningPermits.IFooterRow.FindIControl<ITextBox>(ControlIDs.NOTES).Text;
                return _txtNotes;
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
            if (gvStreetOpeningPermits.IFooterRow != null)
            {
                gvStreetOpeningPermits.IFooterRow.Visible =
                    btnCreatePermit.Visible = visible;
            }

            foreach (var row in gvStreetOpeningPermits.IRows)
            {
                ToggleEditControlsInRow(row, visible);
                TogglePermitButton(row, visible);
            }
        }

        private void TogglePermitButton(IGridViewRow row, bool visible)
        {
            var permitId = row.FindIControl<ILabel>(ControlIDs.PERMIT_ID);
            if (!String.IsNullOrEmpty(permitId.Text) && visible)
            {
                var payment = row.FindIControl<ICheckBox>(ControlIDs.IS_PAID_FOR).Checked;
                if (!payment)
                {
                    var button = row.FindIControl<ILinkButton>(ControlIDs.PERMIT_LINK);
                    button.Visible = true;
                    button.PostBackUrl =
                        string.Format(
                            CREATE_PERMIT_FORMAT_URL_WITH_ID,
                            WorkOrderID, permitId.Text);
                }
            }
        }

        protected override void SetDataSource(int workOrderID)
        {
            odsStreetOpeningPermits.SelectParameters["WorkOrderID"].DefaultValue = workOrderID.ToString();
        }

        #endregion

        #region Event Handlers

        protected override void Page_Prerender(object sender, EventArgs e)
        {
            ToggleEditAndInsertControls(CurrentMvpMode != DetailViewMode.ReadOnly);
        }

        protected void lbSOPInsert_Click(object sender, EventArgs e)
        {
            odsStreetOpeningPermits.Insert();
        }

        protected void lbSOPCancel_Click(object sender, EventArgs e)
        {

        }
        
        protected void btnCreatePermit_Click(object sender, EventArgs e)
        {
            IResponse.Redirect(String.Format(CREATE_PERMIT_FORMAT_URL, WorkOrderID));
        }

        protected void odsStreetOpeningPermits_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.InputParameters[StreetOpeningPermitsParameterNames.WORK_ORDER_ID] = WorkOrderID;
            e.InputParameters[StreetOpeningPermitsParameterNames.STREETOPENINGPERMIT_NUMBER] = StreetOpeningPermitNumber;
            e.InputParameters[StreetOpeningPermitsParameterNames.DATE_REQUESTED] = DateRequested;
            e.InputParameters[StreetOpeningPermitsParameterNames.NOTES] = Notes;
            e.InputParameters[StreetOpeningPermitsParameterNames.DATE_ISSUED] = DateIssued;
            e.InputParameters[StreetOpeningPermitsParameterNames.EXPIRATION_DATE] = ExpirationDate;
        }

        #endregion
    }

    public interface IWorkOrderStreetOpeningPermitForm : IWorkOrderDetailControl
    {
    }

}