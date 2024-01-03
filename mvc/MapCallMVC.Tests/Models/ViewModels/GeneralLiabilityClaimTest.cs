using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class GeneralLiabilityClaimTest : MapCallMvcInMemoryDatabaseTestBase<GeneralLiabilityClaim>
    {
        #region Fields

        private GeneralLiabilityClaimViewModel _viewModel;
        private GeneralLiabilityClaim _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private ViewModelTester<GeneralLiabilityClaimViewModel, GeneralLiabilityClaim> _vmTester;
        
        #endregion
        
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<GeneralLiabilityClaim>().Create();
            _viewModel = _viewModelFactory.Build<GeneralLiabilityClaimViewModel, GeneralLiabilityClaim>(_entity);
            _vmTester = new ViewModelTester<GeneralLiabilityClaimViewModel, GeneralLiabilityClaim>(_viewModel, _entity);
            _user = new User();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }
        
        #endregion

        #region Mapping

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _entity.OtherTypeOfCrash = "Other type";
            _entity.CrashType.Id = CrashType.Indices.OTHER;
            _vmTester.CanMapBothWays(x => x.ClaimNumber);
            _vmTester.CanMapBothWays(x => x.MeterBox);
            _vmTester.CanMapBothWays(x => x.CurbValveBox);
            _vmTester.CanMapBothWays(x => x.Excavation);
            _vmTester.CanMapBothWays(x => x.Barricades);
            _vmTester.CanMapBothWays(x => x.Vehicle);
            _vmTester.CanMapBothWays(x => x.WaterMeter);
            _vmTester.CanMapBothWays(x => x.FireHydrant);
            _vmTester.CanMapBothWays(x => x.Backhoe);
            _vmTester.CanMapBothWays(x => x.WaterQuality);
            _vmTester.CanMapBothWays(x => x.WaterPressure);
            _vmTester.CanMapBothWays(x => x.WaterMain);
            _vmTester.CanMapBothWays(x => x.ServiceLine);
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.Name);
            _vmTester.CanMapBothWays(x => x.PhoneNumber);
            _vmTester.CanMapBothWays(x => x.Address);
            _vmTester.CanMapBothWays(x => x.Email);
            _vmTester.CanMapBothWays(x => x.DriverName);
            _vmTester.CanMapBothWays(x => x.DriverPhone);
            _vmTester.CanMapBothWays(x => x.PhhContacted);
            _vmTester.CanMapBothWays(x => x.OtherDriver);
            _vmTester.CanMapBothWays(x => x.OtherDriverPhone);
            _vmTester.CanMapBothWays(x => x.OtherDriverAddress);
            _vmTester.CanMapBothWays(x => x.LocationOfIncident);
            _vmTester.CanMapBothWays(x => x.IncidentDateTime);
            _vmTester.CanMapBothWays(x => x.VehicleYear);
            _vmTester.CanMapBothWays(x => x.VehicleMake);
            _vmTester.CanMapBothWays(x => x.VehicleVin);
            _vmTester.CanMapBothWays(x => x.LicenseNumber);
            _vmTester.CanMapBothWays(x => x.PoliceCalled);
            _vmTester.CanMapBothWays(x => x.PoliceDepartment);
            _vmTester.CanMapBothWays(x => x.PoliceCaseNumber);
            _vmTester.CanMapBothWays(x => x.WitnessStatement);
            _vmTester.CanMapBothWays(x => x.Witness);
            _vmTester.CanMapBothWays(x => x.WitnessPhone);
            _vmTester.CanMapBothWays(x => x.AnyInjuries);
            _vmTester.CanMapBothWays(x => x.ReportedBy);
            _vmTester.CanMapBothWays(x => x.ReportedByPhone);
            _vmTester.CanMapBothWays(x => x.IncidentNotificationDate);
            _vmTester.CanMapBothWays(x => x.IncidentReportedDate);
            _vmTester.CanMapBothWays(x => x.CompletedDate);
            _vmTester.CanMapBothWays(x => x.Why1);
            _vmTester.CanMapBothWays(x => x.Why2);
            _vmTester.CanMapBothWays(x => x.Why3);
            _vmTester.CanMapBothWays(x => x.Why4);
            _vmTester.CanMapBothWays(x => x.Why5);
            _vmTester.CanMapBothWays(x => x.DateSubmitted);
            _vmTester.CanMapBothWays(x => x.FiveWhysCompleted);
            _vmTester.CanMapBothWays(x => x.OtherTypeOfCrash);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            _entity.OperatingCenter = opc;
            _vmTester.MapToViewModel();
            Assert.AreEqual(opc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();
            Assert.AreSame(opc, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestCoordinateCanMapBothWays()
        {
            var c = GetFactory<CoordinateFactory>().Create();
            _entity.Coordinate = c;
            _vmTester.MapToViewModel();
            Assert.AreEqual(c.Id, _viewModel.Coordinate);

            _entity.Coordinate = null;
            _vmTester.MapToEntity();
            Assert.AreSame(c, _entity.Coordinate);
        }

        [TestMethod]
        public void TestLiabilityTypeCanMapBothWays()
        {
            var liabilityType = GetEntityFactory<LiabilityType>().Create(new {Description = "Flerghs"});
            _entity.LiabilityType = liabilityType;
            _vmTester.MapToViewModel();
            Assert.AreEqual(liabilityType.Id, _viewModel.LiabilityType);

            _entity.LiabilityType = null;
            _vmTester.MapToEntity();
            Assert.AreSame(liabilityType, _entity.LiabilityType);
        }

        [TestMethod]
        public void TestGeneralLiabilityClaimTypeCanMapBothWays()
        {
            var generalLiabilityClaimType = GetEntityFactory<GeneralLiabilityClaimType>().Create(new { Description = "Preventable" });
            _entity.GeneralLiabilityClaimType = generalLiabilityClaimType;
            _vmTester.MapToViewModel();
            Assert.AreEqual(generalLiabilityClaimType.Id, _viewModel.GeneralLiabilityClaimType);

            _entity.GeneralLiabilityClaimType = null;
            _vmTester.MapToEntity();
            Assert.AreSame(generalLiabilityClaimType, _entity.GeneralLiabilityClaimType);
        }

        [TestMethod]
        public void TestCrashTypeCanMapBothWays()
        {
            var crashType = GetEntityFactory<CrashType>().Create(new { Description = "Rear End" });
            _entity.CrashType = crashType;
            _vmTester.MapToViewModel();
            Assert.AreEqual(crashType.Id, _viewModel.CrashType);

            _entity.CrashType = null;
            _vmTester.MapToEntity();
            Assert.AreSame(crashType, _entity.CrashType);
        }

        [TestMethod]
        public void TestOtherCrashTypeCanMapBothWays()
        {
            var crashType = GetFactory<OtherCrashTypeFactory>().Create();
            _entity.CrashType = crashType;
            _entity.OtherTypeOfCrash = "Test describe Other";
            _vmTester.MapToViewModel();
            Assert.AreEqual(crashType.Id, _viewModel.CrashType);
            Assert.AreEqual("Test describe Other", _viewModel.OtherTypeOfCrash);

            _entity.CrashType = null;
            _entity.OtherTypeOfCrash = null;
            _vmTester.MapToEntity();
            Assert.AreSame(crashType, _entity.CrashType);
            Assert.AreSame("Test describe Other", _entity.OtherTypeOfCrash);
        }

        [TestMethod]
        public void TestCompanyContactCanMapBothWays()
        {
            var emp = GetFactory<EmployeeFactory>().Create();
            _entity.CompanyContact = emp;
            _vmTester.MapToViewModel();
            Assert.AreEqual(emp.Id, _viewModel.CompanyContact);

            _entity.CompanyContact = null;
            _vmTester.MapToEntity();
            Assert.AreSame(emp, _entity.CompanyContact);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredFieldsAreRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter, "The District field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CompanyContact);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ClaimsRepresentative);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ReportedBy);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentNotificationDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentDateTime);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Why1, GeneralLiabilityClaim.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Why2, GeneralLiabilityClaim.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Why3, GeneralLiabilityClaim.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Why4, GeneralLiabilityClaim.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Why5, GeneralLiabilityClaim.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.DateSubmitted, DateTime.Today, x => x.FiveWhysCompleted, true, false, "This field is required when 'Five Whys Completed' is Yes.");
        }

        [TestMethod]
        public void TestStringLengthsAreProperLike()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ClaimNumber, GeneralLiabilityClaim.StringLengths.CLAIM_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Name, GeneralLiabilityClaim.StringLengths.NAME, error: "The field Claimant Name must be a string with a maximum length of 50.");
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PhoneNumber, GeneralLiabilityClaim.StringLengths.PHONE_NUMBER, error: "The field Claimant Phone Number must be a string with a maximum length of 20.");
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Email, GeneralLiabilityClaim.StringLengths.EMAIL, error: "The field Claimant Email must be a string with a maximum length of 50.");
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.DriverName, GeneralLiabilityClaim.StringLengths.DRIVER_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.DriverPhone, GeneralLiabilityClaim.StringLengths.DRIVER_PHONE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.OtherDriver, GeneralLiabilityClaim.StringLengths.OTHER_DRIVER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.OtherDriverPhone, GeneralLiabilityClaim.StringLengths.OTHER_DRIVER_PHONE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LocationOfIncident, GeneralLiabilityClaim.StringLengths.LOCATION_OF_INCIDENT);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.VehicleMake, GeneralLiabilityClaim.StringLengths.VEHICLE_MAKE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.VehicleVin, GeneralLiabilityClaim.StringLengths.VEHICLE_VIN);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LicenseNumber, GeneralLiabilityClaim.StringLengths.LICENSE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PoliceDepartment, GeneralLiabilityClaim.StringLengths.POLICE_DEPARTMENT);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PoliceCaseNumber, GeneralLiabilityClaim.StringLengths.POLICE_CASE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Witness, GeneralLiabilityClaim.StringLengths.WITNESS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.WitnessPhone, GeneralLiabilityClaim.StringLengths.WITNESS_PHONE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ReportedBy, GeneralLiabilityClaim.StringLengths.REPORTED_BY);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ReportedByPhone, GeneralLiabilityClaim.StringLengths.REPORTED_BY_PHONE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.OtherTypeOfCrash, GeneralLiabilityClaim.StringLengths.OTHER_TYPE_OF_CRASH);
        }

        [TestMethod]
        public void TestPoliceDepartmentRequiredWhenPoliceCalled()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PoliceDepartment, "the fuzz", x => x.PoliceCalled, true, false);
        }

        [TestMethod]
        public void TestPoliceCaseNumberRequiredWhenPoliceCalled()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PoliceCaseNumber, "123", x => x.PoliceCalled, true, false);
        }

        [TestMethod]
        public void TestWRequiredWhenPoliceCalled()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PoliceCaseNumber, "123", x => x.PoliceCalled, true, false);
        }

        [TestMethod]
        public void TestFiveWhysFieldsRequiredWhenFiveWhysCompleted()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.Why1, "some string value", x => x.FiveWhysCompleted, true, false, "This field is required when 'Five Whys Completed' is Yes.");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.DateSubmitted, DateTime.Now, x => x.FiveWhysCompleted, true, false, "This field is required when 'Five Whys Completed' is Yes.");
        }

        [TestMethod]
        public void TestOtherTypeOfCrashRequiredValidation()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.OtherTypeOfCrash, "Other Type Of Crash", x => x.CrashType, 5);
        }

        #endregion
    }

    [TestClass]
    public class CreateGeneralLiabilityClaimTest : MapCallMvcInMemoryDatabaseTestBase<GeneralLiabilityClaim>
    {
        #region Fields

        private ViewModelTester<CreateGeneralLiabilityClaim, GeneralLiabilityClaim> _vmTester;
        private CreateGeneralLiabilityClaim _viewModel;
        private GeneralLiabilityClaim _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IAuthenticationService>().Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<GeneralLiabilityClaim>().Create();
            _viewModel = _viewModelFactory.Build<CreateGeneralLiabilityClaim, GeneralLiabilityClaim>( _entity);
            _vmTester = new ViewModelTester<CreateGeneralLiabilityClaim, GeneralLiabilityClaim>(_viewModel, _entity);
            _user = new User();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion
    }
}
