using System;
using System.Collections.Generic;
using System.Linq;
using LINQTo271.Views.WorkOrders.Finalization;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Finalization
{
    /// <summary>
    /// Summary description for WorkOrderFinalizationSearchViewTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderFinalizationSearchViewTest : EventFiringTestClass
    {
        #region Private Members

        private TestWorkOrderFinalizationSearchView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _target = new TestWorkOrderFinalizationSearchViewBuilder();
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
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
        public void TestBaseExpressionFiltersOutAssignmentsWithDatesInFuture()
        {
            _mocks.ReplayAll();

            WorkOrder woYesterday = new WorkOrder { OperatingCenter = new OperatingCenter() },
                      woToday = new WorkOrder { OperatingCenter = new OperatingCenter() },
                      woTomorrow = new WorkOrder { OperatingCenter = new OperatingCenter() },
                      woNoAssignment = new WorkOrder { OperatingCenter = new OperatingCenter() };
            CrewAssignment caYesterday = new CrewAssignment {
                               AssignedFor = DateTime.Today.Date.AddDays(-1),
                               WorkOrder = woYesterday
                           },
                           caToday = new CrewAssignment {
                               AssignedFor = DateTime.Today.Date,
                               WorkOrder = woToday
                           },
                           caTomorrow = new CrewAssignment {
                               AssignedFor = DateTime.Today.GetNextDay(),
                               WorkOrder = woTomorrow
                           };
            IEnumerable<WorkOrder> orders = new[] {
                woYesterday, woToday, woTomorrow, woNoAssignment
            };

            var result = orders.Where(_target.BaseExpression.Compile()).ToList();

            Assert.IsTrue(result.Contains(woYesterday));
            Assert.IsTrue(result.Contains(woToday));
            Assert.IsFalse(result.Contains(woTomorrow));
            Assert.IsFalse(result.Contains(woNoAssignment));
        }

        [TestMethod]
        public void TestBaseExpressionAllowsEmergencyOrdersToPass()
        {
            _mocks.ReplayAll();

            WorkOrder woRoutine =
                          new TestWorkOrderBuilder().WithPriority(
                              TestWorkOrderPriorityBuilder.Routine),
                      woHighPriority =
                          new TestWorkOrderBuilder().WithPriority(
                              TestWorkOrderPriorityBuilder.HighPriority),
                      woEmergency =
                          new TestWorkOrderBuilder().WithPriority(
                              TestWorkOrderPriorityBuilder.Emergency);
            IEnumerable<WorkOrder> orders = new[] {
                woRoutine, woHighPriority, woEmergency
            };

            var result = orders.Where(_target.BaseExpression.Compile()).ToList();
            

            Assert.IsFalse(result.Contains(woRoutine));
            Assert.IsFalse(result.Contains(woHighPriority));
            Assert.IsTrue(result.Contains(woEmergency));
        }

        [TestMethod]
        public void TestBaseExpressionAllowsEquipmentOrdersToPass()
        {
            _mocks.ReplayAll();
            var equipmentAssetType = new TestAssetTypeBuilder().WithTypeName(TestAssetTypeBuilder.Descriptions.EQUIPMENT);
            WorkOrder woRoutine = new TestWorkOrderBuilder().WithPriority(TestWorkOrderPriorityBuilder.Routine),
                      woHighPriority = new TestWorkOrderBuilder().WithPriority(TestWorkOrderPriorityBuilder.HighPriority),
                      woEquipment = new TestWorkOrderBuilder().WithEquipment(new Equipment()).WithAssetType(equipmentAssetType);
            IEnumerable<WorkOrder> orders = new[] { woRoutine, woHighPriority, woEquipment };

            var result = orders.Where(_target.BaseExpression.Compile()).ToList();

            Assert.IsFalse(result.Contains(woRoutine));
            Assert.IsFalse(result.Contains(woHighPriority));
            Assert.IsTrue(result.Contains(woEquipment));
        }

        [TestMethod]
        public void TestBaseExpressionExcludesContractorAssignedOrders()
        {
            _mocks.ReplayAll();
            WorkOrder woContractor = new TestWorkOrderBuilder().WithPriority(
                              TestWorkOrderPriorityBuilder.Emergency);
            woContractor.AssignedContractorID = 50;
            WorkOrder woNoContractor = new TestWorkOrderBuilder().WithPriority(
                    TestWorkOrderPriorityBuilder.Emergency);

            IEnumerable<WorkOrder> orders = new[] {
                woContractor, woNoContractor
            };

            var result = orders.Where(_target.BaseExpression.Compile()).ToList();

            Assert.IsFalse(result.Contains(woContractor));
            Assert.IsTrue(result.Contains(woNoContractor));
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
        public void TestBaseExpressionExcludesRetiredRemovedCancelledValveWorkOrders()
        {
            _mocks.ReplayAll();
            var assetType = new TestAssetTypeBuilder().WithTypeName(TestAssetTypeBuilder.Descriptions.VALVE);
            var statuses = new[] {
                MapCall.Common.Model.Entities.AssetStatus.Indices.RETIRED,
                MapCall.Common.Model.Entities.AssetStatus.Indices.REMOVED,
                MapCall.Common.Model.Entities.AssetStatus.Indices.CANCELLED
            };

            foreach (var status in statuses)
            {
                var valve = new TestValveBuilder().WithValveStatusID(status);
                var wo = new TestWorkOrderBuilder().BuildForFinalization().WithAssetType(assetType).WithValve(valve).Build();
                IEnumerable<WorkOrder> orders = new[] { wo };

                var result = orders.Where(_target.BaseExpression.Compile()).ToList();

                Assert.IsFalse(result.Contains(wo));
            }
        }

        [TestMethod]
        public void TestBaseExpressionDoesNotExcludeOtherStatusesValveWorkOrders()
        {
            _mocks.ReplayAll();
            var assetType = new TestAssetTypeBuilder().WithTypeName(TestAssetTypeBuilder.Descriptions.VALVE);
            var statuses = new[] {
                MapCall.Common.Model.Entities.AssetStatus.Indices.ACTIVE,
                MapCall.Common.Model.Entities.AssetStatus.Indices.PENDING,
                MapCall.Common.Model.Entities.AssetStatus.Indices.INACTIVE,
                MapCall.Common.Model.Entities.AssetStatus.Indices.INSTALLED,
                MapCall.Common.Model.Entities.AssetStatus.Indices.REQUEST_RETIREMENT,
                MapCall.Common.Model.Entities.AssetStatus.Indices.REQUEST_CANCELLATION
            };

            foreach (var status in statuses)
            {
                var valve = new TestValveBuilder().WithValveStatusID(status);
                var wo = new TestWorkOrderBuilder().BuildForFinalization().WithAssetType(assetType).WithValve(valve).Build();
                IEnumerable<WorkOrder> orders = new[] { wo };

                var result = orders.Where(_target.BaseExpression.Compile()).ToList();

                Assert.IsTrue(result.Contains(wo));
            }
        }

        [TestMethod]
        public void TestBaseExpressionExcludesRetiredRemovedCancelledHydrantWorkOrders()
        {
            _mocks.ReplayAll();
            var assetType = new TestAssetTypeBuilder().WithTypeName(TestAssetTypeBuilder.Descriptions.HYDRANT);
            var statuses = new[] {
                MapCall.Common.Model.Entities.AssetStatus.Indices.RETIRED,
                MapCall.Common.Model.Entities.AssetStatus.Indices.REMOVED,
                MapCall.Common.Model.Entities.AssetStatus.Indices.CANCELLED
            };

            foreach (var status in statuses)
            {
                var hydrant = new TestHydrantBuilder().WithHydrantStatusID(status);
                var wo = new TestWorkOrderBuilder().BuildForFinalization().WithAssetType(assetType).WithHydrant(hydrant).Build();
                IEnumerable<WorkOrder> orders = new[] { wo };

                var result = orders.Where(_target.BaseExpression.Compile()).ToList();

                Assert.IsFalse(result.Contains(wo), status.ToString());
            }
        }

        [TestMethod]
        public void TestBaseExpressionDoesNotExcludeOtherStatusesHydrantWorkOrders()
        {
            _mocks.ReplayAll();
            var assetType = new TestAssetTypeBuilder().WithTypeName(TestAssetTypeBuilder.Descriptions.HYDRANT);
            var statuses = new[] {
                MapCall.Common.Model.Entities.AssetStatus.Indices.ACTIVE,
                MapCall.Common.Model.Entities.AssetStatus.Indices.PENDING,
                MapCall.Common.Model.Entities.AssetStatus.Indices.INACTIVE,
                MapCall.Common.Model.Entities.AssetStatus.Indices.INSTALLED,
                MapCall.Common.Model.Entities.AssetStatus.Indices.REQUEST_RETIREMENT,
                MapCall.Common.Model.Entities.AssetStatus.Indices.REQUEST_CANCELLATION
            };

            foreach (var status in statuses)
            {
                var hydrant = new TestHydrantBuilder().WithHydrantStatusID(status);
                var wo = new TestWorkOrderBuilder().BuildForFinalization().WithAssetType(assetType).WithHydrant(hydrant).Build();
                IEnumerable<WorkOrder> orders = new[] { wo };

                var result = orders.Where(_target.BaseExpression.Compile()).ToList();

                Assert.IsTrue(result.Contains(wo));
            }
        }

        [TestMethod]
        public void TestBaseExpressionExcludesRetiredRemovedCancelledSewerOpeningWorkOrders()
        {
            _mocks.ReplayAll();
            var assetType = new TestAssetTypeBuilder().WithTypeName(TestAssetTypeBuilder.Descriptions.SEWER_OPENING);
            var statuses = new[] {
                MapCall.Common.Model.Entities.AssetStatus.Indices.RETIRED,
                MapCall.Common.Model.Entities.AssetStatus.Indices.REMOVED,
                MapCall.Common.Model.Entities.AssetStatus.Indices.CANCELLED
            };

            foreach (var status in statuses)
            {
                var sewerOpening = new TestSewerOpeningBuilder().WithAssetStatusID(status).Build();
                var wo = new TestWorkOrderBuilder().BuildForFinalization().WithAssetType(assetType).WithSewerOpening(sewerOpening).Build();
                IEnumerable<WorkOrder> orders = new[] { wo };

                var result = orders.Where(_target.BaseExpression.Compile()).ToList();

                Assert.IsFalse(result.Contains(wo), status.ToString());
            }
        }

        [TestMethod]
        public void TestBaseExpressionDoesNotExcludeOtherStatusesSewerOpeningWorkOrders()
        {
            _mocks.ReplayAll();
            var assetType = new TestAssetTypeBuilder().WithTypeName(TestAssetTypeBuilder.Descriptions.SEWER_OPENING);
            var statuses = new[] {
                MapCall.Common.Model.Entities.AssetStatus.Indices.ACTIVE,
                MapCall.Common.Model.Entities.AssetStatus.Indices.PENDING,
                MapCall.Common.Model.Entities.AssetStatus.Indices.INACTIVE,
                MapCall.Common.Model.Entities.AssetStatus.Indices.INSTALLED,
                MapCall.Common.Model.Entities.AssetStatus.Indices.REQUEST_RETIREMENT,
                MapCall.Common.Model.Entities.AssetStatus.Indices.REQUEST_CANCELLATION
            };

            foreach (var status in statuses)
            {
                var sewerOpening = new TestSewerOpeningBuilder().WithAssetStatusID(status).Build();
                var wo = new TestWorkOrderBuilder().BuildForFinalization().WithAssetType(assetType).WithSewerOpening(sewerOpening).Build();
                IEnumerable<WorkOrder> orders = new[] { wo };

                var result = orders.Where(_target.BaseExpression.Compile()).ToList();

                Assert.IsTrue(result.Contains(wo));
            }
        }

        #endregion
    }

    internal class TestWorkOrderFinalizationSearchViewBuilder : TestDataBuilder<TestWorkOrderFinalizationSearchView>
    {
        #region Private Members

        private bool _postBack;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderFinalizationSearchView Build()
        {
            var obj = new TestWorkOrderFinalizationSearchView();
            obj.SetPostBack(_postBack);
            return obj;
        }

        public TestWorkOrderFinalizationSearchViewBuilder WithPostBack(bool postBack)
        {
            _postBack = postBack;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderFinalizationSearchView : WorkOrderFinalizationSearchView
    {
        #region Exposed Methods

        public void SetPostBack(bool postBack)
        {
            _isMvpPostBack = postBack;
        }

        #endregion
    }
}
