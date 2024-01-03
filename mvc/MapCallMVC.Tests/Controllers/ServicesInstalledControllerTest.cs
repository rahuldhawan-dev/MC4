using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class ServicesInstalledControllerTest : MapCallMvcControllerTestBase<ServicesInstalledController, Service, ServiceRepository>
    {
        #region Constants

        private IOperatingCenterRepository _operatingCenterRepository;

        #endregion

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
            e.For<IImageToPdfConverter>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _operatingCenterRepository = _container.GetInstance<OperatingCenterRepository>();
        }

        #endregion
        
        #region Private Methods

        private void SetupServicesForIndex()
        {
            var opCntr = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var serviceCategory1 = GetFactory<ServiceCategoryFactory>().Create(new { Description = "A" });
            var serviceSize1 = GetFactory<ServiceSizeFactory>().Create(new { ServiceSizeDescription = "2" });
            var services = GetFactory<ServiceFactory>().CreateList(6, new
            {
                OperatingCenter = opCntr,
                Town = town,
                ServiceCategory = serviceCategory1,
                ServiceSize = serviceSize1
            });
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesAssets;
                a.RequiresRole("~/Reports/ServicesInstalled/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Reports/ServicesInstalled/Index/", module, RoleActions.Read);
            });
        }

        #endregion

        #region Search
        
        [TestMethod]
        public void TestSearchSetsLookupData()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();

            var result = _target.Search(new SearchServicesInstalled());
            var opData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];

            Assert.IsNotNull(opData);
            Assert.AreEqual(1, opData.Count());
            Assert.AreEqual(operatingCenter.ToString(), opData.First().Text);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var services = GetFactory<ServiceFactory>().CreateList(6);

            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;
            var search = new SearchServicesInstalled();

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(services[0].Id, "Id");
                helper.AreEqual(services[0].OperatingCenter.ToString(), "OperatingCenter");
                helper.AreEqual(services[0].ServiceNumber, "ServiceNumber");
                helper.AreEqual(services[1].Id, "Id", 1);
                helper.AreEqual(services[1].OperatingCenter.ToString(), "OperatingCenter", 1);
                helper.AreEqual(services[1].ServiceNumber, "ServiceNumber", 1);
            }
        }

        #endregion
    }
}
