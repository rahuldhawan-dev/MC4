using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
[TestFixture]
public class CreateVehicle
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
public void TheCreateVehicleTest()
{
			selenium.Select("content_cphMain_cphMain_template_tabView_detailView_ddlVehicleIconID", "label=" + vehicleIcon);
			selenium.Click("//table[@id='content_cphMain_cphMain_template_tabView_detailView']/tbody/tr[58]/td/a[1]");
			selenium.WaitForPageToLoad("30000");
}
}
}
