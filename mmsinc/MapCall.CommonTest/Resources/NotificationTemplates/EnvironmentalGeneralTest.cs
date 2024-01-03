using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class EnvironmentalGeneralTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestEnvironmentalNonComplianceEventCreatedTemplate()
        {
            var model = new EnvironmentalNonComplianceEventCreatedNotification {
                CreatedByFullName = "Ronald McDonald",
                EnvironmentalNonComplianceEvent = new EnvironmentalNonComplianceEvent {
                    Id = 42,
                    State = new State {Abbreviation = "XX"},
                    OperatingCenter = new OperatingCenter {
                        OperatingCenterCode = "QQ1",
                        OperatingCenterName = "OCN"
                    },
                    PublicWaterSupply = new PublicWaterSupply {
                        Identifier = "I",
                        OperatingArea = "OA",
                        System = "S"
                    },
                    WasteWaterSystem = new WasteWaterSystem {
                        //PermitNumber = "PN",
                        WasteWaterSystemName = "WWSN",
                        OperatingCenter = new OperatingCenter {
                            OperatingCenterCode = "QQ1",
                            OperatingCenterName = "OCN"
                        },
                        Id = 2
                    },
                    EventDate = new DateTime(1984, 4, 24, 4, 0, 4),
                    IssueStatus = new EnvironmentalNonComplianceEventStatus {Description = "Some status"},
                    IssueType = new EnvironmentalNonComplianceEventType {Description = "Some type"},
                    IssueSubType = new EnvironmentalNonComplianceEventSubType {Description = "Some type"},
                    FailureType = new EnvironmentalNonComplianceEventFailureType {Description = "Some failure type"},
                    FailureTypeDescription = "Some failure type description"
                },
                RecordUrl = "https://mapcall.amwater.com/modules/mvc/Environmental/EnvironmentalNonComplianceEvent/Show/42"
            };
            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Environmental.General.EnvironmentalNonComplianceEventCreated.cshtml";

            var expected = @"<h2>Environmental Non-Compliance Event Created</h2>

<a href=""https://mapcall.amwater.com/modules/mvc/Environmental/EnvironmentalNonComplianceEvent/Show/42"">View on MapCall</a> <br /><br />

Created By: Ronald McDonald <br />
State: XX <br />
Operating Center: QQ1 - OCN <br />
Public Water System: I - OA - S <br />
WW System: QQ1WW0002 - WWSN <br />
Event Date: 4/24/1984 4:00:04 AM <br />
Status: Some status <br />
Type: Some type <br />
Sub Type: Some type <br />
Failure Type: Some failure type <br />
Failure Type Description: Some failure type description <br />";

            var template = RenderTemplate(streamPath, model);
            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void TestEnvironmentalNonComplianceEventCreatedTemplateWhenNullsShowUp()
        {
            var model = new EnvironmentalNonComplianceEventCreatedNotification {
                CreatedByFullName = "Ronald McDonald",
                EnvironmentalNonComplianceEvent = new EnvironmentalNonComplianceEvent {
                    Id = 42,
                    State = new State {Abbreviation = "XX"},
                    OperatingCenter = new OperatingCenter {
                        OperatingCenterCode = "QQ1",
                        OperatingCenterName = "OCN"
                    },
                    EventDate = new DateTime(1984, 4, 24, 4, 0, 4),
                    IssueStatus = new EnvironmentalNonComplianceEventStatus {Description = "Some status"},
                    IssueType = new EnvironmentalNonComplianceEventType {Description = "Some type"},
                },
                RecordUrl = "https://mapcall.amwater.com/modules/mvc/Environmental/EnvironmentalNonComplianceEvent/Show/42"
            };
            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Environmental.General.EnvironmentalNonComplianceEventCreated.cshtml";
            var expected = @"<h2>Environmental Non-Compliance Event Created</h2>

<a href=""https://mapcall.amwater.com/modules/mvc/Environmental/EnvironmentalNonComplianceEvent/Show/42"">View on MapCall</a> <br /><br />

Created By: Ronald McDonald <br />
State: XX <br />
Operating Center: QQ1 - OCN <br />
Public Water System:  <br />
WW System:  <br />
Event Date: 4/24/1984 4:00:04 AM <br />
Status: Some status <br />
Type: Some type <br />
Sub Type:  <br />
Failure Type:  <br />
Failure Type Description:  <br />";

            var template = RenderTemplate(streamPath, model);
            Assert.AreEqual(expected, template);
        }
    }
}
