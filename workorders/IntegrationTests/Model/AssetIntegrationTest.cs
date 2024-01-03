using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Interface;
using MMSINC.Utilities.StructureMap;
using StructureMap;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for AssetIntegrationTest
    /// </summary>
    [TestClass]
    public class AssetIntegrationTest
    {
        #region Private Members

        private HttpSimulator _simulator;
        private IContainer _container;
        
        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void AssetTestInitialize()
        {
            _container = new Container();
            _simulator = new HttpSimulator();
            _simulator = _simulator.SimulateRequest();
            _container.Inject<IDataContext>(new WorkOrdersDataContext());
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public void AssetTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestGetAssetByTypeAndKey()
        {
            var assetType = AssetTypeRepository.Valve;
            var valve = ValveTest.GetValidValve();
            object assetKey = valve.ValveID;

            var expected = new Asset(assetType, valve);
            var actual = Asset.GetAssetByTypeAndKey(assetType, assetKey);

            Assert.AreEqual(expected.AssetType, actual.AssetType);
            Assert.AreEqual(expected.Valve, actual.Valve);
        }



        [TestMethod]
        public void TestAssetConstructedWithValveReflectsValvesProperties()
        {
            var assetType = AssetTypeRepository.Valve;
            var asset = ValveTest.GetValidValve();
            var target = new Asset(assetType, asset);

            Assert.AreEqual(asset, target.Valve);
            Assert.AreEqual(assetType, target.AssetType);
            Assert.AreEqual(asset.FullValveSuffix, target.AssetID);
            Assert.AreEqual(asset.ValveID, target.AssetKey);
            Assert.AreEqual(target.WorkOrders, asset.WorkOrders);
        }
    }
}
