using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for AssetTypeIntegrationTest
    /// </summary>
    [TestClass]
    public class AssetTypeIntegrationTest : WorkOrdersTestClass<AssetType>
    {
        #region Constants

        private const int MIN_EXPECTED_COUNT = 11;

        #endregion

        #region Exposed Static Methods

        public static AssetType GetValidAssetType()
        {
            return AssetTypeRepository.Valve;
        }

        public static void DeleteAssetType(AssetType entity)
        {
            AssetTypeRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        protected override AssetType GetValidObject()
        {
            return GetValidAssetType();
        }

        protected override AssetType GetValidObjectFromDatabase()
        {
            var obj = GetValidObject();
            AssetTypeRepository.Insert(obj);
            return obj;
        }

        protected override void DeleteObject(AssetType entity)
        {
            DeleteAssetType(entity);
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void AssetTypeIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void AssetTypeIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestCannotCreateNewAssetType()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new AssetType
                {
                    Description = "Test"
                };

                MyAssert.Throws(() => AssetTypeRepository.Insert(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotChangeAssetType()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.Description = "Test";

                MyAssert.Throws(() => AssetTypeRepository.Update(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotDeleteAssetType()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.Throws(() => AssetTypeRepository.Delete(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestStaticValues()
        {
            using (_simulator.SimulateRequest())
            {
                AssetType target, expected;
                Assert.AreEqual(MIN_EXPECTED_COUNT, new AssetTypeRepository().Count());

                target = AssetTypeRepository.Valve;
                expected =
                    AssetTypeRepository.GetEntity(AssetTypeRepository.Indices.VALVE);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(AssetTypeRepository.Descriptions.VALVE, target.Description);
                Assert.AreEqual(AssetTypeEnum.Valve, target.TypeEnum);

                target = AssetTypeRepository.Hydrant;
                expected =
                    AssetTypeRepository.GetEntity(AssetTypeRepository.Indices.HYDRANT);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(AssetTypeRepository.Descriptions.HYDRANT, target.Description);
                Assert.AreEqual(AssetTypeEnum.Hydrant, target.TypeEnum);

                target = AssetTypeRepository.Main;
                expected =
                    AssetTypeRepository.GetEntity(AssetTypeRepository.Indices.MAIN);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(AssetTypeRepository.Descriptions.MAIN, target.Description);
                Assert.AreEqual(AssetTypeEnum.Main, target.TypeEnum);

                target = AssetTypeRepository.Service;
                expected =
                    AssetTypeRepository.GetEntity(AssetTypeRepository.Indices.SERVICE);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(AssetTypeRepository.Descriptions.SERVICE, target.Description);
                Assert.AreEqual(AssetTypeEnum.Service, target.TypeEnum);

                target = AssetTypeRepository.SewerOpening;
                expected =
                    AssetTypeRepository.GetEntity(
                        AssetTypeRepository.Indices.SEWER_OPENING);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(AssetTypeRepository.Descriptions.SEWER_OPENING,
                    target.Description);
                Assert.AreEqual(AssetTypeEnum.SewerOpening, target.TypeEnum);

                target = AssetTypeRepository.StormCatch;
                expected =
                    AssetTypeRepository.GetEntity(
                        AssetTypeRepository.Indices.STORM_CATCH);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(AssetTypeRepository.Descriptions.STORM_CATCH,
                    target.Description);
                Assert.AreEqual(AssetTypeEnum.StormCatch, target.TypeEnum);

                target = AssetTypeRepository.SewerMain;
                expected =
                    AssetTypeRepository.GetEntity(
                        AssetTypeRepository.Indices.SEWER_MAIN);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(AssetTypeRepository.Descriptions.SEWER_MAIN,
                    target.Description);
                Assert.AreEqual(AssetTypeEnum.SewerMain, target.TypeEnum);

                target = AssetTypeRepository.SewerLateral;
                expected =
                    AssetTypeRepository.GetEntity(
                        AssetTypeRepository.Indices.SEWER_LATERAL);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(AssetTypeRepository.Descriptions.SEWER_LATERAL,
                    target.Description);
                Assert.AreEqual(AssetTypeEnum.SewerLateral, target.TypeEnum);

            }
        }
    }
}