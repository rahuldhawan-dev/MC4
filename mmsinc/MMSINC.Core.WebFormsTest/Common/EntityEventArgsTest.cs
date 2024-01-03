using System;
using MMSINC.Common;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINCTestImplementation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Common
{
    /// <summary>
    /// Summary description for DataEventArgsTest
    /// </summary>
    [TestClass]
    public class EntityEventArgsTest
    {
        #region Constructors

        public EntityEventArgsTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

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

        [TestMethod]
        public void TestConstructWithNullArgumentThrowsException()
        {
#pragma warning disable 219
            // temporary storage, it doesn't matter that the value never gets used
            EntityEventArgs<Employee> target;
#pragma warning restore 219
            MyAssert.Throws(() => target = new EntityEventArgs<Employee>(null),
                typeof(ArgumentNullException));
        }

        [TestMethod]
        public void TestEntityProperty()
        {
            Employee target = new Employee();
            EntityEventArgs<Employee> actual = new EntityEventArgs<Employee>(target);
            MyAssert.IsNotNullButInstanceOfType(actual.Entity, typeof(Employee));
            Assert.AreSame(target, actual.Entity);
        }

        [TestMethod]
        public void TestEmptyStaticProperty()
        {
            EntityEventArgs<Employee> target = EntityEventArgs<Employee>.Empty;
            MyAssert.IsNotNullButInstanceOfType(target, typeof(EntityEventArgs<Employee>));
            MyAssert.IsNotNullButInstanceOfType(target.Entity, typeof(Employee));
        }

        [TestMethod]
        public void TestToString()
        {
            Employee target = new Employee();
            EntityEventArgs<Employee> actual = new EntityEventArgs<Employee>(target);
            Assert.AreEqual(actual.ToString(), target.ToString());
        }
    }
}
