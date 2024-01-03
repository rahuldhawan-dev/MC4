using System.Web.Mvc;
using LINQTo271.Views.Assets;
using MMSINC.Data.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Interface;
using MMSINC.Utilities.StructureMap;
using StructureMap;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Presenters.Assets;

namespace IntegrationTests.Views.Assets
{
    /// <summary>
    /// Summary description for AssetEntityEventArgsTest
    /// </summary>
    [TestClass]
    public class AssetEntityEventArgsTest
    {
        #region Private Members

        private HttpSimulator _simulator;
        private IContainer _container;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void AssetEntityEventArgsTestInitialize()
        {
            _simulator = new HttpSimulator();
            _container = new Container(i => {
                i.For<IRepository<AssetType>>().Use<MockAssetTypeRepository>();
                i.For<IRepository<Valve>>().Use<MockValveRepository>();
                i.For<IRepository<Hydrant>>().Use<MockHydrantRepository>();
            });
            _container.Inject<IDataContext>(new WorkOrdersDataContext());
            /* TODO: Uncomment these when Main and Service classes are created
            ConfiguredContainer.AddImplementorFor<IRepository<Main>>(
                typeof(MockMainRepository));
            ConfiguredContainer.AddImplementorFor<IRepository<Service>>(
                typeof(MockServiceRepository));
            */
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public void AssetEntityEventArgsTestCleanup()
        {
            _simulator.Dispose();
            /* TODO: Uncomment these when Main and Service classes are created
            ConfiguredContainer.RemoveImplementorOf<IRepository<Main>>();
            ConfiguredContainer.RemoveImplementorOf<IRepository<Service>>();
            */
        }

        #endregion

        [TestMethod]
        public void TestConstructorGetsValveFromRepositoryWhenAssetTypeIsValve()
        {
            using (_simulator.SimulateRequest())
            {
                const int assetTypeID = AssetTypeRepository.Indices.VALVE,
                          assetID = 39443; // first ID in the actual table
                var target = new AssetEntityEventArgs(assetTypeID, assetID);
                var expectedType = new MockAssetTypeRepository().Get(assetTypeID);
                var actualType = target.Entity.AssetType;
                var expectedAsset = new MockValveRepository().Get(assetID);
                var actaulAsset = target.Entity.InnerAsset;

                // have to use property values here because equality doesn't
                // work on these directly
                Assert.AreEqual(expectedType.TypeEnum, actualType.TypeEnum);
                Assert.AreEqual(expectedAsset.AssetKey, actaulAsset.AssetKey);
            }
        }

        [TestMethod]
        public void TestConstructorGetsHydrantFromRepositoryWhenAssetTypeIsHydrant()
        {
            using (_simulator.SimulateRequest())
            {
                const int assetTypeID = AssetTypeRepository.Indices.HYDRANT,
                          assetID = 222; // id from actual table
                var target = new AssetEntityEventArgs(assetTypeID, assetID);
                var expectedType = new MockAssetTypeRepository().Get(assetTypeID);
                var actualType = target.Entity.AssetType;
                var expectedAsset = new MockHydrantRepository().Get(assetID);
                var actaulAsset = target.Entity.InnerAsset;

                // have to use property values here because equality doesn't
                // work on these directly
                Assert.AreEqual(expectedType.TypeEnum, actualType.TypeEnum);
                Assert.AreEqual(expectedAsset.AssetKey, actaulAsset.AssetKey);
            }
        }

        [TestMethod]
        public void TestConstructorGetSewerOpeningFromRepositoryWhenAssetTypeIsSewerOpening()
        {
            using (_simulator.SimulateRequest())
            {
                const int assetTypeID =
                    AssetTypeRepository.Indices.SEWER_OPENING,
                          assetID = 3509; //actual table id
                var target = new AssetEntityEventArgs(assetTypeID, assetID);
                var expectedType = new MockAssetTypeRepository().Get(assetTypeID);
                var actualType = target.Entity.AssetType;
                var expectedAsset = new MockSewerOpeningRepository().Get(assetID);
                var actualAsset = target.Entity.InnerAsset;

                // have to use property values here because equality doesn't
                // work on these directly
                Assert.AreEqual(expectedType.TypeEnum, actualType.TypeEnum);
                Assert.AreEqual(expectedAsset.AssetKey, actualAsset.AssetKey);
            }
        }

        [TestMethod]
        public void TestConstructorPassesLatitudeAndLongitudeValuesToAsset()
        {
            using (_simulator.SimulateRequest())
            {
                const int assetTypeID = AssetTypeRepository.Indices.VALVE,
                          assetID = 39443; // first id in the actual table
                const double expectedLongitude = 1.1, expectedLatitude = 1.2;
                var target = new AssetEntityEventArgs(assetTypeID, assetID,
                                                      expectedLatitude, expectedLongitude);

                Assert.AreEqual(expectedLatitude,
                                target.Entity.InnerAsset.Latitude);
                Assert.AreEqual(expectedLongitude,
                                target.Entity.InnerAsset.Longitude);
            }
        }
    }
}