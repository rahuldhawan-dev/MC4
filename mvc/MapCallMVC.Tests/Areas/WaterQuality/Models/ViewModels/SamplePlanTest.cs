using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.WaterQuality.Models.ViewModels
{
    [TestClass]
    public class CreateSamplePlanTest : MapCallMvcInMemoryDatabaseTestBase<SamplePlan>
    {
        #region Fields

        private ViewModelTester<CreateSamplePlan, SamplePlan> _vmTester;
        private CreateSamplePlan _viewModel;
        private SamplePlan _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateSamplePlan(_container);
            _entity = new SamplePlan();
            _vmTester = new ViewModelTester<CreateSamplePlan, SamplePlan>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Cws);
            _vmTester.CanMapBothWays(x => x.Ntnc);
            _vmTester.CanMapBothWays(x => x.MonitoringPeriodFrom);
            _vmTester.CanMapBothWays(x => x.MonitoringPeriodTo);
            _vmTester.CanMapBothWays(x => x.Standard);
            _vmTester.CanMapBothWays(x => x.Reduced);
            _vmTester.CanMapBothWays(x => x.MinimumSamplesRequired);
            _vmTester.CanMapBothWays(x => x.NameOfCertifiedLaboratory);
            _vmTester.CanMapBothWays(x => x.SameAsPreviousPeriod);
            _vmTester.CanMapBothWays(x => x.AllSamplesTier1);
            _vmTester.CanMapBothWays(x => x.Tier2Sites);
            _vmTester.CanMapBothWays(x => x.Tier3Sites);
            _vmTester.CanMapBothWays(x => x.Tier1SitesVerified);
            _vmTester.CanMapBothWays(x => x.LeadServiceLines);
            _vmTester.CanMapBothWays(x => x.LeadLinesVerified);
            _vmTester.CanMapBothWays(x => x.FiftyPercent);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PWSID);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ContactPerson);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MonitoringPeriodFrom);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MonitoringPeriodTo);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MinimumSamplesRequired);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.NameOfCertifiedLaboratory);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SameAsPreviousPeriod);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AllSamplesTier1);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Tier2Sites);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Tier3Sites);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Tier1SitesVerified);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LeadServiceLines);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LeadLinesVerified);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FiftyPercent);
        }

        #endregion
    }

    [TestClass]
    public class EditSamplePlanTest : MapCallMvcInMemoryDatabaseTestBase<SamplePlan>
    {
        #region Fields

        private ViewModelTester<EditSamplePlan, SamplePlan> _vmTester;
        private EditSamplePlan _viewModel;
        private SamplePlan _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditSamplePlan(_container);
            _entity = new SamplePlan();
            _vmTester = new ViewModelTester<EditSamplePlan, SamplePlan>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Cws);
            _vmTester.CanMapBothWays(x => x.Ntnc);
            _vmTester.CanMapBothWays(x => x.MonitoringPeriodFrom);
            _vmTester.CanMapBothWays(x => x.MonitoringPeriodTo);
            _vmTester.CanMapBothWays(x => x.Standard);
            _vmTester.CanMapBothWays(x => x.Reduced);
            _vmTester.CanMapBothWays(x => x.MinimumSamplesRequired);
            _vmTester.CanMapBothWays(x => x.NameOfCertifiedLaboratory);
            _vmTester.CanMapBothWays(x => x.SameAsPreviousPeriod);
            _vmTester.CanMapBothWays(x => x.AllSamplesTier1);
            _vmTester.CanMapBothWays(x => x.Tier2Sites);
            _vmTester.CanMapBothWays(x => x.Tier3Sites);
            _vmTester.CanMapBothWays(x => x.Tier1SitesVerified);
            _vmTester.CanMapBothWays(x => x.LeadServiceLines);
            _vmTester.CanMapBothWays(x => x.LeadLinesVerified);
            _vmTester.CanMapBothWays(x => x.FiftyPercent);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PWSID);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ContactPerson);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MonitoringPeriodFrom);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MonitoringPeriodTo);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.MinimumSamplesRequired);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.NameOfCertifiedLaboratory);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SameAsPreviousPeriod);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AllSamplesTier1);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Tier2Sites);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Tier3Sites);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Tier1SitesVerified);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LeadServiceLines);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LeadLinesVerified);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FiftyPercent);
        }

        #endregion
    }
}
