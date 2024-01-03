using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.SAP.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.SAP.service;

namespace SAP.DataTest.Model.Entities
{
    [TestClass()]
    public class SAPProcessWorkOrderTest
    {
        private WorkOrder workOrder;

        [TestInitialize]
        public void TestInitialize() { }

        [TestMethod()]
        public void TestConstructorSetsPropertiesForSAPProcessWorkOrderWithCrew()
        {
            workOrder = GetTestProcesWorkOrderWithCrew();
            var target = new SAPProgressWorkOrder(workOrder);

            Assert.AreEqual(target.OperatingCenter, workOrder.OperatingCenter);
            Assert.AreEqual(target.SAPNotificationNo, workOrder.SAPNotificationNumber?.ToString());
            Assert.AreEqual(target.SAPWorkOrderNo, workOrder.SAPWorkOrderNumber?.ToString());
            Assert.AreEqual(target.HouseNo, workOrder.StreetNumber);
            Assert.AreEqual(target.Street, workOrder.Street?.FullStName);
            Assert.AreEqual(target.CrossStreet, workOrder.NearestCrossStreet?.FullStName);
            Assert.AreEqual(target.Town, workOrder.Town?.ToString());
            Assert.AreEqual(target.TownSection, workOrder.TownSection);
            Assert.AreEqual(target.Zipcode, workOrder.Town?.Zip);
            Assert.AreEqual(target.State, workOrder.Town?.State?.Abbreviation);
            Assert.AreEqual(target.Country, "US");
            Assert.AreEqual(target.Latitude, workOrder.Latitude?.ToString());
            Assert.AreEqual(target.Longitude, workOrder.Longitude?.ToString());
            Assert.AreEqual(target.AssetType, workOrder.AssetType?.Description);
            Assert.AreEqual(target.SAPFunctionalLoc, workOrder.Valve?.FunctionalLocation?.Description);
            Assert.AreEqual(target.SAPEquipmentNo, workOrder.Valve?.SAPEquipmentNumber.PadLeft(18, '0'));
            Assert.AreEqual(target.RequestedBY, workOrder.RequestedBy?.Description);
            Assert.AreEqual(target.Customer, workOrder.CustomerName);
            Assert.AreEqual(target.Priority, workOrder.Priority?.Description);
            Assert.AreEqual(target.Premise, workOrder.PremiseNumber);
            Assert.AreEqual(target.Notes, workOrder.Notes);
            Assert.AreEqual(target.AccountCharged, workOrder.AccountCharged);
            Assert.AreEqual(target.WorkDescription, workOrder.WorkDescription?.Description);
            Assert.AreEqual(target.PMActType, workOrder.WorkDescription?.PlantMaintenanceActivityType?.Code);

            ProgressWorkOrder progressWorkOrder = target.ProcessWorkOrderRequest();

            if (target.sapCrewAssignments != null)
            {
                List<SAPCrewAssignment> lstSAPCrewAssignment = target.sapCrewAssignments.ToList();
                Assert.AreEqual(progressWorkOrder.WorkOrder.CrewAssignment[0].CrewAssign,
                    lstSAPCrewAssignment[0].CrewAssign);
                Assert.AreEqual(progressWorkOrder.WorkOrder.CrewAssignment[0].DateCompleted,
                    lstSAPCrewAssignment[0].DateCompleted);
                Assert.AreEqual(progressWorkOrder.WorkOrder.CrewAssignment[0].DateStart,
                    lstSAPCrewAssignment[0].DateStart);
                Assert.AreEqual(progressWorkOrder.WorkOrder.CrewAssignment[0].DateEnd, lstSAPCrewAssignment[0].DateEnd);
            }
        }

