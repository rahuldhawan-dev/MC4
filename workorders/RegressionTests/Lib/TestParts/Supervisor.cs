using System;
using System.Globalization;
using MapCall.Common.Testing.Selenium;
using MMSINC.Testing.Selenium;
using NUnit.Framework;
using Selenium;

namespace RegressionTests.Lib.TestParts
{
    public static class Supervisor
    {
        #region Constants

        public struct NecessaryIDs
        {
            public const string 
                TXT_WORK_ORDER_NUMBER = Global.CONTROL_BASE_ID + 
                    "wofrvWorkOrders_wosvWorkOrders_baseSearch_txtWorkOrderNumber",
                TXT_ACCOUNT_CHARGED = Global.CONTROL_BASE_ID + 
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_woAccountForm_fvWorkOrder_txtAccountCharged",
                TXT_WORK_ORDER_NUMBER_ORCOM = Global.CONTROL_BASE_ID + 
                    "ctl00_wosvWorkOrders_baseSearch_txtWorkOrderNumber",
                TXT_SAP_REQUISITION_NUMBER = Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_worRequisitions_gvRequisitions_txtSAPRequisitionNumber",
                TXT_SAP_REQUISITION_NUMBER_EDIT = Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_worRequisitions_gvRequisitions_txtSAPRequisitionNumber_0",
                TXT_REJECTION_NOTES = Global.CONTROL_BASE_ID + 
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_woAccountForm_fvWorkOrder_txtRejectionNotes";

            public const string
                BTN_REJECT = Global.CONTROL_BASE_ID + 
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_woAccountForm_fvWorkOrder_btnReject",
                BTN_REJECT_SAVE = Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_woAccountForm_fvWorkOrder_btnNotesSubmit",
                BTN_SEARCH = Global.CONTROL_BASE_ID + 
                    "wofrvWorkOrders_wosvWorkOrders_btnSearch",
                BTN_SAVE = Global.CONTROL_BASE_ID + 
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_woAccountForm_fvWorkOrder_btnSave",
                BTN_SEARCH_ORCOM = Global.CONTROL_BASE_ID + 
                    "ctl00_wosvWorkOrders_btnSearch",
                BTN_SAVE_ORCOM = Global.CONTROL_BASE_ID + 
                "ctl00_wodvWorkOrder_fvWorkOrder_wooocf_btnOrcomOrderUpdate";

            public const string 
                ACCOUNT_TAB     = "//a[@id='accountTab']/span", 
                ORCOM_TAB       = "//a[@id='orcomTab']/span",
                REQUISITION_TAB = "//a[@id='requisitionsTab']/span";

            public const string 
                LNK_SELECT = "link=Select",
                LNK_REQUISITION_INSERT = Global.CONTROL_BASE_ID + 
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_worRequisitions_gvRequisitions_lbInsert",
                LNK_REQUISITION_EDIT = Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_worRequisitions_gvRequisitions_lbEdit_0",
                LNK_REQUISITION_UPDATE = Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_worRequisitions_gvRequisitions_lbSave_0",
                LNK_REQUISITION_DELETE = Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_worRequisitions_gvRequisitions_lbDelete_0";

            public const string 
                LBL_WORK_ORDER = Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_lblWorkOrderID",
                LBL_RESULTS_COUNT = 
                    "content_cphMain_cphMain_ctl00_wolvWorkOrders_lblCount",
                LBL_REQUISITION_TYPE = Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_worRequisitions_gvRequisitions_lblRequisitionType_0",
                LBL_SAP_REQUISITION_NUMBER = Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_worRequisitions_gvRequisitions_lblSAPRequisitionNumber_0",
                LBL_NOTES = Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_ctl01_fvWorkOrder_lblNotes";

            public const string
                DDL_REQUISITION_TYPE = Global.CONTROL_BASE_ID +
                     "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_worRequisitions_gvRequisitions_ddlRequisitionType",
                DDL_REQUISITION_TYPE_EDIT = Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_worRequisitions_gvRequisitions_ddlRequisitionType_0";
        }

        public struct URLS
        {
            public const string GENERAL =
                "Modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=update&arg={0}";

        }

        public const string ZERO_COUNT = "0 Result(s)";

        #endregion

        #region Private Helper Methods

        private static void Noop(ISelenium selenium, Types.WorkOrder workOrder) {}

        #endregion

        #region Public Static Methods

        public static void ApproveMainBreakOrder(ISelenium selenium, Types.WorkOrder order)
        {
            ApproveOrder(selenium, order);
        }
        
        public static void ApproveOrder(ISelenium selenium, Types.WorkOrder order, Action<ISelenium, Types.WorkOrder> extraSetup = null, Action<ISelenium, Types.WorkOrder> extraAssertion = null)
        {
            extraSetup = extraSetup ?? Noop;
            extraAssertion = extraAssertion ?? Noop;

            order.AccountCharged = "012345678901234567890123456789";
            selenium.Type(NecessaryIDs.TXT_WORK_ORDER_NUMBER, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_SELECT);
            selenium.Click(NecessaryIDs.ACCOUNT_TAB);

            extraSetup(selenium, order);

            selenium.Type(NecessaryIDs.TXT_ACCOUNT_CHARGED, order.AccountCharged);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SAVE);
            Assert.IsTrue(
                selenium.GetLocation().EndsWith(
                    Navigate.NecessaryURLs.SUPERVISOR_APPROVAL,
                    true,
                    CultureInfo.CurrentCulture));
            Assert.IsTrue(
                selenium.GetText(NecessaryIDs.LBL_WORK_ORDER).Equals(order.WorkOrderID));

