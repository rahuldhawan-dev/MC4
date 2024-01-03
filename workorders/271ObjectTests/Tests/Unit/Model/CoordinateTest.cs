using MMSINC.Data.Linq;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for CoordinateTestTest
    /// </summary>
    [TestClass]
    public class CoordinateTest
    {
        #region Private Members

        private IRepository<Coordinate> _repository;
        private TestCoordinate _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void CoordinateTestInitialize()
        {
            _repository = new MockCoordinateRepository();
            _target = new TestCoordinateBuilder();
        }

        #endregion

        [TestMethod]
        public void TestCreateNewCoordinate()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithLatitudeOfZero()
        {
            _target = new TestCoordinateBuilder()
                .WithLatitude(0);

            MyAssert.Throws(() => _repository.InsertNewEntity(_target),
                typeof(DomainLogicException),
                "Attempting to save a Coordinate object with a Latitude value of 0 should throw an excecption.");
        }

        [TestMethod]
        public void TestCannotSaveWithLongitudeOfZero()
        {
            _target = new TestCoordinateBuilder()
                .WithLatitude(0);

            MyAssert.Throws(() => _repository.InsertNewEntity(_target),
                typeof(DomainLogicException),
                "Attempting to save a Coordinate object with a Longitude value of 0 should throw an excecption.");
        }
    }

    internal class TestCoordinateBuilder : TestDataBuilder<TestCoordinate>
    {
        #region Constants

        public struct DefaultCoordinates
        {
            public const double LATITUDE = 1,
                                 LONGITUDE = 1;
        }

        #endregion

        #region Private Members

        private double _latitude = DefaultCoordinates.LATITUDE,
                         _longitude = DefaultCoordinates.LONGITUDE;

        #endregion

        #region Exposed Methods

        public override TestCoordinate Build()
        {
            var obj = new TestCoordinate();
            if (_latitude != null)
                obj.Latitude = _latitude;
            if (_longitude != null)
                obj.Longitude = _longitude;
            return obj;
        }

        public TestCoordinateBuilder WithLatitude(double latitude)
        {
            _latitude = latitude;
            return this;
        }

        public TestCoordinateBuilder WithLongitude(double longitude)
        {
            _longitude = longitude;
            return this;
        }

        #endregion
    }

    internal class TestCoordinate : Coordinate
    {
    }

    internal class MockCoordinateRepository : MockRepository<Coordinate>
    {
    }
}
