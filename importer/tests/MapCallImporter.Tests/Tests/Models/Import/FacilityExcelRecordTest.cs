using MapCall.Common.Model.Entities;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallImporter.Tests.Models.Import
{
    [TestClass]
    public class FacilityExcelRecordTest : ExcelRecordTestBase<Facility, MyCreateFacility, FacilityExcelRecord>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateStuffForFacilitiesInAberdeenNJ(_container);
        }

        #endregion

        protected override FacilityExcelRecord CreateTarget()
        {
            return new FacilityExcelRecord {
                Department = ProductionDepartment.DESCRIPTION,
                OperatingCenter = OperatingCenters.NJ7.CODE + " - blah blah this doesn't matter",
                PWSID = PublicWaterSupplies._1345001.PWSID
            };
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<Facility, MyCreateFacility, FacilityExcelRecord> test)
        {
            test.String(x => x.FunctionalLocation, x => x.FunctionalLocation);
            test.String(x => x.FacilityName, x => x.FacilityName);
            test.String(x => x.StreetNumber, x => x.StreetNumber);
            test.String(x => x.ZipCode, x => x.ZipCode);
            test.TestedElsewhere(x => x.PWSID);
            test.TestedElsewhere(x => x.FacilityOwnership);
            test.TestedElsewhere(x => x.Department);
            test.TestedElsewhere(x => x.OperatingCenter);
            test.TestedElsewhere(x => x.PlanningPlant);
            test.TestedElsewhere(x => x.Town);
            test.TestedElsewhere(x => x.Street);
            test.TestedElsewhere(x => x.Latitude);
            test.TestedElsewhere(x => x.Longitude);
            test.TestedElsewhere(x => x.FacilityStatus);
            test.TestedElsewhere(x => x.CompanySubsidiary);
            test.TestedElsewhere(x => x.PublicWaterSupplyPressureZone);
            test.TestedElsewhere(x => x.WasteWaterSystemId);
            test.TestedElsewhere(x => x.WasteWaterSystemBasin);
            test.TestedElsewhere(x => x.SystemDeliveryType);
            test.TestedElsewhere(x => x.ChemicalStorageLocation);
            test.TestedElsewhere(x => x.ChemicalFeed);
            test.Decimal(x => x.FacilityTotalCapacityMGD, x => x.FacilityTotalCapacityMGD);
            test.Decimal(x => x.FacilityReliableCapacityMGD, x => x.FacilityReliableCapacityMGD);
            test.Decimal(x => x.FacilityOperatingCapacityMGD, x => x.FacilityOperatingCapacityMGD);
            test.Decimal(x => x.FacilityRatedCapacityMGD, x => x.FacilityRatedCapacityMGD);
            test.Decimal(x => x.FacilityAuxPowerCapacityMGD, x => x.FacilityAuxPowerCapacityMGD);

            test.Boolean(x => x.PropertyOnly, x => x.PropertyOnly);
            test.Boolean(x => x.Administration, x => x.Administration);
            test.Boolean(x => x.EmergencyPower, x => x.EmergencyPower);
            test.Boolean(x => x.GroundWaterSupply, x => x.GroundWaterSupply);
            test.Boolean(x => x.SurfaceWaterSupply, x => x.SurfaceWaterSupply);
            test.Boolean(x => x.Reservoir, x => x.Reservoir);
            test.Boolean(x => x.Dam, x => x.Dam);
            test.Boolean(x => x.Interconnection, x => x.Interconnection);
            test.Boolean(x => x.WaterTreatmentFacility, x => x.WaterTreatmentFacility);
            
            test.Boolean(x => x.DPCC, x => x.DPCC);
            test.Boolean(x => x.UsedInProductionCapacityCalculation, x => x.UsedInProductionCapacityCalculation);
            //RMP/PointOfEntry are conditional fields, When tested with a bool value of true, another string field becomes required and causes an error.
            //but this file requires all props to tested. TestedElsewhere will let this pass - Greg - 1/31/2020, ARR 11/17/2020
            test.TestedElsewhere(x => x.RMP);
            test.TestedElsewhere(x => x.PointOfEntry);
            test.Boolean(x => x.Filtration, x => x.Filtration);
            test.Boolean(x => x.ResidualsGeneration, x => x.ResidualsGeneration);
            test.Boolean(x => x.TReport, x => x.TReport);
            test.Boolean(x => x.DistributivePumping, x => x.DistributivePumping);
            test.Boolean(x => x.BoosterStation, x => x.BoosterStation);
            test.Boolean(x => x.PressureReducing, x => x.PressureReducing);
            test.Boolean(x => x.GroundStorage, x => x.GroundStorage);
            test.Boolean(x => x.ElevatedStorage, x => x.ElevatedStorage);
            test.Boolean(x => x.OnSiteAnalyticalInstruments, x => x.OnSiteAnalyticalInstruments);
            test.Boolean(x => x.SewerLiftStation, x => x.SewerLiftStation);
            test.Boolean(x => x.WasteWaterTreatmentFacility, x => x.WasteWaterTreatmentFacility);
            test.Boolean(x => x.FieldOperations, x => x.FieldOperations);
            test.Boolean(x => x.SpoilsStaging, x => x.SpoilsStaging);
            test.Boolean(x => x.CellularAntenna, x => x.CellularAntenna);
            test.Boolean(x => x.SCADAIntrusionAlarm, x => x.SCADAIntrusionAlarm);
            test.Boolean(x => x.PSM, x => x.PSM);
            test.Boolean(x => x.BasicGroundWaterSupply, x => x.BasicGroundWaterSupply);
            test.Boolean(x => x.RawWaterPumpStation, x => x.RawWaterPumpStation);
            test.Boolean(x => x.Radionuclides, x => x.Radionuclides);
            test.Boolean(x => x.SWMStation, x => x.SWMStation);
            test.Boolean(x => x.WellProd, x => x.WellProd);
            test.Boolean(x => x.WellMonitoring, x => x.WellMonitoring);
            test.Boolean(x => x.ClearWell, x => x.ClearWell);
            test.Boolean(x => x.RawWaterIntake, x => x.RawWaterIntake);
            test.Boolean(x => x.CommunityRightToKnow, x => x.CommunityRightToKnow);
            test.Boolean(x => x.IgnitionEnterprisePortal, x => x.IgnitionEnterprisePortal);
            test.Boolean(x => x.ArcFlashLabelRequired, x => x.ArcFlashLabelRequired);
            test.Boolean(x => x.HasConfinedSpaceRequirement, x => x.HasConfinedSpaceRequirement);
            test.Boolean(x => x.SampleStation, x => x.SampleStation);
            test.Boolean(x => x.UsedInProductionCapacityCalculation, x => x.UsedInProductionCapacityCalculation);
        }

        #region Coordinate/Latitude-Longitude

        [TestMethod]
        public void TestCoordinateIsCreatedFromLatitudeAndLongitude()
        {
            _target.Latitude = 1;
            _target.Longitude = 2;

            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.AreEqual(1, result.Coordinate.Latitude);
                Assert.AreEqual(2, result.Coordinate.Longitude);
            });
        }

        [TestMethod]
        public void TestThrowsWhenLatitudeIsProvidedButNotLongitude()
        {
            _target.Latitude = 1;

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestThrowsWhenLongitudeIsProvidedButNotLatitude()
        {
            _target.Longitude = 2;

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region FacilityOwnership/FacilityOwner

        [TestMethod]
        public void TestFacilityOwnerIsMappedFromFacilityOwnership()
        {
            _target.FacilityOwnership = AmericanWaterFacilityOwner.DESCRIPTION;

            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.FacilityOwnership, _target.MapToEntity(uow, 1, MappingHelper).FacilityOwner.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenFacilityOwnershipNotFound()
        {
            _target.FacilityOwnership = "there is no way this is a valid facility owner";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region FacilityStatus

        [TestMethod]
        public void TestFacilityStatusIsMappedFromFacilityStatus()
        {
            foreach (var value in new[] {"Active", "Inactive"})
            {
                _target.FacilityStatus = value;

                WithUnitOfWork(uow => {
                    Assert.AreEqual(value, _target.MapToEntity(uow, 1, MappingHelper).FacilityStatus.Description);
                });
            }
        }

        [TestMethod]
        public void TestThrowsWhenFacilityStatusNotFound()
        {
            _target.FacilityStatus = "not a valid status";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion

        #region Department

        [TestMethod]
        public void TestDepartmentIsMappedFromDepartment()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.Department, _target.MapToEntity(uow, 1, MappingHelper).Department.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenDepartmentNotFound()
        {
            _target.Department = "this is not a valid department";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenDepartmentNotProvided()
        {
            foreach (var value in new[] {null, " ", string.Empty})
            {
                _target.Department = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region OperatingCenter

        [TestMethod]
        public void TestOperatingCenterIsMappedFromOperatingCenter()
        {
            WithUnitOfWork(uow => {
                Assert.IsTrue(_target.OperatingCenter.StartsWith(_target
                                                                .MapToEntity(uow, 1, MappingHelper).OperatingCenter
                                                                .OperatingCenterCode));
            });
        }

        [TestMethod]
        public void TestThrowsWhenOperatingCenterNotFound()
        {
            _target.OperatingCenter = "MI666 - this is not a real operating center";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenOperatingCenterCannotBeParsed()
        {
            foreach (var value in new[] {"blah", "blah - blah"})
            {
                _target.OperatingCenter = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        [TestMethod]
        public void TestThrowsWhenOperatingCenterNotProvided()
        {
            foreach (var value in new[] {null, " ", string.Empty})
            {
                _target.OperatingCenter = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        [TestMethod]
        public void TestDoesNotChokeOnOperatingCenterCodeWithoutNumbers()
        {
            WithUnitOfWork(uow => {
                var frequencyUnit = new RecurringFrequencyUnit {
                    Id = RecurringFrequencyUnit.Indices.YEAR
                };
                var oc = uow.Insert(new OperatingCenter {
                    OperatingCenterCode = "ILIU",
                    OperatingCenterName = "Belleville/East St Louis",
                    HydrantInspectionFrequencyUnit = frequencyUnit,
                    LargeValveInspectionFrequencyUnit = frequencyUnit,
                    SmallValveInspectionFrequencyUnit = frequencyUnit,
                    SewerOpeningInspectionFrequencyUnit = frequencyUnit,
                    State = new State {Id = NJState.ID}
                });

                _target.OperatingCenter = $"{oc.OperatingCenterCode} - {oc.OperatingCenterName}";

                Assert.AreEqual(oc.Id, _target.MapToEntity(uow, 1, MappingHelper).OperatingCenter.Id);
            });
        }

        #endregion

        #region PlanningPlant

        [TestMethod]
        public void TestPlannignPlantIsMappedFromPlanningPlant()
        {
            _target.PlanningPlant =
                $"{P218PlanningPlant.CODE} - {OperatingCenters.NJ7.CODE} - {P218PlanningPlant.DESCRIPTION}";

            WithUnitOfWork(uow => {
                Assert.IsTrue(
                    _target.PlanningPlant.StartsWith(_target.MapToEntity(uow, 1, MappingHelper).PlanningPlant.Code));
            });
        }

        [TestMethod]
        public void TestThrowsWhenPlanningPlantNotFound()
        {
            _target.PlanningPlant = "P666 - MI666 - this is not a real planning plant";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenPlanningPlantCannotBeParsed()
        {
            foreach (var value in new[] {"blah", "blah - blah", "blah - blah - blah"})
            {
                _target.PlanningPlant = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region Town

        [TestMethod]
        public void TestTownMapsToTown()
        {
            _target.Town = AberdeenMonmouthNJTown.TOWN;

            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.Town, _target.MapToEntity(uow, 1, MappingHelper).Town.ShortName);
            });
        }

        [TestMethod]
        public void TestThrowsWhenTownProvidedDoesNotExistInOperatingCenterProvided()
        {
            _target.Town = "faketon";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion

        #region Street

        [TestMethod]
        public void TestStreetMapsToStreet()
        {
            _target.Town = AberdeenMonmouthNJTown.TOWN;
            _target.Street = AberdeenMonmouthNJStreets.IdlewildLane.NAME;

            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.Street, _target.MapToEntity(uow, 1, MappingHelper).Street.FullStName);
            });
        }

        [TestMethod]
        public void TestThrowsWhenStreetProvidedDoesNotExistWithinTownProvided()
        {
            _target.Town = AberdeenMonmouthNJTown.TOWN;
            _target.Street = "not a real street";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenStreetProvidedButNoTownProvided()
        {
            _target.Street = AberdeenMonmouthNJStreets.IdlewildLane.NAME;

            foreach (var value in new[] {null, " ", string.Empty})
            {
                _target.Town = value;
                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region CompanySubsidiary

        [TestMethod]
        public void TestCompanySubsidiaryIsMappedFromCompanySubsidiary()
        {
            _target.CompanySubsidiary = CompanySubsidiaries.NJAW.DESCRIPTION;

            WithUnitOfWork(uow => {
                Assert.AreEqual(CompanySubsidiaries.NJAW.ID, _target.MapToEntity(uow, 1, MappingHelper).CompanySubsidiary.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenCompanySubsidiaryNotFound()
        {
            _target.CompanySubsidiary = "not a valid company subsidiary";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion

        #region FunctionalLocation

        [TestMethod]
        public void TestDoesNotAllowDuplicatesByFunctionalLocation()
        {
            var fl = "lah de frickin da";

            WithUnitOfWork(uow => {
                GetEntityFactory<Facility>().Create(new {
                    FunctionalLocation = fl,
                    Department = uow.Find<Department>(ProductionDepartment.ID),
                    OperatingCenter = uow.Find<OperatingCenter>(OperatingCenters.NJ7.ID)
                });

                _target.FunctionalLocation = fl;

                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestDoesNotChokeWhenMultipleFacilitiesAlreadyExistWithSameFunctionalLocation()
        {
            var fl = "what he said";

            WithUnitOfWork(uow => {
                GetEntityFactory<Facility>().Create(new {
                    FacilityId = "foo",
                    FunctionalLocation = fl,
                    Department = uow.Find<Department>(ProductionDepartment.ID),
                    OperatingCenter = uow.Find<OperatingCenter>(OperatingCenters.NJ7.ID)
                });
                GetEntityFactory<Facility>().Create(new {
                    FacilityId = "bar",
                    FunctionalLocation = fl,
                    Department = uow.Find<Department>(ProductionDepartment.ID),
                    OperatingCenter = uow.Find<OperatingCenter>(OperatingCenters.NJ7.ID)
                });

                _target.FunctionalLocation = fl;

                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region PWSID/PublicWaterSupply

        [TestMethod]
        public void TestPWSIDMapsToPublicWaterSupply()
        {
            _target.PWSID = PublicWaterSupplies._1345001.PWSID;

            WithUnitOfWork(uow => Assert.AreEqual(PublicWaterSupplies._1345001.ID,
                _target.MapToEntity(uow, 1, MappingHelper).PublicWaterSupply.Id));
        }

        [TestMethod]
        public void TestThrowsWhenPublicWaterSupplyNotFoundByPWSID()
        {
            _target.PWSID = "not a valid pwsid";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion

        #region PublicWaterSupplyPressureZone

        [TestMethod]
        public void TestPublicWaterSupplyPressureZoneMapsToPublicWaterSupplyPressureZone()
        {
            _target.PublicWaterSupplyPressureZone = PublicWaterSupplyPressureZones.HYDRAULIC_MODEL_NAME;

            WithUnitOfWork(uow => Assert.AreEqual(PublicWaterSupplyPressureZones.ID,
                _target.MapToEntity(uow, 1, MappingHelper).PublicWaterSupplyPressureZone.Id));
        }

        [TestMethod]
        public void TestThrowsWhenPublicWaterSupplyPressureZoneNotFound()
        {
            _target.PublicWaterSupplyPressureZone = "not a valid public water supply pressure zone";

            WithUnitOfWork(uow => {ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion

        #region WasteWaterSystem

        [TestMethod]
        public void TestWasteWaterSystemMapsToWasteWaterSystem()
        {
            _target.WasteWaterSystemId = OperatingCenters.NJ7.CODE + "WW" + "0042";
            _target.PWSID = null;

            WithUnitOfWork(uow => Assert.AreEqual(WasteWaterSystems.ID,
                _target.MapToEntity(uow, 1, MappingHelper).WasteWaterSystem.Id));
        }

        [TestMethod]
        public void TestThrowsWhenWasteWaterSystemNotFound()
        {
            _target.WasteWaterSystemId = "Mr. Hankey's House of Horrors";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion

        #region WasteWaterSystemBasin

        [TestMethod]
        public void TestWasteWaterSystemBasinMapsToWasteWaterSystemBasin()
        {
            _target.WasteWaterSystemBasin = WasteWaterSystemBasins.BASIN_NAME;
            _target.WasteWaterSystemId = OperatingCenters.NJ7.CODE + "WW" + "0042";
            _target.PWSID = null;

            WithUnitOfWork(uow => Assert.AreEqual(WasteWaterSystemBasins.ID,
                _target.MapToEntity(uow, 1, MappingHelper).WasteWaterSystemBasin.Id));
        }

        [TestMethod]
        public void TestThrowsWhenWasteWaterSystemBasinNotFound()
        {
            _target.WasteWaterSystemBasin = "Mr. Hankey's House of Horrors";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion

        #region SystemDeliveryType

        [TestMethod]
        public void TestSystemDeliveryTypeMapsToSystemDeliveryType()
        {
            _target.SystemDeliveryType = SystemDeliveryTypes.Water.DESCRIPTION;

            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.SystemDeliveryType, _target.MapToEntity(uow, 1, MappingHelper).SystemDeliveryType.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenSystemDeliveryEntryNotFound()
        {
            _target.SystemDeliveryType = "milk and chocolate chip cookies";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion

        #region PublicWaterSupply/WasteWaterSystem

        [TestMethod]
        public void TestThrowsWhenPublicWaterSupplyAndWasteWaterSystemSpecified()
        {
            _target.WasteWaterSystemId = OperatingCenters.NJ7.CODE + "WW" + "0042";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenNeitherPublicWaterSupplyNorWasteWaterSystemSpecified()
        {
            _target.WasteWaterSystemId = null;
            _target.PWSID = null;

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion

        #region Chemical Feed

        [TestMethod]
        public void TestChemicalFeedIsMappedFromChemicalFeed()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.ChemicalFeed, _target.MapToEntity(uow, 1, MappingHelper).ChemicalFeed);
            });
        }

        #endregion

        #region Chemical Storage Location

        [TestMethod]
        public void TestChemicalStorageLocationIsMappedFromChemicalStorageLocation()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.ChemicalStorageLocation, _target.MapToEntity(uow, 1, MappingHelper).ChemicalStorageLocation);
            });
        }

        #endregion

        #region ChemicalFeed/ChemicalStorageLocation

        [TestMethod]
        public void TestThrowsWhenChemicalFeedIsTrueAndChemicalStorageLocationNotSpecified()
        {
            _target.ChemicalFeed = true;
            _target.ChemicalStorageLocation = null;

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion
    }
}