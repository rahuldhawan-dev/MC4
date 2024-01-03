using NUnit.Framework;
using RegressionTests.Lib;
using RegressionTests.Lib.TestParts;
using MMSINC.Testing.Selenium;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class SickBankBroTest : BaseTest<Types.SickBank>
    {
        #region Consts
        
        public struct NecessaryIDs
        {
            public const string LINK_HUMAN_RESOURCES = "link=Human Resources",
                                LINK_SICK_BANK = "link=Sick Bank",
                                BTN_ADD = "content_cphMain_cphMain_btnAdd",
                                DDL_CALENDAR_YEAR = "content_cphMain_cphMain_DataElement1_DetailsView1_dv_ddlCalendar_Year",
                                BTN_INSERT = "content_cphMain_cphMain_DataElement1_DetailsView1_LinkButton1",
                                LABEL_CALENDAR_YEAR = "//table[@id='content_cphMain_cphMain_DataElement1_DetailsView1']/tbody/tr[2]/td[2]";
        }

        #endregion

        #region Exposed Methods

        [Test]
        public override void TestCreate()
        {
            NavigateToCreatePage();
            try
            {
                var sickBank = Create();
                VerifyWasCreated(sickBank);
            }
            finally
            {
                // Delete Sick Bank, kthx
            }
        }

        #endregion

        #region Private Methods

        protected override void NavigateToCreatePage()
        {
            base.NavigateToCreatePage();
            Selenium.Click(NecessaryIDs.LINK_HUMAN_RESOURCES);
            Selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LINK_SICK_BANK);
            Selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_ADD);
        }

        protected override Types.SickBank Create()
        {
            var sb = new Types.SickBank();
            sb.CalendarYear = "2008";

            Selenium.Select(NecessaryIDs.DDL_CALENDAR_YEAR, "label=" + sb.CalendarYear);
            Selenium.ClickAndWaitForPageToLoad(NecessaryIDs.BTN_INSERT);

            return sb;
        }

        protected override void VerifyWasCreated(Types.SickBank item)
        {
            Assert.AreEqual(item.CalendarYear, Selenium.GetText(NecessaryIDs.LABEL_CALENDAR_YEAR));
        }

        #endregion
    }
}
