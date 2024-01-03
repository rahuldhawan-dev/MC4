using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class EmployeeAssignmentControllerTest : MapCallMvcControllerTestBase<EmployeeAssignmentController, EmployeeAssignment>
    {
        #region Fields

        private DateTime _now;
        private DataType _pwoDataType;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>()
             .Add(new TestDateTimeProvider(_now = DateTime.Now));
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _pwoDataType = GetEntityFactory<DataType>().Create(new {
                TableName = nameof(ProductionWorkOrder) + "s"
            });
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { area = "Production", controller = "Scheduling", action = "Search" };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionWorkManagement;
            Authorization.Assert(auth => {
                auth.RequiresRole("~/Production/EmployeeAssignment/Show", role);
                auth.RequiresRole("~/Production/EmployeeAssignment/Search", role);
                auth.RequiresRole("~/Production/EmployeeAssignment/Index", role);

                auth.RequiresRole("~/Production/EmployeeAssignment/Edit", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/EmployeeAssignment/Update", role, RoleActions.Edit);

                auth.RequiresRole("~/Production/EmployeeAssignment/Start", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/EmployeeAssignment/End", role, RoleActions.Edit);

                auth.RequiresRole("~/Production/EmployeeAssignment/Create", role, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<EmployeeAssignment>().Create();
            var expected = DateTime.Now;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEmployeeAssignment, EmployeeAssignment>(eq, new {
                DateStarted= expected
            }));

            Assert.AreEqual(expected, Session.Get<EmployeeAssignment>(eq.Id).DateStarted);
        }

        #endregion

        #region Create

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // Test override needed because this creates multiple records.
            var order = GetEntityFactory<ProductionWorkOrder>().Create();
            var employee = GetEntityFactory<Employee>().Create();
            var now = DateTime.Now;
            var entity = new CreateEmployeeAssignment { ProductionWorkOrderIds = new[] { order.Id }, AssignedTo = new [] { employee.Id }, AssignedFor = now };
            ActionResult result = null;

            MyAssert.CausesIncrease(
                () => result = _target.Create(entity),
                () => Repository.GetAll().Count());
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            var entity = new CreateEmployeeAssignment();
            _target.ModelState.AddModelError("error", "here");
            ActionResult result = null;

            MyAssert.DoesNotCauseDecrease(
                () => result = _target.Create(entity),
                () => Repository.GetAll().Count());
        }

        #endregion

        #region Start/End

        [TestMethod]
        public void TestStartStartsAssignmentAndRedirectsToProductionWorkOrder()
        {
            var emp = GetEntityFactory<Employee>().Create();
            var orders = GetEntityFactory<ProductionWorkOrder>().CreateList(5);
            var eq1 = GetEntityFactory<EmployeeAssignment>().Create(new { AssignedTo = emp, ProductionWorkOrder = orders[3] });

            var model = _viewModelFactory.BuildWithOverrides<StartEmployeeAssignment, EmployeeAssignment>(eq1,
                new {DateStarted = DateTime.Now});
            var result = _target.Start(model) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["Action"]);
            Assert.AreEqual("ProductionWorkOrder", result.RouteValues["Controller"]);
            Assert.AreEqual(orders[3].Id, result.RouteValues["Id"]);
        }

        [TestMethod]
        public void TestEndStartsAssignmentAndRedirectsToProductionWorkOrder()
        {
            var emp = GetEntityFactory<Employee>().Create();
            var orders = GetEntityFactory<ProductionWorkOrder>().CreateList(5);
            var eq1 = GetEntityFactory<EmployeeAssignment>().Create(new {
                AssignedTo = emp, ProductionWorkOrder = orders[2]
            });

            var model = _viewModelFactory.BuildWithOverrides<EndEmployeeAssignment, EmployeeAssignment>(eq1,
                new {DateEnded = DateTime.Now});
            var result = _target.End(model) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["Action"]);
            Assert.AreEqual("ProductionWorkOrder", result.RouteValues["Controller"]);
            Assert.AreEqual(orders[2].Id, result.RouteValues["Id"]);
        }

        [TestMethod]
        public void TestEndRedirectsToFinalizationTabForLastAssignmentFromProductionWorkOrders()
        {
            var emp = GetEntityFactory<Employee>().Create();
            var orders = GetEntityFactory<ProductionWorkOrder>().CreateList(5);
            var eq1 = GetEntityFactory<EmployeeAssignment>().Create(new {
                AssignedTo = emp,
                ProductionWorkOrder = orders[2]
            });
            orders[2].CurrentAssignments.Add(eq1);

            var model = _viewModelFactory.BuildWithOverrides<EndEmployeeAssignment, EmployeeAssignment>(eq1,
                new { DateEnded = DateTime.Now, DateStarted = DateTime.Now });

            const string url = "http://somesite.com/Production/ProductionWorkOrder/Show/3";
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri(url));

            var result = _target.End(model) as RedirectResult;

            Assert.AreEqual(url + EmployeeAssignmentController.FINALIZATION_FRAGMENT, result?.Url);
        }

        [TestMethod]
        public void TestEndRedirectsToProductionWorkOrdersShowForLastAssignmentFromEmployeeAssignments()
        {
            var emp = GetEntityFactory<Employee>().Create();
            var orders = GetEntityFactory<ProductionWorkOrder>().CreateList(5);
            var eq1 = GetEntityFactory<EmployeeAssignment>().Create(new {
                AssignedTo = emp,
                ProductionWorkOrder = orders[2]
            });
            orders[2].CurrentAssignments.Add(eq1);

            var model = _viewModelFactory.BuildWithOverrides<EndEmployeeAssignment, EmployeeAssignment>(eq1,
                new { DateEnded = DateTime.Now, DateStarted = DateTime.Now });

            const string url = "http://somesite.com/Production/EmployeeAssignments/Show/3";
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri(url));

            var result = _target.End(model) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["Action"]);
            Assert.AreEqual("ProductionWorkOrder", result.RouteValues["Controller"]);
            Assert.AreEqual(orders[2].Id, result.RouteValues["Id"]);
        }

        [TestMethod]
        public void TestEndRedirectsToProductionWorkOrdersShowWhenNotLastAssignmentFromProductionWorkOrders()
        {
            var emp = GetEntityFactory<Employee>().Create();
            var orders = GetEntityFactory<ProductionWorkOrder>().CreateList(5);
            var eq1 = GetEntityFactory<EmployeeAssignment>().Create(new {
                AssignedTo = emp,
                ProductionWorkOrder = orders[2]
            });
            orders[2].CurrentAssignments.Add(eq1);
            var emp2 = GetEntityFactory<Employee>().Create();
            var eq2 = GetEntityFactory<EmployeeAssignment>().Create(new {
                AssignedTo = emp2,
                ProductionWorkOrder = orders[2]
            });
            orders[2].CurrentAssignments.Add(eq2);

            var model = _viewModelFactory.BuildWithOverrides<EndEmployeeAssignment, EmployeeAssignment>(eq1,
                new { DateEnded = DateTime.Now, DateStarted = DateTime.Now });

            const string url = "http://somesite.com/Production/ProductionWorkOrder/Show/3";
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri(url));

            var result = _target.End(model) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["Action"]);
            Assert.AreEqual("ProductionWorkOrder", result.RouteValues["Controller"]);
            Assert.AreEqual(orders[2].Id, result.RouteValues["Id"]);
        }

        [TestMethod]
        public void TestEndSavesANote()
        {
            var noteRepo = _container.GetInstance<IRepository<Note>>();
            var emp = GetEntityFactory<Employee>().Create();
            var orders = GetEntityFactory<ProductionWorkOrder>().CreateArray(5);
            var eq1 = GetEntityFactory<EmployeeAssignment>().Create(new {
                AssignedTo = emp,
                ProductionWorkOrder = orders[2]
            });

            var model = _viewModelFactory.BuildWithOverrides<EndEmployeeAssignment, EmployeeAssignment>(eq1,
                new {
                    DateEnded = _now,
                    Notes = "foo bar"
                });

            MyAssert.CausesIncrease(() => _target.End(model), noteRepo.GetAll().Count);
            var note = noteRepo.GetAll().ToList().Last();
            Assert.AreEqual("foo bar", note.Text);
            Assert.AreEqual(_currentUser.UserName, note.CreatedBy);
            Assert.AreEqual(_pwoDataType, note.DataType);
            Assert.AreEqual(orders[2].Id, note.LinkedId);
        }

        #endregion

        #region EmployeeAssignment

        [TestMethod]
        public void TestAddEmployeeAssignmentSendsNotificationToTheAssignedEmployeesEmailAddressForEachProductionWorkOrder()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var notificationPurpose = ProductionWorkOrderController.ASSIGNED_NOTIFICATION;
            var employee = GetEntityFactory<Employee>().Create(new { EmailAddress = "employee@work.com" });
            var existingPwo = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = opc });
            var existingPwo2 = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = opc });
            var viewModel = new CreateEmployeeAssignment {
                AssignedTo = new[] { employee.Id },
                AssignedFor = DateTime.Now,
                ProductionWorkOrderIds = new [] { existingPwo.Id, existingPwo2.Id }
            };

            _target.Create(viewModel);

            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose && a.Address == employee.EmailAddress)), Times.Exactly(2));
        }

        [TestMethod]
        public void TestAddEmployeeAssignmentDisplaysNotificationErrorIfTheAssignedEmployeeDoesNotHaveAnEmailAddress()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employees = GetEntityFactory<Employee>().CreateList(2);
            employees.ForEach(x => x.EmailAddress = null);
            var existingPwo = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = opc,
            });

            var viewModel = new CreateEmployeeAssignment {
                AssignedTo = employees.Select(x => x.Id).ToArray(),
                AssignedFor = DateTime.Now,
                ProductionWorkOrderIds = new[] { existingPwo.Id }
            };

            _target.Create(viewModel);

            foreach (var employee in employees)
            {
                _target.AssertTempDataContainsMessage($"Unable to send assignment notification to {employee.FullName} because their employee record is missing an email address.", MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY);
            }

            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        #endregion

        #endregion
    }
}
