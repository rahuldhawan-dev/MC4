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
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class SewerOpeningInspectionTest : MapCallMvcInMemoryDatabaseTestBase<SewerOpeningInspection>
    {
        #region Fields

        private ViewModelTester<SewerOpeningInspectionViewModel, SewerOpeningInspection> _vmTester;
        private SewerOpeningInspectionViewModel _viewModel;
        private SewerOpeningInspection _entity;
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
            e.For<ISewerOpeningInspectionRepository>().Use<SewerOpeningInspectionRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            _entity = GetEntityFactory<SewerOpeningInspection>().Create();
            _viewModel = _viewModelFactory.Build<SewerOpeningInspectionViewModel, SewerOpeningInspection>(_entity);
            _vmTester = new ViewModelTester<SewerOpeningInspectionViewModel, SewerOpeningInspection>(_viewModel, _entity);
        }

        #endregion
        #region Tests

        [TestMethod]
        public void TestsPropertiesThatCanMapBothWays()
        {
            var factoryService = _container.GetInstance<ITestDataFactoryService>();
            _vmTester.CanMapBothWays(x => x.SewerOpening, factoryService);
            _vmTester.CanMapBothWays(x => x.Remarks);
            _vmTester.CanMapBothWays(x => x.DateInspected);
            _vmTester.CanMapBothWays(x => x.PipesIn);
            _vmTester.CanMapBothWays(x => x.PipesOut);
            _vmTester.CanMapBothWays(x => x.RimHeightAboveBelowGrade);
            _vmTester.CanMapBothWays(x => x.RimToWaterLevelDepth);
            _vmTester.CanMapBothWays(x=> x.AmountOfDebrisGritCubicFeet);
        }

        [TestMethod]
        public void TestRequiredStringLengths()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PipesIn, SewerOpeningInspection.StringLengths.PIPES);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PipesOut, SewerOpeningInspection.StringLengths.PIPES);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Remarks, SewerOpeningInspection.StringLengths.REMARKS);
        }

        [TestMethod]
        public void TestRequiredFieldsAreRequiredRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DateInspected);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RimHeightAboveBelowGrade);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RimToWaterLevelDepth);
        }
        [TestMethod]
        public void TestRequiredRange()
        {
            ValidationAssert.PropertyHasRequiredRange(_viewModel, x => x.AmountOfDebrisGritCubicFeet, 0m, 99999.99m);
            ValidationAssert.PropertyHasRequiredRange(_viewModel, x => x.RimToWaterLevelDepth, 0m, 99.99m);
            ValidationAssert.PropertyHasRequiredRange(_viewModel, x => x.RimHeightAboveBelowGrade, 0m, 99.99m);
        }
        #endregion
    }

    [TestClass]
    public class CreateSewerOpeningInspectionTest : MapCallMvcInMemoryDatabaseTestBase<SewerOpeningInspection>
    {
        #region Fields

        private ViewModelTester<CreateSewerOpeningInspection, SewerOpeningInspection> _vmTester;
        private CreateSewerOpeningInspection _viewModel;
        private SewerOpeningInspection _entity;
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
            e.For<ISewerOpeningInspectionRepository>().Use<SewerOpeningInspectionRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            _entity = GetEntityFactory<SewerOpeningInspection>().Create();
            _viewModel = _viewModelFactory.Build<CreateSewerOpeningInspection, SewerOpeningInspection>(_entity);
            _vmTester = new ViewModelTester<CreateSewerOpeningInspection, SewerOpeningInspection>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsInspectedBy()
        {
            _entity.InspectedBy = null;
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.InspectedBy);
        }
       
        [TestMethod]
        public void TestMapToEntityAmountOfDebrisGritCubicFeet()
        {
            _vmTester.ViewModel.AmountOfDebrisGritCubicFeet = (decimal)99999.00;
            _vmTester.MapToEntity();
            Assert.AreEqual((decimal)99999.00, _entity.AmountOfDebrisGritCubicFeet);
        }
        #endregion
    }

    [TestClass]
    public class EditSewerOpeningInspectionTest : MapCallMvcInMemoryDatabaseTestBase<SewerOpeningInspection>
    {
        #region Fields

        private ViewModelTester<EditSewerOpeningInspection, SewerOpeningInspection> _vmTester;
        private EditSewerOpeningInspection _viewModel;
        private SewerOpeningInspection _entity;
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
            e.For<ISewerOpeningInspectionRepository>().Use<SewerOpeningInspectionRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            _entity = GetEntityFactory<SewerOpeningInspection>().Create();
            _viewModel = _viewModelFactory.Build<EditSewerOpeningInspection, SewerOpeningInspection>(_entity);
            _vmTester = new ViewModelTester<EditSewerOpeningInspection, SewerOpeningInspection>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestInspectedByReturnsUser()
        {
            _entity.InspectedBy = _user;
            Assert.AreSame(_user, _viewModel.InspectedBy);
        }

        #endregion
    }


}
