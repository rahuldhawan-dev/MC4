using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class RegulationTest
    {
        #region Tests

        [TestMethod]
        public void TestDescriptionReturnsTitle()
        {
            var target = new Regulation();
            target.Title = "Neat";
            Assert.AreEqual("Neat", target.Description);
        }

        [TestMethod]
        public void TestDescriptionReturnsTitleAndRegulationShortIfRegulationShortIsNotNull()
        {
            var target = new Regulation();
            target.Title = "Neat";
            target.RegulationShort = "Also Neat";
            Assert.AreEqual("Neat - Also Neat", target.Description);
        }

        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var target = new Regulation();
            target.Title = "Neat";
            Assert.AreEqual("Neat", target.ToString());
        }

        #endregion
    }
}
