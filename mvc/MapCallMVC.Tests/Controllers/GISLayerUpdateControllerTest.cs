using System;
using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class GISLayerUpdateControllerTest : MapCallMvcControllerTestBase<GISLayerUpdateController, GISLayerUpdate>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = GISLayerUpdateController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/GISLayerUpdate/Show/", role);
                a.RequiresRole("~/GISLayerUpdate/Index/", role);
                a.RequiresRole("~/GISLayerUpdate/New/", role, RoleActions.Add);
                a.RequiresRole("~/GISLayerUpdate/Create/", role, RoleActions.Add);
                a.RequiresRole("~/GISLayerUpdate/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/GISLayerUpdate/Update/", role, RoleActions.Edit);
			});
		}

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<GISLayerUpdate>().Create();
            var expected = DateTime.Today.AddDays(2);

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditGISLayerUpdate, GISLayerUpdate>(eq, new {
                Updated = expected
            }));

            Assert.AreEqual(expected, Session.Get<GISLayerUpdate>(eq.Id).Updated);
        }

        #endregion
	}
}
