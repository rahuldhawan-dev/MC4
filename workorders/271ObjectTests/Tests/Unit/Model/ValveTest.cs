using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for ValveTestTest
    /// </summary>
    [TestClass]
    public class ValveTest : WorkOrdersTestClass<Valve>
    {
        #region Constants

        // valve on Brighton Ave, Shark River Hills, Neptune, NJ
        private const int REFERENCE_VALVE_ID = 53827;

        #endregion

        #region Private Methods

        protected override Valve GetValidObject()
        {
            return GetValidObjectFromDatabase();
        }

        protected override Valve GetValidObjectFromDatabase()
        {
            return GetValidValve();
        }

        protected override void DeleteObject(Valve entity)
        {
            DeleteValve(entity);
        }
        
        #endregion

        #region Exposed Static Methods

        public static Valve GetValidValve()
        {
            return ValveRepository.GetEntity(REFERENCE_VALVE_ID);
        }

        public static void DeleteValve(Valve entity)
        {
            throw new DomainLogicException("Cannot delete Valve objects in this context.");
        }

        #endregion

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestFullValveSuffixPropertyHasValveNumberWithFullSuffixAndNoPrefix()
        {
            const string valveNumber = "VST-105a";
            const string fullValveSuffix = "105a";
            var target = new Valve {
                ValveNumber = valveNumber
            };

            Assert.AreNotEqual(target.ValveNumber, target.FullValveSuffix);
            Assert.AreEqual(fullValveSuffix, target.FullValveSuffix);

            const string valveNumberAdditional = "VST2-105a";
            target = new Valve {
                ValveNumber = valveNumberAdditional
            };

            Assert.AreEqual(fullValveSuffix, target.FullValveSuffix);
        }

        [TestMethod]
        public void TestToStringMethodReflectsFullValveSufficProperty()
        {
            const string valveNumber = "VST-105a";
            var target = new Valve {
                ValveNumber = valveNumber
            };

            Assert.AreEqual(target.FullValveSuffix, target.ToString());
        }

        [TestMethod]
        public void TestIAssetAssetIDReflectsFullValveSuffix()
        {
            const string valveNumber = "VST-105a";
            var target = new Valve {
                ValveNumber = valveNumber
            };

            Assert.AreEqual(target.FullValveSuffix, target.AssetID);
        }

        [TestMethod]
        public void TestIAssetAssetKeyReflectsValveID()
        {
            const int valveID = 1;
            var target = new Valve {
                ValveID = valveID
            };

            Assert.AreEqual(target.ValveID, target.AssetKey);
        }

        [TestMethod]
        public void TestIAssetLatitudeReflectsCoordinateLatitude()
        {
            var expected = (double?)44.1234;
            var coordinate = new Coordinate
            {
                Latitude = expected.Value
            };
            var target = new Valve
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
            var target = new Valve
            {
                Coordinate = coordinate
            };
            Assert.AreEqual(expected, target.Coordinate.Longitude);
            Assert.AreEqual(expected, target.Longitude);
        }
    }

    internal class TestValveBuilder : TestDataBuilder<Valve>
    {
        #region Constants

        private struct DefaultValues
        {
            public const int VALVE_ID = 1;
        }

	    #endregion

        #region Private Members

        private int? _valveID = DefaultValues.VALVE_ID, _valveStatusID;
        private string _assetID, _valveNumber;
        private Coordinate _coordinate;

        #endregion

        #region Exposed Methods

        public override Valve Build()
        {
            var valve = new Valve();
            if (_valveID != null)
                valve.ValveID = _valveID.Value;
            if (_valveNumber != null)
                valve.ValveNumber = _valveNumber;
            // because AssetID is generated
            else if (_assetID != null)
                valve.ValveNumber = "a-" + _assetID;
            if (_valveStatusID != null)
                valve.AssetStatusID = _valveStatusID;
            if (_coordinate != null)
            {
                valve.Coordinate = _coordinate;
            }
            return valve;
        }

        public TestValveBuilder WithAssetID(string assetID)
        {
            _assetID = assetID;
            return this;
        }

        public TestValveBuilder WithValveID(int? valveID)
        {
            _valveID = valveID;
            return this;
        }

        public TestValveBuilder WithValveStatusID(int valveStatusID)
        {
            _valveStatusID = valveStatusID;
            return this;
        }

        public TestValveBuilder WithCoordinate(Coordinate coordinate)
        {
            _coordinate = coordinate;
            return this;
        }

        #endregion
    }
}
