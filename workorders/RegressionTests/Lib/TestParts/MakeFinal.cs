using MMSINC.Testing.Selenium;
using Selenium;
using System;
using System.Text.RegularExpressions;

namespace RegressionTests.Lib.TestParts
{
    public static class MakeFinal
    {
        public static class FromCrewAssignmentsScreen
        {
            #region Constants

            public struct NecessaryIDs
            {
                public const string LNK_CREW_ASSIGNMENTS =
                    "link=Crew Assignments";

                public const string DDL_SEARCH_CREW = Global.CONTROL_BASE_ID + "carv_lvCrewAssignmentsSearchView_ddlCrew",
                                    DDL_MATERIALS_USED_PART_NUMBER =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_womufMaterialsUsed_gvMaterialsUsed_ddlPartNumber",
                                    DDL_MATERIALS_USED_STOCK_LOCATION =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_womufMaterialsUsed_gvMaterialsUsed_ddlStockLocation",
                                    DDL_STORAGE_LOCATION =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_wospSpoils_gvSpoils_ddlSpoilStorageLocation",
                                    DDL_RESTORATION_TYPE =
                                        "ctl03_rdvRestoration_fvRestoration_ddlRestorationType",
                                    DDL_MAIN_BREAK_MATERIAL =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlMaterial",
                                    DDL_MAIN_BREAK_MAIN_CONDITION =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlCondition",
                                    DDL_MAIN_BREAK_FAILURE_TYPE =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlFailureType",
                                    DDL_MAIN_BREAK_SOIL_CONDITION =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlSoilCondition",
                                    DDL_MAIN_BREAK_SIZE =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlServiceSize",
                                    DDL_MAIN_BREAK_DISINFECTION_METHOD =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlDisinfectionMethod",
                                    DDL_MAIN_BREAK_FLUSH_METHOD =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlFlushMethod";

                public const string TAB_MATERIALS_USED = 
                                        "//a[@id='materialsUsedTab']/span",
                                    TAB_CREW_ASSIGNMENTS =
                                        "//a[@id='crewAssignmentsTab']/span",
                                    TAB_SPOILS =
                                        "//a[@id='spoilsTab']/span",
                                    TAB_RESTORATION =
                                        "//a[@id='restorationTab']/span",
                                    TAB_MAIN_BREAK =
                                        "//a[@id='mainBreakTab']/span",
                                    TAB_ADDITIONAL =
                                        "//a[@id='additionalTab']/span";


                public const string TXT_MATERIALS_USED_QUANTITY = 
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_womufMaterialsUsed_gvMaterialsUsed_txtQuantity",
                                    TXT_CREW_ASSIGNMENTS_EMPLOYEES_ON_JOB =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_woCrewAssignments_gvCrewAssignments_txtEmployeesOnJob_0",
                                    TXT_DATE_COMPLETED =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_txtDateCompleted",
                                    TXT_SPOILS_QUANTITY =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_wospSpoils_gvSpoils_txtQuantity",
                                    TXT_RESTORATION_FOOTAGE =
                                        "ctl03_rdvRestoration_fvRestoration_txtPavingSquareFootage",
                                    TXT_MAIN_BREAK_DEPTH =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_txtDepth",
                                    TXT_MAIN_BREAK_CUSTOMERS_AFFECTED =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_txtCustomersAffected",
                                    TXT_MAIN_BREAK_SHUT_DOWN_TIME =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_txtShutdownTime",
                                    TXT_MAIN_BREAK_CHLORINE_RESIDUAL =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_txtChlorineResidual",
                                    TXT_LOST_WATER = Global.CONTROL_BASE_ID +
                                                     "ctl00_wodvWorkOrder_fvWorkOrder_woafiAdditionalInfo_fvWorkOrder_txtLostWater",
                                    TXT_DISTANCE_FROM_CROSS_STREET =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_woafiAdditionalInfo_fvWorkOrder_txtDistanceFromCrossStreet",
                                    TXT_CREW_ASSIGNMENTS_DATE = Global.CONTROL_BASE_ID +
                                        "carv_lvCrewAssignmentsSearchView_ccDate";