        [TestMethod()]
        public void TestConstructorSetsPropertiesForSAPProcessWorkOrderWithMaterial()
        {
            workOrder = GetTestProcesWorkOrderWithMaterial();
            var target = new SAPProgressWorkOrder(workOrder);
            Assert.AreEqual(target.OperatingCenter, workOrder.OperatingCenter?.DistributionPlanningPlant?.Code);
            Assert.AreEqual(target.SAPNotificationNo, workOrder.SAPNotificationNumber?.ToString());
            Assert.AreEqual(target.SAPWorkOrderNo, workOrder.SAPWorkOrderNumber?.ToString());
            Assert.AreEqual(target.HouseNo, workOrder.StreetNumber);
            Assert.AreEqual(target.Street, workOrder.Street?.FullStName);
            Assert.AreEqual(target.CrossStreet, workOrder.NearestCrossStreet?.FullStName);
            Assert.AreEqual(target.Town, workOrder.Town?.ToString());
            Assert.AreEqual(target.TownSection, workOrder.TownSection);
            Assert.AreEqual(target.Zipcode, workOrder.Town?.Zip);
            Assert.AreEqual(target.State, workOrder.Town?.State?.Abbreviation);
            Assert.AreEqual(target.Country, "US");
            Assert.AreEqual(target.Latitude, workOrder.Latitude.ToString());
            Assert.AreEqual(target.Longitude, workOrder.Longitude.ToString());
            Assert.AreEqual(target.AssetType, workOrder.AssetType?.Description);
            Assert.AreEqual(target.SAPFunctionalLoc, workOrder.Valve?.FunctionalLocation?.Description);
            Assert.AreEqual(target.SAPEquipmentNo, workOrder.Valve?.SAPEquipmentNumber.PadLeft(18, '0'));
            Assert.AreEqual(target.RequestedBY, workOrder.RequestedBy?.Description);
            Assert.AreEqual(target.Customer, workOrder.CustomerName);
            Assert.AreEqual(target.Priority, workOrder.Priority?.Description);
            Assert.AreEqual(target.Premise, workOrder.PremiseNumber);
            Assert.AreEqual(target.Notes, workOrder.Notes);
            Assert.AreEqual(target.AccountCharged, workOrder.AccountCharged);
            Assert.AreEqual(target.WorkDescription, workOrder.WorkDescription?.Description);
            Assert.AreEqual(target.PMActType, workOrder.WorkDescription?.PlantMaintenanceActivityType?.Code);

            List<SAPProductionWorkOrderMaterialUsed> lstSAPMaterialUsed = target.sapMaterialsUsed?.ToList();
            List<MaterialUsed> lstMaterialUsed = workOrder.MaterialsUsed?.ToList();

            if (lstSAPMaterialUsed != null && lstSAPMaterialUsed.Count > 0)
            {
                Assert.AreEqual(lstMaterialUsed[0].Material?.Description, lstSAPMaterialUsed[0].Description);
                Assert.AreEqual(lstMaterialUsed[0].Quantity.ToString(), lstSAPMaterialUsed[0].Quantity);
                Assert.AreEqual(lstMaterialUsed[0].StockLocation?.SAPStockLocation,
                    lstSAPMaterialUsed[0].StcokLocation);
                Assert.AreEqual(lstMaterialUsed[0].StockLocation?.OperatingCenter?.OperatingCenterCode,
                    lstSAPMaterialUsed[0].PlanningPlan);
                Assert.AreEqual(lstMaterialUsed[0].Material?.PartNumber, lstSAPMaterialUsed[0].PartNumber);
            }

            if (lstMaterialUsed != null && lstMaterialUsed.Count > 0)
            {
                Assert.AreEqual(lstMaterialUsed[1].Material?.Description, lstSAPMaterialUsed[1].Description);
                Assert.AreEqual(lstMaterialUsed[1].Quantity.ToString(), lstSAPMaterialUsed[1].Quantity);
                Assert.AreEqual(lstMaterialUsed[1].StockLocation?.SAPStockLocation,
                    lstSAPMaterialUsed[1].StcokLocation);
                Assert.AreEqual(lstMaterialUsed[1].StockLocation?.OperatingCenter?.OperatingCenterCode,
                    lstSAPMaterialUsed[1].PlanningPlan);
                Assert.AreEqual(lstMaterialUsed[1].Material?.PartNumber, lstSAPMaterialUsed[1].PartNumber);
            }
        }

