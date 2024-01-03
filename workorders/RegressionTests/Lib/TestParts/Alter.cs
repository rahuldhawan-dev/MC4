using System;
using System.IO;
using System.Text.RegularExpressions;
using MapCall.Common.Testing.Selenium;
using MMSINC.Testing.Selenium;
using Selenium;
using Config = MMSINC.Testing.Utilities.RegressionTestConfiguration;

namespace RegressionTests.Lib.TestParts
{
    public static class Alter
    {
        #region Constants

        public struct NecessaryIDs
        {
            public const string TXT_WORK_ORDER =
                Global.CONTROL_BASE_ID +
                "wogrvWorkOrders_wosvWorkOrders_baseSearch_txtWorkOrderNumber",
                                TXT_LOCATION = "txtLocation",
                                TXT_PREMISE_NUMBER =
                                    Global.CONTROL_BASE_ID +
                                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_txtPremiseNumber",
                                TXT_SERVICE_NUMBER =
                                    Global.CONTROL_BASE_ID +
                                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_txtServiceNumber",
                                TXT_CREW_ASSIGNMENT_DATE_STARTED =
                                    Global.CONTROL_BASE_ID +
                                    "ctl00_wodvWorkOrder_fvWorkOrder_woCrewAssignments_gvCrewAssignments_txtStartDate_0",
                                TXT_CREW_ASSIGNMENT_DATE_ENDED =
                                    Global.CONTROL_BASE_ID +
                                    "ctl00_wodvWorkOrder_fvWorkOrder_woCrewAssignments_gvCrewAssignments_txtEndDate_0",
                                TXT_CREW_ASSIGNMENT_EMPLOYEES_ON_CREW =
                                    Global.CONTROL_BASE_ID +
                                    "ctl00_wodvWorkOrder_fvWorkOrder_woCrewAssignments_gvCrewAssignments_txtEmployeesOnJob_0",
                                TXT_ACCOUNT_CHARGED = Global.CONTROL_BASE_ID +
                                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_txtAccountCharged";

            public const string BTN_SEARCH =
                Global.CONTROL_BASE_ID + "wogrvWorkOrders_wosvWorkOrders_btnSearch",
                                BTN_EDIT =
                                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_btnEdit",
                                BTN_GEO_CODE = "btnGeoCode",
                                BTN_SAVE_COORDINATES = "btnSave",
                                BTN_SAVE_INITIAL_INFORMATION =
                                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_btnSave";

            public const string LBL_COUNT =
                Global.CONTROL_BASE_ID +
                "wogrvWorkOrders_wolvWorkOrders_lblCount",
                                LBL_NOTES =
                                    "content_cphMain_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_lblNotes",
                                LBL_CREW_ASSIGNMENT_DATE_STARTED =
                                    Global.CONTROL_BASE_ID +
                                    "ctl00_wodvWorkOrder_fvWorkOrder_woCrewAssignments_gvCrewAssignments_lblStartDate_0",
                                LBL_CREW_ASSIGNMENT_DATE_ENDED =
                                    Global.CONTROL_BASE_ID +
                                    "ctl00_wodvWorkOrder_fvWorkOrder_woCrewAssignments_gvCrewAssignments_lblEndDate_0",
                                LBL_CREW_ASSIGNMENT_EMPLOYEES_ON_CREW =
                                    Global.CONTROL_BASE_ID +
                                    "ctl00_wodvWorkOrder_fvWorkOrder_woCrewAssignments_gvCrewAssignments_lblEmployeesOnJob_0";

            public const string LNK_SELECT = "link=Select",
                                LNK_CREW_ASSIGNMENTS_TAB = "crewAssignmentsTab",
                                LNK_CREW_ASSIGNMENT_EDIT =
                                    "//table[@id='content_cphMain_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_woCrewAssignments_gvCrewAssignments']/tbody/tr[2]/td/a",
                                LNK_CREW_ASSIGNMENT_UPDATE =
                                    "//table[@id='content_cphMain_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_woCrewAssignments_gvCrewAssignments']/tbody/tr[2]/td/a[1]";

