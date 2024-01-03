using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Testing.SeleniumMvc;
using MMSINC.Testing.SpecFlow.Library;
using MMSINC.Testing.SpecFlow.StepDefinitions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using TechTalk.SpecFlow;

namespace RegressionTests.Steps
{
    [Binding]
    public sealed class CrewAssignmentsPage
    {
        #region Consts

        private const string CREW_CALENDAR_SELECTOR_ID = "#crewCalendar";

        #endregion

        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;

        public CrewAssignmentsPage(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then("the assignments table row should have the css class \"(.+)\"")]
        public void ThenTheAssignmentsTableRowShouldHaveTheCssClass(string cssClass)
        {
            var table = WebDriverHelper.Current.FindElement(By.Id("assignmentsTable"));

            // This will just throw an error if it fails.
            table.FindElement(By.ClassName(cssClass));
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

        [When("^I double click the start assignment link for ([^\"]+): \"([^\"]+)\"")]
        public void WhenIDoubleCLickTheStartAssignmentLink(string className,
            string namedEntity)
        {
            var ca = TestObjectCache.Instance.Lookup<CrewAssignment>(className, namedEntity);
            var link = WebDriverHelper
                      .Current.FindElements(By.LinkText("Start")).Single(l =>
                           l.GetAttribute("href").Contains($"Start/{ca.Id}"));
            var actions = new Actions(WebDriverHelper.Current.InternalDriver);
            actions.DoubleClick(link.InternalElement).Perform();
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

        [Then("I should be finalizing work order: \"([^\"]+)\"")]
        public void ThenIShouldBeAtTheFinalizePage(string workOrder)
        {
            var wo = TestObjectCache.Instance.Lookup<WorkOrder>("work order", workOrder);

            if (wo == null)
            {
                Assert.Fail($"Could not find cached work order using key '{workOrder}'.");
            }

            Assert.IsTrue(WebDriverHelper.Current.CurrentUri.ToString().EndsWith(
                $"/modules/WorkOrders/Views/WorkOrders/Finalization/WorkOrderFinalizationResourceRPCPage.aspx?cmd=update&arg={wo.Id}"));
        }
    }
}
