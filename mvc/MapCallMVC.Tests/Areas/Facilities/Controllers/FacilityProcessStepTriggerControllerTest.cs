using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class FacilityProcessStepTriggerControllerTest : MapCallMvcControllerTestBase<FacilityProcessStepTriggerController, FacilityProcessStepTrigger>
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
            _target = Request.CreateAndInitializeController<FacilityProcessStepTriggerController>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.DestroyRedirectsToRouteOnSuccessArgs = (id) => {
                var parentStep = Repository.Find(id)?.FacilityProcessStep.Id;
                return new { action = "Show", controller = "FacilityProcessStep", id = parentStep };
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
                a.RequiresRole("~/Facilities/FacilityProcessStepTrigger/New/", module, RoleActions.Add);
                a.RequiresRole("~/Facilities/FacilityProcessStepTrigger/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Facilities/FacilityProcessStepTrigger/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/FacilityProcessStepTrigger/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Facilities/FacilityProcessStepTrigger/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Facilities/FacilityProcessStepTrigger/Destroy/", module, RoleActions.Delete);
            });
        }

        #endregion
    }
}
