using System.Collections.Generic;
using System.Linq;
using Contractors.Models.ViewModels;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using StructureMap;
using CrewAssignmentRepository = Contractors.Data.Models.Repositories.CrewAssignmentRepository;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass]
    public class CrewAssignmentIndexTest : ContractorsControllerTestBase<CrewAssignment, CrewAssignmentRepository>
    {
        #region Fields

        private CrewAssignmentIndex _target;
        private WorkOrder _workOrder;
        private IViewModelFactory _viewModelFactory;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Mock();
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _workOrder = new WorkOrder
                             {
                                 CrewAssignments = new List<CrewAssignment>() 
                             };
            _workOrder.SetPropertyValueByName("Id", 32);
            var ass = new CrewAssignment();
            ass.WorkOrder = _workOrder;
            _workOrder.CrewAssignments.Add(ass);
            _viewModelFactory = _container.GetInstance<ViewModelFactory>();
            _target = _viewModelFactory.Build<CrewAssignmentIndex, WorkOrder>(_workOrder);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConstructorSetsWorkOrderID()
        {
            Assert.AreEqual(_workOrder.Id, _target.WorkOrder);
        }

        [TestMethod]
        public void TestConstructorCallsSetCrewAssignments()
        {
            Assert.IsNotNull(_target.CrewAssignments);
        }

        [TestMethod]
        public void TestSetCrewAssignmentsCreatesRowViewsForEachAssignmentThatIsAssignedToCurrentContractorForTheGivenWorkOrder()
        {
            _container.Inject(_container.GetInstance<CrewAssignmentRepository>());

            var goodWorkOrder = GetFactory<SchedulingWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor 
            });
            var badWorkOrder = GetFactory<SchedulingWorkOrderFactory>().Create();
            var badAssignment1 = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = goodWorkOrder
            });
            var badAssignment2 = GetFactory<CrewAssignmentFactory>().Create(new
            {
                WorkOrder = goodWorkOrder
            });

            var goodCrew = GetFactory<CrewFactory>().Create(new{ Contractor = _currentUser.Contractor });
            var goodAssignment = GetFactory<CrewAssignmentFactory>().Create(new
            {
                Crew = goodCrew,
                WorkOrder = goodWorkOrder
            });
            var badAssignment3 = GetFactory<CrewAssignmentFactory>().Create(new
            {
                Crew = goodCrew,
                WorkOrder = badWorkOrder
            });
            var target = _viewModelFactory.Build<CrewAssignmentIndex, WorkOrder>(goodWorkOrder);

            Assert.AreEqual(1, target.CrewAssignments.Count());
            Assert.AreNotEqual(goodAssignment.Id, 0, "Sanity");
            Assert.AreEqual(goodAssignment.Id, target.CrewAssignments.Single().CrewAssignment.Id);
        }

        [TestMethod]
        public void TestClassHasParameterlessConstructorForMVC()
        {
            Assert.IsNotNull(typeof(CrewAssignmentIndex).HasParameterlessConstructor());
        }

        #endregion
    }
}
