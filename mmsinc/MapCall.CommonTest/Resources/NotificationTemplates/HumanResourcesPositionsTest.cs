using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class HumanResourcesPositionsTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.HumanResources.Positions.{0}.cshtml";

        [TestMethod]
        public void TestPositionGroupsCreatedNotification()
        {
            var model = new PositionGroup {
                Id = 1,
                SAPPositionGroupKey = "Key1",
                PositionDescription = "Some Description"
            };

            var model2 = new PositionGroup {
                Id = 2,
                SAPPositionGroupKey = "Key2",
                PositionDescription = "Some Other Description"
            };
            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "PositionGroupsCreated", new[] {model, model2})
               .Trim();

            Assert.AreEqual(@"<h2>Position Groups Created</h2>

The MapCall employee sync process has created the following new position groups:<br /><br />

MapCall ID: 1<br />
SAP Position Group Key: Key1<br />
Position Description: Some Description<br /><br />
MapCall ID: 2<br />
SAP Position Group Key: Key2<br />
Position Description: Some Other Description<br /><br />", template);
        }
    }
}
