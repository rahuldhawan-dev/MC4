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
    public class OneCallMarkoutResponseControllerTest : MapCallMvcControllerTestBase<OneCallMarkoutResponseController, OneCallMarkoutResponse>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = OneCallMarkoutResponseController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/OneCallMarkoutResponse/Search/", role);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutResponse/Show/", role);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutResponse/Index/", role);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutResponse/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutResponse/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutResponse/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutResponse/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutResponse/Destroy/", role, RoleActions.Delete);
            });
        }				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<OneCallMarkoutResponse>().Create(new {Comments = "description 0"});
            var entity1 = GetEntityFactory<OneCallMarkoutResponse>().Create(new {Comments = "description 1"});
            var search = new SearchOneCallMarkoutResponse();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Comments, "Comments");
                helper.AreEqual(entity1.Comments, "Comments", 1);
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override because of New parameters.
            var ticket = GetEntityFactory<OneCallMarkoutTicket>().Create();
            var result = (ViewResult)_target.New(ticket.Id, String.Empty);

            MyAssert.IsInstanceOfType<ActionResult>(result);
            MyAssert.IsInstanceOfType<CreateOneCallMarkoutResponse>(result.Model);
        }

        [TestMethod]
        public void TestNewThrowsNotFoundResultWhenNotStartedFromATicket()
        {
            var result = _target.New(666, String.Empty) as HttpNotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(OneCallMarkoutResponseController.TICKET_NOT_FOUND, result.StatusDescription);

            result = _target.New(null, String.Empty) as HttpNotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(OneCallMarkoutResponseController.STARTS_WITH_TICKET, result.StatusDescription);
        }

        [TestMethod]
        public void TestNewSetsViewDataWithIndexQueryStringIfSent()
        {
            var ticket = GetEntityFactory<OneCallMarkoutTicket>().Create();
            var indexQS = "foo bar";

            _target.New(ticket.Id, indexQS);

            Assert.AreEqual(indexQS, _target.ViewData[OneCallMarkoutTicketController.TICKET_INDEX_QS_KEY]);
        }

        [TestMethod]
        public void TestCreateRedirectsToIndexWithIndexQueryStringOnSuccessIfSent()
        {
            var ticket = GetEntityFactory<OneCallMarkoutTicket>().Create();
            var model = _viewModelFactory.Build<CreateOneCallMarkoutResponse, OneCallMarkoutResponse>( GetEntityFactory<OneCallMarkoutResponse>().Build(new { OneCallMarkoutTicket = ticket }));
            model.IndexQS = "foo bar";

            var result = (RedirectResult)_target.Create(model);

            Assert.IsNotNull(result);
            Assert.AreEqual("/FieldOperations/OneCallMarkoutTicketfoo bar", result.Url);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<OneCallMarkoutResponse>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditOneCallMarkoutResponse, OneCallMarkoutResponse>(eq, new {
                Comments = expected
            }));

            Assert.AreEqual(expected, Session.Get<OneCallMarkoutResponse>(eq.Id).Comments);
        }

        #endregion
    }
}
