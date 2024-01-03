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
    public class ProcessControllerTest : MapCallMvcControllerTestBase<ProcessController, Process>
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
            _target = Request.CreateAndInitializeController<ProcessController>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.ProductionFacilities;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Facilities/Process/New/", module, RoleActions.Add);
                a.RequiresRole("~/Facilities/Process/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Facilities/Process/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/Process/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/Process/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Facilities/Process/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Facilities/Process/ByProcessStage/", module, RoleActions.Read);
            });
        }

        #region New

        [TestMethod]
        public void TestNewSetsProcessStageDropDownData()
        {
            var expected = GetFactory<ProcessStageFactory>().Create();
            _target.New();
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["ProcessStage"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.Description, vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        #endregion

        #region Edit

        [TestMethod]
        public void TestEditSetsProcessStageDropDownData()
        {
            // This is flakey since ProcessFactory creates a ProcessStage.
            var entity = GetFactory<ProcessFactory>().Create();
            var expected = entity.ProcessStage;
            _target.Edit(entity.Id);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["ProcessStage"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.Description, vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        #endregion

        #endregion
    }
}
