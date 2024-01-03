using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class CreateProductionWorkOrderFromPlanTest : ViewModelTestBase<ProductionWorkOrder, CreateProductionWorkOrderFromPlan>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        #endregion

        #region Validations

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.BasicStart);
            _vmTester.CanMapBothWays(x => x.MaintenancePlan);
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.PlanningPlant);
            _vmTester.CanMapBothWays(x => x.Facility);
            _vmTester.CanMapBothWays(x => x.EquipmentType);
            _vmTester.CanMapBothWays(x => x.ProductionWorkDescription);
            _vmTester.CanMapBothWays(x => x.FunctionalLocation);
            _vmTester.CanMapBothWays(x => x.Priority);
            _vmTester.CanMapBothWays(x => x.CorrectiveOrderProblemCode);
            _vmTester.CanMapBothWays(x => x.BreakdownIndicator);
            _vmTester.CanMapBothWays(x => x.RequestedBy);
            _vmTester.CanMapBothWays(x => x.OrderNotes);
            _vmTester.CanMapBothWays(x => x.LocalTaskDescription);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.MaintenancePlan, GetEntityFactory<MaintenancePlan>().Create());
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(x => x.PlanningPlant, GetEntityFactory<PlanningPlant>().Create());
            ValidationAssert.EntityMustExist(x => x.Facility, GetEntityFactory<Facility>().Create());
            ValidationAssert.EntityMustExist(x => x.EquipmentType, GetEntityFactory<EquipmentType>().Create());
            ValidationAssert.EntityMustExist(x => x.ProductionWorkDescription, GetEntityFactory<ProductionWorkDescription>().Create());
            ValidationAssert.EntityMustExist(x => x.Priority, GetEntityFactory<ProductionWorkOrderPriority>().Create());
            ValidationAssert.EntityMustExist(x => x.CorrectiveOrderProblemCode, GetEntityFactory<CorrectiveOrderProblemCode>().Create());
            ValidationAssert.EntityMustExist(x => x.RequestedBy, GetEntityFactory<Employee>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.BasicStart);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.OrderNotes, ProductionWorkOrder.StringLengths.NOTES);
            ValidationAssert.PropertyHasMaxStringLength(x => x.LocalTaskDescription, ProductionWorkOrder.StringLengths.LOCAL_TASK_DESCRIPTION);
        }
        
        [TestMethod]
        public void TestMapToEntityAssignsDueDateByFrequency()
        {
            var plan = GetFactory<MaintenancePlanFactory>().Create(new {
                ProductionWorkOrderFrequency = new ProductionWorkOrderFrequency { Id = ProductionWorkOrderFrequency.Indices.DAILY }
            });
            _viewModel.MaintenancePlan = plan.Id;

            _vmTester.MapToEntity();

            Assert.IsNotNull(_entity.StartDate);
            Assert.IsNotNull(_entity.DateReceived);
            Assert.IsNotNull(_entity.DueDate);

            // These should be the same because the frequency is "Daily"
            Assert.AreEqual(_entity.DateReceived, _entity.DueDate);
        }

        [TestMethod]
        public void TestMapToEntityAssignsStartDateFromToday()
        {
            var plan = GetFactory<MaintenancePlanFactory>().Create(new {
                ProductionWorkOrderFrequency = new ProductionWorkOrderFrequency { Id = ProductionWorkOrderFrequency.Indices.DAILY }
            });
            _viewModel.MaintenancePlan = plan.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(_entity.StartDate, DateTime.Today);
        }

        [TestMethod]
        public void TestMapToEntityAssignsEstimatedHours()
        {
            var expectedEstimatedHours = (decimal)42;
            var plan = GetFactory<MaintenancePlanFactory>().Create(new {
                EstimatedHours = expectedEstimatedHours
            });
            _viewModel.MaintenancePlan = plan.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedEstimatedHours, _entity.EstimatedCompletionHours);
        }

        [TestMethod]
        public void TestMapToEntityBreakdownIndicator()
        {
            var plan = GetFactory<MaintenancePlanFactory>().Create();
            _viewModel.MaintenancePlan = plan.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(false, _entity.BreakdownIndicator);
        }

        #endregion
    }
}