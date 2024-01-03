using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Models.ViewModels
{
    [TestClass]
    public class CreateRoadwayImprovementNotificationTest : MapCallMvcInMemoryDatabaseTestBase<RoadwayImprovementNotification>
    {
        #region Fields

        private ViewModelTester<CreateRoadwayImprovementNotification, RoadwayImprovementNotification> _vmTester;
        private CreateRoadwayImprovementNotification _viewModel;
        private RoadwayImprovementNotification _entity;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateRoadwayImprovementNotification(_container);
            _entity = new RoadwayImprovementNotification();
            _vmTester = new ViewModelTester<CreateRoadwayImprovementNotification, RoadwayImprovementNotification>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.ExpectedProjectStartDate);
            _vmTester.CanMapBothWays(x => x.DateReceived);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RoadwayImprovementNotificationEntity);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ExpectedProjectStartDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DateReceived);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Coordinate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RoadwayImprovementNotificationStatus);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create(new {OperatingCenterCode = "NJ7"});
            _entity.OperatingCenter = opc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(opc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(opc, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestTownCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create(new { ShortName = "Foo" });
            _entity.Town = town;

            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();

            Assert.AreSame(town, _entity.Town);
        }

        [TestMethod]
        public void TestRoadwayImprovementNotificationEntityCanMapBothWays()
        {
            var rie = GetEntityFactory<RoadwayImprovementNotificationEntity>().Create(new { Description = "Foo" });
            _entity.RoadwayImprovementNotificationEntity = rie;

            _vmTester.MapToViewModel();

            Assert.AreEqual(rie.Id, _viewModel.RoadwayImprovementNotificationEntity);

            _entity.RoadwayImprovementNotificationEntity = null;
            _vmTester.MapToEntity();

            Assert.AreSame(rie, _entity.RoadwayImprovementNotificationEntity);
        }

        [TestMethod]
        public void TestRoadwayImprovementNotificationStatusCanMapBothWays()
        {
            var rins = GetEntityFactory<RoadwayImprovementNotificationStatus>().Create(new {Description = "Foo"});
            _entity.RoadwayImprovementNotificationStatus = rins;

            _vmTester.MapToViewModel();

            Assert.AreEqual(rins.Id, _viewModel.RoadwayImprovementNotificationStatus);

            _entity.RoadwayImprovementNotificationStatus = null;
            _vmTester.MapToEntity();

            Assert.AreSame(rins, _entity.RoadwayImprovementNotificationStatus);
        }

        #endregion
    }

    [TestClass]
    public class EditRoadwayImprovementNotificationTest : MapCallMvcInMemoryDatabaseTestBase<RoadwayImprovementNotification>
    {
        #region Fields

        private ViewModelTester<EditRoadwayImprovementNotification, RoadwayImprovementNotification> _vmTester;
        private EditRoadwayImprovementNotification _viewModel;
        private RoadwayImprovementNotification _entity;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditRoadwayImprovementNotification(_container);
            _entity = new RoadwayImprovementNotification();
            _vmTester = new ViewModelTester<EditRoadwayImprovementNotification, RoadwayImprovementNotification>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.ExpectedProjectStartDate);
            _vmTester.CanMapBothWays(x => x.DateReceived);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RoadwayImprovementNotificationEntity);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ExpectedProjectStartDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DateReceived);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Coordinate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RoadwayImprovementNotificationStatus);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create(new { OperatingCenterCode = "NJ7" });
            _entity.OperatingCenter = opc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(opc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(opc, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestTownCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create(new { ShortName = "Foo" });
            _entity.Town = town;

            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();

            Assert.AreSame(town, _entity.Town);
        }

        [TestMethod]
        public void TestRoadwayImprovementNotificationEntityCanMapBothWays()
        {
            var rie = GetEntityFactory<RoadwayImprovementNotificationEntity>().Create(new { Description = "Foo" });
            _entity.RoadwayImprovementNotificationEntity = rie;

            _vmTester.MapToViewModel();

            Assert.AreEqual(rie.Id, _viewModel.RoadwayImprovementNotificationEntity);

            _entity.RoadwayImprovementNotificationEntity = null;
            _vmTester.MapToEntity();

            Assert.AreSame(rie, _entity.RoadwayImprovementNotificationEntity);
        }

        [TestMethod]
        public void TestRoadwayImprovementNotificationStatusCanMapBothWays()
        {
            var rins = GetEntityFactory<RoadwayImprovementNotificationStatus>().Create(new { Description = "Foo" });
            _entity.RoadwayImprovementNotificationStatus = rins;

            _vmTester.MapToViewModel();

            Assert.AreEqual(rins.Id, _viewModel.RoadwayImprovementNotificationStatus);

            _entity.RoadwayImprovementNotificationStatus = null;
            _vmTester.MapToEntity();

            Assert.AreSame(rins, _entity.RoadwayImprovementNotificationStatus);
        }

        #endregion
    }
}
