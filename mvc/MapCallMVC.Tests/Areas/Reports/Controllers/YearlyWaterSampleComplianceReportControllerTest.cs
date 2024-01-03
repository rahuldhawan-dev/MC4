using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class YearlyWaterSampleComplianceReportControllerTest : MapCallMvcControllerTestBase<YearlyWaterSampleComplianceReportController, PublicWaterSupply, PublicWaterSupplyRepository>
    {
        #region Fields

        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.EnvironmentalGeneral;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/YearlyWaterSampleComplianceReport/Index", role);
                a.RequiresRole("~/Reports/YearlyWaterSampleComplianceReport/Search", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me");
        }

        #endregion
    }
}
