using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    public abstract class NpdesRegulatorInspectionViewModelTestBase<TViewModel>
        : ViewModelTestBase<NpdesRegulatorInspection, TViewModel>
        where TViewModel : NpdesRegulatorInspectionViewModel
    {
        #region Private Members

        protected Mock<IAuthenticationService<User>> _authenticationService;
        protected User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authenticationService = e.For<IAuthenticationService<User>>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            _authenticationService.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert
               .PropertyIsRequired(x => x.ArrivalDateTime)
               .PropertyIsRequired(x => x.HasInfiltration);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.NpdesRegulatorInspectionType);
            _vmTester.CanMapBothWays(x => x.DischargeDuration);
            _vmTester.CanMapBothWays(x => x.ArrivalDateTime);
            _vmTester.CanMapBothWays(x => x.HasInfiltration);
            _vmTester.CanMapBothWays(x => x.IsDischargePresent);
            _vmTester.CanMapBothWays(x => x.RainfallEstimate);
            _vmTester.CanMapBothWays(x => x.DischargeFlow);
            _vmTester.CanMapBothWays(x => x.Remarks);
            _vmTester.CanMapBothWays(x => x.SampleLocation);
            _vmTester.CanMapBothWays(x => x.IsPlumePresent);
            _vmTester.CanMapBothWays(x => x.IsErosionPresent);
            _vmTester.CanMapBothWays(x => x.IsSolidFloatPresent);
            _vmTester.CanMapBothWays(x => x.IsAdditionalEquipmentNeeded);
            _vmTester.CanMapBothWays(x => x.HasSamplesBeenTaken);
            _vmTester.CanMapBothWays(x => x.HasFlowMeterMaintenanceBeenPerformed);
            _vmTester.CanMapBothWays(x => x.HasDownloadedFlowMeterData);
            _vmTester.CanMapBothWays(x => x.HasCalibratedFlowMeter);
            _vmTester.CanMapBothWays(x => x.HasInstalledFlowMeter);
            _vmTester.CanMapBothWays(x => x.HasRemovedFlowMeter);
            _vmTester.CanMapBothWays(x => x.HasFlowMeterBeenMaintainedOther);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.Remarks,
                NpdesRegulatorInspection.StringLengths.REMARKS);
            ValidationAssert.PropertyHasMaxStringLength(x => x.SampleLocation,
                NpdesRegulatorInspection.StringLengths.SAMPLE_LOCATION);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist(
                    x => x.SewerOpening, 
                    GetEntityFactory<SewerOpening>().Create());
            ValidationAssert
               .EntityMustExist(
                    x => x.NpdesRegulatorInspectionType,
                    GetEntityFactory<NpdesRegulatorInspectionType>().Create())
               .EntityMustExist<GateStatusAnswerType>()
               .EntityMustExist<BlockCondition>()
               .EntityMustExist<DischargeCause>()
               .EntityMustExist<DischargeWeatherRelatedType>()
               .EntityMustExist<OutfallCondition>()
               .EntityMustExist<WeatherCondition>();
        }
    }
}
