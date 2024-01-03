using System;
using System.Collections.Generic;
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
    public class MonthlyServicesInstalledByCategoryControllerTest : MapCallMvcControllerTestBase<MonthlyServicesInstalledByCategoryController, Service, ServiceRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IImageToPdfConverter>().Mock();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexDisplaysViewWhenNoResults = true;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = MonthlyServicesInstalledByCategoryController.ROLE;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/MonthlyServicesInstalledByCategory/Search", role);
                a.RequiresRole("~/Reports/MonthlyServicesInstalledByCategory/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because of view model but also ???????????????
            var operatingCenter1 =
                GetFactory<UniqueOperatingCenterFactory>()
                    .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var operatingCenter2 =
                GetFactory<UniqueOperatingCenterFactory>()
                    .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var serviceCategory1 =
                GetEntityFactory<ServiceCategory>().Create(new { Description = "Fire Service Retire Only" });
            var serviceCategory2 =
                GetEntityFactory<ServiceCategory>().Create(new { Description = "Fire Service Installation" });

            // the good
            var service1 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2016, 1, 1)
                        });
            var service2 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2016, 2, 2)
                        });
            var service21 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter2,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2016, 1, 1)
                        });
            var service22 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter2,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2016, 2, 2)
                        });
            // the bad
            var service3 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory1,
                            DateInstalled = new DateTime(2016, 1, 1)
                        });
            var service4 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2015, 1, 1)
                        });
            var service5 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2015, 1, 1)
                        });

            var search = new SearchMonthlyServicesInstalledByCategory { Year = 2016 };

            var result = _target.Index(search) as ViewResult;
            var resultModel = (IEnumerable<MonthlyServicesInstalledByCategoryReportViewModel>)result.Model;

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(4, resultModel.Count());
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Implement and test me");
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var operatingCenter1 =
                GetFactory<UniqueOperatingCenterFactory>()
                    .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var operatingCenter2 =
                GetFactory<UniqueOperatingCenterFactory>()
                    .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var serviceCategory1 =
                GetEntityFactory<ServiceCategory>().Create(new { Description = "Fire Service Retire Only" });
            var serviceCategory2 =
                GetEntityFactory<ServiceCategory>().Create(new { Description = "Fire Service Installation" });

            // the good
            var service1 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2016, 1, 1)
                        });
            var service2 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2016, 2, 2)
                        });
            var service21 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter2,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2016, 1, 1)
                        });
            var service22 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter2,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2016, 2, 2)
                        });
            // the bad
            var service3 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory1,
                            DateInstalled = new DateTime(2016, 1, 1)
                        });
            var service4 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2015, 1, 1)
                        });
            var service5 =
                GetEntityFactory<Service>()
                    .Create(
                        new
                        {
                            OperatingCenter = operatingCenter1,
                            ServiceCategory = serviceCategory2,
                            DateInstalled = new DateTime(2015, 1, 1)
                        });
            var search = new SearchMonthlyServicesInstalledByCategory { Year = 2016 };
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(operatingCenter1, "OperatingCenter");
                helper.AreEqual(operatingCenter1, "OperatingCenter", 1);
                //helper.AreEqual(entity0.StreetNumber, "123A");
                //helper.AreEqual(entity1.StreetNumber, "123B", 1);
            }

        }
    }
}