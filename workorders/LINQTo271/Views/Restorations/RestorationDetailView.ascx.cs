using System;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using WorkOrders.Views.Restorations;
using SecurityServiceClass = WorkOrders.Library.Permissions.SecurityService;

namespace LINQTo271.Views.Restorations
{
    public partial class RestorationDetailView : WorkOrdersDetailView<Restoration>, IRestorationDetailView
    {
        #region Constants

        public struct EntityKeys
        {
            public const string APPROVED_BY_ID = "ApprovedByID",
                                DATE_APPROVED = "DateApproved",
                                REJECTED_BY_ID = "RejectedByID",
                                DATE_REJECTED = "DateRejected",
                                PARTIAL_RESTORATION_METHOD_ID =
                                    "PartialRestorationMethodID",
                                FINAL_RESTORATION_METHOD_ID =
                                    "FinalRestorationMethodID",
                                WORK_ORDER_ID = "WorkOrderID";
        }

        public struct ControlIDs
        {
            public const string EIGHT_INCH_STAB_BASE_BY_COMPANY_FORCES =
                "chkEightInchStabilizeBaseByCompanyForces";

            public const string PARTIAL_RESTORATION_DATE =
                "ccPartialRestorationDate",
                                TOTAL_INITAL_ACTUAL_COST =
                                    "txtTotalInitalActualCost",
                                PARTIAL_RESTORATION_INVOICE_NUMBER =
                                    "txtPartialRestorationInvoiceNumber",
                                PARTIAL_RESTORATION_COMPLETED_BY =
                                    "txtPartialRestorationCompletedBy",
                                FINAL_RESTORATION_DATE =
                                    "ccFinalRestorationDate",
                                FINAL_RESTORATION_ACTUAL_COST =
                                    "txtFinalRestorationActualCost",
                                FINAL_RESTORATION_INVOICE_NUMBER =
                                    "txtFinalRestorationInvoiceNumber",
                                FINAL_RESTORATION_COMPLETED_BY =
                                    "txtFinalRestorationCompletedBy";
        }

        #endregion

        #region Control Declarations

        protected IDetailControl fvRestoration;
        protected IObjectContainerDataSource odsRestoration;

        #endregion

        #region Private Members

        protected ISecurityService _securityService;

        #endregion

        #region Properties

        public override IDetailControl DetailControl
        {
            get { return fvRestoration; }
        }

        public override IObjectContainerDataSource DataSource
        {
            get { return odsRestoration; }
        }

        protected ISecurityService SecurityService
        {
            get
            {
                if (_securityService == null)
                    _securityService = SecurityServiceClass.Instance;
                return _securityService;
            }
        }

        public int WorkOrderID { get; set; }

        #region fvRestoration Controls

        //Initial
        public ITextBox InitialRestorationDate
        {
            get
            {
                return fvRestoration.FindIControl<ITextBox>(ControlIDs.PARTIAL_RESTORATION_DATE);
            }
        }

        public ITextBox TotalInitialActualCost
        {
            get
            {
                return fvRestoration.FindIControl<ITextBox>(ControlIDs.TOTAL_INITAL_ACTUAL_COST);
            }
        }

        public ITextBox InitialRestorationInvoiceNumber
        {
            get
            {
                return fvRestoration.FindIControl<ITextBox>(ControlIDs.PARTIAL_RESTORATION_INVOICE_NUMBER);
            }
        }

        public ITextBox PartialRestorationCompletedBy
        {
            get
            {
                return fvRestoration.FindIControl<ITextBox>(ControlIDs.PARTIAL_RESTORATION_COMPLETED_BY);
            }
        }

        public ICheckBox EightInchStabilizeBaseByCompanyForces
        {
            get
            {
                return
                    fvRestoration.FindIControl<ICheckBox>(
                        ControlIDs.EIGHT_INCH_STAB_BASE_BY_COMPANY_FORCES);
            }
        }

        //Final
        public ITextBox FinalRestorationDate
        {
            get
            {
                return
                    fvRestoration.FindIControl<ITextBox>(
                        ControlIDs.FINAL_RESTORATION_DATE);
            }
        }

        public ITextBox FinalRestorationActualCost
        {
            get
            {
                return
                    fvRestoration.FindIControl<ITextBox>(
                        ControlIDs.FINAL_RESTORATION_ACTUAL_COST);
            }
        }

        public ITextBox FinalRestorationInvoiceNumber
        {
            get
            {
                return
                    fvRestoration.FindIControl<ITextBox>(ControlIDs.FINAL_RESTORATION_INVOICE_NUMBER);
            }
        }

        public ITextBox FinalRestorationCompletedBy
        {
            get
            {
                return
                    fvRestoration.FindIControl<ITextBox>(
                       ControlIDs.FINAL_RESTORATION_COMPLETED_BY);
            }
        }

        #endregion

        #endregion

        #region Event Handlers

        protected void fvRestoration_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (!IPage.IsValid)
                return;

            e.Values[EntityKeys.WORK_ORDER_ID] = WorkOrderID;

            var currentUserID = SecurityService.GetEmployeeID();

            if (!string.IsNullOrEmpty( (e.Values[EntityKeys.DATE_APPROVED] ?? string.Empty).ToString()))
                e.Values[EntityKeys.APPROVED_BY_ID] = currentUserID;
            if (!string.IsNullOrEmpty((e.Values[EntityKeys.DATE_REJECTED] ?? string.Empty).ToString()))
                e.Values[EntityKeys.REJECTED_BY_ID] = currentUserID;
        }

        protected void fvRestoration_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (!IPage.IsValid)
                return;

            String oldDateApproved =
                       e.OldValues[EntityKeys.DATE_REJECTED].ToString(),
                   newDateApproved =
                       e.NewValues[EntityKeys.DATE_APPROVED].ToString(),
                   oldDateRejected =
                       e.OldValues[EntityKeys.DATE_REJECTED].ToString(),
                   newDateRejected =
                       e.NewValues[EntityKeys.DATE_REJECTED].ToString();

            var currentUserID = SecurityService.GetEmployeeID();

            if (oldDateApproved != newDateApproved)
                e.NewValues[EntityKeys.APPROVED_BY_ID] = currentUserID;
            if (oldDateRejected != newDateRejected)
                e.NewValues[EntityKeys.REJECTED_BY_ID] = currentUserID;
        }

        #endregion

        #region Private Methods

        protected void CheckInitialRestorationDateFields(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (string.IsNullOrEmpty(InitialRestorationDate.Text) ||
                            EightInchStabilizeBaseByCompanyForces.Checked ||
                            ((!string.IsNullOrEmpty(
                                   InitialRestorationInvoiceNumber.Text)) &&
                             !string.IsNullOrEmpty(
                                  PartialRestorationCompletedBy.Text)));
        }

        protected void CheckFinalRestorationDateFields(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (string.IsNullOrEmpty(FinalRestorationDate.Text) ||
                            ((!string.IsNullOrEmpty(
                                   FinalRestorationActualCost.Text) &&
                              !string.IsNullOrEmpty(
                                   FinalRestorationInvoiceNumber.Text)) &&
                             !string.IsNullOrEmpty(
                                  FinalRestorationCompletedBy.Text)));
        }

        #endregion
    }
}