            public const string DDL_ASSET_TYPE =
                Global.CONTROL_BASE_ID +
                "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlAssetType",
                DDL_VALVE =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlValve",
                DDL_DESCRIPTION_OF_WORK =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlDescriptionOfWork",
                DDL_STREET =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlStreet",
                DDL_CROSS_STREET =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlNearestCrossStreet",
                DDL_TOWN =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlTown",
                DDL_TOWN_SECTION =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlTownSection",
                DDL_CUSTOMER_IMPACT =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlCustomerImpactRange",
                DDL_REPAIR_TIME =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlRepairTimeRange",
                DDL_SIGNIFICANT_TRAFFIC_IMPACT =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlSignificantTrafficImpact",
                DDL_ALERT_ISSUED =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlAlertIssued",
                DDL_CANCELLATION_REASONS =
                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_ddlWorkOrderCancellationReasons",
                DDL_PMAT =
                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_ddlPlantMaintenanceActivityTypeOverride",
                BTN_CANCEL_ORDER =
                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_btnCancelOrder",
                BTN_CANCEL_DELETE_ORDER =
                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_btnDelete";
            public const string IMG_SHOW_PICKER =
                Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_llpAsset_imgShowPicker";

            public const string HID_LONGITUDE = "//input[@id=\"hidLongitude\"]";
        }

        #endregion

        #region Public Static Methods

        public static void ChangeOrderAssetMainToValveAndSwapStreets(ISelenium selenium, ref Types.WorkOrder order)
        {
            var street = order.NearestCrossStreet;
            order.NearestCrossStreet = order.Street;
            order.Street = street;
            order.AssetType = "Valve";
            order.ValveID = "894 - ACTIVE";
            order.DescriptionOfWork = "VALVE BLOW OFF REPLACEMENT";

            AlterOrderInitialInformation(selenium, order);
            VerifyOrderAssetInformation(selenium, order);
        }

        public static void ChangeOrderAssetValveToMain(ISelenium selenium, ref Types.WorkOrder order)
        {
            order.AssetType = "Main";
            order.ValveID = null;
            order.DescriptionOfWork = "WATER MAIN BLEEDERS";

            AlterOrderInitialInformation(selenium, order);
            VerifyOrderAssetInformation(selenium, order);
        }

        public static void ChangeOrderAssetHydrantToValve(ISelenium selenium, ref Types.WorkOrder order)
        {
            order.AssetType = "Valve";
            order.ValveID = "893 - ACTIVE";
            order.DescriptionOfWork = "VALVE BLOW OFF REPLACEMENT";
            order.HydrantID = null;

            AlterOrderInitialInformation(selenium, order);
            VerifyOrderAssetInformation(selenium, order);
        }

        public static void ChangeOrderAssetSewerOpeningToValve(ISelenium selenium, ref Types.WorkOrder order)
        {
            order.AssetType = "Valve";
            order.ValveID = "3028 - ACTIVE";
            order.DescriptionOfWork = "VALVE BLOW OFF INSTALLATION";
            order.SewerOpeningID = null;

            AlterOrderInitialInformation(selenium, order);
            VerifyOrderAssetInformation(selenium, order);
        }

        public static void ChangeOrderAssetServiceToMain(ISelenium selenium, ref Types.WorkOrder order)
        {
            order.AssetType = "Main";
            order.DescriptionOfWork = "WATER MAIN BLEEDERS";
            order.ServiceNumber = null;
            order.PremiseNumber = null;

            AlterOrderInitialInformation(selenium, order);
            VerifyOrderAssetInformation(selenium, order);
        }

        public static void ChangeOrderAssetServiceToMainInvestigation(ISelenium selenium, ref Types.WorkOrder order)
        {
            order.AssetType = "Main";
            order.DescriptionOfWork = "MAIN INVESTIGATION";
            order.ServiceNumber = null;
            order.PremiseNumber = null;

            AlterOrderInitialInformation(selenium, order);
            VerifyOrderAssetInformation(selenium, order);
        }

        public static void ChangeOrderAssetServiceToMainForMainBreak(ISelenium selenium, ref Types.WorkOrder order)
        {
            order.AssetType = "Main";
            order.DescriptionOfWork = "WATER MAIN BREAK REPAIR";
            order.ServiceNumber = null;
            order.PremiseNumber = null;
            order.CustomerImpactRange = "0-50";
            order.RepairTimeRange = "4-6";
            order.AlertIssued = "Yes";
            order.SignificantTrafficImpact = "Yes";

            AlterOrderInitialInformation(selenium, order);
            VerifyOrderAssetInformation(selenium, order);
        }

