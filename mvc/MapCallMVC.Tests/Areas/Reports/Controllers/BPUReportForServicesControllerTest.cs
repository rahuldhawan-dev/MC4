using System;
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
    public class BPUReportForServicesControllerTest : MapCallMvcControllerTestBase<BPUReportForServicesController, Service, ServiceRepository>
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
            var role = BPUReportForServicesController.ROLE;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/BPUReportForServices/Search", role);
                a.RequiresRole("~/Reports/BPUReportForServices/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create(new
            {
                OperatingCenterCode = "NJ4",
                OperatingCenterName = "Lakewood"
            });
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create(new
            {
                OperatingCenterCode = "NJ7",
                OperatingCenterName = "Shrewsbury"
            });
            var serviceCategory1 = GetEntityFactory<ServiceCategory>().Create(new
            {
                Description = "Fire Service Retire Only"
            });

            var serviceSize = GetEntityFactory<ServiceSize>().Create();
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create();
            var categoryOfServiceGroups = GetEntityFactory<CategoryOfServiceGroup>().CreateList(3);

            var serviceType1 = GetEntityFactory<ServiceType>().Create(new { Description = "Foo", OperatingCenter = operatingCenter1, ServiceCategory = serviceCategory1, CategoryOfServiceGroup = categoryOfServiceGroups[0] });
            var serviceType2 = GetEntityFactory<ServiceType>().Create(new { Description = "Foo", OperatingCenter = operatingCenter2, ServiceCategory = serviceCategory1, CategoryOfServiceGroup = categoryOfServiceGroups[1] });

            Session.Flush();
            Session.Clear();

            // the good
            var service1 = GetEntityFactory<Service>().Create(new
            {
                OperatingCenter = operatingCenter1,
                ServiceCategory = serviceCategory1,
                DateInstalled = new DateTime(2016, 1, 1),
                ServiceMaterial = serviceMaterial,
                ServiceSize = serviceSize
            });
            var service2 = GetEntityFactory<Service>().Create(new
            {
                OperatingCenter = operatingCenter1,
                ServiceCategory = serviceCategory1,
                DateInstalled = new DateTime(2016, 2, 2),
                ServiceMaterial = serviceMaterial,
                ServiceSize = serviceSize
            });
            var service3 = GetEntityFactory<Service>().Create(new
            {
                OperatingCenter = operatingCenter1,
                ServiceCategory = serviceCategory1,
                DateInstalled = new DateTime(2016, 1, 1),
                ServiceMaterial = serviceMaterial,
                ServiceSize = serviceSize
            });
            var service4 = GetEntityFactory<Service>().Create(new
            {
                OperatingCenter = operatingCenter2,
                ServiceCategory = serviceCategory1,
                DateInstalled = new DateTime(2016, 2, 2),
                ServiceMaterial = serviceMaterial,
                ServiceSize = serviceSize
            });

            var search = new SearchBPUReportForServices { Year = 2016 };

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchBPUReportForServices)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreEqual(3, resultModel[0].InstalledNew);
            Assert.AreEqual(1, resultModel[1].Replaced);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create(new
            {
                OperatingCenterCode = "NJ4",
                OperatingCenterName = "Lakewood"
            });
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create(new
            {
                OperatingCenterCode = "NJ7",
                OperatingCenterName = "Shrewsbury"
            });
            var serviceCategory1 = GetEntityFactory<ServiceCategory>().Create(new
            {
                Description = "Fire Service Retire Only"
            });

            var serviceSize = GetEntityFactory<ServiceSize>().Create();
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create();
            var categoryOfServiceGroups = GetEntityFactory<CategoryOfServiceGroup>().CreateList(3);

            var serviceType1 = GetEntityFactory<ServiceType>().Create(new { Description = "Foo", OperatingCenter = operatingCenter1, ServiceCategory = serviceCategory1, CategoryOfServiceGroup = categoryOfServiceGroups[0] });
            var serviceType2 = GetEntityFactory<ServiceType>().Create(new { Description = "Foo", OperatingCenter = operatingCenter2, ServiceCategory = serviceCategory1, CategoryOfServiceGroup = categoryOfServiceGroups[1] });

            Session.Flush();
            Session.Clear();

            // the good
            var service1 = GetEntityFactory<Service>().Create(new
            {
                OperatingCenter = operatingCenter1,
                ServiceCategory = serviceCategory1,
                DateInstalled = new DateTime(2016, 1, 1),
                ServiceMaterial = serviceMaterial,
                ServiceSize = serviceSize
            });
            var service2 = GetEntityFactory<Service>().Create(new
            {
                OperatingCenter = operatingCenter1,
                ServiceCategory = serviceCategory1,
                DateInstalled = new DateTime(2016, 2, 2),
                ServiceMaterial = serviceMaterial,
                ServiceSize = serviceSize
            });
            var service3 = GetEntityFactory<Service>().Create(new
            {
                OperatingCenter = operatingCenter1,
                ServiceCategory = serviceCategory1,
                DateInstalled = new DateTime(2016, 1, 1),
                ServiceMaterial = serviceMaterial,
                ServiceSize = serviceSize
            });
            var service4 = GetEntityFactory<Service>().Create(new
            {
                OperatingCenter = operatingCenter2,
                ServiceCategory = serviceCategory1,
                DateInstalled = new DateTime(2016, 2, 2),
                ServiceMaterial = serviceMaterial,
                ServiceSize = serviceSize
            });

            var search = new SearchBPUReportForServices { Year = 2016 };
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(operatingCenter1, "OperatingCenter");
                helper.AreEqual(operatingCenter2, "OperatingCenter", 1);
                helper.AreEqual(3m,"InstalledNew");
                helper.AreEqual(0m, "Replaced");
                helper.AreEqual(0m, "InstalledNew", 1);
                helper.AreEqual(1m, "Replaced", 1);
            }

        }
    }
}