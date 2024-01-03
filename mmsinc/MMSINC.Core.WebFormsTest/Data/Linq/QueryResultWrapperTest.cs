using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.Linq;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINCTestImplementation.Model;
using Rhino.Mocks;

namespace MMSINC.Core.WebFormsTest.Data.Linq
{
    /// <summary>
    /// Summary description for QueryResultWrapperTest.
    /// </summary>
    [TestClass]
    public class QueryResultWrapperTest : EventFiringTestClass
    {
        #region Private Members

        private TestQueryResultWrapper _target;
        private ITable<Employee> _iTable;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks.DynamicMock(out _iTable);

            _target =
                new TestQueryResultWrapperBuilder().WithITable(_iTable);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestWhereReturnsWrappedQueryResult()
        {
            Expression<Func<Employee, bool>> func = e => true;
            QueryResultWrapper<Employee>.SetQueryFactory((qry, expr) => _iTable);

            using (_mocks.Record())
            {
                SetupResult.For(_iTable.Where(func)).Return(_iTable);
            }

            using (_mocks.Playback())
            {
                var result = _target.Where(func);

                var fi = result.GetType().GetField("_iQueryable",
                    BindingFlags.Instance | BindingFlags.NonPublic);

                Assert.IsInstanceOfType(result, typeof(QueryResultWrapper<Employee>));
                Assert.AreSame(_iTable, fi.GetValue(result));
            }

            QueryResultWrapper<Employee>.ResetQueryFactory();
        }

        [TestMethod]
        public void TestElementTypeReturnsCorrectType()
        {
            var type = typeof(Employee);
            using (_mocks.Record())
            {
                SetupResult.For(_iTable.ElementType).Return(type);
            }

            using (_mocks.Playback())
            {
                Assert.AreSame(type, _target.ElementType);
            }
        }

        [TestMethod]
        public void TestExpressionReturnsExpressionFromWrappedQueryResult()
        {
            Expression<Func<Employee, bool>> expression = e => true;
            using (_mocks.Record())
            {
                SetupResult.For(_iTable.Expression).Return(expression);
            }

            using (_mocks.Playback())
            {
                Assert.AreSame(expression, _target.Expression);
            }
        }

        [TestMethod]
        public void TestProviderReturnsProviderFromWrappedQueryResult()
        {
            var provider = _mocks.DynamicMock<IQueryProvider>();
            using (_mocks.Record())
            {
                SetupResult.For(_iTable.Provider).Return(provider);
            }

            using (_mocks.Playback())
            {
                Assert.AreSame(provider, _target.Provider);
            }
        }

        [TestMethod]
        public void TestGetEnumeratorGenericReturnsEnumeratorFromWrappedQueryResult()
        {
            var enumerator = _mocks.DynamicMock<IEnumerator<Employee>>();
            using (_mocks.Record())
            {
                SetupResult.For(_iTable.GetEnumerator()).Return(enumerator);
            }

            using (_mocks.Playback())
            {
                Assert.AreSame(enumerator, ((IEnumerable)_target).GetEnumerator());
            }
        }
    }

    internal class TestQueryResultWrapperBuilder : TestDataBuilder<TestQueryResultWrapper>
    {
        #region Private Members

        private ITable<Employee> _iTable;

        #endregion

        #region Exposed Methods

        public override TestQueryResultWrapper Build()
        {
            var obj = new TestQueryResultWrapper(_iTable);
            return obj;
        }

        public TestQueryResultWrapperBuilder WithITable(
            ITable<Employee> iTable)
        {
            _iTable = iTable;
            return this;
        }

        #endregion
    }

    internal class TestQueryResultWrapper : QueryResultWrapper<Employee>
    {
        public TestQueryResultWrapper(ITable<Employee> iQueryable)
            : base(iQueryable) { }
    }
}