        [TestMethod()]
        public void TestConstructorSetsNullForSAPProcessWorkOrderWithMaterial()
        {
            workOrder = new WorkOrder();
            var target = new SAPProgressWorkOrder(workOrder);
            Assert.AreEqual(target.OperatingCenter, workOrder.OperatingCenter);
            Assert.AreEqual(target.SAPNotificationNo, workOrder.SAPNotificationNumber?.ToString());
            Assert.AreEqual(target.SAPWorkOrderNo, workOrder.SAPWorkOrderNumber?.ToString());
            Assert.AreEqual(target.HouseNo, workOrder.StreetNumber);
            Assert.AreEqual(target.Street, workOrder.Street?.FullStName);
            Assert.AreEqual(target.CrossStreet, workOrder.NearestCrossStreet?.FullStName);
            Assert.AreEqual(target.Town, workOrder.Town?.ToString());
            Assert.AreEqual(target.TownSection, workOrder.TownSection);
            Assert.AreEqual(target.Zipcode, workOrder.Town?.Zip);
            Assert.AreEqual(target.State, workOrder.Town?.State?.Abbreviation);
            Assert.AreEqual(target.Country, "US");
            Assert.AreEqual(target.Latitude, workOrder.Latitude?.ToString());
            Assert.AreEqual(target.Longitude, workOrder.Longitude?.ToString());
            Assert.AreEqual(target.AssetType, workOrder.AssetType?.Description);
            Assert.AreEqual(target.SAPFunctionalLoc, workOrder.Valve?.FunctionalLocation?.Description);
            Assert.AreEqual(target.SAPEquipmentNo, workOrder.Valve?.SAPEquipmentNumber.PadLeft(18, '0'));
            Assert.AreEqual(target.RequestedBY, workOrder.RequestedBy?.Description);
            Assert.AreEqual(target.Customer, workOrder.CustomerName);
            Assert.AreEqual(target.Priority, workOrder.Priority?.Description);
            Assert.AreEqual(target.Premise, workOrder.PremiseNumber);
            Assert.AreEqual(target.Notes, workOrder.Notes);
            Assert.AreEqual(target.AccountCharged, workOrder.AccountCharged);
            Assert.AreEqual(target.WorkDescription,
                workOrder.WorkDescription?.PlantMaintenanceActivityType?.Description);
            Assert.AreEqual(target.PMActType, workOrder.WorkDescription?.PlantMaintenanceActivityType?.Code);

            List<SAPProductionWorkOrderMaterialUsed> lstSAPMaterialUsed = target.sapMaterialsUsed?.ToList();
            List<MaterialUsed> lstMaterialUsed = workOrder.MaterialsUsed?.ToList();

            if (lstSAPMaterialUsed != null && lstSAPMaterialUsed.Count > 0)
            {
                Assert.AreEqual(lstMaterialUsed[0].Material?.Description, lstSAPMaterialUsed[0].Description);
                Assert.AreEqual(lstMaterialUsed[0].Quantity.ToString(), lstSAPMaterialUsed[0].Quantity);
                Assert.AreEqual(lstMaterialUsed[0].StockLocation?.SAPStockLocation,
                    lstSAPMaterialUsed[0].StcokLocation);
                Assert.AreEqual(lstMaterialUsed[0].Material?.PartNumber, lstSAPMaterialUsed[0].PartNumber);
            }

            if (lstMaterialUsed != null && lstMaterialUsed.Count > 0)
            {
                Assert.AreEqual(lstMaterialUsed[1].Material?.Description, lstSAPMaterialUsed[1].Description);
                Assert.AreEqual(lstMaterialUsed[1].Quantity.ToString(), lstSAPMaterialUsed[1].Quantity);
                Assert.AreEqual(lstMaterialUsed[1].StockLocation?.SAPStockLocation,
                    lstSAPMaterialUsed[1].StcokLocation);
                Assert.AreEqual(lstMaterialUsed[1].Material?.PartNumber, lstSAPMaterialUsed[1].PartNumber);
            }
        }

