using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class CreateWorkOrderTest : WorkOrderViewModelTestBase<CreateWorkOrder>
    {
        #region Private Members

        private Coordinate _coordinate;
        private OperatingCenter _operatingCenter;

        #endregion

        #region Private Methods

        protected override CreateWorkOrder CreateViewModel()
        {
            _coordinate = GetEntityFactory<Coordinate>().Create();
            _operatingCenter = GetEntityFactory<OperatingCenter>().Create();

            return _viewModelFactory.BuildWithOverrides<CreateWorkOrder>(new {
                OperatingCenter = _operatingCenter.Id,
                CoordinateId = _coordinate.Id
            });
        }

        #endregion

        #region Tests
        
        #region creating from various assets and things

        [TestMethod]
        public void Test_CreateWorkOrderFromEquipment_SetsAppropriateFields()
        {
            var streets = GetEntityFactory<Street>().CreateList(2);
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var town = GetEntityFactory<Town>().Create();
            var townSection = GetEntityFactory<TownSection>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {
                StreetNumber = "123A",
                TownSection = townSection, 
                Town = town
            });
            var equipment = GetEntityFactory<Equipment>().Create(new {
                Facility = facility,
                Coordinate = coordinate,
            });
            var target = new CreateWorkOrder(_container, equipment);

            Assert.AreEqual(facility.OperatingCenter.Id, target.OperatingCenter);
            Assert.AreEqual(facility.Town.Id, target.Town);
            Assert.AreEqual(facility.TownSection.Id, target.TownSection);
            Assert.AreEqual(AssetType.Indices.EQUIPMENT, target.AssetType);
            Assert.AreEqual(facility.StreetNumber, target.StreetNumber);
            Assert.AreEqual(equipment.Coordinate.Id, target.CoordinateId);
            Assert.AreEqual(equipment.Id, target.Equipment);
        }

        [TestMethod]
        public void Test_CreateWorkOrderFromHydrant_SetsAppropriateFields()
        {
            var streets = GetEntityFactory<Street>().CreateList(2);
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var townSection = GetEntityFactory<TownSection>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new {
                Street = streets[0],
                CrossStreet = streets[1],
                Coordinate = coordinate,
                StreetNumber = "123A", 
                TownSection = townSection
            });
            var target = new CreateWorkOrder(_container, hydrant);

            Assert.AreEqual(hydrant.OperatingCenter.Id, target.OperatingCenter);
            Assert.AreEqual(hydrant.Town.Id, target.Town);
            Assert.AreEqual(hydrant.TownSection.Id, target.TownSection);
            Assert.AreEqual(AssetType.Indices.HYDRANT, target.AssetType);
            Assert.AreEqual(hydrant.StreetNumber, target.StreetNumber);
            Assert.AreEqual(hydrant.Street.Id, target.Street);
            Assert.AreEqual(hydrant.CrossStreet.Id, target.NearestCrossStreet);
            Assert.AreEqual(hydrant.Coordinate.Id, target.CoordinateId);
            Assert.AreEqual(hydrant.Id, target.Hydrant);
        }

        [TestMethod]
        public void Test_CreateWorkOrderFromMainCrossing_SetsAppropriateFields()
        {
            var streets = GetEntityFactory<Street>().CreateList(2);
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var mainCrossing = GetEntityFactory<MainCrossing>().Create(new {
                Street = streets[0], ClosestCrossStreet = streets[1],
                Coordinate = coordinate
            });
            var target = new CreateWorkOrder(_container, mainCrossing);

            Assert.AreEqual(mainCrossing.OperatingCenter.Id, target.OperatingCenter);
            Assert.AreEqual(mainCrossing.Town.Id, target.Town);
            Assert.AreEqual(AssetType.Indices.MAIN_CROSSING, target.AssetType);
            Assert.AreEqual(mainCrossing.Street.Id, target.Street);
            Assert.AreEqual(mainCrossing.ClosestCrossStreet.Id, target.NearestCrossStreet);
            Assert.AreEqual(mainCrossing.Coordinate.Id, target.CoordinateId);
            Assert.AreEqual(mainCrossing.Id, target.MainCrossing);
        }

        [TestMethod]
        public void Test_CreateWorkOrderFromService_SetsAppropriateFields()
        {
            var streets = GetEntityFactory<Street>().CreateList(2);
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var servicePriority = GetEntityFactory<ServicePriority>().Create();
            var service = GetEntityFactory<Service>().Create(new {
                Street = streets[0],
                CrossStreet = streets[1],
                Coordinate = coordinate,
                StreetNumber = "123A",
                ApartmentNumber = "Garbage",
                Zip = "12345",
                ServiceNumber = (long?)123,
                PremiseNumber = "456789123",
                ServicePriority = servicePriority
            });
            var target = new CreateWorkOrder(_container, service);

            Assert.AreEqual(service.OperatingCenter.Id, target.OperatingCenter);
            Assert.AreEqual(service.Town.Id, target.Town);
            Assert.AreEqual(AssetType.Indices.SERVICE, target.AssetType);
            Assert.AreEqual(service.StreetNumber, target.StreetNumber);
            Assert.AreEqual(service.Street.Id, target.Street);
            Assert.AreEqual(service.CrossStreet.Id, target.NearestCrossStreet);
            Assert.AreEqual(service.Coordinate.Id, target.CoordinateId);
            Assert.AreEqual(service.Id, target.Service);
            Assert.AreEqual(service.Zip, target.ZipCode);
            Assert.AreEqual(service.ServiceNumber.ToString(), target.ServiceNumber);
            Assert.AreEqual(service.PremiseNumber, target.PremiseNumber);
            Assert.IsNotNull(WorkOrderPriority.Indices.EMERGENCY, target.Priority.Value.ToString());
            Assert.AreEqual(service.ApartmentNumber, target.ApartmentAddtl);
        }
        
        [TestMethod]
        public void Test_CreateWorkOrderFromValve_SetsAppropriateFields()
        {
            var streets = GetEntityFactory<Street>().CreateList(2);
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var townSection = GetEntityFactory<TownSection>().Create();
            var valve = GetEntityFactory<Valve>().Create(new {
                Street = streets[0],
                CrossStreet = streets[1],
                Coordinate = coordinate,
                StreetNumber = "123A",
                TownSection = townSection
            });
            var target = new CreateWorkOrder(_container, valve);

            Assert.AreEqual(valve.OperatingCenter.Id, target.OperatingCenter);
            Assert.AreEqual(valve.Town.Id, target.Town);
            Assert.AreEqual(valve.TownSection.Id, target.TownSection);
            Assert.AreEqual(AssetType.Indices.VALVE, target.AssetType);
            Assert.AreEqual(valve.StreetNumber, target.StreetNumber);
            Assert.AreEqual(valve.Street.Id, target.Street);
            Assert.AreEqual(valve.CrossStreet.Id, target.NearestCrossStreet);
            Assert.AreEqual(valve.Coordinate.Id, target.CoordinateId);
            Assert.AreEqual(valve.Id, target.Valve); 
        }

        [TestMethod]
        public void Test_CreateWorkOrderFromSewerOpening_SetsAppropriateFields()
        {
            var streets = GetEntityFactory<Street>().CreateList(2);
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var townSection = GetEntityFactory<TownSection>().Create();
            var sewerOpening = GetEntityFactory<SewerOpening>().Create(new {
                Street = streets[0],
                IntersectingStreet = streets[1],
                Coordinate = coordinate,
                StreetNumber = "123A",
                TownSection = townSection
            });
            var target = new CreateWorkOrder(_container, sewerOpening);

            Assert.AreEqual(sewerOpening.OperatingCenter.Id, target.OperatingCenter);
            Assert.AreEqual(sewerOpening.Town.Id, target.Town);
            Assert.AreEqual(sewerOpening.TownSection.Id, target.TownSection);
            Assert.AreEqual(AssetType.Indices.SEWER_OPENING, target.AssetType);
            Assert.AreEqual(sewerOpening.StreetNumber, target.StreetNumber);
            Assert.AreEqual(sewerOpening.Street.Id, target.Street);
            Assert.AreEqual(sewerOpening.IntersectingStreet.Id, target.NearestCrossStreet);
            Assert.AreEqual(sewerOpening.Coordinate.Id, target.CoordinateId);
            Assert.AreEqual(sewerOpening.Id, target.SewerOpening);
        }

        [TestMethod]
        public void Test_CreateWorkOrderFromEchoshoreLeakAlert_SetsAppropriateFields()
        {
            var streets = GetEntityFactory<Street>().CreateList(2);
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var townSection = GetEntityFactory<TownSection>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new {
                Street = streets[0],
                CrossStreet = streets[1],
                Coordinate = coordinate,
                StreetNumber = "123A",
                TownSection = townSection
            });
            var echoshoreSite = GetEntityFactory<EchoshoreSite>().Create(new {
                hydrant.OperatingCenter,
                hydrant.Town,
                Description = "site description"
            });
            var echoshoreLeakAlert = GetEntityFactory<EchoshoreLeakAlert>().Create(new {
                PersistedCorrelatedNoiseId = 108,
                EchoshoreSite = echoshoreSite,
                Hydrant1 = hydrant,
                Note = "n/a",
                DistanceFromHydrant1 = 1m,
                DistanceFromHydrant2 = 2m,
                FieldInvestigationRecommendedOn = new DateTime(2022, 11, 30)
            });
            var target = new CreateWorkOrder(_container, echoshoreLeakAlert);

            Assert.AreEqual(hydrant.OperatingCenter.Id, target.OperatingCenter);
            Assert.AreEqual(hydrant.Town.Id, target.Town);
            Assert.AreEqual(hydrant.TownSection.Id, target.TownSection);
            Assert.AreEqual(AssetType.Indices.HYDRANT, target.AssetType);
            Assert.AreEqual(hydrant.StreetNumber, target.StreetNumber);
            Assert.AreEqual(hydrant.Street.Id, target.Street);
            Assert.AreEqual(hydrant.CrossStreet.Id, target.NearestCrossStreet);
            Assert.AreEqual(hydrant.Coordinate.Id, target.CoordinateId);
            Assert.AreEqual(hydrant.Id, target.Hydrant);
            Assert.AreEqual((int)WorkOrderPurpose.Indices.SAFETY, target.Purpose);
            Assert.AreEqual(
                string.Format(
                    CreateWorkOrder.ECHOSHORE_NOTE,
                    echoshoreLeakAlert.PersistedCorrelatedNoiseId,
                    echoshoreLeakAlert.Hydrant1Text,
                    echoshoreLeakAlert.Hydrant2Text,
                    echoshoreSite,
                    echoshoreLeakAlert.Note,
                    echoshoreLeakAlert.DistanceFromHydrant1,
                    echoshoreLeakAlert.DistanceFromHydrant2),
                target.Notes);
            Assert.AreEqual(echoshoreLeakAlert.Id, target.EchoshoreLeakAlert);
            Assert.AreEqual(echoshoreLeakAlert.FieldInvestigationRecommendedOn, target.DateReceived);
        }

        [TestMethod]
        public void Test_CreateWorkOrderFromEchoshoreLeakAlert_SetsTownFromHydrantWhenDifferentFromEchoshoreSite()
        {
            // hydrant will create its own town
            var hydrant = GetEntityFactory<Hydrant>().Create();
            // echoshore site will use this new town
            var town = GetEntityFactory<Town>().Create();
            var echoshoreSite = GetEntityFactory<EchoshoreSite>().Create(new {
                hydrant.OperatingCenter,
                Town = town,
                Description = "site description"
            });
            // sanity check
            Assert.AreNotEqual(town, hydrant.Town);
            var echoshoreLeakAlert = GetEntityFactory<EchoshoreLeakAlert>().Create(new {
                PersistedCorrelatedNoiseId = 108,
                EchoshoreSite = echoshoreSite,
                Hydrant1 = hydrant,
                Note = "n/a",
                DistanceFromHydrant1 = 1m,
                DistanceFromHydrant2 = 2m,
                FieldInvestigationRecommendedOn = new DateTime(2022, 11, 30)
            });

            var target = new CreateWorkOrder(_container, echoshoreLeakAlert);

            Assert.AreEqual(hydrant.Town.Id, target.Town);
            Assert.AreNotEqual(echoshoreSite.Town, target.Town);
        }

        #region Creating a work order from a premise record

        [TestMethod]
        public void Test_CreateWorkOrderFromPremise_SetsAppropriateFields()
        {
            var street = GetEntityFactory<Street>().Create();
            // StreetFactory should really be doing this.
            street.Town.Streets.Add(street);
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var premise = GetEntityFactory<Premise>().Create(new {
                ServiceCity = street.Town,
                ServiceAddressStreet = street.FullStName,
                ServiceZip = "07720",
                Coordinate = coordinate,
                PremiseNumber = "123456",
                DeviceLocation = "location thing",
                Installation = "Some installation",
                Equipment = "0000001234",
                MeterSerialNumber = "Millions of meters! Meters for me!"
            });

            var target = new CreateWorkOrder(_container, premise);

            Assert.AreEqual(premise.OperatingCenter.Id, target.OperatingCenter);
            Assert.AreEqual(premise.ServiceCity.Id, target.Town);
            Assert.AreEqual(premise.Street.Id, target.Street);
            Assert.AreEqual("07720", target.ZipCode);
            Assert.AreEqual(premise.Coordinate.Id, target.CoordinateId);
        }

        [TestMethod]
        public void Test_CreateWorkOrderFromPremise_DoesNotSetSomeFields_WhenServiceUtilityTypeIsNull()
        {
            var premise = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = "123456",
                DeviceLocation = "location thing",
                Installation = "Some installation",
                Equipment = "0000001234",
                MeterSerialNumber = "Millions of meters! Meters for me!"
            });
            premise.ServiceUtilityType = null;

            var target = new CreateWorkOrder(_container, premise);

            Assert.IsNull(target.PremiseNumber);
            Assert.IsNull(target.DeviceLocation);
            Assert.IsNull(target.Installation);
            Assert.IsNull(target.DeviceLocation);
            Assert.IsNull(target.SAPEquipmentNumber);
            Assert.IsNull(target.AssetType);
            Assert.IsNull(target.ServiceUtilityType);
            Assert.IsNull(target.PremiseAddress);
        }

        [TestMethod]
        public void Test_CreateWorkOrderFromPremise_SetsSomeFields_WhenServiceUtilityTypeIsNotNull()
        {
            var town = GetEntityFactory<Town>().Create(new { ShortName = "Short Town" });
            var premise = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = "123456",
                DeviceLocation = "1234567890",
                Installation = "1234567891",
                Equipment = "0000001234",
                MeterSerialNumber = "Millions of meters! Meters for me!",
                ServiceUtilityType = typeof(ServiceUtilityTypeFactory),
                FullStreetAddress = "123 Some Street",
                ServiceCity = town,
                ServiceZip = "07720",
            });

            var target = new CreateWorkOrder(_container, premise);

            Assert.AreEqual("123456", target.PremiseNumber);
            Assert.AreEqual(1234567890, target.DeviceLocation);
            Assert.AreEqual(1234567891, target.Installation);
            Assert.AreEqual(1234, target.SAPEquipmentNumber);
            Assert.AreEqual("Millions of meters! Meters for me!", target.MeterSerialNumber);
            Assert.AreEqual(premise.ServiceUtilityType.Description, target.ServiceUtilityType);
            Assert.AreEqual("123 Some Street, Short Town, NJ 07720", target.PremiseAddress);

            // Make sure setting PremiseAdderess doesn't choke when ServiceCity is null
            premise.ServiceCity = null;
            target = new CreateWorkOrder(_container, premise);
            Assert.AreEqual("123 Some Street, ,  07720", target.PremiseAddress);
        }

        [TestMethod]
        public void Test_CreateWorkOrderFromPremise_SetsAssetType_BasedOnServiceUtilityType()
        {
            var premise = new Premise();
            var serviceUtilityType = new ServiceUtilityType();
            premise.ServiceUtilityType = serviceUtilityType;

            var assetTypeIdsByServiceUtilitTypeId = new Dictionary<int, int> {
                { ServiceUtilityType.Indices.DOMESTIC_WASTEWATER, AssetType.Indices.SEWER_LATERAL },
                { ServiceUtilityType.Indices.NON_POTABLE, AssetType.Indices.SEWER_LATERAL },
                { 
                    ServiceUtilityType.Indices.WASTE_WATER_WITH_DEDUCT_SERVICE,
                    AssetType.Indices.SEWER_LATERAL
                },
                { ServiceUtilityType.Indices.PUBLIC_FIRE_SERVICE, AssetType.Indices.SERVICE },
                { ServiceUtilityType.Indices.PRIVATE_FIRE_SERVICE, AssetType.Indices.SERVICE },
                { ServiceUtilityType.Indices.DOMESTIC_WATER, AssetType.Indices.SERVICE },
                { ServiceUtilityType.Indices.BULK_WATER, AssetType.Indices.SERVICE },
                { ServiceUtilityType.Indices.BULK_WATER_MASTER, AssetType.Indices.SERVICE },
            };

            foreach (var sut in assetTypeIdsByServiceUtilitTypeId)
            {
                serviceUtilityType.Id = sut.Key;
                var target = new CreateWorkOrder(_container, premise);
                Assert.AreEqual(sut.Value, target.AssetType);
            }
        }

        [TestMethod]
        public void Test_CreateWorkOrderFromPremise_DoesNotError_DueToNullReferencesOnThePremise()
        {
            var premise = new Premise();
            MyAssert.DoesNotThrow(() => new CreateWorkOrder(_container, premise));
        }

        #endregion

        #endregion

        #region SetDefaults

        [TestMethod]
        public void TestSetDefaultsDefaultsRequestingEmployee()
        {
            _viewModel.SetDefaults();

            Assert.AreEqual(_viewModel.RequestingEmployee, _user.Id);
        }

        [TestMethod]
        public void TestSetDefaultsDoesNotDefaultOperatingCenterIfAlreadySet()
        {
            _viewModel.OperatingCenter = 108;
            _viewModel.SetDefaults();

            Assert.AreNotEqual(_viewModel.OperatingCenter, _user.DefaultOperatingCenter.Id);
            Assert.AreEqual(108, _viewModel.OperatingCenter);
        }

        [TestMethod]
        public void TestSetDefaultsSetsDefaultOperatingCenterIfNotAlreadySet()
        {
            _viewModel.SetDefaults();

            Assert.AreEqual(_viewModel.OperatingCenter, _user.DefaultOperatingCenter.Id);
        }

        #endregion

        #region Mapping
        
        [TestMethod]
        public void Test_MapToEntity_SetsSendToSAPFalse_WhenOperatingCenterSAPEnabledFalse()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc1.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void Test_MapToEntity_SetsSendToSAPFalse_WhenOperatingCenterSAPEnabledTrueAndContractedOps()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc1.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void Test_MapToEntity_SetsSendToSAPTrue_WhenOperatingCenterSAPEnabledTrue()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                SAPEnabled = true,
                SAPWorkOrdersEnabled = true
            });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc1.Id;

            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void Test_MapToEntity_SetsDigitalAsBuiltRequiredToTrue_WhenWorkDescriptionCallsForIt()
        {
            _entity.WorkDescription.DigitalAsBuiltRequired = true;
            Session.Save(_entity.WorkDescription);
            Session.Flush();
            _viewModel.DigitalAsBuiltRequired = false;
            _viewModel.WorkDescription = _entity.WorkDescription.Id;

            var result = _viewModel.MapToEntity(new WorkOrder());
            
            Assert.IsTrue(result.DigitalAsBuiltRequired);
        }

        [TestMethod]
        public void Test_MapToEntity_LeavesDigitalAsBuiltAlone_WhenWorkDescriptionDoesNotRequiredIt()
        {
            _entity.WorkDescription.DigitalAsBuiltRequired = false;
            Session.Save(_entity.WorkDescription);
            Session.Flush();
            _viewModel.DigitalAsBuiltRequired = false;
            _viewModel.WorkDescription = _entity.WorkDescription.Id;

            var result = _viewModel.MapToEntity(new WorkOrder());
            
            Assert.IsFalse(result.DigitalAsBuiltRequired);
        }

        [TestMethod]
        public void Test_MapToEntity_SetsSmartCovertAlertFields()
        {
            var applicationDescriptionType = GetFactory<SmartCoverAlertApplicationDescriptionTypeFactory>()
               .Create();
            var smartCoverAlert = GetEntityFactory<SmartCoverAlert>().Create(new {
                ApplicationDescription = applicationDescriptionType
            });
            _viewModel.SmartCoverAlert = smartCoverAlert.Id;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(smartCoverAlert.Id, _entity.SmartCoverAlert.Id);
            Assert.IsTrue(_entity.SmartCoverAlert.Acknowledged);
            Assert.IsTrue(_entity.SmartCoverAlert.NeedsToSync);
        }

        [TestMethod]
        public void
            Test_MapToEntity_SetsMaterialsAndSizesToMostRecentlyInstalledValues_ForServiceRenewals()
        {
            // exception will be generated without these
            GetFactory<ServiceLineRenewalWorkDescriptionFactory>().Create();
            GetFactory<ServiceLineRenewalLeadWorkDescriptionFactory>().Create();
            GetFactory<ServiceLineRenewalStormRestorationWorkDescriptionFactory>().Create();

            var installation = 1234567890;

            var premise = GetEntityFactory<Premise>().Create(new {
                Installation = installation.ToString(),
                OperatingCenter = _operatingCenter
            });
            var service = GetEntityFactory<Service>().Create(new {
                Premise = premise
            });

            _viewModel.OperatingCenter = _operatingCenter.Id;
            _viewModel.Installation = installation;
            _viewModel.AssetType = AssetType.Indices.SERVICE;

            foreach (var description in WorkDescription.SERVICE_LINE_RENEWALS)
            {
                _viewModel.WorkDescription = description;

                var entity = _viewModel.MapToEntity(_entity);
                
                Assert.AreEqual(service.ServiceMaterial, entity.CompanyServiceLineMaterial);
                Assert.AreEqual(service.ServiceSize, entity.CompanyServiceLineSize);
                Assert.AreEqual(service.CustomerSideMaterial, entity.CustomerServiceLineMaterial);
                Assert.AreEqual(service.CustomerSideSize, entity.CustomerServiceLineSize);

                entity.CompanyServiceLineMaterial = null;
                entity.CompanyServiceLineSize = null;
                entity.CustomerServiceLineMaterial = null;
                entity.CustomerServiceLineSize = null;
            }
        }

        [TestMethod]
        public void
            Test_MapToEntity_DoesNotSetMaterialsAndSizesToMostRecentlyInstalledValues_ForServiceOrdersWhichAreNotRenewals()
        {
            var installation = 1234567890;

            var premise = GetEntityFactory<Premise>().Create(new {
                Installation = installation.ToString(),
                OperatingCenter = _operatingCenter
            });
            var service = GetEntityFactory<Service>().Create(new {
                Premise = premise
            });

            _viewModel.OperatingCenter = _operatingCenter.Id;
            _viewModel.Installation = installation;
            _viewModel.AssetType = AssetType.Indices.SERVICE;

            var descriptions = Session
                              .Query<WorkDescription>()
                              .Where(wd => wd.AssetType.Id == AssetType.Indices.SERVICE &&
                                           !WorkDescription.SERVICE_LINE_RENEWALS.Contains(wd.Id))
                              .Select(wd => wd.Id);
            
            foreach (var description in descriptions)
            {
                _viewModel.WorkDescription = description;

                var entity = _viewModel.MapToEntity(_entity);
                
                Assert.IsNull(entity.CompanyServiceLineMaterial);
                Assert.IsNull(entity.CompanyServiceLineSize);
                Assert.IsNull(entity.CustomerServiceLineMaterial);
                Assert.IsNull(entity.CustomerServiceLineSize);
            }
        }

        [TestMethod]
        public void Test_MapToEntity_SetsMaterialsToNull_ForServiceRenewalsWhenUnknown()
        {
            // exception will be generated without these
            GetFactory<ServiceLineRenewalWorkDescriptionFactory>().Create();
            GetFactory<ServiceLineRenewalLeadWorkDescriptionFactory>().Create();
            GetFactory<ServiceLineRenewalStormRestorationWorkDescriptionFactory>().Create();

            var installation = 1234567890;
            var unknownMaterial = GetEntityFactory<ServiceMaterial>().Create(new {
                Description = "UnKnoWn"
            });
            var premise = GetEntityFactory<Premise>().Create(new {
                Installation = installation.ToString(),
                OperatingCenter = _operatingCenter
            });
            var service = GetEntityFactory<Service>().Create(new {
                Premise = premise,
                CustomerSideMaterial = unknownMaterial,
                ServiceMaterial = unknownMaterial
            });

            _viewModel.OperatingCenter = _operatingCenter.Id;
            _viewModel.Installation = installation;
            _viewModel.AssetType = AssetType.Indices.SERVICE;

            foreach (var description in WorkDescription.SERVICE_LINE_RENEWALS)
            {
                _viewModel.WorkDescription = description;

                var entity = _viewModel.MapToEntity(_entity);

                Assert.IsNull(entity.CompanyServiceLineMaterial);
                Assert.AreEqual(service.ServiceSize, entity.CompanyServiceLineSize);
                Assert.IsNull(entity.CustomerServiceLineMaterial);
                Assert.AreEqual(service.CustomerSideSize, entity.CustomerServiceLineSize);

                entity.CompanyServiceLineMaterial = null;
                entity.CompanyServiceLineSize = null;
                entity.CustomerServiceLineMaterial = null;
                entity.CustomerServiceLineSize = null;
            }
        }

        [TestMethod]
        public void Test_MapToEntity_SetsRequestingEmployeeToNull_WhenRequestedByIsNotEmployee()
        {
            _entity.RequestingEmployee = GetEntityFactory<User>().Create();
            _viewModel.RequestingEmployee = 1;
            _viewModel.RequestedBy = WorkOrderRequester.Indices.CUSTOMER;

            var entity = _viewModel.MapToEntity(_entity);

            Assert.IsNull(entity.RequestingEmployee);
        }

        [TestMethod]
        public void Test_MapToEntity_DoesNotSetRequestingEmployeeToNull_WhenRequestedByIsEmployee()
        {
            _entity.RequestingEmployee = GetEntityFactory<User>().Create();
            _viewModel.RequestingEmployee = 1;
            _viewModel.RequestedBy = WorkOrderRequester.Indices.EMPLOYEE;

            var entity = _viewModel.MapToEntity(_entity);

            Assert.IsNotNull(entity.RequestingEmployee);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            base.TestEntityMustExistValidation();

            ValidationAssert
               .EntityMustExist<CustomerImpactRange>(x => x.EstimatedCustomerImpact)
               .EntityMustExist<MarkoutRequirement>(x => x.MarkoutRequirement)
               .EntityMustExist<RepairTimeRange>(x => x.AnticipatedRepairTime)
               .EntityMustExist<Street>(x => x.NearestCrossStreet)
               .EntityMustExist<Street>(x => x.Street)
               .EntityMustExist<TownSection>(x => x.TownSection)
               .EntityMustExist<User>(x => x.RequestingEmployee)
               .EntityMustExist<WorkDescription>(x => x.WorkDescription)
               .EntityMustExist<WorkOrder>(x => x.OriginalOrderNumber)
               .EntityMustExist<WorkOrderPriority>(x => x.Priority)
               .EntityMustExist<WorkOrderPurpose>(x => x.Purpose);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();

            _vmTester
               .CanMapBothWays(x => x.AlertIssued)
               .CanMapBothWays(
                    x => x.AnticipatedRepairTime,
                    GetFactory<FourToSixRepairTimeRangeFactory>().Create())
               .CanMapBothWays(x => x.CustomerName)
               .CanMapBothWays(
                    x => x.EstimatedCustomerImpact,
                    GetFactory<ZeroToFiftyCustomerImpactRangeFactory>().Create())
               .CanMapBothWays(x => x.HasSampleSite)
               .CanMapBothWays(x => x.MarkoutRequirement)
               .CanMapBothWays(x => x.NearestCrossStreet)
               .CanMapBothWays(x => x.Notes)
               .CanMapBothWays(x => x.SpecialInstructions)
               .CanMapBothWays(x => x.PhoneNumber)
               .CanMapBothWays(x => x.PremiseNumber)
               .CanMapBothWays(x => x.Priority)
               .CanMapBothWays(x => x.Purpose)
               .CanMapBothWays(x => x.SAPNotificationNumber)
               .CanMapBothWays(x => x.SAPWorkOrderNumber)
               .CanMapBothWays(x => x.SecondaryPhoneNumber)
               .CanMapBothWays(x => x.SignificantTrafficImpact)
               .CanMapBothWays(x => x.Street)
               .CanMapBothWays(x => x.StreetOpeningPermitRequired)
               .CanMapBothWays(x => x.TownSection)
               .CanMapBothWays(x => x.TrafficControlRequired)
               .CanMapBothWays(x => x.WorkDescription)
               .CanMapBothWays(x => x.ZipCode)
               .CanMapBothWays(x => x.PlannedCompletionDate);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();

            ValidationAssert
               .PropertyIsRequired(x => x.MarkoutRequirement)
               .PropertyIsRequired(x => x.NearestCrossStreet)
               .PropertyIsRequired(x => x.Priority)
               .PropertyIsRequired(x => x.Purpose)
               .PropertyIsRequired(x => x.Street)
               .PropertyIsRequired(x => x.WorkDescription);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            base.TestStringLengthValidation();

            ValidationAssert
               .PropertyHasMaxStringLength(x => x.CustomerName, WorkOrder.StringLengths.CUSTOMER_NAME)
               .PropertyHasMaxStringLength(x => x.PhoneNumber, WorkOrder.StringLengths.PHONE_NUMBER)
               .PropertyHasMaxStringLength(x => x.SecondaryPhoneNumber, WorkOrder.StringLengths.SECONDARY_PHONE_NUMBER)
               .PropertyHasMaxStringLength(x => x.ZipCode, WorkOrder.StringLengths.ZIP_CODE);
            // TODO: this is too long to be tested this way, generates an OOM exception
            //.PropertyHasMaxStringLength(
            //    x => x.Notes,
            //    int.MaxValue);
        }

        [TestMethod]
        public void Test_PMATOverride_IsRequired_WhenBRBWorkDescription()
        {
            foreach (var id in Enum.GetValues(typeof(WorkDescription.Indices)))
            {
                _viewModel.WorkDescription = (int)id;
                _viewModel.SendToSAP = true;
                if (WorkDescription.BRB_PMAT_DESCRIPTIONS.Contains((int)id))
                    ValidationAssert.ModelStateHasError(x => x.PlantMaintenanceActivityTypeOverride,
                        WorkOrderViewModel.ErrorMessages.BRB_PLANT_MAINTENANCE_ACTIVITY_CODE);
            }

            foreach (var id in Enum.GetValues(typeof(WorkDescription.Indices)))
            {
                _viewModel.WorkDescription = (int)id;
                _viewModel.SendToSAP = false;
                if (WorkDescription.BRB_PMAT_DESCRIPTIONS.Contains((int)id))
                    ValidationAssert.ModelStateIsValid(x => x.PlantMaintenanceActivityTypeOverride);
            }
        }
        
        [TestMethod]
        public void TestPlannedCompletionDateErrorsForEmergencyPriorityWorkOrdersForYesterday()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            _viewModel.Priority = (int?)WorkOrderPriority.Indices.EMERGENCY;
            _viewModel.PlannedCompletionDate = now.AddDays(-1);

            ValidationAssert.ModelStateHasError(x => x.PlannedCompletionDate,
                CreateWorkOrder.PLANNED_COMPLETION_DATE_ERROR_MESSAGE);
        }

        [TestMethod]
        public void TestPlannedCompletionDateDoesNotErrorForEmergencyPriorityWorkOrdersForToday()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            _viewModel.Priority = (int?)WorkOrderPriority.Indices.EMERGENCY;
            _viewModel.PlannedCompletionDate = now;

            ValidationAssert.ModelStateIsValid(x => x.PlannedCompletionDate);
        }

        [TestMethod]
        public void TestPlannedCompletionDateErrorsForNonEmergencyWorkOrdersForTomorrow()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            _viewModel.PlannedCompletionDate = now.AddDays(1);

            ValidationAssert.ModelStateHasError(x => x.PlannedCompletionDate,
                CreateWorkOrder.PLANNED_COMPLETION_DATE_ERROR_MESSAGE);
        }

        [TestMethod]
        public void TestPlannedCompletionDateHasNoErrorsForNonEmergencyWorkOrdersForTwoDaysFromNow()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            _viewModel.PlannedCompletionDate = now.AddDays(2);

            ValidationAssert.ModelStateIsValid(x => x.PlannedCompletionDate);
        }

        public void TestMapToEntitySetsPremiseWhenMatchingPremiseExists()
        {
            var premiseNumber = "12345678";
            long deviceLocation = 554785477;
            long installation = 887542541;
            var premise = GetFactory<PremiseFactory>().Create(new {PremiseNumber = premiseNumber, DeviceLocation = deviceLocation.ToString(), Installation = installation.ToString()});

            _viewModel.PremiseNumber = premiseNumber;
            _viewModel.DeviceLocation = deviceLocation;
            _viewModel.Installation = installation;
            
            var result = _viewModel.MapToEntity(_entity);
            
            Assert.IsNotNull(result.Premise);
            Assert.AreEqual(premise.Id, result.Premise.Id);
        }
        
        [TestMethod]
        public void TestMapToEntityDoesNotSetPremiseWhenNoMatchingPremiseExists()
        {
            var premiseNumber = "12345678";
            long deviceLocation = 554785477;
            long installation = 887542541;
            var premise = GetFactory<PremiseFactory>().Create(new {PremiseNumber = premiseNumber, DeviceLocation = deviceLocation.ToString(), Installation = installation.ToString()});

            _viewModel.PremiseNumber = "123456789";
            _viewModel.DeviceLocation = deviceLocation;
            _viewModel.Installation = installation;
            
            var result = _viewModel.MapToEntity(_entity);
            
            Assert.IsNull(result.Premise);
        }
        
        #endregion

        #endregion
    }
}
