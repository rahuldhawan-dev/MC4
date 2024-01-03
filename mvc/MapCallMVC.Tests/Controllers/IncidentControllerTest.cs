using System;
using System.Collections.Generic;
using MapCall.Common.Configuration;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC;
using MMSINC.Data;
using MMSINC.Helpers;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using Moq;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class IncidentControllerTest : MapCallMvcControllerTestBase<IncidentController, Incident, IncidentRepository>
    {
        #region Fields

        private Mock<INotificationService> _notifier;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IRoleService> _roleServ;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            var user = GetFactory<AdminUserFactory>().Create(new {
                DefaultOperatingCenter = GetFactory<OperatingCenterFactory>().Create()
            });

            Session.Save(user.UserType);

            return user;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotificationService>();
            _container.Inject(_notifier.Object);
            _roleServ = new Mock<IRoleService>();
            _container.Inject(_roleServ.Object);
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject(_dateTimeProvider.Object);
            _target = Request.CreateAndInitializeController<IncidentController>();

            // Needed for Create tests
            ((FakeViewEngine)Application.ViewEngine).Views["Pdf"] = new Mock<IView>().Object;
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var incident = GetFactory<IncidentFactory>().Create();
                incident.Employee.ReportsTo = GetFactory<EmployeeFactory>().Create();
                return incident;
            };
        }

        #endregion

        #region Private Methods

        private Role CreateRole(User user, RoleModules module, RoleActions action, OperatingCenter opc)
        {
            var mod = GetFactory<ModuleFactory>().Create(new { Id = (int)module });
            var act = GetFactory<ActionFactory>().Create(new { Id = (int)action });
            return GetFactory<RoleFactory>().Create(new { User = _currentUser, Module = mod, Action = act, OperatingCenter = opc });
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.OperationsIncidents;
            Authorization.Assert(a => {
                a.RequiresRole("~/Incident/New/", module, RoleActions.Add);
                a.RequiresRole("~/Incident/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Incident/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Incident/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Incident/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Incident/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Incident/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Incident/Destroy/", module, RoleActions.Delete);
                a.RequiresRole("~/Incident/ChartIncidentClassifications/", module, RoleActions.Read);
                a.RequiresRole("~/Incident/ChartIncidentTypes/", module, RoleActions.Read);
                a.RequiresRole("~/Incident/ChartAtRiskBehaviors/", module, RoleActions.Read);
                a.RequiresRole("~/Incident/AddIncidentEmployeeAvailability/", module, RoleActions.Edit);
                a.RequiresRole("~/Incident/RemoveIncidentEmployeeAvailability/", module, RoleActions.Edit);
                a.RequiresRole("~/Incident/RemoveIncidentInvestigation/", module, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/Incident/SendNotification/"); 
                a.RequiresLoggedInUserOnly("~/Incident/ByOperatingCenter");
                a.RequiresLoggedInUserOnly("~/Incident/ByEmployeeId");
            });
        }

        #region Create

        [TestMethod]
        public void TestCreateSendsNotificationEmail()
        {
            var expectedBytes = new byte[] { 1, 2, 3 };
            var pdfRenderer = new Mock<IHtmlToPdfConverter>();
            _container.Inject(pdfRenderer.Object);

            pdfRenderer.Setup(x => x.RenderHtmlToPdfBytes(It.IsAny<string>())).Returns(expectedBytes);
            var empType = GetFactory<EmployeeTypeFactory>().Create(new { Id = 1, Description = "Employee" });
            var incident = GetFactory<IncidentFactory>().Create(new { EmployeeType = empType });
            incident.Employee.Status = GetFactory<ActiveEmployeeStatusFactory>().Create();
            incident.Employee.ReportsTo = GetFactory<EmployeeFactory>().Create();

            var model = _viewModelFactory.Build<CreateIncident, Incident>(incident);
            model.Id = 0;

            // If this fails, the rest of the test will fail. Sanity check.
            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.OperationsIncidents, resultArgs.Module);
            Assert.AreEqual(IncidentController.HS_INCIDENT_NOTIFICATION_PURPOSE, resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.IsNull(resultArgs.Subject);
            Assert.IsTrue(expectedBytes.ByteArrayEquals(resultArgs.Attachments.Single().BinaryData));
        }

        #endregion

        #region New

        [TestMethod]
        public void TestNewSetsOperatingCenterDropDownDataBasedOnRoleModuleAndAddActionForUser()
        {
            var expected = GetFactory<OperatingCenterFactory>().Create();
            var role = CreateRole(_currentUser, RoleModules.OperationsIncidents, RoleActions.Add, expected);
            var badRole = CreateRole(_currentUser, RoleModules.OperationsIncidents, RoleActions.Add,
                GetFactory<OperatingCenterFactory>().Create());

            _target.New();
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        [TestMethod]
        public void TestNewSetsMedicalProviderStateDropDownData()
        {
            var expected = GetFactory<StateFactory>().Create();
            _target.New();
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["MedicalProviderState"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.Abbreviation, vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        [TestMethod]
        public void TestNewSetsAccidentStateDropDownData()
        {
            var expected = GetFactory<StateFactory>().Create();
            _target.New();
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["AccidentState"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.Abbreviation, vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersInLookupData()
        {
            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _target.New();

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion

        #region Edit

        [TestMethod]
        public void TestEditSetsOperatingCenterDropDownDataBasedOnRoleModuleAndAddActionForUser()
        {
            var expected = GetFactory<OperatingCenterFactory>().Create();
            var role = CreateRole(_currentUser, RoleModules.OperationsIncidents, RoleActions.Edit, expected);
            var badRole = CreateRole(_currentUser, RoleModules.OperationsIncidents, RoleActions.Edit,
                GetFactory<OperatingCenterFactory>().Create());
            var entity = GetFactory<IncidentFactory>().Create(); 

            _target.Edit(entity.Id);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        [TestMethod]
        public void TestEditSetsMedicalProviderStateDropDownData()
        {
            var entity = GetFactory<IncidentFactory>().Create();
            var expected = entity.AccidentTown.State; // Creating an incident automatically creates a state.
            _target.Edit(entity.Id);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["MedicalProviderState"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.Abbreviation, vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        [TestMethod]
        public void TestEditSetsAccidentStateDropDownData()
        {
            var entity = GetFactory<IncidentFactory>().Create();
            var expected = entity.AccidentTown.State; // Creating an incident automatically creates a state.
            _target.Edit(entity.Id);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["AccidentState"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.Abbreviation, vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            var entity = GetFactory<IncidentFactory>().Create();
            InitializeControllerAndRequest("~/Incident/Show/" + entity.Id + ".pdf");
            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(PdfResult));
        }

        [TestMethod]
        public void TestShowRespondsToMap()
        {
            var good = GetFactory<IncidentFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });
            var bad = GetFactory<IncidentFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });
            InitializeControllerAndRequest("~/Incident/Show/" + good.Id + ".map");

            var result = (MapResult)_target.Show(good.Id);

            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
            Assert.IsTrue(result.ModelStateIsValid);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetFactory<IncidentFactory>().Create();
            var entity1 = GetFactory<IncidentFactory>().Create();
            var search = new SearchIncident();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
            }
        }

        [TestMethod]
        public void TestIndexXLSExportsExcelForDrugTesting()
        {
            var entity0 = GetFactory<IncidentFactory>().Create();
            var ny1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { OperatingCenterCode = "NY1", Id = 20 });
            var entity1 = GetFactory<IncidentFactory>().Create(new { OperatingCenter = ny1 });
            var search = new SearchIncident();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            _roleServ.Setup(x => x.CanAccessRole(RoleModules.OperationsIncidentsDrugTesting, RoleActions.Read, entity0.OperatingCenter)).Returns(true);
            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.DrugAndAlcoholTestingDecision, "DrugAndAlcoholTestingDecision");
                helper.AreEqual(entity0.DrugAndAlcoholTestingResult, "DrugAndAlcoholTestingResult");
                helper.AreEqual(entity0.DrugAndAlcoholTestingNotes, "DrugAndAlcoholTestingNotes");
                helper.IsNull("DrugAndAlcoholTestingDecision", 1);
                helper.IsNull("DrugAndAlcoholTestingResult", 1);
                helper.IsNull("DrugAndAlcoholTestingNotes", 1);
            }
        }

        [TestMethod]
        public void TestIndexJSONExportsJSON()
        {
            var now = DateTime.Now;
            var entity0 = GetFactory<IncidentFactory>().Create();
            var entity1 = GetFactory<IncidentFactory>().Create();
            var search = new SearchIncident {
                IncidentDate = new DateRange {
                    Start = now.AddDays(-1),
                    End = now,
                    Operator = RangeOperator.Between
                }
            };
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.JSON;

            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result.Data);

            helper.AreEqual(entity0.Id, "Id");
            helper.AreEqual(entity1.Id, "Id", 1);
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeIsNotSent()
        {
            var entity0 = GetFactory<IncidentFactory>().Create();
            var entity1 = GetFactory<IncidentFactory>().Create();
            var search = new SearchIncident();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.JSON;

            MyAssert.Throws<InvalidOperationException>(() =>
                _target.Index(search));
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeIsLongerThanOneMonth()
        {
            var now = DateTime.Now;
            var entity0 = GetFactory<IncidentFactory>().Create();
            var entity1 = GetFactory<IncidentFactory>().Create();
            var search = new SearchIncident {
                IncidentDate = new DateRange {
                    Start = now.AddDays(-31),
                    End = now,
                    Operator = RangeOperator.Between
                }
            };
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.JSON;

            MyAssert.Throws<InvalidOperationException>(() =>
                _target.Index(search));
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeOperatorIsNotBetween()
        {
            foreach (var op in new[] {
                RangeOperator.Equal, RangeOperator.GreaterThan, RangeOperator.GreaterThanOrEqualTo,
                RangeOperator.LessThan, RangeOperator.LessThanOrEqualTo
            })
            {
                var now = DateTime.Now;
                var entity0 = GetFactory<IncidentFactory>().Create();
                var entity1 = GetFactory<IncidentFactory>().Create();
                var search = new SearchIncident {
                    IncidentDate = new DateRange {
                        Start = now.AddDays(-1),
                        End = now,
                        Operator = op
                    }
                };
                _target.ControllerContext = new ControllerContext();
                _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                    ResponseFormatter.KnownExtensions.JSON;

                MyAssert.Throws<InvalidOperationException>(() =>
                    _target.Index(search));
            }
        }

        [TestMethod]
        public void TestIndexRespondsToMapWithExpectedModels()
        {
            InitializeControllerAndRequest("~/Incident/Index.map");
            var good = GetFactory<IncidentFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });
            var bad = GetFactory<IncidentFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });
            var model = new SearchIncident {
                OperatingCenter = new[] {good.OperatingCenter.Id}
            };
            var result = (MapResult)_target.Index(model);

            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
            Assert.IsTrue(result.ModelStateIsValid);
        }

        [TestMethod]
        public void TestIndexRespondsToMapWithoutModelsIfModelStateIsNotValid()
        {
            InitializeControllerAndRequest("~/Incident/Index.map");
            var good = GetFactory<IncidentFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });

            var model = new SearchIncident();
            var validResult = (MapResult)_target.Index(model);

            Assert.IsTrue(validResult.ModelStateIsValid);
            Assert.AreEqual(1, validResult.CoordinateSets.Single().Coordinates.Count());
            Assert.IsTrue(validResult.CoordinateSets.Single().Coordinates.Contains(good));

            _target.ModelState.AddModelError("error", "error");
            var badResult = (MapResult)_target.Index(model);

            Assert.IsFalse(badResult.CoordinateSets.Any());
        }

        #endregion

        #region ChartIncidentClassifications

        [TestMethod]
        public void TestChartIncidentClassificationsReturnsChartResultWithDataGroupedByIncidentClassificationDescriptionAndNumberOfOccurrances()
        {
            var classOne = GetFactory<IncidentClassificationFactory>().Create(new { Description = "Hurrah!" });
            var classTwo = GetFactory<IncidentClassificationFactory>().Create(new { Description = "Yippee!" });
            var incidentOne = GetFactory<IncidentFactory>().Create(new { IncidentClassification = classOne });
            var incidentTwo = GetFactory<IncidentFactory>().Create(new { IncidentClassification = classTwo });
            var incidentThree = GetFactory<IncidentFactory>().Create(new { IncidentClassification = classTwo });

            var result = (ChartResult)_target.ChartIncidentClassifications(new SearchIncident());
            var chart = (ChartBuilder<string, int>)result.Chart;
            Assert.AreEqual(ChartLegendPosition.None, chart.LegendPosition);
            Assert.AreEqual(ChartType.SingleSeriesBar, chart.Type);
            Assert.AreEqual("Incident Classification Breakdown", chart.Title);
            Assert.AreEqual(ChartSortType.Alphabetical, chart.SortType);
            Assert.AreEqual(0, chart.YMinValue);
            Assert.AreEqual(1, chart.Series.Count, "SingleSeriesBar can only have one series.");

            var series = chart.Series.Single();
            Assert.AreEqual(1, series["Hurrah!"]);
            Assert.AreEqual(2, series["Yippee!"]);
        }

        #endregion

        #region ChartIncidentTypes

        [TestMethod]
        public void TestChartIncidentTypesReturnsChartResultWithDataGroupedByIncidentTypeDescriptionAndNumberOfOccurrances()
        {
            var classOne = GetFactory<IncidentTypeFactory>().Create(new { Description = "Hurrah!" });
            var classTwo = GetFactory<IncidentTypeFactory>().Create(new { Description = "Yippee!" });
            var incidentOne = GetFactory<IncidentFactory>().Create(new { IncidentType = classOne });
            var incidentTwo = GetFactory<IncidentFactory>().Create(new { IncidentType = classTwo });
            var incidentThree = GetFactory<IncidentFactory>().Create(new { IncidentType = classTwo });

            var result = (ChartResult)_target.ChartIncidentTypes(new SearchIncident());
            var chart = (ChartBuilder<string, int>)result.Chart;
            Assert.AreEqual(ChartLegendPosition.None, chart.LegendPosition);
            Assert.AreEqual(ChartType.SingleSeriesBar, chart.Type);
            Assert.AreEqual("Incident Type Breakdown", chart.Title);
            Assert.AreEqual(ChartSortType.Alphabetical, chart.SortType);
            Assert.AreEqual(0, chart.YMinValue);
            Assert.AreEqual(1, chart.Series.Count, "SingleSeriesBar can only have one series.");

            var series = chart.Series.Single();
            Assert.AreEqual(1, series["Hurrah!"]);
            Assert.AreEqual(2, series["Yippee!"]);
        }

        #endregion

        #region ChartAtRiskBehaviors

        [TestMethod]
        public void TestChartAtRiskBehaviorsReturnsChartResultWithDataGroupedByAtRiskBehaviorSectionDescriptionAndNumberOfOccurrances()
        {
            var classOne = GetEntityFactory<AtRiskBehaviorSection>().Create(new { Description = "Hurrah!" });
            var classTwo = GetEntityFactory<AtRiskBehaviorSection>().Create(new { Description = "Yippee!" });
            var incidentOne = GetFactory<IncidentFactory>().Create(new { AtRiskBehaviorSection = classOne });
            var incidentTwo = GetFactory<IncidentFactory>().Create(new { AtRiskBehaviorSection = classTwo });
            var incidentThree = GetFactory<IncidentFactory>().Create(new { AtRiskBehaviorSection = classTwo });

            var result = (ChartResult)_target.ChartAtRiskBehaviors(new SearchIncident());
            var chart = (ChartBuilder<string, int>)result.Chart;
            Assert.AreEqual(ChartLegendPosition.None, chart.LegendPosition);
            Assert.AreEqual(ChartType.SingleSeriesBar, chart.Type);
            Assert.AreEqual("At Risk Behavior Section Breakdown", chart.Title);
            Assert.AreEqual(ChartSortType.Alphabetical, chart.SortType);
            Assert.AreEqual(0, chart.YMinValue);
            Assert.AreEqual(1, chart.Series.Count, "SingleSeriesBar can only have one series.");

            var series = chart.Series.Single();
            Assert.AreEqual(1, series["Hurrah!"]);
            Assert.AreEqual(2, series["Yippee!"]);
        }

        #endregion

        #region SendNotification

        [TestMethod]
        public void TestSendNotificationSendsNotificationEmailAndReturnsUserToShowView()
        {
            var expectedBytes = new byte[] { 1, 2, 3 };
            var pdfRenderer = new Mock<IHtmlToPdfConverter>();
            _container.Inject(pdfRenderer.Object);

            pdfRenderer.Setup(x => x.RenderHtmlToPdfBytes(It.IsAny<string>())).Returns(expectedBytes);

            var entity = GetFactory<IncidentFactory>().Create();
            entity.Employee.Status = GetFactory<ActiveEmployeeStatusFactory>().Create();
            entity.Employee.ReportsTo = GetFactory<EmployeeFactory>().Create();

            // If this fails, the rest of the test will fail. Sanity check.
            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.SendNotification(entity.Id, false);
            MvcAssert.RedirectsToRoute(result, new {action = "Show", controller = "Incident", id = entity.Id});
            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.OperationsIncidents, resultArgs.Module);
            Assert.AreEqual(IncidentController.HS_INCIDENT_NOTIFICATION_PURPOSE, resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.IsNull(resultArgs.Subject);
            Assert.IsTrue(expectedBytes.ByteArrayEquals(resultArgs.Attachments.Single().BinaryData));
        }

        #endregion

        #region EmployeeAvailability

        [TestMethod]
        public void TestEmployeeAvailabliltyStuff()
        {
            Assert.Inconclusive("TODO because no one ever wrote these");
        }

        #endregion

        #region RemoveIncidentInvestigation

        [TestMethod]
        public void TestRemoveIncidentInvestigationRedirectsToIncidentShowOnSuccess()
        {
            var investigation = GetEntityFactory<IncidentInvestigation>().Create();
            var model = _viewModelFactory.Build<RemoveIncidentInvestigation>();
            model.Id = investigation.Incident.Id;
            model.IncidentInvestigationId = investigation.Id;

            var result = _target.RemoveIncidentInvestigation(model);
            Assert.IsTrue(_target.ModelState.IsValid, "Sanity test to ensure we're getting a success result here");

            MvcAssert.RedirectsToRoute(result, new { area = string.Empty, controller = "Incident", action = "Show", id = investigation.Incident.Id });
        }

        [TestMethod]
        public void TestRemoveIncidentInvestigationDeletesTheIncidentInvestigationRecord()
        {
            var investigation = GetEntityFactory<IncidentInvestigation>().Create();
            var incident = investigation.Incident;
            Assert.IsTrue(incident.IncidentInvestigations.Contains(investigation), "Sanity, this needs to exist in the collection");
            Session.Evict(incident); // Without the evict, the delete just never gets called for some reason. This works fine in practice.

            var model = _viewModelFactory.Build<RemoveIncidentInvestigation>();
            model.Id = incident.Id;
            model.IncidentInvestigationId = investigation.Id;

            _target.RemoveIncidentInvestigation(model);

            Assert.IsNull(Session.QueryOver<IncidentInvestigation>().Where(x => x.Id == investigation.Id).SingleOrDefault(), "Investigation should have been deleted, too");
            Assert.IsNotNull(Session.QueryOver<Incident>().Where(x => x.Id == incident.Id).SingleOrDefault(), "Incident should not have been deleted.");
        }

        [TestMethod]
        public void TestRemoveIncidentInvestigationRedirectsToIncidentShowOnError()
        {
            var investigation = GetEntityFactory<IncidentInvestigation>().Create();
            var model = _viewModelFactory.Build<RemoveIncidentInvestigation>();
            model.Id = investigation.Incident.Id;
            model.IncidentInvestigationId = investigation.Id;
            _target.ModelState.AddModelError("Whoops", "oops");

            var result = _target.RemoveIncidentInvestigation(model);

            MvcAssert.RedirectsToRoute(result, new { area = string.Empty, controller = "Incident", action = "Show", id = investigation.Incident.Id });
        }

        #endregion

        #region ByOperatingCenterId

        [TestMethod]
        public void TestByOperatingCenterIdReturnsCascadingResultForMatchingOperatingCenters()
        {
            _currentUser.IsAdmin = true;
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var goodIncident = GetFactory<IncidentFactory>().Create(new { OperatingCenter = opc });
            var badIncident = GetFactory<IncidentFactory>().Create();

            var result = (CascadingActionResult)_target.ByOperatingCenter(opc.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(goodIncident.Id.ToString(), actual[1].Value);
        }

        #endregion

        #region ByEmployeeId

        [TestMethod]
        public void TestByEmployeeIdReturnsCascadingResultForMatchingEmployeeIds()
        {
            _currentUser.IsAdmin = true;
            var emp = GetFactory<ActiveEmployeeFactory>().Create();
            var goodIncident = GetFactory<IncidentFactory>().Create(new { Employee = emp });
            var badIncident = GetFactory<IncidentFactory>().Create();

            var result = (CascadingActionResult)_target.ByEmployeeId(emp.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(goodIncident.Id.ToString(), actual[1].Value);
        }

        #endregion

        #endregion
    }
}
