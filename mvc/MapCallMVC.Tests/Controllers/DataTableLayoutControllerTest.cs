using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class DataTableLayoutControllerTest : MapCallMvcControllerTestBase<DataTableLayoutController, DataTableLayout>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateDataTableLayout)vm;
                model.Properties = new string[0];
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditDataTableLayout)vm;
                model.Properties = new string[] { nameof(TestViewModel.Property) };
                model.TypeGuid = TypeCache.RegisterType(typeof(TestViewModel));
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            // We need to use a url for an actual EntityLookup here, can't just use EntityLookup since it's not valid.
            Authorization.Assert(a =>
            {
                a.RequiresLoggedInUserOnly("~/DataTableLayout/Create/");
                a.RequiresLoggedInUserOnly("~/DataTableLayout/Update/");
                a.RequiresLoggedInUserOnly("~/DataTableLayout/Destroy/");
            });
        }

        #region Create

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // noop: This returns a json result. Tested below.
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // override, returns a json result when successful
            var model = new CreateDataTableLayout(_container);
            model.Area = "";
            model.Controller = "SomeController";
            model.LayoutName = "This is a layout";
            model.Properties = new string[] { };

            var result = (JsonResult)_target.Create(model);

            dynamic data = result.Data;
            Assert.IsTrue(data.success, "The success field should be true.");
            Assert.AreEqual(model.Id, data.id, "The id field should be equal to the new record that was created.");
            Assert.AreNotEqual(0, model.Id, "Sanity. The Id field should have been updated by the controller after saving.");
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // override, returns a json result when errors
            var model = new CreateDataTableLayout(_container);
            model.Properties = new string[] { };
            _target.ModelState.AddModelError("SomeErrorKey", "Some error message");
            var result = (JsonResult)_target.Create(model);

            dynamic data = result.Data;
            Assert.IsFalse(data.success);
        }

        [TestMethod]
        public void TestCreateDoesNotRequireSecureForm()
        {
            MyAssert.MethodHasAttribute<RequiresSecureFormAttribute>(_target, "Create",
                typeof(CreateDataTableLayout));
        }

        #endregion

        #region Update

        [TestMethod]
        public void TestUpdateDoesNotRequireSecureForm()
        {
            MyAssert.MethodHasAttribute<RequiresSecureFormAttribute>(_target, "Update",
                typeof(EditDataTableLayout));
        }

        [TestMethod]
        public override void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            Assert.Inconclusive("Test me. I return a json result");
        }

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            Assert.Inconclusive("Test me. I return a json result");
        }

        #endregion

        #region Destroy

        [TestMethod]
        public void TestDestroyDoesNotRequireSecureForm()
        {
            MyAssert.MethodHasAttribute<RequiresSecureFormAttribute>(_target, "Destroy",
                typeof(int));
        }

        [TestMethod]
        public override void TestDestroyRedirectsBackToShowPageOfAttemptedDeletedRecordIfThereAreModelStateErrors()
        {
            Assert.Inconclusive("Test me. I return a json result");
        }

        [TestMethod]
        public override void TestDestroyRedirectsToSearchPageWhenRecordIsSuccessfullyDestroyed()
        {
            Assert.Inconclusive("Test me. I return a json result");
        }

        #endregion

        #endregion

        #region Helper classes

        private class TestViewModel
        {
            public string Property { get; set; }
        }

        #endregion
    }
}
