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

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class DevelopmentProjectControllerTest : MapCallMvcControllerTestBase<DevelopmentProjectController, DevelopmentProject>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = DevelopmentProjectController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/DevelopmentProject/Search/", role);
                a.RequiresRole("~/ProjectManagement/DevelopmentProject/Show/", role);
                a.RequiresRole("~/ProjectManagement/DevelopmentProject/Index/", role);
                a.RequiresRole("~/ProjectManagement/DevelopmentProject/New/", role, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/DevelopmentProject/Create/", role, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/DevelopmentProject/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/DevelopmentProject/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/DevelopmentProject/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<DevelopmentProject>().Create(new {ProjectDescription = "description 0"});
            var entity1 = GetEntityFactory<DevelopmentProject>().Create(new {ProjectDescription = "description 1"});
            var search = new SearchDevelopmentProject();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.ProjectDescription, "ProjectDescription");
                helper.AreEqual(entity1.ProjectDescription, "ProjectDescription", 1);
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateSendsNotification()
        {
            Assert.Inconclusive("Test me.");
        }
        
        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersForUserRoleInLookupData()
        {
            var application = GetEntityFactory<MapCall.Common.Model.Entities.Application>().Create(new { Id = RoleApplications.FieldServices });
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

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<DevelopmentProject>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditDevelopmentProject, DevelopmentProject>(eq, new {
                ProjectDescription = expected
            }));

            Assert.AreEqual(expected, Session.Get<DevelopmentProject>(eq.Id).ProjectDescription);
        }

        #endregion
    }
}
