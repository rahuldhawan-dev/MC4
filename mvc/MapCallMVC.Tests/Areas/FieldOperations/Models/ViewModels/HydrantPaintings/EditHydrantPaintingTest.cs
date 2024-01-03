using MapCallMVC.Areas.FieldOperations.Models.ViewModels.HydrantPaintings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.HydrantPaintings
{
    [TestClass]
    public class EditHydrantPaintingTest : HydrantPaintingViewModelTestBase<EditHydrantPainting>
    {
        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // noop
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.PaintedAt);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // noop
        }
    }
}
