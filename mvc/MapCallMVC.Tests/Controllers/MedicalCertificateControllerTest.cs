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
    public class MedicalCertificateControllerTest : MapCallMvcControllerTestBase<MedicalCertificateController, MedicalCertificate, MedicalCertificateRepository>
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
                var model = (CreateMedicalCertificate)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var role = MedicalCertificateController.ROLE;
                a.RequiresRole("~/MedicalCertificate/Search/", role);
                a.RequiresRole("~/MedicalCertificate/Index/", role);
                a.RequiresRole("~/MedicalCertificate/Show/", role);
                a.RequiresRole("~/MedicalCertificate/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/MedicalCertificate/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/MedicalCertificate/New/", role, RoleActions.Add);
                a.RequiresRole("~/MedicalCertificate/Renew/", role, RoleActions.Add);
                a.RequiresRole("~/MedicalCertificate/Create/", role, RoleActions.Add);
                a.RequiresRole("~/MedicalCertificate/Destroy/", role, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<MedicalCertificate>().Create(new {Comments = "comments 0"});
            var entity1 = GetEntityFactory<MedicalCertificate>().Create(new {Comments = "comments 1"});
            var search = new SearchMedicalCertificate();
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
            var eq = GetEntityFactory<MedicalCertificate>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditMedicalCertificate, MedicalCertificate>(eq, new {
                Comments = expected
            }));

            Assert.AreEqual(expected, Session.Get<MedicalCertificate>(eq.Id).Comments);
        }

        #endregion

        #region Renew

        [TestMethod]
        public void TestRenewReturnsRenewViewWithACreateModelThatIncludesTheExistingOperatingCenterAndEmployeeValues()
        {
            var entity = GetFactory<MedicalCertificateFactory>().Create();
            // Need to refresh because otherwise OperatingCenter will be null due to it being mapped as a formula.
            Session.Refresh(entity);
            var result = (ViewResult)_target.Renew(entity.Id);
            MvcAssert.IsViewNamed(result, "Renew");
            var model = (CreateMedicalCertificate)result.Model;
            Assert.AreEqual(entity.OperatingCenter.Id, model.OperatingCenter);
            Assert.AreEqual(entity.Employee.Id, model.Employee);
            Assert.IsNull(model.CertificationDate);
            Assert.IsNull(model.Comments);
            Assert.IsNull(model.ExpirationDate);
        }

        #endregion
    }
}
