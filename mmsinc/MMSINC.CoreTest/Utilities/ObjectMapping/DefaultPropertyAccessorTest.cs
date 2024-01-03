using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Utilities.ObjectMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Utilities.ObjectMapping
{
    [TestClass]
    public class DefaultPropertyAccessorTest
    {
        #region Fields

        private DefaultPropertyAccessor _target;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            var prop = typeof(TestObject).GetProperty("PublicProperty");
            _target = new DefaultPropertyAccessor(prop);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetValueReturnsValueOnObject()
        {
            var object1 = new TestObject {PublicProperty = "Dingus"};
            var object2 = new TestObject {PublicProperty = new object()};

            Assert.AreEqual("Dingus", _target.GetValue(object1));
            Assert.AreSame(object2.PublicProperty, _target.GetValue(object2));
        }

        [TestMethod]
        public void TestSetValueSetsValueOnObject()
        {
            var object1 = new TestObject();
            _target.SetValue(object1, "blah");
            Assert.AreEqual("blah", object1.PublicProperty);

            _target.SetValue(object1, null);
            Assert.IsNull(object1.PublicProperty);
        }

        #endregion

        #region Models

        private class TestObject
        {
            public object PublicProperty { get; set; }
        }

        #endregion
    }
}
