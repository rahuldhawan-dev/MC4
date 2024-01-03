using System;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using IWorkOrderRepository = Contractors.Data.Models.Repositories.IWorkOrderRepository;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass]
    public class CreateMarkoutTest : MapCallMvcInMemoryDatabaseTestBase<Markout>
    {
        #region Fields

        private ViewModelTester<CreateMarkout, Markout> _vmTester;
        private CreateMarkout _viewModel;
        private Markout _entity;
        private Mock<IAuthenticationService<ContractorUser>> _authServ;
        private ContractorUser _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<ContractorUser>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<ContractorUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetFactory<MarkoutFactory>().Create();
            _viewModel = _container.GetInstance<CreateMarkout>();
            _viewModel.Map(_entity);
            _vmTester = new ViewModelTester<CreateMarkout, Markout>(_viewModel, _entity);

            // Needs to be done or else any querying related to the WorkOrder will fail.
            _entity.WorkOrder.AssignedContractor = _user.Contractor;
            Session.Save(_entity.WorkOrder);
            Session.Flush();
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestMappingSimplePropertiesBothWays()
        {
            _vmTester.CanMapBothWays(x => x.DateOfRequest);
            _vmTester.CanMapBothWays(x => x.Note);
            _vmTester.CanMapBothWays(x => x.MarkoutNumber);
        }

        [TestMethod]
        public void TestMappingMarkoutTypeBothWays()
        {
            var markoutType = GetFactory<MarkoutTypeFactory>().Create();
            _entity.MarkoutType = markoutType;
            _vmTester.MapToViewModel();
            Assert.AreEqual(markoutType.Id, _viewModel.MarkoutType);

            _entity.MarkoutType = null;
            _vmTester.MapToEntity();
            Assert.AreSame(markoutType, _entity.MarkoutType);
        }

        [TestMethod]
        public void TestMappingWorkOrderBothWays()
        {
            var workOrder = _entity.WorkOrder;
            _entity.WorkOrder = workOrder;
            _vmTester.MapToViewModel();
            Assert.AreEqual(workOrder.Id, _viewModel.WorkOrder);

            _entity.WorkOrder = null;
            _vmTester.MapToEntity();
            Assert.AreSame(workOrder, _entity.WorkOrder);
        }

        [TestMethod]
        public void TestMappingReadyDateWhenMarkoutsEditableIsTrue()
        {
            _entity.WorkOrder.OperatingCenter.MarkoutsEditable = true;
            var expectedDate = new DateTime(1984, 4, 24, 05, 30, 00);
            _entity.ReadyDate = null;
            _viewModel.ReadyDate = expectedDate;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.ReadyDate);
        }

        [TestMethod]
        public void TestMappingReadyDateWhenMarkoutsEditableIsFalse()
        {
            // First test that nothing happens to ReadyDate if MarkoutRequired is false.
            _entity.WorkOrder.OperatingCenter.MarkoutsEditable = false;
            Assert.IsFalse(_entity.WorkOrder.MarkoutRequired, "Sanity");
            _entity.ReadyDate = null;
            _viewModel.ReadyDate = DateTime.Now;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.ReadyDate);

            var markoutRequirement = GetFactory<MarkoutRequirementRoutineFactory>().Create();
           // var newMarkoutType = GetFactory<MarkoutTypeFactory>().Create();
            _entity.WorkOrder.MarkoutRequirement = markoutRequirement;
            Assert.IsTrue(_entity.WorkOrder.MarkoutRequired, "Sanity");
            var expectedDateOfRequest = DateTime.Today;
            _viewModel.DateOfRequest = expectedDateOfRequest;
            var expectedReadyDate = WorkOrdersWorkDayEngine.GetReadyDate(expectedDateOfRequest, markoutRequirement.MarkoutRequirementEnum);
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedReadyDate, _entity.ReadyDate);
        }

        [TestMethod]
        public void TestMappingExpirationDateWhenMarkoutsEditableIsTrue()
        {
            _entity.WorkOrder.OperatingCenter.MarkoutsEditable = true;
            var expectedDate = new DateTime(1984, 4, 24, 05,30,00);
            _entity.ExpirationDate = null;
            _viewModel.ExpirationDate = expectedDate;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.ExpirationDate);
        }

        [TestMethod]
        public void TestMappingExpirationDateWhenMarkoutsEditableIsFalse()
        {
            // First test that nothing happens to ExpirationDate if MarkoutRequired is false.
            _entity.WorkOrder.OperatingCenter.MarkoutsEditable = false;
            Assert.IsFalse(_entity.WorkOrder.MarkoutRequired, "Sanity");
            _entity.ExpirationDate = null;
            _viewModel.ExpirationDate = DateTime.Now;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.ExpirationDate);

            var markoutRequirement = GetFactory<MarkoutRequirementRoutineFactory>().Create();
            // var newMarkoutType = GetFactory<MarkoutTypeFactory>().Create();
            _entity.WorkOrder.MarkoutRequirement = markoutRequirement;
            Assert.IsTrue(_entity.WorkOrder.MarkoutRequired, "Sanity");
            var expectedDateOfRequest = DateTime.Today;
            _viewModel.DateOfRequest = expectedDateOfRequest;
            var expectedExpirationDate = WorkOrdersWorkDayEngine.GetExpirationDate(expectedDateOfRequest, markoutRequirement.MarkoutRequirementEnum, _entity.WorkOrder.WorkStarted);
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedExpirationDate, _entity.ExpirationDate);
        }

        [TestMethod]
        public void TestMapDoesNotExtendARoutineMarkoutExpirationDateWhenWorkStarted()
        {
            // we have a work order, started crew assignment
            // when we add a new routine markout, the new markout' expiration date should be standard,
            // and not extended due to the existing assignment

            // use a specific date, so we know the expected dates
            var now = new DateTime(2023, 8, 20, 8, 8, 8);

            var routineMarkoutRequirement = GetFactory<RoutineMarkoutRequirementFactory>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new { MarkoutRequirement = routineMarkoutRequirement });
            workOrder.OperatingCenter.MarkoutsEditable = false;
            _viewModel.WorkOrder = workOrder.Id;
            _viewModel.DateOfRequest = now;

            var crew = GetEntityFactory<Crew>().Create(new { OperatingCenter = workOrder.OperatingCenter });
            // danger, WorkStarted uses DateTime.Now
            var assignment = GetEntityFactory<CrewAssignment>().Create(new { WorkOrder = workOrder, Crew = crew, DateStarted = now.AddDays(5) });
            workOrder.CrewAssignments.Add(assignment);
            Session.Flush();

            _vmTester.MapToEntity();

            // these are testing a specific date, so we don't have to deal with testing the work day engine which is already tested
            Assert.AreEqual(now.AddDays(5), _entity.ReadyDate, "A markout called in on 8/20/2023 is ready 5 days after its called in");
            Assert.AreEqual(now.AddDays(16), _entity.ExpirationDate, "A markout call in on 8/20/2023 will have an expiration date of 9/5");
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestSimpleRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DateOfRequest);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MarkoutNumber);
        }

        [TestMethod]
        public void TestMarkoutNumberMustBe9CharactersInLengthIfWorkOrderOperatingCenterMarkoutEditableIsFalse()
        {
            //_entity.WorkOrder.AssignedContractor = _user.Contractor;
            //Session.Save(_entity.WorkOrder);
            //Session.Flush();

            Assert.AreEqual(_entity.WorkOrder.Id, _viewModel.WorkOrder);
            var opc = _entity.WorkOrder.OperatingCenter;
         //   _user.Contractor.OperatingCenters.Add(opc);
            opc.MarkoutsEditable = true;

            _viewModel.MarkoutNumber = "123456789";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.MarkoutNumber);

            _viewModel.MarkoutNumber = "12345678";
            ValidationAssert.ModelStateHasError(_viewModel, x => x.MarkoutNumber, "The field MarkoutNumber must be a string with a minimum length of 9 and a maximum length of 20.");

            _viewModel.MarkoutNumber = "123456789";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.MarkoutNumber);
        }

        [TestMethod]
        public void TestNoteIsRequiredWhenMarkoutTypeIdIs38()
        {
            _viewModel.Note = null;
            _viewModel.MarkoutType = GetFactory<MarkoutTypeFactory>().Create().Id;

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Note);

            var markoutNone = GetFactory<NoneMarkoutTypeFactory>().Create();
            _viewModel.MarkoutType = markoutNone.Id;

            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Note, "Required.");

            _viewModel.Note = "Blah";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Note);
        }

        [TestMethod]
        public void TestReadyDateIsRequiredWhenWorkOrderOperatingCenterMarkoutsEditable()
        {
            _entity.WorkOrder.OperatingCenter.MarkoutsEditable = false;
            _viewModel.ReadyDate = null;

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ReadyDate);

            _entity.WorkOrder.OperatingCenter.MarkoutsEditable = true;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ReadyDate);

            _viewModel.ReadyDate = DateTime.Now;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ReadyDate);
        }

        [TestMethod]
        public void TestExpirationDateIsRequiredWhenWorkOrderOperatingCenterMarkoutsEditable()
        {
            _entity.WorkOrder.OperatingCenter.MarkoutsEditable = false;
            _viewModel.ExpirationDate = null;

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ExpirationDate);

            _entity.WorkOrder.OperatingCenter.MarkoutsEditable = true;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ExpirationDate);

            _viewModel.ExpirationDate = DateTime.Now;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ExpirationDate);
        }


        #endregion

        #endregion
    }
}
