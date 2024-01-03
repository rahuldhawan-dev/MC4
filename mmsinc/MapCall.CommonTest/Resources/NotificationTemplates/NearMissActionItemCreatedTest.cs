using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class NearMissActionItemCreatedTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestNearMissActionItemCreatedNotification()
        {
            const int nearMissId = 100;

            var nearMiss = new NearMiss {
                Id = nearMissId, 
                RecordUrl = "http://recordUrl",
                ActionItems = new List<ActionItem<NearMiss>> {
                    new ActionItem<NearMiss> { 
                        ActionItem = new ActionItem {
                            Id = 42,
                            LinkedId = nearMissId,
                            Note = "testing-note",
                            Type = new ActionItemType { 
                                Id = 4, 
                                Description = "type-description"
                            },
                            NotListedType = "not-listed-type",
                            TargetedCompletionDate = new DateTime(2023, 6, 27, 12, 30, 00)
                        }
                    }
                }
            };

            var template = RenderTemplate(
                "MapCall.Common.Resources.NotificationTemplates.Operations.HealthAndSafety.NearMissActionItemCreated.cshtml",
                nearMiss);

            Assert.AreEqual(@"<h2>Action Item Created</h2>
<br />Action item created for Near Miss Record Id: <a href=""http://recordUrl"">100</a>
<br />Action Item: testing-note
<br />Type: type-description
<br />Not Listed Type: not-listed-type
<br />Targeted Completion Date: 6/27/2023 12:30:00 PM", template);
        }
    }
}
