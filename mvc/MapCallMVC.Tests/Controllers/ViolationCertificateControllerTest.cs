using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class ViolationCertificateControllerTest : MapCallMvcControllerTestBase<ViolationCertificateController, ViolationCertificate, ViolationCertificateRepository>
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
                var model = (CreateViolationCertificate)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }
		
        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var role = ViolationCertificateController.ROLE;
                a.RequiresRole("~/ViolationCertificate/Search/", role);
                a.RequiresRole("~/ViolationCertificate/Index/", role);
                a.RequiresRole("~/ViolationCertificate/Show/", role);
                a.RequiresRole("~/ViolationCertificate/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/ViolationCertificate/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/ViolationCertificate/New/", role, RoleActions.Add);
                a.RequiresRole("~/ViolationCertificate/Renew/", role, RoleActions.Add);
                a.RequiresRole("~/ViolationCertificate/Create/", role, RoleActions.Add);
                a.RequiresRole("~/ViolationCertificate/Destroy/", role, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ViolationCertificate>().Create(new { Comments = "comments 0" });
            var entity1 = GetEntityFactory<ViolationCertificate>().Create(new { Comments = "comments 1" });
            var search = new SearchViolationCertificate();
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

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<ViolationCertificate>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditViolationCertificate, ViolationCertificate>(eq, new {
                Comments = expected
            }));

            Assert.AreEqual(expected, Session.Get<ViolationCertificate>(eq.Id).Comments);
        }

        #endregion

        #region Renew

        [TestMethod]
        public void TestRenewReturnsRenewViewWithModelWithExistingOperatingCenterAndEmployeeSet()
        {
            var entity = GetFactory<ViolationCertificateFactory>().Create(new {
                Comments = "Comments yo",
                CertificateDate = DateTime.Now 
            });
            Session.Refresh(entity); // need to refresh in order to get OperatingCenter filled(it's a formula).
            var result = (ViewResult)_target.Renew(entity.Id);
            MvcAssert.IsViewNamed(result, "Renew");
            var model = (CreateViolationCertificate)result.Model;
            Assert.AreEqual(entity.OperatingCenter.Id, model.OperatingCenter);
            Assert.AreEqual(entity.Employee.Id, model.Employee);
            Assert.IsNull(model.CertificateDate);
            Assert.IsNull(model.Comments);
        }

        #endregion
    }
}
