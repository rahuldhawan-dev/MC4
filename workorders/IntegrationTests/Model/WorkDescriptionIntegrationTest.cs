using System.Reflection;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests;
using _271ObjectTests.Tests.Unit.Model;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for WorkDescriptionIntegrationTest
    /// </summary>
    [TestClass]
    public class WorkDescriptionIntegrationTest : WorkOrdersTestClass<WorkDescription>
    {
        #region Constants

        private const short MIN_EXPECTED_COUNT = 164;
        public const short REFERENCE_INDEX = 69;
        private const string REFERENCE_VALUE = "VALVE REPAIR";

        #endregion

        #region Private Members

        private WorkDescription _target;

        #endregion

        #region Exposed Static Methods

        public static WorkDescription GetValidWorkDescription()
        {
            return WorkDescriptionRepository.GetEntity(REFERENCE_INDEX);
        }

        public static void DeleteWorkDescription(WorkDescription entity)
        {
            WorkDescriptionRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        protected override WorkDescription GetValidObject()
        {
            return new WorkDescription {
                Description = "Test",
                AssetType = AssetTypeTest.GetValidAssetType()
            };
        }

        protected override WorkDescription GetValidObjectFromDatabase()
        {
            return GetValidWorkDescription();
        }

        protected override void DeleteObject(WorkDescription entity)
        {
            DeleteWorkDescription(entity);
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkDescriptionIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
            _simulator = _simulator.SimulateRequest();
            // need to get the AssetType from the AssetTypeRepository,
            // unfortunately
            _target =
                new TestWorkDescriptionBuilder().WithAssetType(
                    AssetTypeRepository.Valve);
        }

        [TestCleanup]
        public void WorkDescriptionIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestStaticValues()
        {
            WorkDescription target;
            MyAssert.IsGreaterThanOrEqualTo(
                new WorkDescriptionRepository().Count(), MIN_EXPECTED_COUNT);

            target = WorkDescriptionRepository.GetEntity(REFERENCE_INDEX);

            Assert.AreEqual(REFERENCE_VALUE, target.Description);
            Assert.AreSame(AssetTypeRepository.Valve, target.AssetType);
        }

        [TestMethod]
        public void TestRepositorySelectByAssetType()
        {
            var assetType = AssetTypeTest.GetValidAssetType();
            var descriptions =
                WorkDescriptionRepository.SelectByAssetType(assetType);

            foreach (var target in descriptions)
                Assert.AreEqual(assetType, target.AssetType);
        }
    }

    internal class TestWorkDescriptionBuilder : TestDataBuilder<WorkDescription>
    {
        #region Constants

        private const string DEFAULT_DESCRIPTION = "Test";
        private const decimal DEFAULT_TIME_TO_COMPLETE = 1.5m;

	    #endregion

        #region Private Members

        private string _description = DEFAULT_DESCRIPTION;
        private AssetType _assetType = new TestAssetTypeBuilder<Valve>();
        private decimal _timeToComplete = DEFAULT_TIME_TO_COMPLETE;

        #endregion

        #region Private Methods

        private void SetTimeToComplete(WorkDescription desc)
        {
            var timeToComplete = desc.GetType().GetField("_timeToComplete",
                BindingFlags.Instance | BindingFlags.NonPublic);
            timeToComplete.SetValue(desc, _timeToComplete);
        }

        #endregion

        #region Exposed Methods

        public override WorkDescription Build()
        {
            var obj = new WorkDescription();
            if (_description != null)
                obj.Description = _description;
            if (_assetType != null)
                obj.AssetType = _assetType;
            SetTimeToComplete(obj);
            return obj;
        }

        public TestWorkDescriptionBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public TestWorkDescriptionBuilder WithAssetType(AssetType assetType)
        {
            _assetType = assetType;
            return this;
        }

        public TestWorkDescriptionBuilder WithTimeToComplete(decimal timeToComplete)
        {
            _timeToComplete = timeToComplete;
            return this;
        }

        #endregion
    }
}