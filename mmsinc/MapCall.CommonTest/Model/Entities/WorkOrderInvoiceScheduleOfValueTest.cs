using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class
        WorkOrderInvoiceScheduleOfValueTest : MapCallMvcInMemoryDatabaseTestBase<WorkOrderInvoiceScheduleOfValue>
    {
        //Id	ScheduleOfValueCategoryId	UnitOfMeasureId	Description	LaborUnitCost	LaborUnitOvertimeCost	MaterialCost	MiscCost
        //71	21	10	Mechanic	96.23	137.20	NULL NULL
        [TestMethod]
        public void TestUnitPriceReturnsCorrectTotalPriceWhenSeveralUnitsUsed()
        {
            var scheduleOfValueCategories = GetEntityFactory<ScheduleOfValueCategory>().CreateList(30);
            var scheduleOfValue = GetEntityFactory<ScheduleOfValue>()
               .Create(new {ScheduleOfValueCategory = scheduleOfValueCategories[22]});
            var target = new WorkOrderInvoiceScheduleOfValue {
                LaborUnitCost = 96.23m,
                Total = 6,
                ScheduleOfValue = scheduleOfValue,
                IncludeMarkup = true
            };
            Assert.AreEqual(110.66m, target.UnitPrice);
            Assert.AreEqual(663.96m, target.TotalPrice);
        }

        [TestMethod]
        public void TestUnitPriceReturnsLaborUnitCostWithMarkupAndAddsMiscCostAfterMarkup()
        {
            //Mechanic		6.00	No	$110.66	$663.99	Edit	Remove

            decimal laborUnitCost = 2m, miscCost = 4m;
            var scheduleOfValueCategory = new ScheduleOfValueCategory();
            var scheduleOfValue = new ScheduleOfValue {ScheduleOfValueCategory = scheduleOfValueCategory};
            var target = new WorkOrderInvoiceScheduleOfValue {
                MiscCost = miscCost,
                LaborUnitCost = laborUnitCost,
                Total = 4,
                ScheduleOfValue = scheduleOfValue,
                IncludeMarkup = true
            };

            Assert.AreEqual(laborUnitCost * WorkOrderInvoiceScheduleOfValue.MARKUP_COST + laborUnitCost + miscCost,
                target.UnitPrice);
        }

        [TestMethod]
        public void TestTotalPriceReturnsLaborUnitCostWithMarkupAndAddsMiscCostAfterMarkupMultipliedByTotal()
        {
            decimal laborUnitCost = 2m, miscCost = 4m;
            int total = 4;
            var scheduleOfValueCategory = new ScheduleOfValueCategory();
            var scheduleOfValue = new ScheduleOfValue {ScheduleOfValueCategory = scheduleOfValueCategory};
            var target = new WorkOrderInvoiceScheduleOfValue {
                MiscCost = miscCost,
                LaborUnitCost = laborUnitCost,
                Total = total,
                ScheduleOfValue = scheduleOfValue,
                IncludeMarkup = true
            };

            Assert.AreEqual(
                (laborUnitCost * WorkOrderInvoiceScheduleOfValue.MARKUP_COST + laborUnitCost + miscCost) * total,
                target.TotalPrice);
        }

        [TestMethod]
        public void TestUnitPriceWithSupervisorMarkUpReturnsLaborUnitCostWithMarkupAndAddsMiscCostAfterMarkup()
        {
            decimal laborUnitCost = 2m, miscCost = 4m;
            var scheduleOfValueType = GetEntityFactory<ScheduleOfValueType>().Create(new {Description = "Foo"});
            var scheduleOfValueCategory = GetEntityFactory<ScheduleOfValueCategory>().Create(new {
                ScheduleOfValueType = scheduleOfValueType,
                Description = "New Small Diameter Services (Includes New Tap on Main)"
            });
            var scheduleOfValue = new ScheduleOfValue {ScheduleOfValueCategory = scheduleOfValueCategory};
            var target = new WorkOrderInvoiceScheduleOfValue {
                MiscCost = miscCost,
                LaborUnitCost = laborUnitCost,
                Total = 4,
                ScheduleOfValue = scheduleOfValue,
                IncludeMarkup = true
            };
            Assert.AreEqual(6.53m, target.UnitPrice);
        }

        [TestMethod]
        public void
            TestTotalPriceWithSupervisorMarkUpReturnsLaborUnitCostWithMarkupAndAddsMiscCostAfterMarkupMultipliedByTotal()
        {
            decimal laborUnitCost = 2m, miscCost = 4m;
            int total = 4;
            var scheduleOfValueType = GetEntityFactory<ScheduleOfValueType>().Create(new {Description = "Foo"});
            var scheduleOfValueCategory = GetEntityFactory<ScheduleOfValueCategory>().Create(new {
                ScheduleOfValueType = scheduleOfValueType,
                Description = "New Small Diameter Services (Includes New Tap on Main)"
            });
            var scheduleOfValue = new ScheduleOfValue {ScheduleOfValueCategory = scheduleOfValueCategory};
            var target = new WorkOrderInvoiceScheduleOfValue {
                MiscCost = miscCost,
                LaborUnitCost = laborUnitCost,
                Total = total,
                ScheduleOfValue = scheduleOfValue,
                IncludeMarkup = true
            };

            Assert.AreEqual(26.12m, target.TotalPrice);
        }

        [TestMethod]
        public void TestWorkOrderScheduleOfValuePriceCalculatesProperly()
        {
            decimal laborUnitCost = 2529.00m, miscCost = 100m, materialCost = 1800m;
            var scheduleOfValueType = GetEntityFactory<ScheduleOfValueType>().Create(new {Description = "Foo"});
            var scheduleOfValueCategory = GetEntityFactory<ScheduleOfValueCategory>().Create(new {
                ScheduleOfValueType = scheduleOfValueType,
                Description = "New Small Diameter Services (Includes New Tap on Main)"
            });
            var scheduleOfValue = new ScheduleOfValue {ScheduleOfValueCategory = scheduleOfValueCategory};
            var target = new WorkOrderInvoiceScheduleOfValue {
                MiscCost = miscCost,
                LaborUnitCost = laborUnitCost,
                MaterialCost = materialCost,
                Total = 1,
                ScheduleOfValue = scheduleOfValue,
                IncludeMarkup = true
            };
            Assert.AreEqual(5576.19m, target.UnitPrice);
        }
    }
}
