using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.HydrantPaintings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.HydrantPaintings
{
    [TestClass]
    public class CreateHydrantPaintingTest : HydrantPaintingViewModelTestBase<CreateHydrantPainting>
    {
        [TestMethod]
        public void Test_MapToEntity_DoesNotOverridePaintedAt_WhenPaintedTodayIsFalse()
        {
            var yesterday = _now.AddDays(-1);
            _viewModel.PaintedAt = yesterday;
            _viewModel.PaintedToday = false;

            var entity = _vmTester.MapToEntity();

            Assert.AreEqual(yesterday, entity.PaintedAt);
        }

        [TestMethod]
        public override void Test_MapToEntity_SetsChangeTrackingColumns()
        {
            base.Test_MapToEntity_SetsChangeTrackingColumns();

            var entity = _vmTester.MapToEntity();

            Assert.AreEqual(_now, entity.CreatedAt);
            Assert.AreEqual(_user, entity.CreatedBy);
        }

        [TestMethod]
        public void Test_MapToEntity_SetsPaintedAtToCurrentDateTime_WhenPaintedTodayIsTrue()
        {
            // this will be ignored
            _viewModel.PaintedAt = _now.AddYears(-1);
            _viewModel.PaintedToday = true;

            var entity = _vmTester.MapToEntity();

            Assert.AreEqual(_now, entity.PaintedAt);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist<Hydrant>(x => x.Hydrant);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();

            _vmTester
               .CanMapBothWays(x => x.Hydrant);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert
               .PropertyIsRequired(x => x.Hydrant)
               .PropertyIsRequiredWhen(x => x.PaintedAt, _now, x => x.PaintedToday, false, true);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no strings here
        }
    }
}
