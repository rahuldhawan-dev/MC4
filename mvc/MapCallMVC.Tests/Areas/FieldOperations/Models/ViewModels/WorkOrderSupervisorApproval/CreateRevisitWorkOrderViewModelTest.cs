using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderSupervisorApproval;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.WorkOrderSupervisorApproval
{
    [TestClass]
    public class CreateRevisitWorkOrderViewModelTest : ViewModelTestBase<WorkOrder, CreateRevisitWorkOrderViewModel>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Mock();
            e.For<IServiceRepository>().Use<ServiceRepository>();
            e.For<ITapImageRepository>().Use<TapImageRepository>();
            e.For<IImageToPdfConverter>().Use<ImageToPdfConverter>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.Purpose, GetEntityFactory<WorkOrderPurpose>().Create());
            ValidationAssert.EntityMustExist(x => x.Priority, GetEntityFactory<WorkOrderPriority>().Create());
            ValidationAssert.EntityMustExist(x => x.WorkDescription, GetEntityFactory<WorkDescription>().Create());
            ValidationAssert.EntityMustExist(x => x.MarkoutRequirement, GetEntityFactory<MarkoutRequirement>().Create());
            ValidationAssert.EntityMustExist(x => x.RequestedBy, GetEntityFactory<WorkOrderRequester>().Create());
            ValidationAssert.EntityMustExist(x => x.Town, GetEntityFactory<Town>().Create());
            ValidationAssert.EntityMustExist(x => x.AssetType, GetEntityFactory<AssetType>().Create());
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(x => x.OriginalOrderNumber, GetEntityFactory<WorkOrder>().Create());
            ValidationAssert.EntityMustExist(x => x.SAPWorkOrderStep, GetEntityFactory<SAPWorkOrderStep>().Create());
            ValidationAssert.EntityMustExist(x => x.PlantMaintenanceActivityTypeOverride, GetEntityFactory<PlantMaintenanceActivityType>().Create());
            ValidationAssert.EntityMustExist(x => x.TownSection, GetEntityFactory<TownSection>().Create());
            ValidationAssert.EntityMustExist(x => x.Street, GetEntityFactory<Street>().Create());
            ValidationAssert.EntityMustExist(x => x.NearestCrossStreet, GetEntityFactory<Street>().Create());
            ValidationAssert.EntityMustExist(x => x.Service, GetEntityFactory<Service>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Purpose);
            _vmTester.CanMapBothWays(x => x.Priority);
            _vmTester.CanMapBothWays(x => x.WorkDescription);
            _vmTester.CanMapBothWays(x => x.MarkoutRequirement);
            _vmTester.CanMapBothWays(x => x.RequestedBy);
            _vmTester.CanMapBothWays(x => x.Town);
            _vmTester.CanMapBothWays(x => x.AssetType);
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.DeviceLocation);
            _vmTester.CanMapBothWays(x => x.SAPEquipmentNumber);
            _vmTester.CanMapBothWays(x => x.Installation);
            _vmTester.CanMapBothWays(x => x.PremiseNumber);
            _vmTester.CanMapBothWays(x => x.ServiceNumber);
            _vmTester.CanMapBothWays(x => x.SAPWorkOrderStep);
            _vmTester.CanMapBothWays(x => x.Latitude);
            _vmTester.CanMapBothWays(x => x.Longitude);
            _vmTester.CanMapBothWays(x => x.Service);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.Purpose);
            ValidationAssert.PropertyIsRequired(x => x.Priority);
            ValidationAssert.PropertyIsRequired(x => x.WorkDescription);
            ValidationAssert.PropertyIsRequired(x => x.MarkoutRequirement);
            ValidationAssert.PropertyIsRequired(x => x.RequestedBy);
            ValidationAssert.PropertyIsRequired(x => x.Town);
            ValidationAssert.PropertyIsRequired(x => x.AssetType);
            ValidationAssert.PropertyIsRequired(x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(x => x.OriginalOrderNumber);
            ValidationAssert.PropertyIsRequired(x => x.SAPWorkOrderStep);
            ValidationAssert.PropertyIsRequired(x => x.TownSection);
            ValidationAssert.PropertyIsRequired(x => x.Street);
            ValidationAssert.PropertyIsRequired(x => x.NearestCrossStreet);
            ValidationAssert.PropertyIsRequired(x => x.PlantMaintenanceActivityTypeOverride);
            ValidationAssert.PropertyIsRequired(x => x.Service);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no properties to validate string length
        }

        #endregion
    }
}