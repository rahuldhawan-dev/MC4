using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using System;
using System.Web.Mvc;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class HepatitisBVaccinationControllerTest : MapCallMvcControllerTestBase<HepatitisBVaccinationController, HepatitisBVaccination, HepatitisBVaccinationRepository>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateHepatitisBVaccination)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }
        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var role = RoleModules.HumanResourcesEmployeeLimited;
                a.RequiresRole("~/HepatitisBVaccination/Search/", role);
                a.RequiresRole("~/HepatitisBVaccination/Index/", role);
                a.RequiresRole("~/HepatitisBVaccination/Show/", role);
                a.RequiresRole("~/HepatitisBVaccination/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/HepatitisBVaccination/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/HepatitisBVaccination/New/", role, RoleActions.Add);
                a.RequiresRole("~/HepatitisBVaccination/Create/", role, RoleActions.Add);
                a.RequiresRole("~/HepatitisBVaccination/Destroy/", role, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<HepatitisBVaccination>().Create(new { ResponseDate = Lambdas.GetNow });
            var entity1 = GetEntityFactory<HepatitisBVaccination>().Create(new { ResponseDate = Lambdas.GetYesterday });
            var search = new SearchHepatitisBVaccination();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.ResponseDate, "ResponseDate");
                helper.AreEqual(entity1.ResponseDate, "ResponseDate", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<HepatitisBVaccination>().Create();
            var expected = DateTime.Today;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditHepatitisBVaccination, HepatitisBVaccination>(eq, new {
                ResponseDate = expected
            }));

            Assert.AreEqual(expected, Session.Get<HepatitisBVaccination>(eq.Id).ResponseDate);
        }

        #endregion

    }
}