        public static void ChangeOrderAssetServiceToValve(ISelenium selenium, ref Types.WorkOrder order)
        {
            order.AssetType = "Valve";
            order.ValveID = "893 - ACTIVE";
            order.DescriptionOfWork = "VALVE BLOW OFF REPLACEMENT";
            order.PremiseNumber = null;
            order.ServiceNumber = null;

            AlterOrderInitialInformation(selenium, order);
            VerifyOrderAssetInformation(selenium, order);
        }

        public static void ChangeOrderAssetValveToService(ISelenium selenium, ref Types.WorkOrder order)
        {
            order.AssetType = "Service";
            order.PremiseNumber = "9180458651";
            order.ServiceNumber = "12345678";
            order.DescriptionOfWork = "CURB BOX REPAIR";
            order.ValveID = null;

            AlterOrderInitialInformation(selenium, order);
            VerifyOrderAssetInformation(selenium, order);
        }

        public static void ChangeOrderTownWithNoTownSection(ISelenium selenium, ref Types.WorkOrder order)
        {
            order.Town = "COLTS NECK";
            order.TownSection = null;
            order.Street = "BELLAIRE CT";
            order.NearestCrossStreet = "AMSTERDAM CT";

            DoAlterOrder(selenium, order);
            VerifyOrderLocationInformation(selenium, order);
        }

        public static void ChangeOrderOrcomOrderNoChanges(ISelenium selenium, ref Types.WorkOrder order)
        {
            DoAlterOrder(selenium, order);
            VerifyOrderNotesHaveNotChanged(selenium, order);
        }

        public static void ChangeOrderCrewAssignmentValues(ISelenium selenium, Types.WorkOrder order, ref Types.CrewAssignment assignment)
        {
            var now = DateTime.Now;
            var then = now.AddHours(-5);

            assignment.DateStarted = then.ToString("g");
            assignment.DateEnded = now.ToString("g");
            assignment.EmployeesOnCrew = (5.5).ToString();

            AlterCrewAssignmentValues(selenium, order, assignment);
            ValidateCrewAssignmentValues(selenium, assignment);
        }