                public const string LB_MATERIALS_USED_INSERT =
                    Global.CONTROL_BASE_ID +
                    "ctl00_wodvWorkOrder_fvWorkOrder_womufMaterialsUsed_gvMaterialsUsed_lbInsert",
                                    LB_CREW_ASSIGNMENTS_END =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_woCrewAssignments_gvCrewAssignments_lbEnd_0",
                                    LB_SPOILS_INSERT =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_wospSpoils_gvSpoils_lbInsert",
                                    LB_RESTORATION_INSERT =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_wofrRestorations_gvRestorations_lbInsertRestoration",
                                    LB_SPOILS_SECOND_EDIT =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_wospSpoils_gvSpoils_lbEdit_1",
                                    LB_MAIN_BREAK_INSERT =
                                        Global.CONTROL_BASE_ID +
                                        "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_lbInsert",
                                    LB_UPDATE = Global.CONTROL_BASE_ID +
                                                "ctl00_wodvWorkOrder_fvWorkOrder_woafiAdditionalInfo_fvWorkOrder_lbUpdate";
                                                                  
                public const string LBL_MATERIALS_USED_DESCRIPTION =
                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_womufMaterialsUsed_gvMaterialsUsed_lblDescription_0",
                                    LBL_CREW_ASSIGNMENTS_END_DATE =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_woCrewAssignments_gvCrewAssignments_lblEndDate_0",
                                    LBL_COMPLETED_BY =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_ctl01_fvWorkOrder_lblCompletedBy",
                                    LBL_DATE_COMPLETED =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_ctl01_fvWorkOrder_lblDateCompleted";

                public const string DP_DATE_COMPLETED_TODAY =
                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_dpDateCompleted_today";

                public const string BTN_SAVE =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_btnSave",
                                    BTN_RESTORATION_SAVE =
                                        "ctl03_rdvRestoration_btnSave", 
                                    BTN_RESTORATION_DONE = "btnDone";
            }

            #endregion

            #region Private Helper Methods

            private static void Noop(ISelenium arg1, Types.WorkOrder arg2, Types.CrewAssignment arg3) {}

            internal static void EnterMainBreakInfo(ISelenium selenium, Types.MainBreak mainBreak)
            {
                selenium.Click(NecessaryIDs.TAB_MAIN_BREAK);
                selenium.SelectLabel(NecessaryIDs.DDL_MAIN_BREAK_MATERIAL,
                    mainBreak.Material);
                selenium.SelectLabel(
                    NecessaryIDs.DDL_MAIN_BREAK_MAIN_CONDITION,
                    mainBreak.MainCondition);
                selenium.SelectLabel(NecessaryIDs.DDL_MAIN_BREAK_FAILURE_TYPE,
                    mainBreak.FailureType);
                selenium.Type(NecessaryIDs.TXT_MAIN_BREAK_DEPTH, mainBreak.Depth);
                selenium.SelectLabel(
                    NecessaryIDs.DDL_MAIN_BREAK_SOIL_CONDITION,
                    mainBreak.SoilCondition);
                selenium.Type(NecessaryIDs.TXT_MAIN_BREAK_CUSTOMERS_AFFECTED,
                    mainBreak.CustomersAffected);
                selenium.Type(NecessaryIDs.TXT_MAIN_BREAK_SHUT_DOWN_TIME,
                    mainBreak.ShutDownTime);
                selenium.SelectLabel(
                    NecessaryIDs.DDL_MAIN_BREAK_DISINFECTION_METHOD,
                    mainBreak.DisinfectionMethod);
                selenium.SelectLabel(NecessaryIDs.DDL_MAIN_BREAK_FLUSH_METHOD,
                    mainBreak.FlushMethod);
                selenium.Type(NecessaryIDs.TXT_MAIN_BREAK_CHLORINE_RESIDUAL,
                    mainBreak.ChlorineResidual);
                selenium.SelectLabel(NecessaryIDs.DDL_MAIN_BREAK_SIZE,
                    mainBreak.Size);
                selenium.ClickAndWait(NecessaryIDs.LB_MAIN_BREAK_INSERT, 5);

                selenium.Click(NecessaryIDs.TAB_ADDITIONAL);
                selenium.Type(NecessaryIDs.TXT_LOST_WATER, "2000000");
                selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LB_UPDATE);
            }

