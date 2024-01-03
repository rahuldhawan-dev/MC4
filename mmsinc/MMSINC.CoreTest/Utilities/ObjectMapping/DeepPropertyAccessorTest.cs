using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Utilities.ObjectMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Utilities.ObjectMapping
{
    [TestClass]
    public class DeepPropertyAccessorTest
    {
        #region Fields

        private DeepPropertyAccessor _target;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new DeepPropertyAccessor(typeof(RootType), "Child.Property");
        }

        [TestMethod]
        public void TestPropertyTypeReturnsTypeOfFinalPropertyAccessor()
        {
            Assert.AreSame(typeof(string), _target.PropertyType);
        }

        [TestMethod]
        public void TestCanGetValue()
        {
            var instance = new RootType {
                Child = new ChildType {
                    Property = "Neat"
                }
            };

            Assert.AreEqual(instance.Child.Property, _target.GetValue(instance));
        }

        [TestMethod]
        public void TestGetValueReturnsNullIfAnyIntermediatePropertyIsNull()
        {
            var instance = new RootType();
            Assert.IsNull(_target.GetValue(instance));
        }

        [TestMethod]
        public void TestCanSetValue()
        {
            var instance = new RootType {
                Child = new ChildType()
            };

            _target.SetValue(instance, "Neat");
            Assert.AreEqual("Neat", instance.Child.Property);
        }

        #endregion

        #region Models

        private class RootType
        {
            public ChildType Child { get; set; }
        }

        private class ChildType
        {
            public string Property { get; set; }
        }

        #endregion
    }
}
