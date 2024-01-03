using MMSINC.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class MultiSelectAttributeTest
    {
        #region Tests

        [TestMethod]
        public void TestDefaultConstructorSetsTypeToMultiSelect()
        {
            var target = new MultiSelectAttribute();
            Assert.AreEqual(SelectType.MultiSelect, target.Type);
        }

        [TestMethod]
        public void TestOverloadConstructorSetsExpectedParameters()
        {
            var target = new MultiSelectAttribute("controller key");
            Assert.AreEqual(SelectType.MultiSelect, target.Type);
            Assert.AreEqual("controller key", target.ControllerViewDataKey);
        }

        [TestMethod]
        public void TestConstructorSetsControllerName()
        {
            var expected = "ControllerDoodad";
            var result = new MultiSelectAttribute(expected, null).Controller;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsActionName()
        {
            var expected = "ActionDoodad";
            var result = new MultiSelectAttribute(null, expected).Action;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsAreaName()
        {
            var expected = "AreaName";
            var result = new MultiSelectAttribute(expected, null, null).Area;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsDependentsRequiredToAllByDefault()
        {
            var result = new MultiSelectAttribute();
            Assert.AreEqual(DependentRequirement.All, result.DependentsRequired);
        }

        #endregion
    }
}
