using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using MMSINC.DataPages;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MMSINC.Core.WebFormsTest.DataPages
{
    /// <summary>
    /// Summary description for FilterBuilderTest
    /// </summary>
    [TestClass]
    public class FilterBuilderTest
    {
        #region Fields

        private FilterBuilder _target;
        private MockRepository _mocks;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void FilterBuilderTestInitialize()
        {
            _target = InitializeTarget();
            _mocks = new MockRepository();
        }

        [TestCleanup]
        public void FilterBuilderTestCleanup()
        {
            _mocks.VerifyAll();
        }

        private TestFilterBuilderBuilder InitializeTarget()
        {
            return new TestFilterBuilderBuilder();
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void TestConstructorCreatesExpressionsList()
        {
            _target = InitializeTarget();
            Assert.IsNotNull(_target.Expressions, "Expressions collection must be initialized by constructor.");
        }

        #region AddExpression method

        [TestMethod]
        public void TestAddExpressionThrowsNullReferenceExceptionWhenArgumentIsNull()
        {
            _target = InitializeTarget();
            MyAssert.Throws(() => _target.AddExpression(null));
        }

        [TestMethod]
        public void TestAddExpressionDoesNotAddDuplicates()
        {
            _target = InitializeTarget();

            var fbe = new FilterBuilderExpression();

            _target.AddExpression(fbe);
            _target.AddExpression(fbe);
            _target.AddExpression(fbe);

            Assert.IsTrue(_target.Expressions.Count == 1);
        }

        #endregion

        [TestMethod]
        public void TestBuildFilterExpressionReturnsEmptyStringIfNoExpressions()
        {
            _target = InitializeTarget();
            Assert.AreEqual(_target.BuildFilter(), string.Empty);
        }

        [TestMethod]
        public void TestBuildWhereClauseAppendsNewLineAtBeginning()
        {
            _target = InitializeTarget();
            _target.AddExpression(new FilterBuilderExpression("someSaram", DbType.Time, "yeah"));
            Assert.IsTrue(
                _target.BuildWhereClause().StartsWith(Environment.NewLine));
        }

        [TestMethod]
        public void TestBuildWhereClauseReturnsEmptyStringIfNoParameters()
        {
            _target = InitializeTarget();
            Assert.AreEqual(string.Empty, _target.BuildWhereClause());
        }

        [TestMethod]
        public void TestBuilderFilterReturnsProperlyFormattedString()
        {
            _target = InitializeTarget();
            _target.AddExpression(new FilterBuilderExpression("someSaram", DbType.Time, "yeah"));

            Assert.AreEqual(_target.BuildFilter(), "[someSaram] = @someSaram");

            _target.AddExpression(new FilterBuilderExpression("anotherParam", DbType.Time, "yeah"));

            Assert.AreEqual(_target.BuildFilter(), "[someSaram] = @someSaram AND [anotherParam] = @anotherParam");
        }

        [TestMethod]
        public void TestBuildParametersReturnsEmptyCollectionWhenThereAreNoValidFilters()
        {
            _target = InitializeTarget();
            var parms = _target.BuildParameters();

            Assert.IsFalse(parms.Any());
        }

        [TestMethod]
        public void TestBuildParametersReturnsExpectedParameters()
        {
            _target = InitializeTarget();

            IFilterBuilderExpression mockedExpression = null;
            _mocks.DynamicMock(out mockedExpression);

            var testParam = new Parameter("someParameter", DbType.Xml, "gaigea");
            var parms = new List<Parameter>();
            parms.Add(testParam);

            using (_mocks.Record())
            {
                SetupResult.For(mockedExpression.BuildParameters()).Return(parms);
            }

            using (_mocks.Playback())
            {
                _target.AddExpression(mockedExpression);
                var result = _target.BuildParameters();
                Assert.IsTrue(result.Contains(testParam));
            }
        }

        #endregion
    }

    public class TestFilterBuilder : FilterBuilder { }

    public class TestFilterBuilderBuilder : TestDataBuilder<TestFilterBuilder>
    {
        #region Fields

        #endregion

        #region Methods

        public override TestFilterBuilder Build()
        {
            var target = new TestFilterBuilder();

            return target;
        }

        #endregion
    }
}
