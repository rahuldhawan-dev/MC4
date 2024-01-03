using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using Moq;
using StructureMap;
using System.Linq;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class BaseMarkoutDamageViewModelTest : MapCallMvcInMemoryDatabaseTestBase<MarkoutDamage>
    {
        #region Fields

        private MarkoutDamage _entity;
        private MarkoutDamageViewModel _viewModel;
        private ViewModelTester<MarkoutDamageViewModel, MarkoutDamage> _vmTester;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private MarkoutDamageToType _otherMarkoutDamageToType;
        private MarkoutDamageToType _ourMarkoutDamageToType;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IStateRepository>().Use<StateRepository>();
            e.For<ICountyRepository>().Use<CountyRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _user = new User();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _ourMarkoutDamageToType = GetFactory<MarkoutDamageToTypeFactory>().Create(new {
                Id = MarkoutDamageToType.Indices.OURS,
                Description = MarkoutDamageToType.ImportantDescriptions.OURS
            });
            _otherMarkoutDamageToType = GetFactory<MarkoutDamageToTypeFactory>().Create(new {
                Id = MarkoutDamageToType.Indices.OTHERS,
                Description = MarkoutDamageToType.ImportantDescriptions.OTHERS
            });

            // This needs to come after the repositories are made or else this fails due to the
            // dynamic requiredwhen validators being unable to find repositories.
            _entity = GetFactory<MarkoutDamageFactory>().Create();
            _viewModel = _viewModelFactory.Build<MarkoutDamageViewModel, MarkoutDamage>(_entity);
            _vmTester = new ViewModelTester<MarkoutDamageViewModel, MarkoutDamage>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ApprovedOn);
            _vmTester.CanMapBothWays(x => x.DamageComments);
            _vmTester.CanMapBothWays(x => x.DamageOn);
            _vmTester.CanMapBothWays(x => x.EmployeesOnJob);
            _vmTester.CanMapBothWays(x => x.Excavator);
            _vmTester.CanMapBothWays(x => x.ExcavatorAddress);
            _vmTester.CanMapBothWays(x => x.ExcavatorCausedDamage);
            _vmTester.CanMapBothWays(x => x.ExcavatorDiscoveredDamage);
            _vmTester.CanMapBothWays(x => x.ExcavatorPhone);
            _vmTester.CanMapBothWays(x => x.IsMarkedOut);
            _vmTester.CanMapBothWays(x => x.IsMismarked);
            _vmTester.CanMapBothWays(x => x.MismarkedByInches);
            _vmTester.CanMapBothWays(x => x.NearestCrossStreet);
            _vmTester.CanMapBothWays(x => x.SAPWorkOrderId);
            _vmTester.CanMapBothWays(x => x.Street);
            _vmTester.CanMapBothWays(x => x.Was911Called);
            _vmTester.CanMapBothWays(x => x.WerePicturesTaken);
        }

        [TestMethod]
        public void TestUtilityDamagesMapsBothWays()
        {
            var utilityDamageType = GetFactory<GasMarkoutDamageUtilityDamageTypeFactory>().Create();
            _entity.UtilityDamages.Add(utilityDamageType);
            _vmTester.MapToViewModel();
            Assert.AreEqual(utilityDamageType.Id, _viewModel.UtilityDamages.Single());

            _entity.UtilityDamages.Clear();
            _vmTester.MapToEntity();
            Assert.AreSame(utilityDamageType, _entity.UtilityDamages.Single());
        }

        [TestMethod]
        public void TestCoordinateCanMapBothWays()
        {
            var c = GetEntityFactory<Coordinate>().Create();
            _entity.Coordinate = c;
            _viewModel.Coordinate = null;

            _vmTester.MapToViewModel();
            Assert.AreEqual(c.Id, _viewModel.Coordinate);

            _entity.Coordinate = null;
            _vmTester.MapToEntity();
            Assert.AreSame(c, _entity.Coordinate);
        }

        [TestMethod]
        public void TestMarkoutDamageToTypeCanMapBothWays()
        {
            var c = GetEntityFactory<MarkoutDamageToType>().Create();
            _entity.MarkoutDamageToType = c;
            _viewModel.MarkoutDamageToType = null;

            _vmTester.MapToViewModel();
            Assert.AreEqual(c.Id, _viewModel.MarkoutDamageToType);

            _entity.MarkoutDamageToType = null;
            _vmTester.MapToEntity();
            Assert.AreSame(c, _entity.MarkoutDamageToType);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var opCenter = GetEntityFactory<OperatingCenter>().Create();
            _entity.OperatingCenter = opCenter;
            _viewModel.OperatingCenter = null;

            _vmTester.MapToViewModel();
            Assert.AreEqual(opCenter.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();
            Assert.AreSame(opCenter, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestSupervisorSignOffEmployeeCanMapBothWays()
        {
            var e = GetEntityFactory<Employee>().Create();
            _entity.SupervisorSignOffEmployee = e;
            _viewModel.SupervisorSignOffEmployee = null;

            _vmTester.MapToViewModel();
            Assert.AreEqual(e.Id, _viewModel.SupervisorSignOffEmployee);

            _entity.SupervisorSignOffEmployee = null;
            _vmTester.MapToEntity();
            Assert.AreSame(e, _entity.SupervisorSignOffEmployee);
        }

        [TestMethod]
        public void TestTownCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create();
            _entity.Town = town;
            _viewModel.Town = null;

            _vmTester.MapToViewModel();
            Assert.AreEqual(town.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();
            Assert.AreSame(town, _entity.Town);
        }

        [TestMethod]
        public void TestMapToViewModelSetsStateBasedOnTown()
        {
            var town = GetEntityFactory<Town>().Create();
            _entity.Town = town;
            _viewModel.State = null;
            _viewModel.Town = null;

            _vmTester.MapToViewModel();
            Assert.AreEqual(town.County.State.Id, _viewModel.State);

            _entity.Town = null;
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.State);
        }

        [TestMethod]
        public void TestMapToViewModelSetsCountyBasedOnTown()
        {
             var town = GetEntityFactory<Town>().Create();
            _entity.Town = town;
            _viewModel.County = null;
            _viewModel.Town = null;

            _vmTester.MapToViewModel();
            Assert.AreEqual(town.County.Id, _viewModel.County);

            _entity.Town = null;
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.County);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestPropertiesWithRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.County);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Coordinate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MarkoutDamageToType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.State);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Street);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.NearestCrossStreet);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DamageOn);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DamageComments);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsMarkedOut);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsMismarked);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ExcavatorDiscoveredDamage);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ExcavatorCausedDamage);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Was911Called, "The Was 911 Called? field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WerePicturesTaken);
        }

        [TestMethod]
        public void TestPropertiesWithMaxStringLengthRequirements()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.RequestNumber, MarkoutDamage.StringLengths.REQUEST_NUM);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Street, MarkoutDamage.StringLengths.STREET);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.NearestCrossStreet, MarkoutDamage.StringLengths.CROSS_STREET);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Excavator, MarkoutDamage.StringLengths.EXCAVATOR);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ExcavatorAddress, MarkoutDamage.StringLengths.EXCAVATOR_ADDRESS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ExcavatorPhone, MarkoutDamage.StringLengths.EXCAVATOR_PHONE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.SAPWorkOrderId, MarkoutDamage.StringLengths.SAP_WORK_ORDER_ID);
        }

        [TestMethod]
        public void TestRequestNumberIsRequiredIfMarkoutDamageToTypeEqualsOthers()
        {
            _viewModel.RequestNumber = null;
            _viewModel.MarkoutDamageToType = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.RequestNumber);

            _viewModel.MarkoutDamageToType = _ourMarkoutDamageToType.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.RequestNumber);

            _viewModel.MarkoutDamageToType = _otherMarkoutDamageToType.Id;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.RequestNumber, "Required when markout damage is to others.");

            _viewModel.RequestNumber = "1231415";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.RequestNumber);
        }

        [TestMethod]
        public void TestExcavatorIsRequiredIfMarkoutDamageToTypeEqualsOurs()
        {
            _viewModel.Excavator = null;
            _viewModel.MarkoutDamageToType = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Excavator);

            _viewModel.MarkoutDamageToType = _otherMarkoutDamageToType.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Excavator);

            _viewModel.MarkoutDamageToType = _ourMarkoutDamageToType.Id;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.Excavator, "Required when markout damage is to our own.");

            _viewModel.Excavator = "Some guy.";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Excavator);
        }

        [TestMethod]
        public void TestUtilityDamagesIsRequiredIfMarkoutDamageToTypeEqualsOthers()
        {
            var damageType = GetEntityFactory<MarkoutDamageUtilityDamageType>().Create();
            _viewModel.UtilityDamages = null;
            _viewModel.MarkoutDamageToType = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.UtilityDamages);

            _viewModel.MarkoutDamageToType = _ourMarkoutDamageToType.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.UtilityDamages);

            _viewModel.MarkoutDamageToType = _otherMarkoutDamageToType.Id;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.UtilityDamages, "Required when markout damage is to others.");

            _viewModel.UtilityDamages = new[] { damageType.Id };
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.UtilityDamages);
        }

        [TestMethod]
        public void TestMismarkedByInchesIsRequiredIfIsMismarkedIsTrue()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.MismarkedByInches, 1, x => x.IsMismarked, true, false);
        }
        
        #endregion

        #endregion

        #region Helper classes

        private class MarkoutDamageViewModel : BaseMarkoutDamageViewModel
        {
            public MarkoutDamageViewModel(IContainer container) : base(container) { }
        }

        #endregion
    }
}
