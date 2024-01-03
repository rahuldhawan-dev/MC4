using System.Reflection;
using MMSINC.Common;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Common
{
    /// <summary>
    /// Summary description for ComparerTest
    /// </summary>
    [TestClass]
    public class EntityComparerTest
    {
        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup]
        // public void MyTestCleanup() { }
        //

        #endregion

        //OVERKILL?
        [TestMethod]
        public void TestCompareConstructorSetsProperty()
        {
            const string expected = "ID";
            var target = new EntityComparer<TestObject>(expected);
            var fieldInfo = target.GetType().GetField("_propertyName", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.AreEqual(expected, (string)fieldInfo.GetValue(target));
        }

        [TestMethod]
        public void TestCompareConstructorThrowsExceptionForNullProperty()
        {
            MyAssert.Throws(() => new EntityComparer<TestObject>(null), typeof(DomainLogicException),
                "Compare Class Allowed a Null PropertyName.");
        }

        [TestMethod]
        public void TestSortExpressionWithNoDirectionSpecifiedDefaultsToAscending()
        {
            var target = new EntityComparer<TestObject>("ID");
            var fieldInfo = target.GetType().GetField("_sortAscending", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsTrue((bool)fieldInfo.GetValue(target));
        }

        [TestMethod]
        public void TestSortExpressionWithDirectionSpecifiedProperlySetsSortAscendingValue()
        {
            var target = new EntityComparer<TestObject>("ID DESC");
            var fieldInfo = target.GetType().GetField("_sortAscending", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsFalse((bool)fieldInfo.GetValue(target));
        }

        [TestMethod]
        public void TestCompareWhereObjectsAreSameReturnsZero()
        {
            EntityComparer<TestObject> target = new EntityComparer<TestObject>("ID");
            TestObject testObject = new TestObject(0);
            Assert.AreEqual(0, target.Compare(testObject, testObject));
        }

        [TestMethod]
        public void TestCompareWhereBothObjectsAreNullReturnsZero()
        {
            var target = new EntityComparer<TestObject>("ID");
            Assert.AreEqual(0, target.Compare(null, null));
        }

        [TestMethod]
        public void TestCompareWhereLeftObjectGreaterReturnsNegative()
        {
            EntityComparer<TestObject> target = new EntityComparer<TestObject>("ID");
            TestObject leftObject = new TestObject(0);
            TestObject rightObject = new TestObject(1);
            MyAssert.IsGreaterThan(target.Compare(rightObject, leftObject), 0);
        }

        [TestMethod]
        public void TestCompareWhereRightObjectNullReturnsNegative()
        {
            var target = new EntityComparer<TestObject>("ID");
            var leftObject = new TestObject(0);
            MyAssert.IsGreaterThan(target.Compare(leftObject, null), 0);
        }

        [TestMethod]
        public void TestCompareWhereRightObjectGreaterReturnsPositive()
        {
            EntityComparer<TestObject> target = new EntityComparer<TestObject>("ID");
            TestObject leftObject = new TestObject(0);
            TestObject rightObject = new TestObject(1);
            int expected = target.Compare(rightObject, leftObject);
            MyAssert.IsGreaterThan(expected, 0);
        }

        [TestMethod]
        public void TestCompareWhereLeftObjectNullReturnsPositive()
        {
            var target = new EntityComparer<TestObject>("ID");
            var rightObject = new TestObject(1);
            MyAssert.IsGreaterThan(0, target.Compare(null, rightObject));
        }

        [TestMethod]
        public void TestCompareOnPropertyOfChildObject()
        {
            EntityComparer<TestObject> target = new EntityComparer<TestObject>("ChildObject.ID");
            TestObject leftObject = new TestObject(new TestObject(0));
            TestObject rightObject = new TestObject(new TestObject(1));

            Assert.AreEqual(0, target.Compare(leftObject, leftObject));

            MyAssert.IsLessThan(target.Compare(leftObject, rightObject), 0);

            MyAssert.IsGreaterThan(
                target.Compare(rightObject, leftObject), 0);
        }
    }

    internal class TestObject
    {
        #region Properties

        public int ID { get; set; }
        public string Field { get; set; }
        public TestObject ChildObject { get; set; }

        #endregion

        #region Constructors

        public TestObject(int id)
        {
            ID = id;
        }

        public TestObject(TestObject child)
        {
            ChildObject = child;
        }

        #endregion
    }
}
