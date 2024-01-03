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
    /// Summary description for FilterBuilderExpressionTest
    /// </summary>
    [TestClass]
    public class FilterBuilderExpressionTest
    {
        #region Fields

        private TestFilterBuilderExpression _target;
        private MockRepository _mocks;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void FilterBuilderExpressionTestInitialize()
        {
            _target = InitializeTarget();
            _mocks = new MockRepository();
        }

        [TestCleanup]
        public void FilterBuilderExpressionTestCleanup()
        {
            _mocks.VerifyAll();
        }

        private TestFilterBuilderExpressionBuilder InitializeTarget()
        {
            return new TestFilterBuilderExpressionBuilder();
        }

        #endregion

        #region Test Methods

        #region Constructor tests

        [TestMethod]
        public void TestAllConstructorsCreateParametersList()
        {
            Action<FilterBuilderExpression> testThis =
                (FilterBuilderExpression fbe) => Assert.IsNotNull(fbe.Parameters,
                    "Parameters collection must be initialized by constructor.");

            testThis(new FilterBuilderExpression());
            testThis(new FilterBuilderExpression("some custom filter"));
            testThis(new FilterBuilderExpression("some param", DbType.Time, null));
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionForNullOrWhitespaceCustomFilterExpressions()
        {
            MyAssert.Throws(() => new FilterBuilderExpression(null));
            MyAssert.Throws(() => new FilterBuilderExpression(string.Empty));
            MyAssert.Throws(() => new FilterBuilderExpression("    "));
        }

        [TestMethod]
        public void TestConstructorSetsCustomFilterExpression()
        {
            var expected = "some filter";
            var test = new FilterBuilderExpression(expected);
            Assert.AreEqual(expected, test.CustomFilterExpression);
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionForNullNameParameter()
        {
            MyAssert.Throws(() => new FilterBuilderExpression(null, DbType.String, null));
        }

        [TestMethod]
        public void TestConstructorAddsInitialParameter()
        {
            const string expectedName = "someParam";
            const DbType expectedDbType = DbType.String;
            const string expectedValue = "some Value";

            var test = new FilterBuilderExpression("someParam", DbType.String,
                "some Value");

            Assert.IsTrue(test.Parameters.Count == 1);

            var parm = test.Parameters.First();

            Assert.AreEqual(expectedName, parm.Name);
            Assert.AreEqual(expectedDbType, parm.DbType);
            Assert.AreEqual(expectedValue, expectedValue);
        }

        #endregion

        #region AddParameter method

        [TestMethod]
        public void TestAddParameterAddsParameter()
        {
            _target = InitializeTarget();

            var p = new FilterBuilderParameter();
            _target.AddParameter(p);

            Assert.IsTrue(_target.Parameters.Contains(p), "Parameter must be added to the parameter collection.");
        }

        [TestMethod]
        public void TestAddParameterOverloadAddsTheParameterThatItReturns()
        {
            _target = InitializeTarget();
            var p = _target.AddParameter("someParam", DbType.Decimal, null);
            Assert.IsTrue(_target.Parameters.Contains(p));
        }

        [TestMethod]
        public void TestAddParameterThrowsNullReferenceExceptionWhenParameterIsNull()
        {
            _target = InitializeTarget();
            MyAssert.Throws(() => _target.AddParameter(null));
        }

        [TestMethod]
        public void TestAddParameterDoesNotAddDuplicates()
        {
            var testParams = new List<IFilterBuilderParameter>();

            _target = InitializeTarget()
                     .WithParameters(testParams)
                     .Build();

            var p = new FilterBuilderParameter {
                Name = "Some Name"
            };

            _target.AddParameter(p);
            _target.AddParameter(p);
            _target.AddParameter(p);

            Assert.IsTrue(_target.Parameters.Count == 1);
        }

        [TestMethod]
        public void TestAddParameterOverloadThrowsExceptionForNullNameArgument()
        {
            MyAssert.Throws(() => _target.AddParameter(null, DbType.Object, null));
        }

        [TestMethod]
        public void TestAddParameterOverloadReturnsFilterParameterWithSameValues()
        {
            var expectedName = "someParam";
            var expectedDbType = DbType.String;
            var expectedValue = "some Value";

            _target = InitializeTarget();

            var parm = _target.AddParameter(expectedName, expectedDbType,
                expectedValue);

            Assert.AreEqual(expectedName, parm.Name);
            Assert.AreEqual(expectedDbType, parm.DbType);
            Assert.AreEqual(expectedValue, expectedValue);
        }

        #endregion

        #region BuildFilterExpression method

        [TestMethod]
        public void TestBuildFilterExpressionAddsFilterExpressionPropertyToPassedArgumentWhenCanBuildIsTrue()
        {
            _target = InitializeTarget();
            _target.AddParameter("param1", DbType.Binary, "ok");

            var list = new List<string>();
            var expected = _target.FilterExpressionTest;

            _target.BuildFilterExpression(list);

            Assert.IsTrue(list.Contains(expected));
        }

        [TestMethod]
        public void TestBuildFilterExpressionDoesNotAddToPassedArgumentIfCanBuildIsFalse()
        {
            _target = InitializeTarget();

            var list = new List<string>();

            _target.BuildFilterExpression(list);

            Assert.IsFalse(list.Any());
        }

        #endregion

        #region BuildParameters method

        [TestMethod]
        public void TestBuildParametersReturnsEmptyListWhenCanBuildIsFalse()
        {
            _target = InitializeTarget();

            var parms = _target.BuildParameters();

            Assert.IsNotNull(parms);

            var parmsAsIenumerable = (IEnumerable<Parameter>)parms;
            Assert.IsFalse(parmsAsIenumerable.Any());
        }

        [TestMethod]
        public void TestBuildParametersCreatesValidParameterFromFilterBuilderParameter()
        {
            var expectedName = "I am a BANANA!";
            var expectedDbType = DbType.Int32;
            var expectedValue = 23592;
            var expectedValueAsString = expectedValue.ToString();

            _target = InitializeTarget();
            var filterParam = _target.AddParameter(expectedName, expectedDbType, expectedValue);

            var parms = _target.BuildParameters();

            Assert.IsTrue(parms.Count() == 1);

            var sqlParam = parms.First();

            Assert.AreEqual(sqlParam.Name, filterParam.ParameterFormattedName);
            Assert.AreEqual(sqlParam.DbType, expectedDbType);
            Assert.AreEqual(sqlParam.DefaultValue, expectedValueAsString);
        }

        #endregion

        #region FilterExpression property

        [TestMethod]
        public void TestFilterExpressionReturnsStringEmptyIfCanBuildIsfalse()
        {
            _target = InitializeTarget();
            Assert.AreEqual(string.Empty, _target.FilterExpressionTest);
        }

        [TestMethod]
        public void TestFilterExpressionReturnsCustomFilterExpression()
        {
            var expected = "some expression";
            _target = InitializeTarget();
            _target.CustomFilterExpression = expected;

            Assert.AreEqual(expected, _target.FilterExpressionTest,
                "FilterExpression property must return CustomFilterExpression if CustomFilterExpression is set.");
        }

        [TestMethod]
        public void TestFilterExpressionThrowsExceptionIfHasMultipleParametersAndNoCustomFilterExpression()
        {
            _target = InitializeTarget();
            _target.AddParameter("param1", DbType.Binary, "ok");
            _target.AddParameter("param2", DbType.Currency, "ok2342");

            string something = null;
            MyAssert.Throws(() => something = _target.FilterExpressionTest);
        }

        [TestMethod]
        public void TestFilterExpressionReturnsProperlyFormattedAutoGeneratedExpression()
        {
            _target = InitializeTarget();
            _target.AddParameter("param1", DbType.String, "some value");

            var expected = "[param1] = @param1";
            Assert.AreEqual(expected, _target.FilterExpressionTest);
        }

        #endregion

        #region CanBuild Property

        [TestMethod]
        public void TestCanBuildReturnsFalseWhenIgnoreNullsTrueAndHasNullValueParameter()
        {
            _target = InitializeTarget();
            _target.IgnoreIfThereAreNullParameters = true;
            _target.AddParameter("something", DbType.SByte, null);

            Assert.IsFalse(_target.CanBuild);
        }

        [TestMethod]
        public void
            TestCanBuildReturnsTrueWhenIgnoreNullsTrueAndDoesNotHaveNullValueParameterAndHasCustomFilterExpression()
        {
            _target = InitializeTarget();
            _target.CustomFilterExpression = "some expression";
            _target.IgnoreIfThereAreNullParameters = true;
            _target.AddParameter("something", DbType.SByte, "some value");

            Assert.IsTrue(_target.CanBuild);
        }

        [TestMethod]
        public void TestCanBuildReturnsTrueWhenIgnoreNullsTrueAndOnlyHasNonNullParametersAndNoCustomExpression()
        {
            _target = InitializeTarget();
            _target.IgnoreIfThereAreNullParameters = true;
            _target.AddParameter("something", DbType.SByte, "some value");

            Assert.IsTrue(_target.CanBuild);
        }

        #endregion

        #endregion
    }

    public class TestFilterBuilderExpression : FilterBuilderExpression
    {
        public string FilterExpressionTest
        {
            get { return FilterExpression; }
        }
    }

    public class TestFilterBuilderExpressionBuilder : TestDataBuilder<TestFilterBuilderExpression>
    {
        #region Fields

        private IList<IFilterBuilderParameter> _params;

        #endregion

        public TestFilterBuilderExpressionBuilder WithParameters(IList<IFilterBuilderParameter> builderParameters)
        {
            _params = builderParameters;
            return this;
        }

        public override TestFilterBuilderExpression Build()
        {
            var target = new TestFilterBuilderExpression();

            if (_params != null)
            {
                target.Parameters = _params;
            }

            return target;
        }
    }
}
