using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
[TestFixture]
public class CreateStormwaterAsset
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
public void TheCreateStormwaterAssetTest()
{
			selenium.Select("content_cphMain_cphMain_dvStormWaterAsset_ddlOpCntr_ddlOpCntr", "label=" + operatingCenter);
			for (int second = 0;; second++) {
				if (second >= 60) Assert.Fail("timeout");
				try
				{
					if (selenium.IsEditable("content_cphMain_cphMain_dvStormWaterAsset_ddlTown")) break;
				}
				catch (Exception)
				{}
				Thread.Sleep(1000);
			}
			selenium.Select("content_cphMain_cphMain_dvStormWaterAsset_ddlTown", "label=" + town);
			selenium.Select("content_cphMain_cphMain_dvStormWaterAsset_ddlStormWaterAssetType", "label=" + stormwaterAssetType);
			selenium.Type("ctl00$ctl00$ctl00$content$cphMain$cphMain$dvStormWaterAsset$ctl02", assetNumber);
			for (int second = 0;; second++) {
				if (second >= 60) Assert.Fail("timeout");
				try
				{
					if (selenium.IsEditable("content_cphMain_cphMain_dvStormWaterAsset_ddlStreet")) break;
				}
				catch (Exception)
				{}
				Thread.Sleep(1000);
			}
			selenium.Select("content_cphMain_cphMain_dvStormWaterAsset_ddlStreet", "label=" + street);
			selenium.Click("content_cphMain_cphMain_dvStormWaterAsset_btnInsert");
			selenium.WaitForPageToLoad("30000");
}
}
}
