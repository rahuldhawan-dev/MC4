using System;
using MMSINC.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib;
using Selenium;
using Config = MMSINC.Testing.Utilities.RegressionTestConfiguration;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class DataStarPageStarTest : BaseSearchTest<DataElementTestPage>
    {
        #region Fields

        public static readonly DataElementTestPage[] PAGES = new DataElementTestPage[] {
            new DataElementTestPage("/Modules/HR/Employee/PositionHistory.aspx") {
                NarrahDownSearch = s => s.Type("content_cphMain_cphMain_txtPCN", "1823233"),
                PreUpdateAction = s =>
                                      {
                                          s.SelectLabel(
                                              "content_cphMain_cphMain_PositionHistory1_DetailsView1_ddlDepartmentName",
                                              "Administration");
                                          s.Type(
                                              "content_cphMain_cphMain_PositionHistory1_DetailsView1_dvTxtPosition_Start_Date_0",
                                              "5/20/2011");
                                      }
                },
            new DataElementTestPage("/Modules/HR/Employee/PositionPostings.aspx"),
            new DataElementTestPage("/Modules/HR/Employee/SickBank.aspx"),
            new DataElementTestPageWithoutViewableRecords("/Reports/HR/UnionAffiliation.aspx"),
       new DataElementTestPage("/Modules/Production/PoliciesPractices.aspx"),
            new DataElementTestPage("/Modules/Management/PurchasingAgreements.aspx"),
            new DataElementTestPage("/Modules/Notifications.aspx"),



            new DataElementTestPage("/Modules/Production/UtilityAccounts.aspx"),
            new DataElementTestPage("/Modules/Production/UtilityRates.aspx"),

            new DataElementTestPage("/Modules/Production/SystemDelivery.aspx")
                {
                    NarrahDownSearch = s => s.SelectLabel("content_cphMain_cphMain_ddlOpCntr_ddl_OpCode", "NJ7 - Shrewsbury")
                },
            new DataElementTestPage("/Modules/Production/SystemDeliveryBudget.aspx")
                {
                    NarrahDownSearch = s => s.Type("content_cphMain_cphMain_DataField2_txtDataField", "2005")
                },
            new DataElementTestPageWithoutViewableRecords("/Modules/Production/ProcessChanges.aspx")
        };

        #endregion

        [Test]
        public void TestTheTestWhileTestingTestTestsTestTest()
        {
            RunTests(PAGES);
        }
        public override void TestTheTestPage(DataElementTestPage testPage)
        {
            Selenium.Open(testPage.Url);
            testPage.NarrahDownSearch(Selenium);
            Selenium.ClickAndWaitForPageToLoad(testPage.SearchButton);
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
        }
    }

    public abstract class TestPage
    {
        #region Constants

        public const string VIEW_LINK = "link=View";
        public const string EDIT_LINK = "link=Edit";
        public const string UPDATE_LINK = "link=Update";
        public const string SEARCH_VERIFICATION_TEXT = "total records";

        #endregion

        #region Fields

        public string Url,
                      SearchButton,
                      SearchVerificationText,
                      ViewLink,
                      EditLink,
                      UpdateLink;

        /// <summary>
        /// Set to false if a table has no records(like ContractorsInsurance). 
        /// </summary>
        public bool ExpectsThereToBeRecords = true;

        public Action<ISelenium> NarrahDownSearch;

        /// <summary>
        /// Fill in any forms that need to be updated in order to update. Some pages
        /// have required fields but have null values because the data was imported
        /// from excel or something. ContractorAgreements AgreementOwner is be 
        /// one of these fields.
        /// </summary>
        public Action<ISelenium> PreUpdateAction = (s) => { /* noop */ };

        /// <summary>
        /// Set to false if a page doesn't have a way to view records. 
        /// </summary>
        public bool ShouldViewRecords;

        /// <summary>
        /// Set to false if a page doesn't have a way to update records.
        /// </summary>
        public bool ShouldUpdateRecords;

        #endregion

        #region Constructors

        public TestPage(string url)
        {
            Url = Config.GetDevSiteUrl() + url;
            SearchVerificationText = SEARCH_VERIFICATION_TEXT;
            ViewLink = VIEW_LINK;
            EditLink = EDIT_LINK;
            UpdateLink = UPDATE_LINK;
            NarrahDownSearch = s => { };
            ShouldViewRecords = true;
            ShouldUpdateRecords = true;
        }

        #endregion

        #region Abstract Methods

        public abstract void PerformFinalVerification(ISelenium selenium);

        #endregion
    }

    public class DataElementTestPage : TestPage
    {
        #region Constants

        public const string SEARCH_BUTTON = "content_cphMain_cphMain_btnSearch";
        public const string RECORD_UPDATED_TEXT = "Record Updated";

        #endregion

        #region Constructors

        public DataElementTestPage(string url)
            : base(url)
        {
            SearchButton = SEARCH_BUTTON;
            FinalVerificationAction = s => s.AssertTextPresent(RECORD_UPDATED_TEXT);
        }

        #endregion

        #region Actions

        public Action<ISelenium> FinalVerificationAction;

        #endregion

        #region Exposed Methods

        public override void PerformFinalVerification(ISelenium selenium)
        {
            FinalVerificationAction(selenium);
        }

        #endregion
    }

    public class DataElementTestPageWithoutViewableRecords : DataElementTestPage
    {
        public DataElementTestPageWithoutViewableRecords(string url) : base(url)
        {
            ShouldViewRecords = false;
            FinalVerificationAction = (s) => { /* noop */ };
        }
    }

}
