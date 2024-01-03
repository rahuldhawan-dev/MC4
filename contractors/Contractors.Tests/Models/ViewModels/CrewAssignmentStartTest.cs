using System;
using System.Linq;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using CrewAssignmentRepository = Contractors.Data.Models.Repositories.CrewAssignmentRepository;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass]
    public class CrewAssignmentStartTest : InMemoryDatabaseTest<CrewAssignment>
    {
        #region Fields

        private CrewAssignmentRepository _crewAssRepo;
        private WorkOrderRepository _workOrderRepo;

        private ContractorUser _user;
        private Crew _crew;

        private MockAuthenticationService<ContractorUser> _authenticationService;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<ContractorUser>>()
             .Singleton()
             .Use<MockAuthenticationService<ContractorUser>>();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<Data.Models.Repositories.ICrewAssignmentRepository>().Use<CrewAssignmentRepository>();
            e.For<Data.Models.Repositories.IWorkOrderRepository>()
             .Use<WorkOrderRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _authenticationService =
                (MockAuthenticationService<ContractorUser>)_container
                   .GetInstance<IAuthenticationService<ContractorUser>>();

            _user = GetFactory<ContractorUserFactory>().Create();
            _crew = GetFactory<CrewFactory>().Create(new { Contractor = _user.Contractor });
            _authenticationService.SetUser(_user);

            _crewAssRepo = _container.GetInstance<CrewAssignmentRepository>();
            _workOrderRepo = _container.GetInstance<WorkOrderRepository>();
        }

        #endregion

        #region MapToEntity

        [TestMethod]
        public void TestMapToEntitySetsExpirationDateIfWorkOrderHasCurrentMarkout()
        {
            var wo = GetFactory<MarkoutRequirementRoutineWithAValidMarkoutWorkOrderFactory>().Create();
            wo.OperatingCenter.MarkoutsEditable = false;
            Session.Flush();
            Session.Clear();
            wo = Session.Load<WorkOrder>(wo.Id);
            var currentMarkout = wo.CurrentMarkout.Markout;
            var expectedExpirationDate = WorkOrdersWorkDayEngine.GetExpirationDate(currentMarkout.DateOfRequest.Value, wo.MarkoutRequirement.MarkoutRequirementEnum, true);
            var crewAssignment = GetEntityFactory<CrewAssignment>().Create(new {
                WorkOrder = wo,
                Crew = _crew
            });

            var target = _container.GetInstance<CrewAssignmentStart>();
            target.MapToEntity(crewAssignment);

            Session.Save(crewAssignment);
            Session.Flush();
            Session.Clear();
            wo = Session.Load<WorkOrder>(wo.Id);

            Assert.AreEqual(expectedExpirationDate, wo.CurrentMarkout.ExpirationDate);
        }

        [TestMethod]
        public void TestMapToEntityDoesNOTSetExpirationDateIfWorkOrderHasCurrentMarkOutAndWorkOrderOperatingCenterHasMarkoutsEditable()
        {
            var wo = GetFactory<MarkoutRequirementRoutineWithAValidMarkoutWorkOrderFactory>().Create();
            wo.OperatingCenter.MarkoutsEditable = true;
            Session.Flush();
            Session.Clear();
            wo = Session.Load<WorkOrder>(wo.Id);
            var currentMarkout = wo.CurrentMarkout;
            var expectedExpirationDate = new DateTime(1984, 4, 24);
            currentMarkout.ExpirationDate = expectedExpirationDate;
            var crewAssignment = new CrewAssignment();
            crewAssignment.WorkOrder = wo;

            var target = _container.GetInstance<CrewAssignmentStart>();
            target.MapToEntity(crewAssignment);

            Assert.AreEqual(expectedExpirationDate, crewAssignment.WorkOrder.CurrentMarkout.ExpirationDate);
        }

        #endregion

        #region Validate

        private void VerifyValidationMessage(CrewAssignmentStart model, string expectedMessage)
        {
            var results = model.Validate(null).ToArray();
            if (!results.Any(x => x.ErrorMessage == expectedMessage))
            {
                if (!results.Any())
                {
                    Assert.Fail("No error message was returned. Expected error message: " + expectedMessage);
                }
                else
                {

                    Assert.Fail("Expected error message not returned: " + expectedMessage + ", but did get error: " + results.First().ErrorMessage);
                }
            }
        }

        private void VerifyNoValidationErrors(CrewAssignmentStart model)
        {
            var results = model.Validate(null).ToArray();
            if (results.Any())
            {
                Assert.Fail("Validation error occurred: " +
                    results.First().ErrorMessage);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFactory"></typeparam>
        /// <param name="errorMessage">Leave null if there shouldn't be an error message</param>
        private void Verify<TFactory>(string errorMessage = null, DateTime? dateStarted = null) where TFactory : TestDataFactory<WorkOrder>
        {
            // TODO: Literally no idea what this is for and why we are setting the view model's DateStarted property for this test. 
            var wo = GetFactory<TFactory>().Create(new
            {
                AssignedContractor = _user.Contractor
            });
            var ass = GetFactory<CrewAssignmentFactory>().Create(new { WorkOrder = wo, Crew = _crew });
            if (dateStarted.HasValue)
            {
                ass.DateStarted = dateStarted;
            }

            var model = _container.GetInstance<CrewAssignmentStart>();
            model.Map(ass);

            if (errorMessage == null)
            {
                VerifyNoValidationErrors(model);
            }
            else
            {
                VerifyValidationMessage(model, errorMessage);
            }
        }

        #region FAILS

        // KINDLY ENJOY MY UNDERSCORED NAMES BECAUSE I CAN'T READ THESE TEST NAMES ANYMORE

        [TestMethod]
        public void Test_ValidateFails_IfWorkOrderIsCompleted()
        {
            var workOrder = GetFactory<CompletedWorkOrderFactory>().Create(new { AssignedContractor = _user.Contractor });
            var ass = GetFactory<CrewAssignmentFactory>().Create(new { WorkOrder = workOrder, Crew = _crew });
            var model = _container.GetInstance<CrewAssignmentStart>();
            model.Map(ass);
            VerifyValidationMessage(model, CrewAssignmentStart.ModelErrors.WORK_ORDER_ALREADY_COMPLETED);
        }

        [TestMethod]
        public void Test_ValidateFails_WhenAssignmentHasAlreadyStarted()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _user.Contractor, DateCompleted = DateTime.Now });
            var ass = GetFactory<CrewAssignmentFactory>().Create(new { WorkOrder = workOrder, Crew = _crew, DateStarted = DateTime.Now });
            var model = _container.GetInstance<CrewAssignmentStart>();
            model.Map(ass);
            VerifyValidationMessage(model, CrewAssignmentStart.ModelErrors.ALREADY_STARTED);
        }

        [TestMethod]
        public void Test_ValidateFails_WhenWorkOrderRequiresStreetOpeningPermitButDoesntHaveAValidOne()
        {
            Verify<StreetOpeningPermitRequiredWorkOrderFactory>(CrewAssignmentStart.ModelErrors.NO_VALID_PERMIT);
        }

        [TestMethod]
        public void Test_ValidateFails_WhenWorkOrderRequiresStreetOpeningPermitAndHasAnExpiredStreetOpeningPermit()
        {
            Verify<StreetOpeningPermitRequiredWithExpiredPermitWorkOrderFactory>(CrewAssignmentStart.ModelErrors.NO_VALID_PERMIT);
        }

        [TestMethod]
        public void Test_ValidateFails_WorkOrderWithARoutineMarkoutRequirementWithAnExpiredMarkout()
        {
            Verify<MarkoutRequirementRoutineWithAnExpiredMarkoutWorkOrderFactory>(CrewAssignmentStart.ModelErrors.NO_VALID_MARKOUT);
        }

        [TestMethod]
        public void Test_ValidateFails_WorkOrderWithARoutineMarkoutRequirementWithNoMarkout()
        {
            Verify<MarkoutRequirementRoutineWorkOrderFactory>(CrewAssignmentStart.ModelErrors.NO_VALID_MARKOUT);
        }

        [TestMethod]
        public void Test_ValidateFails_WorkOrderWithAStreetOpeningPermitRequiredWithAStreetOpeningPermitInTheFuture()
        {
            Verify<StreetOpeningPermitRequiredWithAStreetOpeningPermitInTheFutureWorkOrderFactory>(CrewAssignmentStart.ModelErrors.NO_VALID_PERMIT, DateTime.Now.AddDays(-40));
        }

        [TestMethod]
        public void Test_ValidateFails_WorkOrderWithARoutineMarkoutAndAMarkoutExistsThatIsNotReadyYet()
        {
            Verify<MarkoutRequirementRoutineWithAValidFutureMarkoutWorkOrderFactory>(CrewAssignmentStart.ModelErrors.NO_VALID_MARKOUT, DateTime.Now.AddDays(-40));
        }

        #endregion

        #region PASSES

        [TestMethod]
        public void Test_ValidatePasses_WhenWorkOrderHasNoMarkoutRequirementsOrStreetOpeningPermitRequirements()
        {
            Verify<WorkOrderFactory>();
        }

        [TestMethod]
        public void Test_ValidatePasses_WhenWorkOrderWithAnEmergencyMarkoutAndNoMarkoutExists()
        {
            Verify<MarkoutRequirementEmergencyWorkOrderFactory>();
        }

        [TestMethod]
        public void Test_ValidatePasses_WhenWorkOrderWithAnEmergencyMarkoutAndAnExpiredMarkoutAndASandwichExists()
        {
            Verify<MarkoutRequirementEmergencyWithExpiredMarkoutWorkOrderFactory>();
        }

        [TestMethod]
        public void Test_ValidatePasses_WorkOrderWithARoutineMarkoutAndAValidMarkoutExists()
        {
            Verify<MarkoutRequirementRoutineWithAValidMarkoutWorkOrderFactory>();
        }

        [TestMethod]
        public void Test_ValidatePasses_WorkOrderWithAnEmergencyMarkoutAndValidMarkoutExists()
        {
            Verify<MarkoutRequirementEmergencyWithValidMarkoutWorkOrderFactory>();
        }

        [TestMethod]
        public void Test_ValidatePasses_WorkOrderWithAnEmergencyMarkoutRequirementAndPermitRequirement()
        {
            Verify<MarkoutRequirementEmergencyPermitRequiredWorkOrderFactory>();
        }

        [TestMethod]
        public void Test_ValidatePasses_WorkOrderWithEmergencyPriorityWithAStreetOpeningPermitRequiredWithoutAnStreetOpeningPermit()
        {
            Verify<StreetOpeningPermitRequiredEmergencyPriorityWithoutAnStreetOpeningPermitWorkOrderFactory>();
        }

        [TestMethod]
        public void Test_ValidatePasses_WorkOrderWithAStreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermitWithinDateRange()
        {
            Verify<StreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermitWorkOrderFactory>();
        }

        #endregion

        #endregion
    }
}
