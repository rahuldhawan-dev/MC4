using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class PositionGroupControllerTest : MapCallMvcControllerTestBase<PositionGroupController, PositionGroup>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject(Repository);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresLoggedInUserOnly("~/PositionGroup/GetByCommonName");
                a.RequiresRole("~/PositionGroup/Index", RoleModules.HumanResourcesPositions);
                a.RequiresRole("~/PositionGroup/Search", RoleModules.HumanResourcesPositions);
                a.RequiresRole("~/PositionGroup/Show", RoleModules.HumanResourcesPositions);
                a.RequiresRole("~/PositionGroup/New", RoleModules.HumanResourcesPositions, RoleActions.Add);
                a.RequiresRole("~/PositionGroup/Create", RoleModules.HumanResourcesPositions, RoleActions.Add);
                a.RequiresRole("~/PositionGroup/Edit", RoleModules.HumanResourcesPositions, RoleActions.Edit);
                a.RequiresRole("~/PositionGroup/Update", RoleModules.HumanResourcesPositions, RoleActions.Edit);
                a.RequiresRole("~/PositionGroup/Destroy", RoleModules.HumanResourcesPositions, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexRespondsToExcel()
        {
            var pGroup = GetFactory<PositionGroupFactory>().Create(new{ State = typeof(StateFactory)});
            var search = new SearchPositionGroup();
            search.EnablePaging = true;

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;
            Assert.IsFalse(search.EnablePaging, "EnablePaging should be disabled always.");

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(pGroup.Id, "Id");
                helper.AreEqual(pGroup.Description, "Description");
                helper.AreEqual(pGroup.Group, "Group");
                helper.AreEqual(pGroup.PositionDescription, "PositionDescription");
                helper.AreEqual(pGroup.CommonName.Description, "CommonName");
                helper.AreEqual(pGroup.BusinessUnit, "BusinessUnit");
                helper.AreEqual(pGroup.BusinessUnitDescription, "BusinessUnitDescription");
                helper.AreEqual(pGroup.State.Abbreviation, "State");
                helper.AreEqual(pGroup.SAPCompanyCode.Description, "SAPCompanyCode");
            }
        }

        #endregion

        #endregion
    }
}
