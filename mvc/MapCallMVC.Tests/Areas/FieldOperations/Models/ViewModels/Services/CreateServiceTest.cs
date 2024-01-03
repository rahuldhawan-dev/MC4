using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.Services
{
    [TestClass]
    public class CreateServiceTest : ServiceViewModelTest<CreateService>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<ICoordinateRepository>().Use<CoordinateRepository>();
        }

        protected override Service CreateEntity()
        {
            return GetEntityFactory<Service>().Create(new {
                ServiceNumber = (long?)null,
                OperatingCenter = _operatingCenter = GetEntityFactory<OperatingCenter>().Create()
            });
        }

        #endregion

        #region Tests

        #region CanMapBothWays

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();

            _vmTester.CanMapBothWays(x => x.ServiceNumber);
        }

        [TestMethod]
        public void TestBackflowDeviceCanMapBothWays()
        {
            var bd = GetEntityFactory<BackflowDevice>().Create(new { Description = "Foo" });
            _entity.BackflowDevice = bd;

            _vmTester.MapToViewModel();

            Assert.AreEqual(bd.Id, _viewModel.BackflowDevice);

            _entity.BackflowDevice = null;
            _vmTester.MapToEntity();

            Assert.AreSame(bd, _entity.BackflowDevice);
        }

        [TestMethod]
        public void TestCoordinateCanMapBothWays()
        {
            var coord = GetEntityFactory<Coordinate>().Create();
            _entity.Coordinate = coord;

            _vmTester.MapToViewModel();

            Assert.AreEqual(coord.Id, _viewModel.Coordinate);

            _entity.Coordinate = null;
            _vmTester.MapToEntity();

            Assert.AreSame(coord, _entity.Coordinate);
        }

        [TestMethod]
        public void TestCrossStreetCanMapBothWays()
        {
            var street = GetEntityFactory<Street>().Create();
            _entity.CrossStreet = street;

            _vmTester.MapToViewModel();

            Assert.AreEqual(street.Id, _viewModel.CrossStreet);

            _entity.CrossStreet = null;
            _vmTester.MapToEntity();

            Assert.AreSame(street, _entity.CrossStreet);
        }

        [TestMethod]
        public void TestServiceStatusCanMapBothWays()
        {
            var ServiceStatus = GetEntityFactory<ServiceStatus>().Create(new { Description = "Foo" });
            _entity.ServiceStatus = ServiceStatus;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ServiceStatus.Id, _viewModel.ServiceStatus);

            _entity.ServiceStatus = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ServiceStatus, _entity.ServiceStatus);
        }

        [TestMethod]
        public void TestMainSizeCanMapBothWays()
        {
            var size = GetEntityFactory<ServiceSize>().Create(new { Description = "Foo" });
            _entity.MainSize = size;

            _vmTester.MapToViewModel();

            Assert.AreEqual(size.Id, _viewModel.MainSize);

            _entity.MainSize = null;
            _vmTester.MapToEntity();

            Assert.AreSame(size, _entity.MainSize);
        }

        [TestMethod]
        public void TestMainTypeCanMapBothWays()
        {
            var mt = GetEntityFactory<MainType>().Create(new { Description = "Foo" });
            _entity.MainType = mt;

            _vmTester.MapToViewModel();

            Assert.AreEqual(mt.Id, _viewModel.MainType);

            _entity.MainType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(mt, _entity.MainType);
        }

        [TestMethod]
        public void TestMeterSettingSizeCanMapBothWays()
        {
            var size = GetEntityFactory<ServiceSize>().Create(new { Description = "Foo" });
            _entity.MeterSettingSize = size;

            _vmTester.MapToViewModel();

            Assert.AreEqual(size.Id, _viewModel.MeterSettingSize);

            _entity.MeterSettingSize = null;
            _vmTester.MapToEntity();

            Assert.AreSame(size, _entity.MeterSettingSize);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var oc = GetEntityFactory<OperatingCenter>().Create();
            _entity.OperatingCenter = oc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(oc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(oc, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestPreviousServiceMaterialCanMapBothWays()
        {
            var sm = GetEntityFactory<ServiceMaterial>().Create(new { Description = "Foo" });
            _entity.PreviousServiceMaterial = sm;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sm.Id, _viewModel.PreviousServiceMaterial);

            _entity.PreviousServiceMaterial = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sm, _entity.PreviousServiceMaterial);
        }

        [TestMethod]
        public void TestPreviousServiceSizeCanMapBothWays()
        {
            var size = GetEntityFactory<ServiceSize>().Create(new { Description = "Foo" });
            _entity.PreviousServiceSize = size;

            _vmTester.MapToViewModel();

            Assert.AreEqual(size.Id, _viewModel.PreviousServiceSize);

            _entity.PreviousServiceSize = null;
            _vmTester.MapToEntity();

            Assert.AreSame(size, _entity.PreviousServiceSize);
        }

        [TestMethod]
        public void TestServiceInstallationPurposeCanMapBothWays()
        {
            var sip = GetEntityFactory<ServiceInstallationPurpose>().Create(new { Description = "Foo" });
            _entity.ServiceInstallationPurpose = sip;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sip.Id, _viewModel.ServiceInstallationPurpose);

            _entity.ServiceInstallationPurpose = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sip, _entity.ServiceInstallationPurpose);
        }

        [TestMethod]
        public void TestServiceCategoryCanMapBothWays()
        {
            var sc = GetEntityFactory<ServiceCategory>().Create(new { Description = "Foo" });
            _entity.ServiceCategory = sc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sc.Id, _viewModel.ServiceCategory);

            _entity.ServiceCategory = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sc, _entity.ServiceCategory);
        }

        [TestMethod]
        public void TestServiceMaterialCanMapBothWays()
        {
            var mat = GetEntityFactory<ServiceMaterial>().Create(new { Description = "Foo" });
            _entity.ServiceMaterial = mat;

            _vmTester.MapToViewModel();

            Assert.AreEqual(mat.Id, _viewModel.ServiceMaterial);

            _entity.ServiceMaterial = null;
            _vmTester.MapToEntity();

            Assert.AreSame(mat, _entity.ServiceMaterial);
        }

        [TestMethod]
        public void TestServicePriorityCanMapBothWays()
        {
            var sp = GetEntityFactory<ServicePriority>().Create(new { Description = "Foo" });
            _entity.ServicePriority = sp;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sp.Id, _viewModel.ServicePriority);

            _entity.ServicePriority = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sp, _entity.ServicePriority);
        }

        [TestMethod]
        public void TestServiceSizeCanMapBothWays()
        {
            var size = GetEntityFactory<ServiceSize>().Create(new { Description = "Foo" });
            _entity.ServiceSize = size;

            _vmTester.MapToViewModel();

            Assert.AreEqual(size.Id, _viewModel.ServiceSize);

            _entity.ServiceSize = null;
            _vmTester.MapToEntity();

            Assert.AreSame(size, _entity.ServiceSize);
        }

        [TestMethod]
        public void TestStateCanMapBothWays()
        {
            var state = GetEntityFactory<State>().Create(new { Description = "Foo" });
            _entity.State = state;

            _vmTester.MapToViewModel();

            Assert.AreEqual(state.Id, _viewModel.State);

            _entity.State = null;
            _vmTester.MapToEntity();

            Assert.AreSame(state, _entity.State);
        }

        [TestMethod]
        public void TestStreetCanMapBothWays()
        {
            var street = GetEntityFactory<Street>().Create();
            _entity.Street = street;

            _vmTester.MapToViewModel();

            Assert.AreEqual(street.Id, _viewModel.Street);

            _entity.Street = null;
            _vmTester.MapToEntity();

            Assert.AreSame(street, _entity.Street);
        }

        [TestMethod]
        public void TestStreetMaterialCanMapBothWays()
        {
            var sm = GetEntityFactory<StreetMaterial>().Create(new { Description = "Foo" });
            _entity.StreetMaterial = sm;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sm.Id, _viewModel.StreetMaterial);

            _entity.StreetMaterial = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sm, _entity.StreetMaterial);
        }

        [TestMethod]
        public void TestTownCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create();
            _entity.Town = town;

            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();

            Assert.AreSame(town, _entity.Town);
        }

        [TestMethod]
        public void TestTownSectionCanMapBothWays()
        {
            var ts = GetEntityFactory<TownSection>().Create(new { Name = "Foo" });
            _entity.TownSection = ts;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ts.Id, _viewModel.TownSection);

            _entity.TownSection = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ts, _entity.TownSection);
        }
        
        #endregion

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            base.TestEntityMustExistValidation();

            ValidationAssert
               .EntityMustExist<OperatingCenter>(x => x.OperatingCenter)
               .EntityMustExist<Street>(x => x.Street)
               .EntityMustExist<Service>(x => x.RenewalOf)
               .EntityMustExist<ServiceMaterial>(x => x.ServiceMaterial)
               .EntityMustExist<WorkOrder>(x => x.WorkOrder)
               .EntityMustExist<TownSection>(x => x.TownSection);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();

            ValidationAssert.PropertyIsRequired(x => x.OperatingCenter);
        }

        #endregion

        [TestMethod]
        public void TestConstructorWithWorkOrderSetsAppropriateValuesOnViewModel()
        {
            var defaultMapIcon = GetFactory<DefaultMapIconFactory>().Create();
            var workOrder = GetFactory<WorkOrderFactory>().Create(new {
                TownSection = typeof(TownSectionFactory),
                Street = typeof(StreetFactory),
                NearestCrossStreet = typeof(StreetFactory),
                ServiceNumber = "12345",
                Installation = (long)54321,
                WorkDescription = GetFactory<ServiceLineInstallationWorkDescriptionFactory>().Create(),
                CompanyServiceLineMaterial = GetFactory<ServiceMaterialFactory>().Create(),
                CustomerServiceLineMaterial = GetFactory<ServiceMaterialFactory>().Create(),
                CompanyServiceLineSize = GetFactory<ServiceSizeFactory>().Create(),
                CustomerServiceLineSize = GetFactory<ServiceSizeFactory>().Create()
            });
            var target = new CreateService(_container, workOrder);

            Assert.AreEqual(workOrder.Id, target.WorkOrder);
            Assert.AreEqual(workOrder.OperatingCenter.Id, target.OperatingCenter);
            Assert.AreEqual(workOrder.Town.Id, target.Town);
            Assert.AreEqual(workOrder.TownSection.Id, target.TownSection);
            Assert.AreEqual(workOrder.StreetNumber, target.StreetNumber);
            Assert.AreEqual(workOrder.Street.Id, target.Street);
            Assert.AreEqual(workOrder.NearestCrossStreet.Id, target.CrossStreet);
            Assert.AreEqual(workOrder.ZipCode, target.Zip);
            Assert.AreEqual(workOrder.PremiseNumber, target.PremiseNumber);
            Assert.AreEqual(workOrder.SAPNotificationNumber, target.SAPNotificationNumber);
            Assert.AreEqual(workOrder.SAPWorkOrderNumber, target.SAPWorkOrderNumber);
            Assert.AreEqual(workOrder.Town.State.Id, target.State);
            Assert.AreEqual(workOrder.ServiceNumber, target.ServiceNumber.ToString());
            Assert.AreEqual(workOrder.Installation.ToString(), target.Installation);
            Assert.IsTrue(target.IsExistingOrRenewal);

            // Assert the coordinate is cloned
            Assert.AreNotEqual(workOrder.Coordinate.Id, target.Coordinate);
            Assert.AreNotEqual(0, target.Coordinate);

            Assert.AreEqual(workOrder.CompanyServiceLineMaterial.Id, target.ServiceMaterial);
            Assert.AreEqual(workOrder.CompanyServiceLineSize.Id, target.ServiceSize);
            Assert.AreEqual(workOrder.CustomerServiceLineMaterial.Id, target.CustomerSideMaterial);
            Assert.AreEqual(workOrder.CustomerServiceLineSize.Id, target.CustomerSideSize);
            Assert.AreEqual(ServiceInstallationPurpose.Indices.NEW_SERVICE, target.ServiceInstallationPurpose);
            Assert.AreEqual((int)ServicePriority.Indices.ROUTINE, target.ServicePriority);
            Assert.AreEqual(ServiceCategory.Indices.WATER_SERVICE_NEW_DOMESTIC, target.ServiceCategory);
        }

        [TestMethod]
        public void TestConstructorWithWorkOrderHandlesNullWorkOrderPropertiesCorrectly()
        {
            var defaultMapIcon = GetFactory<DefaultMapIconFactory>().Create();
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            workOrder.Street = null; // This is nullable in the db even though no records have a null street.
            workOrder.NearestCrossStreet = null; // This is actually nullable and null in the db
            var target = new CreateService(_container, workOrder);

            Assert.IsNull(workOrder.TownSection);
            Assert.IsNull(target.TownSection);
            Assert.IsNull(workOrder.Street);
            Assert.IsNull(target.Street);
            Assert.IsNull(workOrder.NearestCrossStreet);
            Assert.IsNull(target.CrossStreet);
            Assert.IsNull(workOrder.ServiceNumber);
            Assert.IsNull(target.ServiceNumber);
        }

        [TestMethod]
        public void TestMapToEntitySetsInitiatorToCurrentUser()
        {
            _entity.Initiator = null;
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.Initiator);
        }

        [TestMethod]
        public void TestMapToEntitySetsIsActive()
        {
            var expected = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expected);

            _vmTester.MapToEntity();
            Assert.AreEqual(true, _entity.IsActive);
        }
        
        [TestMethod]
        public void TestMapToEntitySetsNeedsToSyncToTrue()
        {
            var serviceUtilityType = GetEntityFactory<ServiceUtilityType>().Create();
            var serviceCategory = GetEntityFactory<ServiceCategory>().Create(new { ServiceUtilityType = serviceUtilityType });
            var premise = GetEntityFactory<Premise>().Create(new {
                Installation = "123456789",
                PremiseNumber = "9100327803",
                ServiceUtilityType = serviceUtilityType
            });
            _viewModel.Installation = premise.Installation;
            _viewModel.PremiseNumber = premise.PremiseNumber;
            _viewModel.ServiceCategory = serviceCategory.Id;

            _entity.NeedsToSync = false;

            _vmTester.MapToEntity();
            Assert.IsTrue(_entity.NeedsToSync);
        }
        
        [TestMethod]
        public void TestMapToEntityDoesNotSetNeedsToSyncToTrueWhenPremiseIsNull()
        {
            _entity.Premise = null;
            _entity.NeedsToSync = false;
            _vmTester.MapToEntity();
            Assert.IsFalse(_entity.NeedsToSync);
        }

        [TestMethod]
        public void TestSetDefaultsSetsDefaultsForRenewals()
        {
            _user.IsAdmin = true;
            var date = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);
            var opcntr = GetEntityFactory<OperatingCenter>().Create();
            var serviceCategory = GetEntityFactory<ServiceCategory>().Create();
            var street = GetEntityFactory<Street>().Create();
            var crossStreet = GetEntityFactory<Street>().Create();
            var serviceType = GetEntityFactory<ServiceType>().Create(new { OperatingCenter = opcntr, ServiceCategory = serviceCategory, Description = "Blergh" });
            var serviceSize = GetEntityFactory<ServiceSize>().Create();
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create();
            var state = GetEntityFactory<State>().Create();
            var town = GetEntityFactory<Town>().Create();
            var service = GetFactory<ServiceFactory>().Create(new
            {
                OperatingCenter = opcntr,
                Street = street,
                CrossStreet = crossStreet,
                ServiceType = serviceType,
                LengthOfService = 1m,
                ServiceSize = serviceSize,
                ServiceMaterial = serviceMaterial,
                State = state, 
                Town = town
            });
            _viewModel.RenewalOf = service.Id;
            _viewModel.Copy = null;

            _viewModel.SetDefaults();

            Assert.AreEqual(service.OperatingCenter.Id, _viewModel.OperatingCenter);
            Assert.AreEqual(service.PremiseNumber, _viewModel.PremiseNumber);
            Assert.AreEqual(service.StreetNumber, _viewModel.StreetNumber);
            Assert.AreEqual(service.Street.Id, _viewModel.Street);
            Assert.AreEqual(service.Town.Id, _viewModel.Town);
            Assert.AreEqual(service.State.Id, _viewModel.State);
            Assert.AreEqual(service.Zip, _viewModel.Zip);
            Assert.AreEqual(service.CrossStreet.Id, _viewModel.CrossStreet);
            Assert.AreEqual(service.ServiceNumber, _viewModel.ServiceNumber);
            Assert.IsTrue(_viewModel.IsExistingOrRenewal, "This should always be true for renewals.");
            Assert.AreEqual(date, _viewModel.ContactDate);
        }

        [TestMethod]
        public void TestSetDefaultsSetsDefaultsWithCopyTrueCorrectly()
        {
            _user.IsAdmin = true;
            var date = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);
            var opcntr = GetEntityFactory<OperatingCenter>().Create();
            var serviceCategory = GetEntityFactory<ServiceCategory>().Create();
            var street = GetEntityFactory<Street>().Create();
            var crossStreet = GetEntityFactory<Street>().Create();
            var serviceType = GetEntityFactory<ServiceType>().Create(new { OperatingCenter = opcntr, ServiceCategory = serviceCategory, Description = "Blergh" });
            var serviceSize = GetEntityFactory<ServiceSize>().Create();
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create();
            var state = GetEntityFactory<State>().Create();
            var town = GetEntityFactory<Town>().Create();
            var service = GetFactory<ServiceFactory>().Create(new
            {
                OperatingCenter = opcntr,
                Street = street,
                CrossStreet = crossStreet,
                ServiceType = serviceType,
                LengthOfService = 1m,
                ServiceSize = serviceSize,
                ServiceMaterial = serviceMaterial,
                State = state,
                Town = town,
                DeviceLocation = "1231"
            });
            _viewModel = _viewModelFactory.BuildWithOverrides<CreateService, Service>(service, new { Copy = true });

            _viewModel.SetDefaults();

            Assert.AreEqual(service.OperatingCenter.Id, _viewModel.OperatingCenter);
            Assert.IsNull( _viewModel.PremiseNumber);
            Assert.IsNull(_viewModel.ServiceNumber);
            Assert.AreEqual(service.StreetNumber, _viewModel.StreetNumber);
            Assert.AreEqual(service.Street.Id, _viewModel.Street);
            Assert.AreEqual(service.Town.Id, _viewModel.Town);
            Assert.AreEqual(service.State.Id, _viewModel.State);
            Assert.AreEqual(service.Zip, _viewModel.Zip);
            Assert.AreEqual(service.CrossStreet.Id, _viewModel.CrossStreet);
            Assert.IsNull(_viewModel.DeviceLocation);
        }

        [TestMethod]
        public void TestSetDefaultsSetsDefaultsWithCopyTrueCorrectlyForSewer()
        {
            _user.IsAdmin = true;
            var date = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);
            var opcntr = GetEntityFactory<OperatingCenter>().Create();
            var serviceCategories = GetEntityFactory<ServiceCategory>().CreateList(50);
            var street = GetEntityFactory<Street>().Create();
            var crossStreet = GetEntityFactory<Street>().Create();
            var serviceSize = GetEntityFactory<ServiceSize>().Create();
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create();
            var state = GetEntityFactory<State>().Create();
            var town = GetEntityFactory<Town>().Create();

            foreach (var category in serviceCategories)
            {
                var serviceType = GetEntityFactory<ServiceType>().Create(new { OperatingCenter = opcntr, ServiceCategory = category, Description = "Blergh" });
                var service = GetFactory<ServiceFactory>().Create(new {
                    ServiceCategory = category,
                    OperatingCenter = opcntr,
                    Street = street,
                    CrossStreet = crossStreet,
                    ServiceType = serviceType,
                    LengthOfService = 1m,
                    ServiceSize = serviceSize,
                    ServiceMaterial = serviceMaterial,
                    State = state,
                    Town = town,
                    DeviceLocation = "1231"
                });
                _viewModel = _viewModelFactory.BuildWithOverrides<CreateService, Service>(service, new { Copy = true });

                _viewModel.SetDefaults();

                Assert.AreEqual(service.OperatingCenter.Id, _viewModel.OperatingCenter);
                Assert.IsNull(_viewModel.PremiseNumber);
                Assert.IsNull(_viewModel.ServiceNumber);
                Assert.AreEqual(service.StreetNumber, _viewModel.StreetNumber);
                Assert.AreEqual(service.Street.Id, _viewModel.Street);
                Assert.AreEqual(service.Town.Id, _viewModel.Town);
                Assert.AreEqual(service.State.Id, _viewModel.State);
                Assert.AreEqual(service.Zip, _viewModel.Zip);
                Assert.AreEqual(service.CrossStreet.Id, _viewModel.CrossStreet);
                Assert.IsNull(_viewModel.DeviceLocation);
            }
        }

        #region Mapping

        [TestMethod]
        public void TestMapToEntitySetsSendServiceWithSampleSitesToTrue()
        {
            var premise = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = "90001"
            });

            var service = GetEntityFactory<Service>().Create(new {
                premise.PremiseNumber,
                Premise = premise,
                Installation = "90001", 
                ServiceCategory = GetEntityFactory<ServiceCategory>().Create()
            });

            GetEntityFactory<SampleSite>().Create(new {
                Premise = premise,
                Status = GetFactory<ActiveSampleSiteStatusFactory>().Create()
            });

            Session.Evict(service);
            service = Session.Load<Service>(service.Id);

            _viewModel = _viewModelFactory.Build<CreateService, Service>(service);
            Assert.IsFalse(_viewModel.SendServiceWithSampleSitesNotificationOnSave);

            _viewModel.MapToEntity(service);

            Assert.IsTrue(_viewModel.HasLinkedSampleSiteByInstallation);
            Assert.IsTrue(_viewModel.SendServiceWithSampleSitesNotificationOnSave);
        }

        [TestMethod]
        public void TestMapToEntitySetsServiceNumberToNextAvailableServiceNumber()
        {
            var service =
                GetEntityFactory<Service>().Create(new {ServiceNumber = (long?)400, OperatingCenter = _operatingCenter});

            _vmTester.MapToEntity();

            Assert.AreEqual(401, _entity.ServiceNumber);
        }

        [TestMethod]
        public void TestMapToEntityAddsWorkOrderToWorkOrdersCollection()
        {
            var wo = GetFactory<WorkOrderFactory>().Create();
            _viewModel.WorkOrder = wo.Id;
            _user.IsAdmin = true;

            _vmTester.MapToEntity();
            Assert.IsTrue(_entity.WorkOrders.Contains(wo));
        }

        #endregion

        #region Validation

        //[TestMethod]
        //public void TestValidationDoesNotAllowAllZeroPremiseNumber()
        //{
        //    _viewModel.PremiseNumber = "0000000000";

        //    ValidationAssert.ModelStateHasError(_viewModel, x => x.PremiseNumber, ServiceViewModel.ErrorMessages.SERVICE_NUMBER_NOT_ZEROS);
        //}

        [TestMethod]
        public void TestWorkIssuedToCanMapBothWays()
        {
            var opc1 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "FOO" });
            var src = GetEntityFactory<ServiceRestorationContractor>().Create(new {
                OperatingCenter = opc1,
                Contractor = "Buh?",
                FinalRestoration = true,
                PartialRestoration = true
            });

            _entity.WorkIssuedTo = src; 

            _vmTester.MapToViewModel();

            Assert.AreEqual(src.Id, _viewModel.WorkIssuedTo);

            _entity.WorkIssuedTo = null;
            _vmTester.MapToEntity();

            Assert.AreSame(src, _entity.WorkIssuedTo);
        }

        #endregion
    }
}
