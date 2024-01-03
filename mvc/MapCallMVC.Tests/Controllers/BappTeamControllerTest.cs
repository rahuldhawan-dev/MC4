using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class BappTeamControllerTest : MapCallMvcControllerTestBase<BappTeamController, BappTeam, BappTeamRepository>
    {
        #region Fields

        private User _user;
        private OperatingCenter _opCenter;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IBappTeamRepository>().Use<BappTeamRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _opCenter = GetFactory<OperatingCenterFactory>().Create();
            _user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = _opCenter, FullName = "Full Name" });
            _authenticationService.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<BappTeam>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditBappTeam, BappTeam>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<BappTeam>(eq.Id).Description);
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/BappTeam/Index/", BappTeamController.ROLE);
                a.RequiresRole("~/BappTeam/Show/", BappTeamController.ROLE);
                a.RequiresRole("~/BappTeam/ByOperatingCenterId/", BappTeamController.ROLE);
                a.RequiresRole("~/BappTeam/New/", BappTeamController.ROLE, RoleActions.Add);
                a.RequiresRole("~/BappTeam/Create/", BappTeamController.ROLE, RoleActions.Add);
                a.RequiresRole("~/BappTeam/Edit/", BappTeamController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/BappTeam/Update/", BappTeamController.ROLE, RoleActions.Edit);
            });
        }

        #endregion
    }
}
