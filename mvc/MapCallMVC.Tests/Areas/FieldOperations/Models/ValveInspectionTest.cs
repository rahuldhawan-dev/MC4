using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class CreateValveInspectionTest : MapCallMvcInMemoryDatabaseTestBase<ValveInspection>
    {
        #region Fields

        private ViewModelTester<CreateValveInspection, ValveInspection> _vmTester;
        private CreateValveInspection _viewModel;
        private ValveInspection _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IValveInspectionRepository>().Use<ValveInspectionRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<ValveInspection>().Create();
            _viewModel = _viewModelFactory.Build<CreateValveInspection, ValveInspection>( _entity);
            _vmTester = new ViewModelTester<CreateValveInspection, ValveInspection>(_viewModel, _entity);
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestValveCanMapBothWays()
        {
            var opCntr1 = GetEntityFactory<OperatingCenter>().Create();
            var valve = GetEntityFactory<Valve>().Create(new {OperatingCenter = opCntr1});

            _entity.Valve = valve;
            _vmTester.MapToViewModel();

            Assert.AreEqual(valve.Id, _viewModel.Valve);

            _entity.Valve = null;
            _vmTester.MapToEntity();

            Assert.AreSame(valve, _entity.Valve);
        }

        [TestMethod]
        public void TestPositionFoundCanMapBothWays()
        {
            var positionFound = GetEntityFactory<ValveNormalPosition>().Create();

            _entity.PositionFound = positionFound;
            _vmTester.MapToViewModel();
            
            Assert.AreEqual(positionFound.Id, _viewModel.PositionFound);

            _entity.PositionFound = null;
            _vmTester.MapToEntity();

            Assert.AreSame(positionFound, _entity.PositionFound);
        }

        [TestMethod]
        public void TestPositionLeftCanMapBothWays()
        {
            var positionLeft = GetEntityFactory<ValveNormalPosition>().Create(new {Description = "Foo"});
            _entity.PositionLeft = positionLeft;

            _vmTester.MapToViewModel();

            Assert.AreEqual(positionLeft.Id, _viewModel.PositionLeft);

            _entity.PositionLeft = null;
            _vmTester.MapToEntity();

            Assert.AreSame(positionLeft, _entity.PositionLeft);
        }
        
        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.DateInspected);
            _vmTester.CanMapBothWays(x => x.Inspected);
            _vmTester.CanMapBothWays(x => x.Turns);
            _vmTester.CanMapBothWays(x => x.TurnsNotCompleted);
            _vmTester.CanMapBothWays(x => x.Remarks);
        }

        [TestMethod]
        public void TestMapToEntitySetsInspectedBy()
        {
            Session.Evict(_entity);
            _entity.InspectedBy = null;
            
            _vmTester.MapToEntity();
            
            Assert.AreSame(_user, _entity.InspectedBy);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledFalse()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
            {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var valve = GetEntityFactory<Valve>().Create(new { Town = town, OperatingCenter = opc1 });
            _viewModel.Valve = valve.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPTrueWhenOperatingCenterSAPEnabledTrue()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
            {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var valve = GetEntityFactory<Valve>().Create(new { Town = town, OperatingCenter = opc1 });
            _viewModel.Valve = valve.Id;

            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledTrueAndContractedOps()
        {
            var opc1 =
                GetFactory<UniqueOperatingCenterFactory>()
                    .Create(new { SAPEnabled = true, IsContractedOperations = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
            {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var valve = GetEntityFactory<Valve>().Create(new { Town = town, OperatingCenter = opc1 });
            _viewModel.Valve = valve.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Valve);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DateInspected);
        }

        [TestMethod]
        public void TestMustExistProperties()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.PositionFound, GetEntityFactory<ValveNormalPosition>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.PositionLeft, GetEntityFactory<ValveNormalPosition>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Valve, GetFactory<ValveFactory>().Create());
        }

        [TestMethod]
        public void TestMinimumTurnsNotCompletedThrowsTurnsError()
        {
            _viewModel.ValveDisplay.Turns = 20;
            _viewModel.Turns = 2;

            ValidationAssert.ModelStateHasError(_viewModel, "Turns", ValveInspectionViewModel.ErrorMessages.TURNS);
        }

        [TestMethod]
        public void TestValidationFailsIfInspectedAndTurnsNotCompleted()
        {
            _viewModel.Inspected = true;
            _viewModel.TurnsNotCompleted = false;
            _viewModel.PositionFound = null;
            _viewModel.PositionLeft = null;
            ValidationAssert.ModelStateHasError(_viewModel, "PositionFound", ValveInspectionViewModel.ErrorMessages.POSITION_FOUND);
            ValidationAssert.ModelStateHasError(_viewModel, "PositionLeft", ValveInspectionViewModel.ErrorMessages.POSITION_LEFT);
            
            _viewModel.Turns = 1;
            _viewModel.TurnsNotCompleted = true;
            ValidationAssert.ModelStateIsValid(_viewModel, "PositionLeft");
        }

        [TestMethod]
        public void TestValidationFailsIfValveIsNotInspectable()
        {
            _entity.Valve.Status.Id = AssetStatus.Indices.CANCELLED;
            Assert.IsFalse(_entity.Valve.IsInspectable, "Sanity");

            ValidationAssert.ModelStateHasError(_viewModel, x => x.Valve, "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.");

            _entity.Valve.Status.Id = AssetStatus.Indices.INACTIVE;
            Assert.IsFalse(_entity.Valve.IsInspectable, "Sanity");

            ValidationAssert.ModelStateHasError(_viewModel, x => x.Valve, "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.");

            _entity.Valve.Status.Id = AssetStatus.Indices.RETIRED;
            Assert.IsFalse(_entity.Valve.IsInspectable, "Sanity");

            ValidationAssert.ModelStateHasError(_viewModel, x => x.Valve, "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.");

            _entity.Valve.Status.Id = AssetStatus.Indices.REMOVED;
            Assert.IsFalse(_entity.Valve.IsInspectable, "Sanity");

            ValidationAssert.ModelStateHasError(_viewModel, x => x.Valve, "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.");

            _entity.Valve.Status.Id = AssetStatus.Indices.ACTIVE;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Valve);
        }

        #endregion

        #region Defaults

        [TestMethod]
        public void TestSetDefaultsSetsDateInspectedToCurrentDateTime()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);

            var target = new CreateValveInspection(_container);
            target.SetDefaults();

            MyAssert.AreClose(now, target.DateInspected.Value);
            //Assert.AreEqual(now.ToString(), target.DateInspected.ToString());
        }

        [TestMethod]
        public void TestSetDefaultsSetsMinTurnsBasedOnValveTurns()
        {
            var turns = 1m;
            var valve = GetEntityFactory<Valve>().Create(new { Turns = turns});
            var valveInspection = GetEntityFactory<ValveInspection>().Create(new {Valve = valve});
            var target = _viewModelFactory.Build<CreateValveInspection, ValveInspection>(valveInspection);

            Assert.AreEqual(1, target.ValveDisplay.MinimumRequiredTurns);
        }

        [TestMethod]
        public void TestMapToEntitySetsOperatingCenterFromHydrant()
        {
            var opc1 =
                GetFactory<UniqueOperatingCenterFactory>()
                    .Create(new { SAPEnabled = true, IsContractedOperations = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
            {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var valve = GetEntityFactory<Valve>().Create(new { Town = town, OperatingCenter = opc1 });
            _viewModel.Valve = valve.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(opc1, _entity.OperatingCenter);
        }


        #endregion
    }

    [TestClass]
    public class EditValveInspectionTest : InMemoryDatabaseTest<ValveInspection>
    {
        #region Fields

        private ViewModelTester<EditValveInspection, ValveInspection> _vmTester;
        private EditValveInspection _viewModel;
        private ValveInspection _entity;
        private IViewModelFactory _viewModelFactory;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModelFactory = _container.GetInstance<IViewModelFactory>();
            _viewModel = _viewModelFactory.Build<EditValveInspection>();
            _entity = new ValveInspection();
            _vmTester = new ViewModelTester<EditValveInspection, ValveInspection>(_viewModel, _entity);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
            i.For<IViewModelFactory>().Use<ViewModelFactory>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsOperatingCenterFromHydrant()
        {
            var opc1 =
                GetFactory<UniqueOperatingCenterFactory>()
                    .Create(new { SAPEnabled = true, IsContractedOperations = true });
            var town = GetEntityFactory<Town>().Create();
            var billing = GetEntityFactory<ValveBilling>().Create();
            var active = GetFactory<ActiveAssetStatusFactory>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
            {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var valve = GetEntityFactory<Valve>().Create(new {
                Town = town,
                OperatingCenter = opc1,
                ValveBilling = billing,
                Status = active,
                ValveNumber = "VZZ-1",
                ValveSuffix = 1
            });
            _entity = GetFactory<ValveInspectionFactory>().Create(new { Valve = valve });
            _viewModel = _viewModelFactory.Build<EditValveInspection, ValveInspection>( _entity);
            _viewModel.Valve = valve.Id;
            _vmTester = new ViewModelTester<EditValveInspection, ValveInspection>(_viewModel, _entity);

            _vmTester.MapToEntity();

            Assert.AreEqual(opc1, _entity.OperatingCenter);
        }

        #endregion
    }
}
