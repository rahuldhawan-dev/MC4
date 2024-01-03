using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class
        FacilityProcessControllerTest : MapCallMvcControllerTestBase<FacilityProcessController, FacilityProcess>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            var user = GetFactory<AdminUserFactory>().Create(new {
                DefaultOperatingCenter = GetFactory<OperatingCenterFactory>().Create()
            });

            Session.Save(user.UserType);

            return user;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = Request.CreateAndInitializeController<FacilityProcessController>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.ProductionFacilities;
            Authorization.Assert(a => {
                a.RequiresRole("~/Facilities/FacilityProcess/New/", module, RoleActions.Add);
                a.RequiresRole("~/Facilities/FacilityProcess/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Facilities/FacilityProcess/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/FacilityProcess/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/FacilityProcess/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/FacilityProcess/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Facilities/FacilityProcess/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Facilities/FacilityProcess/Destroy/", module, RoleActions.Delete);
                a.RequiresRole("~/Facilities/FacilityProcess/RemoveFacilityProcessStep/", module, RoleActions.Edit);
                a.RequiresRole("~/Facilities/FacilityProcess/ByFacilityId/", module, RoleActions.Read);
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

        [TestMethod]
        public void TestNewWithFacilityIdSetsFacilityAndOperatingCenterOnModel()
        {
            var expected = GetFactory<FacilityFactory>().Create();
            var result = (ViewResult)_target.New(expected.Id);
            var resultModel = (FacilityProcessViewModel)result.Model;
            Assert.AreEqual(expected.OperatingCenter.Id, resultModel.OperatingCenter.Value);
            Assert.AreEqual(expected.Id, resultModel.Facility.Value);
        }

        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersInLookupData()
        {
            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _target.New();

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion

        #region Edit

        [TestMethod]
        public void TestEditSetsProcessStageDropDownData()
        {
            // This is flakey since ProcessFactory creates a ProcessStage.
            var entity = GetFactory<FacilityProcessFactory>().Create();
            var expected = entity.Process.ProcessStage;
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
