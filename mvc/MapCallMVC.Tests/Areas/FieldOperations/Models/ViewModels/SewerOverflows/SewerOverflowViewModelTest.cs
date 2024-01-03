using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerOverflows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.SewerOverflows
{
    [TestClass]
    public class SewerOverflowViewModelTest<TViewModel> : ViewModelTestBase<SewerOverflow, TViewModel> where TViewModel : SewerOverflowViewModel
    {
        #region Fields

        protected User _user;
        protected DateTime _now;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        protected Mock<IAuthenticationService<User>> _authService;
        protected IList<SewerOverflowDischargeLocation> _dischargeLocations;
        
        #endregion

        #region Private Methods
        
        private void InitializeGenericRequiredModelValues()
        {
            _viewModel.IncidentDate = _now;
            _viewModel.GallonsOverflowedEstimated = 1;
            _viewModel.SewageRecoveredGallons = 1;
            _viewModel.DischargeLocation = _dischargeLocations[SewerOverflowDischargeLocation.Indices.STORM_SEWER - 1].Id;
            _viewModel.WeatherType = GetFactory<DischargeWeatherRelatedTypeFactory>().Create().Id;
            _viewModel.OverflowType = GetFactory<SSOSewerOverflowTypeFactory>().Create().Id;
            _viewModel.OverflowCause = GetFactory<DebrisSewerOverflowCauseFactory>().Create().Id;
            _viewModel.CallReceived = _now;
            _viewModel.CrewArrivedOnSite = _now;
            _viewModel.SewageContained = _now;
            _viewModel.StoppageCleared = _now;
            _viewModel.WorkCompleted = _now;
            _viewModel.LocationOfStoppage = "test location";
        }
        
        #endregion
        
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IAuthenticationService<User>>().Use((_authService = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetEntityFactory<User>().Create(new {
                Employee = GetEntityFactory<Employee>().Create()
            });

            _authService.Setup(x => x.CurrentUser)
                        .Returns(_user);

            _now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate())
                             .Returns(_now);
            _dischargeLocations = GetFactory<SewerOverflowDischargeLocationFactory>().CreateAll();

            _container.Inject(_authService.Object);
            _container.Inject(_dateTimeProvider.Object);
            
            InitializeGenericRequiredModelValues();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(x => x.WasteWaterSystem, GetEntityFactory<WasteWaterSystem>().Create());
            ValidationAssert.EntityMustExist(x => x.Town, GetEntityFactory<Town>().Create());
            ValidationAssert.EntityMustExist(x => x.Street, GetEntityFactory<Street>().Create());
            ValidationAssert.EntityMustExist(x => x.CrossStreet, GetEntityFactory<Street>().Create());
            ValidationAssert.EntityMustExist(x => x.BodyOfWater, GetEntityFactory<BodyOfWater>().Create());
            ValidationAssert.EntityMustExist(x => x.SewerClearingMethod, GetEntityFactory<SewerClearingMethod>().Create());
            ValidationAssert.EntityMustExist(x => x.AreaCleanedUpTo, GetEntityFactory<SewerOverflowArea>().Create());
            ValidationAssert.EntityMustExist(x => x.ZoneType, GetEntityFactory<ZoneType>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.EnforcingAgencyCaseNumber, SewerOverflow.StringLengths.ENFORCING_AGENCY_CASE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.LocationOfStoppage, SewerOverflow.StringLengths.LOCATION_OF_STOPPAGE);
            ValidationAssert.PropertyHasMaxStringLength(x => x.StreetNumber, SewerOverflow.StringLengths.STREET_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.TalkedTo, SewerOverflow.StringLengths.TALKED_TO);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.IncidentDate);
            _vmTester.CanMapBothWays(x => x.TalkedTo);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.GallonsOverflowedEstimated);
            _vmTester.CanMapBothWays(x => x.GallonsFlowedIntoBodyOfWater);
            _vmTester.CanMapBothWays(x => x.EnforcingAgencyCaseNumber);
            _vmTester.CanMapBothWays(x => x.CallReceived);
            _vmTester.CanMapBothWays(x => x.CrewArrivedOnSite);
            _vmTester.CanMapBothWays(x => x.SewageContained);
            _vmTester.CanMapBothWays(x => x.StoppageCleared);
            _vmTester.CanMapBothWays(x => x.WorkCompleted);
            _vmTester.CanMapBothWays(x => x.LocationOfStoppage);
            _vmTester.CanMapBothWays(x => x.OverflowCustomers);
            _vmTester.CanMapBothWays(x => x.SewageRecoveredGallons);
            _vmTester.CanMapBothWays(x => x.WeatherType);
            _vmTester.CanMapBothWays(x => x.DischargeLocation);
            _vmTester.CanMapBothWays(x => x.DischargeLocationOther);
            _vmTester.CanMapBothWays(x => x.OverflowCause);
            _vmTester.CanMapBothWays(x => x.OverflowType);

            _vmTester.CanMapBothWays(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            _vmTester.CanMapBothWays(x => x.WasteWaterSystem, GetEntityFactory<WasteWaterSystem>().Create());
            _vmTester.CanMapBothWays(x => x.Town, GetEntityFactory<Town>().Create());
            _vmTester.CanMapBothWays(x => x.Street, GetEntityFactory<Street>().Create());
            _vmTester.CanMapBothWays(x => x.CrossStreet, GetEntityFactory<Street>().Create());
            _vmTester.CanMapBothWays(x => x.BodyOfWater, GetEntityFactory<BodyOfWater>().Create());
            _vmTester.CanMapBothWays(x => x.SewerClearingMethod, GetEntityFactory<SewerClearingMethod>().Create());
            _vmTester.CanMapBothWays(x => x.AreaCleanedUpTo, GetEntityFactory<SewerOverflowArea>().Create());
            _vmTester.CanMapBothWays(x => x.ZoneType, GetEntityFactory<ZoneType>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.State);
            ValidationAssert.PropertyIsRequired(x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(x => x.WasteWaterSystem);
            ValidationAssert.PropertyIsRequired(x => x.Town);
            ValidationAssert.PropertyIsRequired(x => x.Coordinate);
            ValidationAssert.PropertyIsRequired(x => x.OverflowType);
            ValidationAssert.PropertyIsRequired(x => x.OverflowCause);
            ValidationAssert.PropertyIsRequired(x => x.IncidentDate);
            ValidationAssert.PropertyIsRequired(x => x.GallonsOverflowedEstimated);
            ValidationAssert.PropertyIsRequired(x => x.SewageRecoveredGallons);
            ValidationAssert.PropertyIsRequired(x => x.DischargeLocation);
            ValidationAssert.PropertyIsRequired(x => x.WeatherType);
            ValidationAssert.PropertyIsRequired(x => x.OverflowType);
            ValidationAssert.PropertyIsRequired(x => x.OverflowCause);
            ValidationAssert.PropertyIsRequired(x => x.CallReceived);
            ValidationAssert.PropertyIsRequired(x => x.CrewArrivedOnSite);
            ValidationAssert.PropertyIsRequired(x => x.StoppageCleared);
            ValidationAssert.PropertyIsRequired(x => x.WorkCompleted);
            ValidationAssert.PropertyIsRequired(x => x.LocationOfStoppage);
        }
        
        #region Validation

        [TestMethod]
        public void TestEnforcingAgencyCaseNumberRequiredWhenOperatingCenterHasMaximumOverflowGallonsAndViewModelHasValue()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                MaximumOverflowGallons = 0
            });

            Session.Save(operatingCenter);
            Session.Flush();

            _viewModel.OperatingCenter = operatingCenter.Id;
            _viewModel.Town = 1;
            _viewModel.Coordinate = 1;
            _viewModel.GallonsOverflowedEstimated = 5;

            ValidationAssert.ModelStateHasError(x => x.EnforcingAgencyCaseNumber,
                "Please enter the Enforcing Agency Case #");
        }

        [TestMethod]
        public void TestBodyOfWaterRequiredWhenDischargeLocationIsSetToBodyOfWater()
        {
            _viewModel.State = GetEntityFactory<State>().Create().Id;
            _viewModel.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            _viewModel.WasteWaterSystem = GetEntityFactory<WasteWaterSystem>().Create().Id;
            _viewModel.Town = 1;
            _viewModel.Coordinate = 1;
            _viewModel.DischargeLocation = _dischargeLocations[SewerOverflowDischargeLocation.Indices.BODY_OF_WATER - 1].Id;
            
            Session.Flush();
            
            ValidationAssert.ModelStateHasError(x => x.BodyOfWater,
                "The BodyOfWater field is required.");
        }
        
        [TestMethod]
        public void TestDischargeLocationOtherRequiredWhenDischargeLocationIsSetToOther()
        {
            _viewModel.State = GetEntityFactory<State>().Create().Id;
            _viewModel.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            _viewModel.WasteWaterSystem = GetEntityFactory<WasteWaterSystem>().Create().Id;
            _viewModel.Town = 1;
            _viewModel.Coordinate = 1;
            _viewModel.DischargeLocation = _dischargeLocations[SewerOverflowDischargeLocation.Indices.OTHER - 1].Id;

            Session.Flush();
            
            ValidationAssert.ModelStateHasError(x => x.DischargeLocationOther,
                "The DischargeLocationOther field is required.");
        }

        [TestMethod]
        public void TestBodyOfWaterIsNullWhenDischargeLocationIsSetToOther()
        {
            _viewModel.State = GetEntityFactory<State>().Create().Id;
            _viewModel.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            _viewModel.WasteWaterSystem = GetEntityFactory<WasteWaterSystem>().Create().Id;
            _viewModel.Town = 1;
            _viewModel.Coordinate = 1;
            _viewModel.DischargeLocation = _dischargeLocations[SewerOverflowDischargeLocation.Indices.OTHER - 1].Id;

            _viewModel.MapToEntity(_entity);
            
            Assert.IsNull(_entity.BodyOfWater);
        }

        [TestMethod]
        public void TestDischargeLocationOtherIsNullWhenDischargeLocationIsSetToBodyOfWater()
        {
            _viewModel.State = GetEntityFactory<State>().Create().Id;
            _viewModel.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            _viewModel.WasteWaterSystem = GetEntityFactory<WasteWaterSystem>().Create().Id;
            _viewModel.Town = 1;
            _viewModel.Coordinate = 1;
            _viewModel.DischargeLocation = _dischargeLocations[SewerOverflowDischargeLocation.Indices.BODY_OF_WATER - 1].Id;

            _viewModel.MapToEntity(_entity);
            
            Assert.IsNull(_entity.DischargeLocationOther);
        }
        
        #endregion

        #endregion
    }
}
