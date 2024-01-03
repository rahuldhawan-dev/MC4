using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FleetManagement.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FleetManagement.Models
{
    [TestClass]
    public class VehicleViewModelTest : MapCallMvcInMemoryDatabaseTestBase<Vehicle>
    {
        private ViewModelTester<TestViewModel, Vehicle> _vmTester;
        private TestViewModel _viewModel;
        private Vehicle _entity;
        private Mock<IAuthenticationService<User>> _authServ;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(new Mock<IDateTimeProvider>().Object);
            e.For<IRepository<Employee>>().Use<EmployeeRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<IVehicleRepository>().Use<VehicleRepository>();
            e.For<ISensorRepository>().Use<SensorRepository>();
            _authServ = e.For<IAuthenticationService<User>>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new TestViewModel(_container);
            _entity = GetFactory<VehicleFactory>().Create();
            _vmTester = new ViewModelTester<TestViewModel, Vehicle>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMostPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.AlvId);
            _vmTester.CanMapBothWays(x => x.ARIVehicleNumber);
            _vmTester.CanMapBothWays(x => x.AssetDetails);
            _vmTester.CanMapBothWays(x => x.Comments);
            _vmTester.CanMapBothWays(x => x.DateInService);
            _vmTester.CanMapBothWays(x => x.DateOrdered);
            _vmTester.CanMapBothWays(x => x.DateRequisitioned);
            _vmTester.CanMapBothWays(x => x.DateRetired);
            _vmTester.CanMapBothWays(x => x.DecalNumber);
            _vmTester.CanMapBothWays(x => x.District);
            _vmTester.CanMapBothWays(x => x.EmergencyUse);
            _vmTester.CanMapBothWays(x => x.Flag);
            _vmTester.CanMapBothWays(x => x.FuelCardNumber);
            _vmTester.CanMapBothWays(x => x.GVW);
            _vmTester.CanMapBothWays(x => x.LeaseCostMth);
            _vmTester.CanMapBothWays(x => x.LeaseExpiration);
            _vmTester.CanMapBothWays(x => x.LeaseTerm);
            _vmTester.CanMapBothWays(x => x.LeasingCompany);
            _vmTester.CanMapBothWays(x => x.LogoWaiver);
            _vmTester.CanMapBothWays(x => x.Make);
            _vmTester.CanMapBothWays(x => x.MileageTracked);
            _vmTester.CanMapBothWays(x => x.ModelYear);
            _vmTester.CanMapBothWays(x => x.NedapSerialNumber);
            _vmTester.CanMapBothWays(x => x.OriginalAssetValueCapCost);
            _vmTester.CanMapBothWays(x => x.PlannedReplacementYear);
            _vmTester.CanMapBothWays(x => x.PlateNumber);
            _vmTester.CanMapBothWays(x => x.PoolUse);
            _vmTester.CanMapBothWays(x => x.RegistrationAnnualCost);
            _vmTester.CanMapBothWays(x => x.RegistrationRenewalDate);
            _vmTester.CanMapBothWays(x => x.RequisitionNumber);
            _vmTester.CanMapBothWays(x => x.ToughbookMount);
            _vmTester.CanMapBothWays(x => x.ToughbookSerialNumber);
            _vmTester.CanMapBothWays(x => x.Upbranded);
            _vmTester.CanMapBothWays(x => x.VehicleIdentificationNumber);
            _vmTester.CanMapBothWays(x => x.VehicleLabel);
            _vmTester.CanMapBothWays(x => x.WBSNumber);
        }

        // Need this separate test because Model maps to ModelType due to some silly mvc thing.
        [TestMethod]
        public void TestModelCanMapBothWays()
        {
            _entity.Model = "Neat";
            _vmTester.MapToViewModel();
            Assert.AreEqual("Neat", _viewModel.ModelType);

            _viewModel.ModelType = "Meat";
            _vmTester.MapToEntity();
            Assert.AreEqual("Meat", _entity.Model);
        }
        

        [TestMethod]
        public void TestAssignmentCategoryCanMapBothWays()
        {
            var refEntity = GetEntityFactory<VehicleAssignmentCategory>().Create();
            _entity.AssignmentCategory = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.AssignmentCategory);

            _entity.AssignmentCategory = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.AssignmentCategory);
        }

        [TestMethod]
        public void TestAssignmentJustificationCanMapBothWays()
        {
            var refEntity = GetEntityFactory<VehicleAssignmentJustification>().Create();
            _entity.AssignmentJustification = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.AssignmentJustification);

            _entity.AssignmentJustification = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.AssignmentJustification);
        }

        [TestMethod]
        public void TestAssignmentStatusCanMapBothWays()
        {
            var refEntity = GetEntityFactory<VehicleAssignmentStatus>().Create();
            _entity.AssignmentStatus = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.AssignmentStatus);

            _entity.AssignmentStatus = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.AssignmentStatus);
        }

        [TestMethod]
        public void TestAccountingRequirementCanMapBothWays()
        {
            var refEntity = GetEntityFactory<VehicleAccountingRequirement>().Create();
            _entity.AccountingRequirement = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.AccountingRequirement);

            _entity.AccountingRequirement = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.AccountingRequirement);
        }

        [TestMethod]
        public void TestEzPassCanMapBothWays()
        {
            var refEntity = GetFactory<VehicleEZPassFactory>().Create();
            _entity.EZPass = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.EZPass);

            _entity.AccountingRequirement = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.EZPass);
        }

        [TestMethod]
        public void TestFacilityCanMapBothWays()
        {
            var refEntity = GetFactory<FacilityFactory>().Create();
            _entity.Facility = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.Facility);

            _entity.Facility = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.Facility);
        }

        [TestMethod]
        public void TestDepartmentCanMapBothWays()
        {
            var refEntity = GetEntityFactory<VehicleDepartment>().Create();
            _entity.Department = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.Department);

            _entity.Department = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.Department);
        }

        [TestMethod]
        public void TestTypeCanMapBothWays()
        {
            var refEntity = GetEntityFactory<VehicleType>().Create();
            _entity.Type = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.Type);

            _entity.Type = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.Type);
        }

        [TestMethod]
        public void TestReplacementVehicleCanMapBothWays()
        {
            var refEntity = GetFactory<VehicleFactory>().Create();
            _entity.ReplacementVehicle = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.ReplacementVehicle);

            _entity.ReplacementVehicle = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.ReplacementVehicle);
        }

        [TestMethod]
        public void TestVehicleStatusCanMapBothWays()
        {
            var refEntity = GetEntityFactory<VehicleStatus>().Create();
            _entity.Status = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.Status);

            _entity.Status = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.Status);
        }

        [TestMethod]
        public void TestIconCanMapBothWays()
        {
            var refEntity = GetEntityFactory<VehicleIcon>().Create();
            _entity.VehicleIcon = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.VehicleIcon);

            _entity.VehicleIcon = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.VehicleIcon);
        }

        [TestMethod]
        public void TestFuelTypeCanMapBothWays()
        {
            var refEntity = GetEntityFactory<VehicleFuelType>().Create();
            _entity.FuelType = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.FuelType);

            _entity.FuelType = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.FuelType);
        }

        [TestMethod]
        public void TestManagerCanMapBothWays()
        {
            var refEntity = GetFactory<EmployeeFactory>().Create();
            _entity.Manager = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.Manager);

            _entity.Manager = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.Manager);
        }

        [TestMethod]
        public void TestFleetContactPersonCanMapBothWays()
        {
            var refEntity = GetFactory<EmployeeFactory>().Create();
            _entity.FleetContactPerson = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.FleetContactPerson);

            _entity.FleetContactPerson = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.FleetContactPerson);
        }

        [TestMethod]
        public void TestPrimaryDriverCanMapBothWays()
        {
            var refEntity = GetFactory<EmployeeFactory>().Create();
            _entity.PrimaryDriver = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.PrimaryDriver);

            _entity.PrimaryDriver = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.PrimaryDriver);
        }

        [TestMethod]
        public void TestServiceCompanyCanMapBothWays()
        {
            var refEntity = GetEntityFactory<VehicleServiceCompany>().Create();
            _entity.ServiceCompany = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.ServiceCompany);

            _entity.ServiceCompany = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.ServiceCompany);
        }

        [TestMethod]
        public void TestVehiclePrimaryUseCanMapBothWays()
        {
            var refEntity = GetEntityFactory<VehiclePrimaryUse>().Create();
            _entity.PrimaryVehicleUse = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.PrimaryVehicleUse);

            _entity.PrimaryVehicleUse = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.PrimaryVehicleUse);
        }

        [TestMethod]
        public void TestVehicleOwnershipTypeCanMapBothWays()
        {
            var refEntity = GetEntityFactory<VehicleOwnershipType>().Create();
            _entity.OwnershipType = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.OwnershipType);

            _entity.OwnershipType = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.OwnershipType);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var refEntity = GetFactory<OperatingCenterFactory>().Create();
            _entity.OperatingCenter = refEntity;
            _vmTester.MapToViewModel();
            Assert.AreEqual(refEntity.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();
            Assert.AreSame(refEntity, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.AlvId, Vehicle.StringLengths.ALV_ID);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ARIVehicleNumber, Vehicle.StringLengths.MAX_ARI);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.AssetDetails, Vehicle.StringLengths.ASSET_DETAILS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Comments, Vehicle.StringLengths.COMMENTS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.DecalNumber, Vehicle.StringLengths.DECAL_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.FuelCardNumber, Vehicle.StringLengths.FUEL_CARD_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LeaseTerm, Vehicle.StringLengths.LEASE_TERM);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LeasingCompany, Vehicle.StringLengths.LEASING_COMPANY);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Make, Vehicle.StringLengths.MAX_MAKE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ModelType, Vehicle.StringLengths.MAX_MODEL);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ModelYear, Vehicle.StringLengths.MAX_MODEL_YEAR);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.NedapSerialNumber, Vehicle.StringLengths.MAX_NEDAP_SERIAL_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PlateNumber, Vehicle.StringLengths.MAX_PLATE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.RequisitionNumber, Vehicle.StringLengths.REQUISITION_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ToughbookMount, Vehicle.StringLengths.TOUGHBOOK_MOUNT);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ToughbookSerialNumber, Vehicle.StringLengths.TOUGHBOOK_SERIAL_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.VehicleIdentificationNumber, Vehicle.StringLengths.MAX_VIN);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.VehicleLabel, Vehicle.StringLengths.VEHICLE_LABEL);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.WBSNumber, Vehicle.StringLengths.WBS_NUMBER);
        }
        
        #endregion

        #region Helpers

        private class TestViewModel : VehicleViewModel
        {
            public TestViewModel(IContainer container) : base(container) { }
        }

        #endregion
    }
}
