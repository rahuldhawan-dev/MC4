using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class RecurringProjectListControllerTest : MapCallMvcControllerTestBase<RecurringProjectListController, RecurringProject, RecurringProjectRepository>
    {
        #region Private Members

        private User _user;

        #endregion
        
        #region Init/Cleanup

        protected override User CreateUser()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            return _user;
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        #endregion

        #region Tests

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesProjects;
                a.RequiresRole("~/Reports/RecurringProjectList/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Reports/RecurringProjectList/Index/", module, RoleActions.Read);
            });
        }

        #endregion

        #region Search
        
        [TestMethod]
        public void TestSearchSetsLookupData()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var ffp = GetFactory<FoundationalFilingPeriodFactory>().Create();
            var ap_approved = GetFactory<APApprovedRecurringProjectStatusFactory>().Create();
            var complete = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            var proposed = GetFactory<ProposedRecurringProjectStatusFactory>().Create();
            Session.Flush();

            var result = _target.Search(new SearchRecurringProjectList());
            var operatingCenterData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            var ffpData = (IEnumerable<SelectListItem>)_target.ViewData["FoundationalFilingPeriod"];
            var rpsData = (IEnumerable<SelectListItem>)_target.ViewData["Status"];

            Assert.IsNotNull(operatingCenterData);
            Assert.AreEqual(1, operatingCenterData.Count());
            Assert.AreEqual(opc.Id.ToString(), operatingCenterData.First().Value);
            Assert.AreEqual(1, ffpData.Count());
            Assert.AreEqual(ffp.Id.ToString(), ffpData.First().Value);
            Assert.AreEqual(3, rpsData.Count());
            Assert.AreEqual(ap_approved.Id.ToString(), rpsData.First().Value);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            var projects = GetFactory<RecurringProjectFactory>().CreateList(2);
            projects[1].Coordinate = GetFactory<CoordinateFactory>().Create();
            var search = new SearchRecurringProjectList();

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(projects[0].ProjectTitle, "ProjectTitle");
                //helper.AreEqual(projects[0].Coordinate.Latitude, "Latitude", CoordinateFactory.LATITUDE.ToString());
                //helper.AreEqual(projects[0].Coordinate.Longitude, "Longitude", CoordinateFactory.LONGITUDE.ToString());

                helper.AreEqual(projects[1].ProjectTitle, "ProjectTitle", 1);
                helper.AreEqual(projects[1].Coordinate.Latitude, "Latitude", 1);
                helper.AreEqual(projects[1].Coordinate.Longitude, "Longitude", 1);
            }
        }

        #endregion

        #endregion
    }
}
