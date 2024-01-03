using System;
using System.Collections.Generic;
using System.Linq;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.WorkOrders.SupervisorApproval;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.SupervisorApproval
{
    /// <summary>
    /// Summary description for WorkOrderSupervisorApprovalSearchViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderSupervisorApprovalSearchViewTest : EventFiringTestClass
    {
        #region Private Members

        private IBaseWorkOrderSearch _baseSearch;
        private IDropDownList _ddlApproved, _ddlRequestedBy;
        private IListBox _lstDrivenBy;
        private TestWorkOrderSupervisorApprovalSearchView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
               .DynamicMock(out _ddlApproved)
               .DynamicMock(out _lstDrivenBy)
               .DynamicMock(out _baseSearch)
               .DynamicMock(out _ddlRequestedBy);

            _target = new TestWorkOrderSupervisorApprovalSearchViewBuilder()
                     .WithDDLApproved(_ddlApproved)
                     .WithLSTDrivenBy(_lstDrivenBy)
                     .WithBaseSearch(_baseSearch)
                     .WithDDLRequestedBy(_ddlRequestedBy);
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
        public void TestBaseExpressionFiltersOutRecordsWhereDateCompletedIsNull()
        {
            var baseExpression = _target.BaseExpression.Compile();

            Assert.IsTrue(baseExpression(new WorkOrder {
                DateCompleted = DateTime.Now, OperatingCenter = new OperatingCenter {  SAPEnabled = false }
            }));
            Assert.IsFalse(baseExpression(new WorkOrder {
                DateCompleted = null,
                OperatingCenter = new OperatingCenter { SAPEnabled = false }
            }));

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestApprovedPropertyReturnsBooleanValueOfApprovedDropDown()
        {
            var values = new bool?[] {
                true, false, null
            };

            foreach (var value in values)
            {
                using (_mocks.Record())
                {
                    SetupResult.For(_ddlApproved.GetBooleanValue()).Return(value);
                }

                using (_mocks.Playback())
                {
                    Assert.AreEqual(value, _target.Approved);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestGeneratedExpressionFiltersByApprovalStatusIfApprovalValueChosen()
        {
            Func<WorkOrder, bool> expression;
            var operatingCenter = new OperatingCenter{
                SAPEnabled = false
            };
            WorkOrder withApprovedBy = new WorkOrder {
                    ApprovedBy = new Employee(),
                    DateCompleted = DateTime.Today,
                    OperatingCenter = operatingCenter
                },
                withoutApprovedBy = new WorkOrder {
                    DateCompleted = DateTime.Today,
                    OperatingCenter = operatingCenter
                };

            using (_mocks.Record())
            {
                SetupResult.For(_ddlApproved.GetBooleanValue()).Return(true);
            }

            using (_mocks.Playback())
            {
                expression = _target.GenerateExpression().Compile();

                Assert.IsTrue(expression(withApprovedBy));
                Assert.IsFalse(expression(withoutApprovedBy));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlApproved.GetBooleanValue()).Return(false);
            }

            using (_mocks.Playback())
            {
                expression = _target.GenerateExpression().Compile();

                Assert.IsFalse(expression(withApprovedBy));
                Assert.IsTrue(expression(withoutApprovedBy));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionDoesNotFilterByApprovalStatusIfNoApprovalValueChosen()
        {
            var operatingCenter = new OperatingCenter {
                SAPEnabled = false
            };
            WorkOrder withApprovedBy = new WorkOrder {
                          ApprovedBy = new Employee(),
                          DateCompleted = DateTime.Today,
                          OperatingCenter = operatingCenter
                      },
                      withoutApprovedBy = new WorkOrder {
                          DateCompleted = DateTime.Today,
                          OperatingCenter = operatingCenter
                      };

            using (_mocks.Record())
            {
                SetupResult.For(_ddlApproved.GetBooleanValue()).Return(null);
            }

            using (_mocks.Playback())
            {
                var expression = _target.GenerateExpression().Compile();

                Assert.IsTrue(expression(withApprovedBy));
                Assert.IsTrue(expression(withoutApprovedBy));
            }
        }

        [TestMethod]
        public void TestBaseExpressionExcludesSAPWorkOrdersWithOutSAPWorkOrderNumbers()
        {
            _mocks.ReplayAll();
            var operatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true };
            WorkOrder woInvalidSapOrder = new TestWorkOrderBuilder().BuildIncompleteOrder()
                .WithOperatingCenter(operatingCenter);

            IEnumerable<WorkOrder> orders = new[] { woInvalidSapOrder };

            var result = orders.Where(_target.BaseExpression.Compile()).ToList();

            Assert.IsFalse(result.Contains(woInvalidSapOrder));
        }
    }

    internal class TestWorkOrderSupervisorApprovalSearchViewBuilder : TestDataBuilder<TestWorkOrderSupervisorApprovalSearchView>
    {
        #region Private Members

        private IBaseWorkOrderSearch _baseSearch;
        private IDropDownList _ddlApproved, _ddlRequestedBy;
        private IListBox _lstDrivenBy;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderSupervisorApprovalSearchView Build()
        {
            var obj = new TestWorkOrderSupervisorApprovalSearchView();
            if (_baseSearch != null)
                obj.SetBaseSearch(_baseSearch);
            if (_ddlApproved != null)
                obj.SetDDLApproved(_ddlApproved);
            if (_lstDrivenBy != null)
                obj.SetLSTDrivenBy(_lstDrivenBy);
            if (_ddlRequestedBy != null)
                obj.SetDDLRequestedBy(_ddlRequestedBy);
            return obj;
        }

        public TestWorkOrderSupervisorApprovalSearchViewBuilder WithDDLApproved(IDropDownList ddlApproved)
        {
            _ddlApproved = ddlApproved;
            return this;
        }

        public TestWorkOrderSupervisorApprovalSearchViewBuilder WithLSTDrivenBy(IListBox lstDrivenBy)
        {
            _lstDrivenBy = lstDrivenBy;
            return this;
        }

        public TestWorkOrderSupervisorApprovalSearchViewBuilder WithBaseSearch(IBaseWorkOrderSearch baseSearch)
        {
            _baseSearch = baseSearch;
            return this;
        }

        #endregion

        public TestWorkOrderSupervisorApprovalSearchView WithDDLRequestedBy(IDropDownList ddlRequestedBy)
        {
            _ddlRequestedBy = ddlRequestedBy;
            return this;
        }
    }

    internal class TestWorkOrderSupervisorApprovalSearchView : WorkOrderSupervisorApprovalSearchView
    {
        #region Exposed Methods

        public void SetDDLApproved(IDropDownList ddl)
        {
            ddlApproved = ddl;
        }

        public void SetLSTDrivenBy(IListBox lb)
        {
            lstDrivenBy = lb;
        }
        public void SetDDLRequestedBy(IDropDownList ddl)
        {
            ddlRequestedBy = ddl;
        }

        public void SetBaseSearch(IBaseWorkOrderSearch ctrl)
        {
            baseSearch = ctrl;
        }

        #endregion

        #region Overrides

        public override int? CrewID
        {
            get { return null; }
        }

        public override DateTime? DateCompleted
        {
            get { return null; }
        }

        public override DateTime? DateCompletedStart
        {
            get { return null; }
        }

        public override DateTime? DateCompletedEnd
        {
            get { return null; }
        }

        #endregion
    }
}
