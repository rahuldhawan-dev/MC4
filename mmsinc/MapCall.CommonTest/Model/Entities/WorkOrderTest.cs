using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class WorkOrderTest : MapCallMvcInMemoryDatabaseTestBase<WorkOrder>
    {
        #region Private Members

        private IDateTimeProvider _dateTimeProvider;
        private DateTime _now;
        private WorkOrder _target;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>()
             .Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new WorkOrder();
            _container.BuildUp(_target);
        }

        #endregion

        #region Address

        [TestMethod]
        public void TestStreetAddressReturnsStreetNumberAndStreetFullName()
        {
            var town = new Town {ShortName = "Anytown", State = new State {Abbreviation = "NJ"}};
            var street = new Street {FullStName = "Main Street"};
            var target = new WorkOrder {StreetNumber = "123", Street = street, Town = town, ZipCode = "12345"};

            Assert.AreEqual(
                String.Format(WorkOrder.FormatStrings.STREET_ADDRESS, target.StreetNumber, street.FullStName),
                target.StreetAddress);
        }

        [TestMethod]
        public void TestTownAddressReturnsTownStateZip()
        {
            var town = new Town {ShortName = "Anytown", State = new State {Abbreviation = "NJ"}};
            var street = new Street {FullStName = "Main Street"};
            var target = new WorkOrder {StreetNumber = "123", Street = street, Town = town, ZipCode = "12345"};

            Assert.AreEqual(
                String.Format(WorkOrder.FormatStrings.TOWN_ADDRESS, target.Town, town.State, target.ZipCode),
                target.TownAddress);
        }

        #endregion

        #region Asset

        [TestMethod]
        public void TestAssetReturnsValveIfAssetTypeIsValve()
        {
            var asset = GetEntityFactory<Valve>().Create();
            var target = new WorkOrder {
                AssetType = GetFactory<ValveAssetTypeFactory>().Create(),
                Valve = asset
            };

            Assert.AreSame(asset, (Valve)target.Asset);
        }

        [TestMethod]
        public void TestAssetReturnsHydrantIfAssetTypeIsHydrant()
        {
            var asset = GetEntityFactory<Hydrant>().Create();
            var target = new WorkOrder {
                AssetType = GetFactory<HydrantAssetTypeFactory>().Create(),
                Hydrant = asset
            };

            Assert.AreSame(asset, (Hydrant)target.Asset);
        }

        [TestMethod]
        public void TestAssetReturnsSewerOpeningIfAssetTypeIsSewerOpening()
        {
            var asset = GetEntityFactory<SewerOpening>().Create();
            var target = new WorkOrder {
                AssetType = GetFactory<SewerOpeningAssetTypeFactory>().Create(),
                SewerOpening = asset
            };

            Assert.AreSame(asset, (SewerOpening)target.Asset);
        }

        [TestMethod]
        public void TestAssetReturnsStormWaterAssetIfAssetTypeIsStormWaterAsset()
        {
            var asset = GetEntityFactory<StormWaterAsset>().Create();
            var target = new WorkOrder {
                AssetType = GetFactory<StormCatchAssetTypeFactory>().Create(),
                StormWaterAsset = asset
            };

            Assert.AreSame(asset, (StormWaterAsset)target.Asset);
        }

        [TestMethod]
        public void TestAssetReturnsEquipmentIfAssetTypeIsEquipment()
        {
            var asset = GetEntityFactory<Equipment>().Create();
            var target = new WorkOrder {
                AssetType = GetFactory<EquipmentAssetTypeFactory>().Create(),
                Equipment = asset
            };

            Assert.AreSame(asset, (Equipment)target.Asset);
        }

        [TestMethod]
        public void TestAssetReturnsMainCrossingIfAssetTypeIsMainCross()
        {
            var asset = GetEntityFactory<MainCrossing>().Create();
            var target = new WorkOrder {
                AssetType = GetFactory<MainCrossingAssetTypeFactory>().Create(),
                MainCrossing = asset
            };

            Assert.AreSame(asset, (MainCrossing)target.Asset);
        }

        #endregion

        #region CurrentAssignment

        [TestMethod]
        public void TestCurrentAssignmentReturnsNullIfNoAssignments()
        {
            var target = GetEntityFactory<WorkOrder>().Create();

            Assert.IsNull(target.CurrentAssignment);
        }

        [TestMethod]
        public void TestCurrentAssignmentReturnsLatestAssignment()
        {
            var target = GetEntityFactory<WorkOrder>().Create();
            var ca1 = GetFactory<CrewAssignmentFactory>().Create(new { WorkOrder = target, AssignedFor = Lambdas.GetYesterday() });
            var ca2 = GetFactory<CrewAssignmentFactory>().Create(new { WorkOrder = target, AssignedFor = Lambdas.GetNow() });
            var ca3 = GetFactory<CrewAssignmentFactory>().Create(new { WorkOrder = target, AssignedFor = Lambdas.GetNow().AddDays(2) });
            var ca4 = GetFactory<CrewAssignmentFactory>().Create(new { WorkOrder = target, AssignedFor = Lambdas.GetNow().AddDays(2) });
            target.CrewAssignments.Add(ca1);
            target.CrewAssignments.Add(ca2);
            target.CrewAssignments.Add(ca3);
            target.CrewAssignments.Add(ca4);

            target = Session.Load<WorkOrder>(target.Id);

            Assert.AreEqual(ca4.AssignedFor, target.CurrentAssignment.AssignedFor);
            Assert.AreEqual(ca4.Id, target.CurrentAssignment.CrewAssignment.Id);
        }

        [TestMethod]
        public void TestCurrentCrewReturnsLatestAssignmentsCrew()
        {
            var target = GetEntityFactory<WorkOrder>().Create();
            var crew1 = GetFactory<CrewFactory>().Create();
            var crew2 = GetFactory<CrewFactory>().Create();
            var ca1 = GetFactory<CrewAssignmentFactory>().Create(new { Crew = crew1, WorkOrder = target, AssignedFor = Lambdas.GetYesterday() });
            var ca2 = GetFactory<CrewAssignmentFactory>().Create(new { Crew = crew2, WorkOrder = target, AssignedFor = Lambdas.GetNow() });
            target.CrewAssignments.Add(ca1);
            target.CrewAssignments.Add(ca2);

            target = Session.Load<WorkOrder>(target.Id);

            Assert.AreEqual(crew2.Id, target.CurrentCrew.Id);
        }

        [TestMethod]
        public void Test_CurrentMarkout_ReturnsMostRecentValidMarkoutWithLatestExpirationDate()
        {
            var target = GetEntityFactory<WorkOrder>().Create();
            var expired = new Markout {ReadyDate = _now.AddDays(-2), ExpirationDate = _now.AddDays(-1), WorkOrder = target, DateOfRequest = DateTime.Now };
            var previousOverlap =
                new Markout { ReadyDate = _now.AddDays(-1), ExpirationDate = _now.AddDays(1), WorkOrder = target, DateOfRequest = DateTime.Now };
            var future =
                new Markout { ReadyDate = _now.AddDays(1), ExpirationDate = _now.AddDays(3), WorkOrder = target, DateOfRequest = DateTime.Now };
            var expected =
                new Markout { ReadyDate = _now.AddDays(-1), ExpirationDate = _now.AddDays(2), WorkOrder = target, DateOfRequest = DateTime.Now };

            target.Markouts.Add(previousOverlap);
            Session.Save(previousOverlap);
            target.Markouts.Add(expected);
            Session.Save(expected);
            target.Markouts.Add(expired);
            Session.Save(expired);
            target.Markouts.Add(future);
            Session.Save(future);

            Session.Save(target);
            Session.Clear();
            target = Session.Load<WorkOrder>(target.Id);

            Assert.AreEqual(expected.ExpirationDate, target.CurrentMarkout.Markout?.ExpirationDate);
        }

        [TestMethod]
        public void Test_CurrentMarkout_ReturnsLastExpiredMarkout_WhenNoValidMarkout()
        {
            var target = GetEntityFactory<WorkOrder>().Create();
            var expected = new Markout {ReadyDate = _now.AddDays(-2), ExpirationDate = _now.AddDays(-1), WorkOrder = target, DateOfRequest = DateTime.Now };
            var older = new Markout {ReadyDate = _now.AddDays(-3), ExpirationDate = _now.AddDays(-2), WorkOrder = target, DateOfRequest = DateTime.Now };
            var oldest = new Markout {ReadyDate = _now.AddDays(-4), ExpirationDate = _now.AddDays(-3), WorkOrder = target, DateOfRequest = DateTime.Now };

            target.Markouts.Add(older);
            Session.Save(older);
            target.Markouts.Add(expected);
            Session.Save(expected);
            target.Markouts.Add(oldest);
            Session.Save(oldest);

            Session.Save(target);
            Session.Clear();
            target = Session.Load<WorkOrder>(target.Id);

            Assert.AreEqual(expected.ExpirationDate, target.CurrentMarkout.Markout?.ExpirationDate);
        }

        [TestMethod]
        public void Test_CurrentMarkout_ReturnsFutureMarkout_WhenNoValidMarkout()
        {
            var target = GetEntityFactory<WorkOrder>().Create();
            var expected = new Markout { ReadyDate = _now.AddDays(2), ExpirationDate = _now.AddDays(5), WorkOrder = target, DateOfRequest = DateTime.Now };
            var older = new Markout { ReadyDate = _now.AddDays(-3), ExpirationDate = _now.AddDays(-2), WorkOrder = target, DateOfRequest = DateTime.Now };
            var oldest = new Markout { ReadyDate = _now.AddDays(-4), ExpirationDate = _now.AddDays(-3), WorkOrder = target, DateOfRequest = DateTime.Now };

            target.Markouts.Add(older);
            Session.Save(older);
            target.Markouts.Add(expected);
            Session.Save(expected);
            target.Markouts.Add(oldest);
            Session.Save(oldest);

            Session.Save(target);
            Session.Clear();
            target = Session.Load<WorkOrder>(target.Id);

            Assert.AreEqual(expected.ExpirationDate, target.CurrentMarkout.Markout?.ExpirationDate);
        }

        [TestMethod]
        public void Test_CurrentMarkout_ReturnsNextReadyMarkout_WhenNoValidMarkoutAndMultipleFutureMarkouts()
        {
            var target = GetEntityFactory<WorkOrder>().Create();
            var future1 = new Markout { ReadyDate = _now.AddDays(2), ExpirationDate = _now.AddDays(5), WorkOrder = target, DateOfRequest = DateTime.Now };
            var future2 = new Markout { ReadyDate = _now.AddDays(3), ExpirationDate = _now.AddDays(6), WorkOrder = target, DateOfRequest = DateTime.Now };
            var future3 = new Markout { ReadyDate = _now.AddDays(3), ExpirationDate = _now.AddDays(7), WorkOrder = target, DateOfRequest = DateTime.Now };

            target.Markouts.Add(future1);
            Session.Save(future1);
            target.Markouts.Add(future2);
            Session.Save(future2);
            target.Markouts.Add(future3);
            Session.Save(future3);

            Session.Save(target);
            Session.Clear();
            target = Session.Load<WorkOrder>(target.Id);

            Assert.AreEqual(future1.ExpirationDate, target.CurrentMarkout.Markout?.ExpirationDate);
        }

        [TestMethod]
        public void Test_CurrentMarkout_ReturnsMostRecentValidMarkoutWhenDuplicateMarkoutsExist()
        {
            var target = GetEntityFactory<WorkOrder>().Create();
            var expired = new Markout { ReadyDate = _now.AddDays(-2), ExpirationDate = _now.AddDays(-1), WorkOrder = target, DateOfRequest = DateTime.Now };
            var previousOverlap =
                new Markout { ReadyDate = _now.AddDays(-1), ExpirationDate = _now.AddDays(1), WorkOrder = target, DateOfRequest = DateTime.Now };
            var future =
                new Markout { ReadyDate = _now.AddDays(1), ExpirationDate = _now.AddDays(3), WorkOrder = target, DateOfRequest = DateTime.Now };
            var expected =
                new Markout { ReadyDate = _now.AddDays(-1), ExpirationDate = _now.AddDays(1), WorkOrder = target, DateOfRequest = DateTime.Now };

            target.Markouts.Add(previousOverlap);
            Session.Save(previousOverlap);
            target.Markouts.Add(expected);
            Session.Save(expected);
            target.Markouts.Add(expired);
            Session.Save(expired);
            target.Markouts.Add(future);
            Session.Save(future);

            Session.Save(target);
            Session.Clear();
            target = Session.Load<WorkOrder>(target.Id);

            Assert.AreEqual(expected.ExpirationDate, target.CurrentMarkout.Markout?.ExpirationDate);
        }

        #endregion

        #region ActuallyCompletedBy

        [TestMethod]
        public void TestActuallyCompletedByReturnsEmptyStringWhenDateCompletedIsNull()
        {
            var target = new WorkOrder();
            
            Assert.AreEqual(string.Empty, target.ActuallyCompletedBy);
        }
        
        [TestMethod]
        public void TestActuallyCompletedByReturnsCompletedByFullNameIfSet()
        {
            var target = new WorkOrder {
                DateCompleted = DateTime.Now,
                CompletedBy = new User { FullName = "Golden Face"}
            };
            
            Assert.AreEqual("Golden Face", target.ActuallyCompletedBy);
        }  
        
        [TestMethod]
        public void TestActuallyCompletedByReturnsAssignedContractorIfSet()
        {
            var target = new WorkOrder {
                DateCompleted = DateTime.Now,
                AssignedContractor = new Contractor { Name = "Vance Refrigeration"}
            };
            
            Assert.AreEqual("Vance Refrigeration", target.ActuallyCompletedBy);
        }

        #endregion
        
        #region HasRealSAPError

        [TestMethod]
        public void Test_HasRealSAPError_ReturnsTrue_WhenCorrectCriteriaMatch()
        {
            var order = new WorkOrder {
                OperatingCenter = new OperatingCenter {
                    SAPEnabled = true,
                    SAPWorkOrdersEnabled = true,
                    IsContractedOperations = false
                },
                SAPErrorCode = "OH NOES!!"
            };

            Assert.IsTrue(order.HasRealSAPError);
        }

        [TestMethod]
        public void Test_HasRealSAPError_ReturnsFalse_WhenOperatingCenterIsNull()
        {
            var order = new WorkOrder {
                SAPErrorCode = "OH NOES!!"
            };

            Assert.IsFalse(order.HasRealSAPError);
        }

        [TestMethod]
        public void Test_HasRealSAPError_ReturnsFalse_WhenOperatingCenterIsNotSAPEnabled()
        {
            var order = new WorkOrder {
                OperatingCenter = new OperatingCenter {
                    SAPEnabled = false,
                    SAPWorkOrdersEnabled = true,
                    IsContractedOperations = false
                },
                SAPErrorCode = "OH NOES!!"
            };

            Assert.IsFalse(order.HasRealSAPError);
        }

        [TestMethod]
        public void Test_HasRealSAPError_ReturnsFalse_WhenOperatingCenterIsNotSAPWorkOrdersEnabled()
        {
            var order = new WorkOrder {
                OperatingCenter = new OperatingCenter {
                    SAPEnabled = true,
                    SAPWorkOrdersEnabled = false,
                    IsContractedOperations = false
                },
                SAPErrorCode = "OH NOES!!"
            };

            Assert.IsFalse(order.HasRealSAPError);
        }

        [TestMethod]
        public void Test_HasRealSAPError_ReturnsFalse_WhenOperatingCenterIsContractedOperations()
        {
            var order = new WorkOrder {
                OperatingCenter = new OperatingCenter {
                    SAPEnabled = true,
                    SAPWorkOrdersEnabled = true,
                    IsContractedOperations = true
                },
                SAPErrorCode = "OH NOES!!"
            };

            Assert.IsFalse(order.HasRealSAPError);
        }

        [TestMethod]
        public void Test_HasRealSAPError_ReturnsFalse_WhenSAPErrorCodeIsNullOrWhitespace()
        {
            var order = new WorkOrder {
                OperatingCenter = new OperatingCenter {
                    SAPEnabled = true,
                    SAPWorkOrdersEnabled = true,
                    IsContractedOperations = false
                }
            };

            Assert.IsFalse(order.HasRealSAPError);

            order.SAPErrorCode = " ";

            Assert.IsFalse(order.HasRealSAPError);
        }

        [TestMethod]
        public void Test_HasRealSAPError_ReturnsFalse_WhenSAPErrorCodeContainsTheWordSuccess()
        {
            var order = new WorkOrder {
                OperatingCenter = new OperatingCenter {
                    SAPEnabled = true,
                    SAPWorkOrdersEnabled = true,
                    IsContractedOperations = false
                },
                SAPErrorCode = "Big Success (but still OH NOES!!)"
            };

            Assert.IsFalse(order.HasRealSAPError);
        }

        #endregion

        [TestMethod]
        public void TestTotalMaterialsUsedCostEqualsTotalMaterialsUsedCost()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var material1 = GetEntityFactory<Material>().Create();
            var material2 = GetEntityFactory<Material>().Create();
            GetEntityFactory<OperatingCenterStockedMaterial>()
               .Create(new {OperatingCenter = operatingCenter, Material = material1, Cost = 10m});
            GetEntityFactory<OperatingCenterStockedMaterial>()
               .Create(new {OperatingCenter = operatingCenter, Material = material2, Cost = 20m});
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {OperatingCenter = operatingCenter});
            GetEntityFactory<MaterialUsed>().Create(new {WorkOrder = workOrder, Material = material1, Quantity = 2});
            GetEntityFactory<MaterialUsed>().Create(new {WorkOrder = workOrder, Material = material2, Quantity = 1});

            Session.Flush();
            Session.Clear();
            workOrder = Session.Load<WorkOrder>(workOrder.Id);

            Assert.AreEqual(40m, workOrder.TotalMaterialCost);
        }

        [TestMethod]
        public void TestDistributionPlanningPlantReturnsDistributionPlanningPlantFromTownSectionWhenSet()
        {
            // arrange
            var planningPlant = new PlanningPlant {Code = "D123"};
            // setup a town section with this planning plant
            var townSection = new TownSection {DistributionPlanningPlant = planningPlant};
            // make sure the work order has this TownSection
            var workOrder = new WorkOrder {TownSection = townSection};

            // act/assert
            Assert.IsNotNull(workOrder.DistributionPlanningPlant);
            Assert.AreEqual(planningPlant, workOrder.DistributionPlanningPlant);
        }

        [TestMethod]
        public void TestDistributionPlanningPlantReturnsDistributionPlanningPlantFromOperatingCenterTownWhenSet()
        {
            // arrange
            var planningPlant = new PlanningPlant {Code = "D123"};
            var town = new Town();
            var operatingCenter = new OperatingCenter();
            var opCntrTown = new OperatingCenterTown
                {OperatingCenter = operatingCenter, Town = town, DistributionPlanningPlant = planningPlant};
            town.OperatingCentersTowns.Add(opCntrTown);

            var workOrder = new WorkOrder {Town = town, OperatingCenter = operatingCenter};

            // act/assert
            Assert.IsNotNull(workOrder.DistributionPlanningPlant);
            Assert.AreEqual(planningPlant, workOrder.DistributionPlanningPlant);
        }

        [TestMethod]
        public void TestDistributionPlanningPlantReturnsDistributionPlanningPlantFromOperatingCenterWhenSet()
        {
            // arrange
            var planningPlant = new PlanningPlant {Code = "D123"};
            var operatingCenter = new OperatingCenter();
            operatingCenter.PlanningPlants.Add(planningPlant);

            var workOrder = new WorkOrder {OperatingCenter = operatingCenter};

            // act/assert
            Assert.IsNotNull(workOrder.DistributionPlanningPlant);
            Assert.AreEqual(planningPlant, workOrder.DistributionPlanningPlant);
        }

        [TestMethod]
        public void TestSewerPlanningPlantReturnsSewerPlanningPlantFromTownSectionWhenSet()
        {
            // arrange
            var planningPlant = new PlanningPlant {Code = "D123"};
            // setup a town section with this planning plant
            var townSection = new TownSection {SewerPlanningPlant = planningPlant};
            // make sure the work order has this TownSection
            var workOrder = new WorkOrder {TownSection = townSection};

            // act/assert
            Assert.IsNotNull(workOrder.SewerPlanningPlant);
            Assert.AreEqual(planningPlant, workOrder.SewerPlanningPlant);
        }

        [TestMethod]
        public void TestSewerPlanningPlantReturnsSewerPlanningPlantFromOperatingCenterTownWhenSet()
        {
            // arrange
            var planningPlant = new PlanningPlant {Code = "D123"};
            var town = new Town();
            var operatingCenter = new OperatingCenter();
            var opCntrTown = new OperatingCenterTown
                {OperatingCenter = operatingCenter, Town = town, SewerPlanningPlant = planningPlant};
            town.OperatingCentersTowns.Add(opCntrTown);

            var workOrder = new WorkOrder {Town = town, OperatingCenter = operatingCenter};

            // act/assert
            Assert.IsNotNull(workOrder.SewerPlanningPlant);
            Assert.AreEqual(planningPlant, workOrder.SewerPlanningPlant);
        }

        [TestMethod]
        public void TestSewerPlanningPlantReturnsSewerPlanningPlantFromOperatingCenterWhenSet()
        {
            // arrange
            var planningPlant = new PlanningPlant {Code = "S123"};
            var operatingCenter = new OperatingCenter();
            operatingCenter.PlanningPlants.Add(planningPlant);

            var workOrder = new WorkOrder {OperatingCenter = operatingCenter};

            // act/assert
            Assert.IsNotNull(workOrder.SewerPlanningPlant);
            Assert.AreEqual(planningPlant, workOrder.SewerPlanningPlant);
        }

        [TestMethod]
        public void TestMainSAPFunctionalLocationReturnsMainSAPFunctionalLocationFromTownSectionWhenSet()
        {
            // arrange
            var functionalLocation = new FunctionalLocation();
            // setup a town section with this functional location
            var townSection = new TownSection {MainSAPFunctionalLocation = functionalLocation};
            // make sure the work order has this TownSection
            var workOrder = new WorkOrder {TownSection = townSection};

            // act/assert
            Assert.IsNotNull(workOrder.MainSAPFunctionalLocation);
            Assert.AreEqual(functionalLocation, workOrder.MainSAPFunctionalLocation);
        }

        [TestMethod]
        public void TestMainSAPFunctionalLocationReturnsMainSAPFunctionalLocationFromOperatingCenterTownWhenSet()
        {
            // arrange
            var functionalLocation = new FunctionalLocation();
            var town = new Town();
            var operatingCenter = new OperatingCenter();
            var opCntrTown = new OperatingCenterTown
                {OperatingCenter = operatingCenter, Town = town, MainSAPFunctionalLocation = functionalLocation};
            town.OperatingCentersTowns.Add(opCntrTown);

            var workOrder = new WorkOrder {Town = town, OperatingCenter = operatingCenter};

            // act/assert
            Assert.IsNotNull(workOrder.MainSAPFunctionalLocation);
            Assert.AreEqual(functionalLocation, workOrder.MainSAPFunctionalLocation);
        }

        [TestMethod]
        public void TestSewerMainSAPFunctionalLocationReturnsSewerMainSAPFunctionalLocationFromTownSectionWhenSet()
        {
            // arrange
            var functionalLocation = new FunctionalLocation();
            // setup a town section with this functional location
            var townSection = new TownSection {SewerMainSAPFunctionalLocation = functionalLocation};
            // make sure the work order has this TownSection
            var workOrder = new WorkOrder {TownSection = townSection};

            // act/assert
            Assert.IsNotNull(workOrder.SewerMainSAPFunctionalLocation);
            Assert.AreEqual(functionalLocation, workOrder.SewerMainSAPFunctionalLocation);
        }

        [TestMethod]
        public void
            TestSewerMainSAPFunctionalLocationReturnsSewerMainSAPFunctionalLocationFromOperatingCenterTownWhenSet()
        {
            // arrange
            var functionalLocation = new FunctionalLocation();
            var town = new Town();
            var operatingCenter = new OperatingCenter();
            var opCntrTown = new OperatingCenterTown
                {OperatingCenter = operatingCenter, Town = town, SewerMainSAPFunctionalLocation = functionalLocation};
            town.OperatingCentersTowns.Add(opCntrTown);

            var workOrder = new WorkOrder {Town = town, OperatingCenter = operatingCenter};

            // act/assert
            Assert.IsNotNull(workOrder.SewerMainSAPFunctionalLocation);
            Assert.AreEqual(functionalLocation, workOrder.SewerMainSAPFunctionalLocation);
        }

        [TestMethod]
        public void TestMainSAPEquipmentIdReturnsMainSAPEquipmentIdFromTownSectionWhenSet()
        {
            // arrange
            var equipmentID = new int();
            // setup a town section with this equipment id
            var townSection = new TownSection {MainSAPEquipmentId = equipmentID};
            // make sure the work order has this TownSection
            var workOrder = new WorkOrder {TownSection = townSection};

            // act/assert
            Assert.IsNotNull(workOrder.MainSAPEquipmentId);
            Assert.AreEqual(equipmentID, workOrder.MainSAPEquipmentId);
        }

        [TestMethod]
        public void TestMainSAPEquipmentIdReturnsMainSAPEquipmentIdFromOperatingCenterTownWhenSet()
        {
            // arrange
            var equipmentID = new int();
            var town = new Town();
            var operatingCenter = new OperatingCenter();
            var opCntrTown = new OperatingCenterTown
                {OperatingCenter = operatingCenter, Town = town, MainSAPEquipmentId = equipmentID};
            town.OperatingCentersTowns.Add(opCntrTown);

            var workOrder = new WorkOrder {Town = town, OperatingCenter = operatingCenter};

            // act/assert
            Assert.IsNotNull(workOrder.MainSAPEquipmentId);
            Assert.AreEqual(equipmentID, workOrder.MainSAPEquipmentId);
        }

        [TestMethod]
        public void TestSewerMainSAPEquipmentIdReturnsSewerMainSAPEquipmentIdFromTownSectionWhenSet()
        {
            // arrange
            var equipmentID = new int();
            // setup a town section with this equipment id
            var townSection = new TownSection {SewerMainSAPEquipmentId = equipmentID};
            // make sure the work order has this TownSection
            var workOrder = new WorkOrder {TownSection = townSection};

            // act/assert
            Assert.IsNotNull(workOrder.SewerMainSAPEquipmentId);
            Assert.AreEqual(equipmentID, workOrder.SewerMainSAPEquipmentId);
        }

        [TestMethod]
        public void TestSewerMainSAPEquipmentIdReturnsSewerMainSAPEquipmentIdFromOperatingCenterTownWhenSet()
        {
            // arrange
            var equipmentID = new int();
            var town = new Town();
            var operatingCenter = new OperatingCenter();
            var opCntrTown = new OperatingCenterTown
                {OperatingCenter = operatingCenter, Town = town, SewerMainSAPEquipmentId = equipmentID};
            town.OperatingCentersTowns.Add(opCntrTown);

            var workOrder = new WorkOrder {Town = town, OperatingCenter = operatingCenter};

            // act/assert
            Assert.IsNotNull(workOrder.SewerMainSAPEquipmentId);
            Assert.AreEqual(equipmentID, workOrder.SewerMainSAPEquipmentId);
        }

        [TestMethod]
        public void TestStatusReturnsStatus()
        {
            var workOrder = GetEntityFactory<WorkOrder>().Create();

            Assert.AreEqual(WorkOrderStatus.Other, workOrder.Status);

            var ca1 = GetFactory<CrewAssignmentFactory>().Create(new { WorkOrder = workOrder, AssignedFor = DateTime.Today.AddDays(-1) });
            workOrder.CrewAssignments.Add(ca1);
            workOrder = Session.Load<WorkOrder>(workOrder.Id);

            Assert.AreEqual(WorkOrderStatus.ScheduledPreviously, workOrder.Status);

            workOrder.CrewAssignments.Remove(ca1);
            var ca2 = GetFactory<CrewAssignmentFactory>().Create(new { WorkOrder = workOrder, AssignedFor = DateTime.Today });
            workOrder.CrewAssignments.Add(ca2);
            workOrder = Session.Load<WorkOrder>(workOrder.Id);

            Assert.AreEqual(WorkOrderStatus.ScheduledCurrently, workOrder.Status);

            workOrder.DateCompleted = DateTime.Now;
            workOrder.CancelledAt = DateTime.Now;

            Assert.AreEqual(WorkOrderStatus.Cancelled, workOrder.Status);

            workOrder.CancelledAt = null;

            Assert.AreEqual(WorkOrderStatus.Completed, workOrder.Status);
        }

        [TestMethod]
        public void TestPlantMaintenanceActivityTypeReturnsFromOverrideWhenUsed()
        {
            var pmat1 = GetEntityFactory<PlantMaintenanceActivityType>()
               .Create(new {Code = "AB1", Description = "ab 1"});
            var pmat2 = GetEntityFactory<PlantMaintenanceActivityType>()
               .Create(new {Code = "AB2", Description = "ab 2"});
            var pmat3 = GetEntityFactory<PlantMaintenanceActivityType>()
               .Create(new {Code = "AB3", Description = "ab 3"});

            var workDescription1 =
                GetEntityFactory<WorkDescription>().Create(new {PlantMaintenanceActivityType = pmat1});
            var workDescription2 =
                GetEntityFactory<WorkDescription>().Create(new {PlantMaintenanceActivityType = pmat2});

            var workOrder = GetEntityFactory<WorkOrder>().Create(new
                {WorkDescription = workDescription1, PlantMaintenanceActivityTypeOverride = pmat2});
            ;

            // if we override directly on the order we use the override instead of the work description pmat
            Assert.AreEqual(pmat2, workOrder.PlantMaintenanceActivityType);
        }

        [TestMethod]
        public void TestPlantMaintenanceActivityTypeReturnsFromWorkDescription()
        {
            var pmat1 = GetEntityFactory<PlantMaintenanceActivityType>()
               .Create(new {Code = "AB1", Description = "ab 1"});
            var workDescription1 =
                GetEntityFactory<WorkDescription>().Create(new {PlantMaintenanceActivityType = pmat1});

            var workOrder = GetEntityFactory<WorkOrder>().Create(new {WorkDescription = workDescription1});
            ;

            // by default we use the work descriptions PMAT
            Assert.AreEqual(pmat1, workOrder.WorkDescription.PlantMaintenanceActivityType);
        }

        [TestMethod]
        public void TestPlantMaintenanceActivityTypeReturnsStateOverrideWhenUsed()
        {
            var pmat1 = GetEntityFactory<PlantMaintenanceActivityType>()
               .Create(new {Code = "AB1", Description = "ab 1"});
            var pmat2 = GetEntityFactory<PlantMaintenanceActivityType>()
               .Create(new {Code = "AB2", Description = "ab 2"});
            var pmat3 = GetEntityFactory<PlantMaintenanceActivityType>()
               .Create(new {Code = "AB3", Description = "ab 3"});
            var workDescription1 =
                GetEntityFactory<WorkDescription>().Create(new {PlantMaintenanceActivityType = pmat1});
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {WorkDescription = workDescription1});
            ;

            // if the state has overrides we use that.
            var stateWorkDescription = GetEntityFactory<StateWorkDescription>().Create(new {
                State = workOrder.OperatingCenter.State,
                WorkDescription = workDescription1,
                PlantMaintenanceActivityType = pmat3
            });
            workOrder.OperatingCenter.State.WorkDescriptionOverrides.Add(stateWorkDescription);
            workOrder.PlantMaintenanceActivityTypeOverride = null;
            Session.Flush();
            Session.Clear();
            Assert.IsTrue(workOrder.OperatingCenter.State.WorkDescriptionOverrides.Count > 0);
            Assert.AreEqual(pmat3, workOrder.PlantMaintenanceActivityType);
        }
        
        [TestMethod]
        public void TestIsEnabled()
        {
            var sop = new StreetOpeningPermit() {
                StreetOpeningPermitNumber = "1234",
                ExpirationDate = DateTime.Now.AddDays(-1)
            };
            var wop = GetEntityFactory<WorkOrderPriority>()
               .Create(new { Id = (int)WorkOrderPriority.Indices.EMERGENCY });
            var mor = GetEntityFactory<MarkoutRequirement>()
               .Create();

            var workOrder = GetEntityFactory<WorkOrder>().Create(new
                { StreetOpeningPermitRequired = true, Priority = wop, MarkoutRequirement = mor });
            
            workOrder.StreetOpeningPermitRequired = true;
            workOrder.StreetOpeningPermits.Add(sop);
            workOrder.Priority = wop;
            Assert.IsFalse(workOrder.IsEnabled);
            workOrder.MarkoutRequirement.Id = (int)MarkoutRequirementEnum.Emergency;
            Assert.IsTrue(workOrder.IsEnabled);
            workOrder.StreetOpeningPermitRequired = false;
            workOrder.OperatingCenter.SAPWorkOrdersEnabled = false;
            workOrder.SAPNotificationNumber = 11111111111;
            workOrder.SAPWorkOrderNumber = 22222222222;
            Assert.IsTrue(workOrder.IsEnabled);
            workOrder.StreetOpeningPermitRequired = true;
            sop.StreetOpeningPermitNumber = null;
            Assert.IsTrue(workOrder.IsEnabled);
            workOrder.StreetOpeningPermitRequired = false;
            workOrder.OperatingCenter.SAPWorkOrdersEnabled = true;
            workOrder.SAPNotificationNumber = null;
            workOrder.SAPWorkOrderNumber = 22222222222;
            Assert.IsFalse(workOrder.IsEnabled);
            workOrder.StreetOpeningPermitRequired = false;
            workOrder.OperatingCenter.SAPWorkOrdersEnabled = true;
            workOrder.SAPNotificationNumber = 11111111111;
            workOrder.SAPWorkOrderNumber = null;
            Assert.IsFalse(workOrder.IsEnabled);
            workOrder.StreetOpeningPermitRequired = false;
            workOrder.OperatingCenter.SAPWorkOrdersEnabled = true;
            workOrder.SAPNotificationNumber = 11111111111;
            workOrder.SAPWorkOrderNumber = 22222222222;
            Assert.IsTrue(workOrder.IsEnabled);
        }

        #region Asset

        [TestMethod]
        public void TestAssetIDReturnsIdentifierFromValueIfAssetTypeIsValve()
        {
            var identifier = "VHF-5000";
            _target.AssetType = new AssetType();
            _target.AssetType.SetPropertyValueByName("Id",
                (int)AssetTypeEnum.Valve);
            _target.Valve = new Valve {ValveNumber = identifier};

            Assert.AreEqual(((IThingWithCoordinate)_target.Asset).Id, _target.Valve.Id);
        }

        [TestMethod]
        public void TestAssetIDReturnsIdentifierFromValueIfAssetTypeIsHydrant()
        {
            var identifier = "HYD-5000";
            _target.AssetType = new AssetType();
            _target.AssetType.SetPropertyValueByName("Id",
                (int)AssetTypeEnum.Hydrant);
            _target.Hydrant = new Hydrant {HydrantNumber = identifier};

            Assert.AreEqual(((IThingWithCoordinate)_target.Asset).Id, _target.Hydrant.Id);
        }

        [TestMethod]
        public void
            TestAssetIDReturnsIdentifierFromValueIfAssetTypeIsSewerOpening()
        {
            var identifier = "1234";
            _target.AssetType = new AssetType();
            _target.AssetType.SetPropertyValueByName("Id",
                (int)AssetTypeEnum.SewerOpening);
            _target.SewerOpening = new SewerOpening {OpeningNumber = identifier};

            Assert.AreEqual(((IThingWithCoordinate)_target.Asset).Id, _target.SewerOpening.Id);
        }

        [TestMethod]
        public void TestAssetIDReturnsIdentifierFromValueIfAssetTypeIsStormCatch
            ()
        {
            var identifier = "1234";
            _target.AssetType = new AssetType();
            _target.AssetType.SetPropertyValueByName("Id",
                (int)AssetTypeEnum.StormCatch);
            _target.StormWaterAsset = new StormWaterAsset {AssetNumber = identifier};

            Assert.AreEqual(((IThingWithCoordinate)_target.Asset).Id, _target.StormWaterAsset.Id);
        }

        #region AssetKey

        [TestMethod]
        public void TestAssetKeyReturnsNullIfThereIsNoAsset()
        {
            _target = new WorkOrder {AssetType = new AssetType()};
            Assert.IsNull(_target.AssetKey);
        }

        [TestMethod]
        public void TestAssetKeyReturnsValveIDFromValveIfAssetTypeIsValve()
        {
            _target = new WorkOrder
                {AssetType = new AssetType(), Valve = new Valve()};
            _target.AssetType.SetPropertyValueByName("Id",
                (int)AssetTypeEnum.Valve);
            _target.Valve.SetPropertyValueByName("ValveNumber", "VMT-106");
            Assert.AreEqual("VMT-106", _target.AssetKey);
        }

        [TestMethod]
        public void TestAssetKeyReturnsHydrantIDFromHydrantIfAssetTypeIsHydrant()
        {
            _target = new WorkOrder
                {AssetType = new AssetType(), Hydrant = new Hydrant()};
            _target.AssetType.SetPropertyValueByName("Id",
                (int)AssetTypeEnum.Hydrant);
            _target.Hydrant.SetPropertyValueByName("HydrantNumber", "HAB-101");
            Assert.AreEqual("HAB-101", _target.AssetKey);
        }

        [TestMethod]
        public void
            TestAssetKeyReturnsSewerOpeningIDFromSewerIfAssetTypeIsSewerOpening()
        {
            _target = new WorkOrder
                {AssetType = new AssetType(), SewerOpening = new SewerOpening()};
            _target.AssetType.SetPropertyValueByName("Id",
                (int)AssetTypeEnum.SewerOpening);
            _target.SewerOpening.SetPropertyValueByName("Id", 888);
            Assert.AreEqual("888", _target.AssetKey);
        }

        [TestMethod]
        public void
            TestAssetKeyReturnsStormCatchIDFromStormCatchIfAssetTypeIsStormCatch
            ()
        {
            _target = new WorkOrder
                {AssetType = new AssetType(), StormWaterAsset = new StormWaterAsset()};
            _target.AssetType.SetPropertyValueByName("Id",
                (int)AssetTypeEnum.StormCatch);
            _target.StormWaterAsset.SetPropertyValueByName("Id", 999);
            Assert.AreEqual("999", _target.AssetKey);
        }

        [TestMethod]
        public void TestAssetKeyReturnsPremiseAndServiceNumberIfAssetTypeIsServiceAndSewerLateral()
        {
            _target = new WorkOrder {
                AssetType = new AssetType {
                    Id = (int)AssetTypeEnum.Service
                }, 
                Service = new Service(),
                PremiseNumber = "9100112375",
                ServiceNumber = "9180564378"
            };
            Assert.AreEqual("p#:9100112375, s#:9180564378", _target.AssetKey);

            _target.AssetType.Id = (int)AssetTypeEnum.SewerLateral;
            _target.PremiseNumber = "9180564378";
            _target.ServiceNumber = "9100112375";

            Assert.AreEqual("p#:9180564378, s#:9100112375", _target.AssetKey);
        }

        #endregion

        #endregion

        #region CanBeApproved

        private WorkOrder CreateValidSupervisorApprovalWorkOrder()
        {
            var asset = new AssetType();
            var wo = new WorkOrder {
                AssetType = asset,
                WorkDescription = new WorkDescription {
                    AssetType = asset
                },
                CancelledAt = null,
                DateCompleted = DateTime.Now,
                OperatingCenter = new OperatingCenter {
                    SAPWorkOrdersEnabled = true,
                },
                SAPWorkOrderNumber = 12345
            };

            return wo; 
        }

        [TestMethod]
        public void TestCanBeApprovedReturnsFalseIfTheWorkOrderIsCancelled()
        {
            _target = CreateValidSupervisorApprovalWorkOrder();
            _target.CancelledAt = DateTime.Now;

            Assert.IsFalse(_target.CanBeApproved);

            _target.CancelledAt = null;
            Assert.IsTrue(_target.CanBeApproved);
        }
        
        [TestMethod]
        public void TestCanBeApprovedReturnsFalseIfTheWorkOrderIsNotCompleted()
        {
            _target = CreateValidSupervisorApprovalWorkOrder();
            _target.DateCompleted = null;

            Assert.IsFalse(_target.CanBeApproved);

            _target.DateCompleted = DateTime.Now;
            Assert.IsTrue(_target.CanBeApproved);
        }

        [TestMethod]
        public void TestCanBeApprovedReturnsFalseIfTheWorkOrderIsNotValidForSAP()
        {
            _target = CreateValidSupervisorApprovalWorkOrder();
            _target.OperatingCenter.SAPWorkOrdersEnabled = false;
            _target.OperatingCenter.IsContractedOperations = false;
            _target.SAPWorkOrderNumber = null;
            Assert.IsTrue(_target.CanBeApproved, "Valid because SAP is not enabled for the operating center.");

            _target.OperatingCenter.SAPWorkOrdersEnabled = true;
            Assert.IsFalse(_target.CanBeApproved);

            _target.OperatingCenter.IsContractedOperations = true;
            Assert.IsTrue(_target.CanBeApproved, "Valid because contracted operations do not require the SAPWorkOrderNumber to be set.");

            _target.OperatingCenter.IsContractedOperations = false;
            _target.SAPWorkOrderNumber = 12345;
            Assert.IsTrue(_target.CanBeApproved, "Valid because SAPWorkOrderNumber is set.");
        }

        [TestMethod]
        public void TestCanBeApprovedReturnsProperlyForServiceWorkOrderWithAndWithOutService()
        {
            // This test is a real pain due to how much logic is going on in this property.

            // This is an otherwise valid WorkOrder for a Service, it just doesn't have the Service yet.
            var serviceAssetType = new AssetType { Id = AssetType.Indices.SERVICE };
            _target = CreateValidSupervisorApprovalWorkOrder();
            _target.AssetType = serviceAssetType;
            _target.WorkDescription = new WorkDescription {
                Id = (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL,
                AssetType = serviceAssetType
            };
           
            Assert.IsFalse(_target.CanBeApproved, "This should fail because there's no Service record attached.");
        
            // This first part of the test is testing the HasServiceApprovalIssue private property.
            // But the property is also hitting HasInvestigativeWorkDescriptionApprovalIssue so the
            // test needs to account for that.
            for (var x = 1; x < 999; x++)
            {
                _target.WorkDescription.Id = x;

                if (WorkDescriptionRepository.SERVICE_APPROVAL_WORK_DESCRIPTIONS.Contains(x) || WorkDescription.INVESTIGATIVE.Contains(x))
                {
                    Assert.IsFalse(_target.CanBeApproved);
                }
                else
                {
                    Assert.IsTrue(_target.CanBeApproved);
                }
            }
            
            // Reset the target back to a valid state.
            _target = CreateValidSupervisorApprovalWorkOrder();
            _target.AssetType = serviceAssetType;
            _target.WorkDescription = new WorkDescription {
                Id = (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL,
                AssetType = serviceAssetType
            };
           
            _target.Service = new Service();
            Assert.IsFalse(_target.CanBeApproved, "This should fail because the service has no DateInstalled value.");

            _target.Service = new Service { DateInstalled = DateTime.Now };
            Assert.IsTrue(_target.CanBeApproved);
        }

        [TestMethod]
        public void TestCanBeApprovedReturnsFalseForInvestigativeWorkDescription()
        {
            _target = CreateValidSupervisorApprovalWorkOrder();
            //_target.WorkDescription = new WorkDescription();
           
            for (var x = 1; x < 999; x++)
            {
                _target.WorkDescription.Id = x;
                if (WorkDescription.INVESTIGATIVE.Contains(x))
                {
                    Assert.IsFalse(_target.CanBeApproved, "Failed on " + x);
                }
                else
                {
                    Assert.IsTrue(_target.CanBeApproved, "Failed on " + x);
                }
            }
        }

        [TestMethod]
        public void TestCanBeApprovedReturnsFalseForSapNotReleasedWorkOrder()
        {
            _target = CreateValidSupervisorApprovalWorkOrder();
            _target.SAPErrorCode = "Error: Order 90941453 is not released";

            Assert.IsFalse(_target.CanBeApproved);

            _target.SAPErrorCode = "WorkOrder Released";

            Assert.IsTrue(_target.CanBeApproved);

            _target.SAPErrorCode = "WorkOrder Release rejected";

            Assert.IsFalse(_target.CanBeApproved);
        }

        [TestMethod]
        public void TestCanBeApprovedReturnsFalseIfAssetTypeDoesNotMatchWorkDescriptionAssetType()
        {
            var asset1 = new AssetType();
            var asset2 = new AssetType();
            _target = CreateValidSupervisorApprovalWorkOrder();
            _target.AssetType = asset1;
            _target.WorkDescription = new WorkDescription {
                AssetType = asset1
            };

            Assert.IsTrue(_target.CanBeApproved, "This work order should be approvable when the work order's asset type matches the work description's asset type.");

            _target.AssetType = asset2;
            Assert.IsFalse(_target.CanBeApproved, "This work order should not be approvable when the work order's asset type does not match the work description's asset type.");
        }

        #endregion

        #region HasRealSAPError

        [TestMethod]
        public void TestHasRealSAPErrorReturnsTrueWhenSAPUpdatableAndErrorMessageDoesNotIncludeSUCCESS()
        {
            _target.OperatingCenter = new OperatingCenter();
            _target.OperatingCenter.SAPEnabled = true;
            _target.OperatingCenter.SAPWorkOrdersEnabled = true;
            _target.OperatingCenter.IsContractedOperations = false;
            Assert.IsTrue(_target.IsSAPUpdatableWorkOrder); // Sanity

            _target.SAPErrorCode = "I am a success!";
            Assert.IsFalse(_target.HasRealSAPError);

            _target.SAPErrorCode = "something bad";
            Assert.IsTrue(_target.HasRealSAPError);

            // And then disable SAP so it returns false again.
            _target.OperatingCenter.SAPEnabled = false;
            Assert.IsFalse(_target.HasRealSAPError);
        }

        #endregion

        #region IsSAPUpdatableWorkOrder

        [TestMethod]
        public void TestIsSAPUpdatableWorkOrder()
        {
            _target.OperatingCenter = new OperatingCenter();
            _target.OperatingCenter.SAPEnabled = false;
            _target.OperatingCenter.SAPWorkOrdersEnabled = false;
            _target.OperatingCenter.IsContractedOperations = false;
            Assert.IsFalse(_target.IsSAPUpdatableWorkOrder);

            _target.OperatingCenter.SAPEnabled = true;
            Assert.IsFalse(_target.IsSAPUpdatableWorkOrder);

            _target.OperatingCenter.SAPWorkOrdersEnabled = true;
            Assert.IsTrue(_target.IsSAPUpdatableWorkOrder);

            _target.OperatingCenter.IsContractedOperations = true;
            Assert.IsFalse(_target.IsSAPUpdatableWorkOrder);
        }

        [TestMethod]
        public void TestIsSAPUpdateableForCancelledWorkOrderReturnsCorrectlyForNotificationAndOrWorkOrderNumber()
        {
            _target.OperatingCenter = new OperatingCenter {
                SAPEnabled = true,
                SAPWorkOrdersEnabled = true,
                IsContractedOperations = false
            };
            _target.CancelledAt = DateTime.Now;

            // without an sap work order number or notification, don't send to sap
            Assert.IsFalse(_target.IsSAPUpdatableWorkOrder);

            // with just an sap work order number, send to sap
            _target.SAPWorkOrderNumber = 123456;
            Assert.IsTrue(_target.IsSAPUpdatableWorkOrder);

            // with both an sap work order number and notification, send to sap
            _target.SAPNotificationNumber = 789;
            Assert.IsTrue(_target.IsSAPUpdatableWorkOrder);

            // with just an sap notification number, send to sap
            _target.SAPWorkOrderNumber = null;
            Assert.IsTrue(_target.IsSAPUpdatableWorkOrder);
        }

        #endregion

        #region Markouts

        [TestMethod]
        public void TestLastMarkoutReturnsThePreviousMarkoutIfThereIsMoreThanOneMarkout()
        {
            var expected = new Markout {
                DateOfRequest = DateTime.Today.AddDays(-1)
            };
            _target.Markouts.Add(new Markout {DateOfRequest = DateTime.Today});
            _target.Markouts.Add(expected);
            Assert.AreSame(expected, _target.LastMarkout);
        }

        [TestMethod]
        public void TestLastMarkoutReturnsNullIfThereAreFewerThanTwoMarkouts()
        {
            Assert.IsFalse(_target.Markouts.Any());
            Assert.IsNull(_target.LastMarkout);
            _target.Markouts.Add(new Markout());
            Assert.IsTrue(_target.Markouts.Count == 1);
            Assert.IsNull(_target.LastMarkout);
        }

        [TestMethod]
        public void TestMarkoutRequiredReturnsTrueWhenMarkoutRequirementIsNotNone()
        {
            _target.MarkoutRequirement = new MarkoutRequirement();

            foreach (
                MarkoutRequirementEnum value in
                Enum.GetValues(typeof(MarkoutRequirementEnum)))
            {
                if (value == MarkoutRequirementEnum.None) continue;
                _target.MarkoutRequirement.SetPropertyValueByName(
                    "Id", (int)value);
                Assert.IsTrue(_target.MarkoutRequired);
            }
        }

        [TestMethod]
        public void TestMarkoutRequiredReturnsFalseWhenMarkoutRequirementIsNone()
        {
            _target.MarkoutRequirement = new MarkoutRequirement();
            _target.MarkoutRequirement.SetPropertyValueByName(
                "Id", (int)MarkoutRequirementEnum.None);

            Assert.IsFalse(_target.MarkoutRequired);
        }

        [TestMethod]
        public void MarkoutExpirationDateReturnsTheCurrentMarkoutsExpirationDate()
        {
            var target = GetEntityFactory<WorkOrder>().Create();
            var expected = new Markout {
                DateOfRequest = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(5),
                ReadyDate = DateTime.Now,
                WorkOrder = target
            };
            target.Markouts.Add(new Markout {
                DateOfRequest = expected.DateOfRequest.Value.AddDays(-20)
            });
            target.Markouts.Add(expected);
            Session.Save(expected);
            Session.Save(target);
            Session.Clear();
            target = Session.Load<WorkOrder>(target.Id);

            Assert.AreEqual(expected.ExpirationDate,
                target.CurrentMarkout.ExpirationDate);
        }

        #endregion

        #region Location/Address

        [TestMethod]
        public void TestStreetAddressReturnsStreetNumberAndFullStName()
        {
            _target.Street = new Street {FullStName = "Wow Street"};
            ;
            _target.StreetNumber = "32";
            Assert.AreEqual("32 Wow Street", _target.StreetAddress);
        }

        [TestMethod]
        public void TestTownAddressReturnsTownNameStateAndZipCode()
        {
            _target.Town = new Town {
                ShortName = "Clown Town",
                State = new State {Abbreviation = "XQ"},
            };
            _target.ZipCode = "00000";
            Assert.AreEqual("Clown Town, XQ 00000", _target.TownAddress);
        }

        #endregion

        #region Crew Assignments

        #region WorkStarted

        [TestMethod]
        public void TestWorkStartedReturnsFalseIfThereAreNoCrewAssignmentsAssociated()
        {
            Assert.IsFalse(_target.CrewAssignments.Any());
            Assert.IsFalse(_target.WorkStarted);
        }

        [TestMethod]
        public void TestWorkStartedReturnsFalseIfThereAreNoStartedCrewAssignments()
        {
            var ca = new CrewAssignment();
            _target.CrewAssignments.Add(ca);
            Assert.IsFalse(_target.WorkStarted);
        }

        [TestMethod]
        public void TestWorkStartedReturnsTrueIfThereAreAnyCrewAssignmentsWithADateStartedValueLessThanOrEqualToToday()
        {
            var ca = new CrewAssignment {DateStarted = DateTime.Today.AddDays(-1)};
            _target.CrewAssignments.Add(ca);
            Assert.IsTrue(_target.WorkStarted);
        }

        #endregion

        #region HasOpenAssignments

        [TestMethod]
        public void TestHasOpenAssignmentsReturnsFalseWhenOrderHasNoAssignments()
        {
            _target = new WorkOrder();

            Assert.IsFalse(_target.HasOpenAssignments);
        }

        [TestMethod]
        public void
            TestHasOpenAssignmentsReturnsFalseWhenOrderHasOnlyClosedAssignments()
        {
            _target = new WorkOrder();
            var assignments = new ClosedCrewAssignmentFactory(_container).BuildList(3);
            assignments.Each(a => _target.CrewAssignments.Add(a));

            Assert.IsFalse(_target.HasOpenAssignments);
        }

        [TestMethod]
        public void
            TestHasOpenAssignmentsReturnsTrueWhenOrderHasAtleastOneOpenAssignment
            ()
        {
            _target = new WorkOrder();
            var assignments = new ClosedCrewAssignmentFactory(_container).BuildList(3);
            assignments =
                assignments.Concat(
                    new OpenCrewAssignmentFactory(_container).BuildList(2));
            assignments.Each(a => _target.CrewAssignments.Add(a));

            Assert.IsTrue(_target.HasOpenAssignments);
        }

        #endregion

        #region TotalManHours

        [TestMethod]
        public void
            TestTotalManHoursReturnsCalculatedManHoursForAllCompletedCrewAssignments
            ()
        {
            var now = DateTime.Now;
            var then = now.AddHours(1);

            var assignments = new[] {
                GetEntityFactory<CrewAssignment>().Build(
                    new {
                        DateStarted = now,
                        DateEnded = then,
                        EmployeesOnJob = (float)1
                    }),
                GetEntityFactory<CrewAssignment>().Build(
                    new {
                        DateStarted = now,
                        DateEnded = then,
                        EmployeesOnJob = (float)2
                    })
            };
            assignments.Each(a => _target.CrewAssignments.Add(a));

            Assert.AreEqual(3, _target.TotalManHours);
        }

        [TestMethod]
        public void
            TestTotalManHoursDoesNotIncludeCrewAssignmentsWithNoCountForEmployeesOnJob
            ()
        {
            var now = DateTime.Now;
            var then = now.AddHours(2);

            var assignments = new[] {
                new CrewAssignmentFactory(_container).Build(
                    new {
                        DateStarted = now,
                        DateEnded = then,
                        EmployeesOnJob = (float)2
                    }),
                new CrewAssignmentFactory(_container).Build(
                    new {
                        DateStarted = now,
                        DateEnded = then,
                        EmployeesOnJob = (float)3
                    }),
                new CrewAssignmentFactory(_container).Build(
                    new {
                        DateStarted = now,
                        DateEnded = then,
                        EmployeesOnJob = (float?)null
                    })
            };
            assignments.Each(a => _target.CrewAssignments.Add(a));

            Assert.AreEqual(10, _target.TotalManHours);
        }

        [TestMethod]
        public void
            TestTotalManHoursReturnsNullIfThereAreAnyIncompleteAssignments()
        {
            var now = DateTime.Now;
            var then = now.AddHours(1);

            var assignments = new[] {
                new CrewAssignmentFactory(_container).Build(
                    new {
                        DateStarted = now,
                        DateEnded = then,
                        EmployeesOnJob = (float)3
                    }),
                new CrewAssignmentFactory(_container).Build(
                    new {
                        DateStarted = now,
                        DateEnded = then,
                        EmployeesOnJob = (float)4
                    }),
                new CrewAssignmentFactory(_container).Build(
                    new {
                        DateStarted = now,
                        DateEnded = (DateTime?)null,
                        EmployeesOnJob = (float?)null
                    })
            };
            assignments.Each(a => _target.CrewAssignments.Add(a));

            Assert.IsNull(_target.TotalManHours);
        }

        #endregion

        #endregion

        #region AppendNotes

        [TestMethod]
        public void TestAppendNotesAppendsNotesToNotes()
        {
            var user = new ContractorUserFactory(_container).Build();
            var expectedDate = DateTime.Now;
            var previousNotes = "some old notes";
            var expectedNotes = "blah blah blah";
            var expectedResult = String.Format(WorkOrder.APPEND_NOTES_FORMAT,
                user.Contractor.Name, user.Email, expectedDate, expectedNotes);
            var allNotes = previousNotes + Environment.NewLine + expectedResult;

            var target = new WorkOrder {Notes = previousNotes};
            target.AppendNotes(user, expectedDate, expectedNotes);

            Assert.IsTrue(target.Notes.EndsWith(expectedResult));
            Assert.AreEqual(allNotes, target.Notes);
        }

        [TestMethod]
        public void TestAppendNotesAddsNotesForUserWithProfile()
        {
            var user = new UserFactory(_container).Build();
            var theNotes = "the notes";
            var expectedNote = $"{user.FullName} {_now.ToString(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS)} {theNotes}";
            var target = new WorkOrder();
            
            target.AppendNotes(user, _dateTimeProvider.GetCurrentDate(), theNotes);

            Assert.AreEqual(expectedNote, target.Notes);
        }
        
        [TestMethod]
        public void TestAppendNotesAppendsNotesForUserWithProfile()
        {
            var user = new UserFactory(_container).Build();
            var theNotes = "the notes";
            var expectedNote = $"Existing notes{Environment.NewLine}{user.FullName} {_now.ToString(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS)} {theNotes}";
            var target = new WorkOrder { Notes = "Existing notes" };
            
            target.AppendNotes(user, _dateTimeProvider.GetCurrentDate(), theNotes);

            Assert.AreEqual(expectedNote, target.Notes);
        }

        #endregion

        #region MainBreak

        [TestMethod]
        public void TestIsMainBreakReturnsTrueWhenIsMainBreak()
        {
            foreach (var x in WorkDescriptionRepository.MAIN_BREAKS)
            {
                var workOrder = new WorkOrder {WorkDescription = new WorkDescription {Id = x}};
                Assert.IsTrue(workOrder.IsMainBreak());
            }
        }

        #endregion

        #region StreetOpeningPermits

        [TestMethod]
        public void CurrentStreetOpeningPermitReturnsTheCurrentStreetOpeningPermit()
        {
            var expected = new StreetOpeningPermit {DateRequested = DateTime.Now};
            _target.StreetOpeningPermits.Add(new StreetOpeningPermit
                {DateRequested = expected.DateRequested.AddDays(-20)});
            _target.StreetOpeningPermits.Add(expected);

            Assert.AreEqual(expected, _target.CurrentStreetOpeningPermit);
        }

        #endregion
        
        #region ArcCollectorLink

        [TestMethod]
        public void TestArcCollectorLink()
        {
            var workOrder = new WorkOrder {
                Latitude = (decimal?)40.247169,
                Longitude = (decimal?)-73.992704,
                OperatingCenter = new OperatingCenter {
                    ArcMobileMapId = "15fdc279b4234fcb85f455ee70897a9e",
                    DataCollectionMapUrl = "arcgis-collector://"
                }
            };

            Assert.AreEqual("arcgis-collector://?referenceContext=center&itemID=15fdc279b4234fcb85f455ee70897a9e&center=40.247169%2c-73.992704&scale=3000", workOrder.ArcCollectorLink);
        }

        #endregion

        #region Coordinate

        [TestMethod]
        public void TestCoordinate()
        {
            var workOrder = new WorkOrder { Latitude = null, Longitude = null };

            Assert.AreEqual(0, workOrder.Coordinate.Latitude);
            Assert.AreEqual(0, workOrder.Coordinate.Longitude);

            workOrder.Latitude = (decimal?)40.247169;
            workOrder.Longitude = (decimal?)-73.992704;

            Assert.AreEqual((decimal)40.247169, workOrder.Coordinate.Latitude);
            Assert.AreEqual((decimal)-73.992704, workOrder.Coordinate.Longitude);
        }

        #endregion
    }
}
