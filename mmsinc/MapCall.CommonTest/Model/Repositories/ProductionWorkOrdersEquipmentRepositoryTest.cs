using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class ProductionWorkOrdersEquipmentRepositoryTest : MapCallMvcSecuredRepositoryTestBase<ProductionWorkOrderEquipment,
        ProductionWorkOrdersEquipmentRepository, User>
    {
        private DateTime _now;

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use(new TestDateTimeProvider(_now = DateTime.Now));
        }

        #endregion

        #region Tests

        #region GetRegulatoryComplianceReport

        [TestMethod]
        public void TestGetRegulatoryComplianceReportByDate()
        {
            var state1 = GetEntityFactory<State>().Create();
            var state2 = GetEntityFactory<State>().Create();
            var state1opCenter1 = GetEntityFactory<OperatingCenter>().Create(new { State = state1 });
            var state2opCenter1 = GetEntityFactory<OperatingCenter>().Create(new { State = state2 });
            var equipment1 = GetEntityFactory<Equipment>().Create(new { HasOshaRequirement = true });
            var orderType = GetFactory<RoutineOrderTypeFactory>().Create();
            var workDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderType });
            var withState1 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = state1opCenter1,
                DateReceived = _now,
                ProductionWorkDescription = workDescription
            });
            var withState2 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = state2opCenter1,
                DateReceived = _now,
                ProductionWorkDescription = workDescription
            });
            var pwoe = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = withState2,
                IsParent = true,
                Equipment = equipment1
            });
            withState2.Equipments.Add(pwoe);
            var search = new SearchRegulatoryCompliance {
                DateReceived = new RequiredDateRange { Operator = RangeOperator.Equal, End = _now }
            };

            Repository.GetRegulatoryComplianceReport(search);

            Assert.IsTrue(search.Results.Any(r => r.State == state1.Abbreviation));
            Assert.IsTrue(search.Results.Any(r => r.State == state2.Abbreviation));
            Assert.AreEqual(1, search.Results.Count());
            Assert.IsFalse(search.Results.Any(r => r.EquipmentId == withState1.Equipment?.Id));
            Assert.IsTrue(search.Results.Any(r => r.EquipmentId == withState2.Equipment.Id));
        }

        [TestMethod]
        public void TestGetRegulatoryComplianceReportReturnsExpectedIncompleteOrderCount()
        {
            var equipment1 = GetEntityFactory<Equipment>().Create(new { OtherCompliance = true, OtherComplianceReason = "testing other compliance" });
            var orderType = GetFactory<PlantMaintenanceOrderTypeFactory>().Create();
            var workDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderType });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
                ProductionWorkDescription = workDescription
            });
            var pwoe = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = order,
                IsParent = true,
                Equipment = equipment1
            });
            order.Equipments.Add(pwoe);
            var search = new SearchRegulatoryCompliance {
                DateReceived = new RequiredDateRange { Operator = RangeOperator.Equal, End = _now }
            };

            Repository.GetRegulatoryComplianceReport(search);
            var result = search.Results.Single();

            Assert.AreEqual(1, result.NumberIncomplete);
            Assert.AreEqual(0, result.NumberCancelled);
            Assert.AreEqual(0, result.NumberCompleted);
            Assert.AreEqual(equipment1.Id, result.EquipmentId);
            Assert.AreEqual(equipment1.OtherComplianceReason, result.OtherComplianceReason);
        }

        [TestMethod]
        public void TestGetRegulatoryComplianceReportReturnsExpectedCompletedOrderCount()
        {
            var orderType1 = GetEntityFactory<OrderType>().Create();
            var equipment1 = GetEntityFactory<Equipment>().Create(new { OtherCompliance = true, OtherComplianceReason = "testing other compliance" });
            var orderType = GetFactory<RoutineOrderTypeFactory>().Create();
            var workDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderType });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
                DateCompleted = _now.AddDays(5),
                ProductionWorkDescription = workDescription
            });
            var pwoe = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = order,
                IsParent = true,
                Equipment = equipment1
            });
            order.Equipments.Add(pwoe);
            var search = new SearchRegulatoryCompliance {
                DateReceived = new RequiredDateRange { Operator = RangeOperator.Equal, End = _now }
            };

            Repository.GetRegulatoryComplianceReport(search);
            var result = search.Results.Single();

            Assert.AreEqual(0, result.NumberIncomplete);
            Assert.AreEqual(0, result.NumberCancelled);
            Assert.AreEqual(1, result.NumberCompleted);
            Assert.AreEqual(equipment1.Id, result.EquipmentId);
            Assert.AreEqual(equipment1.OtherComplianceReason, result.OtherComplianceReason);
        }

        [TestMethod]
        public void TestGetRegulatoryComplianceReportReturnsExpectedCancelledOrderCount()
        {
            var orderType1 = GetEntityFactory<OrderType>().Create();
            var equipment1 = GetEntityFactory<Equipment>().Create(new { OtherCompliance = true, OtherComplianceReason = "testing other compliance" });
            var orderType = GetFactory<RoutineOrderTypeFactory>().Create();
            var workDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderType });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
                DateCancelled = _now.AddDays(3),
                ProductionWorkDescription = workDescription
            });
            var pwoe = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = order,
                IsParent = true,
                Equipment = equipment1
            });
            order.Equipments.Add(pwoe);
            var search = new SearchRegulatoryCompliance {
                DateReceived = new RequiredDateRange { Operator = RangeOperator.Equal, End = _now }
            };

            Repository.GetRegulatoryComplianceReport(search);
            var result = search.Results.Single();

            Assert.AreEqual(0, result.NumberIncomplete);
            Assert.AreEqual(1, result.NumberCancelled);
            Assert.AreEqual(0, result.NumberCompleted);
            Assert.AreEqual(equipment1.Id, result.EquipmentId);
            Assert.AreEqual(equipment1.OtherComplianceReason, result.OtherComplianceReason);
        }

        [TestMethod]
        public void TestGetRegulatoryComplianceReportWithHasRegulatoryRequirement()
        {
            GetEntityFactory<OrderType>().Create();
            var equipment1 = GetEntityFactory<Equipment>().Create(new { HasRegulatoryRequirement = true });
            var equipment2 = GetEntityFactory<Equipment>().Create(new { HasProcessSafetyManagement = true });
            var orderType = GetFactory<RoutineOrderTypeFactory>().Create();
            var workDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderType });
            var order1 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
                DateCancelled = _now.AddDays(3),
                ProductionWorkDescription = workDescription
            });
            var pwoe1 = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = order1,
                IsParent = true,
                Equipment = equipment1
            });
            order1.Equipments.Add(pwoe1);
            var order2 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
                DateCancelled = _now.AddDays(3),
            });
            var pwoe2 = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = order2,
                IsParent = true,
                Equipment = equipment2
            });
            order2.Equipments.Add(pwoe2);
            var search = new SearchRegulatoryCompliance {
                DateReceived = new RequiredDateRange { Operator = RangeOperator.Equal, End = _now },
                HasRegulatoryRequirement = true
            };

            Repository.GetRegulatoryComplianceReport(search);
            var result = search.Results.Single();

            Assert.AreEqual(0, result.NumberIncomplete);
            Assert.AreEqual(1, result.NumberCancelled);
            Assert.AreEqual(0, result.NumberCompleted);
            Assert.AreNotEqual(equipment2.HasRegulatoryRequirement, result.HasRegulatoryRequirement);
            Assert.AreEqual(equipment1.Id, result.EquipmentId);
            Assert.AreEqual(equipment1.HasRegulatoryRequirement, result.HasRegulatoryRequirement);
        }

        #endregion

        #region Helper classes

        private class SearchRegulatoryCompliance : SearchSet<RegulatoryCompliance>, ISearchRegulatoryCompliance
        {
            public int[] State { get; set; }
            public int[] OperatingCenter { get; set; }
            public int[] PlanningPlant { get; set; }
            public int[] Facility { get; set; }
            public int[] EquipmentType { get; set; }
            public int[] EquipmentPurpose { get; set; }
            public string Description { get; set; }
            [Search(CanMap = false)]
            public RequiredDateRange DateReceived { get; set; }
            public string[] SelectedFacilities { get; set; }
            public string[] SelectedEquipmentTypes { get; set; }
            public string[] SelectedEquipmentPurposes { get; set; }
            public string[] SelectedStates { get; set; }
            public string[] SelectedOperatingCenters { get; set; }
            public string[] SelectedPlanningPlants { get; set; }
            [Search(CanMap = false)]
            public bool? HasProcessSafetyManagement { get; set; }
            [Search(CanMap = false)]
            public bool? HasCompanyRequirement { get; set; }
            [Search(CanMap = false)]
            public bool? HasRegulatoryRequirement { get; set; }
            [Search(CanMap = false)]
            public bool? HasOshaRequirement { get; set; }
            [Search(CanMap = false)]
            public bool? OtherCompliance { get; set; }
        }

        #endregion

        #endregion
    }
}
