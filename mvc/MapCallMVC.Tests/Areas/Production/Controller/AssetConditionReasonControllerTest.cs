using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class AssetConditionReasonControllerTest : MapCallMvcControllerTestBase<AssetConditionReasonController, AssetConditionReason>
    {
        #region Private Methods

        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.ExpectedEditViewName = "~/Areas/Production/Views/AssetConditionReason/Edit.cshtml";
            options.ExpectedNewViewName = "~/Areas/Production/Views/AssetConditionReason/New.cshtml";
            options.ExpectedShowViewName = "~/Areas/Production/Views/AssetConditionReason/Show.cshtml";
            options.InitializeCreateViewModel = (vm) => {
                var model = (AssetConditionReasonViewModel)vm;
                model.ConditionType = GetEntityFactory<ConditionType>().Create().Id;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (AssetConditionReasonViewModel)vm;
                model.ConditionType = GetEntityFactory<ConditionType>().Create().Id;
            };
        }

        #endregion

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionDataAdministration;
            Authorization.Assert(auth => {
                auth.RequiresRole("~/Production/AssetConditionReason/Show", role);
                auth.RequiresRole("~/Production/AssetConditionReason/Index", role);
                auth.RequiresRole("~/Production/AssetConditionReason/New", role, RoleActions.Add);
                auth.RequiresRole("~/Production/AssetConditionReason/Create", role, RoleActions.Add);
                auth.RequiresRole("~/Production/AssetConditionReason/Edit", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/AssetConditionReason/Update", role, RoleActions.Edit);
            });
        }

        #endregion
    }
}
