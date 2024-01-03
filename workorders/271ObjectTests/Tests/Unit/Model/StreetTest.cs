using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for StreetTestTest
    /// </summary>
    [TestClass]
    public class StreetTest : WorkOrdersTestClass<Street>
    {
        #region Constants

        // Brighton Ave, Shark River Hills, Neptune, NJ
        private const short REFERENCE_STREET_ID = 7067;

        #endregion

        #region Private Methods

        protected override Street GetValidObject()
        {
            return StreetRepository.GetEntity(REFERENCE_STREET_ID);
        }

        protected override Street GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(Street entity)
        {
            StreetRepository.Delete(entity);
        }

        #endregion

        [TestMethod]
        public void TestToStringReflectsFullStreetName()
        {
            const string fullStName = "Elm St";
            var target = new Street {
                FullStName = fullStName
            };

            Assert.AreEqual(fullStName, target.ToString(),
                "Street#ToString() should reflect the FullStName property.");
        }

        [TestMethod]
        public void TestStreetID()
        {
            // may seem like a dumb test, but this was set at RecID
            // (and still is in the db table).  this test won't even
            // compile if that fix gets broken.
            
            const int expected = 1;
            var target = new Street {
                StreetID = expected
            };

            Assert.AreEqual(expected, target.StreetID);
        }
    }

    internal class TestStreetBuilder : TestDataBuilder<Street>
    {
        #region Private Members

        private string _streetName;
        private StreetPrefix _prefix;
        private StreetSuffix _suffix;

        #endregion

        #region Exposed Methods

        public override Street Build()
        {
            var street = new Street();
            if (_streetName != null)
            {
                street.StreetName = _streetName;
                street.Suffix = _suffix;
                street.Prefix = _prefix;
                street.FullStName = _streetName;
            }
            return street;
        }

        public TestStreetBuilder WithStreetName(string streetName)
        {
            _streetName = streetName;
            return this;
        }

        public TestStreetBuilder WithPrefix(string prefix)
        {
            _prefix = new StreetPrefix { Description = prefix };
            return this;
        }

        public TestStreetBuilder WithSuffix(string suffix)
        {
            _suffix = new StreetSuffix { Description = suffix };
            return this; 
        }

        #endregion
    }
}
