using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using NHibernate.Linq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    public abstract class BaseJobSiteCheckListViewModelTest<TViewModel> : ViewModelTestBase<JobSiteCheckList, TViewModel>
        where TViewModel : BaseJobSiteCheckListViewModel
    {
        #region Private Methods

        [TestInitialize]
        public void BaseJobSiteCheckListViewModelTestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);
            _authServ = new Mock<IAuthenticationService<User>>();
            _user = GetFactory<AdminUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);
            _weatherHazardTypes = GetEntityFactory<JobSiteCheckListSafetyBriefWeatherHazardType>().CreateList();
            _trafficHazardTypes = GetEntityFactory<JobSiteCheckListSafetyBriefTrafficHazardType>().CreateList();
            _timeOfDayConstraintTypes =
                GetEntityFactory<JobSiteCheckListSafetyBriefTimeOfDayConstraintType>().CreateList();
            _overheadHazardTypes = GetEntityFactory<JobSiteCheckListSafetyBriefOverheadHazardType>().CreateList();
            _undergroundHazardTypes = GetEntityFactory<JobSiteCheckListSafetyBriefUndergroundHazardType>().CreateList();
            _electricalHazardTypes = GetEntityFactory<JobSiteCheckListSafetyBriefElectricalHazardType>().CreateList();
        }

        #endregion

        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        public Mock<IDateTimeProvider> _dateTimeProvider;
        public User _user;
        public IList<JobSiteCheckListSafetyBriefWeatherHazardType> _weatherHazardTypes;
        public IList<JobSiteCheckListSafetyBriefTrafficHazardType> _trafficHazardTypes;
        public IList<JobSiteCheckListSafetyBriefTimeOfDayConstraintType> _timeOfDayConstraintTypes;
        public IList<JobSiteCheckListSafetyBriefOverheadHazardType> _overheadHazardTypes;
        public IList<JobSiteCheckListSafetyBriefUndergroundHazardType> _undergroundHazardTypes;
        public IList<JobSiteCheckListSafetyBriefElectricalHazardType> _electricalHazardTypes;

        #endregion

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            // A bunch of these have false/true set as test values because they have
            // default true values from JobSiteChecklistFactory and the ViewModelTester doesn't like that.
            _vmTester.CanMapBothWays(x => x.Address);
            _vmTester.CanMapBothWays(x => x.AllEmployeesWearingAppropriatePersonalProtectionEquipment);
            _vmTester.CanMapBothWays(x => x.AllMaterialsSetBackFromEdgeOfTrenches);
            _vmTester.CanMapBothWays(x => x.AllStructuresSupportedOrProtected);
            _vmTester.CanMapBothWays(x => x.AreExposedUtilitiesProtected);
            _vmTester.CanMapBothWays(x => x.AtmosphericCarbonMonoxideLevel);
            _vmTester.CanMapBothWays(x => x.AtmosphericLowerExplosiveLimit);
            _vmTester.CanMapBothWays(x => x.AtmosphericOxygenLevel);
            _vmTester.CanMapBothWays(x => x.CheckListDate);
            _vmTester.CanMapBothWays(x => x.CompetentEmployee);
            _vmTester.CanMapBothWays(x => x.CompliesWithStandards);
            _vmTester.CanMapBothWays(x => x.Coordinate);
            _vmTester.CanMapBothWays(x => x.HasAtmosphereBeenTested);
            _vmTester.CanMapBothWays(x => x.HasBarricadesForTrafficControl);
            _vmTester.CanMapBothWays(x => x.HasConesForTrafficControl);
            _vmTester.CanMapBothWays(x => x.HasExcavationFiveFeetOrDeeper);
            _vmTester.CanMapBothWays(x => x.HasExcavationOverFourFeetDeep);
            _vmTester.CanMapBothWays(x => x.HasFlagPersonForTrafficControl);
            _vmTester.CanMapBothWays(x => x.HaveYouInspectedSlings);
            _vmTester.CanMapBothWays(x => x.HasPoliceForTrafficControl);
            _vmTester.CanMapBothWays(x => x.HasSignsForTrafficControl);
            _vmTester.CanMapBothWays(x => x.IsALadderInPlace);
            _vmTester.CanMapBothWays(x => x.IsEmergencyMarkoutRequest);
            _vmTester.CanMapBothWays(x => x.IsLadderOnSlope);
            _vmTester.CanMapBothWays(x => x.IsMarkoutValidForSite);
            _vmTester.CanMapBothWays(x => x.IsShoringSystemUsed);
            _vmTester.CanMapBothWays(x => x.IsSlopeAngleNotLessThanOneHalfHorizontalToOneVertical);
            _vmTester.CanMapBothWays(x => x.LadderExtendsAboveGrade);
            _vmTester.CanMapBothWays(x => x.MapCallWorkOrder);
            _vmTester.CanMapBothWays(x => x.MarkedElectric);
            _vmTester.CanMapBothWays(x => x.MarkedFuelGas);
            _vmTester.CanMapBothWays(x => x.MarkedOther);
            _vmTester.CanMapBothWays(x => x.MarkedSanitarySewer);
            _vmTester.CanMapBothWays(x => x.MarkedTelephone);
            _vmTester.CanMapBothWays(x => x.MarkedWater);
            _vmTester.CanMapBothWays(x => x.MarkoutNumber);
            _vmTester.CanMapBothWays(x => x.NoRestraintReason);
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.PressurizedRiskRestrainedType);
            _vmTester.CanMapBothWays(x => x.RestraintMethod);
            _vmTester.CanMapBothWays(x => x.SAPWorkOrderId);
            _vmTester.CanMapBothWays(x => x.ShoringSystemInstalledTwoFeetFromBottomOfTrench);
            _vmTester.CanMapBothWays(x => x.ShoringSystemSidesExtendAboveBaseOfSlope);
            _vmTester.CanMapBothWays(x => x.SupervisorSignOffDate);
            _vmTester.CanMapBothWays(x => x.SupervisorSignOffEmployee);
            _vmTester.CanMapBothWays(x => x.WaterControlSystemsInUse);
            _vmTester.CanMapBothWays(x => x.SpotterAssigned);
            _vmTester.CanMapBothWays(x => x.IsManufacturerDataOnSiteForShoringOrShieldingEquipment);
            _vmTester.CanMapBothWays(x => x.IsTheExcavationGuardedFromAccidentalEntry);
            _vmTester.CanMapBothWays(x => x.AreThereAnyVisualSignsOfPotentialSoilCollapse);
            _vmTester.CanMapBothWays(x => x.IsTheExcavationSubjectToVibration);
        }

        [TestMethod]
        public void TestHasTrafficControlCanMapToViewModel()
        {
            _vmTester.MapToViewModel();
            Assert.IsFalse(_viewModel.HasTrafficControl.Value);

            _entity.HasBarricadesForTrafficControl = true;
            _vmTester.MapToViewModel();
            Assert.IsTrue(_viewModel.HasTrafficControl.Value);
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
        public void TestProtectionTypesCanMapBothWays()
        {
            var pType = GetFactory<JobSiteExcavationProtectionTypeFactory>().Create();
            _entity.ProtectionTypes.Add(pType);

            _vmTester.MapToViewModel();

            Assert.AreEqual(pType.Id, _viewModel.ProtectionTypes.Single());

            _entity.ProtectionTypes.Clear();

            _vmTester.MapToEntity();

            Assert.AreSame(pType, _entity.ProtectionTypes.Single());
        }

        [TestMethod]
        public void TestProtectionTypesDoesNotAddDuplicatesToEntityDuringMapToEntity()
        {
            var pType = GetFactory<JobSiteExcavationProtectionTypeFactory>().Create();
            _entity.ProtectionTypes.Add(pType);
            _viewModel.ProtectionTypes.Add(pType.Id);

            _vmTester.MapToEntity();

            Assert.AreSame(pType, _entity.ProtectionTypes.Single());
        }

        [TestMethod]
        public void TestSupervisorSignOffEmployeeCanMapBothWays()
        {
            var emp = GetFactory<EmployeeFactory>().Create();
            _entity.SupervisorSignOffEmployee = emp;
            _vmTester.MapToViewModel();
            Assert.AreEqual(emp.Id, _viewModel.SupervisorSignOffEmployee);

            _entity.SupervisorSignOffEmployee = null;
            _vmTester.MapToEntity();
            Assert.AreSame(emp, _entity.SupervisorSignOffEmployee);

            _entity.SupervisorSignOffEmployee = null;
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.SupervisorSignOffEmployee);
        }

        [TestMethod]
        public void TestMapBothWaysForTimeOfDayConstraintTypes()
        {
            var expected = GetEntityFactory<JobSiteCheckListSafetyBriefTimeOfDayConstraintType>().Create();
            var intArray = new int[] { expected.Id };
            _viewModel.SafetyBriefTimeOfDayConstraintTypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.SafetyBriefTimeOfDayConstraintTypes.Single().Id);

            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.SafetyBriefTimeOfDayConstraintTypes[0]);
        }

        [TestMethod]
        public void TestMapBothWaysForTrafficHazardTypes()
        {
            var expected = GetEntityFactory<JobSiteCheckListSafetyBriefTrafficHazardType>().Create();
            var intArray = new int[] { expected.Id };
            _viewModel.SafetyBriefTrafficHazardTypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.SafetyBriefTrafficHazardTypes.Single().Id);

            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.SafetyBriefTrafficHazardTypes[0]);
        }

        [TestMethod]
        public void TestMapBothWaysForUndergroundHazardTypes()
        {
            var expected = GetEntityFactory<JobSiteCheckListSafetyBriefUndergroundHazardType>().Create();
            var intArray = new int[] { expected.Id };
            _viewModel.SafetyBriefUndergroundHazardTypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.SafetyBriefUndergroundHazardTypes.Single().Id);

            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.SafetyBriefUndergroundHazardTypes[0]);
        }

        [TestMethod]
        public void TestMapBothWaysForOverheadHazardTypes()
        {
            var expected = GetEntityFactory<JobSiteCheckListSafetyBriefOverheadHazardType>().Create();
            var intArray = new int[] { expected.Id };
            _viewModel.SafetyBriefOverheadHazardTypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.SafetyBriefOverheadHazardTypes.Single().Id);

            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.SafetyBriefOverheadHazardTypes[0]);
        }

        [TestMethod]
        public void TestMapBothWaysForElectricalHazardTypes()
        {
            var expected = GetEntityFactory<JobSiteCheckListSafetyBriefElectricalHazardType>().Create();
            var intArray = new int[] { expected.Id };
            _viewModel.SafetyBriefElectricalHazardTypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.SafetyBriefElectricalHazardTypes.Single().Id);

            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.SafetyBriefElectricalHazardTypes[0]);
        }

        [TestMethod]
        public void TestMapToViewModelMapsExistingExcavations()
        {
            var excavation = new JobSiteExcavation {Id = 431};
            _entity.Excavations.Add(excavation);

            _vmTester.MapToViewModel();

            Assert.AreEqual(1, _viewModel.Excavations.Count);
            Assert.AreEqual(431, _viewModel.Excavations.Single().Id);
        }

        [TestMethod]
        public void TestMapToEntityMapsNewExcavationsToEntity()
        {
            var excavation = new CreateJobSiteExcavation(_container);
            _viewModel.Excavations.Add(excavation);

            Assert.AreEqual(0, _entity.Excavations.Count);
            _vmTester.MapToEntity();
            Assert.AreEqual(1, _entity.Excavations.Count);

            var result = _entity.Excavations.Single();
            Assert.AreSame(_entity, result.JobSiteCheckList);
        }

        [TestMethod]
        public void TestMapToEntitySetsCreatedByToCurrentUserForNewExcavations()
        {
            _user.UserName = "Wow!";
            var excavation = new CreateJobSiteExcavation(_container);
            _viewModel.Excavations.Add(excavation);

            _vmTester.MapToEntity();

            var result = _entity.Excavations.Single();
            Assert.AreEqual("Wow!", result.CreatedBy);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotChangeCreatedByOnExistingUnmodifiedExcavations()
        {
            var model = GetFactory<JobSiteExcavationFactory>().Create(new
            {
                JobSiteCheckList = _entity,
                CreatedBy = "That guy!"
            });

            _user.UserName = "I'm wrong!";
            var excavation = _viewModelFactory.Build<CreateJobSiteExcavation, JobSiteExcavation>(model);
            _viewModel.Excavations.Add(excavation);

            _vmTester.MapToEntity();

            var result = _entity.Excavations.Single();

            Assert.AreEqual("That guy!", result.CreatedBy);
        }

        [TestMethod]
        public void TestMapToEntityRemovesDeletedExcavationsFromEntity()
        {
            var excavationToKeep = new JobSiteExcavation {Id = 431};
            _entity.Excavations.Add(excavationToKeep);

            var excavationToRemove = new JobSiteExcavation {Id = 12};
            _entity.Excavations.Add(excavationToRemove);

            var toKeepViewModel = _viewModelFactory.Build<CreateJobSiteExcavation, JobSiteExcavation>(excavationToKeep);
            _viewModel.Excavations.Add(toKeepViewModel);

            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.Excavations.Contains(excavationToKeep));
            Assert.IsFalse(_entity.Excavations.Contains(excavationToRemove));
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.CompetentEmployee,
                GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Coordinate, GetEntityFactory<Coordinate>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter,
                GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.NoRestraintReason,
                GetEntityFactory<JobSiteCheckListNoRestraintReasonType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.PressurizedRiskRestrainedType,
                GetEntityFactory<JobSiteCheckListPressurizedRiskRestrainedType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.RestraintMethod,
                GetEntityFactory<JobSiteCheckListRestraintMethodType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter,
                GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.SupervisorSignOffEmployee,
                GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.MapCallWorkOrder,
                GetEntityFactory<WorkOrder>().Create());
        }

        [TestMethod]
        public void TestValidationFailsIfRecordIsApprovedBySupervisorAlreadyAndTheUserIsNotAnAdmin()
        {
            _entity = GetFactory<JobSiteCheckListFactory>().Create();
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(false);

            Assert.IsTrue(_entity.IsApprovedBySupervisor, "sanity check");

            _viewModel.SupervisorSignOffDate = null;
            _viewModel.SupervisorSignOffEmployee = null;
            _viewModel.Id = _entity.Id;

            ValidationAssert.ModelStateHasError(_viewModel, "", "Record can not be edited after it has been approved by a supervisor.");
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Address);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AllMaterialsSetBackFromEdgeOfTrenches, true,
                x => x.HasExcavation, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AreExposedUtilitiesProtected, true,
                x => x.HasExcavation, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AtmosphericOxygenLevel, (decimal)1.23,
                x => x.HasAtmosphereBeenTested, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AtmosphericCarbonMonoxideLevel, (decimal)1.23,
                x => x.HasAtmosphereBeenTested, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AtmosphericLowerExplosiveLimit, (decimal)1.23,
                x => x.HasAtmosphereBeenTested, true, false, "Required");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CompetentEmployee);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.CompliesWithStandards, true,
                x => x.HasTrafficControl, true, false, "Required");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Coordinate);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.HasAtmosphereBeenTested, true,
                x => x.HasExcavationOverFourFeetDeep, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IsALadderInPlace, true,
                x => x.HasExcavationOverFourFeetDeep, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IsLadderOnSlope, true,
                x => x.HasExcavationOverFourFeetDeep, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IsEmergencyMarkoutRequest, true,
                x => x.IsMarkoutValidForSite, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IsShoringSystemUsed, true,
                x => x.HasExcavationFiveFeetOrDeeper, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel,
                x => x.IsSlopeAngleNotLessThanOneHalfHorizontalToOneVertical, true,
                x => x.HasExcavationFiveFeetOrDeeper, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.LadderExtendsAboveGrade, true,
                x => x.HasExcavationOverFourFeetDeep, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.MarkoutNumber, "things",
                x => x.IsMarkoutValidForSite, true, false);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PressurizedRiskRestrainedType, GetEntityFactory<JobSiteCheckListPressurizedRiskRestrainedType>().Create().Id,
                x => x.IsPressurizedRisksRestrainedFieldRequired, true, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ShoringSystemSidesExtendAboveBaseOfSlope, true,
                x => x.HasExcavationFiveFeetOrDeeper, true, false,"Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ShoringSystemInstalledTwoFeetFromBottomOfTrench,
                true, x => x.HasExcavationFiveFeetOrDeeper, true, false,"Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.SupervisorSignOffEmployee, GetEntityFactory<Employee>().Create().Id,
                x => x.SupervisorSignOffDate, _dateTimeProvider.Object.GetCurrentDate(), null);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.SupervisorSignOffDate, _dateTimeProvider.Object.GetCurrentDate(),
                x => x.SupervisorSignOffEmployee, GetEntityFactory<Employee>().Create().Id, null);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.WaterControlSystemsInUse, true,
                x => x.HasExcavation, true, false, "Required");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SafetyBriefDateTime);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AnyPotentialWeatherHazards);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.SafetyBriefWeatherHazardTypes,
                new int[] {_weatherHazardTypes.First().Id}, x => x.AnyPotentialWeatherHazards, true, false);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HadConversationAboutWeatherHazard);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AnyTimeOfDayConstraints);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.SafetyBriefTimeOfDayConstraintTypes,
                new int[] {_timeOfDayConstraintTypes.First().Id}, x => x.AnyTimeOfDayConstraints, true, false);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AnyTrafficHazards);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.SafetyBriefTrafficHazardTypes,
                new int[] {_trafficHazardTypes.First().Id}, x => x.AnyTrafficHazards, true, false);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.InvolveConfinedSpace);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AnyPotentialOverheadHazards);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.SafetyBriefOverheadHazardTypes,
                new int[] {_overheadHazardTypes.First().Id}, x => x.AnyPotentialOverheadHazards, true, false);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AnyUndergroundHazards);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.SafetyBriefUndergroundHazardTypes,
                new int[] {_undergroundHazardTypes.First().Id}, x => x.AnyUndergroundHazards, true, false);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AreThereElectricalHazards);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.SafetyBriefElectricalHazardTypes,
                new int[] {_electricalHazardTypes.First().Id}, x => x.AreThereElectricalHazards, true, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.CrewMembersTrainedInACPipe, true,
                x => x.WorkingWithACPipe, true, false);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HaveEquipmentToDoJobSafely);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.HaveEquipmentToDoJobSafelyNotes, "things",
                x => x.HaveEquipmentToDoJobSafely, false, true);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ReviewedErgonomicHazards);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ReviewedErgonomicHazardsNotes, "things",
                x => x.ReviewedErgonomicHazards, false, true);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OtherHazardsIdentified);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.OtherHazardNotes, "things",
                x => x.OtherHazardsIdentified, true, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PPEOtherNotes, "things", x => x.PPEOther, true,
                false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.SpotterAssigned, true,
                x => x.HasExcavation, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IsTheExcavationGuardedFromAccidentalEntry, true,
                x => x.HasExcavation, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IsManufacturerDataOnSiteForShoringOrShieldingEquipment, true,
                x => x.HasExcavation, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AreThereAnyVisualSignsOfPotentialSoilCollapse, true,
                x => x.HasExcavation, true, false, "Required");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IsTheExcavationSubjectToVibration, true,
                x => x.HasExcavation, true, false, "Required");

            // This denotes a safety brief thats not complete
            _viewModel.IsCreate = true;
            _viewModel.AnyPotentialWeatherHazards = false;

            // Need to manually test these as they use a logical prop for requirement
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.AllEmployeesWearingAppropriatePersonalProtectionEquipment);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.AllStructuresSupportedOrProtected);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.HasExcavation);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.HasTrafficControl);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.HaveYouInspectedSlings);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.IsMarkoutValidForSite);

            // these two props denote a completed safety brief
            _viewModel.IsCreate = false;
            _viewModel.AnyPotentialWeatherHazards = true;

            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AllEmployeesWearingAppropriatePersonalProtectionEquipment, "Required");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AllStructuresSupportedOrProtected, "Required");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HasExcavation, "Required");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsMarkoutValidForSite, "Required");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CheckListDate);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MarkoutNumber,
                JobSiteCheckList.StringLengths.MARKOUT_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.SAPWorkOrderId,
                JobSiteCheckList.StringLengths.WORK_ORDER_ID);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.HadConversationAboutWeatherHazardNotes,
                JobSiteCheckList.StringLengths.NOTES);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.HaveEquipmentToDoJobSafelyNotes,
                JobSiteCheckList.StringLengths.NOTES);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ReviewedErgonomicHazardsNotes,
                JobSiteCheckList.StringLengths.NOTES);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.OtherHazardNotes,
                JobSiteCheckList.StringLengths.NOTES);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.HadDiscussionAboutHazardsAndPrecautionsNotes,
                JobSiteCheckList.StringLengths.NOTES);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PPEOtherNotes,
                JobSiteCheckList.StringLengths.NOTES);
        }

        #endregion
    }

    [TestClass]
    public class CreateJobSiteCheckListTest : BaseJobSiteCheckListViewModelTest<CreateJobSiteCheckList>
    {
        #region Private Members

        #region Fields

        private JobSiteCheckListPressurizedRiskRestrainedType _yesPressure, _noPressure;

        #endregion

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService>().Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IMarkoutRequirementRepository>().Use<MarkoutRequirementRepository>();
            e.For<IJobSiteCheckListPressurizedRiskRestrainedTypeRepository>()
             .Use<JobSiteCheckListPressurizedRiskRestrainedTypeRepository>();
            e.For<IJobSiteCheckListRepository>().Use<JobSiteCheckListRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var mapIcon = GetFactory<MapIconFactory>().Create(new {FileName = "blergh/pin_black.ext"});
            // This needs to exist
            var requirement = GetFactory<EmergencyMarkoutRequirementFactory>().Create();

            _yesPressure = GetFactory<YesJobSiteCheckListPressurizedRiskRestrainedTypeFactory>().Create();
            _noPressure = GetFactory<NoJobSiteCheckListPressurizedRiskRestrainedTypeFactory>().Create();
        }

        #endregion

        #region Tests

        #region SetDefaults

        [TestMethod]
        public void TestIsPressurizedRisksRestrainedFieldRequiredIsSetToFalse()
        {
            _viewModel.IsPressurizedRisksRestrainedFieldRequired = false;
            _viewModel.SetDefaults();
            Assert.IsFalse(_viewModel.IsPressurizedRisksRestrainedFieldRequired.Value);
        }

        [TestMethod]
        public void TestSetDefaultsSetsCheckListDateToCurrentDate()
        {
            var expected = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expected);
            _viewModel.SetDefaults();
            Assert.AreEqual(expected, _viewModel.CheckListDate);
        }

        [TestMethod]
        public void TestSetDefaultsSetsOperatingCenterFromExistingWorkOrderIfWorkOrderExists()
        {
            _viewModel.MapCallWorkOrder = null;
            _viewModel.OperatingCenter = null;
            _viewModel.SetDefaults();
            Assert.IsNull(_viewModel.OperatingCenter, "Should be null because MapCallWorkOrder is not set.");

            var wo = GetFactory<WorkOrderFactory>().Create();
            _viewModel.MapCallWorkOrder = wo.Id;
            _viewModel.SetDefaults();
            Assert.AreEqual(wo.OperatingCenter.Id, _viewModel.OperatingCenter);
        }

        [TestMethod]
        public void TestSetDefaultsCreatesCoordinateFromWorkOrderLatitudeAndLongitude()
        {
            decimal expectedLat = 12;
            decimal expectedLong = 42;
            _viewModel.Coordinate = null;
            var wo = GetFactory<WorkOrderFactory>().Create(new {Latitude = expectedLat, Longitude = expectedLong});
            _viewModel.MapCallWorkOrder = wo.Id;
            _viewModel.SetDefaults();
            var coord = Session.Query<Coordinate>().Single(x => x.Id == _viewModel.Coordinate.Value);
            Assert.AreEqual(Convert.ToDecimal(expectedLat), coord.Latitude);
            Assert.AreEqual(Convert.ToDecimal(expectedLong), coord.Longitude);
            Assert.IsTrue(coord.Icon.FileName.Contains("pin_black"), "The default icon should be used.");
        }

        [TestMethod]
        public void TestSetDefaultsSetsMarkoutNumberIfCurrentMarkoutIsNotNull()
        {
            var wo = GetFactory<WorkOrderFactory>().Create();
            var expectedMarkout =
                GetFactory<MarkoutFactory>().Create(new {WorkOrder = wo, DateOfRequest = DateTime.Now, ReadyDate = DateTime.Now, ExpirationDate = DateTime.Now.AddDays(1) });
            var unexpectedMarkout =
                GetFactory<MarkoutFactory>().Create(new {WorkOrder = wo, DateOfRequest = DateTime.Now.AddDays(-2), ReadyDate = DateTime.Now.AddDays(-2), ExpirationDate = DateTime.Now.AddDays(-1) });
            Session.Save(expectedMarkout);
            Session.Save(unexpectedMarkout);
            Session.Clear();
            wo = Session.Load<WorkOrder>(wo.Id);
            _viewModel.MapCallWorkOrder = wo.Id;

            _viewModel.SetDefaults();
            Assert.AreEqual(expectedMarkout.MarkoutNumber, _viewModel.MarkoutNumber);
        }

        [TestMethod]
        public void TestSetDefaultsSetsIsEmergencyMarkoutRequestIfWorkOrderMarkoutRequirementIsEmergency()
        {
            var emergency = Session.Get<MarkoutRequirement>((int)MarkoutRequirement.Indices.EMERGENCY);
            var wo = GetFactory<WorkOrderFactory>().Create(new {
                MarkoutRequirement = emergency
            });
            _viewModel.MapCallWorkOrder = wo.Id;
            _viewModel.IsEmergencyMarkoutRequest = null;

            _viewModel.SetDefaults();
            Assert.IsTrue(_viewModel.IsEmergencyMarkoutRequest.Value);

            wo.MarkoutRequirement = null;
            _viewModel.IsEmergencyMarkoutRequest = null;
            _viewModel.SetDefaults();
            Assert.IsNull(_viewModel.IsEmergencyMarkoutRequest,
                "This is supposed to be null, not false, if the work order is not an emergency. Why? Because.");
        }

        [TestMethod]
        public void TestSetDefaultsSetsCompetentEmployeeToTheEmployeeThatMatchesTheEmployeeNumberOfTheCurrentUser()
        {
            var employee = GetFactory<EmployeeFactory>().Create(new {EmployeeId = "123"});
            _user.Employee = employee;
            var wo = GetFactory<WorkOrderFactory>().Create();
            _viewModel.MapCallWorkOrder = wo.Id;

            _viewModel.SetDefaults();
            Assert.AreEqual(employee.Id, _viewModel.CompetentEmployee);

            _viewModel.CompetentEmployee = null;
            _user.Employee = null;
            //_user.EmployeeId = "not 123";
            _viewModel.SetDefaults();
            Assert.IsNull(_viewModel.CompetentEmployee, "This should be null if there is not a matching employee.");
        }

        #endregion

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();
            _vmTester.CanMapBothWays(x => x.IsPressurizedRisksRestrainedFieldRequired, false, true);
        }

        [TestMethod]
        public void TestMapToEntityAddsANewJobSiteCheckListCommentIftheCommentFieldHasAValue()
        {
            var expected = "Here's some comments for you!";
            _viewModel.Comments = expected;

            _entity.Comments.Clear();
            _vmTester.MapToEntity();

            Assert.AreEqual(expected, _entity.Comments.Single().Comments);

            _entity.Comments.Clear();
            _viewModel.Comments = null;
            _vmTester.MapToEntity();

            Assert.IsFalse(_entity.Comments.Any());
        }

        [TestMethod]
        public void TestMapToEntitySetsCreatedByAndCreatedOnToTheEntitiesValuesForNewComments()
        {
            var expectedCreatedBy = _user;
            var expectedComments = "Here's some comments for you!";
            _viewModel.Comments = expectedComments;

            _entity.Comments.Clear();
            _vmTester.MapToEntity();
            var result = _entity.Comments.Single();
            Assert.AreEqual(expectedComments, result.Comments);
        }

        [TestMethod]
        public void TestMapToEntityAddsANewJobSiteCheckListCrewMembersItemIftheCrewMembersFieldHasAValue()
        {
            var expected = "Here's some crew members for you!";
            _viewModel.CrewMembers = expected;

            _entity.CrewMembers.Clear();
            _vmTester.MapToEntity();

            Assert.AreEqual(expected, _entity.CrewMembers.Single().CrewMembers);

            _entity.CrewMembers.Clear();
            _viewModel.CrewMembers = null;
            _vmTester.MapToEntity();

            Assert.IsFalse(_entity.CrewMembers.Any());
        }

        [TestMethod]
        public void TestMapToEntityCreatesNewJobSiteCheckListCrewMembersItems()
        {
            var expectedCrewMembers = "Here's some crew members for you!";
            _viewModel.CrewMembers = expectedCrewMembers;

            _entity.CrewMembers.Clear();
            _vmTester.MapToEntity();
            var result = _entity.CrewMembers.Single();
            Assert.AreEqual(expectedCrewMembers, result.CrewMembers);
        }

        [TestMethod]
        public void TestMapSetsIsCreateToTrue()
        {
            _viewModel.Map(_entity);
            Assert.IsTrue(_viewModel.IsCreate.Value);
        }

        #endregion

        #endregion
    }

    [TestClass]
    public class EditJobSiteCheckListTest : BaseJobSiteCheckListViewModelTest<EditJobSiteCheckList>
    {
        #region Private Members

        private JobSiteCheckListPressurizedRiskRestrainedType _yesPressure, _noPressure;

        #endregion

        #region Private Methods

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService>().Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IMarkoutRequirementRepository>().Use<MarkoutRequirementRepository>();
            e.For<IJobSiteCheckListPressurizedRiskRestrainedTypeRepository>()
             .Use<JobSiteCheckListPressurizedRiskRestrainedTypeRepository>();
            e.For<IJobSiteCheckListRepository>().Use<JobSiteCheckListRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _yesPressure = GetFactory<YesJobSiteCheckListPressurizedRiskRestrainedTypeFactory>().Create();
            _noPressure = GetFactory<NoJobSiteCheckListPressurizedRiskRestrainedTypeFactory>().Create();
        }

        #endregion

        [TestMethod]
        public void TestIsPressurizedRisksRestrainedFieldRequiredIsSetTrue()
        {
            _viewModel.IsPressurizedRisksRestrainedFieldRequired = true;
            _viewModel.SetDefaults();
            Assert.IsTrue(_viewModel.IsPressurizedRisksRestrainedFieldRequired.Value);
        }

        [TestMethod]
        public void TestMapPopulatesMarkoutInformationWhenNullOnEdit()
        {
            var markoutRequired = GetFactory<EmergencyMarkoutRequirementFactory>().Create();
            var wo = GetEntityFactory<WorkOrder>().Create(new {MarkoutRequirement = markoutRequired});
            var markout = GetEntityFactory<Markout>().Create(new {WorkOrder = wo});
            Session.Save(markout);
            Session.Clear();
            wo = Session.Load<WorkOrder>(wo.Id);
            _entity.MapCallWorkOrder = wo;
            _entity.MarkoutNumber = null;
            _viewModel.Map(_entity);
            Assert.AreEqual(_viewModel.MarkoutNumber, markout.MarkoutNumber);
            Assert.AreEqual(_viewModel.IsEmergencyMarkoutRequest, true);
        }

        [TestMethod]
        public void TestMapSetsIsCreateToFalse()
        {
            _viewModel.Map(_entity);
            Assert.IsFalse(_viewModel.IsCreate.Value);
        }

        [TestMethod]
        public void TestMapToEntityAddsANewJobSiteCheckListCommentIftheCommentFieldHasAValue()
        {
            var expected = "Here's some comments for you!";
            _viewModel.Comments = expected;

            _entity.Comments.Clear();
            _vmTester.MapToEntity();

            Assert.AreEqual(expected, _entity.Comments.Single().Comments);

            _entity.Comments.Clear();
            _viewModel.Comments = null;
            _vmTester.MapToEntity();

            Assert.IsFalse(_entity.Comments.Any());
        }

        [TestMethod]
        public void TestMapToEntityAddsANewJobSiteCheckListCrewMembersItemIftheCrewMembersFieldHasAValue()
        {
            var expected = "Here's some crew members for you!";
            _viewModel.CrewMembers = expected;

            _entity.CrewMembers.Clear();
            _vmTester.MapToEntity();

            Assert.AreEqual(expected, _entity.CrewMembers.Single().CrewMembers);

            _entity.CrewMembers.Clear();
            _viewModel.CrewMembers = null;
            _vmTester.MapToEntity();

            Assert.IsFalse(_entity.CrewMembers.Any());
        }

        [TestMethod]
        public void TestMapToEntityCreatesNewJobSiteCheckListCrewMembersItems()
        {
            var expectedCrewMembers = "Here's some crew members for you!";
            _viewModel.CrewMembers = expectedCrewMembers;
            _entity.CrewMembers.Clear();

            _vmTester.MapToEntity();
            var result = _entity.CrewMembers.Single();
            Assert.AreEqual(expectedCrewMembers, result.CrewMembers);
        }
    }
}
