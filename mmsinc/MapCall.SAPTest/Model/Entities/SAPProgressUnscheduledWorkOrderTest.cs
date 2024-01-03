using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SAP.DataTest.Model.Entities
{
    [TestClass()]
    public class SAPProgressUnscheduledWorkOrderTest
    {
        #region Private Members

        private ProductionWorkOrder productionWorkOrder;

        #endregion

        #region Public Methods

        public ProductionWorkOrder GetTestProductionWorkOrderProgress()
        {
            productionWorkOrder = new ProductionWorkOrder();
            productionWorkOrder.PlanningPlant = new PlanningPlant {Code = ""};
            productionWorkOrder.SAPNotificationNumber = 15625900;
            productionWorkOrder.SAPWorkOrder = "90451000";
            productionWorkOrder.FunctionalLocation = "NJLK-HO-ADDIS";
            productionWorkOrder.RequestedBy = new Employee {FirstName = "Bill", LastName = "Preston", MiddleName = ""};
            productionWorkOrder.Priority = new ProductionWorkOrderPriority {Description = "", Id = 0};
            productionWorkOrder.ProductionWorkDescription = new ProductionWorkDescription {
                BreakdownIndicator = true, Description = "", Id = 0,
                PlantMaintenanceActivityType = new PlantMaintenanceActivityType {Description = "", Code = ""}
            };

            productionWorkOrder.DateCompleted = DateTime.Now;
            productionWorkOrder.DateReceived = DateTime.Now;
            productionWorkOrder.WBSElement = "";
            var employeeAssignment = new HashSet<EmployeeAssignment> {
                new EmployeeAssignment {
                    AssignedTo = new Employee {EmployeeId = "50248997"}, AssignedOn = DateTime.Now,
                    DateStarted = DateTime.Now, DateEnded = DateTime.Today.AddDays(1)
                },
                new EmployeeAssignment {
                    AssignedTo = new Employee {EmployeeId = "50358653"}, AssignedOn = DateTime.Now,
                    DateStarted = DateTime.Now, DateEnded = DateTime.Today.AddDays(1)
                }
            };
            productionWorkOrder.EmployeeAssignments = employeeAssignment;
            return productionWorkOrder;
        }

        public ProductionWorkOrder GetTestProductionWorkOrderProgressForMaterial()
        {
            productionWorkOrder = new ProductionWorkOrder();
            productionWorkOrder.PlanningPlant = new PlanningPlant {Code = "D217"};
            productionWorkOrder.SAPNotificationNumber = 15625264;
            productionWorkOrder.SAPWorkOrder = "90450701";
            productionWorkOrder.FunctionalLocation = "NJMM-OT-MAINS";
            productionWorkOrder.RequestedBy = new Employee {FirstName = "", LastName = "", MiddleName = ""};
            productionWorkOrder.Priority = new ProductionWorkOrderPriority {Description = "Emergency", Id = 1};
            productionWorkOrder.ProductionWorkDescription = new ProductionWorkDescription {
                BreakdownIndicator = true, Description = "Added", Id = 1,
                PlantMaintenanceActivityType = new PlantMaintenanceActivityType {Description = "", Code = ""}
            };

            //productionWorkOrder.DateCompleted = DateTime.Now;
            productionWorkOrder.DateReceived = DateTime.Now;
            productionWorkOrder.MaterialsPlannedOn = DateTime.Now;
            productionWorkOrder.WBSElement = "";
            //EmployeeAssignment[] employeeAssignment = new EmployeeAssignment[2]
            //  {
            //        new EmployeeAssignment { AssignedOn = DateTime.Now, DateStarted = DateTime.Now},
            //        new EmployeeAssignment { AssignedOn = DateTime.Now, DateStarted = DateTime.Now}
            //  };
            //productionWorkOrder.EmployeeAssignments = employeeAssignment;

            var productionWorkOrderMaterialUsed = new HashSet<ProductionWorkOrderMaterialUsed> {
                new ProductionWorkOrderMaterialUsed {
                    Material = new Material {PartNumber = "1600040"}, Quantity = 3,
                    StockLocation = new StockLocation {Description = "", Id = 2600}
                },
                // new ProductionWorkOrderMaterialUsed {  Material = new Material { PartNumber = "ABC" }, Quantity = 1,StockLocation = new StockLocation { Description ="" } },
            };
            productionWorkOrder.ProductionWorkOrderMaterialUsed = productionWorkOrderMaterialUsed;
            return productionWorkOrder;
        }

        public ProductionWorkOrder GetTestScheduleWorkOrderProgress()
        {
            productionWorkOrder = new ProductionWorkOrder();
            //productionWorkOrder.PlanningPlant = new PlanningPlant { Code = "P212" };
            productionWorkOrder.SAPNotificationNumber = 15625264;
            productionWorkOrder.SAPWorkOrder = "90450701";

            var productionWorkOrderEquipment = new ProductionWorkOrderEquipment();
            productionWorkOrder.ProductionWorkOrderMeasurementPointValues =
                new List<ProductionWorkOrderMeasurementPointValue>();
            //productionWorkOrderEquipment = new ProductionWorkOrderEquipment();
            productionWorkOrderEquipment.SAPNotificationNumber = 15626921;
            var equipment = new Equipment();
            equipment.SAPEquipmentId = 20176025;
            productionWorkOrderEquipment.Equipment = new Equipment();
            productionWorkOrderEquipment.Equipment = equipment;
            productionWorkOrder.Equipments = new HashSet<ProductionWorkOrderEquipment>();
            productionWorkOrder.Equipments.Add(productionWorkOrderEquipment);
            productionWorkOrder.CompleteMeasurementPoints = true;
            var productionWorkOrderMeasurementPointValues = new ProductionWorkOrderMeasurementPointValue();
            productionWorkOrderMeasurementPointValues.Value = "120";
            productionWorkOrderMeasurementPointValues.MeasurementPointEquipmentType =
                new MeasurementPointEquipmentType();
            productionWorkOrderMeasurementPointValues.MeasurementPointEquipmentType.UnitOfMeasure =
                new UnitOfMeasure();
            productionWorkOrderMeasurementPointValues.MeasurementPointEquipmentType.UnitOfMeasure.SAPCode = "PST";
            productionWorkOrderMeasurementPointValues.MeasurementPointEquipmentType.Description = "Static Pressure";
            productionWorkOrderMeasurementPointValues.Equipment = new Equipment();
            productionWorkOrderMeasurementPointValues.Equipment = equipment;
            productionWorkOrderMeasurementPointValues.MeasurementDocId = 3753505;

            productionWorkOrder.ProductionWorkOrderMeasurementPointValues.Add(
                productionWorkOrderMeasurementPointValues);

            //productionWorkOrderMeasurementPointValues = new ProductionWorkOrderMeasurementPointValue();
            //productionWorkOrderMeasurementPointValues.Value = "100";
            //productionWorkOrderMeasurementPointValues.MeasurementPointEquipmentType = new MeasurementPointEquipmentType();
            //productionWorkOrderMeasurementPointValues.MeasurementPointEquipmentType.UnitOfMeasure = new UnitOfMeasure();
            //productionWorkOrderMeasurementPointValues.MeasurementPointEquipmentType.UnitOfMeasure.SAPCode = "PPM";            
            //productionWorkOrderMeasurementPointValues.MeasurementPointEquipmentType.Description = "Chlorine after Dechlor";
            //productionWorkOrderMeasurementPointValues.Equipment = new Equipment();
            //productionWorkOrderMeasurementPointValues.Equipment = equipment;
            //productionWorkOrderMeasurementPointValues.MeasurementDocId = null;

            //productionWorkOrder.ProductionWorkOrderMeasurementPointValues.Add(productionWorkOrderMeasurementPointValues);

            //                new ProductionWorkOrderMeasuringPoints { MeasuringPoint1 ="Static Pressure",CompleteNotification="",MeasuringReading1 ="120",NoReadingTakenFlag="",Unit1="PSI",MeasuringDocument = "",CancellationFlag = false}
            //                ,new ProductionWorkOrderMeasuringPoints { MeasuringPoint1 ="Chlorine after Dechlor",CompleteNotification="",MeasuringReading1 ="100",NoReadingTakenFlag="",Unit1="PPM",MeasuringDocument = "",CancellationFlag = false}

            //ProductionWorkOrderChildNotification[] productionWorkOrderChildNotification = new ProductionWorkOrderChildNotification[2]
            //{
            //        new ProductionWorkOrderChildNotification
            //        {
            //            NotificationType = "40",
            //            NotificationLongText ="" ,
            //            //SAPFunctionalLocation ="NJWO-WT-DALE.-CHM.",
            //            SAPFunctionalLocation ="NJOC-BH-HYDRT",
            //            SAPEquipmentNumber ="20176027",
            //            ReqStartDate ="20171130",
            //            Purpose  ="0002",
            //            CompleteFlag ="",
            //            NotificationNumber = "",
            //            //ProductionWorkOrderMeasuringPoints = new ProductionWorkOrderMeasuringPoints[1] {
            //            //    new ProductionWorkOrderMeasuringPoints { MeasuringPoint1 ="1",CompleteNotification="2",MeasuringReading1 ="3",NoReadingTakenFlag="4",Unit1="5"}
            //            //},
            //            //ProductionWorkOrderActions = new ProductionWorkOrderActions[1] {
            //            //    new ProductionWorkOrderActions { Code= "1",CodeGroup="2"}
            //            //},
            //            //   ProductionWorkOrderDependencies = new ProductionWorkOrderDependencies[1] {
            //            //    new ProductionWorkOrderDependencies { Code= "3",CodeGroup="4"}
            //            //},

            //            ProductionWorkOrderMeasuringPoints = new ProductionWorkOrderMeasuringPoints[2] {
            //                new ProductionWorkOrderMeasuringPoints { MeasuringPoint1 ="",CompleteNotification="",MeasuringReading1 ="",NoReadingTakenFlag="",Unit1="",MeasuringDocument = "",CancellationFlag = false},
            //                new ProductionWorkOrderMeasuringPoints { MeasuringPoint1 ="",CompleteNotification="",MeasuringReading1 ="",NoReadingTakenFlag="",Unit1="",MeasuringDocument = "",CancellationFlag = false}
            //            },
            //            ProductionWorkOrderActions = new ProductionWorkOrderActions[1] {
            //                new ProductionWorkOrderActions { Code= "",CodeGroup=""}
            //            },
            //               ProductionWorkOrderDependencies = new ProductionWorkOrderDependencies[1] {
            //                new ProductionWorkOrderDependencies { Code= "",CodeGroup=""}
            //            },

            //        },
            //        new ProductionWorkOrderChildNotification
            //        {
            //            NotificationType = "40",
            //            NotificationLongText ="" ,
            //            //SAPFunctionalLocation ="NJWO-WT-DALE.-CHM.",
            //            SAPFunctionalLocation ="NJOC-BH-HYDRT",
            //            SAPEquipmentNumber ="20176025",
            //            ReqStartDate ="",
            //            Purpose  ="001",
            //            CompleteFlag ="",
            //            NotificationNumber = "15626665",

            //            //ProductionWorkOrderMeasuringPoints = new ProductionWorkOrderMeasuringPoints[1] {
            //            //    new ProductionWorkOrderMeasuringPoints { MeasuringPoint1 ="1",CompleteNotification="2",MeasuringReading1 ="3",NoReadingTakenFlag="4",Unit1="5"}
            //            //},
            //            //ProductionWorkOrderActions = new ProductionWorkOrderActions[1] {
            //            //    new ProductionWorkOrderActions { Code= "1",CodeGroup="2"}
            //            //},
            //            //   ProductionWorkOrderDependencies = new ProductionWorkOrderDependencies[1] {
            //            //    new ProductionWorkOrderDependencies { Code= "3",CodeGroup="4"}
            //            //},

            //            ProductionWorkOrderMeasuringPoints = new ProductionWorkOrderMeasuringPoints[2] {
            //                new ProductionWorkOrderMeasuringPoints { MeasuringPoint1 ="Static Pressure",CompleteNotification="",MeasuringReading1 ="120",NoReadingTakenFlag="",Unit1="PSI",MeasuringDocument = "",CancellationFlag = false}
            //                ,new ProductionWorkOrderMeasuringPoints { MeasuringPoint1 ="Chlorine after Dechlor",CompleteNotification="",MeasuringReading1 ="100",NoReadingTakenFlag="",Unit1="PPM",MeasuringDocument = "",CancellationFlag = false}
            //            },
            //            ProductionWorkOrderActions = new ProductionWorkOrderActions[1] {
            //                new ProductionWorkOrderActions { Code= "",CodeGroup=""}
            //            },
            //               ProductionWorkOrderDependencies = new ProductionWorkOrderDependencies[1] {
            //                new ProductionWorkOrderDependencies { Code= "",CodeGroup=""}
            //            },

            //        }

            //};
            //productionWorkOrder.ProductionWorkOrderChildNotification = productionWorkOrderChildNotification;
            //productionWorkOrder.Equipments = productionWorkOrderEquipment;

            return productionWorkOrder;
        }

        #endregion

        #region Constructor

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestProductionWorkOrderConstructorSetsValues()
        {
            var productionWorkOrder = GetTestProductionWorkOrderProgress();

            var target = new SAPProgressUnscheduledWorkOrder(productionWorkOrder);

            Assert.AreEqual(target.OperatingCenter, productionWorkOrder.PlanningPlant.Code);
            Assert.AreEqual(target.SAPNotificationNo, productionWorkOrder.SAPNotificationNumber.ToString());
            Assert.AreEqual(target.SAPWorkOrderNo, productionWorkOrder.SAPWorkOrder);
            Assert.AreEqual(target.SAPFunctionalLoc, productionWorkOrder.FunctionalLocation);
            Assert.AreEqual(target.SAPEquipmentNo, productionWorkOrder.Equipment.SAPEquipmentId.ToString());
            Assert.AreEqual(target.RequestedBY, productionWorkOrder.RequestedBy.LastName);
            Assert.AreEqual(target.Priority, productionWorkOrder.Priority.Description);
            // Notes - method below
            Assert.AreEqual(target.WorkDescription, productionWorkOrder.ProductionWorkDescription.Description);
            // BreakdownIndicator - method below
            // CancelOrder - method below
            Assert.AreEqual(target.CancellationReason, productionWorkOrder.CancellationReason.SAPCode);
            // WBSElement - method below
            Assert.AreEqual(target.PMActType,
                productionWorkOrder.ProductionWorkDescription.PlantMaintenanceActivityType.Code);
            // MaterialsUsed - method below
            // EmployeesAssigned - method below
            // Capitalized - method below
            // Dependencies - method below
            // Actions - method below
            // ChildNotifications - method below
            // CompleteMeasurementPoints - method below
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestNotesAreSetFromCancellationReason()
        {
            Assert.Fail("Test not implemented.");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestCancelOrderIsSetProperly()
        {
            Assert.Fail("Test not implemented.");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestProductionWorkOrderConstructorSetsPurposeGroup()
        {
            Assert.Fail("Test not implemented.");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestProductionWorkOrderConstructorSetsWBSElement()
        {
            Assert.Fail("Test not implemented.");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestProductionWorkOrderConstructorSetsBreakDownIndicator()
        {
            Assert.Fail("Test not implemented.");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestProductionWorkOrderConstructorSetsNotes()
        {
            Assert.Fail("Test not implemented.");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestMaterialsUsedAreSet()
        {
            Assert.Fail("Test not implemented.");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestEmployeeAssignmentsAreSet()
        {
            Assert.Fail("Test not implemented.");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestCapitalizedIsSet()
        {
            Assert.Fail("Test not implemented.");
        }

        #endregion

        #region Measurement Points

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestChildNotificationsAndMeasurementPointsAreAddedProperly()
        {
            var equipment = new Equipment();
            var otherEquipment = new Equipment();
            var productionWorkOrder = new ProductionWorkOrder();

            var eqNotification1 = new ProductionWorkOrderEquipment {
                CompletedOn = DateTime.Now,
                Equipment = equipment,
                ProductionWorkOrder = productionWorkOrder,
                SAPNotificationNumber = 123456
            };
            var eqNotification2 = new ProductionWorkOrderEquipment {
                Equipment = otherEquipment,
                ProductionWorkOrder = productionWorkOrder,
                SAPNotificationNumber = 123457
            };
            var mpv1 = new ProductionWorkOrderMeasurementPointValue
                {ProductionWorkOrder = productionWorkOrder, Equipment = equipment, Value = "1.1"};
            var mpv2 = new ProductionWorkOrderMeasurementPointValue
                {ProductionWorkOrder = productionWorkOrder, Equipment = otherEquipment, Value = "2.1"};
            var mpv3 = new ProductionWorkOrderMeasurementPointValue
                {ProductionWorkOrder = productionWorkOrder, Equipment = otherEquipment, Value = "2.2"};

            productionWorkOrder.Equipments.Add(eqNotification1);
            productionWorkOrder.Equipments.Add(eqNotification2);
            productionWorkOrder.ProductionWorkOrderMeasurementPointValues.Add(mpv1);
            productionWorkOrder.ProductionWorkOrderMeasurementPointValues.Add(mpv2);
            productionWorkOrder.ProductionWorkOrderMeasurementPointValues.Add(mpv3);

            var target = new SAPProgressUnscheduledWorkOrder(productionWorkOrder);

            Assert.AreEqual(2, target.SapProductionWorkOrderChildNotification.Count);
            Assert.AreEqual(eqNotification1.SAPNotificationNumber.ToString(),
                target.SapProductionWorkOrderChildNotification.First().NotificationNumber);
            Assert.AreEqual(eqNotification2.SAPNotificationNumber.ToString(),
                target.SapProductionWorkOrderChildNotification.Last().NotificationNumber);
            Assert.AreEqual(1,
                target.SapProductionWorkOrderChildNotification.First().SapProductionWorkOrderMeasuringPoints.Count);
            Assert.AreEqual(2,
                target.SapProductionWorkOrderChildNotification.Last().SapProductionWorkOrderMeasuringPoints.Count);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestCompleteMeasurementPointsSetsCompleteFlag()
        {
            Assert.Fail("Test not implemented.");
        }

        #endregion
    }
}
