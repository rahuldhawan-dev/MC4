using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Controllers
{
    [TestClass]
    public class LargeServiceProjectControllerTest : MapCallMvcControllerTestBase<LargeServiceProjectController, LargeServiceProject>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = LargeServiceProjectController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/LargeServiceProject/Search/", role);
                a.RequiresRole("~/ProjectManagement/LargeServiceProject/Show/", role);
                a.RequiresRole("~/ProjectManagement/LargeServiceProject/Index/", role);
                a.RequiresRole("~/ProjectManagement/LargeServiceProject/New/", role, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/LargeServiceProject/Create/", role, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/LargeServiceProject/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/LargeServiceProject/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/LargeServiceProject/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<LargeServiceProject>().Create(new {ProjectTitle = "description 0"});
            var entity1 = GetEntityFactory<LargeServiceProject>().Create(new {ProjectTitle = "description 1"});
            var search = new SearchLargeServiceProject();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.ProjectTitle, "ProjectTitle");
                helper.AreEqual(entity1.ProjectTitle, "ProjectTitle", 1);
            }
        }

        #endregion
        
        #region New

        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersForUserRoleInLookupData()
        {
            var application = GetEntityFactory<Application>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetEntityFactory<Module>().Create(new { Id = RoleModules.FieldServicesProjects, Application = application });
            var addAction = GetEntityFactory<RoleAction>().Create(new { Id = RoleActions.Add });

            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });
            GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = addAction,
                User = _currentUser,
                OperatingCenter = activeOpc
            });
            GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = addAction,
                User = _currentUser,
                OperatingCenter = inactiveOpc
            });

            _target.New();

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<LargeServiceProject>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditLargeServiceProject, LargeServiceProject>(eq, new {
                ProjectTitle = expected
            }));

            Assert.AreEqual(expected, Session.Get<LargeServiceProject>(eq.Id).ProjectTitle);
        }

        #endregion
    }
}
