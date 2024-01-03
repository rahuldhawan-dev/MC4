using System;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.HydrantPaintings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class HydrantPaintingControllerTest
        : MapCallMvcControllerTestBase<HydrantPaintingController, HydrantPainting>
    {
        #region Setup/Teardown

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);

            options.ExpectedIndexViewName = "_Index";
            options.IndexDisplaysViewWhenNoResults = false;
            options.IndexRedirectsToShowForSingleResult = false;
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesAssets;
            var path = "~/HydrantPainting/";
            Authorization.Assert(test => {
                test.RequiresRole(path + "Index/", role);
                test.RequiresRole(path + "Create/", role, RoleActions.Edit);
                test.RequiresRole(path + "Update/", role, RoleActions.Edit);
                test.RequiresRole(path + "Destroy/", role, RoleActions.Edit);
            });
        }

        #endregion

        #region Create

        [TestMethod]
        public void Test_Create_RedirectsToTheHydrantShowPage_AfterSuccessfullySaving()
        {
            var hydrant = GetEntityFactory<Hydrant>().Create();
            var viewModel = _viewModelFactory.BuildWithOverrides<CreateHydrantPainting>(new {
                Hydrant = hydrant.Id,
                PaintedAt = _now
            });

            var result = _target.Create(viewModel);

            MvcAssert.RedirectsToRoute(
                result,
                "Hydrant",
                "Show",
                new { hydrant.Id });
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            Assert.Inconclusive("this test is not valid for this controller");
        }

        [TestMethod]
        public void Test_Create_RedirectsToTheHydrantShowPage_IfModelStateErrorsExist()
        {
            var hydrant = GetEntityFactory<Hydrant>().Create();
            // PaintedAt is a required property
            var viewModel = _viewModelFactory.BuildWithOverrides<CreateHydrantPainting>(new {
                Hydrant = hydrant.Id
            });

            var result = _target.Create(viewModel);

            MvcAssert.RedirectsToRoute(
                result,
                "Hydrant",
                "Show",
                new { hydrant.Id });
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            Assert.Inconclusive("this test is not valid for this controller");
        }

        #endregion

        #region Update

        [TestMethod]
        public void Test_Update_RedirectsToTheHydrantShowPage_AfterSuccessfullySaving()
        {
            var hydrant = GetEntityFactory<Hydrant>().Create();
            var painting = GetEntityFactory<HydrantPainting>().Create(new { Hydrant = hydrant });
            var viewModel = _viewModelFactory
               .BuildWithOverrides<EditHydrantPainting, HydrantPainting>(painting, new {
                    PaintedAt = _now.AddDays(1)
                });

            var result = _target.Update(viewModel);

            MvcAssert.RedirectsToRoute(
                result,
                "Hydrant",
                "Show",
                new { hydrant.Id });
        }

        [TestMethod]
        public override void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            Assert.Inconclusive("this test is not valid for this controller");
        }

        [TestMethod]
        public void Test_Update_RedirectsToTheHydrantShowPage_IfModelStateErrorsExist()
        {
            var hydrant = GetEntityFactory<Hydrant>().Create();
            var painting = GetEntityFactory<HydrantPainting>().Create(new { Hydrant = hydrant });
            var viewModel = _viewModelFactory
               .BuildWithOverrides<EditHydrantPainting, HydrantPainting>(painting, new {
                    PaintedAt = (DateTime?)null
                });

            var result = _target.Update(viewModel);

            MvcAssert.RedirectsToRoute(
                result,
                "Hydrant",
                "Show",
                new { hydrant.Id });
        }

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            Assert.Inconclusive("this test is not valid for this controller");
        }

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.FRAGMENT;

            base.TestIndexReturnsResults();
        }

        #endregion

        #region Destroy

        [TestMethod]
        public override void TestDestroyRedirectsToSearchPageWhenRecordIsSuccessfullyDestroyed()
        {
            Assert.Inconclusive("this test is not valid for this controller");
        }

        [TestMethod]
        public override void TestDestroyRedirectsBackToShowPageOfAttemptedDeletedRecordIfThereAreModelStateErrors()
        {
            Assert.Inconclusive("this test is not valid for this controller");
        }

        [TestMethod]
        public void Test_Destroy_RedirectsToTheHydrantShowPage_AfterSuccessfullySaving()
        {
            var hydrant = GetEntityFactory<Hydrant>().Create();
            var painting = GetEntityFactory<HydrantPainting>().Create(new { Hydrant = hydrant });
            var viewModel = _viewModelFactory.Build<DeleteHydrantPainting, HydrantPainting>(painting);

            var result = _target.Destroy(viewModel);

            MvcAssert.RedirectsToRoute(
                result,
                "Hydrant",
                "Show",
                new { hydrant.Id });
        }

        #endregion
    }
}
