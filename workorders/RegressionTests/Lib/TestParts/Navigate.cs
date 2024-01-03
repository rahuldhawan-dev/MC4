using System;
using System.Text.RegularExpressions;
using MMSINC.Testing.Selenium;
using Selenium;
using Config = MMSINC.Testing.Utilities.RegressionTestConfiguration;

namespace RegressionTests.Lib.TestParts
{
    public class Navigate
    {
        #region Constants

        public struct NecessaryURLs
        {
            public const string INPUT_SCREEN = "/modules/WorkOrders/Views/WorkOrders/Input/WorkOrderInputResourceView.aspx",
                                SCHEDULING_SCREEN = "/modules/mvc/FieldOperations/WorkOrderScheduling/Search",
                                SUPERVISOR_APPROVAL = "/modules/WorkOrders/Views/WorkOrders/SupervisorApproval/WorkOrderSupervisorApprovalResourceView.aspx",
                                GENERAL_SEARCH = "/modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceView.aspx",
                                STOCK_APPROVAL = "/modules/WorkOrders/Views/WorkOrders/StockToIssue/WorkOrderStockToIssueResourceView.aspx",
                                PLANNING = "/modules/WorkOrders/Views/WorkOrders/Planning/WorkOrderPlanningResourceView.aspx",
                                ORCOM_ORDER_COMPLETION = "/modules/WorkOrders/Views/WorkOrders/OrcomOrderCompletions/OrcomOrderCompletionsresourceView.aspx",
                                CREW_ASSIGNMENTS = "/modules/WorkOrders/Views/CrewAssignments/CrewAssignmentsResourceView.aspx",
                                CREW_MANAGEMENT= "/modules/WorkOrders/Views/Crews/CrewResourceView.aspx",
                                COMPLETED_WORK_ORDERS = "/modules/WorkOrders/Views/Reports/CompletedWorkOrders.aspx",
                                PRE_PLANNING = "/modules/WorkOrders/Views/WorkOrders/PrePlanning/WorkOrderPrePlanningResourceView.aspx",
                                MARKOUT_PLANNING = "/modules/WorkOrders/Views/WorkOrders/MarkoutPlanning/WorkOrderMarkoutPlanningResourceView.aspx",
                                SOP_PROCESSING = "/modules/WorkOrders/Views/WorkOrders/SOPProcessing/SOPProcessingResourceView.aspx",
                                FINALIZATION = "/modules/WorkOrders/Views/WorkOrders/Finalization/WorkOrderFinalizationResourceView.aspx",
                                RESTORATION_PROCESSING = "/modules/WorkOrders/Views/WorkOrders/RestorationProcessing/WorkOrderRestorationProcessingResourceView.aspx",
                                CREW_ASSIGNMENT_SUMMARY_REPORT = "/modules/WorkOrders/Views/Reports/CrewAssignmentSummaryReport.aspx";
        }

        public struct NecessaryIDs
        {
            public const string DDL_OPERATING_CENTER_ID =
                Global.CONTROL_BASE_ID +
                "woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_ddlOperatingCenter",
                TXT_WORK_ORDER =
                    Global.CONTROL_BASE_ID +
                    "wogrvWorkOrders_wosvWorkOrders_baseSearch_txtWorkOrderNumber";

            public const string BTN_SEARCH =
                Global.CONTROL_BASE_ID +
                "wogrvWorkOrders_wosvWorkOrders_btnSearch";

            public const string LBL_COUNT =
                Global.CONTROL_BASE_ID +
                "wogrvWorkOrders_wolvWorkOrders_lblCount";

            public const string LNK_SELECT = "link=Select";
        }

        #endregion

        #region Private Static Methods

        private static void OpenFromRoot(ISelenium selenium, string url)
        {
            selenium.Open(Config.GetDevSiteUrl() + url);
        }

        private static void NavigatingTo(string page)
        {
            Console.WriteLine($"Navigating to {page}...");
        }

