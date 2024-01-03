using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class EnvironmentalPermitRequirementControllerTest : MapCallMvcControllerTestBase<EnvironmentalPermitRequirementController, EnvironmentalPermitRequirement>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditEnvironmentalPermitRequirement)vm;
                return new { action = "Show", controller = "EnvironmentalPermit", area = "Environmental", id = model.EnvironmentalPermit };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = EnvironmentalPermitRequirementController.ROLE;
                a.RequiresRole("~/Environmental/EnvironmentalPermitRequirement/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalPermitRequirement/Update/", module, RoleActions.Edit);
            });
        }

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            Assert.Inconclusive("Action throws error if server validation fails.");
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<EnvironmentalPermitRequirement>().Create();
            var expected = "notes";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEnvironmentalPermitRequirement, EnvironmentalPermitRequirement>(eq, new {
                Notes = expected
            })) as RedirectToRouteResult;

            Assert.AreEqual(expected, Session.Get<EnvironmentalPermitRequirement>(eq.Id).Notes);
        }
    }
}