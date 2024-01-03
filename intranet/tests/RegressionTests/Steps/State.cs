using System;
using DeleporterCore.Client;
using MMSINC.Testing.SpecFlow.StepDefinitions;
using NUnit.Framework;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.SeleniumMvc;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace RegressionTests.Steps
{
    [Binding]
    public class State
    {
        [Given("I (can|can not) see the missing role error")]
        [When("I (see|do not see) the missing role error")]
        [Then("I (should|should not) see the missing role error")]
        public static void ISeeTheMissingRoleError(string shouldernt)
        {
            const string errmagerd = "You do not have the roles necessary to access this resource.";
            if (StepHelper.IsTruthy(shouldernt))
            {
                MMSINC.Testing.SpecFlow.StepDefinitions.State.ThenIShouldSeeText(errmagerd);
            }
            else
            {
                MMSINC.Testing.SpecFlow.StepDefinitions.State.ThenIShouldNotSeeText(errmagerd);
            }
        }

        [Given("^I am logged in as \"([^\"]+)\"$"),
        When("^I log in as \"([^\"]+)\"$")]
        public static void GivenIAmLoggedInAsUser(string username)
        {
            var rootUrl = ConfigurationManager.AppSettings["RootUrl"];
            var driver = WebDriverHelper.Current;
            driver.GoToUrl(new Uri(rootUrl));
            driver.WaitUntilSingleElementExists(By.Id("okta-signin-username"));
            driver.FindElement(By.Id("okta-signin-username")).TypeValue(username);
            driver.FindElement(By.Id("okta-signin-password")).TypeValue(ConfigurationManager.AppSettings["DevPass"]);
            driver.FindElement(By.Id("okta-signin-submit")).Click();
            driver.WaitUntilSingleElementExistsByIdOrName("logout-button", 120);
        }

        [Then(@"I should be able to download ([^:]+) ""([^""]+)""'s pdf")]
        public void ICanDownloadThePdf(string type, string name)
        {
            // This is doing a really basic test to make sure a pdf view will compile.  It's
            // done via ajax to get around dealing with the file download prompts.

            var pdfUrl = Navigation.GetUrlFor("Show", type, name) + ".pdf";
            pdfUrl = Navigation.GetAbsoluteUriFromRelativePath(pdfUrl).ToString();

            var ajax = "var result = null; $.ajax({ async: false, url: '" + pdfUrl + "', success: function(data, textStatus) { result = 'good pdf ' + data ; }, error : function() { result = 'bad pdf'; } }); return result;";
            //var result = ScriptRunner.Run(string.Format(ScriptRunner.SELF_EXECUTING_SCRIPT_FORMAT, ajax));
            var result = WebDriverHelper.Current.ExecuteScript(ajax).ToString();

            if (!result.StartsWith("good pdf"))
            {
                // If this fails, we wanna navigate directly to the pdf url so we can get the error html and junk.
                Navigation.VisitRelativePath(pdfUrl);
            }
            else if (result.Contains("You do not have the roles necessary to access this resource."))
            {
                // The RoleAuthorizationFilter is incorrectly returning a 200 code, so the error
                // handler for the ajax isn't being called.
                Navigation.VisitRelativePath(pdfUrl);
                MMSINC.Testing.SpecFlow.StepDefinitions.State.ThenIShouldNotSeeText("You do not have the roles necessary to access this resource.");
            }
        }

        [Then("I should see a 404 error message")]
        public void IShouldSeeA404()
        {
            if (!WebDriverHelper.Current.FindElements(By.Id("err404")).Any())
            {
                Assert.Fail("The current page does not appear to contain a 404 error message.");
            }
        }

        [Given("I can see the navigation menu")]
        public void ICanSeeTheNavigationMenu()
        {
            // This step is to force the menu back to being visible since we otherwise hide it during
            // all other regression tests.
            WebDriverHelper.Current.ExecuteScript("Menu.setMenuVisibility(true);");
        }

        [Then("I (should|should not) see the \"([^\"]+)\" button in the action bar")]
        public void IMightSeeTheButtonInTheActionBar(string shouldernt, string buttonClassSuffix)
        {
            var should = StepHelper.IsTruthy(shouldernt);
            var exists = WebDriverHelper.Current.FindElements(By.ClassName("ab-" + buttonClassSuffix)).Any();
            if (should && !exists)
            {
                Assert.Fail($"Unable to find {buttonClassSuffix} button in the action bar.");
            }
            else if (!should && exists)
            {
                Assert.Fail($"Unexpectedly found {buttonClassSuffix} button in the action bar.");
            }
        }

        /// <summary>
        /// This display uses a shared view that generates a custom "For" value on the
        /// label. ---  label for="answer_@Model.LockoutFormQuestion.Id"
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        [Then("^I should see a display for lockout form question: \"([^\"]+)\" with \"([^\\s]+)\"")]
        public static void ThenIShouldSeeADisplayForThingWith(string name, string value)
        {
            var namedItem = MMSINC.Testing.SpecFlow.StepDefinitions.Data.GetCachedEntity("lockout form question", name);
            var field = $"answer_{namedItem.GetPropertyValueByName("Id")}";
            MMSINC.Testing.SpecFlow.StepDefinitions.State.ThenIShouldSeeADisplayForWith(field, value);
        }
    }
}
