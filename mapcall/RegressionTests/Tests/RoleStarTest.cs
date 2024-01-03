using MMSINC.Testing.Selenium;
using MapCall.Common.Testing.Selenium.TestParts;
using NUnit.Framework;
using RegressionTests.Lib;
using Selenium;

namespace RegressionTests.Tests
{
    [TestFixture]
    class RoleStarTest : BaseSearchTest<RoleTestPage>
    {
        #region Fields

        public static readonly RoleTestPage[] PAGES = new RoleTestPage[] {
            // DATAPAGEBASE
            new RoleTestPage("/Modules/Contractors/ContractorInsuranceMinimumRequirements.aspx"),
            new RoleTestPage("/Modules/Contractors/ContractorWorkCategories.aspx"),
            new RoleTestPage("/Modules/Customer/H2OSurveys.aspx"),
            new RoleTestPage("/Modules/Customer/MeterRouteDetails.aspx", true),
            new RoleTestPage("/Modules/Customer/MeterRoutes.aspx", true),
            new RoleTestPage("/Modules/FieldServices/FlushingSchedules.aspx", true),
            new RoleTestPage("/Modules/FieldServices/StormDrainInspectionCleanings.aspx"),
            new RoleTestPage("/Modules/HR/Administrator/BusPerformanceKPIMeasurement.aspx"),
            new RoleTestPage("/Modules/HR/Administrator/EmployeePositionControls.aspx"),
            new RoleTestPage("/Modules/Management/WebLinkCategories.aspx", true),
            new RoleTestPage("/Modules/Management/WebLinks.aspx", true),
            new RoleTestPage("/Reports/Customer/H2OSurveyReport.aspx"),
            new RoleTestPage("/Reports/Customer/H2OSurveyMonthlyReport.aspx", true),
            new RoleTestPage("/Reports/HR/Employee/EmployeePositionControlReport.aspx"),

            new RoleTestPage("/Modules/Notifications.aspx", true), // everyone can view, shouldn't be able to add
            
            // every page that does an access denied.
            new RoleTestPage("/Admin/Timeout.aspx"),
            new RoleTestPage("/Modules/BPU/ManagementAudits.aspx"),
            new RoleTestPage("/Modules/BusinessPerformance/InitiativeSteps.aspx"),
            new RoleTestPage("/Modules/BusinessPerformance/StrategicElements.aspx"),
            new RoleTestPage("/Modules/Customer/AccountStatuses.aspx"),
            new RoleTestPage("/Modules/Customer/BillingClassifications.aspx"),
            new RoleTestPage("/Modules/Customer/CollectionRequirements.aspx"),
            new RoleTestPage("/Modules/Customer/CriticalCustomers.aspx"),
            new RoleTestPage("/Modules/Customer/CustomerSurvey.aspx"),
            new RoleTestPage("/Modules/Customer/CustomerSurveyData.aspx"),
            new RoleTestPage("/Modules/Customer/Districts.aspx"),
            new RoleTestPage("/Modules/Customer/MeterLocations.aspx"),
            new RoleTestPage("/Modules/Customer/MeterPeriodicTestingRequirements.aspx"),
            new RoleTestPage("/Modules/Customer/MeterReadingCodes.aspx"),
            new RoleTestPage("/Modules/Customer/MeterSizes.aspx"),
            new RoleTestPage("/Modules/Customer/MeterWaterUtilityTypes.aspx"),
            new RoleTestPage("/Modules/Customer/NoticeCodes.aspx"),
            
            new RoleTestPage("/Modules/Customer/ProcessSequences.aspx"),
            new RoleTestPage("/Modules/Customer/ServiceOrderCategories.aspx"),
            new RoleTestPage("/Modules/Customer/ServiceOrderCodes.aspx"),
            new RoleTestPage("/Modules/Customer/ServiceOrderCodeTypes.aspx"),
            new RoleTestPage("/Modules/Customer/ServiceOrderScheduleTypes.aspx"),
            new RoleTestPage("/Modules/Customer/ServiceOrderTypes.aspx"),
            new RoleTestPage("/Modules/Customer/ServiceUtilityTypes.aspx"),
            new RoleTestPage("/Modules/Data/Services/InactiveServices.aspx"),
            new RoleTestPage("/Modules/FieldServices/ConsecutiveEstimates.aspx"),
            new RoleTestPage("/Modules/FieldServices/MeterContractors.aspx"),
            new RoleTestPage("/Modules/FieldServices/MeterProfiles.aspx"),
            new RoleTestPage("/Modules/FieldServices/MeterRecorders.aspx"),
            //new RoleTestPage("/Modules/FieldServices/Meters.aspx"),
            new RoleTestPage("/Modules/FieldServices/MeterTestComparisonMeters.aspx"),
            //new RoleTestPage("/Modules/FieldServices/MeterTests.aspx"),
            new RoleTestPage("/Modules/FieldServices/StormWaterAssets.aspx"),
            new RoleTestPage("/Modules/FieldServices/UploadCount.aspx", true),
            new RoleTestPage("/Modules/HR/Administrator/BusPerformanceGoals.aspx"),
            new RoleTestPage("/Modules/HR/Administrator/BusPerformanceInitiatives.aspx"),
            new RoleTestPage("/Modules/HR/Administrator/BusPerformanceKPI.aspx"),
            new RoleTestPage("/Modules/HR/Administrator/BusPerformanceReportingRequirements.aspx"),
            new RoleTestPage("/Modules/HR/Administrator/BusProcessFlowDiagrams.aspx"),
            new RoleTestPage("/Modules/HR/Administrator/DocumentTypes.aspx"),
            new RoleTestPage("/Modules/HR/Administrator/editLookups.aspx"),
            new RoleTestPage("/Modules/HR/Administrator/EmailNotificationConfiguration.aspx", true),
            new RoleTestPage("/Modules/HR/Administrator/NextRankingNumber.aspx"),
            new RoleTestPage("/Modules/HR/Documents.aspx"),
            new RoleTestPage("/Modules/HR/Employee/PositionHistory.aspx"),
            new RoleTestPage("/Modules/HR/Employee/PositionPostings.aspx"),
            new RoleTestPage("/Modules/HR/Employee/Positions.aspx"),
            new RoleTestPage("/Modules/HR/Employee/SickBank.aspx"),
            new RoleTestPage("/Modules/Management/PurchasingAgreements.aspx"),
            new RoleTestPage("/Modules/Notes.aspx"),
            new RoleTestPage("/Modules/Production/AllocationTransactions.aspx"),
            new RoleTestPage("/Modules/Production/ExpenseLines.aspx"),
            new RoleTestPage("/Modules/Production/GeologicalFormations.aspx"),
            new RoleTestPage("/Modules/Production/PoliciesPractices.aspx"),
            new RoleTestPage("/Modules/Production/ProcessChanges.aspx"),
            
            new RoleTestPage("/Modules/Production/PwsidCustomerData.aspx"),
            new RoleTestPage("/Modules/Production/SystemDelivery.aspx"),
            new RoleTestPage("/Modules/Production/SystemDeliveryBudget.aspx"),
            new RoleTestPage("/Modules/Production/UtilityAccounts.aspx"),
            new RoleTestPage("/Modules/Production/UtilityRates.aspx"),
            new RoleTestPage("/Reports/BusinessPerformance/RptBudgetReportByOpCode.aspx"),
            new RoleTestPage("/Reports/BusinessPerformance/RptBudgetReportByOpCodeBULineCategory.aspx"),
            new RoleTestPage("/Reports/BusinessPerformance/RptBudgetRollupDepartmentArea.aspx"),
            new RoleTestPage("/Reports/Data/Services/FireServiceEstimatedBackBilling.aspx"),
            new RoleTestPage("/Reports/Data/Services/FireServiceMonthlyBilling.aspx"),
            new RoleTestPage("/Reports/Data/Services/FireServiceRecoveredBilling.aspx"),
            new RoleTestPage("/Reports/FieldServices/SewerOverflowReport.aspx"),
            new RoleTestPage("/Reports/HR/Employee/emp_Licenses.aspx"),
            new RoleTestPage("/Reports/HR/Employee/emp_Seniority.aspx"),
            new RoleTestPage("/Reports/HR/Employee/emp_SickBank.aspx"),
            new RoleTestPage("/Reports/HR/Employee/SickBankHours.aspx"),
            new RoleTestPage("/Reports/HR/Employee/TrainingRecordsAttendance.aspx"),
            new RoleTestPage("/Reports/HR/Employee/TrainingRecordsTrainingHours.aspx"),
            new RoleTestPage("/Reports/HR/UnionAffiliation.aspx"),
            
            // Random Other Pages?
            new RoleTestPage("/Reports/HR/Employee/EmployeeEssentialEmergencyResponsePriority.aspx"),
        };

