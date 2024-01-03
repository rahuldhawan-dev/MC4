using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.SAPTest.Model.Entities
{
    [TestClass]
    public class SAPProgressWorkOrderTest
    {
        /// <summary>
        /// This was always returning the OperatingCenter.DistributionPlanningPlant,
        /// This test is to ensure it's setting it to what the work order currently returns, not the operating center
        /// In this case it'll be TownSection.Distribution.PlanningPlant
        ///
        /// This test brings up a lot of refactoring that should happen, but going further down this
        /// rabbit hole right now is not an option.
        /// </summary>
        [TestMethod]
        public void TestConstructorSetsOperatingCenterToPlanningPlantFromWorkOrderTownSectionNotOperatingCenter()
        {
            var distributionPlanningPlant = new PlanningPlant { Code = "D108" };
            var wrongPlant = new PlanningPlant { Code = "D817" };
            var operatingCenter = new OperatingCenter();
            operatingCenter.PlanningPlants.Add(wrongPlant);
            var townSection = new TownSection {DistributionPlanningPlant = distributionPlanningPlant};
            var workDescription = new WorkDescription { Description = "blah" };

            foreach (var at in new[] {"HYDRANT", "VALVE", "MAIN", "SERVICE"})
            {
                var assetType = new AssetType {Description = at};
                var order = new WorkOrder {
                    AssetType = assetType,
                    OperatingCenter = operatingCenter,
                    TownSection = townSection,
                    WorkDescription = workDescription
                };

                var target = new SAPProgressWorkOrder(order);

                Assert.AreEqual(distributionPlanningPlant.Code, target.OperatingCenter, $"Failed for {at}");
            }
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForSAPWorkOrderServiceGatheringEquipmentNoAndFunctionalLocationFromOperatingCenterTownWhenDescriptionIsNoPremise()
        {
            var town = new Town { Id = 1 };
            var operatingCenter = new OperatingCenter { Id = 2 };
            var oct = new OperatingCenterTown {
                Town = town,
                OperatingCenter = operatingCenter,
                MainSAPEquipmentId = 3,
                MainSAPFunctionalLocation = new FunctionalLocation {
                    Description = nameof(FunctionalLocation)
                }
            };
            town.OperatingCentersTowns.Add(oct);
            operatingCenter.OperatingCenterTowns.Add(oct);

            var workOrder = new WorkOrder {
                AssetType = new AssetType { Description = "SERVICE" },
                WorkDescription = new WorkDescription { Description = "SERVICE LINE RETIRE NO PREMISE" },
                Town = town,
                OperatingCenter = operatingCenter
            };

            var target = new SAPProgressWorkOrder(workOrder);
            
            Assert.AreEqual(oct.MainSAPFunctionalLocation.ToString(), target.SAPFunctionalLoc);
            Assert.AreEqual(oct.MainSAPEquipmentId.ToString(), target.SAPEquipmentNo);
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForSAPWorkOrderServiceGatheringEquipmentNoAndFunctionalLocationFromTownSectionWhenDescriptionIsNoPremiseAndOperatingCenterTownDoesNotHaveThem()
        {
            var town = new Town { Id = 1 };
            var operatingCenter = new OperatingCenter { Id = 2 };
            var oct = new OperatingCenterTown {
                Town = town,
                OperatingCenter = operatingCenter
            };
            town.OperatingCentersTowns.Add(oct);
            operatingCenter.OperatingCenterTowns.Add(oct);
            var townSection = new TownSection {
                MainSAPEquipmentId = 3,
                MainSAPFunctionalLocation = new FunctionalLocation {
                    Description = nameof(FunctionalLocation)
                }
            };

            var workOrder = new WorkOrder {
                AssetType = new AssetType { Description = "SERVICE" },
                WorkDescription = new WorkDescription { Description = "SERVICE LINE RETIRE NO PREMISE" },
                Town = town,
                OperatingCenter = operatingCenter,
                TownSection = townSection
            };

            var target = new SAPProgressWorkOrder(workOrder);
            
            Assert.AreEqual(townSection.MainSAPFunctionalLocation.ToString(), target.SAPFunctionalLoc);
            Assert.AreEqual(townSection.MainSAPEquipmentId.ToString(), target.SAPEquipmentNo);
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesFor()
        {
            var meterLocationSAPCode = "0002";
            var target = new SAPProgressWorkOrder(new WorkOrder {
                PlannedCompletionDate = DateTime.Today,
                MeterLocation = new MeterLocation { SAPCode = meterLocationSAPCode}
            });

            var result = target.ProcessWorkOrderRequest();

            Assert.AreEqual(DateTime.Today.ToString(SAPProgressWorkOrder.SAP_DATE_FORMAT), target.BasicFinish);
            Assert.AreEqual(meterLocationSAPCode, target.Location);
        }

        [TestMethod]
        public void TestMapToWorkOrderSetsBusinessUnit()
        {
            var order = new WorkOrder();
            var target = new SAPProgressWorkOrder(order) {
                CostCenter = "1234"
            };

            target.MapToWorkOrder(order);

            Assert.AreEqual(target.CostCenter, order.BusinessUnit);
        }

        [TestMethod]
        public void TestMapToWorkOrderDoesNotNullOutBusinessUnit()
        {
            var order = new WorkOrder { BusinessUnit = "12354" };
            var target = new SAPProgressWorkOrder(order) {
                CostCenter = ""
            };

            target.MapToWorkOrder(order);

            Assert.AreEqual("12354", order.BusinessUnit);
        }

        [TestMethod]
        public void TestMapToWorkOrderSetsSAPOrderNumber()
        {
            var order = new WorkOrder();
            var target = new SAPProgressWorkOrder(order) {
                OrderNumber = "1234"
            };

            target.MapToWorkOrder(order);

            Assert.AreEqual(target.OrderNumber, order.SAPWorkOrderNumber.ToString());
        }

        [TestMethod]
        public void TestMapToWorkOrderDoesNotNullOutSAPOrderNumber()
        {
            var order = new WorkOrder { SAPWorkOrderNumber = 1234 };
            var target = new SAPProgressWorkOrder(order) {
                OrderNumber = ""
            };

            target.MapToWorkOrder(order);

            Assert.AreEqual(1234, order.SAPWorkOrderNumber);
        }

        public void TestMapToWorkOrderSetsWBSElement()
        {
            var order = new WorkOrder();
            var target = new SAPProgressWorkOrder(order) {
                WBSElement = "1234"
            };

            target.MapToWorkOrder(order);

            Assert.AreEqual(target.WBSElement, order.AccountCharged);
        }

        [TestMethod]
        public void TestMapToWorkOrderDoesNotNullOutWBSElement()
        {
            var order = new WorkOrder { AccountCharged = "12354" };
            var target = new SAPProgressWorkOrder(order) {
                WBSElement = ""
            };

            target.MapToWorkOrder(order);

            Assert.AreEqual("12354", order.AccountCharged);
        }

        public void TestMapToWorkOrderSetsMaterialDocId()
        {
            var order = new WorkOrder();
            var target = new SAPProgressWorkOrder(order) {
                MaterialDocument = "1234"
            };

            target.MapToWorkOrder(order);

            Assert.AreEqual(target.MaterialDocument, order.MaterialsDocID);
        }

        [TestMethod]
        public void TestMapToWorkOrderDoesNotNullOutMaterialDocId()
        {
            var order = new WorkOrder { MaterialsDocID = "12354" };
            var target = new SAPProgressWorkOrder(order) {
                MaterialDocument = ""
            };

            target.MapToWorkOrder(order);

            Assert.AreEqual("12354", order.MaterialsDocID);
        }

        public void TestMapToWorkOrderSetsSAPNotificationNumber()
        {
            var order = new WorkOrder();
            var target = new SAPProgressWorkOrder(order) {
                NotificationNumber = "1234"
            };

            target.MapToWorkOrder(order);

            Assert.AreEqual(target.NotificationNumber, order.SAPNotificationNumber);
        }

        [TestMethod]
        public void TestMapToWorkOrderDoesNotNullOutSAPNotificationNumber()
        {
            var order = new WorkOrder { SAPNotificationNumber = 12354 };
            var target = new SAPProgressWorkOrder(order) {
                NotificationNumber = ""
            };

            target.MapToWorkOrder(order);

            Assert.AreEqual(12354, order.SAPNotificationNumber);
        }

        [TestMethod]
        public void TestMapToWorkOrderSetsSAPErrorCodeToStatus()
        {
            var order = new WorkOrder();
            var target = new SAPProgressWorkOrder(order) {
                Status = "This is my error, there are many like it, but this one is mine."
            };

            target.MapToWorkOrder(order);

            Assert.AreEqual(target.Status, order.SAPErrorCode);
        }

        [TestMethod]
        public void TestConstructorSetsMeterSetToYWhenServiceInstallationsAndWorkDescriptionValid()
        {
            foreach (var workDescriptionId in WorkDescription.NEW_SERVICE_INSTALLATION)
            {
                var order = new WorkOrder {
                    WorkDescription = new WorkDescription { Id = workDescriptionId },
                    DateCompleted = DateTime.Now,
                    CrewAssignments = new[] { new CrewAssignment { DateStarted = DateTime.Now } }
                };
                order.ServiceInstallations.Add(new ServiceInstallation());

                var target = new SAPProgressWorkOrder(order);

                Assert.AreEqual("Y", target.SetMeter);
            }
        }

        [TestMethod]
        public void TestConstructorSetsMeterSetToNWhenNoServiceInstallations()
        {
            foreach (var workDescriptionId in WorkDescription.NEW_SERVICE_INSTALLATION)
            {
                var order = new WorkOrder {
                    WorkDescription = new WorkDescription { Id = workDescriptionId },
                    DateCompleted = DateTime.Now,
                    CrewAssignments = new[] { new CrewAssignment { DateStarted = DateTime.Now} }
                };

                var target = new SAPProgressWorkOrder(order);

                Assert.AreEqual("N", target.SetMeter);
            }
        }

        [TestMethod]
        public void TestConstructorDoesNotSetMeterSetWhenCancelled()
        {
            foreach (var workDescriptionId in WorkDescription.NEW_SERVICE_INSTALLATION)
            {
                var order = new WorkOrder {
                    WorkDescription = new WorkDescription { Id = workDescriptionId },
                    CancelledAt = DateTime.Now,
                    CrewAssignments = new[] { new CrewAssignment { DateStarted = DateTime.Now} }
                };

                var target = new SAPProgressWorkOrder(order);

                Assert.IsNull(target.SetMeter);
            }
        }

        [TestMethod]
        public void TestConstructorSetsCrewAssignmentFromDateCompleted()
        {
            foreach (var workDescriptionId in WorkDescription.NEW_SERVICE_INSTALLATION)
            {
                var order = new WorkOrder {
                    WorkDescription = new WorkDescription { Id = workDescriptionId },
                    DateCompleted = DateTime.Now
                };

                var target = new SAPProgressWorkOrder(order);

                Assert.AreEqual(DateTime.Today.ToString(SAPProgressWorkOrder.SAP_DATE_FORMAT), target.sapCrewAssignments.FirstOrDefault().DateCompleted);
                Assert.IsNull(target.SetMeter);
            }
        }
    }
}