using System;
using System.Collections.Generic;
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
    /// Summary description for CrewRepositoryTest
    /// </summary>
    [TestClass]
    public class CrewRepositoryTest : EventFiringTestClass
    {
        #region Private Members

        private TestCrewRepository _target;
        private ITable<Crew> _dataTable;
        private ISecurityService _securityService;
        private OperatingCenter nj4, nj7;
        private Crew nj4Crew, nj7Crew, twoLiveCrew;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _dataTable)
                .DynamicMock(out _securityService);

            CreateSampleData();

            _target = new TestCrewRepositoryBuilder()
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
            nj7Crew = new Crew {
                OperatingCenter = nj7
            };
            nj4Crew = new Crew {
                OperatingCenter = nj4
            };
            twoLiveCrew = new Crew();
        }

        #endregion

        #region Delegates

        private delegate bool TestExpressionDelegate(Expression<Func<Crew, bool>> expr);

        #endregion

        [TestMethod]
        public void TestGetFilteredDataFiltersData()
        {
            // needed because ExpressionBuilder#And(expr) will not accept a null value
            var expr = PredicateBuilder.True<Crew>();

            using(_mocks.Record())
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

        private bool TestGeneratedExpression(Expression<Func<Crew, bool>> expr)
        {
            var fn = expr.Compile();

            Assert.IsTrue(fn(nj7Crew),
                "The configured nj7 crew should pass the requirement of the search filter by op center");
            Assert.IsFalse(fn(nj4Crew),
                "The configured nj4 crew should fail the requirement of the search filter by op center");
            Assert.IsFalse(fn(twoLiveCrew),
                "A crew with no operating center at all certainly shouldn't pass.  I don't recall why I put it in here.");

            return true;
        }
    }

    internal class TestCrewRepositoryBuilder : TestDataBuilder<TestCrewRepository>
    {
        #region Private Members

        private ISecurityService _securityService;
        private ITable<Crew> _dataTable;

        #endregion

        #region Exposed Methods

        public override TestCrewRepository Build()
        {
            var obj = new TestCrewRepository();
            if (_dataTable != null)
                obj.SetDataTable(_dataTable);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            return obj;
        }

        public TestCrewRepositoryBuilder WithDataTable(ITable<Crew> dataTable)
        {
            _dataTable = dataTable;
            return this;
        }

        public TestCrewRepositoryBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        #endregion
    }

    internal class TestCrewRepository : CrewRepository, IDisposable
    {
        #region Exposed Methods

        public void SetDataTable(ITable<Crew> dataTable)
        {
            _dataTable = dataTable;
        }

        public void SetSecurityService(ISecurityService service)
        {
            GetType().SetHiddenStaticFieldValueByName("_securityService", service);
        }

        public IEnumerable<Crew> ExposedGetFilteredData(Expression<Func<Crew, bool>> expr)
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