using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Production.ViewModels.PreJobSafetyBriefs
{
    public abstract class ProductionPreJobSafetyBriefViewModelTestBase<TViewModel>
        : ViewModelTestBase<ProductionPreJobSafetyBrief, TViewModel>
        where TViewModel : ProductionPreJobSafetyBriefViewModelBase
    {
        #region Fields

        protected Mock<IAuthenticationService<User>> _authServ;
        public Mock<IDateTimeProvider> _dateTimeProvider;
        public User _user;
        public IList<ProductionPreJobSafetyBriefWeatherHazardType> _weatherHazardTypes;
        public IList<ProductionPreJobSafetyBriefTrafficHazardType> _trafficHazardTypes;
        public IList<ProductionPreJobSafetyBriefTimeOfDayConstraintType> _timeOfDayConstraintTypes;
        public IList<ProductionPreJobSafetyBriefOverheadHazardType> _overheadHazardTypes;
        public IList<ProductionPreJobSafetyBriefUndergroundHazardType> _undergroundHazardTypes;
        public IList<ProductionPreJobSafetyBriefElectricalHazardType> _electricalHazardTypes;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _weatherHazardTypes =
                GetEntityFactory<ProductionPreJobSafetyBriefWeatherHazardType>().CreateList();
            _trafficHazardTypes =
                GetEntityFactory<ProductionPreJobSafetyBriefTrafficHazardType>().CreateList();
            _timeOfDayConstraintTypes =
                GetEntityFactory<ProductionPreJobSafetyBriefTimeOfDayConstraintType>().CreateList();
            _overheadHazardTypes =
                GetEntityFactory<ProductionPreJobSafetyBriefOverheadHazardType>().CreateList();
            _undergroundHazardTypes =
                GetEntityFactory<ProductionPreJobSafetyBriefUndergroundHazardType>().CreateList();
            _electricalHazardTypes =
                GetEntityFactory<ProductionPreJobSafetyBriefElectricalHazardType>().CreateList();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            _authServ = e.For<IAuthenticationService<User>>().Mock();
        }

        protected override TViewModel CreateViewModel()
        {
            var vm = base.CreateViewModel();
            // This always needs to be true for a valid model.
            vm.HaveAllHazardsAndPrecautionsBeenReviewed = true;
            return vm;
        }

        #region Validation

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // noop
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            // Some of these properties are true by default. They need to be set to false first
            // or else CanMapBothWays fails due to the default test values used for the specific
            // overload for bools.
            _vmTester
               .CanMapBothWays(x => x.AnyPotentialOverheadHazards)
               .CanMapBothWays(x => x.AnyPotentialWeatherHazards)
               .CanMapBothWays(x => x.AnyTimeOfDayConstraints)
               .CanMapBothWays(x => x.AnyTrafficHazards)
               .CanMapBothWays(x => x.AnyUndergroundHazards)
               .CanMapBothWays(x => x.AnyWorkPerformedGreaterThanOrEqualToFourFeetAboveGroundLevel)
               .CanMapBothWays(x => x.AreThereElectricalOrOtherEnergyHazards);
            _viewModel.CrewMembersRemindedOfStopWorkAuthority = false;
            _vmTester
               .CanMapBothWays(x => x.CrewMembersRemindedOfStopWorkAuthority)
               .CanMapBothWays(x => x.CrewMembersRemindedOfStopWorkAuthorityNotes)
               .CanMapBothWays(x => x.DoesJobInvolveUseOfChemicals)
               .CanMapBothWays(x => x.ElectricalProtection)
               .CanMapBothWays(x => x.EyeProtection)
               .CanMapBothWays(x => x.FaceShield)
               .CanMapBothWays(x => x.FallProtection)
               .CanMapBothWays(x => x.FootProtection)
               .CanMapBothWays(x => x.HadConversationAboutWeatherHazard)
               .CanMapBothWays(x => x.HadConversationAboutWeatherHazardNotes);
            _viewModel.HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection = false;
            _vmTester
               .CanMapBothWays(x => x.HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection)
               .CanMapBothWays(x =>
                    x.HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspectionNotes);
            _viewModel.HasStretchAndFlexBeenPerformed = false;
            _vmTester
               .CanMapBothWays(x => x.HasStretchAndFlexBeenPerformed)
               .CanMapBothWays(x => x.HasStretchAndFlexBeenPerformedNotes);
            _viewModel.HaveEquipmentToDoJobSafely = false;
            _vmTester
               .CanMapBothWays(x => x.HaveEquipmentToDoJobSafely)
               .CanMapBothWays(x => x.HaveEquipmentToDoJobSafelyNotes)
               .CanMapBothWays(x => x.HandProtection)
               .CanMapBothWays(x => x.HeadProtection)
               .CanMapBothWays(x => x.HearingProtection)
               .CanMapBothWays(x => x.IsSafetyDataSheetAvailableForEachChemical)
               .CanMapBothWays(x => x.InvolveConfinedSpace)
               .CanMapBothWays(x => x.OtherHazardsIdentified)
               .CanMapBothWays(x => x.OtherHazardNotes)
               .CanMapBothWays(x => x.PPEOther)
               .CanMapBothWays(x => x.PPEOtherNotes)
               .CanMapBothWays(x => x.RespiratoryProtection);
            _viewModel.ReviewedErgonomicHazards = false;
            _vmTester
               .CanMapBothWays(x => x.ReviewedErgonomicHazards)
               .CanMapBothWays(x => x.ReviewedErgonomicHazardsNotes);
            _viewModel.ReviewedLocationOfSafetyEquipment = false;
            _vmTester
               .CanMapBothWays(x => x.ReviewedLocationOfSafetyEquipment)
               .CanMapBothWays(x => x.ReviewedLocationOfSafetyEquipmentNotes)
               .CanMapBothWays(x => x.SafetyBriefDateTime)
               .CanMapBothWays(x => x.IsSafetyDataSheetAvailableForEachChemicalNotes)
               .CanMapBothWays(x => x.SafetyGarment)
               .CanMapBothWays(x => x.TypeOfFallPreventionProtectionSystemBeingUsed);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert
               .PropertyIsRequired(x => x.AnyPotentialOverheadHazards)
               .PropertyIsRequired(x => x.AnyPotentialWeatherHazards)
               .PropertyIsRequired(x => x.AnyTimeOfDayConstraints)
               .PropertyIsRequired(x => x.AnyTrafficHazards)
               .PropertyIsRequired(x => x.AnyUndergroundHazards)
               .PropertyIsRequired(x => x.AnyWorkPerformedGreaterThanOrEqualToFourFeetAboveGroundLevel)
               .PropertyIsRequired(x => x.AreThereElectricalOrOtherEnergyHazards)
               .PropertyIsRequired(x => x.CrewMembersRemindedOfStopWorkAuthority)
               .PropertyIsRequiredWhen(
                    x => x.CrewMembersRemindedOfStopWorkAuthorityNotes,
                    "things",
                    x => x.CrewMembersRemindedOfStopWorkAuthority,
                    false,
                    true)
               .PropertyIsRequired(x => x.DoesJobInvolveUseOfChemicals)
               .PropertyIsRequiredWhen(
                    x => x.HadConversationAboutWeatherHazard,
                    true,
                    x => x.AnyPotentialWeatherHazards,
                    true,
                    false)
               .PropertyIsRequired(x =>
                    x.HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection)
               .PropertyIsRequiredWhen(
                    x => x.HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspectionNotes,
                    "valid",
                    x => x.HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection,
                    false)
               .PropertyIsRequired(x => x.HasStretchAndFlexBeenPerformed)
               .PropertyIsRequired(x => x.HasStretchAndFlexBeenPerformed)
               .PropertyIsRequiredWhen(
                    x => x.HasStretchAndFlexBeenPerformedNotes,
                    "things",
                    x => x.HasStretchAndFlexBeenPerformed,
                    false,
                    true)
               .PropertyIsRequired(
                    x => x.HaveAllHazardsAndPrecautionsBeenReviewed,
                    ProductionPreJobSafetyBriefViewModelBase.PLEASE_SPEAK_TO_YOUR_SUPERVISOR)
               .PropertyIsRequired(x => x.HaveEquipmentToDoJobSafely)
               .PropertyIsRequiredWhen(
                    x => x.HaveEquipmentToDoJobSafelyNotes,
                    "things",
                    x => x.HaveEquipmentToDoJobSafely,
                    false,
                    true)
               .PropertyIsRequired(x => x.InvolveConfinedSpace)
               .PropertyIsRequiredWhen(
                    x => x.IsSafetyDataSheetAvailableForEachChemical,
                    true,
                    x => x.DoesJobInvolveUseOfChemicals,
                    true)
               .PropertyIsRequiredWhen(
                    x => x.IsSafetyDataSheetAvailableForEachChemicalNotes,
                    "valid string",
                    x => x.IsSafetyDataSheetAvailableForEachChemical,
                    false)
               .PropertyIsRequired(x => x.OtherHazardsIdentified)
               .PropertyIsRequiredWhen(
                    x => x.OtherHazardNotes,
                    "things",
                    x => x.OtherHazardsIdentified,
                    true,
                    false)
               .PropertyIsRequiredWhen(x => x.PPEOtherNotes, "things", x => x.PPEOther, true, false)
               .PropertyIsRequired(x => x.ReviewedErgonomicHazards)
               .PropertyIsRequiredWhen(
                    x => x.ReviewedErgonomicHazardsNotes,
                    "things",
                    x => x.ReviewedErgonomicHazards,
                    false,
                    true)
               .PropertyIsRequired(x => x.ReviewedLocationOfSafetyEquipment)
               .PropertyIsRequiredWhen(
                    x => x.ReviewedLocationOfSafetyEquipmentNotes,
                    "things",
                    x => x.ReviewedLocationOfSafetyEquipment,
                    false,
                    true)
               .PropertyIsRequired(x => x.SafetyBriefDateTime)
               .PropertyIsRequiredWhen(
                    x => x.SafetyBriefElectricalHazardTypes,
                    new[] { _electricalHazardTypes.First().Id },
                    x => x.AreThereElectricalOrOtherEnergyHazards,
                    true,
                    false)
               .PropertyIsRequiredWhen(
                    x => x.SafetyBriefOverheadHazardTypes,
                    new[] { _overheadHazardTypes.First().Id },
                    x => x.AnyPotentialOverheadHazards,
                    true,
                    false)
               .PropertyIsRequiredWhen(
                    x => x.SafetyBriefTimeOfDayConstraintTypes,
                    new[] { _timeOfDayConstraintTypes.First().Id },
                    x => x.AnyTimeOfDayConstraints,
                    true,
                    false)
               .PropertyIsRequiredWhen(
                    x => x.SafetyBriefTrafficHazardTypes,
                    new[] { _trafficHazardTypes.First().Id },
                    x => x.AnyTrafficHazards,
                    true,
                    false)
               .PropertyIsRequiredWhen(
                    x => x.SafetyBriefUndergroundHazardTypes,
                    new[] { _undergroundHazardTypes.First().Id },
                    x => x.AnyUndergroundHazards,
                    true,
                    false)
               .PropertyIsRequiredWhen(
                    x => x.SafetyBriefWeatherHazardTypes,
                    new[] { _weatherHazardTypes.First().Id },
                    x => x.AnyPotentialWeatherHazards,
                    true,
                    false)
               .PropertyIsRequiredWhen(
                    x => x.TypeOfFallPreventionProtectionSystemBeingUsed,
                    "things",
                    x => x.AnyWorkPerformedGreaterThanOrEqualToFourFeetAboveGroundLevel,
                    true);
        }

        [TestMethod]
        public void TestAtLeastOneEmployeeOrContractorIsRequired()
        {
            const string EXPECTED = "At least one employee or contractor must be selected.";
            _viewModel.Employees = null;
            _viewModel.Contractors = null;
            ValidationAssert.ModelStateHasNonPropertySpecificError(EXPECTED);

            _viewModel.Employees = new int[] { };
            _viewModel.Contractors = new string[] { };
            ValidationAssert.ModelStateHasNonPropertySpecificError(EXPECTED);

            // Need to ensure we're filtering out empty strings when validating this too.
            _viewModel.Contractors = new[] {string.Empty};
            ValidationAssert.ModelStateHasNonPropertySpecificError(EXPECTED);

            _viewModel.Employees = new int[] {1};
            ValidationAssert.ModelStateIsValid(x => x.Employees);

            _viewModel.Employees = null;
            _viewModel.Contractors = new[] {"neat"};
            ValidationAssert.ModelStateIsValid(x => x.Employees);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            const int notesLength = ProductionPreJobSafetyBrief.StringLengths.NOTES;
            ValidationAssert
               .PropertyHasMaxStringLength(x => x.CrewMembersRemindedOfStopWorkAuthorityNotes, notesLength)
               .PropertyHasMaxStringLength(x => x.HadConversationAboutWeatherHazardNotes, notesLength)
               .PropertyHasMaxStringLength(
                    x => x.HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspectionNotes,
                    notesLength)
               .PropertyHasMaxStringLength(x => x.HasStretchAndFlexBeenPerformedNotes, notesLength)
               .PropertyHasMaxStringLength(x => x.HaveEquipmentToDoJobSafelyNotes, notesLength)
               .PropertyHasMaxStringLength(
                    x => x.IsSafetyDataSheetAvailableForEachChemicalNotes,
                    notesLength)
               .PropertyHasMaxStringLength(x => x.OtherHazardNotes, notesLength)
               .PropertyHasMaxStringLength(x => x.PPEOtherNotes, notesLength)
               .PropertyHasMaxStringLength(x => x.ReviewedErgonomicHazardsNotes, notesLength)
               .PropertyHasMaxStringLength(x => x.ReviewedLocationOfSafetyEquipmentNotes, notesLength)
               .PropertyHasMaxStringLength(
                    x => x.TypeOfFallPreventionProtectionSystemBeingUsed,
                    notesLength);
        }
 
        [TestMethod]
        public void TestHaveAllHazardsAndPrecautionsBeenReviewedMustBeTrue()
        {
            // null test case is already tested in the Required test above.
            _viewModel.HaveAllHazardsAndPrecautionsBeenReviewed = false;
            ValidationAssert.ModelStateHasError(
                x => x.HaveAllHazardsAndPrecautionsBeenReviewed,
                ProductionPreJobSafetyBriefViewModelBase.PLEASE_SPEAK_TO_YOUR_SUPERVISOR);

            _viewModel.HaveAllHazardsAndPrecautionsBeenReviewed = true;
            ValidationAssert.ModelStateIsValid(x => x.HaveAllHazardsAndPrecautionsBeenReviewed);
        }

        [TestMethod]
        public void TestMapBothWaysForTimeOfDayConstraintTypes()
        {
            var expected = GetEntityFactory<ProductionPreJobSafetyBriefTimeOfDayConstraintType>().Create();
            var intArray = new int[] {expected.Id};
            _viewModel.SafetyBriefTimeOfDayConstraintTypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.SafetyBriefTimeOfDayConstraintTypes.Single().Id);

            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.SafetyBriefTimeOfDayConstraintTypes[0]);
        }

        [TestMethod]
        public void TestMapBothWaysForTrafficHazardTypes()
        {
            var expected = GetEntityFactory<ProductionPreJobSafetyBriefTrafficHazardType>().Create();
            var intArray = new int[] {expected.Id};
            _viewModel.SafetyBriefTrafficHazardTypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.SafetyBriefTrafficHazardTypes.Single().Id);

            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.SafetyBriefTrafficHazardTypes[0]);
        }

        [TestMethod]
        public void TestMapBothWaysForUndergroundHazardTypes()
        {
            var expected = GetEntityFactory<ProductionPreJobSafetyBriefUndergroundHazardType>().Create();
            var intArray = new int[] {expected.Id};
            _viewModel.SafetyBriefUndergroundHazardTypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.SafetyBriefUndergroundHazardTypes.Single().Id);

            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.SafetyBriefUndergroundHazardTypes[0]);
        }

        [TestMethod]
        public void TestMapBothWaysForOverheadHazardTypes()
        {
            var expected = GetEntityFactory<ProductionPreJobSafetyBriefOverheadHazardType>().Create();
            var intArray = new int[] {expected.Id};
            _viewModel.SafetyBriefOverheadHazardTypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.SafetyBriefOverheadHazardTypes.Single().Id);

            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.SafetyBriefOverheadHazardTypes[0]);
        }

        [TestMethod]
        public void TestMapBothWaysForElectricalHazardTypes()
        {
            var expected = GetEntityFactory<ProductionPreJobSafetyBriefElectricalHazardType>().Create();
            var intArray = new int[] {expected.Id};
            _viewModel.SafetyBriefElectricalHazardTypes = intArray;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(intArray[0], _entity.SafetyBriefElectricalHazardTypes.Single().Id);

            _viewModel.Map(_entity);

            Assert.AreEqual(intArray[0], _viewModel.SafetyBriefElectricalHazardTypes[0]);
        }

        #region Mapping the Workers propperty to/from Employees and Contractors

        [TestMethod]
        public void TestMapSetsContractorsAndEmployeesFromWorkers()
        {
            var expectedEmployee = GetEntityFactory<ProductionPreJobSafetyBriefWorker>().Create(new {
                ProductionPreJobSafetyBrief = _entity
            });

            var expectedContractor = GetEntityFactory<ProductionPreJobSafetyBriefWorker>().Create(new {
                ProductionPreJobSafetyBrief = _entity,
                Employee = (Employee)null,
                Contractor = "Some contractor"
            });

            _vmTester.MapToViewModel();
            
            Assert.AreEqual(1, _viewModel.Employees.Count());
            Assert.AreEqual(1, _viewModel.Contractors.Count());
            Assert.IsTrue(_viewModel.Employees.Contains(expectedEmployee.Employee.Id));
            Assert.IsTrue(_viewModel.Contractors.Contains("Some contractor"));
        }

        [TestMethod]
        public void TestMapToEntityRemovesRemovedEmployeesFromWorkers()
        {
            var expectedToRemove = GetEntityFactory<ProductionPreJobSafetyBriefWorker>().Create(new {
                ProductionPreJobSafetyBrief = _entity
            });
            var expectedToStay = GetEntityFactory<ProductionPreJobSafetyBriefWorker>().Create(new {
                ProductionPreJobSafetyBrief = _entity
            });
            _viewModel.Employees = new[] { expectedToStay.Employee.Id };

            _vmTester.MapToEntity();

            Assert.AreSame(expectedToStay, _entity.Workers.Single());
        }

        [TestMethod]
        public void TestMapToEntityAddsNewEmployeesWithCurrentDateTimeForSignedAtToTheWorkersProperty()
        {
            var expectedDate = new DateTime(1984, 4, 24, 4, 0, 4);
            var expectedEmployee = GetEntityFactory<Employee>().Create();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            _viewModel.Employees = new[] { expectedEmployee.Id };

            _vmTester.MapToEntity();

            var result = _entity.Workers.Single();
            Assert.AreSame(expectedEmployee, result.Employee);
            Assert.AreEqual(expectedDate, result.SignedAt);
            Assert.AreSame(_entity, result.ProductionPreJobSafetyBrief);
        }

        [TestMethod]
        public void
            TestMapToEntityDoesNotUpdateExistingWorkersSignedAtDateTimeForEmployeesThatAlreadySigned()
        {
            var existingDate = DateTime.Now;
            var existing = GetEntityFactory<ProductionPreJobSafetyBriefWorker>().Create(new {
                ProductionPreJobSafetyBrief = _entity,
                SignedAt = existingDate
            });
            var expectedDate = new DateTime(1984, 4, 24, 4, 0, 4);
            var expectedEmployee = GetEntityFactory<Employee>().Create();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            _viewModel.Employees = new[] { expectedEmployee.Id, existing.Employee.Id };

            _vmTester.MapToEntity();

            Assert.AreEqual(2, _entity.Workers.Count);
            Assert.IsTrue(
                _entity.Workers.Contains(existing),
                "Sanity check. Make sure we didn't replace the existing one with a new instance.");
            Assert.AreEqual(
                existingDate,
                existing.SignedAt,
                "Sanity check. Make sure the date wasn't modified.");
            var newResult = _entity.Workers.Single(x => x.Id != existing.Id);
            Assert.AreEqual(expectedDate, newResult.SignedAt);
        }

        [TestMethod]
        public void TestMapToEntityRemovesRemovedContractorsFromWorkers()
        {
            var expectedToRemove = GetEntityFactory<ProductionPreJobSafetyBriefWorker>().Create(new {
                ProductionPreJobSafetyBrief = _entity,
                Contractor = "Some removed contractor",
                Employee = (Employee)null
            });
            var expectedToStay = GetEntityFactory<ProductionPreJobSafetyBriefWorker>().Create(new {
                ProductionPreJobSafetyBrief = _entity,
                Contractor = "Some contractor",
                Employee = (Employee)null
            });
            _viewModel.Contractors = new[] { "Some contractor" };

            _vmTester.MapToEntity();

            Assert.AreSame(expectedToStay, _entity.Workers.Single());
        }
         
        [TestMethod]
        public void TestMapToEntityDoesNotAddEmptyStringContractorsToWorkersProperty()
        {
            _viewModel.Employees = null;
            _viewModel.Contractors = new[] {string.Empty};
            _vmTester.MapToEntity();

            Assert.IsFalse(_entity.Workers.Any());
        }

        [TestMethod]
        public void
            TestMapToEntityAddsNewContractorsAsWorkersWithCurrentDateTimeForSignedAtToWorkersProperty()
        {
            var expectedDate = new DateTime(1984, 4, 24, 4, 0, 4);
            var expectedContractor = "Some contractor";
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            _viewModel.Contractors = new[] { expectedContractor };

            _vmTester.MapToEntity();

            var result = _entity.Workers.Single();
            Assert.AreSame(expectedContractor, result.Contractor);
            Assert.AreEqual(expectedDate, result.SignedAt);
            Assert.AreSame(_entity, result.ProductionPreJobSafetyBrief);
        }

        [TestMethod]
        public void
            TestMapToEntityDoesNotUpdateExistingWorkersSignedAtDateTimeForContractorsThatAlreadySigned()
        {
            var existingDate = DateTime.Now;
            var expectedContractor = "Some contractor";
            var existing = GetEntityFactory<ProductionPreJobSafetyBriefWorker>().Create(new {
                ProductionPreJobSafetyBrief = _entity, 
                SignedAt = existingDate,
                Employee = (Employee)null,
                Contractor = expectedContractor
            });
            var expectedDate = new DateTime(1984, 4, 24, 4, 0, 4);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            _viewModel.Contractors = new[] { expectedContractor };

            _vmTester.MapToEntity();

            Assert.AreEqual(1, _entity.Workers.Count);
            Assert.IsTrue(
                _entity.Workers.Contains(existing),
                "Sanity check. Make sure we didn't replace the existing one with a new instance.");
            Assert.AreEqual(
                existingDate,
                existing.SignedAt,
                "Sanity check. Make sure the date wasn't modified.");
        }

        #endregion

        #endregion
    }
}
