using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.CrewAssignments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.CrewAssignments
{
    [TestClass]
    public class EditCrewAssignmentTest : ViewModelTestBase<CrewAssignment, EditCrewAssignment>
    {
        #region Tests

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.WorkOrder, GetEntityFactory<WorkOrder>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.DateStarted);
            _vmTester.CanMapBothWays(x => x.DateEnded);
            _vmTester.CanMapBothWays(x => x.EmployeesOnJob);
            _vmTester.CanMapBothWays(x => x.WorkOrder);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.WorkOrder);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no properties to validate length
        }

        #endregion
    }
}
