using System;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.SAPTest.Model.Entities
{
    [TestClass]
    public class SAPWorkOrderTest
    {
        #region Private Members

        private WorkOrder _workOrder;

        #endregion

        #region Public Methods

        public WorkOrder GetTestWorkOrderForHydrant()
        {
            var workOrder = new WorkOrder {
                //SAPNotificationNumber = 10002160,
                Id = 263575,
                AssetType = new AssetType {Description = "HYDRANT"},
                Purpose = new WorkOrderPurpose {Description = "Compliance", SapCode = "I04"},
                Priority = new WorkOrderPriority {Description = "Emergency"},
                Hydrant = new Hydrant {
                    FunctionalLocation = new FunctionalLocation {Description = "NJOC-BH-HYDRT"}, SAPEquipmentId = 20004029
                },
                WorkDescription = new WorkDescription {
                    Description = "Apurva",
                    PlantMaintenanceActivityType = new PlantMaintenanceActivityType
                        {Code = "MLS", Description = "HYDRANT FROZEN"}
                },
                Notes = "test",
                StreetNumber = "280",
                Street = new Street {FullStName = "1ST ST"},
                NearestCrossStreet = new Street {FullStName = "12TH ST"},
                Town = new Town {ShortName = "", Zip = "08043", State = new State {Abbreviation = "NJ"}},
                Latitude = 123,
                Longitude = 345,
                DateReceived = Convert.ToDateTime("01/01/2009 11:52:02 AM"),
                DateCompleted = DateTime.Now,
                PlannedCompletionDate = DateTime.Now.AddDays(30)
            };

            return workOrder;
        }

        public WorkOrder GetTestWorkOrderForValve()
        {
            var workOrder = new WorkOrder {
                WorkDescription = new WorkDescription {
                    Description = "Work order desc",
                    PlantMaintenanceActivityType = new PlantMaintenanceActivityType { Code = "MLS" }
                },
                //SAPNotificationNumber = 10002192,
                AssetType = new AssetType { Description = "VALVE" },
                Valve = new Valve {
                    FunctionalLocation = new FunctionalLocation { Description = "NJLK-LK-VALVE" },
                    SAPEquipmentId = 10011957
                },
                Id = 263575,
                Purpose = new WorkOrderPurpose { Description = "Compliance" },
                Priority = new WorkOrderPriority { Description = "Emergency" },
                Notes = "test",
                StreetNumber = "280",
                Street = new Street { FullStName = "1ST ST" },
                NearestCrossStreet = new Street { FullStName = "12TH ST" },
                Town = new Town { ShortName = "", Zip = "08043", State = new State { Abbreviation = "NJ" } },
                Latitude = 0,
                Longitude = 0,
                DateReceived = DateTime.Now,
                DateCompleted = DateTime.Now,
            };
            var operatingCenter = new OperatingCenter();
            operatingCenter.PlanningPlants.Add(new PlanningPlant {Code = "D205"});
            workOrder.OperatingCenter = operatingCenter;
            return workOrder;
        }

        public WorkOrder GetTestWorkOrderForSewerOpening()
        {
            return new WorkOrder {
                //SAPNotificationNumber = 10002166,
                AssetType = new AssetType {Description = "SEWER OPENING"},
                SewerOpening = new SewerOpening {
                    SAPEquipmentId = 5116360,
                    FunctionalLocation = new FunctionalLocation {Description = "NJLKW-LK-MAINS-MH"}
                },
                WorkDescription = new WorkDescription {
                    Description = "SEWER OPENING REPAIR",
                    PlantMaintenanceActivityType = new PlantMaintenanceActivityType {Code = "MLS"}
                },
                Purpose = new WorkOrderPurpose {Description = "Compliance"},
                Priority = new WorkOrderPriority {Description = "Emergency"},
                Notes = "test",
                StreetNumber = "280",
                Street = new Street {FullStName = "ALBION ST"},
                NearestCrossStreet = new Street {FullStName = "ALBION ST"},
                RequestedBy = new WorkOrderRequester {Description = "Call Center"},
                Town = new Town {ShortName = "LAKEWOED", Zip = "08043", State = new State {Abbreviation = "NJ"}},
                Latitude = 0,
                Longitude = 0,
                DateReceived = DateTime.Now,
                DateCompleted = DateTime.Now,
                OperatingCenter = new OperatingCenter {PlanningPlants = new[] {new PlanningPlant {Code = "S205"}}} 
            };
        }

        public WorkOrder GetTestWorkOrderForMain()
        {
            var town = new Town {
                ShortName = "LAKEWOOD",
                Zip = "08043",
                State = new State {Abbreviation = "NJ"}
            };
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                MainSAPFunctionalLocation = new FunctionalLocation {Description = "NJLK-LK-MAINS"},
                MainSAPEquipmentId = 30000502,
                Town = town
            });
            return new WorkOrder {
                //SAPNotificationNumber = 10002250,
                AssetType = new AssetType { Description = "MAIN" },
                Town = town,
                WorkDescription = new WorkDescription {
                    Description = "MAIN INVESTIGATION",
                    PlantMaintenanceActivityType = new PlantMaintenanceActivityType { Code = "MLS" }
                },
                Purpose = new WorkOrderPurpose { Description = "Compliance" },
                Priority = new WorkOrderPriority { Description = "Emergency" },
                Notes = "test",
                StreetNumber = "280",
                Street = new Street { FullStName = "LAKEWOOD RD" },
                NearestCrossStreet = new Street { FullStName = "LAKEWOOD RD" },
                Latitude = 0,
                Longitude = 0,
                DateReceived = DateTime.Now,
                DateCompleted = DateTime.Now,
                OperatingCenter = new OperatingCenter { PlanningPlants = new[] { new PlanningPlant { Code = "D205" } } }
            };
        }

        public WorkOrder GetTestWorkOrderForSewerMain()
        {
            var town = new Town {
                ShortName = "LAKEWOOD",
                Zip = "08043",
                State = new State { Abbreviation = "NJ" }
            };
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = town,
                SewerMainSAPFunctionalLocation = new FunctionalLocation {Description = "NJLKW-LK-MAINS"},
                SewerMainSAPEquipmentId = 5006346
            });
            return new WorkOrder {
                AssetType = new AssetType {Description = "SEWER MAIN"},
                Town = town,
                WorkDescription = new WorkDescription {
                    Description = "SEWER MAIN CLEANING",
                    PlantMaintenanceActivityType = new PlantMaintenanceActivityType {Code = "MLS"}
                },
                Purpose = new WorkOrderPurpose {Description = "Compliance"},
                Priority = new WorkOrderPriority {Description = "Emergency"},
                Notes = "test",
                StreetNumber = "280",
                Street = new Street {FullStName = "LAKEWOOD AVE"},
                NearestCrossStreet = new Street {FullStName = "LAKEWOOD AVE"},
                Latitude = 0,
                Longitude = 0,
                DateReceived = DateTime.Now,
                DateCompleted = DateTime.Now,
                OperatingCenter = new OperatingCenter {PlanningPlants = new[] {new PlanningPlant {Code = "S205"}}} 
            };
        }

        public WorkOrder GetTestWorkOrderForService()
        {
            return new WorkOrder {
                //SAPNotificationNumber = 10002167,
                AssetType = new AssetType {Description = "SERVICE"},
                PremiseNumber = "9180458651",
                WorkDescription = new WorkDescription {
                    Description = "INSTALL METER",
                    PlantMaintenanceActivityType = new PlantMaintenanceActivityType {Code = "MLS"}
                },
                Purpose = new WorkOrderPurpose {Description = "Compliance"},
                Notes = "test",
                StreetNumber = "704",
                Street = new Street {FullStName = "GREENS AVE"},
                NearestCrossStreet = new Street {FullStName = "CEDAR AVE"},
                Town = new Town
                    {ShortName = "LONG BRANCH", Zip = "07740", State = new State {Abbreviation = "NJ"}},
                Latitude = 0,
                Longitude = 0,
                DateReceived = Convert.ToDateTime("01/27/2017"),
                Priority = new WorkOrderPriority {Description = "Emergency"},
                //phase 2 changes
                SAPEquipmentNumber = 52337392,
                DeviceLocation = 6002074816,
                Installation = 7003409809,
                OperatingCenter = new OperatingCenter {PlanningPlants = new[] {new PlanningPlant {Code = "D203"}}}
            };
        }

        public WorkOrder GetTestWorkOrderForSewerLateral()
        {
            return new WorkOrder {
                AssetType = new AssetType { Description = "SEWER LATERAL" },
                PremiseNumber = "9180458651",
                WorkDescription = new WorkDescription { Description = "asd" },
                Id = 263575,
                Purpose = new WorkOrderPurpose { Description = "Compliance" },
                Priority = new WorkOrderPriority { Description = "Emergency" },
                Notes = "test",
                StreetNumber = "280",
                Street = new Street { FullStName = "1ST ST" },
                NearestCrossStreet = new Street { FullStName = "12TH ST" },
                Town = new Town { ShortName = "", Zip = "08043", State = new State { Abbreviation = "NJ" } },
                Latitude = 0,
                Longitude = 0,
                DateReceived = DateTime.Now,
                DateCompleted = DateTime.Now,
                //phase 2 changes
                SAPEquipmentNumber = 52337392,
                DeviceLocation = 6002074816,
                Installation = 7003409809,
                OperatingCenter = new OperatingCenter { PlanningPlants = new[] { new PlanningPlant { Code = "S205" } } }
            };
        }

        public WorkOrder GetTestWorkOrderForMainCrossing()
        {
            var town = new Town {
                ShortName = "",
                Zip = "08043",
                State = new State {Abbreviation = "NJ"}                
            };
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                MainSAPFunctionalLocation = new FunctionalLocation {Description = "NJLK-LK-MAINS"},
                MainSAPEquipmentId = 30000502,
                Town = town
            });
            return new WorkOrder {
                //SAPNotificationNumber = 10002250,
                AssetType = new AssetType {Description = "MAIN CROSSING"},
                Town = town,
                WorkDescription = new WorkDescription {Description = "asd"},
                Id = 263575,
                Purpose = new WorkOrderPurpose {Description = "Compliance"},
                Priority = new WorkOrderPriority {Description = "Emergency"},
                Notes = "test",
                StreetNumber = "280",
                Street = new Street {FullStName = "1ST ST"},
                NearestCrossStreet = new Street {FullStName = "12TH ST"},
                Latitude = 0,
                Longitude = 0,
                DateReceived = DateTime.Now,
                DateCompleted = DateTime.Now,
            };
        }

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _workOrder = GetTestWorkOrderForHydrant();
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForSAPWorkOrderHydrant()
        {
            var target = new SAPWorkOrder(_workOrder);
            Assert.AreEqual(target.SAPNotificationNo, _workOrder.SAPNotificationNumber?.ToString());
            Assert.AreEqual(target.DocumentTitle,
                _workOrder.Id.ToString() + " " + _workOrder.WorkDescription?.Description);

            Assert.AreEqual(target.AssetType, _workOrder.AssetType?.Description.ToUpper());
            Assert.AreEqual(target.Purpose, _workOrder.Purpose?.SapCode);
            Assert.AreEqual(target.PurposeCodeGroup, "N-D-PUR1");
            Assert.AreEqual(target.Priority, _workOrder.Priority?.Description);
            Assert.AreEqual(target.ShortText, _workOrder.WorkDescription?.Description.ToString());
            Assert.AreEqual(target.LongText, _workOrder.Notes?.ToString());
            Assert.AreEqual(target.RequestedBy, _workOrder.RequestedBy?.Description);
            Assert.AreEqual(target.House, _workOrder.StreetNumber);
            Assert.AreEqual(target.Street1, _workOrder.Street?.FullStName);
            Assert.AreEqual(target.Street3, _workOrder.NearestCrossStreet?.FullStName?.ToString());
            Assert.AreEqual(target.City, _workOrder.Town?.ShortName);
            Assert.AreEqual(target.PostalCode, _workOrder.Town?.Zip);
            Assert.AreEqual(target.State, _workOrder?.Town?.State?.Abbreviation);
            Assert.AreEqual(target.SearchTerm1, _workOrder.Latitude?.ToString());
            Assert.AreEqual(target.SearchTerm2, _workOrder.Longitude?.ToString());
            Assert.AreEqual(target.MaintActivityType,
                _workOrder.WorkDescription?.PlantMaintenanceActivityType?.Code.ToString()); //PMActType
            Assert.AreEqual(target.DateReceived,
                (_workOrder.DateReceived.HasValue && _workOrder.DateReceived?.Year < 2010)
                    ? "20100101"
                    : _workOrder.DateReceived?.Date.ToString("yyyyMMdd"));
            Assert.AreEqual(target.Operation, _workOrder.WorkDescription?.Description?.ToString());
            Assert.AreEqual(target.OperationDuration, _workOrder.WorkDescription?.TimeToComplete.ToString());
            Assert.AreEqual(target.BasicFinish, _workOrder.PlannedCompletionDate?.Date.ToString("yyyyMMdd"));
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForSAPWorkOrderValve()
        {
            _workOrder.SAPNotificationNumber = 10002081;
            _workOrder.AssetType = new AssetType {Description = "VALVE"};
            _workOrder.Valve = new Valve {
                FunctionalLocation = new FunctionalLocation {Description = "NJLKW-LK-MAINS"}, SAPEquipmentId = 10011989
            };
            var target = new SAPWorkOrder(_workOrder);

            Assert.AreEqual(target.FunctionalLocation, _workOrder.Valve?.FunctionalLocation?.Description);
            Assert.AreEqual(target.EquipmentNo, _workOrder.Valve?.SAPEquipmentNumber);
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForSAPWorkOrderOpening()
        {
            _workOrder.AssetType = new AssetType {Description = "SEWER OPENING"};
            _workOrder.SewerOpening = new SewerOpening {
                FunctionalLocation = new FunctionalLocation {Description = "NJLKW-LK-MAINS-MH"},
                SAPEquipmentId = 5129271
            };
            var target = new SAPWorkOrder(_workOrder);

            Assert.AreEqual(target.FunctionalLocation, _workOrder.SewerOpening?.FunctionalLocation?.Description);
            Assert.AreEqual(target.EquipmentNo, _workOrder.SewerOpening?.SAPEquipmentNumber);
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForSAPWorkOrderMain()
        {
            var town = new Town();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = town, MainSAPFunctionalLocation = new FunctionalLocation {Description = "adasd"},
                MainSAPEquipmentId = 0
            });
            _workOrder.AssetType = new AssetType {Description = "MAIN"};
            _workOrder.Town = town;
            var target = new SAPWorkOrder(_workOrder);

            Assert.AreEqual(target.FunctionalLocation, _workOrder.MainSAPFunctionalLocation?.Description);
            Assert.AreEqual(target.EquipmentNo, _workOrder.MainSAPEquipmentId.ToString());
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForSAPWorkOrderSewerMain()
        {
            var town = new Town();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                SewerMainSAPFunctionalLocation = new FunctionalLocation {Description = ""}, SewerMainSAPEquipmentId = 0,
                Town = town
            });
            _workOrder.AssetType = new AssetType {Description = "SEWER MAIN"};
            _workOrder.Town = town;
            var target = new SAPWorkOrder(_workOrder);

            Assert.AreEqual(target.FunctionalLocation, _workOrder.SewerMainSAPFunctionalLocation?.Description);
            Assert.AreEqual(target.EquipmentNo, _workOrder.SewerMainSAPEquipmentId.ToString());
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForSAPWorkOrderService()
        {
            _workOrder.AssetType = new AssetType {Description = "SERVICE"};
            _workOrder.PremiseNumber = "123123";
            var target = new SAPWorkOrder(_workOrder);

            Assert.AreEqual(target.PremiseNumber, _workOrder.PremiseNumber);
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

            _workOrder.AssetType = new AssetType { Description = "SERVICE" };
            _workOrder.WorkDescription = new WorkDescription { Description = "SERVICE LINE RETIRE NO PREMISE" };
            _workOrder.Town = town;
            _workOrder.OperatingCenter = operatingCenter;

            var target = new SAPWorkOrder(_workOrder);
            
            Assert.AreEqual(oct.MainSAPFunctionalLocation.ToString(), target.FunctionalLoc);
            Assert.AreEqual(oct.MainSAPFunctionalLocation.ToString(), target.FunctionalLocation);
            Assert.AreEqual(oct.MainSAPEquipmentId.ToString(), target.Equipment);
            Assert.AreEqual(oct.MainSAPEquipmentId.ToString(), target.EquipmentNo);
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

            _workOrder.AssetType = new AssetType { Description = "SERVICE" };
            _workOrder.WorkDescription = new WorkDescription { Description = "SERVICE LINE RETIRE NO PREMISE" };
            _workOrder.Town = town;
            _workOrder.OperatingCenter = operatingCenter;
            _workOrder.TownSection = townSection;

            var target = new SAPWorkOrder(_workOrder);
            
            Assert.AreEqual(townSection.MainSAPFunctionalLocation.ToString(), target.FunctionalLoc);
            Assert.AreEqual(townSection.MainSAPFunctionalLocation.ToString(), target.FunctionalLocation);
            Assert.AreEqual(townSection.MainSAPEquipmentId.ToString(), target.Equipment);
            Assert.AreEqual(townSection.MainSAPEquipmentId.ToString(), target.EquipmentNo);
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForSAPWorkOrderSewerLateral()
        {
            _workOrder.AssetType = new AssetType {Description = "SEWER LATERAL"};
            _workOrder.PremiseNumber = "123123";
            var target = new SAPWorkOrder(_workOrder);

            Assert.AreEqual(target.PremiseNumber, _workOrder.PremiseNumber);
        }

        [TestMethod]
        public void TestConstructorSetsPropertiesForSAPWorkOrderMainCrossing()
        {
            var town = new Town();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                MainSAPFunctionalLocation = new FunctionalLocation {Description = "adasd"},
                MainSAPEquipmentId = 0,
                Town = town
            });
            _workOrder.AssetType = new AssetType {Description = "MAIN CROSSING"};
            _workOrder.Town = town;
            var target = new SAPWorkOrder(_workOrder);

            Assert.AreEqual(target.FunctionalLocation, _workOrder.MainSAPFunctionalLocation?.Description);
            Assert.AreEqual(target.EquipmentNo, _workOrder.MainSAPEquipmentId.ToString());
        }
    }
}
