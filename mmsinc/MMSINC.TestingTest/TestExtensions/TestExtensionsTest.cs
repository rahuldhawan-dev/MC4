using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.TestingTest.TestExtensions
{
    /// <summary>
    /// Summary description for TestExtensionsTest
    /// </summary>
    [TestClass]
    public class MyAssertThrowsTest
    {
        [TestMethod]
        public void TestTestFailsWhenNoExceptionThrown()
        {
            var testFailed = false;
            try
            {
                MyAssert.Throws(() => { });
            }
            catch (AssertFailedException)
            {
                testFailed = true;
            }

            Assert.IsTrue(testFailed, "Error with MyAssert.Throws(Action)");
        }

        [TestMethod]
        public void TestTestSucceedsWhenExceptionThrown()
        {
            var testFailed = false;
            try
            {
                MyAssert.Throws(() => { throw new Exception(); });
            }
            catch (AssertFailedException)
            {
                testFailed = true;
            }

            Assert.IsFalse(testFailed, "Error with MyAssert.Throws(Action)");
        }

        [TestMethod]
        public void TestBasicFailureMessage()
        {
            string actual = null;
            try
            {
                MyAssert.Throws(() => { });
            }
            catch (AssertFailedException e)
            {
                actual = e.Message;
            }

            Assert.IsNotNull(actual);
            Assert.AreEqual(MyAssert.BaseFailureMessages.EXCEPTION_NOT_THROWN, actual);
        }

        [TestMethod]
        public void TestUserProvidedFailureMessage()
        {
            var userMessage = "So there.";
            var expected = String.Format(
                "{0}  {1}", MyAssert.BaseFailureMessages.EXCEPTION_NOT_THROWN,
                userMessage);
            string actual = null;

            try
            {
                MyAssert.Throws(() => { }, message: userMessage);
            }
            catch (AssertFailedException e)
            {
                actual = e.Message;
            }

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestTestFailsWhenExceptionTypeNotMatched()
        {
            var testFailed = false;
            try
            {
                MyAssert.Throws(
                    () => { throw new Exception(); },
                    typeof(ArgumentNullException));
            }
            catch (AssertFailedException)
            {
                testFailed = true;
            }

            Assert.IsTrue(testFailed, "Error with MyAssert.Throws(Action, Type)");
        }

        [TestMethod]
        public void TestTestSucceedsWhenExceptionTypeMatched()
        {
            var testFailed = false;
            try
            {
                MyAssert.Throws(
                    () => { throw new ArgumentNullException(); },
                    typeof(ArgumentNullException));
            }
            catch (AssertFailedException)
            {
                testFailed = true;
            }

            Assert.IsFalse(testFailed, "Error with MyAssert.Throws(Action, Type)");
        }

        [TestMethod]
        public void TestExceptionTypeMismatchFailureMessage()
        {
            var detail = "foo";
            var message = "bar";
            var expected =
                MyAssert.BaseFailureMessages.EXCEPTION_TYPE_MISMATCH +
                String.Format(
                    MyAssert.FailureMessageFormatStrings.EXCEPTION_TYPE_MISMATCH,
                    "ArgumentNullException", "Exception", detail,
                    "  " + message);
            string actual = null;

            try
            {
                MyAssert.Throws(
                    () => { throw new Exception(detail); },
                    typeof(ArgumentNullException), message);
            }
            catch (AssertFailedException e)
            {
                actual = e.Message;
            }

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class MyAssertDoesNotThrowTest
    {
        [TestMethod]
        public void TestTestSucceedsWhenNoExceptionThrown()
        {
            var testFailed = false;

            try
            {
                MyAssert.DoesNotThrow(() => { });
            }
            catch (AssertFailedException)
            {
                testFailed = true;
            }

            Assert.IsFalse(testFailed);
        }

        [TestMethod]
        public void TestTestFailsWhenExceptionThrown()
        {
            var testFailed = false;

            try
            {
                MyAssert.DoesNotThrow(() => { throw new Exception(); });
            }
            catch (AssertFailedException)
            {
                testFailed = true;
            }

            Assert.IsTrue(testFailed);
        }

        [TestMethod]
        public void TestBasicFailureMessage()
        {
            string actual = null;
            var ex = new Exception();
            var expected =
                String.Format(
                    MyAssert.FailureMessageFormatStrings.EXCEPTION_THROWN,
                    "System.Exception", ex.Message);
            try
            {
                MyAssert.DoesNotThrow(() => { throw ex; });
            }
            catch (AssertFailedException e)
            {
                actual = e.Message;
            }

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class MyAssertIsGreaterThanTest
    {
        [TestMethod]
        public void TestTestSucceedsWhenLeftObjectIsGreater()
        {
            var failed = false;
            try
            {
                MyAssert.IsGreaterThan(1, 0);
            }
            catch (AssertFailedException)
            {
                failed = true;
            }

            Assert.IsFalse(failed);
        }

        [TestMethod]
        public void TestTestFailsWhenRightObjectIsGreater()
        {
            var failed = false;
            try
            {
                MyAssert.IsGreaterThan(0, 1);
            }
            catch (AssertFailedException)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }
    }

    [TestClass]
    public class MyAssertIsLessThanTest
    {
        [TestMethod]
        public void TestTestSucceedsWhenLeftObjectIsLess()
        {
            var failed = false;
            try
            {
                MyAssert.IsLessThan(0, 1);
            }
            catch (AssertFailedException)
            {
                failed = true;
            }

            Assert.IsFalse(failed);
        }

        [TestMethod]
        public void TestTestFailsWhenRightObjectIsLess()
        {
            var failed = false;
            try
            {
                MyAssert.IsLessThan(1, 0);
            }
            catch (AssertFailedException)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }
    }

    [TestClass]
    public class MyAssertAreCloseTest
    {
        [TestMethod]
        public void TestTestSucceedsWhenDifferenceBetweenValuesIsWithinThreshold()
        {
            var now = DateTime.Now;

            MyAssert.DoesNotThrow(
                () =>
                    MyAssert.AreClose(now, now.AddSeconds(59), new TimeSpan(0, 1, 0)));
            MyAssert.DoesNotThrow(
                () =>
                    MyAssert.AreClose(now, now.AddMinutes(59), new TimeSpan(1, 0, 0)));
            MyAssert.DoesNotThrow(
                () =>
                    MyAssert.AreClose(now, now.AddHours(23),
                        new TimeSpan(1, 0, 0, 0)));
        }

        [TestMethod]
        public void TestTestFailsWhenDifferenceBetweenValuesIsNotWithinThreshold()
        {
            var now = DateTime.Now;

            MyAssert.Throws<AssertFailedException>(
                () =>
                    MyAssert.AreClose(now, now.AddMinutes(1), new TimeSpan(0, 0, 59)));
            MyAssert.Throws<AssertFailedException>(
                () =>
                    MyAssert.AreClose(now, now.AddHours(1), new TimeSpan(0, 59, 0)));
            MyAssert.Throws<AssertFailedException>(
                () =>
                    MyAssert.AreClose(now, now.AddDays(1), new TimeSpan(0, 23, 0)));
        }

        [TestMethod]
        public void TestDefaultThresholdIsOneSecond()
        {
            var now = DateTime.Now;
            var secondLater = now.AddSeconds(1);
            var minuteLater = now.AddMinutes(1);
            var dayLater = now.AddDays(1);

            MyAssert.DoesNotThrow(() => MyAssert.AreClose(now, secondLater));

            MyAssert.Throws<AssertFailedException>(
                () => MyAssert.AreClose(now, minuteLater));
            MyAssert.Throws<AssertFailedException>(
                () => MyAssert.AreClose(now, dayLater));
        }
    }

    [TestClass]
    public class MyAssertCollectionsAreSimilarTest
    {
        private static void Passes<T>(IEnumerable<T> expected, IEnumerable<T> actual, bool canBeEmpty)
        {
            MyAssert.DoesNotThrow(() => MyAssert.CollectionsAreSimilar(expected, actual, canBeEmpty));
        }

        private static void Fails<T>(IEnumerable<T> expected, IEnumerable<T> actual, bool canBeEmpty)
        {
            MyAssert.Throws<AssertFailedException>(() => MyAssert.CollectionsAreSimilar(expected, actual, canBeEmpty));
        }

        #region Passes

        [TestMethod]
        public void TestPassesIfBothCollectionsAreTheSameNonEmptyInstance()
        {
            var list = new List<string> {"sup"};
            Passes(list, list, false);
        }

        [TestMethod]
        public void TestPassesIfBothCollectionsAreSameEmptyInstanceAndCanBeEmptyIsTrue()
        {
            var list = new List<string>();
            Passes(list, list, true);
        }

        [TestMethod]
        public void TestPassesIfBothCollectionsAreDifferentInstanceAndEmptyAndCanBeEmptyIsTrue()
        {
            Passes(new List<string>(), new List<string>(), true);
        }

        [TestMethod]
        public void TestPassesIfBothCollectionsHaveTheSameItemsInAnyOrder()
        {
            var expected = new List<string> {"one", "two"};
            var actual = new List<string> {"two", "one"};
            Passes(expected, actual, false);
            Passes(expected, actual, true);
        }

        [TestMethod]
        public void TestPassesIfBothCollectionsHaveTheSameItemsInSameOrder()
        {
            var expected = new List<string> {"one", "two"};
            var actual = new List<string> {"one", "two"};
            Passes(expected, actual, false);
            Passes(expected, actual, true);
        }

        #endregion

        #region Fails

        [TestMethod]
        public void TestFailsIfBothCollectionsAreTheSameEmptyInstanceAndCanBeEmptyIsFalse()
        {
            var list = new List<string>();
            Fails(list, list, false);
        }

        [TestMethod]
        public void TestFailsIfBothCollectionsAreEmptyAndCanBeEmptyIsFalse()
        {
            Fails(new List<string>(), new List<string>(), false);
        }

        [TestMethod]
        public void TestFailsIfCollectionsHaveDifferentCounts()
        {
            var expected = new List<string> {"one"};
            var actual = new List<string> {"one", "two"};
            Fails(expected, actual, false);
            Fails(expected, actual, true);
        }

        [TestMethod]
        public void TestFailsIfCollectionsHaveSameCountButDifferentItems()
        {
            var expected = new List<string> {"one", "two"};
            var actual = new List<string> {"two", "three"};
            Fails(expected, actual, false);
            Fails(expected, actual, true);
        }

        #endregion
    }

    [TestClass]
    public class CanGetAndSetPropertyTest
    {
        #region Fields

        private SomeClass<string> _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new SomeClass<string>();
        }

        #endregion

        #region Tests

        private static void Passes<TObj, TMember>(TObj obj, Expression<Func<TMember>> exp, params TMember[] values)
        {
            MyAssert.DoesNotThrow(() => MyAssert.CanGetAndSetProperty(obj, exp, values));
        }

        private static void Fails<TObj, TMember>(TObj obj, Expression<Func<TMember>> exp, params TMember[] values)
        {
            MyAssert.Throws<AssertFailedException>(() => MyAssert.CanGetAndSetProperty(obj, exp, values));
        }

        [TestMethod]
        public void TestAllValuesGetPassedToSetterInPassingScenario()
        {
            var expected = new[] {"string", "some dude", ""};
            Passes(_target, () => _target.Property, expected);
            MyAssert.AreEqual(expected, _target.ValuesSet);
        }

        [TestMethod]
        public void TestAllValuesGetRetrievedFromGetterInPassingScenario()
        {
            var expected = new[] {"string", "some dude", ""};
            Passes(_target, () => _target.Property, expected);
            MyAssert.AreEqual(expected, _target.ValuesGotten);
        }

        [TestMethod]
        public void TestFailsIfValueSetIsNotReturnedByGet()
        {
            Fails(_target, () => _target.PropertyThatDoesNotSet, new[] {"yeah"});
        }

        #endregion

        #region Test class

        private class SomeClass<T>
        {
            private T _property;

            public List<T> ValuesGotten = new List<T>();
            public List<T> ValuesSet = new List<T>();

            public T Property
            {
                get
                {
                    ValuesGotten.Add(_property);
                    return _property;
                }
                set
                {
                    _property = value;
                    ValuesSet.Add(value);
                }
            }

            public T PropertyThatDoesNotSet
            {
                get { return _property; }
                set
                {
                    // noop.
                }
            }
        }

        #endregion
    }
}
