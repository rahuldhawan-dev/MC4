using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using System.Web.Mvc;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace MapCallMVC.Tests.Controllers
{
    /// <summary>
    /// Summary description for UnionContractControllerTest
    /// </summary>
    [TestClass]
    public class UnionContractControllerTest : MapCallMvcControllerTestBase<UnionContractController, UnionContract, UnionContractRepository>
    {
        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _authenticationService.SetupGet(x => x.CurrentUser).Returns(_currentUser);
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.HumanResourcesUnion;
                a.RequiresRole("~/UnionContract/Search/", module);
                a.RequiresRole("~/UnionContract/Index/", module);
                a.RequiresRole("~/UnionContract/Show/", module);
                a.RequiresRole("~/UnionContract/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/UnionContract/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/UnionContract/New/", module, RoleActions.Add);
                a.RequiresRole("~/UnionContract/Create/", module, RoleActions.Add);
                a.RequiresRole("~/UnionContract/Destroy/", module, RoleActions.Delete);
                a.RequiresRole("~/UnionContract/ByOperatingCenterId/", module);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = _container.GetInstance<TestDataFactory<UnionContract>>().Create(new {Description = "description 0"});
            var entity1 = _container.GetInstance<TestDataFactory<UnionContract>>().Create(new {Description = "description 1"});
            var search = new SearchUnionContract();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = _container.GetInstance<TestDataFactory<UnionContract>>().Create();
            var expected = "term of contract field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditUnionContract, UnionContract>(eq, new {
                TermOfContract = expected
            }));

            Assert.AreEqual(expected, Session.Get<UnionContract>(eq.Id).TermOfContract);
        }

        #endregion
    }
}
