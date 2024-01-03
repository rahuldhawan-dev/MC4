using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
[TestFixture]
public class CreateHydrant
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
public void TheCreateHydrantTest()
{
			selenium.Select("OpCntr", "label=" + opCenterCreate);
			selenium.Click("Image1");
			selenium.WaitForPageToLoad("30000");
			selenium.Select("TwnRecID", "label=" + town);
			selenium.Click("Image1");
			selenium.WaitForPageToLoad("30000");
			selenium.Select("StName", "label=" + street);
			selenium.Type("BillingDate", billingDate);
			selenium.Type("premiseNumber", premiseNumber);
			selenium.Click("B2");
			selenium.WaitForPageToLoad("30000");
}
}
}