        public static void ChangeOrderAlertIssued(ISelenium selenium, Types.WorkOrder order, bool alertIssued)
        {
            Navigate.FindAndSelectOrder(selenium, order);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_EDIT);
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_ALERT_ISSUED, (alertIssued) ? "Yes" : "No");
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SAVE_INITIAL_INFORMATION);
        }

        public static void CancelOrder(IExtendedSelenium selenium, Types.WorkOrder order)
        {
            Navigate.FindAndSelectOrder(selenium, order);
            selenium.ChooseOkOnNextConfirmation();

            try
            {
                selenium.Click(NecessaryIDs.BTN_CANCEL_ORDER);
            }
            catch
            {
                var ssPath =
                    Path.GetFullPath(
                        $"./CancelButtonMissing{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}.png");
                Console.WriteLine($"Could not find cancel button, generating screenshot at '{ssPath}'...");
                selenium.CaptureEntirePageScreenshot(ssPath, string.Empty);
                throw;
            }

            selenium.SelectLabel(NecessaryIDs.DDL_CANCELLATION_REASONS, "Created In Error");
            selenium.Click(NecessaryIDs.BTN_CANCEL_DELETE_ORDER);
            selenium.AssertConfirmation("Are you sure you want to cancel the order?");
            selenium.WaitForPageToLoad("10000");
        }

        #endregion

        #region Private Static Methods

        private static void DoAlterOrder(ISelenium selenium, Types.WorkOrder order)
        {
            selenium.Type(NecessaryIDs.TXT_WORK_ORDER, order.WorkOrderID);
            selenium.Click(NecessaryIDs.BTN_SEARCH);
            selenium.WaitForText(NecessaryIDs.LBL_COUNT,
                new Regex(@"^.* Result\(s\)$"));
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_SELECT);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_EDIT);
            AlterOrderLocationInformation(selenium, order);
            AlterOrderAssetInformation(selenium, order);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.BTN_SAVE_INITIAL_INFORMATION);
        }

        private static void AlterOrderInitialInformation(ISelenium selenium, Types.WorkOrder order)
        {
            Navigate.FindAndSelectOrder(selenium, order);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_EDIT);
            AlterOrderLocationInformation(selenium, order);
            AlterOrderAssetInformation(selenium, order);
            selenium.ClickAndWaitForPageToLoad(
                NecessaryIDs.BTN_SAVE_INITIAL_INFORMATION);
        }

        private static void AlterOrderLocationInformation(ISelenium selenium, Types.WorkOrder order)
        {
            // TODO: work in progress, only does streets for now
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_TOWN, order.Town);
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_STREET, order.Street);
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_CROSS_STREET,
                order.NearestCrossStreet);
        }

        private static void VerifyOrderLocationInformation(ISelenium selenium, Types.WorkOrder order)
        {
            selenium.AssertSelectedLabel(NecessaryIDs.DDL_TOWN, order.Town);
            // might also be "None Found"
            //selenium.AssertSelectedLabel(NecessaryIDs.DDL_TOWN_SECTION,
            //    order.TownSection ?? "--Select Here--");
            selenium.AssertSelectedLabel(NecessaryIDs.DDL_STREET, order.Street);
            selenium.AssertSelectedLabel(NecessaryIDs.DDL_CROSS_STREET,
                order.NearestCrossStreet);
        }

        private static void AlterOrderAssetInformation(ISelenium selenium, Types.WorkOrder order)
        {
            selenium.WaitForNotSelectedLabel(NecessaryIDs.DDL_ASSET_TYPE,
                "[Loading Asset Types...]");
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_ASSET_TYPE,
                order.AssetType);
            selenium.WaitForNotSelectedLabel(
                NecessaryIDs.DDL_DESCRIPTION_OF_WORK,
                "[Loading Descriptions...]");
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_DESCRIPTION_OF_WORK,
                order.DescriptionOfWork);
            switch (order.AssetType)
            {
                case "Service":
                    selenium.Type(NecessaryIDs.TXT_PREMISE_NUMBER,
                        order.PremiseNumber);
                    selenium.Type(NecessaryIDs.TXT_SERVICE_NUMBER,
                        order.ServiceNumber);
                    break;
                case "Valve":
                    selenium.WaitForNotSelectedLabel(NecessaryIDs.DDL_VALVE,
                        "[Loading Valves]");
                    selenium.WaitThenSelectLabel(NecessaryIDs.DDL_VALVE,
                        order.ValveID);
                    break;
                case "Main":
                    if (new Regex(@"^WATER MAIN BREAK REP(AIR|LACE)$").IsMatch(order.DescriptionOfWork))
                    {
                        selenium.SelectLabel(NecessaryIDs.DDL_CUSTOMER_IMPACT,
                            order.CustomerImpactRange);
                        selenium.SelectLabel(NecessaryIDs.DDL_REPAIR_TIME,
                            order.RepairTimeRange);
                        selenium.SelectLabel(NecessaryIDs.DDL_ALERT_ISSUED, order.AlertIssued);
                        selenium.SelectLabel(
                            NecessaryIDs.DDL_SIGNIFICANT_TRAFFIC_IMPACT,
                            order.SignificantTrafficImpact);
                    }
                    break;
                default:
                    throw new NotImplementedException(
                        String.Format(
                            "Logic for altering work order changing asset type to '{0}' is not yet implemented.",
                            order.AssetType));
            }
            selenium.Click(NecessaryIDs.IMG_SHOW_PICKER);
            selenium.WaitForElementPresent(NecessaryIDs.BTN_GEO_CODE);
            // TODO: this next line only ever times out
