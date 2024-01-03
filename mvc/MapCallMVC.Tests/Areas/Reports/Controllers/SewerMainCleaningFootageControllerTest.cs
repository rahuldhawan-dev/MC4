using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using System.Web.Mvc;
using System.Linq;
using MapCall.Common.Testing.Data;
using System.Collections.Generic;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class SewerMainCleaningFootageControllerTest : MapCallMvcControllerTestBase<
        SewerMainCleaningFootageController, SewerMainCleaning, SewerMainCleaningRepository>
    {
        #region Constants

        const RoleModules role = RoleModules.FieldServicesAssets;

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const string path = "~/Reports/SewerMainCleaningFootage/";
                a.RequiresRole(path + "Search", role);
                a.RequiresRole(path + "Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var today = Lambdas.GetNow();
            var town = GetFactory<TownFactory>().Create();
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var mainCleaningPMSewerMainInspectionType = GetFactory<MainCleaningPMSewerMainInspectionTypeFactory>().Create();
            var smc = GetFactory<SewerMainCleaningFactory>().Create(new {
                OperatingCenter = operatingCenter,
                Town = town,
                InspectionType = mainCleaningPMSewerMainInspectionType,
                today.Year,
                today.Month,
                FootageOfMainInspected = 250f
            });

            var search = new SearchSewerMainCleaningFootageReport { Year = today.Year };

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((IEnumerable<SewerMainCleaningFootageReport>)result.Model).ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count());
            Assert.AreEqual(smc.OperatingCenter?.OperatingCenterCode, resultModel[0].OperatingCenter);
            Assert.AreEqual(smc.FootageOfMainInspected, resultModel[0].Total);
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Index doesn't redirect to search");
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfThereAreZeroResults()
        {
            Assert.Inconclusive("Index doesn't redirect to search");
        }

        #endregion
    }
}