        #endregion

        #region Public Static Methods

        public static void ToInput(ISelenium selenium)
        {
            NavigatingTo("Input");
            OpenFromRoot(selenium, NecessaryURLs.INPUT_SCREEN);
            selenium.WaitForEditable(NecessaryIDs.DDL_OPERATING_CENTER_ID);
        }

        public static void ToScheduling(ISelenium selenium)
        {
            NavigatingTo("Scheduling");
            OpenFromRoot(selenium, NecessaryURLs.SCHEDULING_SCREEN);
        }

        public static void ToCrewManagement(ISelenium selenium)
        {
            NavigatingTo("CrewManagement");
            OpenFromRoot(selenium, NecessaryURLs.CREW_MANAGEMENT);
        }

        public static void ToSupervisorApproval(ISelenium selenium)
        {
            NavigatingTo("SupervisorApproval");
            OpenFromRoot(selenium, NecessaryURLs.SUPERVISOR_APPROVAL);
        }

        public static void ToGeneralSearch(ISelenium selenium)
        {
            NavigatingTo("GeneralSearch");
            OpenFromRoot(selenium, NecessaryURLs.GENERAL_SEARCH);
        }

        public static void ToStockApproval(ISelenium selenium)
        {
            NavigatingTo("StockApproval");
            OpenFromRoot(selenium, NecessaryURLs.STOCK_APPROVAL);
        }

        public static void ToPlanning(ISelenium selenium)
        {
            NavigatingTo("Planning");
            OpenFromRoot(selenium, NecessaryURLs.PLANNING);
        }

        public static void ToCompletedOrders(ISelenium selenium)
        {
            NavigatingTo("CompletedOrders");
            OpenFromRoot(selenium, NecessaryURLs.COMPLETED_WORK_ORDERS);
        }

        public static void ToPrePlanning(ISelenium selenium)
        {
            NavigatingTo("PrePlanning");
            OpenFromRoot(selenium, NecessaryURLs.PRE_PLANNING);
        }

        public static void ToMarkoutPlanning(ISelenium selenium)
        {
            NavigatingTo("MarkoutPlanning");
            OpenFromRoot(selenium, NecessaryURLs.MARKOUT_PLANNING);
        }

        public static void ToSOPProcessing(ISelenium selenium)
        {
            NavigatingTo("SOPProcessing");
            OpenFromRoot(selenium, NecessaryURLs.SOP_PROCESSING);
        }

        public static void ToFinalization(ISelenium selenium)
        {
            NavigatingTo("Finalization");
            OpenFromRoot(selenium, NecessaryURLs.FINALIZATION);
        }

        public static void ToRestorationProcessing(ISelenium selenium)
        {
            NavigatingTo("RestorationProcessing");
            OpenFromRoot(selenium, NecessaryURLs.RESTORATION_PROCESSING);
        }

        public static void ToCrewAssignmentSummaryReport(ISelenium selenium)
        {
            NavigatingTo("CrewAssignmentSummaryReport");
            OpenFromRoot(selenium, NecessaryURLs.CREW_ASSIGNMENT_SUMMARY_REPORT);
        }

        public static void FindAndSelectOrder(ISelenium selenium, Types.WorkOrder order)
        {
            Console.WriteLine($"Finding and Selecting order {order.WorkOrderID}...");
            selenium.Type(NecessaryIDs.TXT_WORK_ORDER, order.WorkOrderID);
            selenium.Click(NecessaryIDs.BTN_SEARCH);
            selenium.WaitForText(NecessaryIDs.LBL_COUNT,
                new Regex(@"^.* Result\(s\)$"));
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_SELECT);
            // sometimes we don't actually get to the show page?
            if (selenium.IsElementPresent(NecessaryIDs.LNK_SELECT))
            {
                selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_SELECT);
            }
        }

        #endregion
    }
}
