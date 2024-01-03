using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class TrafficControlTicketCheckControllerTest : MapCallMvcControllerTestBase<TrafficControlTicketCheckController, TrafficControlTicketCheck>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = TrafficControlTicketCheckController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/TrafficControlTicketCheck/Search/", role);
                a.RequiresRole("~/FieldOperations/TrafficControlTicketCheck/Show/", role);
                a.RequiresRole("~/FieldOperations/TrafficControlTicketCheck/Index/", role);
                a.RequiresRole("~/FieldOperations/TrafficControlTicketCheck/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/TrafficControlTicketCheck/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/TrafficControlTicketCheck/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/TrafficControlTicketCheck/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/TrafficControlTicketCheck/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion

        #region Show

        [TestMethod]
        public void TestShowShowsANotificationIfADuplicateCheckNumberExists()
        {
            var entity = GetEntityFactory<TrafficControlTicketCheck>().Create();
            var dup = GetEntityFactory<TrafficControlTicketCheck>().Create(new {CheckNumber = entity.CheckNumber});

            var result = _target.Show(entity.Id) as ViewResult;

            MvcAssert.IsViewWithModel(result, entity);
            _target.AssertTempDataContainsMessage(
                String.Format(TrafficControlTicketCheckController.DUPLICATE_CHECK_NUMBER, entity.CheckNumber), 
                TrafficControlTicketCheckController.NOTIFICATION_MESSAGE_KEY);
        }

		#endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<TrafficControlTicketCheck>().Create(new {Memo = "description 0"});
            var entity1 = GetEntityFactory<TrafficControlTicketCheck>().Create(new {Memo = "description 1"});
            var search = new SearchTrafficControlTicketCheck();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Memo, "Memo");
                helper.AreEqual(entity1.Memo, "Memo", 1);
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override needed for New parameter
            var ticket = GetEntityFactory<TrafficControlTicket>().Create();
            var result = (ViewResult)_target.New(ticket.Id);

            MyAssert.IsInstanceOfType<CreateTrafficControlTicketCheck>(result.Model);
        }

        [TestMethod]
        public void TestNewReturnsNotFoundBecauseTicketNotProvided()
        {
            var result = _target.New(null) as HttpNotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(TrafficControlTicketCheckController.STARTS_WITH_TICKET, result.StatusDescription);
        }

        [TestMethod]
        public void TestNewReturnsNotFoundBecauseTicketWasNotFound()
        {
            var result = _target.New(666) as HttpNotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(TrafficControlTicketCheckController.TICKET_NOT_FOUND, result.StatusDescription);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<TrafficControlTicketCheck>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditTrafficControlTicketCheck, TrafficControlTicketCheck>(eq, new {
                Memo = expected
            }));

            Assert.AreEqual(expected, Session.Get<TrafficControlTicketCheck>(eq.Id).Memo);
        }

        #endregion
	}
}
