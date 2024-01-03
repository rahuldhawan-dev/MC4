using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class CrewAssignmentControllerTest : MapCallMvcControllerTestBase<CrewAssignmentController, CrewAssignment, CrewAssignmentRepository>
    {
        #region Private Members

        private CrewAssignment _ass;
        private Crew _crew;
        private WorkOrder _workOrder;
        private Mock<HttpContextBase> _httpContext;
        private Mock<HttpRequestBase> _httpRequest;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _workOrder = GetEntityFactory<WorkOrder>().Create();
            _workOrder.JobSiteCheckLists.Add(GetEntityFactory<JobSiteCheckList>().Create(new {SafetyBriefDateTime = DateTime.Now}));
            _crew = GetEntityFactory<Crew>().Create(new {OperatingCenter = _currentUser.DefaultOperatingCenter});
            _ass = GetEntityFactory<CrewAssignment>().Create(new {WorkOrder = _workOrder, Crew = _crew});
        }

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.ExpectedEditViewName = "_Edit";
            options.EditReturnsPartialView = true;
            options.UpdateRedirectsToReferrerOnError = true;
            options.UpdateRedirectsToReferrerOnSuccess = true;
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
            var wo = GetFactory<TFactory>().Create();

            var model = new SchedulingCrewAssignment(_container)
            {
                AssignFor = assignFor ?? DateTime.Today,
                Crew = _crew.Id,
                WorkOrderIDs = { wo.Id }
            };
            Session.Flush();
            _target.RunModelValidation(model);
            var result = _target.Create(model);
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual("WorkOrderScheduling", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Search", redirectResult.RouteValues["action"]);
            Assert.AreEqual(errorMessageFn(model), _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        #endregion

        #region Tests

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var role = CrewAssignmentController.ROLE;

                a.RequiresRole("~/CrewAssignment/End", role);
                a.RequiresRole("~/CrewAssignment/Index", role);
                a.RequiresRole("~/CrewAssignment/IndexTabs", role);
                a.RequiresRole("~/CrewAssignment/ShowCalendar", role);
                a.RequiresRole("~/CrewAssignment/Start", role);
                a.RequiresRole("~/CrewAssignment/Create", role, RoleActions.Add);
                a.RequiresRole("~/CrewAssignment/Manage", role, RoleActions.Edit);
                a.RequiresRole("~/CrewAssignment/Edit", role, RoleActions.Edit);
                a.RequiresRole("~/CrewAssignment/Update", role, RoleActions.Edit);
                a.RequiresRole("~/CrewAssignment/UpdatePriority", role, RoleActions.Edit);
                a.RequiresRole("~/CrewAssignment/Destroy", role, RoleActions.Delete);
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
            var result = (ViewResult)_target.ShowCalendar(new CrewAssignmentCalendarSearch { Crew = _crew.Id, Date = null });
            var model = (CrewAssignmentCalendarSearch)result.Model;
            Assert.AreEqual(DateTime.Today, model.Date);
        }

        [TestMethod]
        public void TestShowCalendarAddsCrewsToViewData()
        {
            _target.ShowCalendar(new CrewAssignmentCalendarSearch { Crew = _crew.Id, Date = DateTime.Today });

            var result = (IEnumerable<SelectListItem>)_target.ViewData[CrewAssignmentController.ViewDataKeys.CREW];
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(x => x.Value == _crew.Id.ToString()));
        }

        #endregion

        #region End

        [TestMethod]
        public void TestEndRedirectsWithErrorsIfModelStateIsInvalid()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create(new {
                MarkoutRequirement = GetFactory<RoutineMarkoutRequirementFactory>().Create()
            });
            var ass = GetFactory<CrewAssignmentFactory>().Create(new {
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

            MvcAssert.RedirectsToRoute(redirectResult, "WorkOrderFinalization", "Edit", new { area = "FieldOperations", id = workOrder.Id });
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
            _ass.DateEnded = _now;
            Session.Save(_ass);
            Session.Clear();

            var model = _container.GetInstance<CrewAssignmentEnd>();
            model.Id = _ass.Id;
            model.EmployeesOnJob = 1;
            _target.RunModelValidation(model);
            var result = _target.End(model);

            var redirectResult = (RedirectToRouteResult)result;

            MvcAssert.RedirectsToRoute(redirectResult, "WorkOrderFinalization", "Edit", new { area = "FieldOperations", id = _ass.WorkOrder.Id });
            Assert.AreEqual(CrewAssignmentEnd.ModelErrors.ALREADY_ENDED,
                _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestEndSetsDateEndedToNow()
        {
            _target.End(_viewModelFactory.Build<CrewAssignmentEnd, CrewAssignment>(_ass));

            var result = Repository.Find(_ass.Id);

            Assert.IsNotNull(result.DateEnded);
            MyAssert.AreClose(_now, result.DateEnded.Value);
        }

        [TestMethod]
        public void TestEndSetsEmployeesOnJobToModelValue()
        {
            var expected = 542f;
            _target.End(
                _viewModelFactory
                   .BuildWithOverrides<CrewAssignmentEnd, CrewAssignment>(_ass,
                        new { EmployeesOnJob = expected }));
            var result = Repository.Find(_ass.Id);
            Assert.AreEqual(expected, result.EmployeesOnJob);
        }

        [TestMethod]
        public void TestEndReturnsRedirectToWorkOrderFinalizationForCrewAssignmentsWorkOrderID()
        {
            _ass.DateStarted = _now;
            Session.Save(_ass);
            Session.Clear();

            var model = _container.GetInstance<CrewAssignmentEnd>();
            model.Id = _ass.Id;
            var result = _target.End(model);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;

            MvcAssert.RedirectsToRoute(redirectResult, "WorkOrderFinalization", "Edit", new { area = "FieldOperations", id = _ass.WorkOrder.Id });
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
            _ass.DateStarted = _now;
            Session.Save(_ass);

            var model = _viewModelFactory.Build<CrewAssignmentStart, CrewAssignment>(_ass);
            _target.RunModelValidation(model);
            var result = _target.Start(model);

            var redirectResult = (RedirectToRouteResult)result;

            MvcAssert.RedirectsToRoute(redirectResult, "WorkOrderFinalization", "Edit", new { area = "FieldOperations", id = _ass.WorkOrder.Id });
            Assert.AreEqual(CrewAssignmentStart.ModelErrors.ALREADY_STARTED,
                _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestStartReturnsRedirectToWorkOrderIfMarkoutIsRequiredAndInvalid()
        {
            // TODO: This should be a view model test, not a controller test.
            var workOrder = GetFactory<WorkOrderFactory>().Create(new {
                MarkoutRequirement = GetFactory<RoutineMarkoutRequirementFactory>().Create()
            });
            workOrder.JobSiteCheckLists.Add(GetEntityFactory<JobSiteCheckList>().Create(new {SafetyBriefDateTime = DateTime.Now}));
            var ass = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = workOrder,
                Crew = _crew
            });
            var expectedMarkout = GetFactory<MarkoutFactory>().Create(new {
                DateOfRequest = _now.AddDays(-10),
                ReadyDate = _now.AddDays(-2),
                ExpirationDate = _now.AddDays(-1),
                WorkOrder = workOrder,
            });
            Session.Flush();
            Session.Clear();

            var model = _viewModelFactory.Build<CrewAssignmentStart, CrewAssignment>(ass);
            _target.RunModelValidation(model);
            var result = _target.Start(model);

            var redirectResult = (RedirectToRouteResult)result;

            MvcAssert.RedirectsToRoute(redirectResult, "WorkOrderFinalization", "Edit", new { area = "FieldOperations", id = workOrder.Id });
            Assert.AreEqual(CrewAssignmentStart.ModelErrors.NO_VALID_MARKOUT,
                _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestStartReturnsRedirectToWorkOrderIfStreetOpeningPermitIsRequiredAndInvalid()
        {
            // TODO: This should be a view model test, not a controller test.
            var workOrder = GetFactory<WorkOrderFactory>().Create(new {
                StreetOpeningPermitRequired = true
            });
            workOrder.JobSiteCheckLists.Add(GetEntityFactory<JobSiteCheckList>().Create(new {SafetyBriefDateTime = DateTime.Now}));
            var ass = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = workOrder,
                Crew = _crew
            });
            var permit = GetFactory<StreetOpeningPermitFactory>().Create(new {
                WorkOrder = workOrder,
                ExpirationDate = _now.AddDays(-1)
            });
            Session.Flush();
            Session.Clear();

            var model = _viewModelFactory.Build<CrewAssignmentStart, CrewAssignment>(ass);
            _target.RunModelValidation(model);
            var result = _target.Start(model);

            var redirectResult = (RedirectToRouteResult)result;

            MvcAssert.RedirectsToRoute(redirectResult, "WorkOrderFinalization", "Edit", new { area = "FieldOperations", id = workOrder.Id });
            Assert.AreEqual(CrewAssignmentStart.ModelErrors.NO_VALID_PERMIT,
                _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestStartExtendsMarkoutExpirationIfCurrentMarkoutIsNotNull()
        {
            var wo = GetFactory<SchedulingWorkOrderFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                    MarkoutsEditable = false
                })
            });
            wo.JobSiteCheckLists.Add(GetEntityFactory<JobSiteCheckList>().Create(new {SafetyBriefDateTime = DateTime.Now}));
            var ass = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = wo,
                Crew = _crew
            });
            Session.Flush();
            Session.Clear();

            _target.Start(_viewModelFactory.Build<CrewAssignmentStart, CrewAssignment>(ass));

            wo = Session.Load<WorkOrder>(wo.Id);

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
        public void TestStartSetsDateStartedOnCrewAssignmentToNow()
        {
            _target.Start(_viewModelFactory.Build<CrewAssignmentStart, CrewAssignment>(_ass));

            var result = Repository.Find(_ass.Id);
            Assert.IsNotNull(result.DateStarted);
            MyAssert.AreClose(_now, result.DateStarted.Value);
        }

        [TestMethod]
        public void TestStartRedirectToJobSiteCheckList()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            var ass = GetFactory<CrewAssignmentFactory>().Create(new
            {
                WorkOrder = workOrder,
                Crew = _crew
            });
            Session.Flush();
            Session.Clear();

            var result = _target.Start(_viewModelFactory.Build<CrewAssignmentStart, CrewAssignment>(ass));

            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual(redirectResult.RouteValues["area"], "HealthAndSafety");
            Assert.AreEqual(redirectResult.RouteValues["controller"], "JobSiteCheckList");
            Assert.AreEqual(redirectResult.RouteValues["action"], "New");
            Assert.AreEqual(redirectResult.RouteValues["workOrderId"], workOrder.Id);
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
            Assert.AreEqual("Search", result.RouteValues["action"]);
            Assert.AreEqual("WorkOrderScheduling", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestCreateCreatesCrewAssignmentFromViewModelForEachWorkOrderIdAndRedirectsToCalendar()
        {
            var wo1 = GetFactory<WorkOrderFactory>().Create();
            var wo2 = GetFactory<WorkOrderFactory>().Create();

            var viewModel = new SchedulingCrewAssignment(_container)
            {
                AssignFor = DateTime.Today,
                Crew = _crew.Id,
                WorkOrderIDs = new[] { wo1.Id, wo2.Id }
            };

            var result = _target.Create(viewModel) as RedirectToRouteResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("ShowCalendar", result.RouteValues["action"]);
            Assert.AreEqual(viewModel.Crew, result.RouteValues["crew"]);
            Assert.AreEqual(viewModel.AssignFor.Value.ToShortDateString(), result.RouteValues["Date"]);
        }

        [TestMethod]
        public void TestCreateDisplaysModelStateErrorsIfCrewDoesNotExist()
        {
            // MONDAY PROBLEMS: These other failing tests are failing because they weren't
            // tested correctly in the first place. The validation errors that previously
            // worked *only* worked because the tests were only calling the IValidatableObject.Validate
            // method. This ignored the property level validators. Things like "No such crew."
            // shouldn't even work in practice because that validator would never be called in prod.
            // I mean it was being called... annoyingly. Some of these tests might need specific
            // custom errors on the validation attribute in question. Also, this test is bad because
            // it's assuming the first result is going to be the expected error. This hurts my brain.
            // Also this needs to be fixed in the Contractors version too most likely.

            var model = new SchedulingCrewAssignment(_container) { Crew = 0, WorkOrderIDs = new[] { 1234 } };
            _target.RunModelValidation(model);
            var result =
                _target.Create(model);

            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual("WorkOrderScheduling", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Search", redirectResult.RouteValues["action"]);
            Assert.AreEqual(SchedulingCrewAssignment.ModelErrors.NO_CREW_FOUND,
                _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestCreateReturns404IfUserDoesNotHaveAccessToWorkOrder()
        {
            var workOrderId = 23532;
            var viewModel = new SchedulingCrewAssignment(_container)
            {
                AssignFor = DateTime.Today,
                Crew = _crew.Id,
                WorkOrderIDs = new[] { workOrderId }
            };

            _target.RunModelValidation(viewModel);
            var result = _target.Create(viewModel);
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual("WorkOrderScheduling", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Search", redirectResult.RouteValues["action"]);
            Assert.AreEqual(
                String.Format(CrewAssignment.ModelErrors.NO_SUCH_WORK_ORDER,
                    workOrderId),
                _target.ModelState.First().Value.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestCreateReturns404IfUserTriesToAssignForADateThatIsInvalidForTheMarkout()
        {
            VerifyRedirectAndError<MarkoutRequirementRoutineWithAValidFutureMarkoutWorkOrderFactory>(
                ca => String
                   .Format(CrewAssignment.ModelErrors.INVALID_MARKOUT,
                        ca.WorkOrderIDs[0]));
            
            // need to clear out the modelstate
            _target.ModelState.Clear();

            VerifyRedirectAndError<MarkoutRequirementRoutineWithAValidFutureMarkoutWorkOrderFactory>(null, _now.AddDays(40),
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
            var wo1 = GetFactory<WorkOrderFactory>().Create();
            var wo2 = GetFactory<StreetOpeningPermitRequiredEmergencyPriorityWithoutAnStreetOpeningPermitWorkOrderFactory>().Create();
            var wo3 = GetFactory<StreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermitWorkOrderFactory>().Create();
            var wo4 = GetFactory<MarkoutRequirementEmergencyWorkOrderFactory>().Create();
            var wo5 = GetFactory<MarkoutRequirementEmergencyWithValidMarkoutWorkOrderFactory>().Create();
            var wo6 = GetFactory<MarkoutRequirementEmergencyWithExpiredMarkoutWorkOrderFactory>().Create();
            var wo7 = GetFactory<MarkoutRequirementEmergencyPermitRequiredWorkOrderFactory>().Create(new { Priority = typeof(EmergencyWorkOrderPriorityFactory) });
            var wo8 = GetFactory<MarkoutRequirementRoutineWithAValidMarkoutWorkOrderFactory>().Create();
            Session.Flush();
            var viewModel = new SchedulingCrewAssignment(_container)
            {
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
               .Create();
            var wo2 = GetFactory<MarkoutRequirementRoutineWithAValidFutureMarkoutWorkOrderFactory>()
               .Create();
            Session.Flush();
            var viewModel = new SchedulingCrewAssignment(_container)
            {
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
            _target.Destroy(_ass.Id);

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

        [TestMethod]
        public void TestDestroyReturnsNoContentBecauseReasons()
        {
            var workOrder = _ass.WorkOrder;
            var result = _target.Destroy(_ass.Id);

            MvcAssert.IsStatusCode(204, result);
        }

        [TestMethod]
        public override void TestDestroyRedirectsToSearchPageWhenRecordIsSuccessfullyDestroyed()
        {
            Assert.Inconclusive("This controller does not do that");
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
            var assOne = GetFactory<CrewAssignmentFactory>().Create(new
            {
                AssignedFor = _now,
                WorkOrder = _workOrder,
                Crew = _crew
            });
            var assTwo = GetFactory<CrewAssignmentFactory>().Create(new
            {
                AssignedFor = _now.AddHours(1),
                WorkOrder = _workOrder,
                Crew = _crew
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
            var assOne = GetFactory<CrewAssignmentFactory>().Create(new
            {
                AssignedFor = _now,
                WorkOrder = _workOrder,
                Crew = _crew
            });
            var assTwo = GetFactory<CrewAssignmentFactory>().Create(new
            {
                AssignedFor = _now.AddHours(1),
                WorkOrder = _workOrder,
                Crew = _crew
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
            var assOne = GetFactory<CrewAssignmentFactory>().Create(new
            {
                AssignedFor = _now,
                WorkOrder = _workOrder,
                Crew = _crew
            });
            var assTwo = GetFactory<CrewAssignmentFactory>().Create(new
            {
                AssignedFor = _now.AddDays(2),
                WorkOrder = _workOrder,
                Crew = _crew
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
