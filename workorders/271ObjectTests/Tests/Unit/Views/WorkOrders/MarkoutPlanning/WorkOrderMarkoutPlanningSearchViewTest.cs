using System;
using System.Collections.Generic;
using System.Linq;
using LINQTo271.Common;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.WorkOrders.MarkoutPlanning;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.MarkoutPlanning
{
    [TestClass]
    public class WorkOrderMarkoutPlanningSearchViewTest : EventFiringTestClass
    {
        #region Private Members

        private TestWorkOrderMarkoutPlanningSearchView _target;

        private IBaseWorkOrderSearch baseSearch;
        protected IDropDownList ddlPriority,
                                ddlRequestedBy,
                                ddlSOPRequirement,
                                ddlDrivenBy,
                                ddlMarkoutRequirement,
                                ddlCreatedBy,
                                ddlStreetOpeningPermitRequested,
                                ddlStreetOpeningPermitIssued;

        protected IDateRange drDateReceived;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out baseSearch)
                .DynamicMock(out drDateReceived)
                .DynamicMock(out ddlRequestedBy)
                .DynamicMock(out ddlMarkoutRequirement)
                .DynamicMock(out ddlSOPRequirement)
                .DynamicMock(out ddlPriority)
                .DynamicMock(out ddlDrivenBy)
                .DynamicMock(out ddlCreatedBy)
                .DynamicMock(out ddlStreetOpeningPermitRequested)
                .DynamicMock(out ddlStreetOpeningPermitIssued);

            _target = new TestWorkOrderMarkoutPlanningSearchViewBuilder()
                .WithBaseSearchControl(baseSearch)
                .WithDRDateReceived(drDateReceived)
                .WithDDLRequestedBy(ddlRequestedBy)
                .WithDDLMarkoutRequirement(ddlMarkoutRequirement)
                .WithDDLSOPRequirement(ddlSOPRequirement)
                .WithDDLPriority(ddlPriority)
                .WithDDLDrivenBy(ddlDrivenBy)
                .WithDDLCreatedBy(ddlCreatedBy)
                .WithDDLStreetOpeningPermitRequested(ddlStreetOpeningPermitRequested)
                .WithDDLStreetOpeningPermitIssued(ddlStreetOpeningPermitIssued)
                .Build();
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Tests

        #region Property Tests

        [TestMethod]
        public void TestDateReceivedReturnsValueOfDRDateReceived()
        {
            var expected = new DateTime(2000, 1, 1);

            using (_mocks.Record())
            {
                SetupResult.For(drDateReceived.Date).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DateReceived);
            }
        }

        // TODO: Overkill? - rest of the properties, methods, etc.

        [TestMethod]
        public void TestSopRequestedAndIssuedPropertyReturnsBooleanValue()
        {
            var expected = true;

            using (_mocks.Record())
            {
                SetupResult.For(ddlStreetOpeningPermitRequested.GetBooleanValue()).
                            Return(expected);
                SetupResult.For(ddlStreetOpeningPermitIssued.GetBooleanValue()).
                            Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.StreetOpeningPermitRequested);
                Assert.AreEqual(expected, _target.StreetOpeningPermitIssued);
            }
        }

        #endregion

        [TestMethod]
        public void TestBaseExpressionFiltersOutRecordsWhereDateCompletedIsNull()
        {
            var baseExpression = _target.BaseExpression.Compile();

            Assert.IsFalse(baseExpression(new WorkOrder
            {
                OperatingCenter = new OperatingCenter(),
                DateCompleted = DateTime.Now
            }));
            Assert.IsTrue(baseExpression(new WorkOrder
            {
                OperatingCenter = new OperatingCenter(),
                DateCompleted = null
            }));

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestBaseExpressionFiltersOutRecordsAssignedToContractors()
        {
            var baseExpression = _target.BaseExpression.Compile();

            Assert.IsFalse(baseExpression(new WorkOrder
            {
                OperatingCenter = new OperatingCenter(),
                AssignedContractorID = 1
            }));

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestBaseExpressionDoesNotReturnWorkOrderWithMarkoutRequirementNone()
        {
            var baseExpression = _target.BaseExpression.Compile();

            Assert.IsFalse(baseExpression(new WorkOrder {
                OperatingCenter = new OperatingCenter(),
                DateCompleted = null,
                MarkoutRequirementID = MarkoutRequirementRepository.Indices.NONE,
            }), "Should not return a work order that has no markout requirement.");

            _mocks.ReplayAll();
        }
        
        [TestMethod]
        public void TestBaseExpressionReturnsARoutineMarkoutWithNoSOPRequired()
        {
            var baseExpression = _target.BaseExpression.Compile();

            Assert.IsTrue(baseExpression(new WorkOrder
            {
                OperatingCenter = new OperatingCenter(),
                DateCompleted = null,
                MarkoutRequirementID = MarkoutRequirementRepository.Indices.ROUTINE,
                StreetOpeningPermitRequired = false
            }));

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestBaseExpressionDoesNotReturnWorkOrderWithStreetOpeningPermitWithoutAValidStreetOpeningPermit()
        {
            var baseExpression = _target.BaseExpression.Compile();

            var workOrderWithValidSOP = new WorkOrder {
                OperatingCenter = new OperatingCenter(),
                DateCompleted = null,
                MarkoutRequirementID =
                    MarkoutRequirementRepository.Indices.ROUTINE,
                StreetOpeningPermitRequired = true
            };
            workOrderWithValidSOP.StreetOpeningPermits.Add(
                new StreetOpeningPermit {
                    DateIssued = DateTime.Now.AddMinutes(-1),
                    ExpirationDate = DateTime.Now.AddDays(1)
                });
            Assert.IsTrue(baseExpression(workOrderWithValidSOP),
                "Should return a WorkOrder with required sop that has a valid sop.");

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestBaseExpressionReturnsWorkOrderWithStreetOpeningPermitWithAmExpiredStreetOpeningPermit()
        {
            var baseExpression = _target.BaseExpression.Compile();
            
            var workOrderWithExpiredSOP = new WorkOrder
            {
                OperatingCenter = new OperatingCenter(),
                DateCompleted = null,
                MarkoutRequirementID = MarkoutRequirementRepository.Indices.ROUTINE,
                StreetOpeningPermitRequired = true
            };
            workOrderWithExpiredSOP.StreetOpeningPermits.Add(new StreetOpeningPermit
            {
                DateIssued = DateTime.Now.AddDays(-1),
                ExpirationDate = DateTime.Now.AddDays(-1)
            });
            Assert.IsFalse(baseExpression(workOrderWithExpiredSOP), "Should not return a WorkOrder with required sop that has expired.");

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestBaseExpressionExcludesSAPWorkOrdersWithOutSAPWorkOrderNumbers()
        {
            _mocks.ReplayAll();
            var operatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true};
            WorkOrder woInvalidSapOrder = new TestWorkOrderBuilder().BuildIncompleteOrder()
                .WithOperatingCenter(operatingCenter);

            IEnumerable<WorkOrder> orders = new[] { woInvalidSapOrder };

            var result = orders.Where(_target.BaseExpression.Compile()).ToList();

            Assert.IsFalse(result.Contains(woInvalidSapOrder));
        }
        
        #endregion
    }

    internal class TestWorkOrderMarkoutPlanningSearchViewBuilder :
        TestDataBuilder<TestWorkOrderMarkoutPlanningSearchView>
    {
        private IBaseWorkOrderSearch _baseSearch;
        protected IDropDownList _ddlPriority,
                                _ddlRequestedBy,
                                _ddlSOPRequirement,
                                _ddlDrivenBy,
                                _ddlMarkoutRequirement,
                                _ddlCreatedBy,
                                _ddlStreetOpeningPermitRequested,
                                _ddlStreetOpeningPermitIssued;


        protected IDateRange _drDateReceived;

        public override TestWorkOrderMarkoutPlanningSearchView Build()
        {
            var obj = new TestWorkOrderMarkoutPlanningSearchView();
            if (_drDateReceived != null)
                obj.SetDRDateReceived(_drDateReceived);
            if (_baseSearch != null)
                obj.SetBaseSearch(_baseSearch);
            if (_ddlRequestedBy != null)
                obj.SetDDLRequestedBy(_ddlRequestedBy);
            if (_ddlMarkoutRequirement != null)
                obj.SetDDLMarkoutRequirement(_ddlMarkoutRequirement);
            if (_ddlSOPRequirement != null)
                obj.SetDDLSOPRequirement(_ddlSOPRequirement);
            if (_ddlPriority != null)
                obj.SetDDLPriority(_ddlPriority);
            if (_ddlDrivenBy != null)
                obj.SetDDLDrivenBy(_ddlDrivenBy);
            if (_ddlCreatedBy != null)
                obj.SetDDLCreatedBy(_ddlCreatedBy);
            if (_ddlStreetOpeningPermitRequested != null)
                obj.SetDDLStreetOpeningPermitRequested(_ddlStreetOpeningPermitRequested);
            if (_ddlStreetOpeningPermitIssued != null)
                obj.SetDDLStreetOpeningPermitIssued(_ddlStreetOpeningPermitIssued);
            return obj;
        }

        public TestWorkOrderMarkoutPlanningSearchViewBuilder WithDRDateReceived(IDateRange dateReceived)
        {
            _drDateReceived = dateReceived;
            return this;
        }

        public TestWorkOrderMarkoutPlanningSearchViewBuilder WithBaseSearchControl(IBaseWorkOrderSearch ctrl)
        {
            _baseSearch = ctrl;
            return this;
        }

        public TestWorkOrderMarkoutPlanningSearchViewBuilder WithDDLRequestedBy(IDropDownList dropDownList)
        {
            _ddlRequestedBy = dropDownList;
            return this;
        }

        public TestWorkOrderMarkoutPlanningSearchViewBuilder WithDDLMarkoutRequirement(IDropDownList dropDownList)
        {
            _ddlMarkoutRequirement = dropDownList;
            return this;
        }

        public TestWorkOrderMarkoutPlanningSearchViewBuilder WithDDLSOPRequirement(IDropDownList dropDownList)
        {
            _ddlSOPRequirement = dropDownList;
            return this;
        }

        public TestWorkOrderMarkoutPlanningSearchViewBuilder WithDDLPriority(IDropDownList dropDownList)
        {
            _ddlPriority = dropDownList;
            return this;
        }

        public TestWorkOrderMarkoutPlanningSearchViewBuilder WithDDLDrivenBy(IDropDownList dropDownList)
        {
            _ddlDrivenBy = dropDownList;
            return this;
        }

        public TestWorkOrderMarkoutPlanningSearchViewBuilder WithDDLCreatedBy(IDropDownList dropDownList)
        {
            _ddlCreatedBy = dropDownList;
            return this;
        }

        public TestWorkOrderMarkoutPlanningSearchViewBuilder WithDDLStreetOpeningPermitRequested(IDropDownList dropDownList)
        {
            _ddlStreetOpeningPermitRequested = dropDownList;
            return this;
        }

        public TestWorkOrderMarkoutPlanningSearchViewBuilder WithDDLStreetOpeningPermitIssued(IDropDownList dropDownList)
        {
            _ddlStreetOpeningPermitIssued = dropDownList;
            return this;
        }
    }

    internal class TestWorkOrderMarkoutPlanningSearchView :
        WorkOrderMarkoutPlanningSearchView
    {
        internal void SetDRDateReceived(IDateRange dateReceived)
        {
            drDateReceived = dateReceived;
        }

        internal void SetBaseSearch(IBaseWorkOrderSearch ctrl)
        {
            baseSearch = ctrl;
        }

        internal void SetDDLRequestedBy(IDropDownList dropDownList)
        {
            ddlRequestedBy = dropDownList;
        }

        internal void SetDDLMarkoutRequirement(IDropDownList dropDownList)
        {
            ddlMarkoutRequirement = dropDownList;
        }

        internal void SetDDLSOPRequirement(IDropDownList dropDownList)
        {
            ddlSOPRequirement = dropDownList;
        }

        internal void SetDDLPriority(IDropDownList dropDownList)
        {
            ddlPriority = dropDownList;
        }

        internal void SetDDLDrivenBy(IDropDownList dropDownList)
        {
            ddlDrivenBy = dropDownList;
        }

        internal void SetDDLCreatedBy(IDropDownList ddl)
        {
            ddlCreatedBy = ddl;
        }

        internal void SetDDLStreetOpeningPermitRequested(IDropDownList ddl)
        {
            ddlStreetOpeningPermitRequested = ddl;
        }

        internal void SetDDLStreetOpeningPermitIssued(IDropDownList ddl)
        {
            ddlStreetOpeningPermitIssued = ddl;
        }
    }
}