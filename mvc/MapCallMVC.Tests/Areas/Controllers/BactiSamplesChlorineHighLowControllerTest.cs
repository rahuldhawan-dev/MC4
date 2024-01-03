using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class BactiSamplesChlorineHighLowControllerTest : MapCallMvcControllerTestBase<BactiSamplesChlorineHighLowController, BacterialWaterSample>
    {
        #region Private Members

        private Town townAberdeen, townNeptune;
        private PublicWaterSupply publicWaterSupply;
        private SampleSite sampleSite1, sampleSite2, sampleSite3, sampleSite4, sampleSite5, sampleSite6, sampleSite7;
        private BacterialWaterSample bactiSampleAberdeen,
            bactiSampleAberdeenCl2FreeMax,
            bactiSampleAberdeenCl2FreeMin,
            bactiSampleAberdeenCl2TotalMax,
            bactiSampleAberdeenCl2TotalMin,
            bactiSampleNeptuneCl2FreeMinTotalMax,
            bactiSampleNeptuneCl2FreeMaxTotalMin;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);
            
            _container.Inject<IBacterialWaterSampleRepository>(_container
               .GetInstance<BacterialWaterSampleRepository>());
            SetupSampleData();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexDisplaysViewWhenNoResults = true;
        }

        private void SetupSampleData()
        {
            publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Identifier = "1111", OperatingArea = "North" });
            townAberdeen = GetEntityFactory<Town>().Create(new { ShortName = "Aberdeen" });
            townNeptune = GetEntityFactory<Town>().Create(new { ShortName = "Neptune" });
            sampleSite1 = GetEntityFactory<SampleSite>().Create(new { Town = townAberdeen, CommonSiteName = "Little Silver BH", PublicWaterSupply = publicWaterSupply });
            sampleSite2 = GetEntityFactory<SampleSite>().Create(new { Town = townAberdeen, CommonSiteName = "Krauszer's", PublicWaterSupply = publicWaterSupply });
            sampleSite3 = GetEntityFactory<SampleSite>().Create(new { Town = townAberdeen, CommonSiteName = "Rumson BH", PublicWaterSupply = publicWaterSupply });
            sampleSite4 = GetEntityFactory<SampleSite>().Create(new { Town = townAberdeen, CommonSiteName = "King James Nursery", PublicWaterSupply = publicWaterSupply });
            sampleSite5 = GetEntityFactory<SampleSite>().Create(new { Town = townAberdeen, CommonSiteName = "Amour Florist", PublicWaterSupply = publicWaterSupply });
            sampleSite6 = GetEntityFactory<SampleSite>().Create(new { Town = townNeptune, CommonSiteName = "Dunkin Donuts", PublicWaterSupply = publicWaterSupply });
            sampleSite7 = GetEntityFactory<SampleSite>().Create(new { Town = townNeptune, CommonSiteName = "Cone Zone", PublicWaterSupply = publicWaterSupply });

            bactiSampleAberdeen = GetEntityFactory<BacterialWaterSample>().Create(new
            {
                SampleSite = sampleSite1,
                Cl2Free = 0.16m,
                Cl2Total = 0.48m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 1)
            });
            bactiSampleAberdeenCl2FreeMax = GetEntityFactory<BacterialWaterSample>().Create(new
            {
                SampleSite = sampleSite2,
                Cl2Free = 0.27m,
                Cl2Total = 1.26m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 1)
            });
            bactiSampleAberdeenCl2FreeMin = GetEntityFactory<BacterialWaterSample>().Create(new
            {
                SampleSite = sampleSite3,
                Cl2Free = 0.14m,
                Cl2Total = 0.9m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 2)
            });
            bactiSampleAberdeenCl2TotalMax = GetEntityFactory<BacterialWaterSample>().Create(new
            {
                SampleSite = sampleSite4,
                Cl2Free = 0.14m,
                Cl2Total = 1.33m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 3)
            });
            bactiSampleAberdeenCl2TotalMin = GetEntityFactory<BacterialWaterSample>().Create(new
            {
                SampleSite = sampleSite5,
                Cl2Free = 0.21m,
                Cl2Total = 0.27m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 5)
            });
            bactiSampleNeptuneCl2FreeMinTotalMax = GetEntityFactory<BacterialWaterSample>().Create(new
            {
                SampleSite = sampleSite6,
                Cl2Free = 0.01m,
                Cl2Total = 0.88m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 8)
            });
            bactiSampleNeptuneCl2FreeMaxTotalMin = GetEntityFactory<BacterialWaterSample>().Create(new
            {
                SampleSite = sampleSite7,
                Cl2Free = 0.99m,
                Cl2Total = 0.02m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 13)
            });
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.WaterQualityGeneral;
                a.RequiresRole("~/Reports/BactiSamplesChlorineHighLow/Search", module, RoleActions.Read);
                a.RequiresRole("~/Reports/BactiSamplesChlorineHighLow/Index", module, RoleActions.Read);
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var search = new SearchBactiSamplesChlorineHighLow();

            var result = (ViewResult)_target.Index(search);
            var resultModel = (IEnumerable<BactiSamplesChlorineHighLowReportViewModel>)result.Model;
            
            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(8, resultModel.Count());
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Implement and test me");
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var search = new SearchBactiSamplesChlorineHighLow();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(publicWaterSupply.Identifier, "PWSID");
                helper.AreEqual(publicWaterSupply.Identifier, "PWSID", 1);
                helper.AreEqual("Cl2 Free Min", "Type");
                helper.AreEqual("Cl2 Free Max", "Type", 1);
                helper.AreEqual("Cl2 Total Min", "Type", 2);
                helper.AreEqual("Cl2 Total Max", "Type", 3);
            }
        }

        #endregion
    }
}