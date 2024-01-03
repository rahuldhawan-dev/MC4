using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class RemoveScheduledAssignmentsTest : MapCallMvcInMemoryDatabaseTestBase<MaintenancePlan>
    {
        private ViewModelTester<RemoveScheduledAssignments, MaintenancePlan> _vmTester;
        private RemoveScheduledAssignments _viewModel;
        private MaintenancePlan _entity;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<MaintenancePlan>().Create();
            _viewModel = _viewModelFactory.Build<RemoveScheduledAssignments, MaintenancePlan>(_entity);
            _vmTester = new ViewModelTester<RemoveScheduledAssignments, MaintenancePlan>(_viewModel, _entity);
        }

        #endregion

        [TestMethod]
        public void TestMapToEntityRemovesScheduledAssignmentsFromMaintenancePlan()
        {
            var assignments = GetEntityFactory<ScheduledAssignment>().CreateList(3, new { MaintenancePlan = _entity });

            _entity.ScheduledAssignments = assignments.ToHashSet();

            _viewModel.SelectedAssignments = assignments.Where(x => x.Id > 1).Select(x => x.Id).ToArray();

            _vmTester.MapToEntity();

            Assert.AreEqual(1, _entity.ScheduledAssignments.Count);
        }
    }
}
