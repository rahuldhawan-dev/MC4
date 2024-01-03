using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class PersonnelAreaControllerTest : MapCallMvcControllerTestBase<PersonnelAreaController, PersonnelArea>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/PersonnelArea/Index", RoleModules.HumanResourcesEmployee, RoleActions.Read);
                a.RequiresRole("~/PersonnelArea/Search", RoleModules.HumanResourcesEmployee, RoleActions.Read);
                a.RequiresRole("~/PersonnelArea/Show", RoleModules.HumanResourcesEmployee, RoleActions.Read);
                a.RequiresRole("~/PersonnelArea/New", RoleModules.HumanResourcesEmployee, RoleActions.Add);
                a.RequiresRole("~/PersonnelArea/Create", RoleModules.HumanResourcesEmployee, RoleActions.Add);
                a.RequiresRole("~/PersonnelArea/Edit", RoleModules.HumanResourcesEmployee, RoleActions.Edit);
                a.RequiresRole("~/PersonnelArea/Update", RoleModules.HumanResourcesEmployee, RoleActions.Edit);
                a.RequiresRole("~/PersonnelArea/Destroy", RoleModules.HumanResourcesEmployee, RoleActions.Delete);
                a.RequiresLoggedInUserOnly("~/PersonnelArea/ByOperatingCenter");
            });
        }

        #region Edit

        [TestMethod]
        public void TestEditSetsOperatingCenterDropDownData()
        {
            var entity = GetFactory<PersonnelAreaFactory>().Create();
            var expected = entity.OperatingCenter;
            _target.Edit(entity.Id);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.Description, vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        #endregion

        #region ByOperatingCenter

        [TestMethod]
        public void TestByOperatingCenterReturnsResultsFilterByOperatingCenter()
        {
            var badPa = GetEntityFactory<PersonnelArea>().Create(new{PersonnelAreaId = 155});
            var goodPa = GetEntityFactory<PersonnelArea>().Create(new { OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create() });
            
            var result = (CascadingActionResult)_target.ByOperatingCenter(goodPa.OperatingCenter.Id);
            var resultData = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(1, resultData.Count());
            Assert.AreEqual(goodPa.Id, resultData.Single().Id);
        }

        #endregion

        #endregion
    }
}
