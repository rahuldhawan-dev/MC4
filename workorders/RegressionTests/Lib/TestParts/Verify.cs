using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Humanizer;
using MapCall.Common.Testing.Selenium;
using MMSINC.Testing.Selenium;
using NUnit.Framework;
using Selenium;

namespace RegressionTests.Lib.TestParts
{
    public static class Verify
    {
        #region Constants

        public struct NecessaryIDs
        {
            public struct GeneralSearch
            {
                public const string TXT_WORK_ORDER_ID =
                    Global.CONTROL_BASE_ID + "wogrvWorkOrders_wosvWorkOrders_baseSearch_txtWorkOrderNumber";

                public const string BTN_SEARCH =
                    Global.CONTROL_BASE_ID +
                    "wogrvWorkOrders_wosvWorkOrders_btnSearch",
                    BTN_EDIT =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_btnEdit",
                    BTN_CANCEL_ORDER =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_btnCancelOrder";

                public const string CHK_SOP_REQUIRED =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ctl14";

                public const string DDL_DATE_TYPE =
                    Global.CONTROL_BASE_ID + "wogrvWorkOrders_wosvWorkOrders_ddlDateType",
                                    DDL_CUSTOMER_IMPACT_RANGE =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlCustomerImpactRange",
                                    DDL_REPAIR_TIME_RANGE =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlRepairTimeRange",
                                    DDL_OPERATING_CENTER =
                                        Global.CONTROL_BASE_ID + "wogrvWorkOrders_wosvWorkOrders_baseSearch_ddlOperatingCenter",
                                    DDL_ASSET_TYPE =
                                        Global.CONTROL_BASE_ID + "wogrvWorkOrders_wosvWorkOrders_ddlAssetType";

                public const string CC_END_DATE =
                    Global.CONTROL_BASE_ID + "wogrvWorkOrders_wosvWorkOrders_drDateToSearch_ccEndDate";

                public const string LBL_COUNT =
                    Global.CONTROL_BASE_ID + "wogrvWorkOrders_wolvWorkOrders_lblCount";

                public const string LNK_SELECT = "link=Select";

                public const string TR_MAIN_BREAK_INFO = "css=tr.trMainBreakInfo";

                public const string HID_ALERT_STARTED = Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_hidAlertStarted";
            }

            public struct Scheduling
            {
                public const string TXT_WORK_ORDER_ID =
                    Global.CONTROL_BASE_ID + "wosrv_wosvWorkOrders_baseSearch_txtWorkOrderNumber";

                public const string BTN_SEARCH =
                    Global.CONTROL_BASE_ID + "wosrv_wosvWorkOrders_btnSearch";

                public const string DDL_OPERATING_CENTER =
                    Global.CONTROL_BASE_ID +
                    "wosrv_wosvWorkOrders_baseSearch_ddlOperatingCenter";
            }

            public struct Planning
            {
                public const string TXT_WORK_ORDER_ID =
                    Global.CONTROL_BASE_ID +
                    "woprv_wosvWorkOrders_baseSearch_txtWorkOrderNumber";

                public const string BTN_SEARCH =
                    Global.CONTROL_BASE_ID + "woprv_wosvWorkOrders_btnSearch";

                public const string LNK_SELECT = "link=Select";
            }

            public struct PrePlanning
            {
                public const string TXT_WORK_ORDER_ID =
                    Global.CONTROL_BASE_ID +
                    "woprv_wosvWorkOrders_baseSearch_txtWorkOrderNumber";

                public const string BTN_SEARCH =
                    Global.CONTROL_BASE_ID + "woprv_wosvWorkOrders_btnSearch";

                public const string DDL_OPERATING_CENTER =
                    Global.CONTROL_BASE_ID +
                    "woprv_wosvWorkOrders_baseSearch_ddlOperatingCenter";
            }

            public struct MarkoutPlanning
            {
                public const string TXT_WORK_ORDER_ID =
                    Global.CONTROL_BASE_ID +
                    "womprv_wosvWorkOrders_baseSearch_txtWorkOrderNumber";
                public const string BTN_SEARCH =
                    Global.CONTROL_BASE_ID + 
                    "womprv_wosvWorkOrders_btnSearch";
                public const string DDL_OPERATING_CENTER =
                    Global.CONTROL_BASE_ID +
                    "womprv_wosvWorkOrders_baseSearch_ddlOperatingCenter";
            }

