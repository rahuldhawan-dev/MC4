using System;
using System.Collections.Generic;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINCTestImplementation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectExtensions = MMSINC.ClassExtensions.ObjectExtensions.ObjectExtensions;

namespace MMSINC.CoreTest.ClassExtensions
{
    /// <summary>
    /// Summary description for ObjectExtensionsTest
    /// </summary>
    [TestClass]
    public class ObjectExtensionsTest
    {
        #region Private Members

        private Employee _target,
                         _level2,
                         _level3;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void ObjectExtensionsTestInitialize()
        {
            SetupTestData();
        }

        #endregion

        #region Private Methods

        private void SetupTestData()
        {
            _level3 = new Employee {
                FirstName = "Big",
                LastName = "Cheese"
            };

            _level2 = new Employee {
                FirstName = "Middle",
                LastName = "Man",
                ReportsTo = _level3
            };

            _target = new Employee {
                FirstName = "Bottom",
                LastName = "Rung",
                ReportsTo = _level2
            };
        }

        #endregion

        #region AsDynamic Tests

        [TestMethod]
        public void TestAsDynamicThrowsArgumentNullExceptionForNullObject()
        {
            SomeDynamicClass target = null;
            MyAssert.Throws<ArgumentNullException>(() => target.AsDynamic());
        }

        [TestMethod]
        public void TestAsDynamicReturnsDynamicFormOfObject()
        {
            var target = new SomeDynamicClass();
            var expected = "Aw hell no!";
            target.StringProp = expected;

            var dynamo = target.AsDynamic();
            Assert.AreEqual(expected, dynamo.StringProp);
        }

        #endregion

        #region GetPropertyValueByName Tests

        [TestMethod]
        public void TestGetPropertyValueByNameReturnsNullWhenObjectIsNull()
        {
            Assert.IsNull(
                ObjectExtensions.GetPropertyValueByName(((object)null), "Anything, doesn't matter really."));
        }

        [TestMethod]
        public void TestGetFirstLevelProperty()
        {
            Assert.AreEqual(_target.FirstName,
                ObjectExtensions.GetPropertyValueByName(_target, "FirstName"),
                "Error getting regular table column property value by name.");

            Assert.AreEqual(_target.FullName,
                ObjectExtensions.GetPropertyValueByName(_target, "FullName"),
                "Error getting logical property value by name.");

            Assert.AreSame(_level2,
                ObjectExtensions.GetPropertyValueByName(_target, "ReportsTo"),
                "Error getting association property value by name.");
        }

        [TestMethod]
        public void TestGetSecondLevelProperty()
        {
            Assert.AreEqual(_level2.FirstName,
                ObjectExtensions.GetPropertyValueByName(_target, "ReportsTo.FirstName"),
                "Error getting second-level table column property by name.");

            Assert.AreEqual(_level2.FullName,
                ObjectExtensions.GetPropertyValueByName(_target, "ReportsTo.FullName"),
                "Error getting second-level logical property value by name.");

            Assert.AreSame(_level3,
                ObjectExtensions.GetPropertyValueByName(_target, "ReportsTo.ReportsTo"),
                "Error getting second-level association property value by name.");
        }

        [TestMethod]
        public void TestGetThirdLevelProperty()
        {
            Assert.AreEqual(_level3.FirstName,
                ObjectExtensions.GetPropertyValueByName(_target, "ReportsTo.ReportsTo.FirstName"),
                "Error getting third-level table column property by name.");

            Assert.AreEqual(_level3.FullName,
                ObjectExtensions.GetPropertyValueByName(_target, "ReportsTo.ReportsTo.FullName"),
                "Error getting third-level logical property by name.");
        }

        #endregion

        #region SetPropertyValueByName Tests

        [TestMethod]
        public void TestSetPropertyValueByNameSetsThePropertyWithTheGivenNameToTheGivenValue()
        {
            var target = new TestSetPropertyValueClass {
                SettableString = "original value"
            };
            var expected = "new value";

            target.SetPropertyValueByName("SettableString", expected);

            Assert.AreEqual(expected, target.SettableString);
        }

        [TestMethod]
        public void TestSetPropertyValueByNameSetsThePropertyWithTheGivenNameToNullIfTheGivenValueIsNull()
        {
            var target = new TestSetPropertyValueClass {
                SettableString = "original value"
            };

            target.SetPropertyValueByName("SettableString", null);

            Assert.IsNull(target.SettableString);
        }

        [TestMethod]
        public void TestSetPropertyValueByNameThrowsExceptionIfPropertyWithTheGivenNameDoesNotExist()
        {
            var target = new TestSetPropertyValueClass();

            MyAssert.Throws<PropertyNotFoundException>(
                () => target.SetPropertyValueByName("NonExistantString", "foo"));
        }

        [TestMethod]
        public void TestHasPublicSetterReturnsTrueIfSetterPropertyWithTheGivenNameExists()
        {
            var target = new TestSetPropertyValueClass();

            Assert.IsTrue(target.HasPublicSetter("SettableString"));
        }

        [TestMethod]
        public void TestHasPublicSetterReturnsFalseIfNoPropertyWithTheGivenNameExists()
        {
            var target = new TestSetPropertyValueClass();

            Assert.IsFalse(target.HasPublicSetter("NonExistantString"));
        }

        [TestMethod]
        public void TestHasPublicSetterReturnsFalsIfPropertyWithTheGivenNameExistsButHasNoPublicSetter()
        {
            var target = new TestSetPropertyValueClass();

            Assert.IsFalse(target.HasPublicSetter("ReadableString"));
        }

        [TestMethod]
        public void TestSetPublicPropertyValueByNameThrowsExceptionIfPropertyWithTheGivenNameHasNoPublicSetter()
        {
            var target = new TestSetPropertyValueClass("original value");

            Assert.AreEqual("original value", target.ReadableString);

            MyAssert.Throws<SetterNotFoundException>(() =>
                target.SetPublicPropertyValueByName("ReadableString",
                    "new value"));

            Assert.AreEqual("original value", target.ReadableString);
        }

        public class TestSetPropertyValueClass
        {
            public string SettableString { get; set; }
            public string ReadableString { get; protected set; }

            public TestSetPropertyValueClass() { }

            public TestSetPropertyValueClass(string readableString)
            {
                ReadableString = readableString;
            }
        }

        #endregion

        #region Dynamic class

        public class SomeDynamicClass
        {
            public string StringProp { get; set; }
        }

        #endregion
    }
}
