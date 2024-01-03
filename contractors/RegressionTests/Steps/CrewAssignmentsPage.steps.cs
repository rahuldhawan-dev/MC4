using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Testing.SeleniumMvc;
using MMSINC.Testing.SpecFlow.Library;
using MMSINC.Testing.SpecFlow.StepDefinitions;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace RegressionTests.Steps
{
    [Binding]
    public class CrewAssignmentsPage
    {
        #region Consts

        private const string CREW_CALENDAR_SELECTOR_ID = "#crewCalendar";

        #endregion

        [Then("the calendar date for \"([^\"]+)\" should have the css class \"(.+)\"")]
        public void ThenTheCalendar(string date, string cssClass)
        {
            DateTime realDate;
            Assert.IsTrue(DateTime.TryParse(date, out realDate), "This is not a date: '{0}'", date);

            // Need that . there for the class. And that space before the .
            var selector = CREW_CALENDAR_SELECTOR_ID + " ." + cssClass;
            var dateCells = WebDriverHelper.Current.FindElements(By.CssSelector(selector));
            var validCell = (from c in dateCells
                             where c.Text == realDate.Day.ToString()
                             select c).SingleOrDefault();
            Assert.IsNotNull(validCell, "Can't find a cell with css class '{0}' and the expected text '{1}'", cssClass, realDate.Day);
        }

        [Then("the assignments table row should have the css class \"(.+)\"")]
        public void ThenTheAssignmentsTableRowShouldHaveTheCssClass(string cssClass)
        {
            var table = WebDriverHelper.Current.FindElement(By.Id("assignmentsTable"));

            // This will just throw an error if it fails.
            table.FindElement(By.ClassName(cssClass));
        }

        // TODO this could be made useful for other links.
        [When("^I press the end assignment link for ([^\"]+): \"([^\"]+)\"")]
        public void WhenIShouldSeeTheLinkToAPageFor(string className, string namedEntity)
        {
            var ca = TestObjectCache.Instance.Lookup<CrewAssignment>(className, namedEntity);
            var link = WebDriverHelper.Current.FindElement(ByHelper.Attribute("data-id", ca.Id.ToString()));
            link.Click();
        }

        [Then("^I (should|should not) see the start assignment link for ([^\"]+): \"([^\"]+)\"")]
        public void ThenIShouldNotSeeTheStartAssignmentLink(string nopterator, string className, string namedEntity)
        {
            var ca = TestObjectCache.Instance.Lookup<CrewAssignment>(className, namedEntity);
            var link = WebDriverHelper
                      .Current.FindElements(By.LinkText("Start")).SingleOrDefault(l =>
                           l.GetAttribute("href").Contains($"Start/{ca.Id}"));

            if (StepHelper.IsTruthy(nopterator))
            {
                Assert.IsNotNull(link, $"The link does not exist for {className}: {namedEntity}");
            }
            else
            {
                Assert.IsNull(link, $"The link exists for {className}: {namedEntity}");
            }
        }

        [When("^I click the start assignment link for ([^\"]+): \"([^\"]+)\"")]
        public void WhenICLickTheStartAssignmentLink(string className,
            string namedEntity)
        {
            var ca = TestObjectCache.Instance.Lookup<CrewAssignment>(className, namedEntity);
            var link = WebDriverHelper
                      .Current.FindElements(By.LinkText("Start")).Single(l =>
                           l.GetAttribute("href").Contains($"Start/{ca.Id}"));

            link.Click();
        }

        // TODO this could be made useful for other links.
        [Then("^I (should|should not) see the end assignment link for ([^\"]+): \"([^\"]+)\"")]
        public void ThenIshouldNotSeeTheLinkFor(string nopterator, string className, string namedEntity)
        {
            var ca = TestObjectCache.Instance.Lookup<CrewAssignment>(className, namedEntity);
            var link = WebDriverHelper.Current.FindElements(ByHelper.Attribute("data-id", ca.Id.ToString())).SingleOrDefault();
            if (StepHelper.IsTruthy(nopterator))
            {
                Assert.IsNotNull(link, $"The link does not exist for {className}: {namedEntity}");
            }
            else
            {
                Assert.IsNull(link, $"The link exists for {className}: {namedEntity}");
            }
        }

        [When("^I click (ok|cancel) in the dialog after pressing the Remove link for ([^\"]+): \"([^\"]+)\"")]
        public void WhenIClickOkAfterClickingRemoveLinkForCrewAssignment(string okCancel, string className, string namedEntity)
        {
            var ca = TestObjectCache.Instance.Lookup<CrewAssignment>(className, namedEntity);
            var link = WebDriverHelper.Current.FindElements(By.LinkText("Remove")).Single(lnk => lnk.GetAttribute("href").Contains($"Destroy/{ca.Id}"));

            Input.IAnswerTheConfirmDialog(() => link.Click(), okCancel);
        }
    }
}
