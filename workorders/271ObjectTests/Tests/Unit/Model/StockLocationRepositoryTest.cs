using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MMSINC.Data.Linq;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using PredicateBuilder = MMSINC.Common.PredicateBuilder;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for StockLocationRepositoryTest.
    /// </summary>
    [TestClass]
    public class StockLocationRepositoryTest : EventFiringTestClass
    {
        #region Private Members

        private TestStockLocationRepository _target;
        private ITable<StockLocation> _dataTable;
        private ISecurityService _securityService;
        private OperatingCenter nj4, nj7;
        private StockLocation nj4StockLocation, nj7StockLocation, locstock2smkbarrels;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void StockLocationRepositoryTestInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _dataTable)
                .DynamicMock(out _securityService);

            CreateSampleData();

            _target = new TestStockLocationRepositoryBuilder()
                .WithDataTable(_dataTable)
                .WithSecurityService(_securityService);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
            _target.Dispose();
        }

        private void CreateSampleData()
        {
            nj7 = new OperatingCenter {
                OpCntrName = "NJ7",
                OperatingCenterID = 10
            };
            nj4 = new OperatingCenter {
                OpCntrName = "NJ4",
                OperatingCenterID = 14
            };
            nj4StockLocation = new StockLocation {
                OperatingCenter = nj4
            };
            nj7StockLocation = new StockLocation {
                OperatingCenter = nj7, 
                IsActive = true
            };
            locstock2smkbarrels = new StockLocation();
        }

        #endregion

        #region Delegates

        private delegate bool TestExpressionDelegate(Expression<Func<StockLocation, bool>> expr);

        #endregion

        //[TestMethod]
        //public void TestSelectActiveByOperatingCenterReturnsOnlyActive()
        //{
        //    int count = 0;
        //    var results = new[] {
        //        nj4StockLocation, nj7StockLocation, locstock2smkbarrels
        //    };
            
        //    using (_mocks.Record())
        //    {
        //        SetupResult.For(_securityService.UserOperatingCenters).Return(
        //            new[] {
        //                nj7
        //            });
        //        Expect.Call(_dataTable.Where(null)).IgnoreArguments();
        //    }
        //    using (_mocks.Playback())
        //    {
        //        var ugh = TestStockLocationRepository.SelectActiveByOperatingCenter(nj4StockLocation.OperatingCenter.OperatingCenterID);
        //        Assert.AreEqual(1, ugh.Count());
        //    }
        //}

        [TestMethod]
        public void TestGetFilteredDataFiltersData()
        {
            // needed because ExpressionBuilder#And(expr) will not accept a null value
            var expr = PredicateBuilder.True<StockLocation>();

            using (_mocks.Record())
            {
                SetupResult.For(_securityService.UserOperatingCenters).Return(
                    new[] {
                        nj7
                    });
                Expect.Call(_dataTable.Where(null)).IgnoreArguments().Callback(
                    new TestExpressionDelegate(TestGeneratedExpression)).Return(null);
            }

            using (_mocks.Playback())
            {
                _target.ExposedGetFilteredData(expr);
            }
        }

        private bool TestGeneratedExpression(Expression<Func<StockLocation, bool>> expr)
        {
            var fn = expr.Compile();

            Assert.IsTrue(fn(nj7StockLocation),
                "The configured nj7 crew should pass the requirement of the search filter by op center");
            Assert.IsFalse(fn(nj4StockLocation),
                "The configured nj4 crew should fail the requirement of the search filter by op center");
            Assert.IsFalse(fn(locstock2smkbarrels),
                "A crew with no operating center at all certainly shouldn't pass.  I don't recall why I put it in here.");

            return true;
        }
    }

    internal class TestStockLocationRepositoryBuilder : TestDataBuilder<TestStockLocationRepository>
    {
        #region Private Members

        private ISecurityService _securityService;
        private ITable<StockLocation> _dataTable;

        #endregion

        #region Exposed Methods

        public override TestStockLocationRepository Build()
        {
            var obj = new TestStockLocationRepository();
            if (_dataTable != null)
                obj.SetDataTable(_dataTable);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            return obj;
        }

        public TestStockLocationRepositoryBuilder WithDataTable(ITable<StockLocation> dataTable)
        {
            _dataTable = dataTable;
            return this;
        }

        public TestStockLocationRepositoryBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }
        #endregion
    }

    internal class TestStockLocationRepository : StockLocationRepository
    {
        #region Exposed Methods

        public void SetDataTable(ITable<StockLocation> dataTable)
        {
            _dataTable = dataTable;
        }

        public void SetSecurityService(ISecurityService service)
        {
            GetType().SetHiddenStaticFieldValueByName("_securityService", service);
        }

        public IEnumerable<StockLocation> ExposedGetFilteredData(Expression<Func<StockLocation, bool>> expr)
        {
            return GetFilteredData(expr);
        }

        public void Dispose()
        {
            // needs to happen because _securityService is static, and that would suck.
            SetSecurityService(null);
        }

        #endregion
    }
}
