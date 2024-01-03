using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.HumanResources.Controllers;
using MapCallMVC.Areas.HumanResources.Models.ViewModels;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Areas.HumanResources.Controllers
{
    [TestClass]
    public class EmployeeHeadCountControllerTest : MapCallMvcControllerTestBase<EmployeeHeadCountController, EmployeeHeadCount>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.HumanResourcesEmployee;

            Authorization.Assert(a => {
                a.RequiresRole("~/HumanResources/EmployeeHeadCount/Show/", role);
                a.RequiresRole("~/HumanResources/EmployeeHeadCount/Search/", role);
                a.RequiresRole("~/HumanResources/EmployeeHeadCount/Index/", role);
                a.RequiresRole("~/HumanResources/EmployeeHeadCount/Create/", role, RoleActions.Add);
                a.RequiresRole("~/HumanResources/EmployeeHeadCount/New/", role, RoleActions.Add);
                a.RequiresRole("~/HumanResources/EmployeeHeadCount/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/HumanResources/EmployeeHeadCount/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/HumanResources/EmployeeHeadCount/Destroy/", role, RoleActions.Delete);
            });
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<EmployeeHeadCount>().Create();
            var expected = "Some notes of sorts";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEmployeeHeadCount, EmployeeHeadCount>(eq, new {
                MiscNotes = expected
            }));

            Assert.AreEqual(expected, Session.Get<EmployeeHeadCount>(eq.Id).MiscNotes);
        }

        #endregion
    }
}
