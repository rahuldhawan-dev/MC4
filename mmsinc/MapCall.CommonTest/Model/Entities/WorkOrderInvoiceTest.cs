using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class WorkOrderInvoiceTest : MapCallMvcInMemoryDatabaseTestBase<WorkOrderInvoice>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        #endregion

        [TestMethod]
        public void TestTotalMaterialPrice()
        {
            var material = GetEntityFactory<Material>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            GetEntityFactory<OperatingCenterStockedMaterial>()
               .Create(new {Material = material, OperatingCenter = operatingCenter, Cost = 4m});
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {OperatingCenter = operatingCenter});
            GetEntityFactory<MaterialUsed>().Create(new {Material = material, WorkOrder = workOrder, Quantity = 5});
            Session.Save(workOrder);
            Session.Flush();
            Session.Clear();
            workOrder = Session.Load<WorkOrder>(workOrder.Id);
            var target = new WorkOrderInvoice {WorkOrder = workOrder, IncludeMaterials = false};

            Assert.AreEqual(0, target.TotalMaterialPrice);

            target.IncludeMaterials = true;

            Assert.AreEqual(20 * WorkOrderInvoice.MATERIAL_MARKUP_RATE, target.TotalMaterialPrice);
        }

        [TestMethod]
        public void TestTotalScheduleOfValuePrice()
        {
            var target = new WorkOrderInvoice();
            var scheduleOfValueCategory = GetEntityFactory<ScheduleOfValueCategory>().Create();
            var scheduleOfValue = new ScheduleOfValue
                {LaborUnitCost = 10m, ScheduleOfValueCategory = scheduleOfValueCategory};
            target.WorkOrderInvoicesScheduleOfValues.Add(new WorkOrderInvoiceScheduleOfValue {
                WorkOrderInvoice = target,
                ScheduleOfValue = scheduleOfValue,
                LaborUnitCost = scheduleOfValue.LaborUnitCost,
                Total = 5m,
                IncludeMarkup = true
            });

            Assert.AreEqual(63.25m, target.TotalScheduleOfValuePrice);

            var scheduleOfValue2 = new ScheduleOfValue
                {LaborUnitCost = 20m, ScheduleOfValueCategory = scheduleOfValueCategory};
            target.WorkOrderInvoicesScheduleOfValues.Add(new WorkOrderInvoiceScheduleOfValue {
                WorkOrderInvoice = target,
                ScheduleOfValue = scheduleOfValue2,
                LaborUnitCost = scheduleOfValue2.LaborUnitCost,
                Total = 2m,
                IncludeMarkup = true
            });

            Assert.AreEqual(113.85m, target.TotalScheduleOfValuePrice);
        }

        [TestMethod]
        public void TestTotalInvoicePrice()
        {
            var material = GetEntityFactory<Material>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            GetEntityFactory<OperatingCenterStockedMaterial>()
               .Create(new {Material = material, OperatingCenter = operatingCenter, Cost = 4m});
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {OperatingCenter = operatingCenter});
            var scheduleOfValueCategory = GetEntityFactory<ScheduleOfValueCategory>().Create();
            GetEntityFactory<MaterialUsed>().Create(new {Material = material, WorkOrder = workOrder, Quantity = 5});
            Session.Save(workOrder);
            Session.Flush();
            Session.Clear();
            workOrder = Session.Load<WorkOrder>(workOrder.Id);
            var target = new WorkOrderInvoice {WorkOrder = workOrder, IncludeMaterials = true};
            var scheduleOfValue = new ScheduleOfValue
                {LaborUnitCost = 10m, ScheduleOfValueCategory = scheduleOfValueCategory};
            target.WorkOrderInvoicesScheduleOfValues.Add(new WorkOrderInvoiceScheduleOfValue {
                WorkOrderInvoice = target,
                ScheduleOfValue = scheduleOfValue,
                LaborUnitCost = scheduleOfValue.LaborUnitCost,
                Total = 5m,
                IncludeMarkup = false
            });
            var scheduleOfValue2 = new ScheduleOfValue
                {LaborUnitCost = 20m, ScheduleOfValueCategory = scheduleOfValueCategory};
            target.WorkOrderInvoicesScheduleOfValues.Add(new WorkOrderInvoiceScheduleOfValue {
                WorkOrderInvoice = target,
                ScheduleOfValue = scheduleOfValue2,
                LaborUnitCost = scheduleOfValue2.LaborUnitCost,
                Total = 2m,
                IncludeMarkup = true
            });

            Assert.AreEqual(130.60m, target.TotalInvoicePrice);
        }

        #region ScheduleOfValuesMatchWorkOrderScheduleOfValues

        [TestMethod]
        public void TestScheduleOfValuesMatchWorkOrderScheduleOfValuesReturnsTrueIfNotLinkedToAWorkOrder()
        {
            var target = new WorkOrderInvoice();

            Assert.IsTrue(target.ScheduleOfValuesMatchWorkOrderScheduleOfValues);
        }

        [TestMethod]
        public void
            TestScheduleOfValuesMatchWorkOrderScheduleOfValuesReturnsFalseIfLinkedToAWorkOrderWithNoScheduleOfValues()
        {
            var target = new WorkOrderInvoice {WorkOrder = new WorkOrder()};

            Assert.IsFalse(target.ScheduleOfValuesMatchWorkOrderScheduleOfValues);
        }

        [TestMethod]
        public void
            TestScheduleOfValuesMatchWorkOrderScheduleOfValuesReturnsFalseIfLinkedToAWorkOrderWithNoMatchingScheduleOfValues()
        {
            var workorder = new WorkOrder();
            var scheduleOfValue = new ScheduleOfValue();
            workorder.WorkOrdersScheduleOfValues.Add(new WorkOrderScheduleOfValue
                {WorkOrder = workorder, ScheduleOfValue = scheduleOfValue});
            var target = new WorkOrderInvoice {WorkOrder = workorder};

            Assert.IsFalse(target.ScheduleOfValuesMatchWorkOrderScheduleOfValues);
        }

        [TestMethod]
        public void
            TestScheduleOfValuesMatchWorkOrderScheduleOfValuesReturnsFalseIfLinkedToAWorkOrderWithNoMatchingScheduleOfValuesWithDifferentTotals()
        {
            var workorder = new WorkOrder();
            var scheduleOfValue = new ScheduleOfValue();
            workorder.WorkOrdersScheduleOfValues.Add(new WorkOrderScheduleOfValue
                {WorkOrder = workorder, ScheduleOfValue = scheduleOfValue, Total = 1});
            var target = new WorkOrderInvoice {WorkOrder = workorder};
            target.WorkOrderInvoicesScheduleOfValues.Add(new WorkOrderInvoiceScheduleOfValue
                {WorkOrderInvoice = target, ScheduleOfValue = scheduleOfValue, Total = 2});

            Assert.IsFalse(target.ScheduleOfValuesMatchWorkOrderScheduleOfValues);
        }

        [TestMethod]
        public void
            TestScheduleOfValuesMatchWorkOrderScheduleOfValuesReturnsTrueIfLinkedToAWorkOrderWithMatchingScheduleOfValues()
        {
            var workorder = new WorkOrder();
            var scheduleOfValue = new ScheduleOfValue();
            workorder.WorkOrdersScheduleOfValues.Add(new WorkOrderScheduleOfValue
                {WorkOrder = workorder, ScheduleOfValue = scheduleOfValue, Total = 2});
            var target = new WorkOrderInvoice {WorkOrder = workorder};
            target.WorkOrderInvoicesScheduleOfValues.Add(new WorkOrderInvoiceScheduleOfValue
                {WorkOrderInvoice = target, ScheduleOfValue = scheduleOfValue, Total = 2});

            Assert.IsTrue(target.ScheduleOfValuesMatchWorkOrderScheduleOfValues);
        }

        #endregion
    }
}
