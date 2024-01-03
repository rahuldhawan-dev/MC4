using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Controllers
{
    [TestClass]
    public class EnvironmentalPermitFeeControllerTest : MapCallMvcControllerTestBase<EnvironmentalPermitFeeController, EnvironmentalPermitFee>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Needs to be Create due to Build overrides causing multiple records to be saved.
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "EnvironmentalPermit", id = vm.EnvironmentalPermit };
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "EnvironmentalPermit", id = vm.EnvironmentalPermit };
            options.CreateValidEntity = () => GetEntityFactory<EnvironmentalPermitFee>().Create();
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.EnvironmentalGeneral;

            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/EnvironmentalPermitFee/New/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalPermitFee/Create/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalPermitFee/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalPermitFee/Update/", role, RoleActions.Edit);
            });
        }

        #endregion

        #region New/Create

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            var permit = GetEntityFactory<EnvironmentalPermit>().Create();
            
            var result = (ViewResult)_target.New(permit.Id);
            MvcAssert.IsViewNamed(result, "New");
           
            var model = (CreateEnvironmentalPermitFee)result.Model;
            Assert.AreEqual(permit.Id, model.EnvironmentalPermit);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var fee = GetEntityFactory<EnvironmentalPermitFee>().Create();
            var expected = 500m;

            var viewModel = _viewModelFactory.Build<EditEnvironmentalPermitFee, EnvironmentalPermitFee>(fee);
            viewModel.Fee = expected;
            var result = _target.Update(viewModel);

            Assert.AreEqual(expected, Session.Get<EnvironmentalPermitFee>(fee.Id).Fee);
        }

        #endregion
    }
}
