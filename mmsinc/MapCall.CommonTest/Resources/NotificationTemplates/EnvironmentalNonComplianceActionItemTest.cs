using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class EnvironmentalNonComplianceActionItemTest : BaseNotificationTest
    {
        #region Constants

        private const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.Environmental.General.{0}.cshtml";

        public const string FAILURE_TYPE_DESCRIPTION = "An enormous hedgehog called Spiny Norman",
                            ACTION_ITEM = "Dinsdale and Doug";

        #endregion

        #region Tests

        [TestMethod]
        public void TestEnvironmentalNonComplianceActionItemNotification()
        {
            var modelEvent = new EnvironmentalNonComplianceEvent() {
                FailureTypeDescription = FAILURE_TYPE_DESCRIPTION,
                PublicWaterSupply = new PublicWaterSupply(),
                WasteWaterSystem = new WasteWaterSystem(),
                EventDate = new DateTime(2061, 12, 24)
            };

            var modelActionItem = new EnvironmentalNonComplianceEventActionItem() {
                Id = 1,
                ActionItem = ACTION_ITEM,
                ResponsibleOwner = new User(),
                TargetedCompletionDate = new DateTime(2041, 7, 4)
            };

            var model = new EnvironmentalNonComplianceActionItemAssignedNotification() {
                EnvironmentalNonComplianceEvent = modelEvent,
                EnvironmentalNonComplianceEventActionItem = modelActionItem,
                AssignedToFullName = modelActionItem.ResponsibleOwner.FullName,
                RecordUrl = $"TestUrl/Environmental/EnvironmentalNonComplianceEventActionItem/Show/{modelActionItem.Id}",
                HelpUrl = "TestUrl/HelpTopic/Show/271"
            };

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "EnvironmentalNonComplianceActionItemAssigned",
                model);

            Assert.AreEqual(@"<h2>Action Item Assigned To You</h2><br />
You’ve been assigned an action item related to a non-compliance event that was entered in MapCall. Instructions for providing updates on action taken and closing the action item are available <a href=""TestUrl/HelpTopic/Show/271"">here</a><br />
You will receive reminders about this action item every 30 days until it has been closed. If you want to extend this to every 90 days, please reach out to your MapCall Champion. If you believe that this action item should be assigned to another individual, please contact your supervisor.<br />
Details of the Non-Compliance Event are as follows:<br />

Failure Type Description: An enormous hedgehog called Spiny Norman<br />
Public Water Supply:  - - <br />
Waste Water Supply: WW0000 - <br />
Event Date: 12/24/2061 12:00:00 AM<br />
Link for record: <a href=""TestUrl/Environmental/EnvironmentalNonComplianceEventActionItem/Show/1"">Environmental Non Compliance Event</a><br />
Action Item: Dinsdale and Doug<br />
Targeted Completion Date: 7/4/2041 12:00:00 AM<br />", template);
        }

        #endregion
    }

}