using System;
using System.Linq.Expressions;
using LINQTo271.Common;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.Abstract
{
    /// <summary>
    /// Summary description for WorkOrderApprovalSearchViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderApprovalSearchViewTest : EventFiringTestClass
    {
        #region Private Members

        private IDateRange _dateRange;
        private IDropDownList _ddlCrew;
        private TestWorkOrderApprovalSearchView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _dateRange)
                .DynamicMock(out _ddlCrew);

            _target = new TestWorkOrderApprovalSearchViewBuilder()
                .WithDateRange(_dateRange)
                .WithDDLCrew(_ddlCrew);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestPhasePropertyDenotesApproval()
        {
            Assert.AreEqual(WorkOrderPhase.Approval, _target.Phase);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestAssetTypeIDOverriddenToReturnNull()
        {
            Assert.IsNull(_target.AssetTypeID);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDescriptionOfWorkIDsOverriddenToReturnNull()
        {
            Assert.IsNull(_target.DescriptionOfWorkIDs);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestNearestCrossStreetIDOverriddenToReturnNull()
        {
            Assert.IsNull(_target.NearestCrossStreetID);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestStreetIDOverriddenToReturnNull()
        {
            Assert.IsNull(_target.StreetID);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestStreetNumberOverriddenToReturnNull()
        {
            Assert.IsNull(_target.StreetNumber);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestTownSectionIDOverriddenToReturnNull()
        {
            Assert.IsNull(_target.TownSectionID);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestBaseExpressionPropertyReturnsGeneratedBaseExpression()
        {
            Expression<Func<WorkOrder, bool>> expr = w => false;
            _target = new TestWorkOrderApprovalSearchViewBuilder()
                .WithBaseExpression(expr);

            Assert.AreSame(expr, _target.BaseExpression);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDateCompletedReturnsDateValueFromDateRange()
        {
            var expected = DateTime.Now;

            using (_mocks.Record())
            {
                SetupResult.For(_dateRange.Date).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DateCompleted.Value);
            }
        }

        [TestMethod]
        public void TestDateCompletedStartReturnsStartDateValueFromDateRange()
        {
            var expected = DateTime.Now;

            using (_mocks.Record())
            {
                SetupResult.For(_dateRange.StartDate).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DateCompletedStart.Value);
            }
        }

        [TestMethod]
        public void TestDateCompletedEndReturnsEndDateValueFromDateRange()
        {
            var expected = DateTime.Now;

            using (_mocks.Record())
            {
                SetupResult.For(_dateRange.EndDate).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DateCompletedEnd.Value);
            }
        }

        [TestMethod]
        public void TestCrewIDPropertyReturnsSelectedValueFromCrewDropDown()
        {
            var expected = 1;

            using (_mocks.Record())
            {
                SetupResult.For(_ddlCrew.GetSelectedValue()).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.CrewID);
            }
        }
    }

    internal class TestWorkOrderApprovalSearchViewBuilder : TestDataBuilder<TestWorkOrderApprovalSearchView>
    {
        #region Private Members

        private bool? _isPostBack;
        private IDateRange _dateRange;
        private IDropDownList _ddlCrew;
        private Expression<Func<WorkOrder, bool>> _baseExpression;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderApprovalSearchView Build()
        {
            var obj = new TestWorkOrderApprovalSearchView();
            if (_baseExpression != null)
                obj.SetBaseExpression(_baseExpression);
            if (_dateRange != null)
                obj.SetDRDateCompleted(_dateRange);
            if (_ddlCrew != null)
                obj.SetDDLCrew(_ddlCrew);
            if (_isPostBack != null)
                obj.SetPostBack(_isPostBack.Value);
            return obj;
        }

        public TestWorkOrderApprovalSearchViewBuilder WithBaseExpression(Expression<Func<WorkOrder, bool>> expr)
        {
            _baseExpression = expr;
            return this;
        }

        public TestWorkOrderApprovalSearchViewBuilder WithDateRange(IDateRange dateRange)
        {
            _dateRange = dateRange;
            return this;
        }

        public TestWorkOrderApprovalSearchViewBuilder WithDDLCrew(IDropDownList ddl)
        {
            _ddlCrew = ddl;
            return this;
        }

        public TestWorkOrderApprovalSearchViewBuilder WithIsPostBack(bool isPostBack)
        {
            _isPostBack = isPostBack;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderApprovalSearchView : WorkOrderApprovalSearchView
    {
        #region Overrides of WorkOrderSearchView

        protected override void ApplySearchFilters(ExpressionBuilder<WorkOrder> builder)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        protected override Expression<Func<WorkOrder, bool>> GetBaseExpression()
        {
            return _baseExpression;
        }

        #endregion

        #region Exposed Methods

        public void SetBaseExpression(Expression<Func<WorkOrder, bool>> expr)
        {
            _baseExpression = expr;
        }

        public void SetDRDateCompleted(IDateRange dateRange)
        {
            drDateCompleted = dateRange;
        }

        public void SetDDLCrew(IDropDownList ddl)
        {
            ddlCrew = ddl;
        }

        public void SetPostBack(bool isPostBack)
        {
            _isMvpPostBack = isPostBack;
        }

        #endregion
    }
}
