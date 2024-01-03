using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.DesignPatterns.Mvp.Common;
using MMSINC.DesignPatterns.Mvp.Model;
using MMSINC.Testing;
using MMSINC.Testing.TestExtensions;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for SpoilRemovalRepositoryTest.
    /// </summary>
    [TestClass]
    public class SpoilRemovalRepositoryTest : EventFiringTestClass
    {
        #region Private Members

        private TestSpoilRemovalRepository _target;
        private ITable<SpoilRemoval> _dataTable;
        private ISecurityService _securityService;
        private OperatingCenter nj4, nj7;
        private SpoilStorageLocation nj4location, nj7location;
        private SpoilRemoval nj4SpoilRemoval, nj7SpoilRemoval;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void SpoilRemovalRepositoryTestInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _dataTable)
                .DynamicMock(out _securityService);

            CreateSampleData();

            _target = new TestSpoilRemovalRepositoryBuilder()
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
            nj7 = new OperatingCenter
            {
                OpCntrName = "NJ7",
                OperatingCenterID = 10
            };
            nj4 = new OperatingCenter
            {
                OpCntrName = "NJ4",
                OperatingCenterID = 14
            };

            nj7location = new SpoilStorageLocation {
                OperatingCenter = nj7
            };

            nj4location = new SpoilStorageLocation {
                OperatingCenter = nj4
            };

            nj4SpoilRemoval = new SpoilRemoval {
                RemovedFrom = nj4location
            };
            nj7SpoilRemoval = new SpoilRemoval {
                RemovedFrom = nj7location
            };
        }

        #endregion

        #region Delegates

        private delegate bool TestExpressionDelegate(Expression<Func<SpoilRemoval, bool>> expr);

        #endregion

        [TestMethod]
        public void TestGetFilteredDataFiltersData()
        {
            // needed because ExpressionBuilder#And(expr) will not accept a null value
            var expr = PredicateBuilder.True<SpoilRemoval>();

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

        private bool TestGeneratedExpression(Expression<Func<SpoilRemoval, bool>> expr)
        {
            var fn = expr.Compile();

            Assert.IsTrue(fn(nj7SpoilRemoval),
                "The configured nj7 spoil removal should pass the requirement of the search filter by op center");
            Assert.IsFalse(fn(nj4SpoilRemoval),
                "The configured nj4 spoil removal should fail the requirement of the search filter by op center");
            
            return true;
        }
    }

    internal class TestSpoilRemovalRepositoryBuilder : TestDataBuilder<TestSpoilRemovalRepository>
    {
        #region Private Members

        private ISecurityService _securityService;
        private ITable<SpoilRemoval> _dataTable;

        #endregion

        #region Exposed Methods

        public override TestSpoilRemovalRepository Build()
        {
            var obj = new TestSpoilRemovalRepository();
            if (_dataTable != null)
                obj.SetDataTable(_dataTable);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            return obj;
        }

        public TestSpoilRemovalRepositoryBuilder WithDataTable(ITable<SpoilRemoval> dataTable)
        {
            _dataTable = dataTable;
            return this;
        }

        public TestSpoilRemovalRepositoryBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        #endregion
    }

    internal class TestSpoilRemovalRepository : SpoilRemovalRepository
    {
        #region Exposed Methods

        public void SetDataTable(ITable<SpoilRemoval> dataTable)
        {
            _dataTable = dataTable;
        }

        public void SetSecurityService(ISecurityService service)
        {
            _securityService = service;
        }

        public IEnumerable<SpoilRemoval> ExposedGetFilteredData(Expression<Func<SpoilRemoval, bool>> expr)
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
