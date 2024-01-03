using System;
using System.Reflection;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINCTestImplementation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.TestLibrary;

namespace MMSINC.TestingTest
{
    /// <summary>
    /// Summary description for LinqUnitTestClassTest
    /// </summary>
    [TestClass]
    public class LinqUnitTestClassTest
    {
        [TestMethod]
        public void TestGetValidObjectReturnsValidObject()
        {
            var sampleTest = new SampleTestClass();
            var target = ReflectionHelper.InvokeNonPublicMethod<Shipper>(sampleTest, "GetValidObject");
            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(Shipper));
        }

        [TestMethod]
        public void TestGetLengthFromVarCharString()
        {
            var sampleTestClass = new SampleTestClass();
            var mi = typeof(SampleTestClass).GetMethod("GetLengthFromVarChar",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var expected = 50;
            var target = (int)mi.Invoke(sampleTestClass, new object[] {"VarChar(50) Not NULL"});

            Assert.IsInstanceOfType(target, typeof(int), "Parsing of of String did not return an Integer");
            Assert.AreEqual(expected, target, "Expected value was not returned.");
        }

        [TestMethod]
        public void TestTestAllStringPropertiesThrowExceptionWhenSetTooLongFailsWhenNotSet()
        {
            var sampleTestClass = new SampleTestClass();
            MyAssert.Throws(() => sampleTestClass.TestAllStringPropertiesThrowsExceptionWhenSetTooLong(),
                typeof(AssertFailedException),
                "Should have thrown AssertFailedException. The TUnitType used in SampleTestClass probably has it's properties checking for length now.");
        }

        ////[TestMethod]
        //public void TestStringPropertiesCheckedForLength()
        //{
        //    var properties = typeof (SampleTestClass).GetProperties();

        //    foreach(var property in properties)
        //    {
        //    }

        //    throw new NotImplementedException();
        //}

        //[TestMethod]
        //public void TestSetPropertyAboveMaxLengthThrowsException()
        //{
        //    var stc = new SampleTestClass();
        //    var methodInfo = typeof (SampleTestClass).GetMethod("SetProperty",
        //                                                        BindingFlags.NonPublic | BindingFlags.Instance);
        //    var propertyInfo = typeof (Employee).GetProperty("FirstName");
        //    MyAssert.Throws(() => 
        //        methodInfo.Invoke(stc, new object[] { propertyInfo, "123456"}),
        //        typeof(DomainLogicException),
        //        "DomainLogicException Thrown"
        //    );
        //}
    }
}
