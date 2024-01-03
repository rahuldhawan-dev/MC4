using System;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for AssetTest
    /// </summary>
    [TestClass]
    public class AssetTest
    {
        [TestMethod]
        public void TestConstructorThrowsExceptionWhenAssetTypeIsValveAndNoValveSupplied()
        {
            var assetType = new TestAssetTypeBuilder<Valve>().Build();

            MyAssert.Throws(() => new Asset(assetType, (Valve)null),
                typeof(ArgumentException));
            MyAssert.Throws(() => new Asset(assetType, (int?)null, null, null, null, null, null),
                typeof(ArgumentException));
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionWhenAssetTypeIsHydrantAndNoHydrantSupplied()
        {
            var assetType = new TestAssetTypeBuilder<Hydrant>().Build();

            MyAssert.Throws(() => new Asset(assetType, (Hydrant)null),
                typeof(ArgumentException));
            MyAssert.Throws(() => new Asset(assetType, (int?)null, null, null, null, null, null),
                typeof(ArgumentException));
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionWhenAssetTypeIsSewerOpeningAndNoSewerOpeningSupplied()
        {
            var assetType = new TestAssetTypeBuilder<SewerOpening>().Build();

            MyAssert.Throws(() => new Asset(assetType, (SewerOpening)null),
                typeof(ArgumentException));
            MyAssert.Throws(() => new Asset(assetType, (int?)null, null, null, null, null, null),
                typeof(ArgumentException));
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionWhenAssetTypeIsStormCatchAndNoStormCatchSupplied()
        {
            var assetType = new TestAssetTypeBuilder<StormCatch>().Build();

            MyAssert.Throws(() => new Asset(assetType, (StormCatch)null),
                typeof(ArgumentException));
            MyAssert.Throws(() => new Asset(assetType, (int?)null, null, null, null, null, null),
                typeof(ArgumentException));
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionWhenAsseTypeIsEquipmentAndNoEquipmentSupplied()
        {
            var assetType = new TestAssetTypeBuilder<Equipment>().Build();
            MyAssert.Throws(() => new Asset(assetType, (Equipment)null),
                typeof(ArgumentException));
            MyAssert.Throws(() => new Asset(assetType, (int?)null, null, null, null, null, null),
                typeof(ArgumentException));
        }
    }

    internal class TestAssetBuilder : TestDataBuilder<Asset>
    {
        #region Private Members

        private Valve _valve;
        private Hydrant _hydrant;
        private SewerOpening _sewerOpening;

        #endregion

        #region Private Methods

        private void ResetAssetFields()
        {
            _valve = null;
            _hydrant = null;
            _sewerOpening = null;
        }

        #endregion

        #region Exposed Methods

        public override Asset Build()
        {
            Asset asset = null;
            
            if (_valve != null)
                asset = new Asset(new TestAssetTypeBuilder<Valve>(), _valve);
            else if (_hydrant != null)
                asset = new Asset(new TestAssetTypeBuilder<Hydrant>(), _hydrant);
            else if (_sewerOpening != null)
                asset = new Asset(new TestAssetTypeBuilder<SewerOpening>(), _sewerOpening);
            return asset;
        }

        public TestAssetBuilder WithValve(Valve valve)
        {
            _valve = valve;
            return this;
        }

        public TestAssetBuilder WithHydrant(Hydrant hydrant)
        {
            _hydrant = hydrant;
            return this;
        }

        public TestAssetBuilder WithSewerOpening(SewerOpening sewerOpening)
        {
            _sewerOpening = sewerOpening;
            return this;
        }

        #endregion
    }
}
