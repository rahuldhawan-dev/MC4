using MMSINC.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib;
using Selenium;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class DataPageBaseStarTest : BaseSearchTest<DataPageBaseStarTest.DataPageBaseTestPage>
    {
        #region Constants

        private const string LANDING_PAGE_SEARCH_LINK = "//a[@class='bc-box-search']";

        #endregion

        #region Fields

        private static readonly DataPageBaseTestPage[] _pages = new[] { 
            new DataPageBaseTestPage("/Modules/Data/Contacts/ContractorContactTypes.aspx") { HasSearchPage = false },
            new DataPageBaseTestPage("/Modules/Data/Contacts/TownContactTypes.aspx") { HasSearchPage = false },
            new DataPageBaseTestPage("/Modules/HR/Employee/ScheduleType.aspx"),
            new DataPageBaseTestPage("/Modules/HR/Employee/ScheduleTypeDetails.aspx"),
            new DataPageBaseTestPage("/Modules/HR/Administrator/EmployeePositionControls.aspx"),
            new DataPageBaseTestPage("/Modules/HR/Administrator/BusPerformanceInitiatives.aspx"),
            new DataPageBaseTestPage("/Modules/HR/Employee/StaffingHours.aspx")
                {
                    ShouldViewRecords = false 
                },
            new DataPageBaseTestPage("/Modules/HR/Union/ContractProposalsReport.aspx")
                {
                    NarrahDownSearch = s => s.SelectLabel("searchContractId", "18 - NJ7 - NJ_NJ7_395_Shrewsbury - Apr 17 1996 - Apr 16 1999"),
                    ShouldViewRecords = false 
                },
         
            new DataPageBaseTestPage("/Modules/Contractors/ContractorInsuranceMinimumRequirements.aspx")
                {
                    HasSearchPage = false 
                },
            new DataPageBaseTestPage("/Modules/Contractors/ContractorWorkCategories.aspx"),
            new DataPageBaseTestPage("/Modules/Customer/H2OSurveys.aspx")
                {
                    NarrahDownSearch = (s) =>
                                           {
                                               // This should cover all the BaseSearchField classes.
                                               s.Type("searchToiletsStart", "0");
                                               s.Type("searchToiletsEnd", "1");
                                               s.Type("searchAuditPerformedDateStart", "4/24/2011");
                                               s.Type("searchAuditPerformedDateEnd", "4/26/2011");
                                               s.SelectLabel("searchDwellingTypeID", "Single Family Home");
                                               s.SelectLabel("searchHasWasher", "Yes");
                                           }
                },
            new DataPageBaseTestPage("/Modules/Customer/MeterRouteDetails.aspx")
                {
                    ExpectsThereToBeRecords = false 
                },
            new DataPageBaseTestPage("/Modules/Customer/MeterRoutes.aspx")  {
                    NarrahDownSearch = (s) =>
                                           {
                                               // This should cover all the BaseSearchField classes.
                                               s.SelectLabel("content_cphMain_cphMain_template_opCntrField_ddlOpCntr", "NJ3 - Fire Road");
                                               s.WaitForEditable("content_cphMain_cphMain_template_opCntrField_ddlTown");
                                               s.SelectLabel("content_cphMain_cphMain_template_opCntrField_ddlTown", "ABSECON");
                                           }
                },

            new DataPageBaseTestPage("/Modules/FieldServices/FlushingSchedules.aspx"),
            new DataPageBaseTestPage("/Modules/FieldServices/StormDrainInspectionCleanings.aspx")
                {
                    NarrahDownSearch = (s) =>
                                           {
                                               s.SelectLabel("searchStormWaterAssetTypeID","Catch Basin");
                                               s.WaitForEditable("content_cphMain_cphMain_template_searchStormWaterAssetSearch");
                                               s.SelectLabel("content_cphMain_cphMain_template_searchStormWaterAssetSearch", "1-1: [PELICAN DR, HERON DR]");
                                           }
                },
            
            new DataPageBaseTestPage("/Modules/HR/Administrator/BusPerformanceKPIMeasurement.aspx"),

            new DataPageBaseTestPage("/Modules/Management/WebLinkCategories.aspx") { HasSearchPage = false },
            new DataPageBaseTestPage("/Modules/Management/WebLinks.aspx"),
            
            new DataPageBaseTestPage("/Reports/Customer/H2OSurveyReport.aspx")
                {
                    ShouldViewRecords = false,
                    NarrahDownSearch = (s) =>
                                           {
                                               s.Type("searchEnrollmentDateStart", "04/12/2011");
                                               s.Type("searchEnrollmentDateEnd", "04/21/2011");
                                           }
                },
            new DataPageBaseTestPage("/Reports/HR/Employee/EmployeePositionControlReport.aspx")
                {
                    // Page doesn't have a view record link. 
                    ShouldViewRecords = false 
                }
        };

        #endregion

        [Test]
        public void TestSearchesAndUpdates()
        {
            RunTests(_pages);
        }

        public override void TestTheTestPage(DataPageBaseTestPage testPage)
        {
            Selenium.Open(testPage.Url);

            if (testPage.HasSearchPage)
            {
                if (Selenium.IsElementPresent(LANDING_PAGE_SEARCH_LINK))
                {
                    Selenium.ClickAndWaitForPageToLoad(LANDING_PAGE_SEARCH_LINK);
                }

                testPage.NarrahDownSearch(Selenium);
                Selenium.ClickAndWaitForPageToLoad(testPage.SearchButton);
            }


            if (testPage.ExpectsThereToBeRecords)
            {
                if (testPage.ShouldViewRecords)
                {
                    Selenium.ClickAndWaitForPageToLoad(testPage.ViewLink);

                    if (testPage.ShouldUpdateRecords)
                    {
                        Selenium.ClickAndWaitForPageToLoad(testPage.EditLink);
                        testPage.PreUpdateAction(Selenium);
                        Selenium.ClickAndWaitForPageToLoad(testPage.UpdateLink);
                    }
                }

                testPage.PerformFinalVerification(Selenium);
            }
            else
            {
                Selenium.AssertTextPresent("There are no records to display");
            }
        }

        public class DataPageBaseTestPage : TestPage
        {
            public const string SEARCH_BUTTON = "btnSearch";

            #region Properties

            /// <summary>
            /// Set to false if there's no search page. LookupPages do not have searches. 
            /// </summary>
            public bool HasSearchPage = true;

            #endregion

            public DataPageBaseTestPage(string url)
                : base(url)
            {
                SearchButton = SEARCH_BUTTON;
            }

            public override void PerformFinalVerification(ISelenium selenium)
            {

            }
        }
    }
}