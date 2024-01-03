using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class FacilityProcessStepControllerTest : MapCallMvcControllerTestBase<FacilityProcessStepController, FacilityProcessStep>
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
            _target = Request.CreateAndInitializeController<FacilityProcessStepController>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.ProductionFacilities;
            Authorization.Assert(a => {
                a.RequiresRole("~/Facilities/FacilityProcessStep/New/", module, RoleActions.Add);
                a.RequiresRole("~/Facilities/FacilityProcessStep/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Facilities/FacilityProcessStep/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/FacilityProcessStep/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/FacilityProcessStep/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/FacilityProcessStep/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Facilities/FacilityProcessStep/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Facilities/FacilityProcessStep/Destroy/", module, RoleActions.Delete);
            });
        }

        #region New
        
        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersInLookupData()
        {
            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _target.New(null);

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion

        #endregion
    }
}
