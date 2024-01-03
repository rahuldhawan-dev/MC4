using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Operations.Controllers;
using MapCallMVC.Areas.Operations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Areas.Operations.Controllers
{
    [TestClass]
    public class AbsenceNotificationControllerTest : MapCallMvcControllerTestBase<AbsenceNotificationController, AbsenceNotification, AbsenceNotificationRepository>
    {
        #region Private Members

        private Mock<INotificationService> _notifier;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotificationService>();

            _container.Inject(_notifier.Object);
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = AbsenceNotificationController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Operations/AbsenceNotification/Search/", role);
                a.RequiresRole("~/Operations/AbsenceNotification/Show/", role);
                a.RequiresRole("~/Operations/AbsenceNotification/Index/", role);
                a.RequiresRole("~/Operations/AbsenceNotification/SendNotification/", role);
                a.RequiresRole("~/Operations/AbsenceNotification/New/", role, RoleActions.Add);
                a.RequiresRole("~/Operations/AbsenceNotification/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Operations/AbsenceNotification/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Operations/AbsenceNotification/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Operations/AbsenceNotification/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { OperatingCenter = operatingCenter });
            var entity0 = GetEntityFactory<AbsenceNotification>().Create(new {SupervisorNotes = "description 0", Employee = employee});
            var entity1 = GetEntityFactory<AbsenceNotification>().Create(new { SupervisorNotes = "description 1", Employee = employee });
            var search = new SearchAbsenceNotification();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.SupervisorNotes, "SupervisorNotes");
                helper.AreEqual(entity1.SupervisorNotes, "SupervisorNotes", 1);
            }
        }

        #endregion

        #region New

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

        #region Create

        [TestMethod]
        public void TestCreateSentsAbsenceNotificationEntryNotifications()
        {
            var ent = GetEntityFactory<AbsenceNotification>().Create();
            
            var model = _viewModelFactory.Build<CreateAbsenceNotification, AbsenceNotification>( ent);
            model.Id = 0;

            // If this fails, the rest of the test will fail. Sanity check.
            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(entity.Employee.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(AbsenceNotificationController.ROLE, resultArgs.Module);
            Assert.AreEqual(AbsenceNotificationController.NOTIFICATION_PURPOSE_ENTRY, resultArgs.Purpose);
            Assert.AreEqual(AbsenceNotificationController.ABSENCE_NOTIFICATION_SUBJECT, resultArgs.Subject);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<AbsenceNotification>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditAbsenceNotification, AbsenceNotification>(eq, new {
                SupervisorNotes = expected
            }));

            Assert.AreEqual(expected, Session.Get<AbsenceNotification>(eq.Id).SupervisorNotes);
        }

        #endregion

        #region Other

        [TestMethod]
        public void TestSendNotificationErrorsWhenEntityNotFound()
        {
            var result = _target.SendNotification(666);

            _target.AssertTempDataContainsMessage(
                String.Format(AbsenceNotificationController.NOT_FOUND, "666"),
                AbsenceNotificationController.ERROR_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestSendNotificationErrorsWhenReportsToNullOrHasNoEmail()
        {
            var entity = GetEntityFactory<AbsenceNotification>().Create();
            
            var result = _target.SendNotification(entity.Id);

            _target.AssertTempDataContainsMessage(AbsenceNotificationController.ABSENCE_NOTIFICATION_SUPERVISOR_ERROR, AbsenceNotificationController.ERROR_MESSAGE_KEY);

            var boss = GetEntityFactory<Employee>().Create(new { EmailAddress = ""});
            entity.Employee.ReportsTo = boss;
            Session.Save(entity);

            Assert.AreSame(boss, entity.Employee.ReportsTo);
            _target.AssertTempDataContainsMessage(AbsenceNotificationController.ABSENCE_NOTIFICATION_SUPERVISOR_ERROR,
                AbsenceNotificationController.ERROR_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestSendsNotificationSendsNotificatWhenAllIsWellAndRightInTheWorld()
        {
            var supervisor = GetEntityFactory<Employee>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { ReportsTo = supervisor});
            var entity = GetEntityFactory<AbsenceNotification>().Create(new { Employee = employee });
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.SendNotification(entity.Id);

            _target.AssertTempDataContainsMessage(AbsenceNotificationController.SENT_SUCCESSFULLY,
                AbsenceNotificationController.SUCCESS_MESSAGE_KEY);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(AbsenceNotificationController.ROLE, resultArgs.Module);
            Assert.AreEqual(AbsenceNotificationController.NOTIFICATION_PURPOSE_SUPERVISOR, resultArgs.Purpose);
        }

        #endregion
    }
}
