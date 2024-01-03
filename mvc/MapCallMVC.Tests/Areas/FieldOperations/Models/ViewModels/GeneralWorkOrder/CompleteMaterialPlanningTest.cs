using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using System;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    [TestClass]
    public class CompleteMaterialPlanningTest : ViewModelTestBase<WorkOrder, CompleteMaterialPlanning>
    {
        #region Tests

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // no properties to validate
        }

        [TestMethod]
        public void TestMapToEntitySetsMaterialsPlannedOnDate()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);

            _vmTester.MapToEntity();

            MyAssert.AreClose(expectedDate, _entity.MaterialPlanningCompletedOn.Value);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            // no properties to validate
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            // no properties to validate
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no properties to validate
        }

        #endregion
    }
}
