using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Admin.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCallMVC.Areas.Admin.Models.ViewModels.Users;
using MMSINC.Testing;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MapCallMVC.Areas.Admin.Models.ViewModels.RoleGroups;

namespace MapCallMVC.Tests.Areas.Admin.Controllers
{
    [TestClass]
    public class RoleGroupControllerTest : MapCallMvcControllerTestBase<RoleGroupController, RoleGroup>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {
                IsAdmin = true
            });
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresUserAdminUser("~/Admin/RoleGroup/Search/");
                a.RequiresUserAdminUser("~/Admin/RoleGroup/Index/");
                a.RequiresUserAdminUser("~/Admin/RoleGroup/Show/");

                a.RequiresSiteAdminUser("~/Admin/RoleGroup/New/");
                a.RequiresSiteAdminUser("~/Admin/RoleGroup/Create/");
                a.RequiresSiteAdminUser("~/Admin/RoleGroup/Edit/");
                a.RequiresSiteAdminUser("~/Admin/RoleGroup/Update/");
                a.RequiresSiteAdminUser("~/Admin/RoleGroup/Destroy/");
            });
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var roleGroup = GetEntityFactory<RoleGroup>().Create();
            var expected = "my new name";
            var vm = _viewModelFactory.Build<RoleGroupViewModel, RoleGroup>(roleGroup);
            vm.Name = expected;

            _target.Update(vm);

            Assert.AreEqual(expected, Session.Get<RoleGroup>(roleGroup.Id).Name);
        }

        #endregion
    }
}