            extraAssertion(selenium, order);
        }

        public static void ApproveSapOrder(ISelenium selenium, Types.WorkOrder order, Action<ISelenium, Types.WorkOrder> extraSetup = null, Action<ISelenium, Types.WorkOrder> extraAssertion = null)
        {
            extraSetup = extraSetup ?? Noop;
            extraAssertion = extraAssertion ?? Noop;

            selenium.Type(NecessaryIDs.TXT_WORK_ORDER_NUMBER, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_SELECT);
            selenium.Click(NecessaryIDs.ACCOUNT_TAB);

            extraSetup(selenium, order);

            //selenium.Type(NecessaryIDs.TXT_ACCOUNT_CHARGED, order.AccountCharged);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SAVE);
            Assert.IsTrue(
                selenium.GetLocation().EndsWith(
                    Navigate.NecessaryURLs.SUPERVISOR_APPROVAL,
                    true,
                    CultureInfo.CurrentCulture));
            Assert.IsTrue(
                selenium.GetText(NecessaryIDs.LBL_WORK_ORDER).Equals(order.WorkOrderID));

            extraAssertion(selenium, order);
        }

        public static void RejectOrder(ISelenium selenium, Types.WorkOrder order, Action<ISelenium, Types.WorkOrder> extraSetup = null, Action<ISelenium, Types.WorkOrder> extraAssertion = null)
        {
            const string rejectionNotes = "Here are some rejection notes.";

            selenium.Type(NecessaryIDs.TXT_WORK_ORDER_NUMBER, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_SELECT);
            selenium.Click(NecessaryIDs.ACCOUNT_TAB);
            selenium.Click(NecessaryIDs.BTN_REJECT);
            selenium.Type(NecessaryIDs.TXT_REJECTION_NOTES, rejectionNotes);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_REJECT_SAVE);
            Assert.IsTrue(selenium.GetText(NecessaryIDs.LBL_NOTES).Contains(rejectionNotes));
            Assert.IsTrue(selenium.GetText(NecessaryIDs.LBL_NOTES).Contains(order.Notes));
        }

        public static void Requisition(ISelenium selenium, Types.WorkOrder order)
        {
            //selenium.Type("content_cphMain_cphMain_wofrvWorkOrders_wosvWorkOrders_baseSearch_txtWorkOrderNumber", order.WorkOrderID);
            selenium.Type(NecessaryIDs.TXT_WORK_ORDER_NUMBER, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_SELECT);
            selenium.Click(NecessaryIDs.REQUISITION_TAB);
            selenium.Select(NecessaryIDs.DDL_REQUISITION_TYPE, "Paving");
            selenium.Type(NecessaryIDs.TXT_SAP_REQUISITION_NUMBER, "82B");
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_REQUISITION_INSERT);
            selenium.Click(NecessaryIDs.REQUISITION_TAB);
            Assert.IsTrue(
                selenium.GetText(NecessaryIDs.LBL_REQUISITION_TYPE).Equals("Paving"));
            Assert.IsTrue(
                selenium.GetText(NecessaryIDs.LBL_SAP_REQUISITION_NUMBER).Equals("82B"));

            selenium.Click(NecessaryIDs.LNK_REQUISITION_EDIT);
            selenium.WaitForElementPresent(NecessaryIDs.DDL_REQUISITION_TYPE_EDIT);
            selenium.Select(NecessaryIDs.DDL_REQUISITION_TYPE_EDIT, "Spoils");
            selenium.Type(NecessaryIDs.TXT_SAP_REQUISITION_NUMBER_EDIT, "");
            selenium.Click(NecessaryIDs.LNK_REQUISITION_UPDATE);
            selenium.WaitForElementPresent(NecessaryIDs.LNK_REQUISITION_DELETE);
            Assert.IsTrue(
                selenium.GetText(NecessaryIDs.LBL_REQUISITION_TYPE).Equals("Spoils"));
            Assert.IsTrue(
                selenium.GetText(NecessaryIDs.LBL_SAP_REQUISITION_NUMBER).Equals(""));

            selenium.Click(NecessaryIDs.LNK_REQUISITION_DELETE);
            selenium.AssertConfirmation("Are you sure?", "The confirmation message did not match.");
            selenium.WaitForElementNotPresent(NecessaryIDs.LBL_REQUISITION_TYPE);
        }

        #endregion

        public static void VerifyCannotApproveOrder(IExtendedSelenium selenium, Types.WorkOrder order)
        {
            selenium.Type(NecessaryIDs.TXT_WORK_ORDER_NUMBER, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_SELECT);
            selenium.AssertElementIsNotPresent(NecessaryIDs.BTN_SAVE);
            selenium.AssertElementPresent(NecessaryIDs.BTN_REJECT);
            selenium.AssertTextPresent("Please find an existing service or create a new one and link it to this WorkOrderID");
        }
    }
}
