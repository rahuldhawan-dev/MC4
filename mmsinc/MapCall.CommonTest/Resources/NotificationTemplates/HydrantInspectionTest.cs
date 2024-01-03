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
    public class HydrantInspectionTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.FieldServices.Assets.{0}.cshtml";

        #region Tests

        #region Notification Tests

        [TestMethod]
        public void TestHydrantInspectionNotification()
        {
            var highValue = Convert.ToDecimal(3.33);
            var lowValue = Convert.ToDecimal(0.1);
            var okValue = Convert.ToDecimal(1.1);
            var model = new HydrantInspection();
            model.OperatingCenter = new OperatingCenter();
            model.OperatingCenter.OperatingCenterCode = "NJ7";
            model.OperatingCenter.OperatingCenterName = "Shrewsbury";
            model.InspectedBy = new User();
            model.InspectedBy.FullName = "Sherlock Homer";
            model.Hydrant = new Hydrant();
            model.ResidualChlorine = lowValue;
            model.TotalChlorine = highValue;
            model.HydrantInspectionType = new HydrantInspectionType();
            model.FullFlow = true; //required
            model.GPM = okValue;
            model.MinutesFlowed = okValue;
            model.StaticPressure = okValue; //required
            model.Hydrant.Town = new Town();
            model.Hydrant.Street = new Street();
            model.Hydrant.StreetNumber = "9001";
            model.Hydrant.CrossStreet = new Street();
            model.FreeNoReadReason = new NoReadReason();
            model.TotalNoReadReason = new NoReadReason();
            model.Hydrant.HydrantNumber = "hll-0";

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "HydrantChlorineReadingOutsideExpectedLimit",
                model);

            Assert.AreEqual(@"
<h2>Chlorine Readings Outside of Limits</h2>

Operating Center: NJ7 - Shrewsbury
<br />
<br />
Inspected by: Sherlock Homer
<br />
<br />
Inspection Record ID: <a>0</a>
<br />
<br />
Asset: <a>hll-0</a>
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
        public void TestHydrantInspectionNotificationValuesInRange()
        {
            var highValue = Convert.ToDecimal(3.33);
            var lowValue = Convert.ToDecimal(0.1);
            var okValue = Convert.ToDecimal(1.1);
            var model = new HydrantInspection();
            model.Hydrant = new Hydrant();
            model.OperatingCenter = new OperatingCenter();
            model.OperatingCenter.OperatingCenterCode = "NJ7";
            model.OperatingCenter.OperatingCenterName = "Shrewsbury";
            model.InspectedBy = new User();
            model.InspectedBy.FullName = "Sherlock Homer";
            model.ResidualChlorine = lowValue;
            model.TotalChlorine = highValue;
            model.HydrantInspectionType = new HydrantInspectionType();
            model.FullFlow = true; //required
            model.GPM = okValue;
            model.MinutesFlowed = okValue;
            model.StaticPressure = okValue; //required
            model.Hydrant.Town = new Town();
            model.Hydrant.Street = new Street();
            model.Hydrant.StreetNumber = "9001";
            model.Hydrant.CrossStreet = new Street();
            model.FreeNoReadReason = new NoReadReason();
            model.TotalNoReadReason = new NoReadReason();
            model.Hydrant.HydrantNumber = "hll-0";

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "HydrantChlorineReadingOutsideExpectedLimit",
                model);

            Assert.AreEqual(@"
<h2>Chlorine Readings Outside of Limits</h2>

Operating Center: NJ7 - Shrewsbury
<br />
<br />
Inspected by: Sherlock Homer
<br />
<br />
Inspection Record ID: <a>0</a>
<br />
<br />
Asset: <a>hll-0</a>
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
        public void TestHydrantInspectionNotificationValuesblank()
        {
            var highValue = Convert.ToDecimal(3.33);
            var lowValue = Convert.ToDecimal(0.1);
            var okValue = Convert.ToDecimal(1.1);
            var model = new HydrantInspection();
            model.Hydrant = new Hydrant();
            model.Hydrant.HydrantNumber = "hll-0";
            model.OperatingCenter = new OperatingCenter();
            model.OperatingCenter.OperatingCenterCode = "NJ7";
            model.OperatingCenter.OperatingCenterName = "Shrewsbury";
            model.InspectedBy = new User();
            model.InspectedBy.FullName = "Sherlock Homer";

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "HydrantChlorineReadingOutsideExpectedLimit",
                model);

            Assert.AreEqual(@"
<h2>Chlorine Readings Outside of Limits</h2>

Operating Center: NJ7 - Shrewsbury
<br />
<br />
Inspected by: Sherlock Homer
<br />
<br />
Inspection Record ID: <a>0</a>
<br />
<br />
Asset: <a>hll-0</a>
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
