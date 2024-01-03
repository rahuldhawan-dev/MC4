using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models.ViewModels;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class InterconnectionControllerTest : MapCallMvcControllerTestBase<InterconnectionController, Interconnection, InterconnectionRepository>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateInterconnection)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.ProductionInterconnections;
                a.RequiresRole("~/Interconnection/Search", module, RoleActions.Read);
                a.RequiresRole("~/Interconnection/Show", module, RoleActions.Read);
                a.RequiresRole("~/Interconnection/Index", module, RoleActions.Read);
                a.RequiresRole("~/Interconnection/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/Interconnection/Update", module, RoleActions.Edit);
                a.RequiresRole("~/Interconnection/New", module, RoleActions.Add);
                a.RequiresRole("~/Interconnection/Create", module, RoleActions.Add);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var interconnection1 = GetFactory<InterconnectionFactory>().Create(new { ProgramInterestNumber = "111" });
            var interconnection2 = GetFactory<InterconnectionFactory>().Create(new { ProgramInterestNumber = "222" });
            var search = new SearchInterconnection();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(interconnection1.Id, "Id");
                helper.AreEqual(interconnection1.ProgramInterestNumber, "ProgramInterestNumber");
                helper.AreEqual(interconnection2.Id, "Id", 1);
                helper.AreEqual(interconnection2.ProgramInterestNumber, "ProgramInterestNumber", 1);
            }
        }

        #endregion
    }
}
