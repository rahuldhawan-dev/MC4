using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
[TestFixture]
public class CreateNewContractor
{
private ISelenium selenium;
private StringBuilder verificationErrors;

[SetUp]
public void SetupTest()
{
selenium = new DefaultSelenium("localhost", 4444, "*chrome", SetupFixtureBase.DEFAULT_BASE_URL);
selenium.Start();
verificationErrors = new StringBuilder();
}

[TearDown]
public void TeardownTest()
{
try
{
selenium.Stop();
}
catch (Exception)
{
// Ignore errors if unable to close the browser
}
Assert.AreEqual("", verificationErrors.ToString());
}

[Test]
public void TheCreateNewContractorTest()
{
			selenium.Type("ctl00$ctl00$ctl00$content$cphMain$cphMain$template$tabView$detailView$txtName", name);
			selenium.Click("link=Insert");
			selenium.WaitForPageToLoad("30000");
			try
			{
				Assert.AreEqual(name, selenium.GetText("//table[@id='content_cphMain_cphMain_template_tabView_detailView']/tbody/tr[2]/td[2]"));
			}
			catch (AssertionException e)
			{
				verificationErrors.Append(e.Message);
			}
}
}
}
