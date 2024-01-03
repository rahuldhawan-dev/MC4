using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class PipeDiameterControllerTest : MapCallMvcControllerTestBase<PipeDiameterController, PipeDiameter>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = PipeDiameterController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/PipeDiameter/Show/", role);
                a.RequiresRole("~/PipeDiameter/Index/", role);
                a.RequiresRole("~/PipeDiameter/New/", role, RoleActions.Add);
                a.RequiresRole("~/PipeDiameter/Create/", role, RoleActions.Add);
                a.RequiresRole("~/PipeDiameter/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/PipeDiameter/Update/", role, RoleActions.Edit);
			});
		}

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<PipeDiameter>().Create();
            var expected = 1.5m ;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditPipeDiameter, PipeDiameter>(eq, new {
                Diameter = expected
            }));

            Assert.AreEqual(expected, Session.Get<PipeDiameter>(eq.Id).Diameter);
        }

        #endregion
	}
}
