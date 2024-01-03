using System.Collections.Generic;
using MapCall.Common.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Metadata
{
    [TestClass]
    public class GenericControllerNameAttributeTest
    {
        #region Tests

        [TestMethod]
        public void TestGetControllerNameReturnsTypeNameOfLastGenericTypeParameter()
        {
            var target = new GenericControllerNameAttribute();
            // This class doesn't actually care what type is passed to it, it just needs to be generic.
            // So we can pass in a dictionary instead of making a custom class for the test.
            var type = typeof(Dictionary<int, string>);
            var result = target.GetControllerName(type);
            Assert.AreEqual("String", result);
        }
        

        #endregion
    }
}
