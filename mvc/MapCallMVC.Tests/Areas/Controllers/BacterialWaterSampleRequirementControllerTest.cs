using System;
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

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class BacterialWaterSampleRequirementControllerTest : MapCallMvcControllerTestBase<BacterialWaterSampleRequirementController, BacterialWaterSample, BacterialWaterSampleRepository>
    {
        #region Private Members

        private PublicWaterSupply _publicWaterSupply1, _publicWaterSupply2, _publicWaterSupply3, _publicWaterSupply4;
        private SampleSite _sampleSite1, _sampleSite2, _sampleSite3, _sampleSite4, _sampleSite5, _sampleSite6, _sampleSite7;
        private List<BacterialSampleType> _bacterialSampleTypes;

        #endregion

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexDisplaysViewWhenNoResults = true;
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.WaterQualityGeneral;
                a.RequiresRole("~/Reports/BacterialWaterSampleRequirement/Search", module, RoleActions.Read);
                a.RequiresRole("~/Reports/BacterialWaterSampleRequirement/Index", module, RoleActions.Read);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            SetupSampleData();

            var search = new SearchBacterialWaterSampleRequirement();
            _target.ControllerContext = new ControllerContext();

            var result = _target.Index(search) as ViewResult;
            var resultModel = (IEnumerable<BacterialWaterSampleRequirementReportViewModel>)result.Model;
            
            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(13, resultModel.Count());
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Implement and test me");
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            SetupSampleData();
            var search = new SearchBacterialWaterSampleRequirement();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(_publicWaterSupply1.ToString(), "PublicWaterSupply");
                helper.AreEqual(_publicWaterSupply1.ToString(), "PublicWaterSupply", 1);
            }
        }

        #region Init/Cleanup

        private void CreateBacterialWaterSample(SampleSite sampleSite, decimal cl2Free, decimal cl2Total, DateTime sampleDate, BacterialSampleType bacterialSampleType)
        {
            GetEntityFactory<BacterialWaterSample>().Create(new
            {
                SampleSite = sampleSite,
                Cl2Free = cl2Free,
                Cl2Total = cl2Total,
                SampleCollectionDTM = sampleDate,
                BacterialSampleType = bacterialSampleType
            });
        }

        private void SetupSampleData()
        {
            var type1 = GetFactory<RoutineBacterialSampleTypeFactory>().Create();
            var type2 = GetFactory<ProcessControlBacterialSampleTypeFactory>().Create();
            var type3 = GetFactory<NewMainBacterialSampleTypeFactory>().Create();

            _bacterialSampleTypes = new[] { type1, type2, type3 }.ToList();
            _publicWaterSupply1 = GetEntityFactory<PublicWaterSupply>().Create(new { Identifier = "1111", OperatingArea = "North", JanuaryRequiredBacterialWaterSamples = 10, MarchRequiredBacterialWaterSamples = 15, SeptemberRequiredBacterialWaterSamples = 20 });
            _publicWaterSupply2 = GetEntityFactory<PublicWaterSupply>().Create(new { Identifier = "1112", OperatingArea = "North", JanuaryRequiredBacterialWaterSamples = 15, AprilRequiredBacterialWaterSamples = 20, OctoberRequiredBacterialWaterSamples = 10 });
            _publicWaterSupply3 = GetEntityFactory<PublicWaterSupply>().Create(new { Identifier = "1113", OperatingArea = "North", JanuaryRequiredBacterialWaterSamples = 20, MayRequiredBacterialWaterSamples = 10, NovemberRequiredBacterialWaterSamples = 15 });
            _publicWaterSupply4 = GetEntityFactory<PublicWaterSupply>().Create(new { Identifier = "1221", OperatingArea = "South", JanuaryRequiredBacterialWaterSamples = 2, MayRequiredBacterialWaterSamples = 1, NovemberRequiredBacterialWaterSamples = 3 });
            _sampleSite1 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Little Silver BH", PublicWaterSupply = _publicWaterSupply1 });
            _sampleSite2 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Krauszer's", PublicWaterSupply = _publicWaterSupply2 });
            _sampleSite3 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Rumson BH", PublicWaterSupply = _publicWaterSupply3 });
            _sampleSite4 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "King James Nursery", PublicWaterSupply = _publicWaterSupply1 });
            _sampleSite5 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Amour Florist", PublicWaterSupply = _publicWaterSupply2 });
            _sampleSite6 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Dunkin Donuts", PublicWaterSupply = _publicWaterSupply3 });
            _sampleSite7 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Cone Zone", PublicWaterSupply = _publicWaterSupply1 });
            var sampleSites = new[] { _sampleSite1, _sampleSite2, _sampleSite3, _sampleSite4, _sampleSite5, _sampleSite6 };

            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 1, 1), _bacterialSampleTypes[0]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 1, 2), _bacterialSampleTypes[0]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 1, 3), _bacterialSampleTypes[0]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 2, 1), _bacterialSampleTypes[0]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 1, 1), _bacterialSampleTypes[1]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 1, 2), _bacterialSampleTypes[1]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 1, 3), _bacterialSampleTypes[2]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 2, 1), _bacterialSampleTypes[1]);

            CreateBacterialWaterSample(_sampleSite3, 0.01m, 0.02m, new DateTime(2014, 1, 3), _bacterialSampleTypes[2]);
            CreateBacterialWaterSample(_sampleSite5, 0.01m, 0.02m, new DateTime(2014, 2, 1), _bacterialSampleTypes[1]);
        }

        #endregion
    }
}