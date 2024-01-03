using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class FieldServicesProjectsRPTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.FieldServices.Projects.{0}.cshtml";

        #region Tests

        #region HS Incident

        [TestMethod]
        public void TestProjectsRPNewRecord()
        {
            var model = new RecurringProject {
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "OJ", OperatingCenterName = "Simpson"},
                Town = new Town {ShortName = "Don't want no short named town"},
                ProjectTitle = "Some Title",
                ProjectDescription = "Some Description",
                District = 43,
                OriginationYear = 1999,
                NJAWEstimate = 5319,
                RecurringProjectType = new RecurringProjectType {Description = "Some Project Type"},
                ProposedLength = 19,
                EstimatedProjectDuration = 15,
                EstimatedInServiceDate = new DateTime(1984, 4, 24),
                ProposedDiameter = new PipeDiameter {Diameter = 3},
                ProposedPipeMaterial = new PipeMaterial {Description = "Some material"},
                AcceleratedAssetInvestmentCategory = new AssetInvestmentCategory {Description = "Whatever this is"},
                SecondaryAssetInvestmentCategory = new AssetInvestmentCategory {Description = "Whatever that is"},
                Justification = "Because",
                CreatedBy = new User {UserName = "Me"}
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "ProjectsRPNewRecord", model);

            Assert.AreEqual(@"<h2>New ProjectsRP Record</h2>

Id: 42 <br />
Operating Center: OJ - Simpson <br />
Town: Don&#39;t want no short named town <br />
Project Title: Some Title <br />
Project Description: Some Description <br />
District: 43 <br />
Originiation Year: 1999 <br />
NJAW Estimate (Dollars): 5319 <br />
Project Type: Some Project Type <br />
Proposed Length (ft): 19 <br />
Estimated Project Duration (days): 15 <br />
Estimated In Service Date: 4/24/1984 <br />
Proposed Diameter: 3 <br />
Proposed Pipe Material: Some material <br />
Accelerated Asset Investment Category: Whatever this is <br />
Secondary Asset Investment Category: Whatever that is <br />
Justification: Because <br />
Created By: Me <br />",
                template);
        }

        [TestMethod]
        public void TestProjectsRPNewRecordWhenAllParametersAreNull()
        {
            var model = new RecurringProject();

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "ProjectsRPNewRecord", model);

            Assert.AreEqual(@"<h2>New ProjectsRP Record</h2>

Id: 0 <br />
Operating Center:  <br />
Town:  <br />
Project Title:  <br />
Project Description:  <br />
District:  <br />
Originiation Year:  <br />
NJAW Estimate (Dollars): 0 <br />
Project Type:  <br />
Proposed Length (ft):  <br />
Estimated Project Duration (days):  <br />
Estimated In Service Date:  <br />
Proposed Diameter:  <br />
Proposed Pipe Material:  <br />
Accelerated Asset Investment Category:  <br />
Secondary Asset Investment Category:  <br />
Justification:  <br />
Created By:  <br />", template);
        }

        #endregion

        #region Roadway Improvement Notification Created

        [TestMethod]
        public void TestRoadwayImprovementNotificationCreated()
        {
            var model = new RoadwayImprovementNotification {
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "OJ", OperatingCenterName = "Simpson"},
                Town = new Town {ShortName = "Don't want no short named town"},
                RoadwayImprovementNotificationEntity = new RoadwayImprovementNotificationEntity {Description = "Neat"},
                Description = "A description",
                ExpectedProjectStartDate = new DateTime(1984, 4, 24),
                DateReceived = new DateTime(1985, 5, 25),
                RoadwayImprovementNotificationStatus = new RoadwayImprovementNotificationStatus {Description = "Cool"},
                PreconMeetingDate = new DateTime(1986, 6, 6),
                RoadwayImprovementNotificationPreconAction = new RoadwayImprovementNotificationPreconAction
                    {Description = "Oh that"}
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "RoadwayImprovementNotificationCreated", model);

            Assert.AreEqual(@"<h2>New Roadway Improvement Notification</h2>

Id: 42<br />
Operating Center: OJ - Simpson<br/>
Town: Don&#39;t want no short named town<br/>
Entity: Neat<br/>
Description: A description<br/>
Expected Project Start Date: 4/24/1984 12:00:00 AM<br />
Date Received: 5/25/1985 12:00:00 AM<br/>
Status: Cool<br/>
Precon Meeting Date: 6/6/1986 12:00:00 AM<br/>
Precon Action Taken: Oh that<br/>",
                template);
        }

        [TestMethod]
        public void TestRoadwayImprovementNotificationCreatedWhenAllParamsAreNull()
        {
            var model = new RoadwayImprovementNotification();

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "RoadwayImprovementNotificationCreated", model);

            Assert.AreEqual(@"<h2>New Roadway Improvement Notification</h2>

Id: 0<br />
Operating Center: <br/>
Town: <br/>
Entity: <br/>
Description: <br/>
Expected Project Start Date: 1/1/0001 12:00:00 AM<br />
Date Received: 1/1/0001 12:00:00 AM<br/>
Status: <br/>
Precon Meeting Date: <br/>
Precon Action Taken: <br/>",
                template);
        }

        #endregion

        #endregion
    }
}
