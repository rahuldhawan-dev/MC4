using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class ServicesRetiredControllerTest : MapCallMvcControllerTestBase<ServicesRetiredController, Service, ServiceRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IImageToPdfConverter>().Mock();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesAssets;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/ServicesRetired/Search", role);
                a.RequiresRole("~/Reports/ServicesRetired/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var town = GetEntityFactory<Town>().Create();
            var serviceSize = GetEntityFactory<ServiceSize>().Create(new { ServiceSizeDescription = "3/5", Size = 0.75m });
            var serviceCategory1 = GetEntityFactory<ServiceCategory>().Create(new { Description = "Fire Service Retire Only" });
            var service1 = GetEntityFactory<Service>().Create(new { ServiceCategory = serviceCategory1, OperatingCenter = operatingCenter1, Town = town, PreviousServiceSize = serviceSize });
            var service2 = GetEntityFactory<Service>().Create(new { ServiceCategory = serviceCategory1, OperatingCenter = operatingCenter1, Town = town, PreviousServiceSize = serviceSize });

            var search = new SearchServicesRetired();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchServicesRetired)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);
            Assert.AreEqual(2, resultModel[0].Total);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var town = GetEntityFactory<Town>().Create();
            var serviceSize = GetEntityFactory<ServiceSize>().Create(new {ServiceSizeDescription = "3/5", Size = 0.75m });
            var serviceCategory1 = GetEntityFactory<ServiceCategory>().Create(new { Description = "Fire Service Retire Only" });
            var service1 = GetEntityFactory<Service>().Create(new { ServiceCategory = serviceCategory1, OperatingCenter = operatingCenter1, Town = town, PreviousServiceSize = serviceSize });
            var service2 = GetEntityFactory<Service>().Create(new { ServiceCategory = serviceCategory1, OperatingCenter = operatingCenter1, Town = town, PreviousServiceSize = serviceSize });

            var search = new SearchServicesRetired();

            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(operatingCenter1, "OperatingCenter");
                helper.AreEqual(2, "Total");
            }
        }
    }
}