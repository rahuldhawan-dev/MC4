using System.Collections.Generic;
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
    public class SpoilFinalProcessingLocationControllerTest : MapCallMvcControllerTestBase<SpoilFinalProcessingLocationController, SpoilFinalProcessingLocation>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateSpoilFinalProcessingLocation)vm;
                model.State = GetEntityFactory<State>().Create().Id;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/FieldOperations/SpoilFinalProcessingLocation/GetByOperatingCenter");
                a.RequiresRole("~/FieldOperations/SpoilFinalProcessingLocation/Edit/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilFinalProcessingLocation/Update/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilFinalProcessingLocation/New/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilFinalProcessingLocation/Create/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilFinalProcessingLocation/Index/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilFinalProcessingLocation/Show/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilFinalProcessingLocation/Search/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because the search model has required properties that
            // the auto-tester can't set with expected values. This means no
            // results are returned.
            var eq1 = GetEntityFactory<SpoilFinalProcessingLocation>().Create();
            var eq2 = GetEntityFactory<SpoilFinalProcessingLocation>().Create();
            var search = new SearchSpoilFinalProcessingLocation();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchSpoilFinalProcessingLocation)result.Model).Results.ToList();

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
            var eq = GetEntityFactory<SpoilFinalProcessingLocation>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditSpoilFinalProcessingLocation, SpoilFinalProcessingLocation>(eq, new {
                Name = expected
            }));

            Assert.AreEqual(expected, Session.Get<SpoilFinalProcessingLocation>(eq.Id).Name);
        }

        [TestMethod]
        public void TestByOperatingCenterIdReturnsFinalDestinationsByOperatingCenterId()
        {
            var operatingCenterA = GetFactory<OperatingCenterFactory>().Create();
            var operatingCenterB = GetFactory<OperatingCenterFactory>().Create();
            var finalDestination1 = GetEntityFactory<SpoilFinalProcessingLocation>().Create(new {
                OperatingCenter = operatingCenterA
            });
            var finalDestination2 = GetEntityFactory<SpoilFinalProcessingLocation>().Create(new {
                OperatingCenter = operatingCenterB
            });

            var actionResult = (CascadingActionResult)_target.GetByOperatingCenter(operatingCenterA.Id);

            var actual = actionResult.GetSelectListItems();

            Assert.AreEqual(2, actual.Count() - 1);
        }
        #endregion
    }
}
