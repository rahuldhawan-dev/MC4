using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Models.ViewModels
{
    public abstract class ShortCycleWorkOrderSafetyBriefViewModelTest<TViewModel> : ViewModelTestBase<ShortCycleWorkOrderSafetyBrief, TViewModel> where TViewModel : ShortCycleWorkOrderSafetyBriefViewModel
    {
        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FSR);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DateCompleted);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HasCompletedDailyStretchingRoutine);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HasPerformedInspectionOnVehicle);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsPPEInGoodCondition);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LocationTypes);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HazardTypes);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PPETypes);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ToolTypes);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.FSR, GetEntityFactory<Employee>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.FSR);
            _vmTester.CanMapBothWays(x => x.DateCompleted);
            _vmTester.CanMapBothWays(x => x.HasCompletedDailyStretchingRoutine);
            _vmTester.CanMapBothWays(x => x.HasPerformedInspectionOnVehicle);
            _vmTester.CanMapBothWays(x => x.IsPPEInGoodCondition);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            //Noop - no string lengths here
        }

        [TestMethod]
        public void TestLocationTypesMapToEntityAndMap()
        {
            var expected = GetEntityFactory<ShortCycleWorkOrderSafetyBriefLocationType>().Create();
            var intArray = new int[] {expected.Id};
            _viewModel.LocationTypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.LocationTypes.Single().Id);

            _viewModel.LocationTypes = null;
            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.LocationTypes[0]);
        }

        [TestMethod]
        public void TestHazardTypesMapToEntityAndMap()
        {
            var expected = GetEntityFactory<ShortCycleWorkOrderSafetyBriefHazardType>().Create();
            var intArray = new int[] {expected.Id};
            _viewModel.HazardTypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.HazardTypes.Single().Id);

            _viewModel.HazardTypes = null;
            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.HazardTypes[0]);
        }

        [TestMethod]
        public void TestPPETypesMapToEntityAndMap()
        {
            var expected = GetEntityFactory<ShortCycleWorkOrderSafetyBriefPPEType>().Create();
            var intArray = new int[] {expected.Id};
            _viewModel.PPETypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.PPETypes.Single().Id);

            _viewModel.PPETypes = null;
            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.PPETypes[0]);
        }

        [TestMethod]
        public void TestToolTypesMapToEntityAndMap()
        {
            var expected = GetEntityFactory<ShortCycleWorkOrderSafetyBriefToolType>().Create();
            var intArray = new int[] {expected.Id};
            _viewModel.ToolTypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.ToolTypes.Single().Id);

            _viewModel.ToolTypes = null;
            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.ToolTypes[0]);
        }
    }

    [TestClass]
    public class CreateShortCycleWorkOrderSafetyBriefTest : ShortCycleWorkOrderSafetyBriefViewModelTest<CreateShortCycleWorkOrderSafetyBrief> { }
}
