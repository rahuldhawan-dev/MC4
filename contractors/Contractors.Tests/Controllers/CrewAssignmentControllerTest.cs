using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Contractors.Controllers;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using Moq;
using NHibernate.Linq;
using StructureMap;
using AuthenticationRepository = Contractors.Data.Models.Repositories.AuthenticationRepository;
using CrewAssignmentRepository = Contractors.Data.Models.Repositories.CrewAssignmentRepository;
using IWorkOrderRepository = Contractors.Data.Models.Repositories.IWorkOrderRepository;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class CrewAssignmentControllerTest : ContractorControllerTestBase<CrewAssignmentController, CrewAssignment, CrewAssignmentRepository>
    {
        #region Private Members

        private CrewAssignment _ass;
        private Crew _crew;
        private MapCall.Common.Model.Entities.WorkOrder _workOrder;
        private Mock<HttpContextBase> _httpContext;
        private Mock<HttpRequestBase> _httpRequest;

        #endregion

        #region Setup/Teardown

        protected override ContractorUser CreateUser()
        {
            var user = base.CreateUser();
            _crew = GetFactory<CrewFactory>().Create(new { user.Contractor, Availability = 8m, Description = "Hello" });
            return user;
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationRepository<ContractorUser>>()
             .Use<AuthenticationRepository>();
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject(Session);

            // TODO: Make this not suck.
            _currentUser.Contractor.Crews.Add(_crew);

            _workOrder = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor
            });
            _ass = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = _workOrder,
                Crew = _crew
            });
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var workOrder = GetFactory<WorkOrderFactory>().Create(new {
                    AssignedContractor = _currentUser.Contractor
                });
                return GetFactory<CrewAssignmentFactory>().Create(new {
                    WorkOrder = workOrder,
                    Crew = _crew
                });
            };
            options.DestroyReturnsHttpStatusCodeNoContentOnSuccess = true;
        }

        #endregion

        #region Private Methods

        private void VerifyRedirectAndError<TFactory>(Func<SchedulingCrewAssignment, string> errorMessageFn)
            where TFactory : TestDataFactory<MapCall.Common.Model.Entities.WorkOrder>
        {
            VerifyRedirectAndError<TFactory>(null, null, errorMessageFn);
        }
        
        private void VerifyRedirectAndError<TFactory>(string errorMessage = null, DateTime? assignFor = null, Func<SchedulingCrewAssignment, string> errorMessageFn = null)
            where TFactory : TestDataFactory<MapCall.Common.Model.Entities.WorkOrder>
        {
            errorMessageFn = errorMessageFn ?? (ca => errorMessage);
            var wo = GetFactory<TFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor
            });

            var model = new SchedulingCrewAssignment(_container) {
                AssignFor = assignFor ?? DateTime.Today,
                Crew = _crew.Id,
                WorkOrderIDs = {wo.Id}
            };
            Session.Flush();
            _target.RunModelValidation(model);
            var result = _target.Create(model);
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual("WorkOrderScheduling", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.AreEqual(errorMessageFn(model),_target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        #endregion

        #region Tests

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/CrewAssignment/Destroy");
                a.RequiresLoggedInUserOnly("~/CrewAssignment/Create");
                a.RequiresLoggedInUserOnly("~/CrewAssignment/End");
                a.RequiresLoggedInUserOnly("~/CrewAssignment/Index");
                a.RequiresLoggedInUserOnly("~/CrewAssignment/IndexTabs");
                a.RequiresSiteAdminUser("~/CrewAssignment/Manage");
                a.RequiresLoggedInUserOnly("~/CrewAssignment/ShowCalendar");
                a.RequiresLoggedInUserOnly("~/CrewAssignment/Start");
                a.RequiresSiteAdminUser("~/CrewAssignment/UpdatePriority");
            });
        }

        #endregion

        #region ShowCalendar

        [TestMethod]
        public void TestShowCalendarReturnsViewWithCrewAssignmentCalendarSearchAsModel()
        {
            var result = (ViewResult)_target.ShowCalendar(new CrewAssignmentCalendarSearch { Crew = _crew.Id, Date = DateTime.Today });
          //  var result = (ViewResult)_target.ShowCalendar(_crew.Id, DateTime.Today);
            var resultModel = (CrewAssignmentCalendarSearch)result.Model;
            Assert.AreEqual(_crew.Id, resultModel.Crew);
            Assert.AreEqual(DateTime.Today, resultModel.Date);
        }

        [TestMethod]
        public void TestShowCalendarSetsDateToTodayIfDateIsNotSet()
        {
            _target.ModelState.AddModelError("Date", "Nope");
            var result = (ViewResult)_target.ShowCalendar(new CrewAssignmentCalendarSearch { Crew = _crew.Id, Date = null});
            var model = (CrewAssignmentCalendarSearch)result.Model;
            Assert.AreEqual(DateTime.Today, model.Date);
        }

        [TestMethod]
        public void TestShowCalendarAddsCrewsToViewData()
        {
            var expectedCrew = _currentUser.Contractor.Crews.First();

            _target.ShowCalendar(new CrewAssignmentCalendarSearch { Crew = _crew.Id, Date = DateTime.Today });

            var result = (IEnumerable<SelectListItem>)_target.ViewData[CrewAssignmentController.ViewDataKeys.CREW];
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(x => x.Value == expectedCrew.Id.ToString()));
        }

        #endregion

        #region End

        [TestMethod]
        public void TestEndHasHttpPostAttribute()
        {
            MyAssert.MethodHasAttribute<HttpPostAttribute>(_target, "End",
                typeof(CrewAssignmentEnd));
        }

        [TestMethod]
        public void TestEndRedirectsWithErrorsIfModelStateIsInvalid()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create(new
            {
                AssignedContractor = _currentUser.Contractor,
                MarkoutRequirement = GetFactory<MarkoutRequirementRoutineFactory>().Create()
            });
            var ass = GetFactory<CrewAssignmentFactory>().Create(new
            {
                WorkOrder = workOrder,
                Crew = _crew,
                DateStarted = DateTime.Today 
            });

            var id = ass.Id;
            var expected = "Error Message";
            var model = _container.GetInstance<CrewAssignmentEnd>();
            model.Id = id;
            _target.ModelState.AddModelError("uhderp", expected);
            
            var result = _target.End(model);

            var redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("WorkOrderFinalization", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Edit", redirectResult.RouteValues["action"]);
            Assert.AreEqual(ass.WorkOrder.Id, redirectResult.RouteValues["id"]);
            Assert.AreEqual(expected, _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestEndReturns404IfCrewAssignmentIsNotFound()
        {
            var id = -3;
            var model = _container.GetInstance<CrewAssignmentEnd>();
            model.Id = id;
            MvcAssert.IsStatusCode(404,
                _target.End(model),
                CrewAssignmentController.NO_CREW_ASSIGNMENT_FOUND);
        }

        [TestMethod]
        public void TestEndRedirectsWithErrorIfCrewAssignmentEndDateIsAlreadySet()
        {
            // TODO: This should be a view model test, not a controller test.
            _ass.DateEnded = DateTime.Now;
            Session.Save(_ass);
            Session.Clear();

            var model = _container.GetInstance<CrewAssignmentEnd>();
            model.Id = _ass.Id;
            model.EmployeesOnJob = 1;

            _target.RunModelValidation(model);
            var result = _target.End(model);
            
            var redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("WorkOrderFinalization", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Edit", redirectResult.RouteValues["action"]);
            Assert.AreEqual(_ass.WorkOrder.Id, redirectResult.RouteValues["id"]);
            Assert.AreEqual(CrewAssignmentEnd.ModelErrors.ALREADY_ENDED,
                _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestEndSetsDateEndedToNow()
        {
            _target.End(_viewModelFactory.Build<CrewAssignmentEnd, CrewAssignment>(_ass));
            
            var result = Repository.Find(_ass.Id);

            Assert.IsNotNull(result.DateEnded);
            MyAssert.AreClose(DateTime.Now, result.DateEnded.Value);
        }

        [TestMethod]
        public void TestEndSetsEmployeesOnJobToModelValue()
        {
            var expected = 542f;
            _target.End(
                _viewModelFactory
                   .BuildWithOverrides<CrewAssignmentEnd, CrewAssignment>(_ass,
                        new {EmployeesOnJob = expected}));
            var result = Repository.Find(_ass.Id);
            Assert.AreEqual(expected, result.EmployeesOnJob);
        }

        [TestMethod]
        public void TestEndReturnsRedirectToWorkOrderFinalizationForCrewAssignmentsWorkOrderID()
        {
            _ass.DateStarted = DateTime.Now;
            Session.Save(_ass);
            Session.Clear();

            var model = _container.GetInstance<CrewAssignmentEnd>();
            model.Id = _ass.Id;
            var result = _target.End(model);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("WorkOrderFinalization", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Edit", redirectResult.RouteValues["action"]);
            Assert.AreEqual(_ass.WorkOrder.Id, redirectResult.RouteValues["id"]);
        }
        
        #endregion

        #region Index

        [TestMethod]
        public void TestIndexReturnsPartialViewWithCrewAssignmentIndexIfWorkOrderExists()
        {
            Assert.Inconclusive("TODO");
        }

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            // noop override: Index does not act like typical Index action.
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // noop override: Index does not act like typical Index action.
        }

        [TestMethod]
        public void TestIndexReturns404IfWorkOrderIsNotFound()
        {
            MvcAssert.IsStatusCode(404, _target.Index(0));
        }

        private void SetupRequestContext()
        {
            _httpContext = new Mock<HttpContextBase>();
            _httpRequest = new Mock<HttpRequestBase>();
            _httpContext.Setup(x => x.Request).Returns(_httpRequest.Object);
            _target.ControllerContext = new ControllerContext(
                _httpContext.Object, new RouteData(), _target);
        }

        [TestMethod]
        public void TestIndexReturnsModelWithIsFinalizationViewSetToTrueIfReferrerIsWorkOrderFinalizatiion()
        {
            Assert.Inconclusive("TODO: This may not be testable due to how the RouteCollection.GetRouteData extension works");
            RouteTable.Routes.MapRoute(
               "Default", // Route name
               "{controller}/{action}/{id}", // URL with parameters
               new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
           );
            SetupRequestContext();
            _httpRequest.Setup(x => x.UrlReferrer).Returns(new Uri("http://wwww.batman.com/WorkOrderFinalization/Edit/32523"));
            _httpRequest.Setup(x => x.AppRelativeCurrentExecutionFilePath).Returns("~/WorkOrderFinalization/Edit/32532");

            var result =
                (PartialViewResult)_target.Index(_workOrder.Id);
            var model = (CrewAssignmentIndex)result.Model;
            Assert.IsTrue(model.IsFinalizationView);
        }
        
        #endregion

        #region Start

        [TestMethod]
        public void TestStartHasHttpPostAttribute()
        {
            MyAssert.MethodHasAttribute<HttpPostAttribute>(_target, "Start", typeof(CrewAssignmentStart));
        }

        [TestMethod]
        public void TestStartReturns404ForInvalidCrewAssignmentID()
        {
            var id = -3;
            var model = _container.GetInstance<CrewAssignmentStart>();
            model.Id = id;
            MvcAssert.IsStatusCode(404,
                _target.Start(model),
                CrewAssignmentController.NO_CREW_ASSIGNMENT_FOUND);
        }

        [TestMethod]
        public void TestStartReturnsRedirectToWorkOrderIfCrewAssignmentHasAlreadyBeenStarted()
        {
            // TODO: This should be a view model test, not a controller test.
            _ass.DateStarted = DateTime.Now;
            Session.Save(_ass);

            var model = _viewModelFactory.Build<CrewAssignmentStart, CrewAssignment>(_ass);
            _target.RunModelValidation(model);
            var result = _target.Start(model);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual("WorkOrderFinalization", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Edit", redirectResult.RouteValues["action"]);
            Assert.AreEqual(_ass.WorkOrder.Id, redirectResult.RouteValues["id"]);
            Assert.AreEqual(CrewAssignmentStart.ModelErrors.ALREADY_STARTED,
                _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestStartReturnsRedirectToWorkOrderIfMarkoutIsRequiredAndInvalid()
        {
            // TODO: This should be a view model test, not a controller test.
            var workOrder = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                MarkoutRequirement = GetFactory<MarkoutRequirementRoutineFactory>().Create()
            });
            var ass = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = workOrder,
                Crew = _crew
            });
            var expectedMarkout = GetFactory<MarkoutFactory>().Create(new {
                DateOfRequest = DateTime.Now.AddDays(-10),
                ReadyDate = DateTime.Now.AddDays(-2),
                ExpirationDate = DateTime.Now.AddDays(-1),
                WorkOrder = workOrder,
            });
            Session.Flush();
            Session.Clear();

            var model = _viewModelFactory.Build<CrewAssignmentStart, CrewAssignment>(ass);
            _target.RunModelValidation(model);
            var result = _target.Start(model);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual("WorkOrderFinalization", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Edit", redirectResult.RouteValues["action"]);
            Assert.AreEqual(ass.WorkOrder.Id, redirectResult.RouteValues["id"]);
            Assert.AreEqual(CrewAssignmentStart.ModelErrors.NO_VALID_MARKOUT,
                _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestStartReturnsRedirectToWorkOrderIfStreetOpeningPermitIsRequiredAndInvalid()
        {
            // TODO: This should be a view model test, not a controller test.
            var workOrder = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                StreetOpeningPermitRequired = true
            });
            var ass = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = workOrder,
                Crew = _crew
            });
            var permit = GetFactory<StreetOpeningPermitFactory>().Create(new {
                WorkOrder = workOrder,
                ExpirationDate = DateTime.Now.AddDays(-1)
            });
            Session.Flush();
            Session.Clear();

            var model = _viewModelFactory.Build<CrewAssignmentStart, CrewAssignment>(ass);
            _target.RunModelValidation(model);
            var result = _target.Start(model);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual("WorkOrderFinalization", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Edit", redirectResult.RouteValues["action"]);
            Assert.AreEqual(ass.WorkOrder.Id, redirectResult.RouteValues["id"]);
            Assert.AreEqual(CrewAssignmentStart.ModelErrors.NO_VALID_PERMIT,
                _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestStartExtendsMarkoutExpirationIfCurrentMarkoutIsNotNull()
        {
            var wo = GetFactory<SchedulingWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                    MarkoutsEditable = false
                })
            });
            var ass = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = wo,
                Crew = _crew
            });
            Session.Flush();
            Session.Clear();

            _target.Start(_viewModelFactory.Build<CrewAssignmentStart, CrewAssignment>(ass));

            wo = Session.Load<MapCall.Common.Model.Entities.WorkOrder>(wo.Id);

            Assert.AreNotEqual(wo.MarkoutExpirationDate, default(DateTime));
            Assert.AreNotEqual(wo.CurrentMarkout.Markout.DateOfRequest,
                default(DateTime), "This test is not written properly.");
            Assert.AreNotEqual(wo.CurrentMarkout.ReadyDate, default(DateTime),
                "This test is not written properly.");
            Assert.IsTrue(
                (wo.MarkoutExpirationDate.Value - wo.CurrentMarkout.ReadyDate.Value).Days > 44,
                $"Expected time from MarkoutExpirationDate to MarkoutReadyDate to be > 44 days, instead was {(wo.MarkoutExpirationDate.Value - wo.CurrentMarkout.ReadyDate.Value).Days}");
        }

        [TestMethod]
        public void TestStartReturnsRedirectToWorkOrderFinalizationForCrewAssignmentsWorkOrderID()
        {
            var result = _target.Start(_viewModelFactory.Build<CrewAssignmentStart, CrewAssignment>(_ass));
            
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("WorkOrderFinalization", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Edit", redirectResult.RouteValues["action"]);
            Assert.AreEqual(_ass.WorkOrder.Id, redirectResult.RouteValues["id"]);
        }

        [TestMethod]
        public void TestStartSetsDateStartedOnCrewAssignmentToNow()
        {
            _target.Start(_viewModelFactory.Build<CrewAssignmentStart, CrewAssignment>(_ass));

            var result = Repository.Find(_ass.Id);
            Assert.IsNotNull(result.DateStarted);
            MyAssert.AreClose(DateTime.Now, result.DateStarted.Value);
        }

        #endregion

        #region Create

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // noop override: can't run test due to ViewModelSet. Also redirects back to WorkOrderScheduling.
            // tested below.
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // noop override: can't run test due to ViewModelSet. Also redirects back to WorkOrderScheduling.
            // tested below.
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // noop override: can't run test due to ViewModelSet. Also redirects back to WorkOrderScheduling.
            // tested below.
        }

        [TestMethod]
        public void TestCreateRedirectsBackToWorkOrderSchedulingIfModelStateIsNotValid()
        {
            _target.ModelState.AddModelError("nah", "nuh uh");

            var result = _target.Create(new SchedulingCrewAssignment(_container)) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("WorkOrderScheduling", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestCreateCreatesCrewAssignmentFromViewModelForEachWorkOrderIdAndRedirectsToCalendar()
        {
            var wo1 = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            var wo2 = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });

            var viewModel = new SchedulingCrewAssignment(_container)
            {
                AssignFor = DateTime.Today,
                Crew = _crew.Id,
                WorkOrderIDs = new[] { wo1.Id, wo2.Id}
            };

            var result = _target.Create(viewModel) as RedirectToRouteResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("ShowCalendar", result.RouteValues["action"]);
            Assert.AreEqual(viewModel.Crew, result.RouteValues["crew"]);
            Assert.AreEqual(viewModel.AssignFor.Value.ToShortDateString(), result.RouteValues["Date"]);
        }

        [TestMethod]
        public void TestCreateReturns404IfCrewDoesNotExist()
        {
            // TODO: This should be a view model test, not a controller test.
            var model = new SchedulingCrewAssignment(_container) { Crew = 0, WorkOrderIDs = new[] { 1234 } };
            _target.RunModelValidation(model);
            var result = _target.Create(model);

            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual("WorkOrderScheduling", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.AreEqual(SchedulingCrewAssignment.ModelErrors.NO_CREW_FOUND,
                _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestCreateReturns404IfUserDoesNotHaveAccessToWorkOrder()
        {
            // TODO: This should be a view model test, not a controller test.
            var workOrderId = 23532;
            var viewModel = new SchedulingCrewAssignment(_container) {
                AssignFor = DateTime.Today,
                Crew = _crew.Id,
                WorkOrderIDs = new[] { workOrderId }
            };

            _target.RunModelValidation(viewModel);
            var result = _target.Create(viewModel);
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual("WorkOrderScheduling", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.AreEqual(
                String.Format(CrewAssignment.ModelErrors.NO_SUCH_WORK_ORDER,
                    workOrderId),
                _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestCreateReturns404IfUserTriesToAssignForADateThatIsInvalidForTheMarkout()
        {
            // TODO: This should be a view model test, not a controller test.
            VerifyRedirectAndError<MarkoutRequirementRoutineWithAValidFutureMarkoutWorkOrderFactory>(
                ca => String
                   .Format(CrewAssignment.ModelErrors.INVALID_MARKOUT,
                        ca.WorkOrderIDs[0]));

            // need to clear out the modelstate
            _target.ModelState.Clear();

            VerifyRedirectAndError<MarkoutRequirementRoutineWithAValidFutureMarkoutWorkOrderFactory>(null,
                DateTime.Now.AddDays(40),
                ca => String
                   .Format(CrewAssignment.ModelErrors.INVALID_MARKOUT,
                        ca.WorkOrderIDs[0]));
        }

        [TestMethod]
        public void TestReturns404IfUserTriesToAssignWhenStreetOpeningPermitRequiredButDoesNotExist()
        {
            VerifyRedirectAndError<StreetOpeningPermitRequiredWorkOrderFactory>(
                ca => String
                    .Format(CrewAssignment.ModelErrors.NO_SUCH_WORK_ORDER,
                        ca.WorkOrderIDs[0]));
        }

        [TestMethod]
        public void TestReturns404IfUserTriesToAssignWhenStreetOpeningPermitRequiredButIsExpired()
        {
            VerifyRedirectAndError<StreetOpeningPermitRequiredWithExpiredPermitWorkOrderFactory>(
                ca => String
                    .Format(CrewAssignment.ModelErrors.NO_SUCH_WORK_ORDER,
                        ca.WorkOrderIDs[0]));
        }

        [TestMethod]
        public void TestReturns404IfUserTriesToAssignWhenMarkoutRequirementRoutineButDoesNotHaveAMarkout()
        {
            VerifyRedirectAndError<MarkoutRequirementRoutineWorkOrderFactory>(
                ca => String
                    .Format(CrewAssignment.ModelErrors.NO_SUCH_WORK_ORDER,
                        ca.WorkOrderIDs[0]));
        }

        [TestMethod]
        public void TestReturns404IfUserTriesToAssignWhenMarkoutRequirementRoutineAndTheMarkoutIsExpired()
        {
            VerifyRedirectAndError<MarkoutRequirementRoutineWithAnExpiredMarkoutWorkOrderFactory>(
                ca => String
                    .Format(CrewAssignment.ModelErrors.NO_SUCH_WORK_ORDER,
                        ca.WorkOrderIDs[0]));
        }

        [TestMethod]
        public void TestCreateCreatesCrewAssignmentsForValidWorkOrderFactories()
        {
            var wo1 = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            var wo2 = GetFactory<StreetOpeningPermitRequiredEmergencyPriorityWithoutAnStreetOpeningPermitWorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            var wo3 = GetFactory<StreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermitWorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            var wo4 = GetFactory<MarkoutRequirementEmergencyWorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            var wo5 = GetFactory<MarkoutRequirementEmergencyWithValidMarkoutWorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            var wo6 = GetFactory<MarkoutRequirementEmergencyWithExpiredMarkoutWorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            var wo7 = GetFactory<MarkoutRequirementEmergencyPermitRequiredWorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor, Priority = typeof(EmergencyWorkOrderPriorityFactory)});
            var wo8 = GetFactory<MarkoutRequirementRoutineWithAValidMarkoutWorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            Session.Flush();
            var viewModel = new SchedulingCrewAssignment(_container) {
                AssignFor = DateTime.Today,
                Crew = _crew.Id,
                WorkOrderIDs = new[] {
                    wo1.Id, 
                    wo2.Id, 
                    wo3.Id, //////
                    wo4.Id, 
                    wo5.Id, 
                    wo6.Id, 
                    wo7.Id, 
                    wo8.Id
                }
            };

            var result = (RedirectToRouteResult)_target.Create(viewModel);

            Assert.AreEqual("ShowCalendar", result.RouteValues["action"]);
            Assert.AreEqual(viewModel.Crew, result.RouteValues["crew"]);
            Assert.AreEqual(viewModel.AssignFor.Value.ToShortDateString(), result.RouteValues["Date"]);
        }

        [TestMethod]
        public void TestCreateCreatesCrewAssignmentsForValidWorkOrderFactoriesInTheFuture()
        {
            var wo1 = GetFactory<StreetOpeningPermitRequiredWithAStreetOpeningPermitInTheFutureWorkOrderFactory>()
                .Create(new {
                    AssignedContractor = _currentUser.Contractor
                });
            var wo2 = GetFactory<MarkoutRequirementRoutineWithAValidFutureMarkoutWorkOrderFactory>()
                .Create(new {
                    AssignedContractor = _currentUser.Contractor
                });
            Session.Flush();
            var viewModel = new SchedulingCrewAssignment(_container) {
                AssignFor = DateTime.Today.AddDays(1),
                Crew = _crew.Id,
                WorkOrderIDs = new[] {
                    wo1.Id, wo2.Id
                }
            };

            var result = (RedirectToRouteResult)_target.Create(viewModel);

            Assert.AreEqual("ShowCalendar", result.RouteValues["action"]);
            Assert.AreEqual(viewModel.Crew, result.RouteValues["crew"]);
            Assert.AreEqual(viewModel.AssignFor.Value.ToShortDateString(), result.RouteValues["Date"]);
        }

        #endregion

        #region Destroy

        [TestMethod]
        public void TestDestroyDoesntDoSomethingStupidLikeDeleteTheWorkOrder()
        {
            var workOrder = _ass.WorkOrder;
            var result = _target.Destroy(_ass.Id);
            MvcAssert.IsStatusCode(204, result, "Sanity.");

            Session.Evict(workOrder);
            workOrder = Session.Query<MapCall.Common.Model.Entities.WorkOrder>()
                               .Single(x => x.Id == workOrder.Id);
            Assert.IsNotNull(workOrder);
        }

        [TestMethod]
        public override void TestDestroyRedirectsBackToShowPageOfAttemptedDeletedRecordIfThereAreModelStateErrors()
        {
            Assert.Inconclusive("Test me");
        }

        #endregion

        #region Manage

        [TestMethod]
        public void TestManageReturns404NotFoundIfCrewDoesntExist()
        {
            // This check is done via validation now, rather than an outright
            // check in the controller. All validation errors return this 404
            // since there's no guaranteed place to redirect it to as far as I can tell.
            _target.ModelState.AddModelError("Crew", "No crew");
            var result = _target.Manage(new CrewAssignmentManage());
            MvcAssert.IsStatusCode(404, CrewAssignmentController.NO_CREW_FOUND, result);
        }

        [TestMethod]
        public void TestManageSetsExpectedPropertiesOnViewModel()
        {
            var model = new CrewAssignmentManage();
            model.Crew = _crew.Id;
            model.Date = DateTime.Today;
            var result = (ViewResult)_target.Manage(model);
            Assert.AreSame(model, result.Model);
            Assert.AreEqual(_crew.Availability, model.Availability, "Availability must be set");
            Assert.AreEqual(_crew.Id, model.Crew, "CrewID must be set");
            Assert.AreEqual(DateTime.Today, model.Date, "Date must be set");
            Assert.AreEqual(_crew.Description, model.CrewDescription, "CrewDescription must be set");
            Assert.IsTrue(model.AssignmentsForDate.Any(x => x.Id == _ass.Id));
            Assert.AreEqual(_crew.Description, model.CrewDescription, "CrewDescription must be set");

            var expectedRemaining = model.Availability - model.AssignmentsForDate.Sum(a => a.WorkOrder.WorkDescription.TimeToComplete);
            Assert.AreEqual(expectedRemaining, model.Remaining);
        }

        #endregion

        #region UpdatePriority

        [TestMethod]
        public void TestUpdatePriorityReturns404NotFoundIfValidationFails()
        {
            _target.ModelState.AddModelError("Oops", ":(");
            var result = _target.UpdatePriority(new CrewAssignmentPriorityUpdate { Crew = 0, Date = DateTime.Today });
            MvcAssert.IsStatusCode(404, CrewAssignmentController.NO_CREW_FOUND, result);
        }

        [TestMethod]
        public void TestUpdatePrioritySetsPrioritiesOnCrewAssignmentsInTheOrderTheyrePostedIn()
        {
            var assOne = GetFactory<CrewAssignmentFactory>().Create(new {
                AssignedFor = DateTime.Now, WorkOrder = _workOrder, Crew = _crew
            });
            var assTwo = GetFactory<CrewAssignmentFactory>().Create(new {
                AssignedFor = DateTime.Now.AddHours(1), WorkOrder = _workOrder, Crew = _crew
            });
            var order = new[] { assTwo.Id, _ass.Id, assOne.Id };
            var model = new CrewAssignmentPriorityUpdate { Crew = _crew.Id, Date = DateTime.Today };
            model.CrewAssignments = order;
            var result = _target.UpdatePriority(model);
            Assert.AreEqual(1, _target.Repository.Find(assTwo.Id).Priority);
            Assert.AreEqual(2, _target.Repository.Find(_ass.Id).Priority);
            Assert.AreEqual(3, _target.Repository.Find(assOne.Id).Priority);

        }

        [TestMethod]
        public void TestUpdateRedirectsToManageIfCrewAssignmentsForDayArentSameAsModelCrewAssignmentIDs()
        {
            var assOne = GetFactory<CrewAssignmentFactory>().Create(new {
                AssignedFor = DateTime.Now, WorkOrder = _workOrder, Crew = _crew
            });
            var assTwo = GetFactory<CrewAssignmentFactory>().Create(new {
                AssignedFor = DateTime.Now.AddHours(1), WorkOrder = _workOrder, Crew = _crew
            });
            var order = new[] { assTwo.Id, assOne.Id };
            var model = new CrewAssignmentPriorityUpdate { Crew = _crew.Id, Date = DateTime.Today };
            model.CrewAssignments = order;
            var result = (RedirectToRouteResult)_target.UpdatePriority(model);
            Assert.AreEqual("Manage", result.RouteValues["action"]);
            Assert.AreEqual(model.Crew, result.RouteValues["crew"]);
            Assert.AreEqual(model.Date, result.RouteValues["date"]);
        }

        [TestMethod]
        public void TestUpdateRedirectsToManageIfCrewAssignmentsForDayArentSameAsModelCrewAssignmentIDsWithSomeThatAreDifferentDates()
        {
            var assOne = GetFactory<CrewAssignmentFactory>().Create(new {
                AssignedFor = DateTime.Now, WorkOrder = _workOrder, Crew = _crew
            });
            var assTwo = GetFactory<CrewAssignmentFactory>().Create(new {
                AssignedFor = DateTime.Now.AddDays(2), WorkOrder = _workOrder, Crew = _crew
            });
            var order = new[] { assTwo.Id, _ass.Id, assOne.Id };
            var model = new CrewAssignmentPriorityUpdate { Crew = _crew.Id, Date = DateTime.Today };
            model.CrewAssignments = order;
            var result = (RedirectToRouteResult)_target.UpdatePriority(model);
            Assert.AreEqual("Manage", result.RouteValues["action"]);
            Assert.AreEqual(model.Crew, result.RouteValues["crew"]);
            Assert.AreEqual(model.Date, result.RouteValues["date"]);
        }

        [TestMethod]
        public void TestUpdateRedirectsToCalendarPageAfterSaving()
        {
            var model = new CrewAssignmentPriorityUpdate { Crew = _crew.Id, Date = DateTime.Today };
            model.CrewAssignments = new[] { _ass.Id };
            var result = (RedirectToRouteResult)_target.UpdatePriority(model);
            Assert.AreEqual("ShowCalendar", result.RouteValues["action"]);
            Assert.AreEqual(model.Crew, result.RouteValues["crew"]);
            Assert.AreEqual(model.Date.Value.ToShortDateString(), result.RouteValues["Date"]);
        }

        #endregion

        #endregion
    }
}