            #endregion

            #region Public Static Methods

            public static void OrderWithMarkoutForMainBreak(ISelenium selenium, ref Types.WorkOrder order, ref Types.CrewAssignment assignment, string shouldBeCompletedBy)
            {
                var mainBreak = new Types.MainBreak {
                    Material = "Cement",
                    MainCondition = "Fair",
                    FailureType = "Circular",
                    Depth = "1.00",
                    SoilCondition = "Clay",
                    CustomersAffected = "12",
                    ShutDownTime = "8.00",
                    DisinfectionMethod = "Chlorination",
                    FlushMethod = "Blowoff",
                    ChlorineResidual = "0.1",
                    Size = "15"
                };
                OrderWithMarkout(selenium, ref order, ref assignment,
                    shouldBeCompletedBy,
                    (s, o, a) => {
                        EnterMainBreakInfo(s, mainBreak);
                        s.Open(s.GetLocation());
                    });
            }

            public static void OrderWithMarkout(ISelenium selenium, ref Types.WorkOrder order, ref Types.CrewAssignment assignment, string shouldBeCompletedBy, Action<ISelenium, Types.WorkOrder, Types.CrewAssignment> beforeEndingAssignment = null, Action<ISelenium, Types.WorkOrder, Types.CrewAssignment> beforeSaving = null, Action<ISelenium, Types.WorkOrder, Types.CrewAssignment> extraAssertions = null)
            {
                Data.FixMaterial();

                beforeEndingAssignment = beforeEndingAssignment ?? Noop;
                beforeSaving = beforeSaving ?? Noop;
                extraAssertions = extraAssertions ?? Noop;

                StartCrewAssignment(selenium, order, assignment);

                beforeEndingAssignment(selenium, order, assignment);

                selenium.Click(NecessaryIDs.TAB_MATERIALS_USED);
                selenium.Type(NecessaryIDs.TXT_MATERIALS_USED_QUANTITY, "1");
                selenium.WaitThenSelectLabel(
                    NecessaryIDs.DDL_MATERIALS_USED_PART_NUMBER, "1600040");
                selenium.WaitThenSelectLabel(
                    NecessaryIDs.DDL_MATERIALS_USED_STOCK_LOCATION, "H&M");
                selenium.Click(NecessaryIDs.LB_MATERIALS_USED_INSERT);
                selenium.Click(NecessaryIDs.TAB_MATERIALS_USED);
                selenium.WaitForText(NecessaryIDs.LBL_MATERIALS_USED_DESCRIPTION, "Nipple, 2\" X 6\" BR"); // Note the site strips any spaces you might have in the description.
                selenium.Click(NecessaryIDs.TAB_CREW_ASSIGNMENTS);
                selenium.Type(NecessaryIDs.TXT_CREW_ASSIGNMENTS_EMPLOYEES_ON_JOB,
                    "5");
                
                selenium.Click(NecessaryIDs.LB_CREW_ASSIGNMENTS_END);
                selenium.WaitForElementPresent(
                    NecessaryIDs.LBL_CREW_ASSIGNMENTS_END_DATE);
                selenium.Click(NecessaryIDs.TXT_DATE_COMPLETED);
                //Completion Date
                selenium.Click(NecessaryIDs.DP_DATE_COMPLETED_TODAY);

                beforeSaving(selenium, order, assignment);

                selenium.ClickAndWait(NecessaryIDs.BTN_SAVE, 25); // SAP is SLOW
                if (selenium.IsConfirmationPresent())
                {
                    selenium.AssertConfirmation("Please Update Leak Repair Location on the Mobile GIS Map Now");
                }
                selenium.AssertText(NecessaryIDs.LBL_COMPLETED_BY,
                    shouldBeCompletedBy);
                selenium.AssertText(NecessaryIDs.LBL_DATE_COMPLETED,
                    new Regex(@"^\d+/\d+/\d+$"));
                order.DateCompleted =
                    selenium.GetText(NecessaryIDs.LBL_DATE_COMPLETED);

                extraAssertions(selenium, order, assignment);
            }

