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
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using AssetStatus = MapCall.Common.Model.Entities.AssetStatus;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class CreateHydrantTest : MapCallMvcInMemoryDatabaseTestBase<Hydrant>
    {
        #region Fields

        private ViewModelTester<CreateHydrant, Hydrant> _vmTester;
        private CreateHydrant _viewModel;
        private Hydrant _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private AssetStatus _activeStatus;
        private AssetStatus _retiredStatus;
        private AssetStatus _cancelledStatus;
        private AssetStatus _removedStatus;
        private HydrantBilling _publicBilling;
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
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<Hydrant>().Create();
            _viewModel = _viewModelFactory.BuildWithOverrides<CreateHydrant, Hydrant>(_entity, new {
                WorkOrderNumber = "work order number"
            });
            _vmTester = new ViewModelTester<CreateHydrant, Hydrant>(_viewModel, _entity);

            GetFactory<AssetStatusFactory>().CreateAll();

            _activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            _retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            _cancelledStatus = GetFactory<CancelledAssetStatusFactory>().Create();
            _removedStatus = GetFactory<RemovedAssetStatusFactory>().Create();

            _publicBilling = GetFactory<PublicHydrantBillingFactory>().Create();

            // This needs to exist
            GetFactory<TownSectionAbbreviationTypeFactory>().Create();
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestTownPropertyCanMapBothWays()
        {
            // Evict the entity because MapToEntity does a query and that
            // causes NHibernate to flush the session changes which is SO STUPID 
            // because then it tries to save Hydrant with a null town and throws
            // an exception.
            Session.Evict(_entity);
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;
            _viewModel.Town = null;
            _viewModel.OperatingCenter = null;
            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.Town);

            _entity.Town = null;
            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();
            Assert.AreSame(town, _entity.Town);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            // Evict the entity because MapToEntity does a query and that
            // causes NHibernate to flush the session changes which is SO STUPID 
            // because then it tries to save Hydrant with a null OperatingCenter and throws
            // an exception.

            Session.Evict(_entity);

            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;
            _viewModel.Town = null;
            _viewModel.OperatingCenter = null;


            _entity.OperatingCenter = opc1;

            _vmTester.MapToViewModel();

            Assert.AreEqual(opc1.Id, _viewModel.OperatingCenter);
            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(opc1, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestMapToEntitySetsHydrantNumberAndHydrantSuffixToNextGeneratedNumberFromRepository()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { OperatingCenterCode = "EW1" });
            var town = GetFactory<TownFactory>().Create(new { ShortName = "Edison" });
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "EDS" });
            var townSection = GetFactory<TownSectionFactory>().Create();

            _viewModel.OperatingCenter = opc1.Id;
            _viewModel.Town = town.Id;
            _viewModel.TownSection = townSection.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual("HEDS-1", _entity.HydrantNumber);
        }

        [TestMethod]
        public void TestMapToEntitySetsInitiatorToCurrentUser()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc1.Id;

            _entity.Initiator = null;
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.Initiator);
        }

        [TestMethod]
        public void TestMapToEntityThrowsExceptionIfTheGeneratedHydrantNumberIsNotUnique()
        {
            _user.IsAdmin = true;
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc, Abbreviation = "ZZ" });
            var differentTown = GetFactory<TownFactory>().Create();
            differentTown.OperatingCentersTowns.Add(new OperatingCenterTown { Town = differentTown, OperatingCenter = opc, Abbreviation = "ZZ" });

            var hydrant = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc, Town = town, HydrantNumber = "HZZ-1", HydrantSuffix = 1 });

            _viewModel.OperatingCenter = opc.Id;
            _viewModel.Town = differentTown.Id;

            MyAssert.Throws<InvalidOperationException>(() => _vmTester.MapToEntity());
        }

        [TestMethod]
        public void TestMapToEntityUsesHydrantSuffixWhenIsFoundHydrantIsTrue()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _viewModel.IsFoundHydrant = true;
            _viewModel.HydrantSuffix = 421;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc1.Id;

            _vmTester.MapToEntity();
            Assert.AreEqual("HZZ-421", _entity.HydrantNumber);
            Assert.AreEqual(421, _entity.HydrantSuffix);
        }


        #endregion

        #region Validation

        [TestMethod]
        public void TestOperatingCenterIsRequired()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            _viewModel.OperatingCenter = null;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
        }

        [TestMethod]
        public void TestTownIsRequired()
        {
            var town = GetFactory<TownFactory>().Create();

            _viewModel.Town = null;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);

            _viewModel.Town = town.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Town);
        }

        [TestMethod]
        public void TestHydrantSuffixIsRequiredWhenIsFoundHydrantIsTrue()
        {
            _viewModel.IsFoundHydrant = true;
            _viewModel.HydrantSuffix = null;

            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HydrantSuffix, "Hydrant Suffix is required for found hydrants.");

            _viewModel.IsFoundHydrant = false;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HydrantSuffix);
        }

        [TestMethod]
        public void TestHydrantSuffixForFoundHydrantMustNotExistAndBeLowerThanTheCurrentMaxHydrantNumberValue()
        {
            _user.IsAdmin = true;
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc, Abbreviation = "ZZ" });
            var hydrantOne = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc, Town = town, HydrantSuffix = 1, HydrantNumber = "HZZ-1", Status = _activeStatus });
            var hydrantThree = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc, Town = town, HydrantSuffix = 3, HydrantNumber = "HZZ-3", Status = _activeStatus });

            _viewModel.IsFoundHydrant = true;
            _viewModel.HydrantSuffix = 4;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc.Id;
            _viewModel.Status = _activeStatus.Id;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.HydrantSuffix, "A found hydrant can not have a suffix greater than the current maximum hydrant number for a given area.");

            _viewModel.HydrantSuffix = 3;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.HydrantSuffix, "A hydrant already exists with this hydrant suffix.");

            _viewModel.HydrantSuffix = 2;

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HydrantSuffix);
        }

        [TestMethod]
        public void TestValidationFailsIfIsFoundHydrantIsTrueButThereAreNoAvailableUnusedNumbers()
        {
            _user.IsAdmin = true;
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc, Abbreviation = "ZZ" });
            var hydrantOne = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc, Town = town, HydrantSuffix = 1, HydrantNumber = "HZZ-1", Status = _activeStatus });
            var hydrantTwo = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc, Town = town, HydrantSuffix = 3, HydrantNumber = "HZZ-2", Status = _activeStatus });

            _viewModel.IsFoundHydrant = true;
            _viewModel.HydrantSuffix = 2;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc.Id;
            _viewModel.Status = _activeStatus.Id;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.HydrantSuffix, "A hydrant already exists with this hydrant suffix.");
        }

        [TestMethod]
        public void TestHydrantSuffixMustBeGreaterThanOrEqualToONE()
        {
            _viewModel.HydrantSuffix = 1;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HydrantSuffix);

            _viewModel.HydrantSuffix = 0;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.HydrantSuffix, "HydrantSuffix must be greater than zero.");
        }

        [TestMethod]
        public void TestSAPFunctionalLocationValidation()
        {
            // This is required only when the selected operating center's IsContractedOperations == false.
            var opcWithContractedOps = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = true, SAPEnabled = true });
            var opcWithoutContractedOps = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = false, SAPEnabled = true });
            var functionalLocation = GetFactory<FunctionalLocationFactory>().Create();

            _viewModel.FunctionalLocation = null;
            _viewModel.OperatingCenter = opcWithContractedOps.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FunctionalLocation);

            _viewModel.OperatingCenter = opcWithoutContractedOps.Id;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FunctionalLocation, "The Functional Location field is required.", validationNotDoneByAttribute: true);

            _viewModel.FunctionalLocation = functionalLocation.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FunctionalLocation);
        }

        [TestMethod]
        public void TestNumberDuplicationTruthTable()
        {
            _viewModel.IsFoundHydrant = true;

            _entity.Town.OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = _entity.Town,
                OperatingCenter = _entity.OperatingCenter,
                Abbreviation = "BUH!"
            });
            Session.Save(_entity.Town);

            this.TestNumberDuplicationTruthTable(_entity, _viewModel, "HydrantSuffix",
                "A hydrant already exists with this hydrant suffix.", x => new {
                    Status = x,
                    _entity.OperatingCenter,
                    _entity.HydrantSuffix,
                    HydrantNumber = $"HBUH!-{_entity.HydrantSuffix}"
                });
        }

        #endregion

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Street);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WorkOrderNumber);
        }

        [TestMethod]
        public void TestCrossStreetCanMapBothWays()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc1.Id;
            var crossStreet = GetEntityFactory<Street>().Create(new { Description = "Foo" });
            _entity.CrossStreet = crossStreet;

            _vmTester.MapToViewModel();

            Assert.AreEqual(crossStreet.Id, _viewModel.CrossStreet);

            _entity.CrossStreet = null;
            _vmTester.MapToEntity();

            Assert.AreSame(crossStreet, _entity.CrossStreet);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledFalse()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false });
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
        public void TestMapToEntitySetsSendToSAPTrueWhenOperatingCenterSAPEnabledTrue()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc1.Id;

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
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc1.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPToFalseIfTheExistingStatusIsCancelledAndTheStatusItNotBeingChanged()
        {
            // This same test should work for Cancelled, Retired, and Removed.
            var statuses = new[] {_cancelledStatus, _retiredStatus, _removedStatus};

            foreach (var status in statuses)
            {
                var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
                _entity.Town = town;
                _entity.OperatingCenter = opc1;
                _entity.Status = status;
                _viewModel.Town = town.Id;
                _viewModel.OperatingCenter = opc1.Id;
                _viewModel.Status = status.Id;

                _vmTester.MapToEntity();

                Assert.IsFalse(_viewModel.SendToSAP);
            }
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPToTrueIfTheExistingStatusWasCancelledAndTheStatusIsChanged()
        {
            // This same test should work for Cancelled, Retired, and Removed.
            var statuses = new[] { _cancelledStatus, _retiredStatus, _removedStatus };

            foreach (var status in statuses)
            {
                var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
                _entity.Town = town;
                _entity.OperatingCenter = opc1;
                _entity.Status = status;
                _viewModel.Town = town.Id;
                _viewModel.OperatingCenter = opc1.Id;
                _viewModel.Status = _activeStatus.Id;

                _vmTester.MapToEntity();

                Assert.IsTrue(_viewModel.SendToSAP);
            }
        }

        [TestMethod]
        public void TestStreetCanMapBothWays()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc1.Id;

            var street = GetEntityFactory<Street>().Create(new { Description = "Foo", IsActive = true });
            _entity.Street = street;

            _vmTester.MapToViewModel();

            Assert.AreEqual(street.Id, _viewModel.Street);

            _entity.Street = null;
            _vmTester.MapToEntity();

            Assert.AreSame(street, _entity.Street);
        }

        #endregion
    }
}
