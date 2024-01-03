using MMSINC.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Data.Linq
{
    /// <summary>
    /// Summary description for PredicateBuilderTest
    /// </summary>
    [TestClass]
    public class PredicateBuilderTest
    {
        [TestMethod]
        public void TestTrueReturnsPredicateWhichEvaluatesToTrue()
        {
            var fn = PredicateBuilder.True<object>().Compile();

            Assert.IsTrue(fn(null));
        }

        [TestMethod]
        public void TestFalseReturnsPredicateWhichEvaluatesToFalse()
        {
            var fn = PredicateBuilder.False<object>().Compile();

            Assert.IsFalse(fn(null));
        }

        [TestMethod]
        public void TestOrReturnsPredicateWhichEvaluatesToTrueWhenAnyPredicateEvaluatesToTrue()
        {
            // initialize to the false expression.
            var initialExpr = PredicateBuilder.False<object>();

            // add a true expression with or
            var newExpr = initialExpr.Or(o => true);

            // compile the expression to get a runnable lambda
            var fn = newExpr.Compile();

            Assert.IsTrue(fn(null));
        }

        [TestMethod]
        public void TestOrReturnsPredicateWhichEvaluatesToFalseWhenAllPredicatesEvaluateToFalse()
        {
            // initialize to the false expression.
            var initialExpr = PredicateBuilder.False<object>();

            // add a false expression with or
            var newExpr = initialExpr.Or(o => false);

            // compile the expression to get a runnable lambda
            var fn = newExpr.Compile();

            Assert.IsFalse(fn(null));
        }

        [TestMethod]
        public void TestAndReturnsPredicateWhichEvaluatesToTrueWhenAllPredicatesEvaluateToTrue()
        {
            // initialize to the true expression.
            var initialExpr = PredicateBuilder.True<object>();

            // add a true expression with and
            var newExpr = initialExpr.And(o => true);

            // compile the expression to get a runnable lambda
            var fn = newExpr.Compile();

            Assert.IsTrue(fn(null));
        }

        [TestMethod]
        public void TestAndReturnsPredicateWhichEvaluatesToFalseWhenAnyPredicateEvaluatesToFalse()
        {
            // initialize to the true expression.
            var initialExpr = PredicateBuilder.True<object>();

            // add a false expression with and
            var newExpr = initialExpr.And(o => false);

            // compile the expression to get a runnable lambda
            var fn = newExpr.Compile();

            Assert.IsFalse(fn(null));
        }
    }
}
