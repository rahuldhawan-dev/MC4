using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for TownSectionTestTest
    /// </summary>
    [TestClass]
    public class TownSectionTest : WorkOrdersTestClass<TownSection>
    {
        #region Constants

        private const short REFERENCE_TOWN_SECTION_ID = 33;

        #endregion

        #region Private Methods

        protected override TownSection GetValidObject()
        {
            return TownSectionRepository.GetEntity(REFERENCE_TOWN_SECTION_ID);
        }

        protected override TownSection GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(TownSection entity)
        {
            TownSectionRepository.Delete(entity);
        }

        #endregion

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestToStringMethodReflectsNameProperty()
        {
            const string townSection = "Wrong Side of the Tracks";
            var target = new TownSection {
                Name = townSection
            };

            Assert.AreEqual(townSection, target.ToString(),
                "TownSection#ToString() should reflect the Name property.");
        }

        [TestMethod]
        public void TestTownSectionID()
        {
            // may seem like a dumb test, but this was set at RecID
            // (and still is in the db table).  this test won't even
            // compile if that fix gets broken.

            const int expected = 1;
            var target = new TownSection {
                TownSectionID = expected
            };

            Assert.AreEqual(expected, target.TownSectionID);
        }
    }
}
