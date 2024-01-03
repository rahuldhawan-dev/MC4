using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class UnionContractProposalControllerTest : MapCallMvcControllerTestBase<UnionContractProposalController, UnionContractProposal, UnionContractProposalRepository>
    {
        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.HumanResourcesUnion;
                a.RequiresRole("~/UnionContractProposal/Search/", module, RoleActions.Read);
                a.RequiresRole("~/UnionContractProposal/Index/", module, RoleActions.Read);
                a.RequiresRole("~/UnionContractProposal/Show/", module, RoleActions.Read);
                a.RequiresRole("~/UnionContractProposal/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/UnionContractProposal/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/UnionContractProposal/New/", module, RoleActions.Add);
                a.RequiresRole("~/UnionContractProposal/Create/", module, RoleActions.Add);
                a.RequiresRole("~/UnionContractProposal/Destroy/", module, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetFactory<UnionContractProposalFactory>().Create(new {Notes = "description 0"});
            var entity1 = GetFactory<UnionContractProposalFactory>().Create(new {Notes = "description 1"});
            var search = new SearchUnionContractProposal();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Notes, "Notes");
                helper.AreEqual(entity1.Notes, "Notes", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetFactory<UnionContractProposalFactory>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditUnionContractProposal, UnionContractProposal>(eq, new {
                Notes = expected
            }));

            Assert.AreEqual(expected, Session.Get<UnionContractProposal>(eq.Id).Notes);
        }

        #endregion
    }
}
