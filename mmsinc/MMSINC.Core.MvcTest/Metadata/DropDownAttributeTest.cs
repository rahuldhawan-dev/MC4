using MMSINC.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class DropDownAttributeTest
    {
        #region Tests

        [TestMethod]
        public void TestDefaultConstructorSetsTypeToDropDown()
        {
            var target = new DropDownAttribute();
            Assert.AreEqual(SelectType.DropDown, target.Type);
        }

        [TestMethod]
        public void TestOverloadConstructorSetsExpectedParameters()
        {
            var target = new DropDownAttribute("controller key");
            Assert.AreEqual(SelectType.DropDown, target.Type);
            Assert.AreEqual("controller key", target.ControllerViewDataKey);
        }

        [TestMethod]
        public void TestConstructorSetsControllerName()
        {
            var expected = "ControllerDoodad";
            var result = new DropDownAttribute(expected, null).Controller;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsActionName()
        {
            var expected = "ActionDoodad";
            var result = new DropDownAttribute(null, expected).Action;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsAreaName()
        {
            var expected = "AreaName";
            var result = new DropDownAttribute(expected, null, null).Area;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsDependentsRequiredToAllByDefault()
        {
            var result = new DropDownAttribute();
            Assert.AreEqual(DependentRequirement.All, result.DependentsRequired);
        }

        #endregion
    }
}