            public static void OrderWithSpoilAndRestoration(ISelenium selenium, ref Types.WorkOrder order, ref Types.CrewAssignment assignment)
            {
                StartCrewAssignment(selenium, order, assignment);
                // Add a spoil
                selenium.Click(NecessaryIDs.TAB_SPOILS);
                selenium.Type(NecessaryIDs.TXT_SPOILS_QUANTITY, "1000.25");
                selenium.SelectValue(NecessaryIDs.DDL_STORAGE_LOCATION, "1");
                selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LB_SPOILS_INSERT);

                // Restorations are done by MVC now so this just breaks.
                // Add a restoration
               // selenium.Click(NecessaryIDs.TAB_RESTORATION);
                // having trouble using waitForPopup here because this is an asp.net 
                // postback link and we don't have an ID for the opened window.
              //  selenium.ClickAndWait(NecessaryIDs.LB_RESTORATION_INSERT, 10);
                //var windows = selenium.GetAllWindowTitles();
                //selenium.SelectWindow(windows[1]);
                
                //selenium.SelectValue(NecessaryIDs.DDL_RESTORATION_TYPE, "1");
                //selenium.Type(NecessaryIDs.TXT_RESTORATION_FOOTAGE, "25");
                //selenium.ClickAndWaitForPageToLoad(
                //    NecessaryIDs.BTN_RESTORATION_SAVE);
                //selenium.Click(NecessaryIDs.BTN_RESTORATION_DONE);

                //// Assert only one spoil exists
               // selenium.SelectWindow(windows[0]);
                selenium.Click(NecessaryIDs.TAB_SPOILS);
                // if there are two Spoil Edit buttons something went wrong.
                selenium.AssertElementIsNotPresent(NecessaryIDs.LB_SPOILS_SECOND_EDIT);
            }

            #endregion

            #region Private Static Methods

            private static void StartCrewAssignment(ISelenium selenium, Types.WorkOrder order, Types.CrewAssignment assignment)
            {
                selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_CREW_ASSIGNMENTS);

