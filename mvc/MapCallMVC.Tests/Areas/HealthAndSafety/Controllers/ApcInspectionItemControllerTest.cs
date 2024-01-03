using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Controllers
{
    [TestClass]
    public class ApcInspectionItemControllerTest : MapCallMvcControllerTestBase<ApcInspectionItemController, ApcInspectionItem>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = ApcInspectionItemController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/HealthAndSafety/ApcInspectionItem/Search/", role);
                a.RequiresRole("~/HealthAndSafety/ApcInspectionItem/Show/", role);
                a.RequiresRole("~/HealthAndSafety/ApcInspectionItem/Index/", role);
                a.RequiresRole("~/HealthAndSafety/ApcInspectionItem/New/", role, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/ApcInspectionItem/Create/", role, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/ApcInspectionItem/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/ApcInspectionItem/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/ApcInspectionItem/Destroy/", role, RoleActions.Delete);
			});
		}

        #endregion

        #region New

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

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ApcInspectionItem>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<ApcInspectionItem>().Create(new {Description = "description 1"});
            var search = new SearchApcInspectionItem();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<ApcInspectionItem>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditApcInspectionItem, ApcInspectionItem>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<ApcInspectionItem>(eq.Id).Description);
        }

        #endregion
    }
}
