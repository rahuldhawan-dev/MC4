using System;
using System.Collections.Generic;
using System.Linq;
using LINQTo271.Views.WorkOrders.SOPProcessing;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.SOPProcessing
{
    /// <summary>
    /// Summary description for SOPProcessingSearchViewTest.
    /// </summary>
    [TestClass]
    public class SOPProcessingSearchViewTest : EventFiringTestClass
    {
        #region Private Members

        private TestSOPProcessingSearchView _target;
        protected IDropDownList _ddlPriority;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void SOPProcessingSearchViewTestInitialize()
        {
            _mocks.DynamicMock(out _ddlPriority);

            _target = new TestSOPProcessingSearchViewBuilder()
                .WithDDLPriority(_ddlPriority).Build();
            
        }
        #endregion

        #region Property Tests
        
        [TestMethod]
        public void TestPhasePropertyDenotesFinalization()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(WorkOrderPhase.Finalization, _target.Phase);
        }

        [TestMethod]
        public void TestPriorityReturnsValueOfPriority()
        {
            var expected = 1;

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPriority.GetSelectedValue())
                    .Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.PriorityID);
            }

        }

        #endregion

        #region Search Expression Tests

        [TestMethod]
        public void TestBaseExpression()
        {
            _mocks.ReplayAll();

            var expr = _target.BaseExpression.Compile();
            
            //Valid work orders should be in finalization, 
            //have a priority of Emergency and require a SOP but have none associated with them.
            WorkOrder valid = new TestWorkOrderBuilder()
                .WithDateCompleted(DateTime.Now.AddDays(-1))
                .WithPriority(TestWorkOrderPriorityBuilder.Emergency);
                
            valid.StreetOpeningPermitRequired = true;

            WorkOrder invalid = new TestWorkOrderBuilder()
                .WithDateCompleted(null);

            Assert.IsTrue(expr(valid));
            Assert.IsFalse(expr(invalid));
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

        [TestMethod]
        public void TestGeneratedExpressionFiltersByPriorityWhenValueChosen()
        {
            WorkOrder valid = new TestWorkOrderBuilder()
                .WithDateCompleted(DateTime.Now.AddDays(-1))
                .WithPriority(TestWorkOrderPriorityBuilder.Routine);
            valid.StreetOpeningPermitRequired = true;

            WorkOrder invalid = new TestWorkOrderBuilder()
                .WithDateCompleted(null);

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPriority.GetSelectedValue())
                    .Return(WorkOrderPriorityRepository.Indices.ROUTINE);
            }

            using (_mocks.Playback())
            {
                var expr = _target.BaseExpression.Compile();
                Assert.IsTrue(expr(valid));
                Assert.IsFalse(expr(invalid));
            }
        }

        [TestMethod]
        public void TestGeneratedExpressionDoesNotFilterByPrioriryWhenNoValueChosen()
        {
            WorkOrder valid = new TestWorkOrderBuilder()
                .WithDateCompleted(DateTime.Now.AddDays(-1))
                .WithPriority(TestWorkOrderPriorityBuilder.Routine);
            valid.StreetOpeningPermitRequired = true;
            WorkOrder alsoValid = new TestWorkOrderBuilder()
                .WithDateCompleted(DateTime.Now.AddDays(-1))
                .WithPriority(TestWorkOrderPriorityBuilder.HighPriority);
            alsoValid.StreetOpeningPermitRequired = true;
            WorkOrder thisTooIsValid = new TestWorkOrderBuilder()
                .WithDateCompleted(DateTime.Now.AddDays(-1))
                .WithPriority(TestWorkOrderPriorityBuilder.Emergency);
            thisTooIsValid.StreetOpeningPermitRequired = true;

            WorkOrder invalid = new TestWorkOrderBuilder()
                .WithDateCompleted(null);

            using (_mocks.Record())
            {
                //noop
            }

            using (_mocks.Playback())
            {
                var expr = _target.BaseExpression.Compile();
                Assert.IsTrue(expr(valid));
                Assert.IsTrue(expr(alsoValid));
                Assert.IsTrue(expr(thisTooIsValid));
                Assert.IsFalse(expr(invalid));
            }
        }

        #endregion
    }

    internal class TestSOPProcessingSearchViewBuilder : TestDataBuilder<TestSOPProcessingSearchView>
    {
        #region Private Members

        private ITextBox _txtStreetOpeningPermitNumber;
        private IDropDownList _ddlPriority;

        #endregion

        #region Exposed Methods

        public override TestSOPProcessingSearchView Build()
        {
            var obj = new TestSOPProcessingSearchView();
            if (_ddlPriority != null)
                obj.SetDDLPriority(_ddlPriority);
            return obj;
        }

        #endregion
        
        public TestSOPProcessingSearchViewBuilder WithDDLPriority(IDropDownList dropDownList)
        {
            _ddlPriority = dropDownList;
            return this;
        }
    }

    internal class TestSOPProcessingSearchView : SOPProcessingSearchView
    {
        internal void SetDDLPriority(IDropDownList dropDownList)
        {
            ddlPriority = dropDownList;
        }

    }
}
