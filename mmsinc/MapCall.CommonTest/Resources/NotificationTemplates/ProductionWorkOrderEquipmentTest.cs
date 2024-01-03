using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class ProductionWorkOrderEquipmentTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestNeedsEmergencyRepairNotification()
        {
            var workOrderEquipment = new ProductionWorkOrderEquipmentNotification {
                RoutineWorkOrder = new ProductionWorkOrder {
                    Id = 1,
                    OperatingCenter = new OperatingCenter { OperatingCenterName = "TestOC" },
                    PlanningPlant = new PlanningPlant { Id = 1, Description = "TestPlant" },
                    Facility = new Facility { Id = 1, FacilityName = "TheFacility" },
                    DateReceived = new DateTime(2019, 11, 1),
                    ProductionWorkDescription = new ProductionWorkDescription {
                        Description = "TestDescription",
                        OrderType = new OrderType { Description = "test", Id = 1 }
                    },
                    CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode { Code = "TestCode" },
                    OrderNotes = "An order note."
                }
            };
            workOrderEquipment.RoutineWorkOrder.Equipments.Add(new ProductionWorkOrderEquipment {
                ProductionWorkOrder = workOrderEquipment.RoutineWorkOrder
            });
            var model = new ProductionWorkOrderEquipmentNotification {
                RoutineWorkOrder = workOrderEquipment.RoutineWorkOrder,
                ProductionWorkOrderId = 2,
                ProductionWorkDescription = "TestDescription",
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode { Code = "TestCode" },
                Equipment = new Equipment { Description = "TestDescription" },
                FacilityUrl = "http://www.microsoft.com",
                FacilityName = "TheFacility - 1",
                RoutineWorkOrderUrl = "http://www.apple.com",
                ProductionWorkOrderUrl = "http://www.internet.com",
                EquipmentUrl = "http://www.google.com",
                DateReceived = new DateTime(2019, 11, 1)
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.AsLeftConditionNeedsEmergencyRepair.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>As Left Condition - Needs Emergency Repair Notification</h2>

Production Work Order Number: <a href=""http://www.internet.com"">2</a><br />

Work Order Description: TestDescription<br />

Corrective Problem Code: TestCode<br />

Operating Center:  - TestOC<br />

Facility Name: <a href=""http://www.microsoft.com"">TheFacility - 1</a><br />

Equipment Description: <a href=""http://www.google.com"">TestDescription</a><br />

Routine Work Order Number: <a href=""http://www.apple.com"">1</a><br />

Routine Work Order Description: TestDescription<br />

Routine Work Order Notes: An order note.<br />

Date Created: 11/01/2019";

            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void TestNeedsReInspectionSoonerThanNormalNotification()
        {
            var workOrderEquipment = new ProductionWorkOrderEquipmentNotification {
                RoutineWorkOrder = new ProductionWorkOrder {
                    Id = 1,
                    OperatingCenter = new OperatingCenter { OperatingCenterName = "TestOC" },
                    PlanningPlant = new PlanningPlant { Id = 1, Description = "TestPlant" },
                    Facility = new Facility { FacilityName = "TheFacility" },
                    DateReceived = new DateTime(2019, 11, 1),
                    ProductionWorkDescription = new ProductionWorkDescription {
                        Description = "TestDescription",
                        OrderType = new OrderType { Description = "test", Id = 1 }
                    },
                    CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode { Code = "TestCode" },
                    OrderNotes = "This is an Order Note, I like order notes"
                }
            };
            workOrderEquipment.RoutineWorkOrder.Equipments.Add(new ProductionWorkOrderEquipment {
                ProductionWorkOrder = workOrderEquipment.RoutineWorkOrder,
            });
            var model = new ProductionWorkOrderEquipmentNotification {
                RoutineWorkOrder = workOrderEquipment.RoutineWorkOrder,
                ProductionWorkOrderId = 2,
                ProductionWorkDescription = "TestDescription",
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode { Code = "TestCode" },
                Equipment = new Equipment { Description = "TestDescription" },
                FacilityUrl = "http://www.microsoft.com",
                FacilityName = "TheFacility - 1",
                RoutineWorkOrderUrl = "http://www.apple.com",
                ProductionWorkOrderUrl = "http://www.internet.com",
                EquipmentUrl = "http://www.google.com",
                DateReceived = new DateTime(2019, 11, 1),
                AsLeftConditionComment = "This is a comment"
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.AsLeftConditionNeedsReInspectionSooner.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>As Left Condition - Needs Re-Inspection Sooner Notification</h2>

Production Work Order Number: <a href=""http://www.internet.com"">2</a><br />

Work Order Description: TestDescription<br />

Needs Re-inspection Sooner Than Normal Comments: This is a comment<br />

Operating Center:  - TestOC<br />

Facility Name: <a href=""http://www.microsoft.com"">TheFacility - 1</a><br />

Equipment Description: <a href=""http://www.google.com"">TestDescription</a><br />";

            Assert.AreEqual(expected, template);
        }
    }
}