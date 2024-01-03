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
    public class ServicesRenewedControllerTest : MapCallMvcControllerTestBase<ServicesRenewedController, Service, ServiceRepository>
    {
        #region Fields

        private IOperatingCenterRepository _operatingCenterRepository;
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
            var serviceCategory2 = GetFactory<ServiceCategoryFactory>().Create(new { Description = "B" });
            var serviceSize1 = GetFactory<ServiceSizeFactory>().Create(new { ServiceSizeDescription = "2" });
            var serviceSize2 = GetFactory<ServiceSizeFactory>().Create(new { ServiceSizeDescription = "15" });

            var services1 = GetFactory<ServiceFactory>().CreateList(6, new
            {
                OperatingCenter = opCntr,
                Town = town,
                ServiceCategory = serviceCategory1,
                ServiceSize = serviceSize1
            });
            var services2 = GetFactory<ServiceFactory>().CreateList(5, new
            {
                OperatingCenter = opCntr,
                Town = town,
                ServiceCategory = serviceCategory1,
                ServiceSize = serviceSize2
            });
            var services3 = GetFactory<ServiceFactory>().CreateList(4, new
            {
                OperatingCenter = opCntr,
                Town = town,
                ServiceCategory = serviceCategory2,
                ServiceSize = serviceSize1
            });
        }

        #endregion

        #region Tests

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.FieldServicesAssets;
                a.RequiresRole("~/Reports/ServicesRenewed/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Reports/ServicesRenewed/Index/", module, RoleActions.Read);
            });
        }

        #endregion

        #region Search

        [TestMethod]
        public void TestSearchSetsLookupData()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();

            var result = _target.Search();
            var operatingCenterData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];

            Assert.IsNotNull(operatingCenterData);
            Assert.AreEqual(1, operatingCenterData.Count());
            Assert.AreEqual(operatingCenter.ToString(), operatingCenterData.First().Text);
        }

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // override because of the tons of data needed for setup
            SetupServicesForIndex();
            var search = new SearchServicesRenewed();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((IEnumerable<AggregatedService>)result.Model).ToList();
            
            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(3, resultModel.Count);
            Assert.AreEqual(6, resultModel[0].ServiceCount);
            Assert.AreEqual(5, resultModel[1].ServiceCount);
            Assert.AreEqual(4, resultModel[2].ServiceCount);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            SetupServicesForIndex();

            //_target.ControllerContext = new FakeHttpContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            var search = new SearchServicesRenewed();

            var result = _target.Index(search) as PartialViewResult;
            var resultModel = ((IEnumerable<AggregatedService>)result.Model).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, resultModel.Count);
            Assert.AreEqual("_Index", result.ViewName);
        }

        #endregion

        #endregion
    }
}
