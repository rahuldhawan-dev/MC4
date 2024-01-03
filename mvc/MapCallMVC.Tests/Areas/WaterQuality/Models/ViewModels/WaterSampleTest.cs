using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.WaterQuality.Models.ViewModels
{
    [TestClass]
    public class CreateWaterSampleTest : MapCallMvcInMemoryDatabaseTestBase<WaterSample>
    {
        #region Fields

        private ViewModelTester<CreateWaterSample, WaterSample> _vmTester;
        private CreateWaterSample _viewModel;
        private WaterSample _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _dateTimeProvider = new Mock<IDateTimeProvider>();

            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);

            _viewModel = new CreateWaterSample(_container);
            _entity = new WaterSample { AnalysisPerformedBy = "Jeanne Poisson" };
            _vmTester = new ViewModelTester<CreateWaterSample, WaterSample>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.SampleDate);
            _vmTester.CanMapBothWays(x => x.CollectedBy);
            _vmTester.CanMapBothWays(x => x.AnalysisPerformedBy);
            _vmTester.CanMapBothWays(x => x.SampleValue);
            _vmTester.CanMapBothWays(x => x.Notes);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SampleDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SampleIdMatrix);
        }

        [TestMethod]
        public void TestSetDefaultsSetsCollectedBy()
        {
            _viewModel.SetDefaults();

            Assert.AreEqual(_user.UserName, _viewModel.CollectedBy);
        }

        [TestMethod]
        public void TestSetDefaultsSetsValuesForSampleSite()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var sampleSite = GetEntityFactory<SampleSite>().Create(new { OperatingCenter = operatingCenter });
            var sampleIdMatrix = GetEntityFactory<SampleIdMatrix>().Create(new {SampleSite = sampleSite});
            Session.Evict(sampleSite);
            sampleSite = Session.Load<SampleSite>(sampleSite.Id);

            _viewModel.SampleSite = sampleSite.Id;

            _viewModel.SetDefaults();

            Assert.AreEqual(operatingCenter.Id, _viewModel.OperatingCenter);
            Assert.AreEqual(sampleSite.Id, _viewModel.SampleSite);
            Assert.AreEqual(sampleIdMatrix.Id, _viewModel.SampleIdMatrix);
        }

        [TestMethod]
        public void TestSetDefaultsSetsValuesForSampleSiteWithMultipleSampleIdMatrices()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var sampleSite = GetEntityFactory<SampleSite>().Create(new { OperatingCenter = operatingCenter });
            var sampleIdMatrix = GetEntityFactory<SampleIdMatrix>().CreateList(2, new { SampleSite = sampleSite });
            Session.Evict(sampleSite);
            sampleSite = Session.Load<SampleSite>(sampleSite.Id);

            _viewModel.SampleSite = sampleSite.Id;

            _viewModel.SetDefaults();

            Assert.AreEqual(operatingCenter.Id, _viewModel.OperatingCenter);
            Assert.AreEqual(sampleSite.Id, _viewModel.SampleSite);
            Assert.IsNull(_viewModel.SampleIdMatrix);
        }

        #endregion
    }

    [TestClass]
    public class EditWaterSampleTest : MapCallMvcInMemoryDatabaseTestBase<WaterSample>
    {
        #region Fields

        private ViewModelTester<EditWaterSample, WaterSample> _vmTester;
        private EditWaterSample _viewModel;
        private WaterSample _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditWaterSample(_container);
            _entity = new WaterSample();
            _vmTester = new ViewModelTester<EditWaterSample, WaterSample>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.SampleDate);
            _vmTester.CanMapBothWays(x => x.CollectedBy);
            _vmTester.CanMapBothWays(x => x.AnalysisPerformedBy);
            _vmTester.CanMapBothWays(x => x.SampleValue);
            _vmTester.CanMapBothWays(x => x.Notes);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SampleDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SampleIdMatrix);
        }

        #endregion
    }
}
