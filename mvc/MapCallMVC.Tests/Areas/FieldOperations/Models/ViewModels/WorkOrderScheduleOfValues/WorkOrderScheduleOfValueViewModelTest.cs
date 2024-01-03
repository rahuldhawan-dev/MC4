using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderScheduleOfValues;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.WorkOrderScheduleOfValues
{
    [TestClass]
    public class WorkOrderScheduleOfValueViewModelTest<TViewModel> : ViewModelTestBase<WorkOrderScheduleOfValue, TViewModel> where TViewModel : WorkOrderScheduleOfValueViewModel
    {
        #region Init/Cleanup

        protected override WorkOrderScheduleOfValue CreateEntity()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            var scheduleOfValue = GetFactory<ScheduleOfValueFactory>().Create();
            return GetEntityFactory<WorkOrderScheduleOfValue>().Build(new {
                WorkOrder = workOrder,
                ScheduleOfValue = scheduleOfValue,
                IsOvertime = false,
                Total = 22M,
                LaborUnitCost = 14M
            });
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.WorkOrder, GetEntityFactory<WorkOrder>().Create())
                            .EntityMustExist(x => x.ScheduleOfValueCategory, GetEntityFactory<ScheduleOfValueCategory>().Create())
                            .EntityMustExist(x => x.ScheduleOfValue, GetFactory<ScheduleOfValueFactory>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.WorkOrder);
            _vmTester.CanMapBothWays(x => x.ScheduleOfValue);
            _vmTester.CanMapBothWays(x => x.Total);
            _vmTester.CanMapBothWays(x => x.OtherDescription);
            _vmTester.CanMapBothWays(x => x.IsOvertime);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.WorkOrder)
                            .PropertyIsRequired(x => x.ScheduleOfValueCategory)
                            .PropertyIsRequired(x => x.ScheduleOfValue)
                            .PropertyIsRequiredWhen(x => x.OtherDescription, "Testing Notes",
                                 x => x.ScheduleOfValueCategory, ScheduleOfValueCategory.Indices.OTHER);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no strings to validate string length
        }

        #endregion
    }
}
