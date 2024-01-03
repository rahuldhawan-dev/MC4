using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class WaterQualityGeneralTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestBactiInputTrigger()
        {
            var model = new BacterialWaterSample {
                Cl2Total = 0.15m,
                Cl2Free = 0.4m,
                FreeAmmonia = 0.2m,
                ColiformConfirm = false
            };
            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.WaterQuality.General.BactiInputTrigger.cshtml";
            var template = RenderTemplate(streamPath, model);

            Assert.AreEqual(@"<h2>WQ Sample Bacti</h2>

Triggered By: <br/>
    
        Cl2 Total : 0.15 <br />
    
<br/><br/>
Detail:<br/>
ID: 0 <br/>
Sample Site:  <br/>
Sample Date:  <br/>
Bacti Sample Type:  <br/>
Collected By:  <br/>
Analysis Performed By:  <br/>
Location:  <br />
Cl2 Free: 0.4 <br />
Cl2 Total: 0.15 <br />
Nitrite:  <br />
Nitrate:  <br />
HPC:  <br />
Monochloramine:  <br />
Free Ammonia: 0.2 <br />
pH:  <br />
Temperature (C): <br />
Value Fe:  <br />
Value Mn:  <br />
Value Turb:  <br />
Value Ortho:  <br />
Value Conductivity:  <br />
Non Sheen Colony Count:  <br />
Non Sheen Colony Count Operator:  <br />
Sheen Colony Count:  <br />
Sheen Colony CountOperator:  <br />
Coliform Confirm: Absent <br />
E Coli Confirm: Absent <br />
DTM Incubator In:  <br />
DTM Incubator Out:  <br />
DTM Data Entered:  <br /> 
Flush Time (min):  <br />", template);
        }

        [TestMethod]
        public void TestBactiInputTriggerWhenAllNullableValuesAreNull()
        {
            var model = new BacterialWaterSample {
                FreeAmmonia = 0.2m,
                ColiformConfirm = false
            };
            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.WaterQuality.General.BactiInputTrigger.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>WQ Sample Bacti</h2>

Triggered By: <br/>
<br/><br/>
Detail:<br/>
ID: 0 <br/>
Sample Site:  <br/>
Sample Date:  <br/>
Bacti Sample Type:  <br/>
Collected By:  <br/>
Analysis Performed By:  <br/>
Location:  <br />
Cl2 Free:  <br />
Cl2 Total:  <br />
Nitrite:  <br />
Nitrate:  <br />
HPC:  <br />
Monochloramine:  <br />
Free Ammonia: 0.2 <br />
pH:  <br />
Temperature (C): <br />
Value Fe:  <br />
Value Mn:  <br />
Value Turb:  <br />
Value Ortho:  <br />
Value Conductivity:  <br />
Non Sheen Colony Count:  <br />
Non Sheen Colony Count Operator:  <br />
Sheen Colony Count:  <br />
Sheen Colony CountOperator:  <br />
Coliform Confirm: Absent <br />
E Coli Confirm: Absent <br />
DTM Incubator In:  <br />
DTM Incubator Out:  <br />
DTM Data Entered:  <br /> 
Flush Time (min):  <br />";

            Assert.AreEqual(expected, template);
        }
    }
}
