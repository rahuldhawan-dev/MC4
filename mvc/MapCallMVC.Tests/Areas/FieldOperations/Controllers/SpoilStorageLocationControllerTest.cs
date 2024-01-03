using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class SpoilStorageLocationControllerTest : MapCallMvcControllerTestBase<SpoilStorageLocationController, SpoilStorageLocation>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateSpoilStorageLocation)vm;
                model.State = GetEntityFactory<State>().Create().Id;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/FieldOperations/SpoilStorageLocation/ByOperatingCenterId");
                a.RequiresRole("~/FieldOperations/SpoilStorageLocation/Edit/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilStorageLocation/Update/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilStorageLocation/New/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilStorageLocation/Create/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilStorageLocation/Index/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilStorageLocation/Show/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilStorageLocation/Search/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because the search model has required properties that
            // the auto-tester can't set with expected values. This means no
            // results are returned.
            var eq1 = GetEntityFactory<SpoilStorageLocation>().Create();
            var eq2 = GetEntityFactory<SpoilStorageLocation>().Create();
            var search = new SearchSpoilStorageLocation();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchSpoilStorageLocation)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreSame(eq1, resultModel[0]);
            Assert.AreSame(eq2, resultModel[1]);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<SpoilStorageLocation>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditSpoilStorageLocation, SpoilStorageLocation>(eq, new {
                Name = expected
            }));

            Assert.AreEqual(expected, Session.Get<SpoilStorageLocation>(eq.Id).Name);
        }

        [TestMethod]
        public void TestByOperatingCenterIdReturnsActiveStorageLocationByOperatingCenterId()
        {
            var operatingCenterA = GetFactory<OperatingCenterFactory>().Create();
            var operatingCenterB = GetFactory<OperatingCenterFactory>().Create();
            var ssl = GetEntityFactory<SpoilStorageLocation>().Create(new {
                OperatingCenter = operatingCenterA,
                Active = true
            });
            var ssl2 = GetEntityFactory<SpoilStorageLocation>().Create(new {
                OperatingCenter = operatingCenterB,
                Active = false
            });

            var actionResult = (CascadingActionResult)_target.ByOperatingCenterId(operatingCenterA.Id);

            var actual = actionResult.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1);
        }

        #endregion
    }
}
