using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class FacilityProcessStepTriggerActionControllerTest : MapCallMvcControllerTestBase<FacilityProcessStepTriggerActionController, FacilityProcessStepTriggerAction>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            var user = GetFactory<AdminUserFactory>().Create(new
            {
                DefaultOperatingCenter = GetFactory<OperatingCenterFactory>().Create()
            });

            Session.Save(user.UserType);

            return user;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = Request.CreateAndInitializeController<FacilityProcessStepTriggerActionController>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.DestroyRedirectsToRouteOnSuccessArgs = (id) => {
                var parentStep = Repository.Find(id)?.Trigger.Id;
                return new { controller = "FacilityProcessStepTrigger", action = "Show", id = parentStep };
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.ProductionFacilities;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Facilities/FacilityProcessStepTriggerAction/New/", module, RoleActions.Add);
                a.RequiresRole("~/Facilities/FacilityProcessStepTriggerAction/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Facilities/FacilityProcessStepTriggerAction/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/FacilityProcessStepTriggerAction/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Facilities/FacilityProcessStepTriggerAction/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Facilities/FacilityProcessStepTriggerAction/Destroy/", module, RoleActions.Delete);
            });
        }

        #endregion
    }
}