            public struct CrewAssignmentsSummaryReports
            {
                public const string DDL_CREW =
                    Global.CONTROL_BASE_ID + "ddlCrew";

                public const string DDL_OPERATING_CENTER =
                    Global.CONTROL_BASE_ID + "ddlOpCode";

                public const string DDL_SEARCH_OPERATORS =
                    Global.CONTROL_BASE_ID + "drAssignmentDate_ddlSearchOp";

                public const string DDL_IS_OPEN =
                    Global.CONTROL_BASE_ID + "ddlIsOpen";

                public const string CC_ASSIGNMENT_DATE =
                    Global.CONTROL_BASE_ID + "drAssignmentDate_ccEndDate";
            }

            public struct CrewManagement
            {
                public const string BTN_NEW =
                    Global.CONTROL_BASE_ID +
                    "crv_clvCrews_btnCreate";
            }

            public struct SOPProcessing
            {
                public const string TXT_WORK_ORDER_ID =
                    Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wosvWorkOrders_baseSearch_txtWorkOrderNumber";
                public const string BTN_SEARCH =
                    Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wosvWorkOrders_btnSearch";
                public const string DDL_OPERATING_CENTER =
                    Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wosvWorkOrders_baseSearch_ddlOperatingCenter";
            }

            public struct StockApproval
            {
                public const string TXT_WORK_ORDER_ID =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wosvWorkOrders_baseSearch_txtWorkOrderNumber";
                public const string BTN_SEARCH =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wosvWorkOrders_btnSearch";
                public const string DDL_OPERATING_CENTER =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wosvWorkOrders_baseSearch_ddlOperatingCenter";
            }

            public struct SupervisorApproval
            {
                public const string TXT_WORK_ORDER_ID =
                    Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wosvWorkOrders_baseSearch_txtWorkOrderNumber";
                public const string BTN_SEARCH =
                    Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wosvWorkOrders_btnSearch";
                public const string DDL_OPERATING_CENTER =
                    Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wosvWorkOrders_baseSearch_ddlOperatingCenter";
            }

            public struct Finalization
            {
                public const string TXT_WORK_ORDER_ID =
                    Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wosvWorkOrders_baseSearch_txtWorkOrderNumber";
                public const string BTN_SEARCH =
                    Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wosvWorkOrders_btnSearch";                
                public const string DDL_OPERATING_CENTER =
                    Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wosvWorkOrders_baseSearch_ddlOperatingCenter";
            }

            public struct RestorationProcessing
            {
                public const string TXT_WORK_ORDER_ID =
                    Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wosvWorkOrders_baseSearch_txtWorkOrderNumber";
                public const string BTN_SEARCH =
                    Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wosvWorkOrders_btnSearch";
                public const string DDL_OPERATING_CENTER =
                    Global.CONTROL_BASE_ID +
                    "wofrvWorkOrders_wosvWorkOrders_baseSearch_ddlOperatingCenter";
            }

            public struct Reports
            {
                public const string CC_END_DATE =
                    Global.CONTROL_BASE_ID + "drCompletedDate_ccEndDate";

                public const string BTN_SEARCH =
                    Global.CONTROL_BASE_ID + "btnSearch";
            }
        }

        public const string NO_RECORDS = "No records found. Please refine your search.",
                            OPERATING_CENTER = "NJ7 - Shrewsbury",
                            INVALID_WORKORDER_ID = "999999999";

        #endregion

        #region Private Static Methods

        private static void Verifying(string what = null)
        {
            what = string.IsNullOrWhiteSpace(what)
                ? ("Verifying" + new StackTrace().GetFrame(1).GetMethod().Name).Humanize()
                : "Verifying" + what;
            Console.WriteLine(what + "...");
        }
        
        #endregion

        #region Public Static Methods

