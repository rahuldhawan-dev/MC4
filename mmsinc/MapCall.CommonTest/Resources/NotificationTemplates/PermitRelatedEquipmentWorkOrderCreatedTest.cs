using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class PermitRelatedEquipmentWorkOrderCreatedTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestNotification()
        {
            var model = new ProductionWorkOrder {
                Id = 321,
                FunctionalLocation = "1123-1222-13211-1111",
                DateReceived = new DateTime(2019, 1, 1),
                ProductionWorkDescription = new ProductionWorkDescription {Description = "do stuff with equipment."},
                RecordUrl = "http://recordUrl1"
            };
            var equipment = new Equipment {Id = 2, Number = 123, Description = "main equipment", RecordUrl = "http://recordUrl2" };
            equipment.EnvironmentalPermits.Add(new EnvironmentalPermit {Id = 1, Description = "foo", RecordUrl = "http://recordUrl4" });
            var equipmentChild = new Equipment {Id = 5, Number = 253, Description = "child equipment", RecordUrl = "http://recordUrl3" };
            equipmentChild.EnvironmentalPermits.Add(new EnvironmentalPermit {Id = 2, Description = "bar", RecordUrl = "http://recordUrl5" });
            equipmentChild.EnvironmentalPermits.Add(new EnvironmentalPermit {Id = 3, Description = "baz", RecordUrl = "http://recordUrl6" });

            model.Equipments.Add(new ProductionWorkOrderEquipment {
                ProductionWorkOrder = model,
                Equipment = equipment
            });
            model.Equipments.Add(new ProductionWorkOrderEquipment {
                ProductionWorkOrder = model, Equipment = equipmentChild
            });

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.PermitRelatedEquipmentWorkOrderCreated.cshtml";
            var template = RenderTemplate(streamPath, model);

            Assert.AreEqual(
                @"A Production Work Order has been created for equipment that is linked to an Environmental Permit.<br />

<h4>Linked Permits:</h4>
<div>Permit Description: <a href=""http://recordUrl4"">foo</a></div>
<div>Permit Description: <a href=""http://recordUrl5"">bar</a></div>
<div>Permit Description: <a href=""http://recordUrl6"">baz</a></div>

<h4>Linked Equipment: </h4>
<a href=""http://recordUrl2"">main equipment</a><br />
<a href=""http://recordUrl3"">child equipment</a><br />

<h4>Work Order:</h4>
Order Number: <a href=""http://recordUrl1"">321</a><br />
Description: do stuff with equipment.<br/>
Created On 1/1/2019 12:00:00 AM<br />", template);
        }
    }
}
