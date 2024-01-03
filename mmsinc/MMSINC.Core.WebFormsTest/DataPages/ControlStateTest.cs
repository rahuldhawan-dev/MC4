using System;
using MMSINC.DataPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.DataPages
{
    /// <summary>
    /// Summary description for FilterBuilderTest
    /// </summary>
    [TestClass]
    public class ControlStateTest
    {
        #region Enums

        public enum ControlStateTestEnum
        {
            Default = 0,
            TestValue = 43
        }

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void FilterBuilderTestInitialize() { }

        [TestCleanup]
        public void FilterBuilderTestCleanup() { }

        private static ControlState InitializeTarget()
        {
            return new ControlState();
        }

        #endregion

        #region Test Methods

        #region Constructors

        [TestMethod]
        public void TestEmptyConstructorDoesNotThrowException()
        {
            try
            {
                var cs = new ControlState();
            }
            catch (Exception)
            {
                Assert.Fail("Empty constructor can not throw exception.");
            }
        }

        [TestMethod]
        public void TestConstructorOverloadSetsExpectedValues()
        {
            var target = InitializeTarget();

            // Setup for the first key check.
            var firstExpectedKey = "this is a key";
            var firstExpectedValue = "a string";

            target.Add(firstExpectedKey, firstExpectedValue, null);

            // Gets the serializable control state object. 
            var controlStateObject = target.GetControlStateObject();

            var newTarget = new ControlState(controlStateObject);

            var result = newTarget.Get<string>(firstExpectedKey);

            Assert.IsNotNull(result, "Expected key not found.");
            Assert.AreEqual(firstExpectedValue, result);
        }

        #endregion

        #region Add method

        [TestMethod]
        public void TestAddMethodAddsValueWhenValueDoesNotEqualDefaultValue()
        {
            var target = InitializeTarget();

            var expectedKey = "key";
            var expectedValue = 24;
            var expectedDefaultValue = 0;

            target.Add(expectedKey, expectedValue, expectedDefaultValue);

            Assert.AreEqual(target.Get<int>(expectedKey), expectedValue);
        }

        [TestMethod]
        public void TestAddMethodDoesNotAddValueIfValueEqualsDefaultValue()
        {
            var target = InitializeTarget();

            var expectedKey = "key";
            var expectedValue = 24;
            var expectedDefaultValue = 24;

            target.Add(expectedKey, expectedValue, expectedDefaultValue);

            Assert.AreNotEqual(target.Get<int>(expectedKey), expectedValue);
        }

        #endregion

        #region GetControlStateObject method

        [TestMethod]
        public void TestGetControlStateObjectReturnsNullWhenItHasNoEntries()
        {
            var target = InitializeTarget();
            var controlStateObject = target.GetControlStateObject();

            Assert.IsNull(controlStateObject,
                "GetControlStateObject must return null if there are no entries in the ControlState object.");
        }

        [TestMethod]
        public void TestGetControlStateObjectConvertsEnumToInt()
        {
            var target = InitializeTarget();
            var expectedKey = "key";
            var expectedValue = (int)ControlStateTestEnum.TestValue;

            target.Add(expectedKey, ControlStateTestEnum.TestValue, ControlStateTestEnum.Default);

            // Reload to get the serialization-parsed values. 
            var controlStateObject = target.GetControlStateObject();
            target.LoadControlState(controlStateObject);

            var result = target.Get<object>(expectedKey);
            Assert.IsTrue(result.GetType() == typeof(int),
                "Enums added to ControlState must be converted to integers to reduce serialization size.");
        }

        #endregion

        #region LoadControlState method

        [TestMethod]
        public void TestLoadControlStateLoadsExpectedKeysAndValues()
        {
            var target = InitializeTarget();

            // Setup for the first key check.
            var firstExpectedKey = "this is a key";
            var firstExpectedValue = "a string";

            target.Add(firstExpectedKey, firstExpectedValue, null);

            // Gets the serializable control state object. 
            var controlStateObject = target.GetControlStateObject();

            target.LoadControlState(controlStateObject);

            var result = target.Get<string>(firstExpectedKey);

            Assert.IsNotNull(result, "Expected key not found.");
            Assert.AreEqual(firstExpectedValue, result);
        }

        #endregion

        #endregion
    }
}
