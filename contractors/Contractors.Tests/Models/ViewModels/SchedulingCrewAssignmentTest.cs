using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Utilities;
using StructureMap;
using CrewAssignmentRepository = Contractors.Data.Models.Repositories.CrewAssignmentRepository;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass]
    public class SchedulingCrewAssignmentTest : MapCallMvcInMemoryDatabaseTestBase<CrewAssignment>
    {
        private IViewModelFactory _viewModelFactory;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<ContractorUser>>().Use(
                (_authenticationService =
                    new MockAuthenticationService<ContractorUser>(_user))
               .Object);
            e.For<IDateTimeProvider>().Use<DateTimeProvider>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<Data.Models.Repositories.IWorkOrderRepository>()
             .Use<WorkOrderRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModelFactory = _container.GetInstance<ViewModelFactory>();

            _user = GetFactory<ContractorUserFactory>().Create();
            _crew = GetFactory<CrewFactory>().Create(new {_user.Contractor});

            _authenticationService.SetUser(_user);

            _crewAssRepo = _container.GetInstance<CrewAssignmentRepository>();
            _workOrderRepo = _container.GetInstance<WorkOrderRepository>();
        }

        #endregion

        #region Fields

        private CrewAssignmentRepository _crewAssRepo;
        private WorkOrderRepository _workOrderRepo;

        private ContractorUser _user;
        private Crew _crew;

        private MockAuthenticationService<ContractorUser> _authenticationService;

        #endregion

        #region Validate

        [TestMethod]
        public void TestValidateThrowsNoSuchWorkOrderErrorWhenOrderDoesNotExist()
        {
            var id = 666;
            var target = new SchedulingCrewAssignment(_container)
                {Crew = _crew.Id};
            target.WorkOrderIDs.Add(id);

            var result = target.Validate(null).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(String.Format(CrewAssignment.ModelErrors.NO_SUCH_WORK_ORDER,id), result[0].ErrorMessage);
        }

        [TestMethod]
        public void TestValidateErrorsWhenAssignedForAfterMarkoutExpiredOrBeforeReadyDate()
        {
            var workOrder = GetFactory<SchedulingWorkOrderFactory>().Create(new { AssignedContractor = _user.Contractor});
            Session.Flush();
            Session.Clear();
            workOrder = Session.Load<WorkOrder>(workOrder.Id);
            var target = new SchedulingCrewAssignment(_container) {
                AssignFor =
                    workOrder.CurrentMarkout.ExpirationDate.Value.AddDays(1),
                    Crew = _crew.Id
            };
            target.WorkOrderIDs.Add(workOrder.Id);
            Session.Flush();
            var result = target.Validate(null).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(String.Format(CrewAssignment.ModelErrors.INVALID_MARKOUT, workOrder.Id), result[0].ErrorMessage);

            target.AssignFor =
                workOrder.CurrentMarkout.ReadyDate.Value.AddDays(-1);

            result = target.Validate(null).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(String.Format(CrewAssignment.ModelErrors.INVALID_MARKOUT, workOrder.Id), result[0].ErrorMessage);
        }

        [TestMethod]
        public void TestValidateDoesNotReturnErrorIfStreetOpeningPermitNotRequiredAndInvalidStreetOpeningPermitsExist()
        {
            var workOrder = GetFactory<SchedulingWorkOrderFactory>().Create(new { AssignedContractor = _user.Contractor });
            Session.Flush();
            Session.Clear();
            workOrder = Session.Load<WorkOrder>(workOrder.Id);
            var target = new SchedulingCrewAssignment(_container) {
                AssignFor =
                    workOrder.CurrentMarkout.ExpirationDate.Value.AddDays(-1),
                Crew = _crew.Id
            };
            target.WorkOrderIDs.Add(workOrder.Id);
            var permit = GetFactory<StreetOpeningPermitFactory>().Create(new { WorkOrder = workOrder, DateIssued = DateTime.Now });
            workOrder.StreetOpeningPermits.Add(permit);
            Session.Flush();
            var result = target.Validate(null).ToArray();

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void TestValidateReturnsErrorIfStreetOpeningPermitRequiredAndIsAssignedForADateOutsideTheRange()
        {
            var workOrder = GetFactory<SchedulingWorkOrderFactory>().Create(new
            {
                AssignedContractor = _user.Contractor,
                StreetOpeningPermitRequired = true
            });
            var markout = GetFactory<MarkoutFactory>().Create(new {
                DateOfRequest = DateTime.Now,
                ReadyDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(10),
                WorkOrder = workOrder
            });
            workOrder.Markouts.Add(markout);
            Session.Flush();
            Session.Clear();
            workOrder = Session.Load<WorkOrder>(workOrder.Id);
            var permit = GetFactory<StreetOpeningPermitFactory>().Create(new { WorkOrder = workOrder, DateIssued = DateTime.Now.AddDays(1), ExpirationDate = DateTime.Now.AddDays(2) });
            workOrder.StreetOpeningPermits.Add(permit);
            var target = new SchedulingCrewAssignment(_container) {
                AssignFor =
                    workOrder.CurrentMarkout.ExpirationDate.Value.AddDays(-1),
                Crew = _crew.Id
            };
            target.WorkOrderIDs.Add(workOrder.Id);
            Session.Flush();
            var result = target.Validate(null).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(String.Format(CrewAssignment.ModelErrors.INVALID_PERMIT, workOrder.Id), result[0].ErrorMessage);
        }

        [TestMethod]
        public void TestValidateErrorsWhenAssignedForBeforeExpiredAndAfterReadyDateOfInvalidMarkout()
        {
            var workOrder = GetFactory<SchedulingWorkOrderFactory>().Create(new { AssignedContractor = _user.Contractor });
            Session.Flush();
            Session.Clear();
            workOrder = Session.Load<WorkOrder>(workOrder.Id);
            var target = new SchedulingCrewAssignment(_container) {
                AssignFor =
                    workOrder.CurrentMarkout.ExpirationDate.Value.AddDays(1),
                Crew = _crew.Id
            };
            workOrder.Markouts.Clear();
            var markout = GetFactory<MarkoutFactory>().Create(new {
                DateOfRequest = DateTime.Today.AddDays(-10),
                ReadyDate = DateTime.Today.AddDays(-9),
                ExpirationDate = DateTime.Today.AddDays(-1),
                WorkOrder = workOrder
            });
            workOrder.Markouts.Add(markout);
            target.WorkOrderIDs.Add(workOrder.Id);
            var result = target.Validate(null).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(String.Format(CrewAssignment.ModelErrors.INVALID_MARKOUT, workOrder.Id), result[0].ErrorMessage);
        }

        #endregion
    }
}
