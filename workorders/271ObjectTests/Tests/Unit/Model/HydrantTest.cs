using System;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for HydrantTestTest
    /// </summary>
    [TestClass]
    public class HydrantTest : WorkOrdersTestClass<Hydrant>
    {
        #region Constants

        // hydrant on Brighton Ave, Shark River Hills, Neptune, NJ
        private const int REFERENCE_HYDRANT_ID = 3877;

        #endregion

        #region Private Members

        protected override Hydrant GetValidObject()
        {
            return HydrantRepository.GetEntity(REFERENCE_HYDRANT_ID);
        }

        protected override Hydrant GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(Hydrant entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestFullHydrantSuffixPropertyReflectsHydrantNumberWithNoPrefix()
        {
            const string hydrantNumber = "HAB-1-ML";
            const string fullHydrantSuffix = "1-ML";
            var target = new Hydrant {
                HydrantNumber = hydrantNumber
            };

            Assert.AreNotEqual(target.HydrantNumber, target.FullHydrantSuffix);
            Assert.AreEqual(fullHydrantSuffix, target.FullHydrantSuffix);
        }

        [TestMethod]
        public void TestToStringMethodReflectsFullHydrantSufficProperty()
        {
            const string hydrantNumber = "HAB-1-ML";
            var target = new Hydrant {
                HydrantNumber = hydrantNumber
            };

            Assert.AreEqual(target.FullHydrantSuffix, target.ToString());
        }

        [TestMethod]
        public void TestIAssetAssetIDReflectsFullHydrantSuffix()
        {
            const string hydrantNumber = "HAB-1-ML";
            var target = new Hydrant {
                HydrantNumber = hydrantNumber
            };

            Assert.AreEqual(target.FullHydrantSuffix, target.AssetID);
        }

        [TestMethod]
        public void TestIAssetAssetKeyReflectsHydrantID()
        {
            const int hydrantID = 1;
            var target = new Hydrant {
                HydrantID = hydrantID
            };

            Assert.AreEqual(target.HydrantID, target.AssetKey);
        }

        [TestMethod]
        public void TestIAssetLatitudeReflectsCoordinateLatitude()
        {
            var expected = (double?)44.1234;
            var coordinate = new Coordinate
            {
                Latitude = expected.Value
            };
            var target = new Hydrant
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
            var target = new Hydrant
            {
                Coordinate = coordinate
            };
            Assert.AreEqual(expected, target.Coordinate.Longitude);
            Assert.AreEqual(expected, target.Longitude);
        }

        [TestMethod]
        public void TestFullHydrantSuffixProperty()
        {
            const string hydrantNumber = "H29WD-318";
            const string fullHydrantSuffix = "318";
            var target = new Hydrant { HydrantNumber = hydrantNumber };

            Assert.AreNotEqual(target.HydrantNumber, target.FullHydrantSuffix);
            Assert.AreEqual(fullHydrantSuffix, target.FullHydrantSuffix);
        }
    }

    internal class TestHydrantBuilder : TestDataBuilder<Hydrant>
    {
        #region Private Members

        private string _assetID;
        private int? _hydrantID;
        private int? _hydrantStatusID;

        #endregion

        #region Public Methods

        public override Hydrant Build()
        {
            var obj = new Hydrant();
            // because of how AssetID works
            if (_assetID != null)
                obj.HydrantNumber = "a-" + _assetID;
            if (_hydrantID != null)
                obj.HydrantID = _hydrantID.Value;
            if (_hydrantStatusID != null)
                obj.AssetStatusID = _hydrantStatusID.Value;
            return obj;
        }

        public TestHydrantBuilder WithAssetID(string assetID)
        {
            _assetID = assetID;
            return this;
        }

        public TestHydrantBuilder WithHydrantID(int? hydrantID)
        {
            _hydrantID = hydrantID;
            return this;
        }

        public TestHydrantBuilder WithHydrantStatusID(int? hydrantStatusID)
        {
            _hydrantStatusID = hydrantStatusID;
            return this;
        }

        #endregion
    }
}