        [TestMethod()]
        public void TestProcessWorkOrderRequestForSAPProcessWorkOrderWithMaterial()
        {
            workOrder = GetTestProcesWorkOrderWithMaterial();
            var target = new SAPProgressWorkOrder(workOrder);
            ProgressWorkOrder progressWorkOrder = target.ProcessWorkOrderRequest();

            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].OperatingCenter, target.OperatingCenter);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].SAPNotificationNo, target.SAPNotificationNo);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].SAPWorkOrderNo, target.SAPWorkOrderNo);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].HouseNo, target.HouseNo);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Street, target.Street);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].CrossStreet, target.CrossStreet);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Town, target.Town);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].TownSection, target.TownSection);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Zipcode, target.Zipcode);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].State, target.State);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Country, target.Country);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Latitude, target.Latitude);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Longitude, target.Longitude);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].AssetType, target.AssetType);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].SAPFunctionalLoc, target.SAPFunctionalLoc);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].SAPEquipmentNo, target.SAPEquipmentNo);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].RequestedBY, target.RequestedBY);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Customer, target.Customer);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].PurposeCode, target.MapCallPurpose);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Premise, target.Premise);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Notes, target.Notes);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].AccountCharged, target.AccountCharged);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].CancelOrder, target.CancelOrder);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].CancellationReason, target.CancellationReason);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].WorkDescription, target.WorkDescription);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].PMActType, target.PMActType);

            List<SAPProductionWorkOrderMaterialUsed> lstSAPMaterialUsed = target.sapMaterialsUsed?.ToList();
            if (lstSAPMaterialUsed != null && lstSAPMaterialUsed.Count > 0)
            {
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[0].PartNumber, lstSAPMaterialUsed[0].PartNumber);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[0].Description,
                    lstSAPMaterialUsed[0].Description);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[0].Quantity, lstSAPMaterialUsed[0].Quantity);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[0].StcokLocation,
                    lstSAPMaterialUsed[0].StcokLocation);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[0].OperatingCenterCode,
                    lstSAPMaterialUsed[0].PlanningPlan);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[0].ItemCategory,
                    lstSAPMaterialUsed[0].ItemCategory);

                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[1].PartNumber, lstSAPMaterialUsed[1].PartNumber);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[1].Description,
                    lstSAPMaterialUsed[1].Description);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[1].Quantity, lstSAPMaterialUsed[1].Quantity);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[1].StcokLocation,
                    lstSAPMaterialUsed[1].StcokLocation);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[1].OperatingCenterCode,
                    lstSAPMaterialUsed[1].PlanningPlan);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[1].ItemCategory,
                    lstSAPMaterialUsed[1].ItemCategory);
            }
        }

        [TestMethod()]
        public void TestProcessWorkOrderRequestForSAPProcessWorkOrderWithMaterialNCrew()
        {
            workOrder = GetTestProcesWorkOrderWithMaterialNCrew();
            var target = new SAPProgressWorkOrder(workOrder);
            ProgressWorkOrder progressWorkOrder = target.ProcessWorkOrderRequest();

            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].OperatingCenter, target.OperatingCenter);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].SAPNotificationNo, target.SAPNotificationNo);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].SAPWorkOrderNo, target.SAPWorkOrderNo);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].HouseNo, target.HouseNo);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Street, target.Street);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].CrossStreet, target.CrossStreet);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Town, target.Town);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].TownSection, target.TownSection);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Zipcode, target.Zipcode);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].State, target.State);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Country, target.Country);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Latitude, target.Latitude);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Longitude, target.Longitude);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].AssetType, target.AssetType);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].SAPFunctionalLoc, target.SAPFunctionalLoc);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].SAPEquipmentNo, target.SAPEquipmentNo);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].RequestedBY, target.RequestedBY);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Customer, target.Customer);
            //Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].PurposeGroup, target.PurposeGroup);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].PurposeCode, target.MapCallPurpose);
            // Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Priority, target.Priority);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Premise, target.Premise);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].Notes, target.Notes);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].AccountCharged, target.AccountCharged);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].CancelOrder, target.CancelOrder);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].CancellationReason, target.CancellationReason);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].WorkDescription, target.WorkDescription);
            Assert.AreEqual(progressWorkOrder.WorkOrder.ChangeOrder[0].PMActType, target.PMActType);

            List<SAPProductionWorkOrderMaterialUsed> lstSAPMaterialUsed = target.sapMaterialsUsed?.ToList();
            if (lstSAPMaterialUsed != null && lstSAPMaterialUsed.Count > 0)
            {
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[0].PartNumber, lstSAPMaterialUsed[0].PartNumber);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[0].Description,
                    lstSAPMaterialUsed[0].Description);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[0].Quantity, lstSAPMaterialUsed[0].Quantity);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[0].StcokLocation,
                    lstSAPMaterialUsed[0].StcokLocation);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[0].OperatingCenterCode,
                    lstSAPMaterialUsed[0].PlanningPlan);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[0].ItemCategory,
                    lstSAPMaterialUsed[0].ItemCategory);

                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[1].PartNumber, lstSAPMaterialUsed[1].PartNumber);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[1].Description,
                    lstSAPMaterialUsed[1].Description);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[1].Quantity, lstSAPMaterialUsed[1].Quantity);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[1].StcokLocation,
                    lstSAPMaterialUsed[1].StcokLocation);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[1].OperatingCenterCode,
                    lstSAPMaterialUsed[1].PlanningPlan);
                Assert.AreEqual(progressWorkOrder.WorkOrder.Materials[1].ItemCategory,
                    lstSAPMaterialUsed[1].ItemCategory);
            }

            List<SAPCrewAssignment> lstSAPCrewAssignment = target.sapCrewAssignments.ToList();
            if (lstSAPCrewAssignment.Count > 0)
            {
                Assert.AreEqual(progressWorkOrder.WorkOrder.CrewAssignment[0].CrewAssign,
                    lstSAPCrewAssignment[0].CrewAssign);
                Assert.AreEqual(progressWorkOrder.WorkOrder.CrewAssignment[0].DateCompleted,
                    lstSAPCrewAssignment[0].DateCompleted);
                Assert.AreEqual(progressWorkOrder.WorkOrder.CrewAssignment[0].DateEnd, lstSAPCrewAssignment[0].DateEnd);
                Assert.AreEqual(progressWorkOrder.WorkOrder.CrewAssignment[0].DateStart,
                    lstSAPCrewAssignment[0].DateStart);
            }
        }

        public WorkOrder GetTestProcesWorkOrderWithMaterial(SAPWorkOrder sapWorkOrder = null)
        {
            workOrder = new WorkOrder();
            workOrder.WorkDescription = new WorkDescription {
                Description = "INSTALL METER",
                PlantMaintenanceActivityType = new PlantMaintenanceActivityType {Code = "ABC", Description = "Abc"}
            };
            workOrder.SAPNotificationNumber =
                sapWorkOrder != null ? Convert.ToInt64(sapWorkOrder.NotificationNumber) : 14912224;
            workOrder.SAPWorkOrderNumber = sapWorkOrder != null ? Convert.ToInt64(sapWorkOrder.OrderNumber) : 90365202;
            workOrder.AssetType = new AssetType {Description = "VALVE"};
            workOrder.Valve = new Valve {
                FunctionalLocation = new FunctionalLocation {Description = "NJOC-TR-VALVE-1002"},
                SAPEquipmentId = 10000501
            };
            workOrder.RequestedBy = new WorkOrderRequester {Description = "xyz"};
            workOrder.Purpose = new WorkOrderPurpose {Description = "Compliance"};
            workOrder.Priority = new WorkOrderPriority {Description = "Emergency"};
            workOrder.PremiseNumber = "122";
            workOrder.Notes = "test";

            PlanningPlant[] plant = new PlanningPlant[1] {new PlanningPlant {Code = "D203"}};
            var opCntr = new OperatingCenter {PlanningPlants = plant};
            workOrder.OperatingCenter = opCntr;

            workOrder.StreetNumber = "280";
            workOrder.Street = new Street {FullStName = "1ST ST"};
            workOrder.NearestCrossStreet = new Street {FullStName = "12TH ST"};
            workOrder.Town = new Town {ShortName = "", Zip = "08043", State = new State {Abbreviation = "NJ"}};
            workOrder.Latitude = 0;
            workOrder.Longitude = 0;
            workOrder.DateCompleted = DateTime.Now;
            workOrder.MaterialPlanningCompletedOn = DateTime.Now;

            //, OperatingCenter = new OperatingCenter {DistributionPlanningPlant = new PlanningPlant {Code = "D201" } }
            MaterialUsed[] MaterialsUsed = new MaterialUsed[3] {
                new MaterialUsed {
                    Material = new Material {PartNumber = "1411970", Description = "YBR,NL,2WY,CC,3/4X1"}, Quantity = 1,
                    StockLocation = new StockLocation {SAPStockLocation = "1700"}
                }, //, OperatingCenter = new OperatingCenter { OperatingCenterCode =  "D103"  }
                new MaterialUsed {
                    Material = new Material {PartNumber = "1407986", Description = "VLV,GATE,OL,MJ,NR,BRNZ,NUT,8"},
                    Quantity = 2, StockLocation = new StockLocation {SAPStockLocation = "1700"}
                }, //, OperatingCenter = new OperatingCenter { OperatingCenterCode = "D103" 
                new MaterialUsed {
                    Material = new Material {PartNumber = "", Description = "Bag o cement"}, Quantity = 2,
                    StockLocation = new StockLocation {SAPStockLocation = "1700"}
                } //, OperatingCenter = new OperatingCenter { OperatingCenterCode = "D103" 
            };
            workOrder.MaterialsUsed = MaterialsUsed;

            return workOrder;
        }

        public WorkOrder GetTestProcesWorkOrderWithCrew(SAPWorkOrder sapWorkOrder = null)
        {
            workOrder = new WorkOrder();
            workOrder.WorkDescription = new WorkDescription {
                Description = "INSTALL METER",
                PlantMaintenanceActivityType = new PlantMaintenanceActivityType {Code = "ABC", Description = "Abc"}
            };
            workOrder.SAPNotificationNumber =
                sapWorkOrder != null ? Convert.ToInt64(sapWorkOrder.NotificationNumber) : 14912224;
            workOrder.SAPWorkOrderNumber = sapWorkOrder != null ? Convert.ToInt64(sapWorkOrder.OrderNumber) : 90365202;
            workOrder.RequestedBy = new WorkOrderRequester {Description = "xyz"};
            workOrder.Purpose = new WorkOrderPurpose {Description = "Compliance"};
            workOrder.Priority = new WorkOrderPriority {Description = "Emergency"};
            workOrder.Notes = "test";
            workOrder.MaterialPlanningCompletedOn = DateTime.Now;
            workOrder.DateCompleted = DateTime.Now;
            workOrder.DateRejected = DateTime.Now;
            workOrder.ApprovedOn = DateTime.Now;
            CrewAssignment[] CrewAssignment = new CrewAssignment[2] {
                new CrewAssignment {
                    Crew = new Crew {Description = "Crew 1"},
                    AssignedOn = Convert.ToDateTime("1/13/2017 11:52:02 AM"),
                    DateStarted = Convert.ToDateTime("1/13/2017 11:52:02 AM"),
                    DateEnded = Convert.ToDateTime("1/13/2017 11:52:02 AM")
                },
                new CrewAssignment {
                    Crew = new Crew {Description = "Crew 2"},
                    AssignedOn = Convert.ToDateTime("10/13/2017 10:52:02 AM"),
                }
            };

            //DateStarted = Convert.ToDateTime("10/13/2017 11:52:02 AM"),

            workOrder.CrewAssignments = CrewAssignment;

            return workOrder;
        }

        public WorkOrder GetTestProcesWorkOrderWithMaterialNCrew(SAPWorkOrder sapWorkOrder = null)
        {
            workOrder = new WorkOrder();
            workOrder.WorkDescription = new WorkDescription {
                Description = "INSTALL METER",
                PlantMaintenanceActivityType = new PlantMaintenanceActivityType {Code = "ABC", Description = "Abc"}
            };
            workOrder.SAPNotificationNumber =
                sapWorkOrder != null ? Convert.ToInt64(sapWorkOrder.NotificationNumber) : 14954115;
            workOrder.SAPWorkOrderNumber = sapWorkOrder != null ? Convert.ToInt64(sapWorkOrder.OrderNumber) : 90365843;
            workOrder.AssetType = new AssetType {Description = "VALVE"};
            workOrder.RequestedBy = new WorkOrderRequester {Description = "xyz"};
            workOrder.Purpose = new WorkOrderPurpose {Description = "Construction Project"};
            workOrder.Priority = new WorkOrderPriority {Description = "Emergency"};
            workOrder.Notes = "test";
            workOrder.DateCompleted = DateTime.Now;
            //phase 2 - changes for technical master interface
            workOrder.SAPEquipmentNumber = null;
            workOrder.DeviceLocation = null;
            workOrder.Installation = null;

            CrewAssignment[] CrewAssignment = new CrewAssignment[2] {
                new CrewAssignment {AssignedOn = Convert.ToDateTime("1/27/2017 3:30 PM")},
                new CrewAssignment {
                    AssignedOn = Convert.ToDateTime("1/30/2017 10:27 AM"),
                    DateStarted = Convert.ToDateTime("1/30/2017 10:28:00 AM")
                }, //, DateEnded = Convert.ToDateTime(""),EmployeesOnJob = 1
            };
            workOrder.CrewAssignments = CrewAssignment;

            MaterialUsed[] MaterialsUsed = new MaterialUsed[2] {
                new MaterialUsed {
                    Material = new Material {PartNumber = "1405415", Description = "MTR BX,RISER,20X4"}, Quantity = 1,
                    StockLocation = new StockLocation {SAPStockLocation = "STKR"}
                },
                new MaterialUsed {
                    Material = new Material {PartNumber = "1405414", Description = "MTR BX,RISER,20X2"}, Quantity = 2,
                    StockLocation = new StockLocation {SAPStockLocation = "STKR"}
                }
            };
            workOrder.MaterialsUsed = MaterialsUsed;

            return workOrder;
        }

        public WorkOrder GetTestProcesWorkOrderCancel(SAPWorkOrder sapWorkOrder = null)
        {
            workOrder = new WorkOrder();
            workOrder.SAPNotificationNumber =
                sapWorkOrder != null ? Convert.ToInt64(sapWorkOrder.NotificationNumber) : 14912224;
            workOrder.SAPWorkOrderNumber = sapWorkOrder != null ? Convert.ToInt64(sapWorkOrder.OrderNumber) : 90365202;
            workOrder.CancelledAt = DateTime.Now;
            workOrder.WorkOrderCancellationReason = new WorkOrderCancellationReason {Status = "CERR"};

            return workOrder;
        }

        public WorkOrder GetTestWorkOrderForHydrant()
        {
            workOrder = new WorkOrder();
            workOrder.Id = 263575;
            workOrder.AssetType = new AssetType {Description = "HYDRANT"};
            workOrder.Purpose = new WorkOrderPurpose {Description = "Compliance"};
            workOrder.Priority = new WorkOrderPriority {Description = "Emergency"};
            workOrder.Hydrant = new Hydrant {
                FunctionalLocation = new FunctionalLocation {Description = "NJOC-BH-HYDRT"}, SAPEquipmentId = 20004029
            };
            workOrder.WorkDescription = new WorkDescription {
                Description = "Apurva",
                PlantMaintenanceActivityType = new PlantMaintenanceActivityType
                    {Code = "MLS", Description = "HYDRANT FROZEN"}
            };
            workOrder.Notes = "test";
            workOrder.StreetNumber = "280";
            workOrder.Street = new Street {FullStName = "1ST ST"};
            workOrder.NearestCrossStreet = new Street {FullStName = "12TH ST"};
            workOrder.Town = new Town {ShortName = "", Zip = "08043", State = new State {Abbreviation = "NJ"}};
            workOrder.Latitude = 123;
            workOrder.Longitude = 345;
            workOrder.DateReceived = Convert.ToDateTime("01/01/2009 11:52:02 AM");
            workOrder.DateCompleted = DateTime.Now;

            return workOrder;
        }
    }
}
