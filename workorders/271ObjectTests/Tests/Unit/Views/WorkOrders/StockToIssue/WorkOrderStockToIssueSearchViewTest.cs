using System;
using System.Collections.Generic;
using System.Linq;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.WorkOrders.StockToIssue;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.StockToIssue
{
    /// <summary>
    /// Summary description for WorkOrderStockToIssueSearchViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderStockToIssueSearchViewTest : EventFiringTestClass
    {
        #region Private Members

        private IBaseWorkOrderSearch _baseSearch;
        private IDropDownList _ddlMaterialsApproved;
        private TestWorkOrderStockToIssueSearchView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _ddlMaterialsApproved)
                .DynamicMock(out _baseSearch);

            _target = new TestWorkOrderStockToIssueSearchViewBuilder()
                .WithDDLMaterialsApproved(_ddlMaterialsApproved)
                .WithBaseSearch(_baseSearch);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestPhasePropertyDenotesStockApproval()
        {
            Assert.AreEqual(WorkOrderPhase.StockApproval, _target.Phase);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestBaseExpressionFiltersOutRecordsNotApprovedOrWithNoMaterials()
        {
            var baseExpression = _target.BaseExpression.Compile();

            // not approved, no materials
            Assert.IsFalse(baseExpression(new WorkOrder() { OperatingCenter = new OperatingCenter() }));
            // not approved, has materials
            Assert.IsFalse(baseExpression(new WorkOrder {
                OperatingCenter = new OperatingCenter(),
                MaterialsUseds = {
                    new MaterialsUsed()
                }
            }));
            // approved, no materials
            Assert.IsFalse(baseExpression(new WorkOrder {
                OperatingCenter = new OperatingCenter(),
                ApprovedBy = new Employee()
            }));
            // approved, has materials
            Assert.IsTrue(baseExpression(new WorkOrder {
                OperatingCenter = new OperatingCenter(),
                ApprovedBy = new Employee(),
                MaterialsUseds = {
                    new MaterialsUsed()
                }
            }));

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestMaterialsApprovedPropertyReturnsBooleanValueOfMaterialsApprovedDropDown()
        {
            var values = new bool?[] {
                true, false, null
            };

            foreach (var value in values)
            {
                using (_mocks.Record())
                {
                    SetupResult.For(_ddlMaterialsApproved.GetBooleanValue()).Return(value);
                }

                using (_mocks.Playback())
                {
                    Assert.AreEqual(value, _target.MaterialsApproved);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestGeneratedExpressionFiltersByMaterialApprovalStatusIfValueChosen()
        {
            Func<WorkOrder, bool> expression;
            
            WorkOrder withApproval = new WorkOrder {
                          OperatingCenter = new OperatingCenter(),
                          ApprovedBy = new Employee(),
                          MaterialsUseds = {
                              new MaterialsUsed()
                          },
                          MaterialsApprovedBy = new Employee()
                      },
                      withoutApproval = new WorkOrder {
                          OperatingCenter = new OperatingCenter(),
                          ApprovedBy = new Employee(),
                          MaterialsUseds = {
                              new MaterialsUsed()
                          },
                      };

            using (_mocks.Record())
            {
                SetupResult.For(_ddlMaterialsApproved.GetBooleanValue()).Return(
                    true);
            }

            using (_mocks.Playback())
            {
                expression = _target.GenerateExpression().Compile();

                Assert.IsTrue(expression(withApproval));
                Assert.IsFalse(expression(withoutApproval));
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlMaterialsApproved.GetBooleanValue()).Return(
                    false);
            }

            using (_mocks.Playback())
            {
                expression = _target.GenerateExpression().Compile();

                Assert.IsFalse(expression(withApproval));
                Assert.IsTrue(expression(withoutApproval));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionDoesNotFilterByMaterialApprovalStatusIfNoValueChosen()
        {
            WorkOrder withApproval = new WorkOrder {
                         OperatingCenter = new OperatingCenter(),
                         ApprovedBy = new Employee(),
                         MaterialsUseds = {
                             new MaterialsUsed()
                         },
                         MaterialsApprovedBy = new Employee()
                      },
                      withoutApproval = new WorkOrder {
                          OperatingCenter = new OperatingCenter(),
                          ApprovedBy = new Employee(),
                          MaterialsUseds = {
                              new MaterialsUsed()
                          },
                      };
            
            using (_mocks.Record())
            {
                SetupResult.For(_ddlMaterialsApproved.GetBooleanValue()).Return(null);
            }

            using (_mocks.Playback())
            {
                var expression = _target.GenerateExpression().Compile();

                Assert.IsTrue(expression(withApproval));
                Assert.IsTrue(expression(withoutApproval));
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

        #endregion
    }

    internal class TestWorkOrderStockToIssueSearchViewBuilder : TestDataBuilder<TestWorkOrderStockToIssueSearchView>
    {
        #region Private Members

        private IDropDownList _ddlMaterialsApproved;
        private IBaseWorkOrderSearch _baseSearch;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderStockToIssueSearchView Build()
        {
            var obj = new TestWorkOrderStockToIssueSearchView();
            if (_ddlMaterialsApproved != null)
                obj.SetDDLMaterialsApproved(_ddlMaterialsApproved);
            if (_baseSearch != null)
                obj.SetBaseSearch(_baseSearch);
            return obj;
        }

        public TestWorkOrderStockToIssueSearchViewBuilder WithDDLMaterialsApproved(IDropDownList ddlMaterialsApproved)
        {
            _ddlMaterialsApproved = ddlMaterialsApproved;
            return this;
        }

        public TestWorkOrderStockToIssueSearchViewBuilder WithBaseSearch(IBaseWorkOrderSearch baseSearch)
        {
            _baseSearch = baseSearch;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderStockToIssueSearchView : WorkOrderStockToIssueSearchView
    {
        #region Exposed Methods

        public void SetDDLMaterialsApproved(IDropDownList ddl)
        {
            ddlMaterialsApproved = ddl;
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
