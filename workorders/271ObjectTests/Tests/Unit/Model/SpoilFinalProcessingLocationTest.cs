using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for SpoilFinalProcessingLocationTest.
    /// </summary>
    [TestClass]
    public class SpoilFinalProcessingLocationTest
    {
        #region Private Members

        private MockRepository<SpoilFinalProcessingLocation> _repository;
        private SpoilFinalProcessingLocation _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void SpoilFinalProcessingLocationTestInitialize()
        {
            _repository = new MockRepository<SpoilFinalProcessingLocation>();
            _target = new TestSpoilFinalProcessingLocationBuilder()
                .WithName("New Location")
                .WithOperatingCenter(new OperatingCenter());
         }

        #endregion

        [TestMethod]
        public void TestCreateNewSpoilFinalProcessingLocation()
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
                new string('x', SpoilFinalProcessingLocation.MAX_NAME_LENGTH));

            MyAssert.Throws<StringTooLongException>(
                () =>
                _target.Name =
                new string('x', SpoilFinalProcessingLocation.MAX_NAME_LENGTH + 1));
        }

        [TestMethod]
        public void TestCannotSaveWithoutOperatingCenter()
        {
            _target.OperatingCenter = null;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }
    }

    internal class TestSpoilFinalProcessingLocationBuilder : TestDataBuilder<SpoilFinalProcessingLocation>
    {
        #region Private Members

        private string _name;
        private OperatingCenter _operatingCenter;

        #endregion

        #region Exposed Methods

        public override SpoilFinalProcessingLocation Build()
        {
            var obj = new SpoilFinalProcessingLocation();
            if (_name != null)
                obj.Name = _name;
            if (_operatingCenter != null)
                obj.OperatingCenter = _operatingCenter;
            return obj;
        }

        public TestSpoilFinalProcessingLocationBuilder WithName(string s)
        {
            _name = s;
            return this;
        }

        public TestSpoilFinalProcessingLocationBuilder WithOperatingCenter(OperatingCenter center)
        {
            _operatingCenter = center;
            return this;
        }

        #endregion
    }
}
