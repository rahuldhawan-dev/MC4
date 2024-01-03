using System.Linq;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.SeleniumMvc;
using MMSINC.Testing.SpecFlow.Library;
using MMSINC.Testing.SpecFlow.StepDefinitions;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace RegressionTests.Steps
{
    [Binding]
    public static class State
    {
        #region Constants

        #endregion

        #region Step Definitions

        #region Page Content

        // TODO: Needs work, could be extended further to allow for more.
        [Then("^I (should|shouldn't|should not|shant) see a row for ([^\"]+) \"([^\"]+)\" in the table ([^\\s]+) with the css class \"(.+)\"$")]
        public static void ThenTheRowForTableWithShouldHaveTheCssClassCompleted(string nopteraptor, string type, string name, string table, string css)
        {
            var namedItem = TestObjectCache.Instance.Lookup(type, name);
            var value = namedItem.GetPropertyValueByName("Id").ToString();
            var row = WebDriverHelper.Current.FindElements(By.CssSelector($"#{table} .{css}"));

            var exists = row.Any() && row.First().FindElements(By.TagName("td")).First().Text == value;

            if (StepHelper.IsTruthy(nopteraptor))
            {
                Assert.IsTrue(exists, "No row exists with the css class");
            }
            else
            {
                Assert.IsFalse(exists, "The row exists with the css class.");
            }
        }

        #endregion

        #region Log In/Out

        [Given("I am not logged in")]
        public static void GivenIAmNotLoggedIn()
        {
            MMSINC.Testing.SpecFlow.StepDefinitions.Navigation.
                VisitRelativePath("/Authentication/LogOff");
        }

        [Given("^I am logged in as \"([^\"]+)\", password: \"([^\"]+)\"$")]
        public static void GivenIAmLoggedInAsUser(string email, string password)
        {
            MMSINC.Testing.SpecFlow.StepDefinitions.Navigation.
                VisitRelativePath(Navigation.PAGE_STRINGS["login"]);
            MMSINC.Testing.SpecFlow.StepDefinitions.Input.EnterValueInField(LoginPage.ControlIDs.EMAIL_INPUT, email);
            MMSINC.Testing.SpecFlow.StepDefinitions.Input.EnterValueInField(LoginPage.ControlIDs.PASSWORD_INPUT, password);
            MMSINC.Testing.SpecFlow.StepDefinitions.Input.PressButton(LoginPage.ControlIDs.LOGIN_BUTTON);

            Assert.AreEqual(email, WebDriverHelper.Current.FindElement(By.Id(LoginPage.ControlIDs.LOGGED_IN_DISPLAY_NAME)).Text);
        }

        #endregion

        #endregion
    }
}