//            selenium.WaitForElementPresent("//area");
            selenium.AssertValue(NecessaryIDs.TXT_LOCATION, order.GetAddress());
            selenium.Click(NecessaryIDs.BTN_GEO_CODE);

            selenium.WaitForNotValue(NecessaryIDs.HID_LONGITUDE, "-74.1481018");
            // No idea where this zero is coming from but we need to wait for it to not be zero.
            selenium.WaitForNotValue(NecessaryIDs.HID_LONGITUDE, "0");
            
            selenium.WaitForElementPresent(NecessaryIDs.BTN_SAVE_COORDINATES);
            selenium.Click(NecessaryIDs.BTN_SAVE_COORDINATES);
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_DESCRIPTION_OF_WORK,
                order.DescriptionOfWork);
        }

        private static void VerifyOrderAssetInformation(ISelenium selenium, Types.WorkOrder order)
        {
            selenium.WaitForNotSelectedLabel(NecessaryIDs.DDL_ASSET_TYPE,
                "'[Loading Asset Types...]'");
            selenium.AssertSelectedLabel(NecessaryIDs.DDL_ASSET_TYPE,
                order.AssetType);
            switch (order.AssetType)
            {
                case "Service":
                    selenium.WaitForNotValue(NecessaryIDs.TXT_PREMISE_NUMBER,
                        String.Empty);
                    selenium.WaitForNotValue(NecessaryIDs.TXT_SERVICE_NUMBER,
                        String.Empty);
                    selenium.AssertValue(NecessaryIDs.TXT_SERVICE_NUMBER,
                        order.ServiceNumber);
                    selenium.AssertValue(NecessaryIDs.TXT_PREMISE_NUMBER,
                        order.PremiseNumber);
                    break;
                case "Valve":
                    selenium.WaitForNotSelectedLabel(NecessaryIDs.DDL_VALVE,
                        "[Loading Valves]");
                    selenium.WaitForNotSelectedLabel(NecessaryIDs.DDL_VALVE,
                        "--Select Here--");
                    selenium.AssertSelectedLabel(NecessaryIDs.DDL_VALVE,
                        order.ValveID);
                    break;
                case "Main":
                    if (new Regex(@"^WATER MAIN BREAK REP(AIR|LACE)$").IsMatch(order.DescriptionOfWork))
                    {
                        selenium.AssertSelectedLabel(NecessaryIDs.DDL_CUSTOMER_IMPACT,
                            order.CustomerImpactRange);
                        selenium.AssertSelectedLabel(NecessaryIDs.DDL_REPAIR_TIME,
                            order.RepairTimeRange);
                        selenium.AssertSelectedLabel(NecessaryIDs.DDL_ALERT_ISSUED, order.AlertIssued);
                        selenium.AssertSelectedLabel(
                            NecessaryIDs.DDL_SIGNIFICANT_TRAFFIC_IMPACT,
                            order.SignificantTrafficImpact);
                    }
                    break;
                default:
                    throw new NotImplementedException(
                        String.Format(
                            "Asset verification for asset type '{0}' not yet implemented.",
                            order.AssetType));
            }
            selenium.AssertAttribute(NecessaryIDs.IMG_SHOW_PICKER, "src",
                Config.GetDevSiteUrl() +
                "/modules/WorkOrders/Includes/map-icon-blue.png");
        }

        private static void AlterCrewAssignmentValues(ISelenium selenium, Types.WorkOrder order, Types.CrewAssignment assignment)
        {
            Navigate.FindAndSelectOrder(selenium, order);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_EDIT);
            selenium.Click(NecessaryIDs.LNK_CREW_ASSIGNMENTS_TAB);
            selenium.Click(NecessaryIDs.LNK_CREW_ASSIGNMENT_EDIT);
            selenium.WaitForElementPresent(
                NecessaryIDs.TXT_CREW_ASSIGNMENT_DATE_STARTED);
            selenium.RunScript("$('#{0}').val('{1}');",
                NecessaryIDs.TXT_CREW_ASSIGNMENT_DATE_STARTED,
                assignment.DateStarted);
            selenium.RunScript("$('#{0}').val('{1}');",
                NecessaryIDs.TXT_CREW_ASSIGNMENT_DATE_ENDED,
                assignment.DateEnded);
            selenium.Type(NecessaryIDs.TXT_CREW_ASSIGNMENT_EMPLOYEES_ON_CREW,
                assignment.EmployeesOnCrew);
            selenium.Click(NecessaryIDs.LNK_CREW_ASSIGNMENT_UPDATE);
        }

        private static void ValidateCrewAssignmentValues(ISelenium selenium, Types.CrewAssignment assignment)
        {
            selenium.WaitForElementPresent(
                NecessaryIDs.LBL_CREW_ASSIGNMENT_DATE_STARTED);
            selenium.AssertText(NecessaryIDs.LBL_CREW_ASSIGNMENT_DATE_STARTED,
                DateTime.Parse(assignment.DateStarted).ToString());
            selenium.AssertText(NecessaryIDs.LBL_CREW_ASSIGNMENT_DATE_ENDED,
                DateTime.Parse(assignment.DateEnded).ToString());
            selenium.AssertText(
                NecessaryIDs.LBL_CREW_ASSIGNMENT_EMPLOYEES_ON_CREW,
                assignment.EmployeesOnCrew);
        }

        private static void VerifyOrderNotesHaveNotChanged(ISelenium selenium, Types.WorkOrder order)
        {
            selenium.AssertText(NecessaryIDs.LBL_NOTES, order.Notes);
        }

        #endregion
    }
}
