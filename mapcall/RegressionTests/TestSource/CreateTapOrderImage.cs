using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
[TestFixture]
public class CreateTapOrderImage
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
public void TheCreateTapOrderImageTest()
{
			selenium.Type("F1Label", serviceNumber);
			selenium.Type("inFile", filename);
			selenium.Type("F3Label", streetNumber);
			selenium.Type("F4Label", street);
			selenium.Type("F6Label", premiseNumber);
			selenium.Select("F9Label", "label=" + serviceType);
			selenium.Type("F10Label", dated);
			selenium.Click("Submit");
			selenium.WaitForPageToLoad("30000");
			selenium.SelectFrame("relative=up");
			selenium.SelectFrame("main");
			Assert.IsTrue(selenium.IsTextPresent("File successfully uploaded."));
}
}
}
