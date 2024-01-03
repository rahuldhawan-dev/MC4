using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using StringLengths = MapCall.Common.Model.Entities.NearMiss.StringLengths;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Models.ViewModels
{
    [TestClass]
    public class EditNearMissTest : ViewModelTestBase<NearMiss, EditNearMiss>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IStateRepository>().Use<StateRepository>();
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<ISeverityTypeRepository>().Use<SeverityTypeRepository>();
        }

        #endregion

        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester
               .CanMapBothWays(x => x.ActionTakenType)
               .CanMapBothWays(x => x.Category)
               .CanMapBothWays(x => x.DateCompleted)
               .CanMapBothWays(x => x.DescribeOther)
               .CanMapBothWays(x => x.Facility)
               .CanMapBothWays(x => x.HaveReviewedNearMiss)
               .CanMapBothWays(x => x.LifeSavingRuleType)
               .CanMapBothWays(x => x.OperatingCenter)
               .CanMapBothWays(x => x.ProductionWorkOrder, GetEntityFactory<ProductionWorkOrder>().Create())
               .CanMapBothWays(x => x.SeriousInjuryOrFatality)
               .CanMapBothWays(x => x.ShortCycleWorkOrderNumber)
               .CanMapBothWays(x => x.StopWorkUsageType)
               .CanMapBothWays(x => x.SubCategory)
               .CanMapBothWays(x => x.SystemType)
               .CanMapBothWays(x => x.Town)
               .CanMapBothWays(x => x.Type)
               .CanMapBothWays(x => x.WorkOrder, GetEntityFactory<WorkOrder>().Create())
               .CanMapBothWays(x => x.WorkOrderNumber)
               .CanMapBothWays(x => x.WorkOrderType);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert
               .PropertyIsRequired(x => x.Category)
               .PropertyIsRequired(x => x.Description)
               .PropertyIsRequired(x => x.OperatingCenter)
               .PropertyIsRequired(x => x.State)
               .PropertyIsRequired(x => x.Type)
               .PropertyIsNotRequired(x => x.Coordinate)
               .PropertyIsNotRequired(x => x.Facility)
               .PropertyIsNotRequired(x => x.HaveReviewedNearMiss)
               .PropertyIsNotRequired(x => x.LocationDetails)
               .PropertyIsNotRequired(x => x.ShortCycleWorkOrderNumber)
               .PropertyIsNotRequired(x => x.Town)
               .PropertyIsRequiredWhen(
                    x => x.ActionTakenType,
                    GetEntityFactory<ActionTakenType>().Create().Id,
                    x => x.Type,
                    NearMissType.Indices.SAFETY)
               .PropertyIsRequiredWhen(
                    x => x.CompletedCorrectiveActions,
                    true,
                    x => x.Type,
                    NearMissType.Indices.ENVIRONMENTAL)
               .PropertyIsRequiredWhen(x => x.DescribeOther, "Describe Other", x => x.Category, 9)
               .PropertyIsRequiredWhen(
                    x => x.StopWorkUsageType,
                    GetEntityFactory<StopWorkUsageType>().Create().Id,
                    x => x.StopWorkAuthorityPerformed,
                    true)
               .PropertyIsRequiredWhen(
                    x => x.SystemType,
                    GetEntityFactory<SystemType>().Create().Id,
                    x => x.Type,
                    NearMissType.Indices.ENVIRONMENTAL);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist<ActionTakenType>()
               .EntityMustExist<Coordinate>()
               .EntityMustExist<Facility>()
               .EntityMustExist<LifeSavingRuleType>()
               .EntityMustExist<NearMissCategory>(x => x.Category)
               .EntityMustExist<NearMissSubCategory>(x => x.SubCategory)
               .EntityMustExist<NearMissType>(x => x.Type)
               .EntityMustExist<OperatingCenter>()
               .EntityMustExist<ProductionWorkOrder>()
               .EntityMustExist<PublicWaterSupply>()
                //.EntityMustExist<SeverityType>(x => x.Severity)
               .EntityMustExist<State>()
               .EntityMustExist<StopWorkUsageType>()
               .EntityMustExist<SystemType>()
               .EntityMustExist<Town>()
               .EntityMustExist<WasteWaterSystem>()
               .EntityMustExist<WorkOrder>()
               .EntityMustExist<WorkOrderType>();
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert
               .PropertyHasStringLength(x => x.ActionTaken, StringLengths.ACTION_TAKEN, 5)
               .PropertyHasMaxStringLength(x => x.DescribeOther, StringLengths.DESCRIBE_OTHER)
               .PropertyHasMaxStringLength(x => x.LocationDetails, StringLengths.LOCATION_DETAILS)
               .PropertyHasMaxStringLength(x => x.WorkOrderNumber, StringLengths.WORK_ORDER_NUMBER);
        }

        [TestMethod]
        public void Test_DisplayNearMiss_ReturnsOriginalNearMiss()
        {
            var entity = GetEntityFactory<NearMiss>().Create();
            var vm = _viewModelFactory.Build<EditNearMiss, NearMiss>(entity);
            Assert.AreSame(entity, vm.DisplayNearMiss);
        }

        #endregion
    }
}
