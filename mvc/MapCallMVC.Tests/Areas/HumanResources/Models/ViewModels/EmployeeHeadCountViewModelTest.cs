using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCallMVC.Areas.HumanResources.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.HumanResources.Models.ViewModels
{
    public abstract class EmployeeHeadCountViewModelTest<TViewModel> : ViewModelTestBase<EmployeeHeadCount, TViewModel> where TViewModel : EmployeeHeadCountViewModel
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        public User _user;
        public Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetEntityFactory<User>().Create();
            _user.Employee = GetEntityFactory<Employee>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
        }

        #endregion

        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.BusinessUnit, GetEntityFactory<BusinessUnit>().Create());
            _vmTester.CanMapBothWays(x => x.Category, GetEntityFactory<EmployeeHeadCountCategory>().Create());
            _vmTester.CanMapBothWays(x => x.EndDate);
            _vmTester.CanMapBothWays(x => x.MiscNotes);
            _vmTester.CanMapBothWays(x => x.NonUnionCount);
            _vmTester.CanMapBothWays(x => x.OtherCount);
            _vmTester.CanMapBothWays(x => x.StartDate);
            _vmTester.CanMapBothWays(x => x.UnionCount);
            _vmTester.CanMapBothWays(x => x.Year);
        }

        [TestMethod]
        public void TestMapSetsOperatingCenterFromBusinessUnit()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create();
            var bu = GetEntityFactory<BusinessUnit>().Create(new { OperatingCenter = opc});
            _entity.BusinessUnit = bu;
            _viewModel.OperatingCenter = null;

            _vmTester.MapToViewModel();

            Assert.AreEqual(opc.Id, _viewModel.OperatingCenter);
        }

        [TestMethod]
        public void TestMapSetsStateFromBusinessUnitOperatingCenter()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create();
            var state = opc.State;
            var bu = GetEntityFactory<BusinessUnit>().Create(new { OperatingCenter = opc});
            _entity.BusinessUnit = bu;
            _viewModel.State = null;

            _vmTester.MapToViewModel();

            Assert.AreEqual(state.Id, _viewModel.State);
        }

        [TestMethod]
        public void TestMapToEntitySetsTotalToSumOfFields()
        {
            _viewModel.UnionCount = 10;
            _viewModel.NonUnionCount = 11;
            _viewModel.OtherCount = 12;
            _entity.TotalCount = 0;

            _vmTester.MapToEntity();

            Assert.AreEqual(33, _entity.TotalCount);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.NonUnionCount);
            ValidationAssert.PropertyIsRequired(x => x.UnionCount);
            ValidationAssert.PropertyIsRequired(x => x.OtherCount);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // noop
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.BusinessUnit, GetEntityFactory<BusinessUnit>().Create());
            ValidationAssert.EntityMustExist(x => x.Category, GetEntityFactory<EmployeeHeadCountCategory>().Create());
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(x => x.State, GetEntityFactory<State>().Create());
        }

        [TestMethod]
        public void TestPropertiesWithMinValueValidation()
        {
            ValidationAssert.PropertyHasMinValueRequirement(x => x.NonUnionCount, 0);
            ValidationAssert.PropertyHasMinValueRequirement(x => x.OtherCount, 0);
            ValidationAssert.PropertyHasMinValueRequirement(x => x.UnionCount, 0);
        }

        [TestMethod]
        public void TestYearMustBeBetween1000and9999()
        {
            ValidationAssert.PropertyHasRequiredRange(x => x.Year, 1000, 9999);
        }

        [TestMethod]
        public void TestStartDateMustBeLessThanOrEqualToEndDate()
        {
            var date = new DateTime(1984, 4, 24);
            _viewModel.StartDate = date;
            _viewModel.EndDate = date.AddSeconds(-1);

            ValidationAssert.ModelStateHasError(x => x.EndDate, "EndDate must be greater than or equal to StartDate.");
            ValidationAssert.ModelStateHasError(x => x.StartDate, "StartDate must be less than or equal to EndDate.");

            _viewModel.EndDate = date;
            ValidationAssert.ModelStateIsValid(x => x.EndDate);
            ValidationAssert.ModelStateIsValid(x => x.StartDate);
        }

        #endregion
    }
}