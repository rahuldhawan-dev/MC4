using MMSINC.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib;
using Selenium;

namespace RegressionTests.Tests
{
    public class ReportsStarTest : BaseSearchTest<ReportsStarTest.ReportTestPage>
    {
        #region Fields

        public static readonly ReportTestPage[] PAGES = new [] {
             new ReportTestPage("/Reports/Data/Services/FireServiceEstimatedBackBilling.aspx"),
             new ReportTestPage("/Reports/Data/Services/FireServiceMonthlyBilling.aspx"),
             new ReportTestPage("/Reports/Data/Services/FireServiceRecoveredBilling.aspx"),
             new ReportTestPage("/Reports/HR/Employee/SickBankHours.aspx"),
             new ReportTestPage("/Reports/FieldServices/InterconnectionInspections.aspx"),
             new ReportTestPage("/Reports/FieldServices/JobObservationSummary.aspx"),
             new ReportTestPage("/Reports/FieldServices/InterconnectionInspections.aspx"),
             new ReportTestPage("/Reports/FieldServices/tailgatetalksSummary.aspx"),
             new ReportTestPage("/Reports/HR/Employee/TrainingRecordsAttendance.aspx"),
             //new ReportTestPage("/Reports/HR/Employee/TrainingRecordsTrainingHours.aspx"),
             new ReportTestPage("/Reports/HR/Employee/EmployeeEssentialEmergencyResponsePriority.aspx"),
             //new ReportTestPage("/modules/WorkOrders/Views/Reports/MainBreaksAndServiceLineRepairs.aspx") {
             //    NarrahDownSearch = s =>
             //        s.SelectLabel("content_cphMain_cphMain_ddlYear", "2010"),
             //    SuccessText = "Completed Work Orders"
             //},
             new ReportTestPage("/Reports/FieldServices/SewerMainCleaningFootage.aspx"),
             new ReportTestPage("/Reports/FieldServices/tailgatetalksattendance.aspx") {
                 NarrahDownSearch = s =>
                     s.SelectLabel("content_cphMain_cphMain_dfTopicCategory_ddlDataField", "Unsafe Acts")
             },
             new ReportTestPage("/Reports/HR/Employee/emp_Licenses.aspx") {
                 NarrahDownSearch = s => s.SelectLabel("content_cphMain_cphMain_ddlOpCode", "NJ7"),
                 SearchButton = "content_cphMain_cphMain_btnView",
                 SuccessText = "Export"
             },
             new ReportTestPage("/Reports/HR/Employee/EmployeePositionControlReport.aspx") {
                 NarrahDownSearch = s => s.SelectLabel("content_cphMain_cphMain_template_searchOpCenter", "NJ7 - Shrewsbury"),
                 SearchButton = "btnSearch"
             },
             new ReportTestPage("/Reports/HR/Employee/emp_Seniority.aspx") {
                 NarrahDownSearch = s => s.SelectValue("content_cphMain_cphMain_ddlLocal", "1"),
                 SearchButton = "content_cphMain_cphMain_btnView",
                 SuccessText = "Export"
             },
             new ReportTestPage("/Reports/HR/Employee/emp_SickBank.aspx") {
                 NarrahDownSearch = s => {
                     s.SelectLabel("content_cphMain_cphMain_ddlOpCode", "NJ7");
                     s.SelectLabel("content_cphMain_cphMain_ddlStatus", "Active");
                 },
                 SearchButton = "content_cphMain_cphMain_btnView",
                 SuccessText = "Export"
             },

             // these have not yet been replaced
             //new ReportTestPage("/Reports/WaterQuality/WQComplaintsDetails.aspx") {
             //    SearchButton = "content_cphMain_cphMain_btnView",
             //    SuccessText = "Export",
             //    NarrahDownSearch = s => s.SelectLabel("content_cphMain_cphMain_ddlOpCode", "NJ4") 
             //},
             //new ReportTestPage("/Reports/WaterQuality/WQComplaintsResponseTimes.aspx") {
             //    SearchButton = "content_cphMain_cphMain_btnView",
             //    SuccessText = "Export"
             //},

             //new ReportTestPage("/Modules/WorkOrders/Views/Reports/PowerPlantAsbuiltReport.aspx") {
             //    NarrahDownSearch = s => {
             //        s.SelectLabel("content_cphMain_cphMain_ddlOpCode", "NJ7");
             //        s.Type("content_cphMain_cphMain_txtDateStart", "2/1/2011");
             //        s.Type("content_cphMain_cphMain_txtDateEnd", "5/11/2011");
             //    },
             //    SuccessText = "Town"
             //},

             ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
             ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
             ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
             // FAILURES:

             // broken for now, times out
             //new ReportTestPage("/Reports/FieldServices/TailgateTalksTrainingHours.aspx") {
             //     NarrahDownSearch = s => {
             //         s.SelectLabel("content_cphMain_cphMain_DataField2_ddlDataField", "NJ7");
             //         s.SelectLabel("content_cphMain_cphMain_DataField4_ddlDataField", "2010");
             //     }
             //}, // needs opcode and year

             // broken for now, times out
             //new ReportTestPage("/Reports/WaterQuality/BactiResults.aspx") {
             //    SearchButton = "content_cphMain_cphMain_btnView"
             //},

        };

        #endregion

        [Test]
        public void TestTheTestWhileTestingTestTestsTestTest()
        {
            RunTests(PAGES);
        }

        public override void TestTheTestPage(ReportTestPage testPage)
        {
            Selenium.Open(testPage.Url);
            if (!testPage.Automatic)
            {
                testPage.NarrahDownSearch(Selenium);
                Selenium.ClickAndWaitForPageToLoad(testPage.SearchButton);
            }
            testPage.PerformFinalVerification(Selenium);
        }

        public class ReportTestPage : DataElementTestPage
        {
            public const string SUCCESS_TEXT = "Total Records:";

            public string SuccessText;
            public bool Automatic;

            public ReportTestPage(string url)
                : base(url)
            {
                SuccessText = SUCCESS_TEXT;
                Automatic = false;
                FinalVerificationAction = s =>
                                          s.AssertTextPresent(SuccessText);
            }

            public override void PerformFinalVerification(ISelenium selenium)
            {
                FinalVerificationAction(selenium);
            }
        }
    }
}
