using System;
using MMSINC.Data.Linq;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for SewerOpeningTest.
    /// </summary>
    [TestClass]
    public class SewerOpeningTest : WorkOrdersTestClass<SewerOpening>
    {
        #region Private Members

        private IRepository<SewerOpening> _repository;
        private TestSewerOpening _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void SewerOpeningTestInitialize()
        {
            _repository = new MockSewerOpeningRepository();
            _target = new TestSewerOpeningBuilder();
        }

        #endregion

        #region Private Methods

        protected override SewerOpening GetValidObjectFromDatabase()
        {
            throw new NotImplementedException();
        }

        protected override void DeleteObject(SewerOpening entity)
        {
            throw new NotImplementedException();
        }
        
        #endregion
        
        [TestMethod]
        public void TestCreateNewSewerOpening()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestIAssetAssetKeyReflectsSewerOpeningID()
        {
            const int expected = 1;
            var target = new SewerOpening {
                Id = expected
            };
            Assert.AreEqual(expected, target.AssetKey);
        }

        [TestMethod]
        public void TestIAssetAssetIDReflectsOpeningNumber()
        {
            const string expected = "1234";
            var target = new SewerOpening {
                OpeningNumber = expected
            };
            Assert.AreEqual(expected, target.AssetID);
        }

        [TestMethod]
        public void TestIAssetLatitudeReflectsCoordinateLatitude()
        {
            var expected = (double?)44.1234;
            var coordinate = new Coordinate {
                Latitude = expected.Value
            };
            var target = new SewerOpening {
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
            var target = new SewerOpening
            {
                Coordinate = coordinate
            };
            Assert.AreEqual(expected, target.Coordinate.Longitude);
            Assert.AreEqual(expected, target.Longitude);
        }
    }

    internal class TestSewerOpeningBuilder : TestDataBuilder<TestSewerOpening>
    {
        #region Constants

        private struct DefaultValues
        {
            public const int SEWER_OPENING_ID = 1;
        }

	    #endregion

        #region Private Members

        private int? _sewerOpeningID = DefaultValues.SEWER_OPENING_ID, _assetStatusID;
        private string _assetID, _openingNumber;

        #endregion
        
        #region Exposed Methods

        public override TestSewerOpening Build()
        {
            var obj = new TestSewerOpening();
            if (_sewerOpeningID!=null)
                obj.Id = _sewerOpeningID.Value;
            if (_openingNumber != null)
                obj.OpeningNumber = _openingNumber;
            else if (_assetID!=null)
                obj.OpeningNumber = _assetID;
            if (_assetStatusID.HasValue)
                obj.AssetStatusID = _assetStatusID;
            return obj;
        }

        public TestSewerOpeningBuilder WithAssetID(string assetID)
        {
            _assetID = assetID;
            return this;
        }

        public TestSewerOpeningBuilder WithAssetStatusID(int assetStatusID)
        {
            _assetStatusID = assetStatusID;
            return this;
        }

        #endregion
    }

    internal class TestSewerOpening : SewerOpening { }

    internal class MockSewerOpeningRepository : MockRepository<SewerOpening> { }
}
