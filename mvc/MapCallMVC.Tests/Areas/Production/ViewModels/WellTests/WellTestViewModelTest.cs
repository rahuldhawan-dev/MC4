using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MapCall.Common.Testing;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels.WellTests;
using MMSINC.Authentication;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.Production.ViewModels.WellTests
{
    [TestClass]
    public class WellTestViewModelTest<TViewModel> : ViewModelTestBase<WellTest, TViewModel> where TViewModel : WellTestViewModel
    {
        #region Fields

        protected User _user;
        protected DateTime _rightNow;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        protected Mock<IAuthenticationService<User>> _authService;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _authService = new Mock<IAuthenticationService<User>>();

            _rightNow = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_rightNow);
            
            _user = GetEntityFactory<User>().Create(new {
                Employee = GetEntityFactory<Employee>().Create()
            });
            _authService.Setup(x => x.CurrentUser).Returns(_user);

            _container.Inject(_authService.Object);
            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ProductionWorkOrder, GetEntityFactory<ProductionWorkOrder>().Create());
            _vmTester.CanMapBothWays(x => x.Equipment, GetEntityFactory<Equipment>().Create());
            _vmTester.CanMapBothWays(x => x.Employee, GetEntityFactory<Employee>().Create());
            _vmTester.CanMapBothWays(x => x.DateOfTest);
            _vmTester.CanMapBothWays(x => x.PumpingRate);
            _vmTester.CanMapBothWays(x => x.MeasurementPoint);
            _vmTester.CanMapBothWays(x => x.GradeType);
            _vmTester.CanMapBothWays(x => x.StaticWaterLevel);
            _vmTester.CanMapBothWays(x => x.PumpingWaterLevel);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProductionWorkOrder);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Equipment);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Employee);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PumpingRate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MeasurementPoint);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.GradeType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.StaticWaterLevel);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PumpingWaterLevel);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DateOfTest);
        }

        [TestMethod]
        public void TestRequiredRangeValidation()
        {
            ValidationAssert.PropertyHasRequiredRange(_viewModel, 
                x => x.PumpingRate, 
                WellTest.Ranges.PUMPING_RATE_MIN, 
                WellTest.Ranges.PUMPING_RATE_MAX);

            ValidationAssert.PropertyHasRequiredRange(_viewModel, 
                x => x.MeasurementPoint, 
                (decimal)WellTest.Ranges.MEASUREMENT_POINT_MIN,
                (decimal)WellTest.Ranges.MEASUREMENT_POINT_MAX);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.ProductionWorkOrder, GetEntityFactory<ProductionWorkOrder>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Equipment, GetEntityFactory<Equipment>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Employee, GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.GradeType, GetEntityFactory<WellTestGradeType>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation() { }

        [TestMethod]
        public void TestModelIsValid()
        {
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.StaticWaterLevel);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PumpingWaterLevel);
        }

        #endregion

        #endregion
    }
}