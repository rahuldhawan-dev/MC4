using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    public abstract class WorkOrderViewModelTestBase<TViewModel> : ViewModelTestBase<WorkOrder, TViewModel>
        where TViewModel : WorkOrderViewModel
    {
        #region Private Members

        protected Mock<IAuthenticationService<User>> _authServ;
        protected User _user;

        #endregion

        #region Private Methods

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IMainCrossingRepository>().Use<MainCrossingRepository>();
            e.For<IEquipmentRepository>().Use<EquipmentRepository>();
            e.For<IAuthenticationRepository<User>>().Use<AuthenticationRepository>();
            e.For<IServiceRepository>().Use<ServiceRepository>();
            e.For<ITapImageRepository>().Use<TapImageRepository>();
            e.For<IImageToPdfConverter>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ApartmentAddtl);
            _vmTester.CanMapBothWays(x => x.AssetType);
            _vmTester.CanMapBothWays(x => x.DigitalAsBuiltRequired);
            _vmTester.CanMapBothWays(x => x.Equipment);
            _vmTester.CanMapBothWays(x => x.HasSampleSite);
            _vmTester.CanMapBothWays(x => x.Hydrant);
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.RequestedBy);
            _vmTester.CanMapBothWays(x => x.ServiceNumber);
            _vmTester.CanMapBothWays(x => x.SewerOpening);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.Town);
            _vmTester.CanMapBothWays(x => x.Valve);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert
               .PropertyIsRequired(x => x.AssetType)
               .PropertyIsRequired(x => x.OperatingCenter)
               .PropertyIsRequired(x => x.RequestedBy)
               .PropertyIsRequired(x => x.StreetNumber, WorkOrderViewModel.ErrorMessages.STREET_NUMBER)
               .PropertyIsRequired(x => x.Town);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist<AcousticMonitoringType>(x => x.AcousticMonitoringType)
               .EntityMustExist<AssetType>(x => x.AssetType)
               .EntityMustExist<Coordinate>(x => x.CoordinateId)
               .EntityMustExist<Equipment>(x => x.Equipment)
               .EntityMustExist<Hydrant>(x => x.Hydrant)
               .EntityMustExist<MainCrossing>(x => x.MainCrossing)
               .EntityMustExist<OperatingCenter>(x => x.OperatingCenter)
               .EntityMustExist<PlantMaintenanceActivityType>(x => x.PlantMaintenanceActivityTypeOverride)
               .EntityMustExist<Service>(x => x.Service)
               .EntityMustExist<SewerOpening>(x => x.SewerOpening)
               .EntityMustExist<Town>(x => x.Town)
               .EntityMustExist<Valve>(x => x.Valve)
               .EntityMustExist<WorkOrderRequester>(x => x.RequestedBy);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert
               .PropertyHasMaxStringLength(x => x.ApartmentAddtl, WorkOrder.StringLengths.APARTMENT_ADDTL)
               .PropertyHasMaxStringLength(
                    x => x.MeterSerialNumber,
                    WorkOrder.StringLengths.METER_SERIAL_NUMBER)
               .PropertyHasMaxStringLength(x => x.ServiceNumber, WorkOrder.StringLengths.SERVICE_NUMBER)
               .PropertyHasMaxStringLength(x => x.StreetNumber, WorkOrder.StringLengths.STREET_NUMBER);
            // TODO: also has a minimum, which breaks testing against the generated validation message
            //.PropertyHasMaxStringLength(x => x.PremiseNumber, WorkOrder.StringLengths.PREMISE_NUMBER)
        }

        [TestMethod]
        public void Test_MapToEntity_SetsDigitalAsBuiltRequired_IfWorkDescriptionSaysTo()
        {
            var workDescription = GetFactory<ServiceLineRenewalWorkDescriptionFactory>().Create();

            _viewModel.WorkDescription = workDescription.Id;

            var result = _viewModel.MapToEntity(_entity);
            
            Assert.IsTrue(result.DigitalAsBuiltRequired);
        }

        [TestMethod]
        public void Test_MapToEntity_DoesNotSetDigitalAsBuiltRequired_IfWorkDescriptionDoesNotSayTo()
        {
            var workDescription = GetFactory<HydrantLandscapingWorkDescriptionFactory>().Create();

            _viewModel.WorkDescription = workDescription.Id;

            var result = _viewModel.MapToEntity(_entity);
            
            Assert.IsFalse(result.DigitalAsBuiltRequired);
        }
        
        #endregion
    }
}
