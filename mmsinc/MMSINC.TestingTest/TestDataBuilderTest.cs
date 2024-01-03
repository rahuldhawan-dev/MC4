using MMSINC.Testing.DesignPatterns;
using MMSINCTestImplementation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.TestingTest
{
    /// <summary>
    /// Summary description for TestDataBuilderTest
    /// </summary>
    [TestClass]
    public class TestDataBuilderTest
    {
        [TestMethod]
        public void TestBuildReturnsValidObjectOfProperType()
        {
            var employee = new TestEmployeeBuilder().Build();

            Assert.IsNotNull(employee);
            Assert.IsInstanceOfType(employee, typeof(Employee));
        }

        [TestMethod]
        public void TestBuildReturnsObjectWithDefaultValues()
        {
            var employee = new TestEmployeeBuilder().Build();

            Assert.AreEqual(TestEmployeeBuilder.FIRST_NAME, employee.FirstName);
            Assert.AreEqual(TestEmployeeBuilder.LAST_NAME, employee.LastName);
        }

        [TestMethod]
        public void TestWithMethodsOverrideDefaults()
        {
            var firstName = "Smith";
            var lastName = "John";

            Employee employee = new TestEmployeeBuilder()
                               .WithFirstName(firstName).WithLastName(lastName);

            Assert.AreEqual(firstName, employee.FirstName);
            Assert.AreEqual(lastName, employee.LastName);
        }
    }

    internal class TestEmployeeBuilder : TestDataBuilder<Employee>
    {
        #region Constants / Defaults

        public const string FIRST_NAME = "John";
        public const string LAST_NAME = "Smith";

        #endregion

        #region Private Members

        private string _firstName = FIRST_NAME, _lastName = LAST_NAME;

        #endregion

        #region Exposed Methods

        public override Employee Build()
        {
            return new Employee {FirstName = _firstName, LastName = _lastName};
        }

        public TestEmployeeBuilder WithFirstName(string firstName)
        {
            _firstName = firstName;
            return this;
        }

        public TestEmployeeBuilder WithLastName(string lastName)
        {
            _lastName = lastName;
            return this;
        }

        #endregion
    }
}
