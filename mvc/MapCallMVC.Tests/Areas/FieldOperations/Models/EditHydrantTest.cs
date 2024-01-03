using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class EditHydrantTest : MapCallMvcInMemoryDatabaseTestBase<Hydrant>
    {
        #region Fields

        private ViewModelTester<EditHydrant, Hydrant> _vmTester;
        private EditHydrant _viewModel;
        private Hydrant _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IHydrantRepository>().Use<HydrantRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IFunctionalLocationRepository>().Use<FunctionalLocationRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
            e.For<IFireDistrictRepository>().Use<FireDistrictRepository>();
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            e.For<ITownSectionRepository>().Use<TownSectionRepository>();
            e.For<IAbbreviationTypeRepository>().Use<AbbreviationTypeRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new {
                IsAdmin = true
            });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<Hydrant>().Create();
            _viewModel = _viewModelFactory.Build<EditHydrant, Hydrant>( _entity);
            _vmTester = new ViewModelTester<EditHydrant, Hydrant>(_viewModel, _entity);
            GetFactory<AssetStatusFactory>().CreateAll();
            GetFactory<HydrantBillingFactory>().CreateAll();
            // These need to exist
            GetFactory<TownSectionAbbreviationTypeFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestDisplayHydrantReturnsOriginalHydrant()
        {
            var entity = GetFactory<HydrantFactory>().Create();
            var vm = _viewModelFactory.Build<EditHydrant, Hydrant>(entity);
            Assert.AreSame(entity, vm.DisplayHydrant);
        }

        [TestMethod]
        public void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.HydrantNumber);
            _vmTester.CanMapBothWays(x => x.PaintingZone);
            _vmTester.CanMapBothWays(x => x.PaintingFrequency);
            //_vmTester.CanMapBothWays(x => x.HydrantSuffix);
        }
        
        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Street);
        }

        [TestMethod]
        public void TestStreetCanMapBothWays()
        {
            var street = GetEntityFactory<Street>().Create(new { Description = "Foo" });
            _entity.Street = street;

            _vmTester.MapToViewModel();

            Assert.AreEqual(street.Id, _viewModel.Street);

            _entity.Street = null;
            _vmTester.MapToEntity();

            Assert.AreSame(street, _entity.Street);
        }

        [TestMethod]
        public void TestCrossStreetCanMapBothWays()
        {
            var crossStreet = GetEntityFactory<Street>().Create(new { Description = "Foo" });
            _entity.CrossStreet = crossStreet;

            _vmTester.MapToViewModel();

            Assert.AreEqual(crossStreet.Id, _viewModel.CrossStreet);

            _entity.CrossStreet = null;
            _vmTester.MapToEntity();

            Assert.AreSame(crossStreet, _entity.CrossStreet);
        }

        [TestMethod]
        public void TestStringLengthValidationForHydrantNumber()
        {
            _viewModel.HydrantNumber = "VAB-1AAAAAAA";
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.HydrantNumber, Hydrant.StringLengths.HYDRANT_NUMBER, true);
        }

        [TestMethod]
        public void TestValidationFailsIfHydrantNumberIsNotUniqueToOperatingCenter()
        {
            var existingHydrant = GetFactory<HydrantFactory>().Create(new { HydrantNumber = "WHOO!", OperatingCenter = _entity.OperatingCenter });
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HydrantNumber);

            _viewModel.HydrantNumber = existingHydrant.HydrantNumber;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.HydrantNumber, "A hydrant already exists for this operating center with the given hydrant number.");
        }

        [TestMethod]
        public void TestValidationFailsIfMultipleRecordsInDatabaseHaveTheSameHydrantNumberForTheEditedHydrant()
        {
            // I can't word that better in test name form. This is testing if a user tries to update an existing hydrant,
            // leaving the hydrant number intact, but it turns out it's not actually unique in the database because someone
            // entered bad data back in the day. They're required to fix their bad data.
            var activeHydrantStatus = GetFactory<ActiveAssetStatusFactory>().Create();

            var existingHydrant = GetFactory<HydrantFactory>().Create(new { HydrantNumber = "HAB-101", OperatingCenter = _entity.OperatingCenter, HydrantSuffix = 101,Status = activeHydrantStatus });
            var existingHydrant2 = GetFactory<HydrantFactory>().Create(new { HydrantNumber = "HAB-101", OperatingCenter = _entity.OperatingCenter, HydrantSuffix = 101,Status = activeHydrantStatus });

            _viewModel.HydrantNumber = "HAB-101";
            ValidationAssert.ModelStateHasError(_viewModel, x => x.HydrantNumber, EditHydrant.ERROR_HYDRANT_NUMBER_ALREADY_USED);
        }

        [TestMethod]
        public void TestValidationDoesNotFailIfMultipleRecordsInDatabaseHaveTheSameHydrantNumberForTheEditedHydrantButOneIsNotActive()
        {
            var retiredHydrantStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            var inActiveHydrantStatus = GetFactory<PendingAssetStatusFactory>().Create();


            var existingHydrant = GetFactory<HydrantFactory>().Create(new { HydrantNumber = "HAB-101", OperatingCenter = _entity.OperatingCenter, HydrantSuffix = 101,Status = retiredHydrantStatus });
            var existingHydrant2 = GetFactory<HydrantFactory>().Create(new { HydrantNumber = "HAB-101", OperatingCenter = _entity.OperatingCenter, HydrantSuffix = 101,Status = inActiveHydrantStatus });

            _viewModel.HydrantNumber = "HAB-101";
            _viewModel.HydrantSuffix = 101;
            ValidationAssert.ModelStateIsValid(_viewModel);
        }

        [TestMethod]
        public void TestValidationFailsIfHydrantSuffixIsNotWithinHydrantNumber()
        {
            var hyd = GetFactory<HydrantFactory>().Create(new { HydrantNumber = "HAB-15A", HydrantSuffix = 15 });
            var target = _viewModelFactory.BuildWithOverrides<EditHydrant, Hydrant>(hyd, new { HydrantSuffix = 16 });

            ValidationAssert.ModelStateHasError(target, x => x.HydrantNumber, Hydrant.HYDRANT_NUMBER_PATTERN_ERROR);

            target.HydrantNumber = "HAB-155A";
            target.HydrantSuffix = 15;
            ValidationAssert.ModelStateHasError(target, x => x.HydrantNumber, Hydrant.HYDRANT_NUMBER_PATTERN_ERROR);

            target.HydrantNumber = "HAB-16A15";
            ValidationAssert.ModelStateHasError(target, x => x.HydrantNumber, Hydrant.HYDRANT_NUMBER_PATTERN_ERROR);

            target.HydrantNumber = "HAB-15A";
            target.HydrantSuffix = 15;
            ValidationAssert.ModelStateIsValid(target);

            target.HydrantNumber = "HAB-15";
            ValidationAssert.ModelStateIsValid(target);
        }

        [TestMethod]
        public void TestSAPFunctionalLocationValidation()
        {
            // This is required only when the selected operating center's IsContractedOperations == false.
            var opcWithContractedOps = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = true, SAPEnabled = true });
            var opcWithoutContractedOps = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = false, SAPEnabled = true });
            var functionalLocation = GetFactory<FunctionalLocationFactory>().Create();

            _viewModel.FunctionalLocation = null;
            _entity.OperatingCenter = opcWithContractedOps;
            //_viewModel.OperatingCenter = opcWithContractedOps.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FunctionalLocation);

            _entity.OperatingCenter = opcWithoutContractedOps;
            //_viewModel.OperatingCenter = opcWithoutContractedOps.Id;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FunctionalLocation, "The Functional Location field is required.", true);

            _viewModel.FunctionalLocation = functionalLocation.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FunctionalLocation);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledFalse()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPTrueWhenOperatingCenterSAPEnabledTrue()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;

            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledTrueAndContractedOps()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPToFalseIfTheExistingStatusIsCancelledAndTheStatusItNotBeingChanged()
        {
            // This same test should work for Cancelled, Retired, and Removed.
            var statuses = new[] { AssetStatus.Indices.CANCELLED, AssetStatus.Indices.RETIRED, AssetStatus.Indices.REMOVED };

            foreach (var status in statuses)
            {
                var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
                _entity.Town = town;
                _entity.OperatingCenter = opc1;
                _entity.Status = _container.GetInstance<IRepository<AssetStatus>>().Find(status);
                _viewModel.Status = status;

                _vmTester.MapToEntity();

                Assert.IsFalse(_viewModel.SendToSAP);
            }
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPToTrueIfTheExistingStatusWasCancelledAndTheStatusIsChanged()
        {
            // This same test should work for Cancelled, Retired, and Removed.
            var statuses = new[] { AssetStatus.Indices.CANCELLED, AssetStatus.Indices.RETIRED, AssetStatus.Indices.REMOVED };

            foreach (var status in statuses)
            {
                var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
                _entity.Town = town;
                _entity.OperatingCenter = opc1;
                _entity.Status = _container.GetInstance<IRepository<AssetStatus>>().Find(status);
                _viewModel.Status = AssetStatus.Indices.ACTIVE;

                _vmTester.MapToEntity();

                Assert.IsTrue(_viewModel.SendToSAP);
            }
        }

        [TestMethod]
        public void TestMapToEntityCancelsAnyAttachedWorkOrders()
        {
            var now = DateTime.Now;
            var reason = GetEntityFactory<WorkOrderCancellationReason>()
               .Create(new {Status = "ARET", Description = "Asset Retired"});
            _entity.WorkOrders = GetFactory<WorkOrderFactory>().CreateList(2);
            _viewModel.DateRetired = now;

            _vmTester.MapToEntity();

            foreach (var workOrder in _entity.WorkOrders)
            {
                Assert.AreEqual(now, workOrder.CancelledAt);
                Assert.AreEqual(reason, workOrder.WorkOrderCancellationReason);
            }
        }

        [TestMethod]
        public void TestMapToEntityDoNotCancelsAnyAttachedWorkOrdersWithMaterials()
        {
            var now = DateTime.Now;
            var reason = GetEntityFactory<WorkOrderCancellationReason>()
               .Create(new { Status = "ARET", Description = "Asset Retired" });
            _entity.WorkOrders = GetFactory<WorkOrderFactory>().CreateList(2, new {
                AssignedToContractorOn = now,
                AssignedContractor = GetFactory<ContractorFactory>().Create()
            });
            var wo3 = GetFactory<WorkOrderFactory>().Create(new {
                AssignedToContractorOn = now,
                AssignedContractor = GetFactory<ContractorFactory>().Create()
            });
            wo3.MaterialsUsed = GetEntityFactory<MaterialUsed>().CreateList(2, new {
                WorkOrder = wo3
            });
            _entity.WorkOrders.Add(wo3);
            _viewModel.DateRetired = now;

            _vmTester.MapToEntity();

            Assert.AreEqual(now, _entity.WorkOrders[0].CancelledAt);
            Assert.AreEqual(reason, _entity.WorkOrders[0].WorkOrderCancellationReason);
            Assert.IsNull(_entity.WorkOrders[0].AssignedToContractorOn);
            Assert.IsNull(_entity.WorkOrders[0].AssignedContractor);

            Assert.AreEqual(now, _entity.WorkOrders[1].CancelledAt);
            Assert.AreEqual(reason, _entity.WorkOrders[1].WorkOrderCancellationReason);
            Assert.IsNull(_entity.WorkOrders[1].AssignedToContractorOn);
            Assert.IsNull(_entity.WorkOrders[1].AssignedContractor);

            Assert.AreNotEqual(now, _entity.WorkOrders[2].CancelledAt);
            Assert.AreNotEqual(reason, _entity.WorkOrders[2].WorkOrderCancellationReason);
            Assert.IsNotNull(_entity.WorkOrders[2].AssignedToContractorOn);
            Assert.IsNotNull(_entity.WorkOrders[2].AssignedContractor);
        }

        [TestMethod]
        public void TestStringLengths()
        {
           ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.WorkOrderNumber, Hydrant.StringLengths.WORKORDER);
        }

        #endregion
    }
}
