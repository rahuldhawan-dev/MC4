using System;
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
    public class DriversLicenseControllerTest : MapCallMvcControllerTestBase<DriversLicenseController, DriversLicense, DriversLicenseRepository>
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
                var model = (CreateDriversLicense)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }

        #endregion

        #region Exposed Methods

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var role = DriversLicenseController.ROLE;
                a.RequiresRole("~/DriversLicense/Search/", role);
                a.RequiresRole("~/DriversLicense/Index/", role);
                a.RequiresRole("~/DriversLicense/Show/", role);
                a.RequiresRole("~/DriversLicense/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/DriversLicense/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/DriversLicense/New/", role, RoleActions.Add);
                a.RequiresRole("~/DriversLicense/Create/", role, RoleActions.Add);
                a.RequiresRole("~/DriversLicense/Renew", role, RoleActions.Add);
                a.RequiresRole("~/DriversLicense/Destroy/", role, RoleActions.Delete);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            DateTime date0 = DateTime.Now, date1 = DateTime.Now;
            var entity0 = GetEntityFactory<DriversLicense>().Create(new { IssuedDate = date0 });
            var entity1 = GetEntityFactory<DriversLicense>().Create(new { IssuedDate = date1 });
            var search = new SearchDriversLicense();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.IssuedDate, "IssuedDate");
                helper.AreEqual(entity1.IssuedDate, "IssuedDate");
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<DriversLicense>().Create();
            var date = DateTime.Now;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditDriversLicense, DriversLicense>(eq, new {
                IssuedDate = date
            }));
            eq = Session.Get<DriversLicense>(eq.Id);

            Assert.AreEqual(date, eq.IssuedDate);
        }

        #endregion

        #region Renew

        [TestMethod]
        public void TestRenewCallsActionHelperAndGetsResult()
        {
            var existing = GetFactory<DriversLicenseFactory>().Create();
            var result = (ViewResult)_target.Renew(existing.Id);
            MvcAssert.IsViewNamed(result, "Renew");
            var model = (CreateDriversLicense)result.Model;

            Assert.IsNull(model.RenewalDate);
            Assert.IsNull(model.IssuedDate);
        }

        #endregion
    }
}
