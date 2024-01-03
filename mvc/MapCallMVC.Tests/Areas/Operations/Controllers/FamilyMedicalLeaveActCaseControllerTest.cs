using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Operations.Controllers;
using MapCallMVC.Areas.Operations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Areas.Operations.Controllers
{
    [TestClass]
    public class FamilyMedicalLeaveActCaseControllerTest : MapCallMvcControllerTestBase<FamilyMedicalLeaveActCaseController, FamilyMedicalLeaveActCase>
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
            var role = FamilyMedicalLeaveActCaseController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Operations/FamilyMedicalLeaveActCase/Search/", role);
                a.RequiresRole("~/Operations/FamilyMedicalLeaveActCase/Show/", role);
                a.RequiresRole("~/Operations/FamilyMedicalLeaveActCase/Index/", role);
                a.RequiresRole("~/Operations/FamilyMedicalLeaveActCase/SendNotification/", role);
                a.RequiresRole("~/Operations/FamilyMedicalLeaveActCase/New/", role, RoleActions.Add);
                a.RequiresRole("~/Operations/FamilyMedicalLeaveActCase/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Operations/FamilyMedicalLeaveActCase/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Operations/FamilyMedicalLeaveActCase/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Operations/FamilyMedicalLeaveActCase/Destroy/", role, RoleActions.Delete);
                a.RequiresLoggedInUserOnly("~/Operations/FamilyMedicalLeaveActCase/GetByEmployeeId/");
			});
		}				

        #endregion

        #region ByEmployeeId

        [TestMethod]
        public void TestByEmployeeIdReturnsFMLACasesForEmployee()
        {
            var employee = GetEntityFactory<Employee>().Create();
            var valid = GetEntityFactory<FamilyMedicalLeaveActCase>().CreateList(3, new {Employee = employee});
            var invalid = GetEntityFactory<FamilyMedicalLeaveActCase>().Create();

            var result = (CascadingActionResult)_target.GetByEmployeeId(employee.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(valid.Count(), actual.Count() - 1); //--select here--
            foreach (var selectListItem in actual)
            {
                Assert.AreNotEqual(selectListItem.Value, invalid.Id.ToString());
            }
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<FamilyMedicalLeaveActCase>().Create(new {FrequencyDays = "description 0"});
            var entity1 = GetEntityFactory<FamilyMedicalLeaveActCase>().Create(new {FrequencyDays = "description 1"});
            var search = new SearchFamilyMedicalLeaveActCase();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.FrequencyDays, "Frequency");
                helper.AreEqual(entity1.FrequencyDays, "Frequency", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<FamilyMedicalLeaveActCase>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditFamilyMedicalLeaveActCase, FamilyMedicalLeaveActCase>(eq, new {
                FrequencyDays = expected
            }));

            Assert.AreEqual(expected, Session.Get<FamilyMedicalLeaveActCase>(eq.Id).FrequencyDays);
        }

        #endregion


        #region Other

        [TestMethod]
        public void TestSendNotificationErrorsWhenEntityNotFound()
        {
            var result = _target.SendNotification(666);

            _target.AssertTempDataContainsMessage(
                String.Format(FamilyMedicalLeaveActCaseController.NOT_FOUND, "666"),
                FamilyMedicalLeaveActCaseController.ERROR_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestSendNotificationErrorsWhenReportsToNullOrHasNoEmail()
        {
            var entity = GetEntityFactory<FamilyMedicalLeaveActCase>().Create();

            var result = _target.SendNotification(entity.Id);

            _target.AssertTempDataContainsMessage(FamilyMedicalLeaveActCaseController.SUPERVISOR_ERROR,
                FamilyMedicalLeaveActCaseController.ERROR_MESSAGE_KEY);

            var boss = GetEntityFactory<Employee>().Create(new { EmailAddress = "" });
            entity.Employee.ReportsTo = boss;
            Session.Save(entity);

            Assert.AreSame(boss, entity.Employee.ReportsTo);
            _target.AssertTempDataContainsMessage(FamilyMedicalLeaveActCaseController.SUPERVISOR_ERROR,
                FamilyMedicalLeaveActCaseController.ERROR_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestSendsNotificationSendsNotificatWhenAllIsWellAndRightInTheWorld()
        {
            var supervisor = GetEntityFactory<Employee>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { ReportsTo = supervisor });
            var entity = GetEntityFactory<FamilyMedicalLeaveActCase>().Create(new { Employee = employee });
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.SendNotification(entity.Id);

            _target.AssertTempDataContainsMessage(
                FamilyMedicalLeaveActCaseController.SENT_SUCCESSFULLY, 
                FamilyMedicalLeaveActCaseController.SUCCESS_MESSAGE_KEY);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(FamilyMedicalLeaveActCaseController.ROLE, resultArgs.Module);
            Assert.AreEqual(FamilyMedicalLeaveActCaseController.NOTIFICATION_PURPOSE, resultArgs.Purpose);
        }

        #endregion

	}
}
