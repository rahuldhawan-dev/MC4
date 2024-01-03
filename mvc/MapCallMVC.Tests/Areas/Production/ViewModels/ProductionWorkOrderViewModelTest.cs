using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class ProductionWorkOrderViewModelTest : ViewModelTestBase<ProductionWorkOrder, ProductionWorkOrderViewModel>
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
            _vmTester.CanMapBothWays(x => x.ProductionWorkDescription);
            _vmTester.CanMapBothWays(x => x.CorrectiveOrderProblemCode);
            _vmTester.CanMapBothWays(x => x.EstimatedCompletionHours);
            _vmTester.CanMapBothWays(x => x.OtherProblemNotes);
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.PlanningPlant);
            _vmTester.CanMapBothWays(x => x.Facility);
            _vmTester.CanMapBothWays(x => x.FacilityFacilityArea);
            _vmTester.CanMapBothWays(x => x.FunctionalLocation);
            _vmTester.CanMapBothWays(x => x.EquipmentType);
            _vmTester.CanMapBothWays(x => x.Coordinate);
            _vmTester.CanMapBothWays(x => x.Priority);
            _vmTester.CanMapBothWays(x => x.PlantMaintenanceActivityTypeOverride);
            _vmTester.CanMapBothWays(x => x.RequestedBy);
            _vmTester.CanMapBothWays(x => x.OrderNotes);
            _vmTester.CanMapBothWays(x => x.BreakdownIndicator);
            _vmTester.CanMapBothWays(x => x.SAPErrorCode);
            _vmTester.CanMapBothWays(x => x.WBSElement);
            _vmTester.CanMapBothWays(x => x.LocalTaskDescription);
            _vmTester.CanMapBothWays(x => x.AutoCreatedCorrectiveWorkOrder);
        }

        // Per conversation with Chris, there is an issue with RequiredWhen and CorrectiveProblemCode.
        // GetCorrectiveWorkDescriptionIds is probably returning nothing
        // I need to do some setup to create those work descriptions
        // However, that's a rather brittle validation. You can't really guarantee there will be a work description
        // There may be a way write that RequiredWhen such that it can "if there exists a work description that is corrective action 20, then it should be required, else not
        // It has been decided to comment out these tests for now and handle them in the future when a better way to validate them is determined.
        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            //ValidationAssert.EntityMustExist(x => x.ProductionWorkDescription, GetEntityFactory<ProductionWorkDescription>().Create());
            //ValidationAssert.EntityMustExist(x => x.CorrectiveOrderProblemCode, GetEntityFactory<CorrectiveOrderProblemCode>().Create());
            //ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            //ValidationAssert.EntityMustExist(x => x.PlanningPlant, GetEntityFactory<PlanningPlant>().Create());
            //ValidationAssert.EntityMustExist(x => x.Facility, GetEntityFactory<Facility>().Create());
            //ValidationAssert.EntityMustExist(x => x.FacilityFacilityArea, GetEntityFactory<FacilityFacilityArea>().Create());
            //ValidationAssert.EntityMustExist(x => x.EquipmentType, GetEntityFactory<EquipmentType>().Create());
            //ValidationAssert.EntityMustExist(x => x.RedTagPermit, GetEntityFactory<RedTagPermit>().Create());
            //ValidationAssert.EntityMustExist(x => x.Priority, GetEntityFactory<ProductionWorkOrderPriority>().Create());
            //ValidationAssert.EntityMustExist(x => x.PlantMaintenanceActivityTypeOverride, GetEntityFactory<PlantMaintenanceActivityType>().Create());
            //ValidationAssert.EntityMustExist(x => x.RequestedBy, GetEntityFactory<Employee>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            //ValidationAssert.PropertyIsRequired(x => x.ProductionWorkDescription);
            //ValidationAssert.PropertyIsRequired(x => x.EstimatedCompletionHours);
            //ValidationAssert.PropertyIsRequired(x => x.OperatingCenter);
            //ValidationAssert.PropertyIsRequired(x => x.Facility);
            //ValidationAssert.PropertyIsRequired(x => x.FunctionalLocation);
            //ValidationAssert.PropertyIsRequired(x => x.RequestedBy);
            //ValidationAssert.PropertyIsRequired(x => x.OrderNotes);
            //ValidationAssert.PropertyIsRequired(x => x.BreakdownIndicator);
            //ValidationAssert.PropertyIsRequired(x => x.ProductionWorkDescription);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            //ValidationAssert.PropertyHasMaxStringLength(x => x.FunctionalLocation, ProductionWorkOrder.StringLengths.FUNCTIONAL_LOCATION);
            //ValidationAssert.PropertyHasMaxStringLength(x => x.OrderNotes, ProductionWorkOrder.StringLengths.NOTES);
            //ValidationAssert.PropertyHasMaxStringLength(x => x.WBSElement, ProductionWorkOrder.StringLengths.WBS_ELEMENT);
            //ValidationAssert.PropertyHasMaxStringLength(x => x.LocalTaskDescription, ProductionWorkOrder.StringLengths.LOCAL_TASK_DESCRIPTION);
        }

        #endregion
    }
}