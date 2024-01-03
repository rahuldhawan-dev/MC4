using MMSINC.Testing.Selenium;
using Selenium;
using System;

namespace RegressionTests.Lib.TestParts
{
    public static class Crew
    {
        #region Constants

	    public struct NecessaryIDs
	    {
            public const string 
                DDL_CREW = Global.CONTROL_BASE_ID + "carv_lvCrewAssignmentsSearchView_ddlCrew",
                TXT_CREW_ASSIGNMENTS_DATE = Global.CONTROL_BASE_ID + "carv_lvCrewAssignmentsSearchView_ccDate";

	    }

	    #endregion

        #region Public Static Methods

        public static void StartWork(ISelenium selenium, Types.WorkOrder order, Types.CrewAssignment assignment)
        {
            // Because we're relying on session state we need to make sure we reset the form to the normal default for this one
            selenium.WaitForElementPresent(NecessaryIDs.TXT_CREW_ASSIGNMENTS_DATE);
            selenium.RunScript("$('#{0}').val('{1}');", NecessaryIDs.TXT_CREW_ASSIGNMENTS_DATE, DateTime.Now.ToShortDateString());

            selenium.SelectLabel(NecessaryIDs.DDL_CREW, order.OperatingCenter.Substring(0, 3) + " " + order.OperatingCenterName + " - " + assignment.Crew);
            selenium.FireEvent(NecessaryIDs.DDL_CREW, "blur");
            selenium.WaitForPageToLoad(Globals.DEFAULT_TIMEOUT);
            var link = selenium.GetLastLinkIdInTable("gvCrewAssignments",
                "lbStart");
            selenium.ClickAndWaitForPageToLoad(link);
        }

        #endregion
    }
}
