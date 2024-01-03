using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallIntranet.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;
using StringLengths = MapCall.Common.Model.Entities.NearMiss.StringLengths;

namespace MapCallIntranet.Tests
{
    [TestClass]
    public class CreateNearMissTest : ViewModelTestBase<NearMiss, CreateNearMiss>
    {
        #region Private Members

        private Mock<HttpContextBase> _httpContext;
        private Mock<HttpRequestBase> _httpRequest;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _httpContext = new Mock<HttpContextBase>();
            _httpRequest = new Mock<HttpRequestBase>();
            _httpContext.Setup(x => x.Request).Returns(_httpRequest.Object);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            
            e.For<IStateRepository>().Use<StateRepository>();
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
        }

        #endregion

        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester
               .CanMapBothWays(x => x.ActionTaken)
               .CanMapBothWays(x => x.ActionTaken)
               .CanMapBothWays(x => x.ActionTakenType)
               .CanMapBothWays(x => x.Category)
               .CanMapBothWays(x => x.DescribeOther)
               .CanMapBothWays(x => x.Description)
               .CanMapBothWays(x => x.Employee)
               .CanMapBothWays(x => x.Facility)
               .CanMapBothWays(x => x.LocationDetails)
               .CanMapBothWays(x => x.NotCompanyFacility)
               .CanMapBothWays(x => x.OccurredAt)
               .CanMapBothWays(x => x.OccurredAt)
               .CanMapBothWays(x => x.ProductionWorkOrder,
                    GetEntityFactory<ProductionWorkOrder>().Create())
               .CanMapBothWays(x => x.ReportAnonymously)
               .CanMapBothWays(x => x.ReportedBy)
               .CanMapBothWays(x => x.ShortCycleWorkOrderNumber)
               .CanMapBothWays(x => x.StopWorkAuthorityPerformed)
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
               .PropertyIsRequired(x => x.Description)
               .PropertyIsRequired(x => x.NotCompanyFacility)
               .PropertyIsRequired(x => x.OccurredAt)
               .PropertyIsRequired(x => x.OperatingCenter)
               .PropertyIsRequired(x => x.State)
               .PropertyIsRequired(x => x.Type)
               .PropertyIsNotRequired(x => x.ActionTaken)
               .PropertyIsNotRequired(x => x.Coordinate)
               .PropertyIsNotRequired(x => x.Facility)
               .PropertyIsNotRequired(x => x.FileUpload)
               .PropertyIsNotRequired(x => x.LocationDetails)
               .PropertyIsNotRequired(x => x.RelatedToContractor)
               .PropertyIsNotRequired(x => x.ReportAnonymously)
               .PropertyIsNotRequired(x => x.ReportAnonymously)
               .PropertyIsNotRequired(x => x.Severity)
               .PropertyIsNotRequired(x => x.ShortCycleWorkOrderNumber)
               .PropertyIsNotRequired(x => x.StopWorkAuthorityPerformed)
               .PropertyIsNotRequired(x => x.SubCategory)
               .PropertyIsNotRequired(x => x.SubmittedOnBehalfOfAnotherEmployee)
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
               .PropertyIsRequiredWhen(
                    x => x.ContractorCompany,
                    "Contractor Company",
                    x => x.RelatedToContractor,
                    true)
               .PropertyIsRequiredWhen(
                    x => x.Employee,
                    GetEntityFactory<Employee>().Create().Id,
                    x => x.SubmittedOnBehalfOfAnotherEmployee,
                    true)
               .PropertyIsRequiredWhen(x => x.ReportedBy, "Name", x => x.ReportAnonymously, false)
               .PropertyIsRequiredWhen(
                    x => x.StopWorkUsageType,
                    GetEntityFactory<StopWorkUsageType>().Create().Id,
                    x => x.StopWorkAuthorityPerformed,
                    true);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist<ActionTakenType>()
               .EntityMustExist<Coordinate>()
               .EntityMustExist<Employee>()
               .EntityMustExist<Facility>()
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
               .PropertyHasMaxStringLength(x => x.ContractorCompany, StringLengths.CONTRACTOR_COMPANY)
               .PropertyHasMaxStringLength(x => x.DescribeOther, StringLengths.DESCRIBE_OTHER)
               .PropertyHasMaxStringLength(x => x.LocationDetails, StringLengths.LOCATION_DETAILS)
               .PropertyHasMaxStringLength(x => x.WorkOrderNumber, StringLengths.WORK_ORDER_NUMBER)
               .PropertyHasMaxStringLength(x => x.ReportedBy, StringLengths.REPORTED_BY);
        }

        #endregion
    }
}
