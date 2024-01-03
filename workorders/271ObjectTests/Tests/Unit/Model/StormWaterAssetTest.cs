using System;
using MMSINC.Data.Linq;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    [TestClass]
    public class StormCatchTest : WorkOrdersTestClass<StormCatch>
    {
        #region Private Members

        private IRepository<StormCatch> _repository;
        private TestStormCatch _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void StormCatchTestInitialize()
        {
            _repository = new MockStormCatchRepository();
            _target = new TestStormCatchBuilder();
        }

        #endregion

        #region Private Methods

        protected override StormCatch GetValidObjectFromDatabase()
        {
            throw new NotImplementedException();
        }

        protected override void DeleteObject(StormCatch entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        [TestMethod]
        public void TestCreateNewStormCatch()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestIAssetAssetKeyReflectsStormCatchID()
        {
            const int expected = 9;
            var target = new StormCatch {
                StormCatchID = expected
            };
            Assert.AreEqual(expected, target.AssetKey);
        }

        [TestMethod]
        public void TestIAssetAssetIDReflectsAssetNumber()
        {
            const string expected = "foo";
            var target = new StormCatch {
                AssetNumber = expected
            };
            Assert.AreEqual(expected, target.AssetID);
        }

        [TestMethod]
        public void TestIAssetLatitudeReflectsCoordinateLatitude()
        {
            var expected = (double?)44.1234;
            var coordinate = new Coordinate
            {
                Latitude = expected.Value
            };
            var target = new StormCatch()
            {
                Coordinate = coordinate
            };
            Assert.AreEqual(expected, target.Coordinate.Latitude);
            Assert.AreEqual(expected, target.Latitude);
        }

        [TestMethod]
        public void TestIAssetLongitudeReflectsCoordinateLongitude()
        {
            var expected = (double?)74.1234;
            var coordinate = new Coordinate
            {
                Longitude = expected.Value
            };
            var target = new StormCatch()
            {
                Coordinate = coordinate
            };
            Assert.AreEqual(expected, target.Coordinate.Longitude);
            Assert.AreEqual(expected, target.Longitude);
        }
    }

    internal class TestStormCatchBuilder : TestDataBuilder<TestStormCatch>
    {
        #region Constants

        private struct DefaultValues
        {
            public const int STORM_CATCH_ID = 1;
        }

        #endregion

        #region Private Members

        private int? _stormCatchID = DefaultValues.STORM_CATCH_ID;
        private string _assetID, _assetNumber;

        #endregion

        #region Exposed Methods

        public override TestStormCatch Build()
        {
            var obj = new TestStormCatch();
            if (_stormCatchID!=null)
                obj.StormCatchID = _stormCatchID.Value;
            if (!String.IsNullOrEmpty(_assetNumber))
                obj.AssetNumber = _assetNumber;
            return obj;
        }

        public TestStormCatchBuilder WithAssetID(string assetID)
        {
            _assetID = assetID;
            return this;
        }

        public TestStormCatchBuilder WithAssetNumber(string assetNumber)
        {
            _assetNumber = assetNumber;
            return this;
        }

        #endregion
    }

    internal class TestStormCatch : StormCatch { }

    internal class MockStormCatchRepository : MockRepository<StormCatch> { }
}
