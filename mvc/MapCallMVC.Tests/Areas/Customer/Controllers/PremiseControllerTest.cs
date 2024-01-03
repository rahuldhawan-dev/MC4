using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Customer.Controllers;
using MapCallMVC.Areas.Customer.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Customer.Controllers
{
    [TestClass]
    public class PremiseControllerTest : MapCallMvcControllerTestBase<PremiseController, Premise>
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
            options.InitializeSearchTester = tester => {
                tester.IgnoredPropertyNames.Add("ServiceDistrict");
                tester.IgnoredPropertyNames.Add("AreaCode");
                tester.IgnoredPropertyNames.Add("RegionCode");
                tester.IgnoredPropertyNames.Add("RouteNumber");
                tester.TestPropertyValues.Add(
                    nameof(SearchPremise.StatusCode),
                    GetFactory<ActivePremiseStatusCodeFactory>().Create().Id);
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/Customer/Premise/Show", role, RoleActions.Read);
                a.RequiresRole("~/Customer/Premise/Search", role, RoleActions.Read);
                a.RequiresRole("~/Customer/Premise/Index", role, RoleActions.Read);
                a.RequiresLoggedInUserOnly("~/Customer/Premise/RegionCodesByOperatingCenterId");
                a.RequiresRole("~/Customer/Premise/Find", role, RoleActions.Read);
            });
        }

        #region Find

        [TestMethod]
        public void TestFindReturnsViewAndDropDownsWithOperatingCenter()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var regionCode = GetEntityFactory<RegionCode>()
               .Create(new { Description = "foo", SAPCode = "FOO" });
            var premise = GetEntityFactory<Premise>()
               .Create(new { OperatingCenter = operatingCenter, RegionCode = regionCode });

            var result = _target.Find(operatingCenter.Id) as PartialViewResult;

            Assert.IsNotNull(result);
            MvcAssert.IsViewNamed(result, "_Find");
            Assert.IsNotNull(result.ViewData["RegionCode"]);
            Assert.IsNotNull(result.ViewData["ServiceUtilityType"]);
        }

        [TestMethod]
        public void TestFindReturnsViewAndDropDownsWithoutOperatingCenter()
        {
            var regionCode = GetEntityFactory<RegionCode>()
               .Create(new { Description = "foo", SAPCode = "FOO" });

            var result = _target.Find(null) as PartialViewResult;

            Assert.IsNotNull(result);
            MvcAssert.IsViewNamed(result, "_Find");
            Assert.IsNotNull(result.ViewData["RegionCode"]);
            Assert.IsNotNull(result.ViewData["ServiceUtilityType"]);
        }

        [TestMethod]
        public void TestFindReturnsFullViewWhenRequested()
        {
            var regionCode = GetEntityFactory<RegionCode>()
               .Create(new { Description = "foo", SAPCode = "FOO" });

            var result = _target.Find(null, false) as ViewResult;

            Assert.IsNotNull(result);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var epaCode = GetFactory<EPACodeFactory>().Create();
            
            var consolidatedCustomerSideMaterials0 = GetEntityFactory<ConsolidatedCustomerSideMaterial>().Create(new {
                ConsolidatedEPACode = epaCode, CustomerSideEPACode = epaCode, CustomerSideExternalEPACode = epaCode
            });
           
            var entity0 = GetEntityFactory<Premise>().Create(new {});
            var entity1 = GetEntityFactory<Premise>().Create(new {
                //Create coordinate data for Long and Lat testing
                Coordinate = typeof(CoordinateFactory)
            });
            var customerSideMaterial = GetEntityFactory<ServiceMaterial>().Create(new {
                CustomerEPACode = epaCode
            });
            entity1.MostRecentService = new MostRecentlyInstalledService {
                Premise = entity1,
                CustomerSideMaterial = customerSideMaterial
            };
            var shortCycleCustomerMaterial = GetEntityFactory<ShortCycleCustomerMaterial>().Create(new {
                TechnicalInspectedOn = _now,
                CustomerSideMaterial = customerSideMaterial,
                Premise = entity1
            });
            entity1.ShortCycleCustomerMaterials = new List<ShortCycleCustomerMaterial> {
                shortCycleCustomerMaterial
            };

            var search = new SearchPremise{ OperatingCenter = entity0.OperatingCenter.Id }; 
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {    
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);

                //Longitude and Latitude can be null at times. Or at least some locations do not contain
                //Long/Lat coordinates.                
                helper.IsNull("Longitude");
                helper.IsNull("Latitude");
                
                //Test for long and lat values
                helper.AreEqual(entity1.Longitude, "Longitude", 1); 
                helper.AreEqual(entity1.Latitude, "Latitude", 1); 
                
                //Test for most recent service epa code
                helper.IsNull("MostRecentServiceCustomerMaterialEPACode");
                helper.AreEqual(entity1.MostRecentServiceCustomerMaterialEPACode, "MostRecentServiceCustomerMaterialEPACode", 1);
                
                //Test for most recent customer material epa code
                helper.IsNull("MostRecentCustomerMaterialEPACode");
                helper.AreEqual(entity1.MostRecentCustomerMaterialEPACode, "MostRecentCustomerMaterialEPACode", 1);
                
                helper.IsNull("ConsolidatedCustomerSideMaterial");
                helper.AreEqual(entity1.ConsolidatedCustomerSideMaterial, "ConsolidatedCustomerSideMaterial", 1);
            }
        }

        [TestMethod]
        public void TestShowShowsErrorMessageWhenServiceWithSampleSiteIsLinkedToAPremise()
        {
            var sampleSites = GetEntityFactory<SampleSite>().CreateList(1);
            var premise = GetEntityFactory<Premise>().Create(new {PremiseNumber = "123123123", SampleSites = sampleSites});
            var services = GetEntityFactory<Service>().CreateList(1, new { ServiceNumber = (long?)123, Premise = premise});
            premise.Services = services;

            var result = _target.Show(premise.Id) as ViewResult;

            Assert.AreEqual(PremiseController.SAMPLE_SITE_WARNING,
                ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        public void TestShowDoesNotShowsErrorMessageWhenServiceWithoutSampleSiteIsLinkedToAPremise()
        {
            var premise = GetEntityFactory<Premise>().Create(new { PremiseNumber = "123123123" });
            var services = GetEntityFactory<Service>().CreateList(1, new { ServiceNumber = (long?)123, Premise = premise });
            premise.Services = services;
            
            var result = _target.Show(premise.Id) as ViewResult;

            Assert.IsNull(result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]);
        }

        #endregion

        #region Show

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // noop: action only lets you view your own record. Covered in other tests.
        }

        #endregion

        #endregion
    }
}