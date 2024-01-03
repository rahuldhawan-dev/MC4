using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Utilities.StructureMap;
using StructureMap;

namespace MMSINC.CoreTest.Data
{
    [TestClass]
    public class NumericRangeTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(new Container()));
        }

        [TestMethod]
        public void TestIsValidReturnsTrue()
        {
            var target = new NumericRange();

            Assert.IsTrue(target.IsValid);
        }

        [TestMethod]
        public void TestIsValidReturnsTrueWhenValid()
        {
            var target = new NumericRange {Start = 0, End = 15};

            Assert.IsTrue(target.IsValid);
        }

        [TestMethod]
        public void TestIsValidReturnsFalseWhenStartNotProvided()
        {
            var target = new NumericRange {End = 15};

            Assert.IsFalse(target.IsValid);
        }

        [TestMethod]
        public void TestIsValidReturnsFalseWhenEndNotProvided()
        {
            var target = new NumericRange {Start = 15.5m,};

            Assert.IsFalse(target.IsValid);
        }

        [TestMethod]
        public void TestValidateReturnsErrorIfOperatorIsBetweenAndEndHasValueButStartDoesNotHaveValue()
        {
            var target = new NumericRange {Operator = RangeOperator.Between, End = 15.5m, Start = null};
            ValidationAssert.ModelStateHasError(target, "", Range.Errors.DEFAULT_START_VALUE_REQUIRED);
        }

        [TestMethod]
        public void TestValidateReturnsErrorIfOperatorIsBetweenAndStartHasValueGreaterThanEnd()
        {
            var target = new NumericRange {Operator = RangeOperator.Between, End = 15.5m, Start = 16};
            ValidationAssert.ModelStateHasError(target, "", Range.Errors.DEFAULT_START_LESS_THAN_END);
        }
    }
}