        #endregion
        
        [Test]
        public void TestTheTestWhileTestingTestTestsTestTest()
        {
            try
            {
                // Log in as MCDistro, he should only have access to the images search.
                Login.AsDistro(Selenium);
                RunTests(PAGES);
            }
            finally
            {
                Login.AsAdmin(Selenium);
            }
        }
        
        public override void TestTheTestPage(RoleTestPage testPage)
        {
            Selenium.OpenAndAllowErrors(testPage.Url);
            // if they can access the page, make sure they can't add
            Selenium.AssertTextNotPresent("The resource cannot be found.");
            var textPresent = Selenium.IsTextPresent("Access Denied");
            if (!textPresent && testPage.CanViewNotAdd)
                Selenium.AssertElementIsNotPresent("btnAdd");
            else
                Assert.IsTrue(textPresent);
        }
    }

    public class RoleTestPage : TestPage
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="canViewNotAdd"> True if the user can view the page, just not add images.</param>
        public RoleTestPage(string url, bool canViewNotAdd=false)
            : base(url)
        {
            _canViewNotAdd = canViewNotAdd;
        }

        #endregion

        #region Private Members

        private readonly bool _canViewNotAdd;

        #endregion

        #region Properties

        public bool CanViewNotAdd { get { return _canViewNotAdd; }}

        #endregion

        public override void PerformFinalVerification(ISelenium selenium)
        {

        }
    }


}
