using MMSINC.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class ControllerNameAttributeTest
    {
        #region Tests

        [TestMethod]
        public void TestGetControllerNameRemovesWordControllerFromTypeNameIfTypeNameEndsWithWordController()
        {
            var target = new ControllerNameAttribute();
            var result = target.GetControllerName(typeof(ClassEndsWithController));
            Assert.AreEqual("ClassEndsWith", result);

            result = target.GetControllerName(typeof(ClassDoesNotEndWithControllerYouSee));
            Assert.AreEqual("ClassDoesNotEndWithControllerYouSee", result);
        }

        #endregion

        #region Test classes

        private class ClassEndsWithController { }

        private class ClassDoesNotEndWithControllerYouSee { }

        #endregion
    }
}
