using System;
using System.Text.RegularExpressions;
using MapCall.Common.Testing.Selenium;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Selenium;
using Selenium;

namespace RegressionTests.Lib.TestParts
{
    public static class StockClerk
    {
        #region Constants

        public struct NecessaryIDs
        {
            public const string TXT_WORK_ORDER_NUMBER =
                Global.CONTROL_BASE_ID + "ctl00_wosvWorkOrders_baseSearch_txtWorkOrderNumber",
                                TXT_DOC_ID =
                                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_woStockApproval_fvWorkOrder_txtDocID";

            public const string BTN_SEARCH =
                                    Global.CONTROL_BASE_ID + "ctl00_wosvWorkOrders_btnSearch",
                                BTN_SAVE =
                                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_woStockApproval_fvWorkOrder_btnSave",
                                BTN_COMPLETE_MATERIAL_PLANNING =
                                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_btnMaterialPlanningComplete";

            public const string LNK_SELECT = "link=Select";

            public const string 
                LBL_COMPLETED_DATE =
                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_woifvInitial_fvWorkOrder_lblDateCompleted",
                LBL_MATERIALS_PLANNED_ON =
                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_lblMaterialPlanningCompletedOn";

            public const string TAB_MATERIALS_USED =
                "//a[@id='materialsUsedTab']/span";
        }

        #endregion

        #region Public Static Methods

        public static void ApproveOrderStock(ISelenium selenium, Types.WorkOrder order)
        {
            selenium.Type(NecessaryIDs.TXT_WORK_ORDER_NUMBER, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_SELECT);
            selenium.AssertText(NecessaryIDs.LBL_COMPLETED_DATE,
                order.DateCompleted);
            selenium.Click(NecessaryIDs.TAB_MATERIALS_USED);
            selenium.Type(NecessaryIDs.TXT_DOC_ID, "423");
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SAVE);
        }

        public static void ApproveSapOrderStock(ISelenium selenium, Types.WorkOrder order)
        {
            Data.ToggleSAPErrorCode(order, "RETRY");
            selenium.Type(NecessaryIDs.TXT_WORK_ORDER_NUMBER, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_SELECT);
            selenium.AssertText(NecessaryIDs.LBL_COMPLETED_DATE, order.DateCompleted);
            selenium.Click(NecessaryIDs.TAB_MATERIALS_USED);
            //selenium.Type(NecessaryIDs.TXT_DOC_ID, "423");
            // Verify Can't Approve
            selenium.AssertAttribute(NecessaryIDs.BTN_SAVE, "disabled", "disabled");

            Data.ToggleSAPErrorCode(order, "success");
            Navigate.ToStockApproval(SetUpFixtureBase.Selenium);
            selenium.Type(NecessaryIDs.TXT_WORK_ORDER_NUMBER, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_SELECT);
            selenium.Click(NecessaryIDs.TAB_MATERIALS_USED);

            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SAVE);
        }
        
        #endregion

        public static void PlanMaterials(IExtendedSelenium selenium, Types.WorkOrder order)
        {
            selenium.Type(Verify.NecessaryIDs.GeneralSearch.TXT_WORK_ORDER_ID, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(Verify.NecessaryIDs.GeneralSearch.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(Verify.NecessaryIDs.GeneralSearch.LNK_SELECT);
            selenium.ClickAndWaitForPageToLoad(Verify.NecessaryIDs.GeneralSearch.BTN_EDIT);
            selenium.Click(NecessaryIDs.TAB_MATERIALS_USED);
            selenium.Type("content_cphMain_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_womuMaterialsUsed_gvMaterialsUsed_txtQuantity", "1");
            selenium.WaitThenSelectLabel("content_cphMain_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_womuMaterialsUsed_gvMaterialsUsed_ddlPartNumber", "1600040");
            selenium.WaitThenSelectLabel("content_cphMain_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_womuMaterialsUsed_gvMaterialsUsed_ddlStockLocation", "H&M");
            selenium.ClickAndWait("content_cphMain_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_womuMaterialsUsed_gvMaterialsUsed_lbInsert",10);
            
            selenium.ClickAndWait(NecessaryIDs.BTN_COMPLETE_MATERIAL_PLANNING, 60); // SAP is SLOW
            if (selenium.IsConfirmationPresent())
            {
                selenium.AssertConfirmation("Are you sure you want to complete material planning?");
            }

            //selenium.
            //content_cphMain_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_lblMaterialPlanningCompletedOn
            //1/23/2017 4:09:16 PM
            MyAssert.AreClose(DateTime.Now, DateTime.Parse(selenium.GetText(NecessaryIDs.LBL_MATERIALS_PLANNED_ON)), new TimeSpan(0,0,2,0));
        }
    }
}
