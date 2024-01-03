using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MapCall.Common.Testing;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits;
using MMSINC.Authentication;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits
{
    [TestClass]
    public class RedTagPermitViewModelTest<TViewModel> : ViewModelTestBase<RedTagPermit, TViewModel> where TViewModel : RedTagPermitViewModel
    {
        #region Fields

        protected User _user;
        protected DateTime _now;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        protected Mock<IAuthenticationService<User>> _authService;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _authService = new Mock<IAuthenticationService<User>>();

            _now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);
            
            _user = GetEntityFactory<User>().Create(new {
                Employee = GetEntityFactory<Employee>().Create()
            });
            _authService.Setup(x => x.CurrentUser).Returns(_user);

            _container.Inject(_authService.Object);
            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ProductionWorkOrder);
            _vmTester.CanMapBothWays(x => x.Equipment);
            _vmTester.CanMapBothWays(x => x.PersonResponsible);
            _vmTester.CanMapBothWays(x => x.ProtectionType);
            _vmTester.CanMapBothWays(x => x.AdditionalInformationForProtectionType);
            _vmTester.CanMapBothWays(x => x.AreaProtected);
            _vmTester.CanMapBothWays(x => x.ReasonForImpairment);
            _vmTester.CanMapBothWays(x => x.NumberOfTurnsToClose);
            _vmTester.CanMapBothWays(x => x.AuthorizedBy);
            _vmTester.CanMapBothWays(x => x.FireProtectionEquipmentOperator);
            _vmTester.CanMapBothWays(x => x.CreatedAt);
            _vmTester.CanMapBothWays(x => x.EmergencyOrganizationNotified);
            _vmTester.CanMapBothWays(x => x.PublicFireDepartmentNotified);
            _vmTester.CanMapBothWays(x => x.HazardousOperationsStopped);
            _vmTester.CanMapBothWays(x => x.HotWorkProhibited);
            _vmTester.CanMapBothWays(x => x.SmokingProhibited);
            _vmTester.CanMapBothWays(x => x.ContinuousWorkAuthorized);
            _vmTester.CanMapBothWays(x => x.OngoingPatrolOfArea);
            _vmTester.CanMapBothWays(x => x.HydrantConnectedToSprinkler);
            _vmTester.CanMapBothWays(x => x.PipePlugsOnHand);
            _vmTester.CanMapBothWays(x => x.FireHoseLaidOut);
            _vmTester.CanMapBothWays(x => x.HasOtherPrecaution);
            _vmTester.CanMapBothWays(x => x.OtherPrecautionDescription);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProductionWorkOrder);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Equipment);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PersonResponsible);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AuthorizedBy);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProtectionType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AreaProtected);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ReasonForImpairment);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.NumberOfTurnsToClose);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AuthorizedBy);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FireProtectionEquipmentOperator);

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, 
                x => x.AdditionalInformationForProtectionType, 
                "some notes about special protection", 
                x => x.ProtectionType, 
                RedTagPermitProtectionType.Indices.SPECIAL_PROTECTION, 
                RedTagPermitProtectionType.Indices.FIRE_PUMP, 
                expectedRequiredErrorMessage: "AdditionalInformationForProtectionType is required for the specified Protection Type.");

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, 
                x => x.AdditionalInformationForProtectionType, 
                "some notes about other protection", 
                x => x.ProtectionType, 
                RedTagPermitProtectionType.Indices.OTHER, 
                RedTagPermitProtectionType.Indices.FIRE_PUMP, 
                expectedRequiredErrorMessage: "AdditionalInformationForProtectionType is required for the specified Protection Type.");

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, 
                x => x.OtherPrecautionDescription, 
                "some notes about having an other precaution", 
                x => x.HasOtherPrecaution,
                true);
        }

        [TestMethod]
        public virtual void TestNonSpecificPropertyErrors()
        {
            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, RedTagPermitViewModel.AT_LEAST_ONE_PRECAUTION_IS_REQUIRED);

            _viewModel.FireHoseLaidOut = true;

            ValidationAssert.ModelStateIsValid(_viewModel);
        }

        [TestMethod]
        public void TestRequiredRangeValidation()
        {
            ValidationAssert.PropertyHasRequiredRange(_viewModel, 
                x => x.NumberOfTurnsToClose, 
                RedTagPermit.Ranges.NUMBER_OF_TURNS_TO_CLOSE_MIN, 
                RedTagPermit.Ranges.NUMBER_OF_TURNS_TO_CLOSE_MAX);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.ProductionWorkOrder, GetEntityFactory<ProductionWorkOrder>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Equipment, GetEntityFactory<Equipment>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.PersonResponsible, GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.AuthorizedBy, GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.ProtectionType, GetEntityFactory<RedTagPermitProtectionType>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.AreaProtected, RedTagPermit.StringLengths.AREA_PROTECTED);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ReasonForImpairment, RedTagPermit.StringLengths.REASON_FOR_IMPAIRMENT);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.OtherPrecautionDescription, RedTagPermit.StringLengths.OTHER_PRECAUTION_DESCRIPTION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.FireProtectionEquipmentOperator, RedTagPermit.StringLengths.FIRE_PROTECTION_EQUIPMENT_OPERATOR);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.AdditionalInformationForProtectionType, RedTagPermit.StringLengths.ADDITIONAL_INFORMATION_FOR_PROTECTION_TYPE);
        }

        #endregion

        #endregion
    }
}