                // Because we're relying on session state we need to make sure we reset the form to the normal default for this one
                selenium.WaitForElementPresent(NecessaryIDs.TXT_CREW_ASSIGNMENTS_DATE);
                selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_CREW_ASSIGNMENTS_DATE, DateTime.Now.ToShortDateString());

                //selenium.SelectAndWaitForPageToLoad(NecessaryIDs.DDL_SEARCH_CREW,assignment.CrewID);
                selenium.WaitThenSelectLabel(NecessaryIDs.DDL_SEARCH_CREW, order.OperatingCenter.Substring(0, 3) + " " + order.OperatingCenterName + " - " + assignment.Crew);
                selenium.FireEvent(NecessaryIDs.DDL_SEARCH_CREW, "blur");
                selenium.WaitForPageToLoad("300000"); // because of the tunnel to RDS on the agent will take some time, increasing to 300 seconds


                selenium.AssertTextPresent(order.WorkOrderID);
                selenium.ClickAndWaitForPageToLoad(
                    String.Format("//tr[td[span={0}]]/td[19]/a", order.WorkOrderID));
            }

            #endregion
        }

        public static class FromFinalizationScreen
        {
            #region Constants

            public struct NecessaryIDs
            {
                public const string LNK_FINALIZATION = "link=Finalization",
                                    LNK_SELECT = "link=Select",
                                    LNK_UPDATE = "link=Update",
                                    LNK_MAIN_BREAK_INSERT =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_lbInsert";

                public const string 
                    TXT_SEARCH_WORKORDER_ID =
                        Global.CONTROL_BASE_ID + "wofrvWorkOrders_wosvWorkOrders_baseSearch_txtWorkOrderNumber", 
                    TXT_MAIN_BREAK_DEPTH =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_txtDepth",
                    TXT_MAIN_BREAK_CUSTOMERS_AFFECTED =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_txtCustomersAffected",
                    TXT_MAIN_BREAK_SHUT_DOWN_TIME =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_txtShutdownTime",
                    TXT_MAIN_BREAK_CHLORINE_RESIDUAL =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_txtChlorineResidual",
                    TXT_DATE_COMPLETED =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_txtDateCompleted",
                    TXT_LOST_WATER_ID =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_woafiAdditionalInfo_fvWorkOrder_txtLostWater";


                public const string DP_DATE_COMPLETED_TODAY =
                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_dpDateCompleted_today";

                public const string BTN_SEARCH =
                    Global.CONTROL_BASE_ID + "wofrvWorkOrders_wosvWorkOrders_btnSearch",
                                    BTN_FINALIZE =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_btnSave";

                public const string TAB_ADDITIONAL =
                    "//a[@id='additionalTab']/span",
                                    TAB_MAIN_BREAK =
                                        "//a[@id='mainBreakTab']/span";

                public const string 
                    DDL_FINAL_WORK_DESCRIPTION =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_woafiAdditionalInfo_fvWorkOrder_ddlFinalWorkDescription",
                    DDL_FINAL_CUSTOMER_IMPACT =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_woafiAdditionalInfo_fvWorkOrder_ddlFinalCustomerImpactRange",
                    DDL_FINAL_SIGNIFICANT_TRAFFIC_IMPACT =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_woafiAdditionalInfo_fvWorkOrder_ddlFinalSignificantTrafficImpact",
                    DDL_FINAL_ALERT_ISSUED =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_woafiAdditionalInfo_fvWorkOrder_ddlFinalAlertIssued",
                    DDL_FINAL_REPAIR_TIME =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_woafiAdditionalInfo_fvWorkOrder_ddlFinalRepairTimeRange",
                    DDL_MAIN_BREAK_MATERIAL =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlMaterial",
                    DDL_MAIN_BREAK_MAIN_CONDITION =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlCondition",
                    DDL_MAIN_BREAK_FAILURE_TYPE =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlFailureType",
                    DDL_MAIN_BREAK_SOIL_CONDITION =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlSoilCondition",
                    DDL_MAIN_BREAK_DISINFECTION_METHOD =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlDisinfectionMethod",
                    DDL_MAIN_BREAK_FLUSH_METHOD =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlFlushMethod",
                    DDL_MAIN_BREAK_SIZE =
                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_ddlServiceSize";

                public const string LBL_INITIAL_INFO_DESCRIPTION_OF_WORK =
                    Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_ctl01_fvWorkOrder_lblDescriptionOfWork",
                                    LBL_INITIAL_INFO_ESTIMATED_CUSTOMER_IMPACT =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_ctl01_fvWorkOrder_lblCustomerImpactRange",
                                    LBL_INITIAL_INFO_ANTICIPATED_REPAIR_TIME =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_ctl01_fvWorkOrder_lblRepairTimeRange",
                                    LBL_MAIN_BREAK_MATERIAL =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_lblMaterial_0",
                                    LBL_MAIN_BREAK_MAIN_CONDITION =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_lblCondition_0",
                                    LBL_MAIN_BREAK_FAILURE_TYPE =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_lblFailureType_0",
                                    LBL_MAIN_BREAK_DEPTH =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_lblDepth_0",
                                    LBL_MAIN_BREAK_SOIL_CONDITION =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_lbSoilCondition_0",
                                    LBL_MAIN_BREAK_CUSTOMERS_AFFECTED =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_lbCustomersAffected_0",
                                    LBL_MAIN_BREAK_SHUT_DOWN_TIME =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_lbShutdownTime_0",
                                    LBL_MAIN_BREAK_DISINFECTION_METHOD =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_lbDisinfectionMethod_0",
                                    LBL_MAIN_BREAK_FLUSH_METHOD =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_lbFlushMethod_0",
                                    LBL_MAIN_BREAK_CHLORINE_RESIDUAL =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_lbChlorineResidual_0",
                                    LBL_MAIN_BREAK_SIZE =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_WorkOrderMainBreakForm_gvMainBreak_lbServiceSize_0",
                                    LBL_DATE_COMPLETED =
                                        Global.CONTROL_BASE_ID + "ctl00_wodvWorkOrder_fvWorkOrder_ctl01_fvWorkOrder_lblDateCompleted";
            }

            #endregion

            #region Public Static Methods

            public static Types.MainBreak OrderForMainBreak(ISelenium selenium, ref Types.WorkOrder order)
            {
                var mainBreak = new Types.MainBreak {
                    Material = "Cement",
                    MainCondition = "Fair",
                    FailureType = "Circular",
                    Depth = "1.00",
                    SoilCondition = "Clay",
                    CustomersAffected = "12",
                    ShutDownTime = "8.00",
                    DisinfectionMethod = "Chlorination",
                    FlushMethod = "Blowoff",
                    ChlorineResidual = "0.1",
                    Size = "15"
                };
                order.DescriptionOfWork = "WATER MAIN BREAK REPAIR";
                order.LostWater = "10";
                order.CustomerImpactRange = "51-100";
                order.RepairTimeRange = "8-10";
                order.AlertIssued = "Yes";
                order.SignificantTrafficImpact = "No";

                VisitOrderInFinalization(selenium, order);
                EnterAdditionalFinalizationInfo(selenium, order);
                FinalizeOrder(selenium, mainBreak);
                VerifyEnteredData(selenium, ref order, mainBreak);
                return mainBreak;
            }

            #endregion

            #region Private Static Methods

            private static void VisitOrderInFinalization(ISelenium selenium, Types.WorkOrder order)
            {
                selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_FINALIZATION);
                selenium.Type(NecessaryIDs.TXT_SEARCH_WORKORDER_ID,
                    order.WorkOrderID);
                selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SEARCH);
                selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_SELECT);
            }

            private static void EnterAdditionalFinalizationInfo(ISelenium selenium, Types.WorkOrder order)
            {
                selenium.Click(NecessaryIDs.TAB_ADDITIONAL);
                selenium.SelectLabel(NecessaryIDs.DDL_FINAL_WORK_DESCRIPTION, order.DescriptionOfWork);
                selenium.Type(NecessaryIDs.TXT_LOST_WATER_ID, order.LostWater);
                
                selenium.SelectLabel(NecessaryIDs.DDL_FINAL_CUSTOMER_IMPACT,
                    order.CustomerImpactRange);
                selenium.SelectLabel(NecessaryIDs.DDL_FINAL_ALERT_ISSUED, order.AlertIssued);
                selenium.SelectLabel(
                    NecessaryIDs.DDL_FINAL_SIGNIFICANT_TRAFFIC_IMPACT,
                    order.SignificantTrafficImpact);

                // without selection for repair time range
                selenium.Click(NecessaryIDs.LNK_UPDATE);
                selenium.AssertTextPresent(
                    "Required when work description is for a main break.");

                selenium.SelectLabel(NecessaryIDs.DDL_FINAL_CUSTOMER_IMPACT,
                    "--Select Here--");
                selenium.SelectLabel(NecessaryIDs.DDL_FINAL_REPAIR_TIME,
                    order.RepairTimeRange);

                // without selection for customer impact
                selenium.Click(NecessaryIDs.LNK_UPDATE);
                selenium.AssertTextPresent(
                    "Required when work description is for a main break.");

                selenium.SelectLabel(NecessaryIDs.DDL_FINAL_CUSTOMER_IMPACT,
                    order.CustomerImpactRange);
                selenium.SelectLabel(
                    NecessaryIDs.DDL_FINAL_SIGNIFICANT_TRAFFIC_IMPACT,
                    "--Select Here--");

                // without selection for significant traffic impact
                selenium.Click(NecessaryIDs.LNK_UPDATE);
                selenium.AssertTextPresent(
                    "Required when work description is for a main break.");

                selenium.SelectLabel(
                    NecessaryIDs.DDL_FINAL_SIGNIFICANT_TRAFFIC_IMPACT,
                    order.SignificantTrafficImpact);

                // without selection for lost water
                selenium.Type(NecessaryIDs.TXT_LOST_WATER_ID, "");
                selenium.FireEvent(NecessaryIDs.TXT_LOST_WATER_ID, "blur");
                selenium.Click(NecessaryIDs.LNK_UPDATE);
                selenium.AssertTextPresent(
                    "Required when work description is for a main break.");
                
                selenium.Type(NecessaryIDs.TXT_LOST_WATER_ID, order.LostWater);
                selenium.FireEvent(NecessaryIDs.TXT_LOST_WATER_ID, "blur");

                // REMOVED 2017-01-10 bug 3471
                // without a main break record
                //selenium.Click(NecessaryIDs.LNK_UPDATE);
                //selenium.AssertTextPresent(
                //    "The final work description indicates a main break. Please enter the main break information by clicking the Main Break tab.");
                
                //EnterMainBreakInfo(selenium, mainBreak);
                //selenium.Click(NecessaryIDs.TAB_ADDITIONAL);

                selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LNK_UPDATE);
                selenium.Click(NecessaryIDs.TAB_ADDITIONAL);
                selenium.AssertSelectedLabel(
                    NecessaryIDs.DDL_FINAL_WORK_DESCRIPTION,
                    order.DescriptionOfWork);
                selenium.AssertSelectedLabel(NecessaryIDs.DDL_FINAL_CUSTOMER_IMPACT,
                    order.CustomerImpactRange);
                selenium.AssertSelectedLabel(NecessaryIDs.DDL_FINAL_REPAIR_TIME,
                    order.RepairTimeRange);
            }

            private static void EnterMainBreakInfo(ISelenium selenium, Types.MainBreak mainBreak)
            {
                selenium.Click(NecessaryIDs.TAB_MAIN_BREAK);
                selenium.SelectLabel(NecessaryIDs.DDL_MAIN_BREAK_MATERIAL,
                    mainBreak.Material);
                selenium.SelectLabel(
                    NecessaryIDs.DDL_MAIN_BREAK_MAIN_CONDITION,
                    mainBreak.MainCondition);
                selenium.SelectLabel(NecessaryIDs.DDL_MAIN_BREAK_FAILURE_TYPE,
                    mainBreak.FailureType);
                selenium.Type(NecessaryIDs.TXT_MAIN_BREAK_DEPTH, mainBreak.Depth);
                selenium.SelectLabel(
                    NecessaryIDs.DDL_MAIN_BREAK_SOIL_CONDITION,
                    mainBreak.SoilCondition);
                selenium.Type(NecessaryIDs.TXT_MAIN_BREAK_CUSTOMERS_AFFECTED,
                    mainBreak.CustomersAffected);
                selenium.Type(NecessaryIDs.TXT_MAIN_BREAK_SHUT_DOWN_TIME,
                    mainBreak.ShutDownTime);
                selenium.SelectLabel(
                    NecessaryIDs.DDL_MAIN_BREAK_DISINFECTION_METHOD,
                    mainBreak.DisinfectionMethod);
                selenium.SelectLabel(NecessaryIDs.DDL_MAIN_BREAK_FLUSH_METHOD,
                    mainBreak.FlushMethod);
                selenium.Type(NecessaryIDs.TXT_MAIN_BREAK_CHLORINE_RESIDUAL,
                    mainBreak.ChlorineResidual);
                selenium.SelectLabel(NecessaryIDs.DDL_MAIN_BREAK_SIZE,
                    mainBreak.Size);
                selenium.ClickAndWait(NecessaryIDs.LNK_MAIN_BREAK_INSERT, 5);
            }

            private static void FinalizeOrder(ISelenium selenium, Types.MainBreak mainBreak)
            {
                selenium.Click(NecessaryIDs.TXT_DATE_COMPLETED);
                selenium.Click(NecessaryIDs.DP_DATE_COMPLETED_TODAY);
                EnterMainBreakInfo(selenium, mainBreak);
                //selenium.Click(NecessaryIDs.TAB_ADDITIONAL);
                selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_FINALIZE);
                if (selenium.IsConfirmationPresent())
                {
                    selenium.AssertConfirmation("Please Update Leak Repair Location on the Mobile GIS Map Now");
                }
            }

            private static void VerifyEnteredData(ISelenium selenium, ref Types.WorkOrder order, Types.MainBreak mainBreak)
            {
                selenium.AssertText(
                    NecessaryIDs.LBL_INITIAL_INFO_DESCRIPTION_OF_WORK,
                    order.DescriptionOfWork);
                selenium.AssertText(
                    NecessaryIDs.LBL_INITIAL_INFO_ESTIMATED_CUSTOMER_IMPACT,
                    order.CustomerImpactRange);
                selenium.AssertText(
                    NecessaryIDs.LBL_INITIAL_INFO_ANTICIPATED_REPAIR_TIME,
                    order.RepairTimeRange);
                selenium.AssertText(NecessaryIDs.LBL_MAIN_BREAK_MATERIAL,
                    mainBreak.Material);
                selenium.AssertText(NecessaryIDs.LBL_MAIN_BREAK_MAIN_CONDITION,
                    mainBreak.MainCondition);
                selenium.AssertText(NecessaryIDs.LBL_MAIN_BREAK_FAILURE_TYPE,
                    mainBreak.FailureType);
                selenium.AssertText(NecessaryIDs.LBL_MAIN_BREAK_DEPTH,
                    mainBreak.Depth);
                selenium.AssertText(NecessaryIDs.LBL_MAIN_BREAK_SOIL_CONDITION,
                    mainBreak.SoilCondition);
                selenium.AssertText(
                    NecessaryIDs.LBL_MAIN_BREAK_CUSTOMERS_AFFECTED,
                    mainBreak.CustomersAffected);
                selenium.AssertText(NecessaryIDs.LBL_MAIN_BREAK_SHUT_DOWN_TIME,
                    mainBreak.ShutDownTime);
                selenium.AssertText(
                    NecessaryIDs.LBL_MAIN_BREAK_DISINFECTION_METHOD,
                    mainBreak.DisinfectionMethod);
                selenium.AssertText(NecessaryIDs.LBL_MAIN_BREAK_FLUSH_METHOD,
                    mainBreak.FlushMethod);
                selenium.AssertText(
                    NecessaryIDs.LBL_MAIN_BREAK_CHLORINE_RESIDUAL,
                    mainBreak.ChlorineResidual);
                selenium.AssertText(NecessaryIDs.LBL_MAIN_BREAK_SIZE,
                    mainBreak.Size);

                order.DateCompleted =
                    selenium.GetText(NecessaryIDs.LBL_DATE_COMPLETED);
            }

            #endregion
        }

    }
}
