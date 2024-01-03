using System;
using System.IO;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using RazorEngine;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class BlowOffInspectionTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.FieldServices.Assets.{0}.cshtml";

        #region Tests

        #region Notification Tests

        [TestMethod]
        public void TestBlowOffInspectionNotification()
        {
            var highValue = Convert.ToDecimal(3.33);
            var lowValue = Convert.ToDecimal(0.1);
            var okValue = Convert.ToDecimal(1.1);
            var model = new BlowOffInspection();
            model.Valve = new Valve();
            model.ResidualChlorine = lowValue;
            model.TotalChlorine = highValue;
            model.HydrantInspectionType = new HydrantInspectionType();
            model.FullFlow = true; //required
            model.GPM = okValue;
            model.Valve.OperatingCenter = new OperatingCenter();
            model.MinutesFlowed = okValue;
            model.StaticPressure = okValue; //required
            model.Valve.Town = new Town();
            model.Valve.Street = new Street();
            model.Valve.StreetNumber = "9001";
            model.Valve.CrossStreet = new Street();
            model.FreeNoReadReason = new NoReadReason();
            model.TotalNoReadReason = new NoReadReason();
            model.Valve.ValveNumber = "hll-0";
            model.InspectedBy = new User();
            model.Valve.OperatingCenter.OperatingCenterCode = "NJ7";
            model.Valve.OperatingCenter.OperatingCenterName = "Shrewsbury";
            model.InspectedBy.FullName = "Sherlock Homer";

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "BOChlorineReadingOutsideExpectedLimit", model);

            Assert.AreEqual(@"
<h2>Chlorine Readings Outside of Limits</h2>

Operating Center: NJ7 - Shrewsbury
<br />
<br />
Inspected by: Sherlock Homer
<br />
<br />
Inspection Record ID: <a> 0</a>
<br />
<br />
Asset: <a> hll-0</a>
<br />
<br />
    <b>Post Residual/Free Chlorine: 0.1</b>
<br />
<br />
    <b>Post Total Chlorine: 3.33</b>
<br />
<br />
Free No Read Reason: 
<br />
<br />
Total No Read Reason: 
<br />
<br />
Inspection Type: 
<br />
<br />
Full Flow: True
<br />
<br />
GPM: 1.1
<br />
<br />
Minutes Flowed: 1.1
<br />
<br />
Total Gallons: 1 (calculated GPM x Minutes Flowed)
<br />
<br />
Static Pressure: 1.1
<br />
<br />
Town: 
<br />
Street Number: 9001
<br />
Street: 
<br />
Cross Street: 
<br />
Date Inspected: 1/1/0001 12:00:00 AM", template);
        }

        [TestMethod]
        public void TestBlowOffInspectionNotificationValuesInRange()
        {
            var highValue = Convert.ToDecimal(3.33);
            var lowValue = Convert.ToDecimal(0.1);
            var okValue = Convert.ToDecimal(1.1);
            var model = new BlowOffInspection();
            model.Valve = new Valve();
            model.ResidualChlorine = okValue;
            model.TotalChlorine = okValue;
            model.HydrantInspectionType = new HydrantInspectionType();
            model.FullFlow = true; //required
            model.GPM = highValue;
            model.MinutesFlowed = highValue;
            model.StaticPressure = okValue; //required
            model.Valve.Town = new Town();
            model.Valve.Street = new Street();
            model.Valve.StreetNumber = "9001";
            model.Valve.CrossStreet = new Street();
            model.FreeNoReadReason = new NoReadReason();
            model.TotalNoReadReason = new NoReadReason();
            model.Valve.ValveNumber = "hll-0";
            model.Valve.OperatingCenter = new OperatingCenter();
            model.InspectedBy = new User();
            model.Valve.OperatingCenter.OperatingCenterCode = "NJ7";
            model.Valve.OperatingCenter.OperatingCenterName = "Shrewsbury";
            model.InspectedBy.FullName = "Sherlock Homer";

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "BOChlorineReadingOutsideExpectedLimit", model);

            Assert.AreEqual(@"
<h2>Chlorine Readings Outside of Limits</h2>

Operating Center: NJ7 - Shrewsbury
<br />
<br />
Inspected by: Sherlock Homer
<br />
<br />
Inspection Record ID: <a> 0</a>
<br />
<br />
Asset: <a> hll-0</a>
<br />
<br />
    Post Residual/Free Chlorine: 1.1
<br />
<br />
    Post Total Chlorine: 1.1
<br />
<br />
Free No Read Reason: 
<br />
<br />
Total No Read Reason: 
<br />
<br />
Inspection Type: 
<br />
<br />
Full Flow: True
<br />
<br />
GPM: 3.33
<br />
<br />
Minutes Flowed: 3.33
<br />
<br />
Total Gallons: 9 (calculated GPM x Minutes Flowed)
<br />
<br />
Static Pressure: 1.1
<br />
<br />
Town: 
<br />
Street Number: 9001
<br />
Street: 
<br />
Cross Street: 
<br />
Date Inspected: 1/1/0001 12:00:00 AM", template);
        }

        [TestMethod]
        public void TestBlowOffInspectionNotificationValuesBlank()
        {
            var highValue = Convert.ToDecimal(3.33);
            var lowValue = Convert.ToDecimal(0.1);
            var okValue = Convert.ToDecimal(1.1);
            var model = new BlowOffInspection();
            model.Valve = new Valve();
            model.Valve.OperatingCenter = new OperatingCenter();
            model.InspectedBy = new User();
            model.Valve.OperatingCenter.OperatingCenterCode = "NJ7";
            model.Valve.OperatingCenter.OperatingCenterName = "Shrewsbury";
            model.InspectedBy.FullName = "Sherlock Homer";

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "BOChlorineReadingOutsideExpectedLimit", model);

            Assert.AreEqual(@"
<h2>Chlorine Readings Outside of Limits</h2>

Operating Center: NJ7 - Shrewsbury
<br />
<br />
Inspected by: Sherlock Homer
<br />
<br />
Inspection Record ID: <a> 0</a>
<br />
<br />
Asset: <a> </a>
<br />
<br />
    Post Residual/Free Chlorine: 
<br />
<br />
    Post Total Chlorine: 
<br />
<br />
Free No Read Reason: 
<br />
<br />
Total No Read Reason: 
<br />
<br />
Inspection Type: 
<br />
<br />
Full Flow: 
<br />
<br />
GPM: 
<br />
<br />
Minutes Flowed: 
<br />
<br />
Total Gallons: 0 (calculated GPM x Minutes Flowed)
<br />
<br />
Static Pressure: 
<br />
<br />
Town: 
<br />
Street Number: 
<br />
Street: 
<br />
Cross Street: 
<br />
Date Inspected: 1/1/0001 12:00:00 AM", template);
        }

        #endregion

        #endregion
    }
}
