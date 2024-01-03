using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class RestorationControllerTest : MapCallMvcControllerTestBase<RestorationController, Restoration>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateRestoration)vm;
                model.WBSNumber = "12345";
                model.EstimatedRestorationFootage = 1m;
                model.InitialPurchaseOrderNumber = "44444";
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditRestoration)vm;
                model.WBSNumber = "12345";
                model.EstimatedRestorationFootage = 1m;
                model.InitialPurchaseOrderNumber = "44444";
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/Restoration/Show", role, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/Restoration/Search", role, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/Restoration/Index", role, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/Restoration/New", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/Restoration/Create", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/Restoration/Edit", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/Restoration/Update", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/Restoration/Destroy", role, RoleActions.Delete);
            });
        }

        #region Show

        [TestMethod]
        public void TestShowRespondsToFragment()
        {
            var good = GetEntityFactory<Restoration>().Create();
            InitializeControllerAndRequest("~/FieldOperations/Restoration/Show/" + good.Id + ".frag");

            var result = _target.Show(good.Id);
            MvcAssert.IsViewNamed(result, "_ShowPopup");
            MvcAssert.IsViewWithModel(result, good);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetFactory<RestorationFactory>().Create();
            var expected = 42;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditRestoration, Restoration>(eq, new {
                FinalPavingSquareFootage = expected
            }));

            Assert.AreEqual(expected, Session.Get<Restoration>(eq.Id).FinalPavingSquareFootage);
        }

        #endregion

        #endregion
    }
}
