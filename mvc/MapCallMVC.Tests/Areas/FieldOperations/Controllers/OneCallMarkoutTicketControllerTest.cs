using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class OneCallMarkoutTicketControllerTest : MapCallMvcControllerTestBase<OneCallMarkoutTicketController, OneCallMarkoutTicket>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.DoUpdateSingleViewModelParameterCheck = false; // There isn't an Update action.
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = OneCallMarkoutTicketController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/OneCallMarkoutTicket/Search/", role);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutTicket/Show/", role);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutTicket/Index/", role);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutTicket/GetTowns/", role);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutTicket/GetCounties/", role);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutTicket/Edit/", role, RoleActions.Edit);
            });
        }				

        #endregion

        #region Show

        [TestMethod]
        public void TestShowAddsViewDataWithQueryStringFromIndexIfFoundOnUrlReferrer()
        {
            var query = "?foo=bar";
            Request.Request.Setup(x => x.UrlReferrer).Returns(new Uri("http://foo.bar/FieldOperations/OneCallMarkoutTicket" + query));
            var entity = GetEntityFactory<OneCallMarkoutTicket>().Create();

            _target.Show(entity.Id);

            Assert.AreEqual(query, _target.ViewData[OneCallMarkoutTicketController.TICKET_INDEX_QS_KEY]);
        }

        [TestMethod]
        public void TestShowDoesNotAddViewDataWithQueryStringFromReferrerIfReferrerWasNotIndex()
        {
            var query = "?foo=bar";
            Request.Request.Setup(x => x.UrlReferrer).Returns(new Uri("http://foo.bar/baz" + query));
            var entity = GetEntityFactory<OneCallMarkoutTicket>().Create();

            _target.Show(entity.Id);

            Assert.IsNull(_target.ViewData[OneCallMarkoutTicketController.TICKET_INDEX_QS_KEY]);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<OneCallMarkoutTicket>().Create();
            var entity1 = GetEntityFactory<OneCallMarkoutTicket>().Create();
            var search = new SearchOneCallMarkoutTicket();
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

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestEditReturnsEditViewWithEditViewModel()
        {
            // noop: this action only ever returns HttpNotFound
        }

        [TestMethod]
        public void TestEditReturns404NotFoundForEverything()
        {
            var realLiveEntity = GetEntityFactory<OneCallMarkoutTicket>().Create();
            MvcAssert.IsNotFound(_target.Edit(realLiveEntity.Id));
        }

        #endregion
    }
}
