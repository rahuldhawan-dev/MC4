using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class ServiceInstallationMaterialControllerTest : MapCallMvcControllerTestBase<ServiceInstallationMaterialController, ServiceInstallationMaterial>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules module = RoleModules.FieldServicesDataLookups;
                a.RequiresRole("~/FieldOperations/ServiceInstallationMaterial/Search/", module);
                a.RequiresRole("~/FieldOperations/ServiceInstallationMaterial/Index/", module);
                a.RequiresRole("~/FieldOperations/ServiceInstallationMaterial/Show/", module);
                a.RequiresRole("~/FieldOperations/ServiceInstallationMaterial/New/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ServiceInstallationMaterial/Create/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ServiceInstallationMaterial/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/ServiceInstallationMaterial/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/ServiceInstallationMaterial/Destroy/", module, RoleActions.Delete);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<ServiceInstallationMaterial>().Create();
            var expected = "3/4";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<ServiceInstallationMaterialViewModel, ServiceInstallationMaterial>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<ServiceInstallationMaterial>(eq.Id).Description);
        }

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var nj = GetEntityFactory<State>().Create(new { Abbreviation = "NJ", Name = "New Jersey" });
            var ny = GetEntityFactory<State>().Create(new { Abbreviation = "NY", Name = "New York" });
            var nj7 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = nj, OperatingCenterCode = "NJ7" });
            var ny1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = ny, OperatingCenterCode = "NY1" });
            var sim1 = GetEntityFactory<ServiceInstallationMaterial>().Create(new { OperatingCenter = nj7 });
            var sim2 = GetEntityFactory<ServiceInstallationMaterial>().Create(new { OperatingCenter = ny1 });

            var searchViewModel = new SearchServiceInstallationMaterial { State = nj.Id };

            var result = _target.Index(searchViewModel);

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, searchViewModel.Count);
            Assert.AreEqual(sim1.Id, searchViewModel.Results.First().Id);

            searchViewModel.State = ny.Id;

            result = _target.Index(searchViewModel);

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, searchViewModel.Count);
            Assert.AreEqual(sim2.Id, searchViewModel.Results.First().Id);
        }

        #endregion
    }
}