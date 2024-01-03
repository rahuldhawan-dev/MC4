using MMSINC.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class CheckBoxListAttributeTest
    {
        #region Tests

        [TestMethod]
        public void TestDefaultConstructorSetsTypeToMultiSelect()
        {
            var target = new CheckBoxListAttribute();
            Assert.AreEqual(SelectType.CheckBoxList, target.Type);
        }

        [TestMethod]
        public void TestOverloadConstructorSetsExpectedParameters()
        {
            var target = new CheckBoxListAttribute("controller key");
            Assert.AreEqual(SelectType.CheckBoxList, target.Type);
            Assert.AreEqual("controller key", target.ControllerViewDataKey);
        }

        [TestMethod]
        public void TestConstructorSetsControllerName()
        {
            var expected = "ControllerDoodad";
            var result = new CheckBoxListAttribute(expected, null).Controller;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsActionName()
        {
            var expected = "ActionDoodad";
            var result = new CheckBoxListAttribute(null, expected).Action;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsAreaName()
        {
            var expected = "AreaName";
            var result = new CheckBoxListAttribute(expected, null, null).Area;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsDependentsRequiredToAllByDefault()
        {
            var result = new CheckBoxListAttribute();
            Assert.AreEqual(DependentRequirement.All, result.DependentsRequired);
        }

        #endregion
    }
}