        public static void CurrentOrderIsApproved(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.GeneralSearch.TXT_WORK_ORDER_ID, order.WorkOrderID);
            selenium.WaitThenSelectLabel(NecessaryIDs.GeneralSearch.DDL_DATE_TYPE,
                "DateCompleted");
            selenium.Type(NecessaryIDs.GeneralSearch.CC_END_DATE, order.DateCompleted);
            selenium.Click(NecessaryIDs.GeneralSearch.BTN_SEARCH);
            selenium.WaitForText(NecessaryIDs.GeneralSearch.LBL_COUNT,
                new Regex(@"^.* Result\(s\)$"));
            selenium.AssertTextPresent(order.WorkOrderID);
        }

        public static void CurrentOrderStreetOpeningPermitRequired(ISelenium selenium, Types.WorkOrder order)
        {
            selenium.Type(NecessaryIDs.GeneralSearch.TXT_WORK_ORDER_ID, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.GeneralSearch.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.GeneralSearch.LNK_SELECT);
            selenium.IsChecked(NecessaryIDs.GeneralSearch.CHK_SOP_REQUIRED);
        }

        public static void CurrentOrderHasExtraMainBreakInformation(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.GeneralSearch.TXT_WORK_ORDER_ID, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.GeneralSearch.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.GeneralSearch.LNK_SELECT);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.GeneralSearch.BTN_EDIT);
            selenium.AssertSelectedLabel(
                NecessaryIDs.GeneralSearch.DDL_CUSTOMER_IMPACT_RANGE,
                order.CustomerImpactRange);
            selenium.AssertSelectedLabel(NecessaryIDs.GeneralSearch.DDL_REPAIR_TIME_RANGE,
                order.RepairTimeRange);
        }

        public static void CurrentOrderHasAlertIssued(ISelenium selenium, Types.WorkOrder order, bool alertIssued)
        {
            Verifying();
            selenium.Type(NecessaryIDs.GeneralSearch.TXT_WORK_ORDER_ID, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.GeneralSearch.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.GeneralSearch.LNK_SELECT);
            var actual = !string.IsNullOrWhiteSpace(selenium.GetValue(NecessaryIDs.GeneralSearch.HID_ALERT_STARTED));

            Assert.AreEqual(alertIssued, actual);
        }

        public static void CurrentOrderAppearsInScheduling(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.Scheduling.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.Scheduling.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.Scheduling.BTN_SEARCH);
            selenium.AssertTextPresent("1 Result(s)");
            selenium.AssertTextPresent(order.WorkOrderID);
        }

        public static void CurrentOrderAppearsInSOPProcessing(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.SOPProcessing.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.SOPProcessing.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.SOPProcessing.BTN_SEARCH);
            selenium.AssertTextPresent("1 Result(s)");
            selenium.AssertTextPresent(order.WorkOrderID);
        }

        public static void CurrentOrderAppearsInStockApproval(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.StockApproval.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.StockApproval.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.StockApproval.BTN_SEARCH);
            selenium.AssertTextPresent("1 Result(s)");
            selenium.AssertTextPresent(order.WorkOrderID);
        }

        public static void CurrentOrderAppearsInSupervisorApproval(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.SupervisorApproval.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.SupervisorApproval.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.SupervisorApproval.BTN_SEARCH);
            selenium.AssertTextPresent("1 Result(s)");
            selenium.AssertTextPresent(order.WorkOrderID);
        }

        public static void CurrentOrderAppearsInRestorationProcessing(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.RestorationProcessing.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.RestorationProcessing.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.RestorationProcessing.BTN_SEARCH);
            selenium.AssertTextPresent("1 Result(s)");
            selenium.AssertTextPresent(order.WorkOrderID);
        }

        public static void CurrentOrderDoesNotAppearInScheduling(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.Scheduling.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.Scheduling.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.Scheduling.BTN_SEARCH);
            selenium.AssertTextPresent(
                "No records found. Please refine your search.");
        }

        public static void CurrentOrderDoesNotAppearInSOPProcessing(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.SOPProcessing.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.SOPProcessing.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.SOPProcessing.BTN_SEARCH);
            selenium.AssertTextPresent(
                "No records found. Please refine your search.");
        }

        public static void CurrentOrderDoesNotAppearInStockApproval(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.StockApproval.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.StockApproval.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.StockApproval.BTN_SEARCH);
            selenium.AssertTextPresent(
                "No records found. Please refine your search.");
        }

        public static void CurrentOrderDoesNotAppearInSupervisorApproval(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.SupervisorApproval.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.SupervisorApproval.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.SupervisorApproval.BTN_SEARCH);
            selenium.AssertTextPresent(
                "No records found. Please refine your search.");
        }

        public static void CurrentOrderDoesNotAppearInRestorationProcessing(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.RestorationProcessing.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.RestorationProcessing.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.RestorationProcessing.BTN_SEARCH);
            selenium.AssertTextPresent(
                "No records found. Please refine your search.");
        }

        public static void CurrentOrderDoesNotAppearInFinalization(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.Finalization.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.Finalization.BTN_SEARCH);
            selenium.AssertTextPresent(
                "No records found. Please refine your search.");
        }

        public static void CurrentOrderDoesNotAppearInPlanning(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.Planning.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.Planning.BTN_SEARCH);
            selenium.AssertTextPresent(
                "No records found. Please refine your search.");
        }

        public static void CurrentOrderDoesNotAppearInPrePlanning(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.PrePlanning.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.PrePlanning.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.PrePlanning.BTN_SEARCH);
            selenium.AssertTextPresent(
                "No records found. Please refine your search.");
        }

        public static void CurrentOrderDoesNotAppearInMarkoutPlanning(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.MarkoutPlanning.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.MarkoutPlanning.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.MarkoutPlanning.BTN_SEARCH);
            selenium.AssertTextPresent(
                "No records found. Please refine your search.");
        }

        public static void CurrentOrderAppearsInPlanning(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.Planning.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.Planning.BTN_SEARCH);
            selenium.AssertTextPresent("1 Result(s)");
            selenium.AssertTextPresent(order.WorkOrderID);
        }

        public static void CurrentOrderAppearsInPrePlanning(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.PrePlanning.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.PrePlanning.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.PrePlanning.BTN_SEARCH);
            selenium.AssertTextPresent("1 Result(s)");
            selenium.AssertTextPresent(order.WorkOrderID);
        }

        public static void CurrentOrderAppearsInMarkoutPlanning(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.MarkoutPlanning.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.MarkoutPlanning.DDL_OPERATING_CENTER,
                order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.MarkoutPlanning.BTN_SEARCH);
            selenium.AssertTextPresent("1 Result(s)");
            selenium.AssertTextPresent(order.WorkOrderID);
        }

        public static void CurrentOrderAppearsInCrewAssignments(ISelenium selenium, Types.WorkOrder order, Types.CrewAssignment assignment)
        {
            Verifying();
            selenium.SelectAndWaitForPageToLoad(NecessaryIDs.CrewAssignmentsSummaryReports.DDL_CREW, assignment.CrewID);
            selenium.AssertTextPresent(order.WorkOrderID);
        }

        public static void CurrentOrderAppearsInFinalization(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.Finalization.TXT_WORK_ORDER_ID, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.Finalization.BTN_SEARCH);
            selenium.AssertTextPresent("1 Result(s)");
            selenium.AssertTextPresent(order.WorkOrderID);
        }

        public static void CompletedOrderIsReturned(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.Reports.CC_END_DATE, order.DateCompleted);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.Reports.BTN_SEARCH);
            selenium.AssertTextPresent(order.WorkOrderID);
            selenium.AssertTextPresent(order.StreetNumber);
            selenium.AssertTextPresent(order.Street);
        }

        public static void PlanningSearchDoesNotError(ISelenium selenium)
        {
            Verifying();
            selenium.Type(NecessaryIDs.Planning.TXT_WORK_ORDER_ID, INVALID_WORKORDER_ID);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.Planning.BTN_SEARCH);
            selenium.AssertTextPresent(NO_RECORDS);
        }

        public static void MarkoutPlanningSearchDoesNotError(ISelenium selenium)
        {
            Verifying();
            selenium.Type(NecessaryIDs.MarkoutPlanning.TXT_WORK_ORDER_ID, INVALID_WORKORDER_ID);
            selenium.SelectLabel(NecessaryIDs.MarkoutPlanning.DDL_OPERATING_CENTER, OPERATING_CENTER);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.MarkoutPlanning.BTN_SEARCH);
            selenium.AssertTextPresent(NO_RECORDS);
        }

        public static void SOPProcessingSearchDoesNotError(ISelenium selenium)
        {
            Verifying();
            selenium.Type(NecessaryIDs.SOPProcessing.TXT_WORK_ORDER_ID, INVALID_WORKORDER_ID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.SOPProcessing.BTN_SEARCH);
            selenium.AssertTextPresent(NO_RECORDS);
        }

        public static void FinalizationSearchDoesNotError(ISelenium selenium)
        {
            Verifying();
            selenium.Type(NecessaryIDs.Finalization.TXT_WORK_ORDER_ID, INVALID_WORKORDER_ID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.Finalization.BTN_SEARCH);
            selenium.AssertTextPresent(NO_RECORDS);
        }

        public static void RestorationProcessingSearchDoesNotError(ISelenium selenium)
        {
            Verifying();
            selenium.Type(NecessaryIDs.RestorationProcessing.TXT_WORK_ORDER_ID, INVALID_WORKORDER_ID);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.RestorationProcessing.BTN_SEARCH);
            selenium.AssertTextPresent(NO_RECORDS);
        }

        public static void CrewManagementDoesNotError(ISelenium selenium)
        {
            Verifying();
            selenium.AssertTextPresent("Crew Management");
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.CrewManagement.BTN_NEW);
            selenium.AssertTextPresent("New Crew");
        }

        public static void CurrentOrderHasEquipmentPurpose(ISelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            selenium.Type(NecessaryIDs.GeneralSearch.TXT_WORK_ORDER_ID,
                order.WorkOrderID);
            selenium.SelectLabel(NecessaryIDs.GeneralSearch.DDL_OPERATING_CENTER, order.OperatingCenter);
            selenium.WaitThenSelectLabel(NecessaryIDs.GeneralSearch.DDL_ASSET_TYPE, order.AssetType);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.GeneralSearch.BTN_SEARCH);
            selenium.AssertTextPresent(order.WorkOrderID);
        }

        public static void CurrentOrderCannotBeCancelled(IExtendedSelenium selenium, Types.WorkOrder order)
        {
            Verifying();
            Navigate.FindAndSelectOrder(selenium, order);
            selenium.AssertElementIsNotPresent(
                NecessaryIDs.GeneralSearch.BTN_CANCEL_ORDER);
        }

        public static void SapStatusMatchesRegex(IExtendedSelenium selenium, string id, string regex)
        {
            selenium.AssertText(id,new Regex($"{regex}"));
        }

        public static void IsOpenFilterTest(ISelenium selenium, Types.WorkOrder openWorkOrder, Types.WorkOrder closedWorkOrder, bool? isOpen)
        {
            string openFilter = "--Select Here--";
            switch (isOpen)
            {
                case true:
                    openFilter = "Yes";
                    break;
                case false:
                    openFilter = "No";
                    break;
            }

            Verifying();

            selenium.SelectLabel(
                NecessaryIDs.CrewAssignmentsSummaryReports.DDL_OPERATING_CENTER, closedWorkOrder.OperatingCenter);
            selenium.SelectLabel(NecessaryIDs.CrewAssignmentsSummaryReports.DDL_SEARCH_OPERATORS, "=");
            selenium.Type(NecessaryIDs.CrewAssignmentsSummaryReports.CC_ASSIGNMENT_DATE, DateTime.Today.ToShortDateString());
            selenium.SelectLabel(NecessaryIDs.CrewAssignmentsSummaryReports.DDL_IS_OPEN, openFilter);

            selenium.ClickAndWaitForPageToLoad("content_cphMain_cphMain_btnSearch");
            
            //Look at results page
            switch (isOpen)
            {
                case true:
                    selenium.AssertTextPresent(openWorkOrder.WorkOrderID);
                    selenium.AssertTextNotPresent(closedWorkOrder.WorkOrderID);
                    break;

                case false:
                    selenium.AssertTextNotPresent(openWorkOrder.WorkOrderID);
                    selenium.AssertTextPresent(closedWorkOrder.WorkOrderID);
                    break;
                
                default:
                    selenium.AssertTextPresent(openWorkOrder.WorkOrderID);
                    selenium.AssertTextPresent(closedWorkOrder.WorkOrderID);
                    break;
            }
        }

        #endregion
    }
}
