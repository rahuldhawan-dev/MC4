using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for SpoilStorageLocationTest.
    /// </summary>
    [TestClass]
    public class SpoilStorageLocationTest
    {
        #region Private Members

        private MockRepository<SpoilStorageLocation> _repository;
        private SpoilStorageLocation _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void SpoilStorageLocationTestInitialize()
        {
            _repository = new MockRepository<SpoilStorageLocation>();

            _target = new TestSpoilStorageLocationBuilder()
                .WithName("New Location")
                .WithOperatingCenter(new OperatingCenter());
        }

        #endregion

        [TestMethod]
        public void TestCreateNewSpoilStorageLocation()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutValueForName()
        {
            _target.Name = null;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));

            _target.Name = string.Empty;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSetNameToAValueLongerThan30Chars()
        {
            MyAssert.DoesNotThrow(
                () =>
                _target.Name =
                new string('x', SpoilStorageLocation.MAX_NAME_LENGTH));

            MyAssert.Throws<StringTooLongException>(
                () =>
                _target.Name =
                new string('x', SpoilStorageLocation.MAX_NAME_LENGTH + 1));
        }

        [TestMethod]
        public void TestCannotSaveWithoutOperatingCenter()
        {
            _target.OperatingCenter = null;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestToStringMethodReturnsNameValue()
        {
            Assert.AreEqual(_target.Name, _target.ToString());
        }
    }

    internal class TestSpoilStorageLocationBuilder : TestDataBuilder<SpoilStorageLocation>
    {
        #region Private Members

        private string _name;
        private OperatingCenter _operatingCenter;

        #endregion

        #region Exposed Methods

        public override SpoilStorageLocation Build()
        {
            var obj = new SpoilStorageLocation();
            if (_name != null)
                obj.Name = _name;
            if (_operatingCenter != null)
                obj.OperatingCenter = _operatingCenter;
            return obj;
        }

        public TestSpoilStorageLocationBuilder WithName(string s)
        {
            _name = s;
            return this;
        }

        public TestSpoilStorageLocationBuilder WithOperatingCenter(OperatingCenter center)
        {
            _operatingCenter = center;
            return this;
        }

        #endregion
    }
}
