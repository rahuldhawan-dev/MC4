using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class TaskGroupViewModelTest : MapCallMvcInMemoryDatabaseTestBase<TaskGroup>
    {
        private ViewModelTester<TaskGroupViewModel, TaskGroup> _vmTester;
        private TaskGroupViewModel _viewModel;
        protected TaskGroup _entity;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<TaskGroupViewModel>();
            _entity = new TaskGroup();
            _vmTester = new ViewModelTester<TaskGroupViewModel, TaskGroup>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.TaskGroupId);
            _vmTester.CanMapBothWays(x => x.TaskGroupName);
            _vmTester.CanMapBothWays(x => x.TaskDetails);
            _vmTester.CanMapBothWays(x => x.TaskDetailsSummary);
            _vmTester.CanMapBothWays(x => x.MaintenancePlanTaskType, GetEntityFactory<MaintenancePlanTaskType>().Create());
            _vmTester.CanMapBothWays(x => x.TaskGroupCategory, GetEntityFactory<TaskGroupCategory>().Create());
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TaskGroupId);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TaskGroupName);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TaskGroupCategory);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MaintenancePlanTaskType);
        }

        [TestMethod]
        public void TestNotRequiredFields()
        {
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.TaskDetails);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.TaskDetailsSummary);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.EquipmentTypes);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.EquipmentPurposes);
        }

        [TestMethod]
        public void TestMaxLengthOnStringProperties()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.TaskGroupId, TaskGroup.StringLengths.TASK_GROUP_ID);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.TaskGroupName, TaskGroup.StringLengths.TASK_GROUP_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.TaskDetailsSummary, TaskGroup.StringLengths.TASK_DETAILS_SUMMARY);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.MaintenancePlanTaskType, GetEntityFactory<MaintenancePlanTaskType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.TaskGroupCategory, GetEntityFactory<TaskGroupCategory>().Create());
        }

        #endregion
    }
}