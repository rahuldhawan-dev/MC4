using MMSINC.Testing.Selenium;
using Selenium;
using System;
using System.Data.SqlTypes;

namespace RegressionTests.Lib.TestParts
{
    public static class Assign
    {
        #region Constants

        public struct NecessaryIDs
        {
            public const string TXT_SEARCH_WORK_ORDER_NUMBER =
                Global.CONTROL_BASE_ID + "wosrv_wosvWorkOrders_baseSearch_txtWorkOrderNumber";

            public const string DDL_SEARCH_OPERATING_CENTER =
                Global.CONTROL_BASE_ID + "wosrv_wosvWorkOrders_baseSearch_ddlOperatingCenter",
                                          DDL_CREW =
                                              Global.CONTROL_BASE_ID + "wosrv_wolvWorkOrders_ddlCrewID";

            public const string BTN_SEARCH =
                Global.CONTROL_BASE_ID + "wosrv_wosvWorkOrders_btnSearch",
                                          BTN_ASSIGN =
                                              Global.CONTROL_BASE_ID + "wosrv_wolvWorkOrders_btnAssign";

            public const string CHK_ASSIGN_TO_CREW =
                Global.CONTROL_BASE_ID + "wosrv_wolvWorkOrders_gvWorkOrders_chkAssignToCrew_0";

            public const string CC_ASSIGNMENT_DATE =
                Global.CONTROL_BASE_ID + "wosrv_wolvWorkOrders_ccAssignmentDate",
                                          CC_ASSIGNMENT_DATE_TODAY =
                                              Global.CONTROL_BASE_ID + "wosrv_wolvWorkOrders_ceAssignmentDate_today";
        }

        #endregion

        #region Exposed Static Methods

        public static Types.CrewAssignment OrderToDefaultCrew(ISelenium selenium, Types.WorkOrder order)
        {
            var assignment = new Types.CrewAssignment {
                Crew = "Edwards" // This needed to be updated since crews were renamed in prod
            };
            selenium.Type(NecessaryIDs.TXT_SEARCH_WORK_ORDER_NUMBER,
                order.WorkOrderID);
            selenium.WaitThenSelectLabel(
                NecessaryIDs.DDL_SEARCH_OPERATING_CENTER, order.OperatingCenter);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_SEARCH);
            selenium.Click(NecessaryIDs.CHK_ASSIGN_TO_CREW);
            selenium.WaitThenSelectLabel(NecessaryIDs.DDL_CREW, assignment.Crew);
            assignment.CrewID = selenium.GetSelectedValue(NecessaryIDs.DDL_CREW);
            selenium.Type(NecessaryIDs.CC_ASSIGNMENT_DATE, DateTime.Now.ToString());
            assignment.AssignedDate =
                selenium.GetText(NecessaryIDs.CC_ASSIGNMENT_DATE);
            selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_ASSIGN);
            return assignment;
        }

        #endregion
    }
}
