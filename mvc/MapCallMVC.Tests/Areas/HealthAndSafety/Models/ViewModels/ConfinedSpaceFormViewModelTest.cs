using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Models.ViewModels
{
    public abstract class ConfinedSpaceFormViewModelTest<TViewModel> : ViewModelTestBase<ConfinedSpaceForm, TViewModel> where TViewModel : ConfinedSpaceFormViewModel
    {
        #region Fields

        protected Mock<IAuthenticationService<User>> _authServ;
        protected User _user;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        private ConfinedSpaceFormHazardType _hazardType1, _hazardType2;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _authServ = new Mock<IAuthenticationService<User>>();
            _user = GetEntityFactory<User>().Create();
            _user.Employee = GetEntityFactory<Employee>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);

            _hazardType1 = GetEntityFactory<ConfinedSpaceFormHazardType>().Create(new { Description = "Hazard One" });
            _hazardType2 = GetEntityFactory<ConfinedSpaceFormHazardType>().Create(new { Description = "Hazard Two" });
            _viewModel.SetDefaults();
        }

        #endregion

        #region Tests

        #region SetDefaults

        [TestMethod]
        public void TestSetDefaultsCreatesHazardViewModelsForEachHazardType()
        {
            // The setup for this test is done in the test initialization. Calling
            // SetDefaults again just to make it obvious. 
            _viewModel.SetDefaults();

            Assert.AreEqual(2, _viewModel.Hazards.Count);
            var one = _viewModel.Hazards[0];
            Assert.AreEqual("Hazard One", one.HazardTypeDescription);
            Assert.AreEqual(_hazardType1.Id, one.HazardType);
            Assert.IsNull(one.Notes);
            Assert.IsFalse(one.IsChecked.Value);

            var two = _viewModel.Hazards[1];
            Assert.AreEqual("Hazard Two", two.HazardTypeDescription);
            Assert.AreEqual(_hazardType2.Id, two.HazardType);
            Assert.IsNull(two.Notes);
            Assert.IsFalse(two.IsChecked.Value);
        }

        #endregion

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.CanBeControlledByVentilationAlone);
            _vmTester.CanMapBothWays(x => x.EmergencyResponseAgency);
            _vmTester.CanMapBothWays(x => x.EmergencyResponseContact);
            _vmTester.CanMapBothWays(x => x.HasFallSafetyEquipment);
            _vmTester.CanMapBothWays(x => x.HasFootSafetyEquipment);
            _vmTester.CanMapBothWays(x => x.GeneralDateTime);
            _vmTester.CanMapBothWays(x => x.HasAccessSafetyEquipment);
            _vmTester.CanMapBothWays(x => x.HasContractRescueService);
            _vmTester.CanMapBothWays(x => x.HasEyeSafetyEquipment);
            _vmTester.CanMapBothWays(x => x.HasGFCISafetyEquipment);
            _vmTester.CanMapBothWays(x => x.HasHandSafetyEquipment);
            _vmTester.CanMapBothWays(x => x.HasHeadSafetyEquipment);
            _vmTester.CanMapBothWays(x => x.HasLightingSafetyEquipment);
            _vmTester.CanMapBothWays(x => x.HasOtherSafetyEquipment);
            _vmTester.CanMapBothWays(x => x.HasOtherSafetyEquipmentNotes);
            _vmTester.CanMapBothWays(x => x.HasRespiratorySafetyEquipment);
            _vmTester.CanMapBothWays(x => x.HasRetrievalSystem);
            _vmTester.CanMapBothWays(x => x.HasVentilationSafetyEquipment);
            _vmTester.CanMapBothWays(x => x.HasWarningSafetyEquipment);
            _vmTester.CanMapBothWays(x => x.IsFireWatchRequired);
            _vmTester.CanMapBothWays(x => x.IsHotWorkPermitRequired);
            _vmTester.CanMapBothWays(x => x.LocationAndDescriptionOfConfinedSpace);
            _vmTester.CanMapBothWays(x => x.MethodOfCommunication, GetEntityFactory<ConfinedSpaceFormMethodOfCommunication>().Create());
            _vmTester.CanMapBothWays(x => x.MethodOfCommunicationOtherNotes);
            _vmTester.CanMapBothWays(x => x.PermitBeginsAt);
            _vmTester.CanMapBothWays(x => x.PermitEndsAt);
            _vmTester.CanMapBothWays(x => x.ProductionWorkOrder, GetEntityFactory<ProductionWorkOrder>().Create());
            _vmTester.CanMapBothWays(x => x.PurposeOfEntry);
            _vmTester.CanMapBothWays(x => x.PermitCancellationNote);
        }

        [TestMethod]
        public void TestOperatingCenterGetsValueFromProductionWorkOrder()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create();
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = opc });
            _entity.ProductionWorkOrder = pwo;

            _vmTester.MapToViewModel();

            Assert.AreEqual(opc.Id, _viewModel.OperatingCenter);
        }

        #region Mapping Hazards

        [TestMethod]
        public void TestMapMapsExistingHazardsToTheHazardViewModels()
        {
            var hazard = GetEntityFactory<ConfinedSpaceFormHazard>().Create(new
            {
                ConfinedSpaceForm = _entity,
                HazardType = _hazardType1,
                Notes = "Some notes"
            });
            _entity.Hazards.Add(hazard);

            _vmTester.MapToViewModel();

            Assert.AreEqual(2, _viewModel.Hazards.Count, "The view model should still only have two hazard view models. They were setup in SetDefaults in the test init.");
            var one = _viewModel.Hazards[0];
            Assert.AreEqual(_hazardType1.Description, one.HazardTypeDescription);
            Assert.AreEqual(_hazardType1.Id, one.HazardType);
            Assert.AreEqual("Some notes", one.Notes);
            Assert.IsTrue(one.IsChecked.Value);

            // This view model should be untouched since the entity would not have one that matches the hazard type.
            var two = _viewModel.Hazards[1];
            Assert.AreEqual("Hazard Two", two.HazardTypeDescription);
            Assert.AreEqual(_hazardType2.Id, two.HazardType);
            Assert.IsNull(two.Notes);
            Assert.IsFalse(two.IsChecked.Value);
        }

        #endregion

        #region Map and signing logic

        [TestMethod]
        public void TestMapSetsIsHazardSectionToTrueIfHazardSectionHasBeenSigned()
        {
            _viewModel.IsHazardSectionSigned = null;

            _entity.HazardSignedBy = GetEntityFactory<Employee>().Create();
            _vmTester.MapToViewModel();
            Assert.IsTrue(_viewModel.IsHazardSectionSigned.Value);

            _entity.HazardSignedBy = null;
            _vmTester.MapToViewModel();
            Assert.IsFalse(_viewModel.IsHazardSectionSigned.Value);
        }

        [TestMethod]
        public void TestMapSetsIsReclassificationSectionToTrueIfReclassificationSectionHasBeenSigned()
        {
            _viewModel.IsReclassificationSectionSigned = null;

            _entity.ReclassificationSignedBy = GetEntityFactory<Employee>().Create();
            _vmTester.MapToViewModel();
            Assert.IsTrue(_viewModel.IsReclassificationSectionSigned.Value);

            _entity.ReclassificationSignedBy = null;
            _vmTester.MapToViewModel();
            Assert.IsFalse(_viewModel.IsReclassificationSectionSigned.Value);
        }

        [TestMethod]
        public void TestMapSetsIsBeginEntrySectionSignedToTrueIfBeginEntrySectionHasBeenSigned()
        {
            _viewModel.IsBeginEntrySectionSigned = null;

            _entity.BeginEntryAuthorizedBy = GetEntityFactory<Employee>().Create();
            _vmTester.MapToViewModel();
            Assert.IsTrue(_viewModel.IsBeginEntrySectionSigned.Value);

            _entity.BeginEntryAuthorizedBy = null;
            _vmTester.MapToViewModel();
            Assert.IsFalse(_viewModel.IsBeginEntrySectionSigned.Value);
        }

        [TestMethod]
        public void TestMapSetsIsBumpTestConfirmedToTrueIfIsBumpTestConfirmedHasBeenSigned()
        {
            _viewModel.IsBumpTestConfirmed = null;

            _entity.BumpTestConfirmedBy = GetEntityFactory<Employee>().Create();
            _vmTester.MapToViewModel();
            Assert.IsTrue(_viewModel.IsBumpTestConfirmed.Value);

            _entity.BumpTestConfirmedBy = null;
            _vmTester.MapToViewModel();
            Assert.IsFalse(_viewModel.IsBumpTestConfirmed.Value);
        }

        [TestMethod]
        public void TestMapSetsIsPermitCancelledSectionSignedToTrueIfPermitCancelledSectionHasBeenSigned()
        {
            _viewModel.IsPermitCancelledSectionSigned = null;

            _entity.PermitCancelledBy = GetEntityFactory<Employee>().Create();
            _vmTester.MapToViewModel();
            Assert.IsTrue(_viewModel.IsPermitCancelledSectionSigned.Value);

            _entity.PermitCancelledBy = null;
            _vmTester.MapToViewModel();
            Assert.IsFalse(_viewModel.IsPermitCancelledSectionSigned.Value);
        }

        #endregion

        #region MapToEntity and hazards

        [TestMethod]
        public void TestMapToEntityCreatesNewHazardsForAnyCheckedHazards()
        {
            var hazard1Vm = _viewModel.Hazards.Single(x => x.HazardType == _hazardType1.Id);
            hazard1Vm.IsChecked = true;
            hazard1Vm.Notes = "These are notes";

            _vmTester.MapToEntity();

            var entityHazard = _entity.Hazards.Single();
            Assert.AreEqual("These are notes", entityHazard.Notes);
            Assert.AreSame(_entity, entityHazard.ConfinedSpaceForm);
            Assert.AreSame(_hazardType1, entityHazard.HazardType);
        }

        [TestMethod]
        public void TestMapToEntityUpdatesAnyExistingHazardsThatAreStillChecked()
        {
            var hazard = GetEntityFactory<ConfinedSpaceFormHazard>().Create(new
            {
                ConfinedSpaceForm = _entity,
                HazardType = _hazardType1,
                Notes = "Some notes"
            });
            _entity.Hazards.Add(hazard);

            var hazard1Vm = _viewModel.Hazards.Single(x => x.HazardType == _hazardType1.Id);
            hazard1Vm.IsChecked = true;
            hazard1Vm.Notes = "These are notes";

            _vmTester.MapToEntity();

            var entityHazard = _entity.Hazards.Single();
            Assert.AreEqual("These are notes", entityHazard.Notes);
            Assert.AreSame(_entity, entityHazard.ConfinedSpaceForm);
            Assert.AreSame(_hazardType1, entityHazard.HazardType);
        }

        [TestMethod]
        public void TestMapToEntityDeletesAnyHazardsThatWerePreviouslyCheckedButThenUnchecked()
        {
            var hazard = GetEntityFactory<ConfinedSpaceFormHazard>().Create(new
            {
                ConfinedSpaceForm = _entity,
                HazardType = _hazardType1,
                Notes = "Some notes"
            });
            _entity.Hazards.Add(hazard);

            _vmTester.MapToEntity();

            Assert.IsFalse(_entity.Hazards.Any());
        }

        [TestMethod]
        public void TestMapToEntityDoesNotDoesNotChangeAnythingAboutExistingHazardsIfTheHazardsPropertyIsNull()
        {
            var hazard = GetEntityFactory<ConfinedSpaceFormHazard>().Create(new
            {
                ConfinedSpaceForm = _entity,
                HazardType = _hazardType1,
                Notes = "Some notes"
            });
            _entity.Hazards.Add(hazard);
            _viewModel.Hazards = null;

            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.Hazards.Contains(hazard), "The existing hazard should not have been removed.");
            Assert.AreEqual(1, _entity.Hazards.Count, "No duplicates should have been created.");
        }

        #endregion

        #region MapToEntity and authorization to begin entry logic

        [TestMethod]
        public void TestMapToEntityDoesNotSetBeginEntrySignatureFieldsWhenIsBeginEntrySectionSignedIsNullOrFalse()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            _user.Employee = goodEmployee;
            _entity.BeginEntryAuthorizedAt = null;
            _entity.BeginEntryAuthorizedBy = null;

            _viewModel.IsBeginEntrySectionSigned = null;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.BeginEntryAuthorizedAt, "BeginEntryAuthorizedAt must be unchanged if IsBeginEntrySectionSigned is null.");
            Assert.IsNull(_entity.BeginEntryAuthorizedBy, "BeginEntryAuthorizedBy must be unchanged if IsBeginEntrySectionSigned is null.");

            _viewModel.IsBeginEntrySectionSigned = false;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.BeginEntryAuthorizedAt, "BeginEntryAuthorizedAt must be unchanged if IsBeginEntrySectionSigned is false.");
            Assert.IsNull(_entity.BeginEntryAuthorizedBy, "BeginEntryAuthorizedBy must be unchanged if IsBeginEntrySectionSigned is false.");
        }

        [TestMethod]
        public void TestMapToEntitySetsBeginEntrySignatureFieldsWhenIsBeginEntrySectionSignedIsTrueAndThefieldsHaveNotBeenSetYet()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            _user.Employee = goodEmployee;
            _entity.BeginEntryAuthorizedAt = null;
            _entity.BeginEntryAuthorizedBy = null;

            _viewModel.IsBeginEntrySectionSigned = true;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.BeginEntryAuthorizedAt, "BeginEntryAuthorizedAt must be set to current datetime when signing.");
            Assert.AreSame(goodEmployee, _entity.BeginEntryAuthorizedBy, "BeginEntryAuthorizedBy must be set to the current employee when signing.");
        }

        [TestMethod]
        public void TestMapToEntityDoesNotOverwriteBeginEntrySignatureFieldsIfIsBeginEntrySectionSignedIsTrueAndTheFieldsWereAlreadySet()
        {
            var expectedDate = DateTime.Now;
            var unexpectedDate = DateTime.Now.AddDays(1);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(unexpectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            var someOtherEmployee = GetEntityFactory<Employee>().Create();
            _entity.BeginEntryAuthorizedAt = expectedDate;
            _entity.BeginEntryAuthorizedBy = goodEmployee;
            _user.Employee = someOtherEmployee;
            _viewModel.IsBeginEntrySectionSigned = true;

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedDate, _entity.BeginEntryAuthorizedAt, "BeginEntryAuthorizedAt must not change if it has already been set.");
            Assert.AreSame(goodEmployee, _entity.BeginEntryAuthorizedBy, "BeginEntryAuthorizedBy must not change if it has already been set.");
        }

        #endregion

        #region MapToEntity and bump confirmation

        [TestMethod]
        public void TestMapToEntityDoesNotSetBumpTestConfirmationFieldsWhenIsBumpTestConfirmedIsNullOrFalse()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            _user.Employee = goodEmployee;
            _entity.BumpTestConfirmedAt = null;
            _entity.BumpTestConfirmedBy = null;

            _viewModel.IsBumpTestConfirmed = null;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.BumpTestConfirmedAt, "BumpTestConfirmedAt must be unchanged if IsBumpTestConfirmed is null.");
            Assert.IsNull(_entity.BumpTestConfirmedBy, "BumpTestConfirmedBy must be unchanged if IsBumpTestConfirmed is null.");

            _viewModel.IsBumpTestConfirmed = false;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.BumpTestConfirmedAt, "BumpTestConfirmedAt must be unchanged if IsBumpTestConfirmed is false.");
            Assert.IsNull(_entity.BumpTestConfirmedBy, "BumpTestConfirmedBy must be unchanged if IsBumpTestConfirmed is false.");
        }

        [TestMethod]
        public void TestMapToEntitySetsBumpTestConfirmationFieldsWhenIsBumpTestConfirmedIsTrueAndThefieldsHaveNotBeenSetYet()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            _user.Employee = goodEmployee;
            _entity.BumpTestConfirmedAt = null;
            _entity.BumpTestConfirmedBy = null;

            _viewModel.IsBumpTestConfirmed = true;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.BumpTestConfirmedAt, "BumpTestConfirmedAt must be set to current datetime when signing.");
            Assert.AreSame(goodEmployee, _entity.BumpTestConfirmedBy, "BumpTestConfirmedBy must be set to the current employee when signing.");
        }

        [TestMethod]
        public void TestMapToEntityDoesNotOverwriteBumpTestConfirmationFieldsIfIsBumpTestConfirmedIsTrueAndTheFieldsWereAlreadySet()
        {
            var expectedDate = DateTime.Now;
            var unexpectedDate = DateTime.Now.AddDays(1);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(unexpectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            var someOtherEmployee = GetEntityFactory<Employee>().Create();
            _entity.BumpTestConfirmedAt = expectedDate;
            _entity.BumpTestConfirmedBy = goodEmployee;
            _user.Employee = someOtherEmployee;
            _viewModel.IsBumpTestConfirmed = true;

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedDate, _entity.BumpTestConfirmedAt, "BumpTestConfirmedAt must not change if it has already been set.");
            Assert.AreSame(goodEmployee, _entity.BumpTestConfirmedBy, "BumpTestConfirmedBy must not change if it has already been set.");
        }

        [TestMethod]
        public void TestMapToEntitySetsGasMonitorFromEntityWhenBumpTestIsConfirmedAndViewModelGasMonitorIsNull()
        {
            var gasMonitor = GetEntityFactory<GasMonitor>().Create();
            _viewModel.GasMonitor = null;
            _entity.GasMonitor = gasMonitor;
            _entity.BumpTestConfirmedBy = _user.Employee;
            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(_entity.GasMonitor, gasMonitor);

        }

        #endregion

        #region MapToEntity and hazard signing logic

        [TestMethod]
        public void TestMapToEntityDoesNotSetHazardSignatureFieldsWhenIsHazardSectionSignedIsNullOrFalse()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            _user.Employee = goodEmployee;
            _entity.HazardSignedAt = null;
            _entity.HazardSignedBy = null;

            _viewModel.IsHazardSectionSigned = null;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.HazardSignedAt, "HazardSignedAt must be unchanged if IsHazardSectionSigned is null.");
            Assert.IsNull(_entity.HazardSignedBy, "HazardSignedBy must be unchanged if IsHazardSectionSigned is null.");

            _viewModel.IsHazardSectionSigned = false;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.HazardSignedAt, "HazardSignedAt must be unchanged if IsHazardSectionSigned is false.");
            Assert.IsNull(_entity.HazardSignedBy, "HazardSignedBy must be unchanged if IsHazardSectionSigned is false.");
        }

        [TestMethod]
        public void TestMapToEntitySetsHazardSignatureFieldsWhenIsHazardSectionSignedIsTrueAndThefieldsHaveNotBeenSetYet()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            _user.Employee = goodEmployee;
            _entity.HazardSignedAt = null;
            _entity.HazardSignedBy = null;

            _viewModel.IsHazardSectionSigned = true;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.HazardSignedAt, "HazardSignedAt must be set to current datetime when signing.");
            Assert.AreSame(goodEmployee, _entity.HazardSignedBy, "HazardSignedBy must be set to the current employee when signing.");
        }

        [TestMethod]
        public void TestMapToEntityDoesNotOverwriteHazardSignatureFieldsIfIsHazardSectionSignedIsTrueAndTheFieldsWereAlreadySet()
        {
            var expectedDate = DateTime.Now;
            var unexpectedDate = DateTime.Now.AddDays(1);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(unexpectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            var someOtherEmployee = GetEntityFactory<Employee>().Create();
            _entity.HazardSignedAt = expectedDate;
            _entity.HazardSignedBy = goodEmployee;
            _user.Employee = someOtherEmployee;
            _viewModel.IsHazardSectionSigned = true;

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedDate, _entity.HazardSignedAt, "HazardSignedAt must not change if it has already been set.");
            Assert.AreSame(goodEmployee, _entity.HazardSignedBy, "HazardSignedBy must not change if it has already been set.");
        }

        #endregion

        #region MapToEntity and permit cancellation logic

        [TestMethod]
        public void TestMapToEntityDoesNotSetPermitCancelledSignatureFieldsWhenIsPermitCancelledSectionSignedIsNullOrFalse()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            _user.Employee = goodEmployee;
            _entity.PermitCancelledAt = null;
            _entity.PermitCancelledBy = null;

            _viewModel.IsPermitCancelledSectionSigned = null;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.PermitCancelledAt, "PermitCancelledAt must be unchanged if PermitCancelledAt is null.");
            Assert.IsNull(_entity.PermitCancelledBy, "PermitCancelledBy must be unchanged if IsPermitCancelledSectionSigned is null.");

            _viewModel.IsPermitCancelledSectionSigned = false;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.PermitCancelledAt, "PermitCancelledAt must be unchanged if PermitCancelledAt is false.");
            Assert.IsNull(_entity.PermitCancelledBy, "PermitCancelledBy must be unchanged if IsPermitCancelledSectionSigned is false.");
        }

        [TestMethod]
        public void TestMapToEntitySetsPermitCancelledSignatureFieldsWhenIsPermitCancelledSectionSignedIsTrueAndThefieldsHaveNotBeenSetYet()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            _user.Employee = goodEmployee;
            _entity.PermitCancelledAt = null;
            _entity.PermitCancelledBy = null;

            _viewModel.IsPermitCancelledSectionSigned = true;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.PermitCancelledAt, "PermitCancelledAt must be set to current datetime when signing.");
            Assert.AreSame(goodEmployee, _entity.PermitCancelledBy, "PermitCancelledBy must be set to the current employee when signing.");
        }

        [TestMethod]
        public void TestMapToEntityDoesNotOverwritePermitCancelledSignatureFieldsIfIsPermitCancelledSectionSignedIsTrueAndTheFieldsWereAlreadySet()
        {
            var expectedDate = DateTime.Now;
            var unexpectedDate = DateTime.Now.AddDays(1);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(unexpectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            var someOtherEmployee = GetEntityFactory<Employee>().Create();
            _entity.PermitCancelledAt = expectedDate;
            _entity.PermitCancelledBy = goodEmployee;
            _user.Employee = someOtherEmployee;
            _viewModel.IsPermitCancelledSectionSigned = true;

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedDate, _entity.PermitCancelledAt, "PermitCancelledAt must not change if it has already been set.");
            Assert.AreSame(goodEmployee, _entity.PermitCancelledBy, "PermitCancelledBy must not change if it has already been set.");
        }

        #endregion

        #region MapToEntity and reclassification signing logic

        [TestMethod]
        public void TestMapToEntityDoesNotSetReclassificationSignatureFieldsWhenIsHazardSectionSignedIsNullOrFalse()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            _user.Employee = goodEmployee;
            _entity.ReclassificationSignedAt = null;
            _entity.ReclassificationSignedBy = null;

            _viewModel.IsReclassificationSectionSigned = null;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.ReclassificationSignedAt, "ReclassificationSignedAt must be unchanged if IsReclassificationSectionSigned is null.");
            Assert.IsNull(_entity.ReclassificationSignedBy, "ReclassificationSignedBy must be unchanged if IsReclassificationSectionSigned is null.");

            _viewModel.IsReclassificationSectionSigned = false;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.ReclassificationSignedAt, "ReclassificationSignedAt must be unchanged if IsReclassificationSectionSigned is false.");
            Assert.IsNull(_entity.ReclassificationSignedBy, "ReclassificationSignedBy must be unchanged if IsReclassificationSectionSigned is false.");
        }

        [TestMethod]
        public void TestMapToEntitySetsReclassificationSignatureFieldsWhenIsHazardSectionSignedIsTrueAndThefieldsHaveNotBeenSetYet()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            _user.Employee = goodEmployee;
            _entity.ReclassificationSignedAt = null;
            _entity.ReclassificationSignedBy = null;

            _viewModel.IsReclassificationSectionSigned = true;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.ReclassificationSignedAt, "ReclassificationSignedAt must be set to current datetime when signing.");
            Assert.AreSame(goodEmployee, _entity.ReclassificationSignedBy, "ReclassificationSignedBy must be set to the current employee when signing.");
        }

        [TestMethod]
        public void TestMapToEntityDoesNotOverwriteReclassificationSignatureFieldsIfIsHazardSectionSignedIsTrueAndTheFieldsWereAlreadySet()
        {
            var expectedDate = DateTime.Now;
            var unexpectedDate = DateTime.Now.AddDays(1);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(unexpectedDate);
            var goodEmployee = GetEntityFactory<Employee>().Create();
            var someOtherEmployee = GetEntityFactory<Employee>().Create();
            _entity.ReclassificationSignedAt = expectedDate;
            _entity.ReclassificationSignedBy = goodEmployee;
            _user.Employee = someOtherEmployee;
            _viewModel.IsHazardSectionSigned = true;

            _vmTester.MapToEntity();

            _user.Employee = GetEntityFactory<Employee>().Create();
            _viewModel.IsReclassificationSectionSigned = true;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.ReclassificationSignedAt, "ReclassificationSignedAt must not change if it has already been set.");
            Assert.AreSame(goodEmployee, _entity.ReclassificationSignedBy, "ReclassificationSignedBy must not change if it has already been set.");
        }

        #endregion

        [TestMethod]
        public void TestMapSetsIsFireWatchRequiredToTrueIfIsHotWorkPermitRequiredIsTrue()
        {
            _entity.IsFireWatchRequired = null;
            _entity.IsHotWorkPermitRequired = null;

            _viewModel.IsHotWorkPermitRequired = true;
            _viewModel.IsFireWatchRequired = false; // Doesn't matter what this is, it should get overwritten.
            _vmTester.MapToEntity();
            Assert.IsTrue(_entity.IsFireWatchRequired.Value);
        }

        #region Atmospheric Tests

        [TestMethod]
        public void TestMapToEntityAddsNewAtmosphericTests()
        {
            // This view model has its own tests dealing with mapping and validation
            // so we're not checking any of that here.
            var newTest = new CreateConfinedSpaceFormAtmosphericTest(_container);
            newTest.TestedAt = DateTime.Now;
            newTest.OxygenPercentageTop = 4.24m;
            _viewModel.NewAtmosphericTests = new List<CreateConfinedSpaceFormAtmosphericTest> { newTest };

            _vmTester.MapToEntity();

            var result = _entity.AtmosphericTests.Single();
            Assert.AreEqual(4.24m, result.OxygenPercentageTop);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotAddNewAtmosphericTestsIfBumpTestWasNotPerformed()
        {
            _viewModel.IsBumpTestConfirmed = null; // Setting this here because _viewModel had bump test set to true for some reason now
            var someExistingTest = new ConfinedSpaceFormAtmosphericTest();
            _entity.AtmosphericTests.Add(someExistingTest);
            var newTest = new CreateConfinedSpaceFormAtmosphericTest(_container);
            newTest.TestedAt = DateTime.Now;
            newTest.OxygenPercentageTop = 4.24m;
            _viewModel.NewAtmosphericTests = new List<CreateConfinedSpaceFormAtmosphericTest> { newTest };
            _entity.BumpTestConfirmedBy = null;
            Assert.IsFalse(_entity.IsBumpTestConfirmed, "No tests should be added if the entity's bump test fields have not been set to confirm");

            _vmTester.MapToEntity();

            var result = _entity.AtmosphericTests.Single();
            Assert.AreSame(result, someExistingTest);
        }

        #endregion

        #region Entrants

        [TestMethod]
        public void TestMapToEntityClearsExistingEntriesIfSection5IsNotEnabled()
        {
            _entity.Entrants.Add(new ConfinedSpaceFormEntrant());
            _entity.CanBeControlledByVentilationAlone = true;
            Assert.IsFalse(_entity.IsSection5Enabled);

            _vmTester.MapToEntity();
            Assert.IsFalse(_entity.Entrants.Any());
        }

        [TestMethod]
        public void TestMapToEntityAddsNewEntries()
        {
            _viewModel.CanBeControlledByVentilationAlone = false; // need to enable section 5 for the test.
            _viewModel.NewEntrants = new List<CreateConfinedSpaceFormEntrant> { _viewModelFactory.Build<CreateConfinedSpaceFormEntrant>() };
            _vmTester.MapToEntity();
            Assert.AreEqual(1, _entity.Entrants.Count);
        }

        [TestMethod]
        public void TestMapToEntityRemovesAnyEntriesThatAreInRemovedEntrantsList()
        {
            _viewModel.CanBeControlledByVentilationAlone = false; // need to enable section 5 for the test.
            var entrantToRemove = GetEntityFactory<ConfinedSpaceFormEntrant>().Create(new { ConfinedSpaceForm = _entity });
            var entrantToKeep = GetEntityFactory<ConfinedSpaceFormEntrant>().Create(new { ConfinedSpaceForm = _entity });
            Session.Refresh(_entity);
            Assert.IsTrue(_entity.Entrants.Contains(entrantToRemove), "Sanity");
            Assert.IsTrue(_entity.Entrants.Contains(entrantToKeep), "Sanity");

            _viewModel.RemovedEntrants = new List<int> { entrantToRemove.Id };
            _vmTester.MapToEntity();

            Func<ConfinedSpaceFormEntrant, bool> CompareEntrants(ConfinedSpaceFormEntrant expected) =>
                actual =>
                    expected.ContractingCompany == actual.ContractingCompany &&
                    expected.ContractorName == actual.ContractorName &&
                    expected.EntrantType?.Id == actual.EntrantType?.Id &&
                    expected.Employee?.Id == actual.Employee?.Id;

            Assert.IsFalse(_entity.Entrants.Any(CompareEntrants(entrantToRemove)));
            Assert.IsTrue(_entity.Entrants.Any(CompareEntrants(entrantToKeep)));
        }

        #region Prerequisite Satisfied

        [TestMethod]
        public void TestMapToEntityUpdatesSatisifedRequirementOnConfinedSpaceFormProductionOrderPreRequisiteWhenConfinedSpaceFormIsComplete()
        {
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create();
            var productionPreReq = GetFactory<IsConfinedSpaceProductionPrerequisiteFactory>().Create();
            var productionWorkOrderPreReq =
                GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new {
                    ProductionWorkOrder = productionWorkOrder,
                    ProductionPrerequisite = productionPreReq
                });

            productionWorkOrder.ProductionWorkOrderProductionPrerequisites.Add(productionWorkOrderPreReq);

            _viewModel.ProductionWorkOrder = productionWorkOrder.Id;
            //Section 2 compleiton
            _viewModel.NewAtmosphericTests = new List<CreateConfinedSpaceFormAtmosphericTest>();
            _viewModel.NewAtmosphericTests.Add(GetNewAtomosphericTest());
            //Section 3 completion
            _viewModel.IsReclassificationSectionSigned = true;
            //Section 4 completion
            _viewModel.CanBeControlledByVentilationAlone = false;
            _viewModel.IsHazardSectionSigned = true;
            //Section 5 completion
            _viewModel.PermitBeginsAt = _dateTimeProvider.Object.GetCurrentDate();
            _viewModel.PermitEndsAt = _dateTimeProvider.Object.GetCurrentDate();
            _viewModel.HasRetrievalSystem = true;
            _viewModel.HasContractRescueService = true;
            _viewModel.EmergencyResponseAgency = "Emergency Response Agency";
            _viewModel.EmergencyResponseContact = "Jeff Winger";

            _vmTester.MapToEntity();

            Assert.IsNotNull(productionWorkOrderPreReq.SatisfiedOn);
            Assert.AreEqual(productionWorkOrderPreReq.SatisfiedOn, _dateTimeProvider.Object.GetCurrentDate());
        }

        [TestMethod]
        public void TestMapToEntityDoesNotUpdateSatisifedRequirementOnConfinedSpaceFormProductionOrderPreRequisiteWhenConfinedSpaceFormIsNotComplete()
        {
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create();
            var productionPreReq = GetFactory<IsConfinedSpaceProductionPrerequisiteFactory>().Create();
            var productionWorkOrderPreReq =
                GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new {
                    ProductionWorkOrder = productionWorkOrder,
                    ProductionPrerequisite = productionPreReq
                });

            productionWorkOrder.ProductionWorkOrderProductionPrerequisites.Add(productionWorkOrderPreReq);

            _viewModel.ProductionWorkOrder = productionWorkOrder.Id;
            //Section 2 compleiton
            _viewModel.NewAtmosphericTests = new List<CreateConfinedSpaceFormAtmosphericTest>();
            _viewModel.NewAtmosphericTests.Add(GetNewAtomosphericTest());

            _vmTester.MapToEntity();

            Assert.IsNull(productionWorkOrderPreReq.SatisfiedOn);
        }

        #endregion

        #endregion

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.GeneralDateTime);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LocationAndDescriptionOfConfinedSpace);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PurposeOfEntry);
        }

        [TestMethod]
        public void TestRequiredWhenValidation()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.HasOtherSafetyEquipmentNotes, "Neato", x => x.HasOtherSafetyEquipment, true);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.MethodOfCommunicationOtherNotes, "Neato", x => x.MethodOfCommunication, ConfinedSpaceFormMethodOfCommunication.Indices.OTHER);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PermitBeginsAt, DateTime.Now, x => x.CanBeControlledByVentilationAlone, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PermitEndsAt, DateTime.Now, x => x.CanBeControlledByVentilationAlone, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.HasRetrievalSystem, true, x => x.CanBeControlledByVentilationAlone, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.HasContractRescueService, true, x => x.CanBeControlledByVentilationAlone, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.EmergencyResponseAgency, "strings", x => x.CanBeControlledByVentilationAlone, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.EmergencyResponseContact, "strings", x => x.CanBeControlledByVentilationAlone, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PermitCancellationNote, "This is a note", x => x.IsPermitCancelledSectionSigned, true);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IsHazardSectionSigned, true, x => x.CanBeControlledByVentilationAlone, true);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            var tooShort = "four";

            _viewModel.EmergencyResponseAgency = tooShort;
            _viewModel.EmergencyResponseContact = tooShort;
            _viewModel.HasOtherSafetyEquipmentNotes = tooShort;
            _viewModel.LocationAndDescriptionOfConfinedSpace = tooShort;
            _viewModel.PurposeOfEntry = tooShort;

            ValidationAssert.ModelStateHasError(_viewModel, "EmergencyResponseAgency", "The field EmergencyResponseAgency must be a string with a minimum length of 5 and a maximum length of 255.");
            ValidationAssert.ModelStateHasError(_viewModel, "EmergencyResponseContact", "The field EmergencyResponseContact must be a string with a minimum length of 5 and a maximum length of 50.");
            ValidationAssert.ModelStateHasError(_viewModel, "HasOtherSafetyEquipmentNotes", "The field HasOtherSafetyEquipmentNotes must be a string with a minimum length of 5 and a maximum length of 255.");
            ValidationAssert.ModelStateHasError(_viewModel, "LocationAndDescriptionOfConfinedSpace", "The field LocationAndDescriptionOfConfinedSpace must be a string with a minimum length of 5 and a maximum length of 255.");
            ValidationAssert.ModelStateHasError(_viewModel, "PurposeOfEntry", "The field PurposeOfEntry must be a string with a minimum length of 5 and a maximum length of 255.");
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PermitCancellationNote, ConfinedSpaceForm.StringLengths.PERMIT_CANCELLATION_NOTE);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.MethodOfCommunication, GetEntityFactory<ConfinedSpaceFormMethodOfCommunication>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.ProductionWorkOrder, GetEntityFactory<ProductionWorkOrder>().Create());
        }

        [TestMethod]
        public void TestValidationFailsIfThePermitHasAlreadyBeenCancelled()
        {
            // TODO: I don't think this init is needed?
            InitializeForSignatureValidationTest();

            _user.Employee = GetEntityFactory<Employee>().Create();
            _entity.PermitCancelledBy = GetEntityFactory<Employee>().Create();

            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "This permit has been cancelled and can no longer be edited.");
        }

        [TestMethod]
        public void TestEntrantCannotAlsoBeAttendant()
        {
            _viewModel.CanBeControlledByVentilationAlone = false; // need to enable section 5 for the test.
            var employee = GetEntityFactory<Employee>().Create();

            _viewModel.NewEntrants = new List<CreateConfinedSpaceFormEntrant> {
                _viewModelFactory.BuildWithOverrides<CreateConfinedSpaceFormEntrant>(new {
                    Employee = employee.Id,
                    EntrantType = GetFactory<EntrantConfinedSpaceFormEntrantTypeFactory>().Create().Id
                }),
                _viewModelFactory.BuildWithOverrides<CreateConfinedSpaceFormEntrant>(new {
                    Employee = employee.Id,
                    EntrantType = GetFactory<AttendantConfinedSpaceFormEntrantTypeFactory>().Create().Id
                }),
            };

            ValidationAssert.ModelStateHasError(_viewModel, "Entrants", "Invalid combination of employees and entrant types detected.");
        }

        [TestMethod]
        public void TestEntrantCannotAlsoBeSupervisor()
        {
            _viewModel.CanBeControlledByVentilationAlone = false; // need to enable section 5 for the test.
            var employee = GetEntityFactory<Employee>().Create();

            _viewModel.NewEntrants = new List<CreateConfinedSpaceFormEntrant> {
                _viewModelFactory.BuildWithOverrides<CreateConfinedSpaceFormEntrant>(new {
                    Employee = employee.Id,
                    EntrantType = GetFactory<EntrantConfinedSpaceFormEntrantTypeFactory>().Create().Id
                }),
                _viewModelFactory.BuildWithOverrides<CreateConfinedSpaceFormEntrant>(new {
                    Employee = employee.Id,
                    EntrantType = GetFactory<EntrySupervisorConfinedSpaceFormEntrantTypeFactory>().Create().Id
                }),
            };

            ValidationAssert.ModelStateHasError(_viewModel, "Entrants", "Invalid combination of employees and entrant types detected.");
        }

        [TestMethod]
        public void TestEntrantCannotBeEntrantTwice()
        {
            _viewModel.CanBeControlledByVentilationAlone = false; // need to enable section 5 for the test.
            var employee = GetEntityFactory<Employee>().Create();

            _viewModel.NewEntrants = new List<CreateConfinedSpaceFormEntrant> {
                _viewModelFactory.BuildWithOverrides<CreateConfinedSpaceFormEntrant>(new {
                    Employee = employee.Id,
                    EntrantType = GetFactory<EntrantConfinedSpaceFormEntrantTypeFactory>().Create().Id
                }),
                _viewModelFactory.BuildWithOverrides<CreateConfinedSpaceFormEntrant>(new {
                    Employee = employee.Id,
                    EntrantType = GetFactory<EntrantConfinedSpaceFormEntrantTypeFactory>().Create().Id
                }),
            };

            ValidationAssert.ModelStateHasError(_viewModel, "Entrants", "Invalid combination of employees and entrant types detected.");
        }

        [TestMethod]
        public void TestEntrantCannotBeAttendantTwice()
        {
            _viewModel.CanBeControlledByVentilationAlone = false; // need to enable section 5 for the test.
            var employee = GetEntityFactory<Employee>().Create();

            _viewModel.NewEntrants = new List<CreateConfinedSpaceFormEntrant> {
                _viewModelFactory.BuildWithOverrides<CreateConfinedSpaceFormEntrant>(new {
                    Employee = employee.Id,
                    EntrantType = GetFactory<AttendantConfinedSpaceFormEntrantTypeFactory>().Create().Id
                }),
                _viewModelFactory.BuildWithOverrides<CreateConfinedSpaceFormEntrant>(new {
                    Employee = employee.Id,
                    EntrantType = GetFactory<AttendantConfinedSpaceFormEntrantTypeFactory>().Create().Id
                }),
            };

            ValidationAssert.ModelStateHasError(_viewModel, "Entrants", "Invalid combination of employees and entrant types detected.");
        }

        [TestMethod]
        public void TestEntrantCannotBeSupervisorTwice()
        {
            _viewModel.CanBeControlledByVentilationAlone = false; // need to enable section 5 for the test.
            var employee = GetEntityFactory<Employee>().Create();

            _viewModel.NewEntrants = new List<CreateConfinedSpaceFormEntrant> {
                _viewModelFactory.BuildWithOverrides<CreateConfinedSpaceFormEntrant>(new {
                    Employee = employee.Id,
                    EntrantType = GetFactory<EntrySupervisorConfinedSpaceFormEntrantTypeFactory>().Create().Id
                }),
                _viewModelFactory.BuildWithOverrides<CreateConfinedSpaceFormEntrant>(new {
                    Employee = employee.Id,
                    EntrantType = GetFactory<EntrySupervisorConfinedSpaceFormEntrantTypeFactory>().Create().Id
                }),
            };

            ValidationAssert.ModelStateHasError(_viewModel, "Entrants", "Invalid combination of employees and entrant types detected.");
        }

        [TestMethod]
        public void TestValidationIncludesOriginal()
        {
            _viewModel.CanBeControlledByVentilationAlone = false; // need to enable section 5 for the test.
            var employee = GetEntityFactory<Employee>().Create();
            _viewModel.Original.Entrants = new List<ConfinedSpaceFormEntrant> {
                GetEntityFactory<ConfinedSpaceFormEntrant>().Build(new {
                    Employee = employee,
                    EntrantType = GetFactory<EntrySupervisorConfinedSpaceFormEntrantTypeFactory>().Create()
                })
            };

            _viewModel.NewEntrants = new List<CreateConfinedSpaceFormEntrant> {
                _viewModelFactory.BuildWithOverrides<CreateConfinedSpaceFormEntrant>(new {
                    Employee = employee.Id,
                    EntrantType = GetFactory<EntrySupervisorConfinedSpaceFormEntrantTypeFactory>().Create().Id
                }),
            };

            ValidationAssert.ModelStateHasError(_viewModel, "Entrants", "Invalid combination of employees and entrant types detected.");
        }

        #region Validating signatures

        private void InitializeForSignatureValidationTest()
        {
            // All sections need to have signing disabled before the tests can run due to the order
            // the validation is ran. Signing should only be enabled for the one section being tested.
            _viewModel.IsBeginEntrySectionSigned = false;
            _viewModel.IsBumpTestConfirmed = false;
            _viewModel.IsHazardSectionSigned = false;
            _viewModel.IsPermitCancelledSectionSigned = false;
            _viewModel.IsReclassificationSectionSigned = false;
        }

        [TestMethod]
        public void TestValidationFailsIfUserSignsBeginEntryAuthorizationSectionButUserDoesNotHaveAssociatedEmployee()
        {
            InitializeForSignatureValidationTest();
            _user.Employee = null;
            _viewModel.IsBeginEntrySectionSigned = true;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.IsBeginEntrySectionSigned, ConfinedSpaceFormViewModel.USER_MUST_HAVE_EMPLOYEE_RECORD);

            _user.Employee = GetEntityFactory<Employee>().Create();

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.IsBeginEntrySectionSigned);
        }

        [TestMethod]
        public void TestValidationFailsIfUserConfirmsBumpTestAuthorizationSectionButUserDoesNotHaveAssociatedEmployee()
        {
            InitializeForSignatureValidationTest();
            _user.Employee = null;
            _entity.BumpTestConfirmedBy = null; // Need to null this out for this test because the factory sets it by default
            _viewModel.IsBumpTestConfirmed = true;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.IsBumpTestConfirmed, ConfinedSpaceFormViewModel.USER_MUST_HAVE_EMPLOYEE_RECORD);

            _user.Employee = GetEntityFactory<Employee>().Create();

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.IsBumpTestConfirmed);
        }

        [TestMethod]
        public void TestValidationFailsIfUserSignsHazardSectionButUserDoesNotHaveAssociatedEmployee()
        {
            InitializeForSignatureValidationTest();
            _user.Employee = null;
            _viewModel.IsHazardSectionSigned = true;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.IsHazardSectionSigned, ConfinedSpaceFormViewModel.USER_MUST_HAVE_EMPLOYEE_RECORD);

            _user.Employee = GetEntityFactory<Employee>().Create();

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.IsHazardSectionSigned);
        }

        [TestMethod]
        public void TestValidationFailsIfUserSignsPermitCancellationSectionButUserDoesNotHaveAssociatedEmployee()
        {
            InitializeForSignatureValidationTest();
            _user.Employee = null;
            _viewModel.IsPermitCancelledSectionSigned = true;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.IsPermitCancelledSectionSigned, ConfinedSpaceFormViewModel.USER_MUST_HAVE_EMPLOYEE_RECORD);

            _user.Employee = GetEntityFactory<Employee>().Create();

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.IsPermitCancelledSectionSigned);
        }

        [TestMethod]
        public void TestValidationFailsIfUserSignsReclassificationSectionButUserDoesNotHaveAssociatedEmployee()
        {
            InitializeForSignatureValidationTest();
            _user.Employee = null;
            _viewModel.IsReclassificationSectionSigned = true;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.IsReclassificationSectionSigned, ConfinedSpaceFormViewModel.USER_MUST_HAVE_EMPLOYEE_RECORD);

            _user.Employee = GetEntityFactory<Employee>().Create();

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.IsReclassificationSectionSigned);
        }

        #endregion

        #endregion

        #region Form Completion

        private CreateConfinedSpaceFormAtmosphericTest GetNewAtomosphericTest()
        {
            var newAtomoTest = _viewModelFactory.Build<CreateConfinedSpaceFormAtmosphericTest>();
            newAtomoTest.OxygenPercentageTop = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MAX;
            newAtomoTest.OxygenPercentageBottom = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MAX;
            newAtomoTest.OxygenPercentageMiddle = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MAX;
            newAtomoTest.CarbonMonoxidePartsPerMillionBottom = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.CO_MAX;
            newAtomoTest.CarbonMonoxidePartsPerMillionMiddle = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.CO_MAX;
            newAtomoTest.CarbonMonoxidePartsPerMillionTop = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.CO_MAX;
            newAtomoTest.HydrogenSulfidePartsPerMillionBottom = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.H2S_MAX;
            newAtomoTest.HydrogenSulfidePartsPerMillionMiddle = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.H2S_MAX;
            newAtomoTest.HydrogenSulfidePartsPerMillionTop = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.H2S_MAX;
            newAtomoTest.LowerExplosiveLimitPercentageBottom = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.LEL_MAX;
            newAtomoTest.LowerExplosiveLimitPercentageMiddle = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.LEL_MAX;
            newAtomoTest.LowerExplosiveLimitPercentageTop = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.LEL_MAX;

            return newAtomoTest;
        }

        // Section 1 is always true, most of is pre filled / required on the VM
        [TestMethod]
        public void TestIsFormCompleteReturnsTrueWhenSectionOneSectionTwoAndSectionThreeAreComplete()
        {
            //Section 2 compleiton
            _viewModel.NewAtmosphericTests = new List<CreateConfinedSpaceFormAtmosphericTest>();
            _viewModel.NewAtmosphericTests.Add(GetNewAtomosphericTest());
            //Section 3 completion
            _viewModel.IsReclassificationSectionSigned = true;

            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.IsCompleted);
            Assert.IsTrue(_entity.IsSection3Completed);
            Assert.IsTrue(_entity.IsSection1Completed);
            Assert.IsTrue(_entity.IsSection2Completed);
            Assert.IsFalse(_entity.IsSection4Completed);
            Assert.IsFalse(_entity.IsSection5Completed);
        }

        [TestMethod]
        public void TestIsFormCompleteReturnsTrueWhenSectionOneSectionTwoAndSectionFourAreComplete()
        {
            //Section 2 compleiton
            _viewModel.NewAtmosphericTests = new List<CreateConfinedSpaceFormAtmosphericTest>();
            _viewModel.NewAtmosphericTests.Add(GetNewAtomosphericTest());
            //Section 4 completion
            _viewModel.CanBeControlledByVentilationAlone = true;
            _viewModel.IsHazardSectionSigned = true;

            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.IsCompleted);
            Assert.IsFalse(_entity.IsSection3Completed);
            Assert.IsTrue(_entity.IsSection1Completed);
            Assert.IsTrue(_entity.IsSection2Completed);
            Assert.IsTrue(_entity.IsSection4Completed);
            Assert.IsFalse(_entity.IsSection5Completed);
        }

        [TestMethod]
        public void TestIsForCompleteReturnsTrueWhenSectionOneSectionTwoSectionThreeSectionFourSectionFiveAreComplete()
        {
            //Section 2 compleiton
            _viewModel.NewAtmosphericTests = new List<CreateConfinedSpaceFormAtmosphericTest>();
            _viewModel.NewAtmosphericTests.Add(GetNewAtomosphericTest());
            //Section 3 completion
            _viewModel.IsReclassificationSectionSigned = true;
            //Section 4 completion
            _viewModel.CanBeControlledByVentilationAlone = false;
            _viewModel.IsHazardSectionSigned = true;
            //Section 5 completion
            _viewModel.PermitBeginsAt = _dateTimeProvider.Object.GetCurrentDate();
            _viewModel.PermitEndsAt = _dateTimeProvider.Object.GetCurrentDate();
            _viewModel.HasRetrievalSystem = true;
            _viewModel.HasContractRescueService = true;
            _viewModel.EmergencyResponseAgency = "Emergency Response Agency";
            _viewModel.EmergencyResponseContact = "Jeff Winger";

            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.IsCompleted);
            Assert.IsTrue(_entity.IsSection3Completed);
            Assert.IsTrue(_entity.IsSection1Completed);
            Assert.IsTrue(_entity.IsSection2Completed);
            Assert.IsTrue(_entity.IsSection4Completed);
            Assert.IsTrue(_entity.IsSection5Completed);
        }

        [TestMethod]
        public void TestIsFormCompleteReturnsFalseWhenSectionOneSectionTwoAreComplete()
        {
            //Section 2 compleiton
            _viewModel.NewAtmosphericTests = new List<CreateConfinedSpaceFormAtmosphericTest>();
            _viewModel.NewAtmosphericTests.Add(GetNewAtomosphericTest());

            _vmTester.MapToEntity();

            Assert.IsFalse(_entity.IsCompleted);
            Assert.IsFalse(_entity.IsSection3Completed);
            Assert.IsTrue(_entity.IsSection1Completed);
            Assert.IsTrue(_entity.IsSection2Completed);
            Assert.IsFalse(_entity.IsSection4Completed);
            Assert.IsFalse(_entity.IsSection5Completed);
        }

        [TestMethod]
        public void TestIsFormCompleteReturnsFalseWhenSectionOneSectionThreeSectionFourSectionFiveAreComplete()
        {
            //Section 3 completion
            _viewModel.IsReclassificationSectionSigned = true;
            //Section 4 completion
            _viewModel.CanBeControlledByVentilationAlone = false;
            _viewModel.IsHazardSectionSigned = true;
            //Section 5 completion
            _viewModel.PermitBeginsAt = _dateTimeProvider.Object.GetCurrentDate();
            _viewModel.PermitEndsAt = _dateTimeProvider.Object.GetCurrentDate();
            _viewModel.HasRetrievalSystem = true;
            _viewModel.HasContractRescueService = true;
            _viewModel.EmergencyResponseAgency = "Emergency Response Agency";
            _viewModel.EmergencyResponseContact = "Jeff Winger";

            _vmTester.MapToEntity();

            Assert.IsFalse(_entity.IsCompleted);
            Assert.IsTrue(_entity.IsSection3Completed);
            Assert.IsTrue(_entity.IsSection1Completed);
            Assert.IsFalse(_entity.IsSection2Completed);
            Assert.IsTrue(_entity.IsSection4Completed);
            Assert.IsTrue(_entity.IsSection5Completed);
        }

        [TestMethod]
        public void TestIsFormCompleteReturnsFalseWhenOnlySectionOneIsComplete()
        {
            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.IsSection1Completed);
            Assert.IsFalse(_entity.IsCompleted);
        }

        [TestMethod]
        public void TestIsFormCompleteReturnsFalseWhenOnlySectionTwoIsComplete()
        {
            //Section 2 compleiton
            _viewModel.NewAtmosphericTests = new List<CreateConfinedSpaceFormAtmosphericTest>();
            _viewModel.NewAtmosphericTests.Add(GetNewAtomosphericTest());
            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.IsSection2Completed);
            Assert.IsFalse(_entity.IsCompleted);
        }

        [TestMethod]
        public void TestIsFormCompleteReturnsFalseWhenOnlySectionThreeIsComplete()
        {
            //Section 3 completion
            _viewModel.IsReclassificationSectionSigned = true;
            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.IsSection3Completed);
            Assert.IsFalse(_entity.IsCompleted);
        }

        [TestMethod]
        public void TestIsFormCompleteReturnsFalseWhenOnlySectionFourIsComplete()
        {
            //Section 4 completion
            _viewModel.CanBeControlledByVentilationAlone = false;
            _viewModel.IsHazardSectionSigned = true;
            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.IsSection4Completed);
            Assert.IsFalse(_entity.IsCompleted);
        }

        [TestMethod]
        public void TestIsFormCompleteReturnsFalseWhenOnlySectionFiveIsComplete()
        {
            //Section 5 completion
            _viewModel.CanBeControlledByVentilationAlone = false;
            _viewModel.PermitBeginsAt = _dateTimeProvider.Object.GetCurrentDate();
            _viewModel.PermitEndsAt = _dateTimeProvider.Object.GetCurrentDate();
            _viewModel.HasRetrievalSystem = true;
            _viewModel.HasContractRescueService = true;
            _viewModel.EmergencyResponseAgency = "Emergency Response Agency";
            _viewModel.EmergencyResponseContact = "Jeff Winger";

            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.IsSection5Completed);
            Assert.IsFalse(_entity.IsCompleted);
        }

        #endregion

        #endregion
    }

    [TestClass]
    public class CreateConfinedSpaceFormTest : ConfinedSpaceFormViewModelTest<CreateConfinedSpaceForm>
    {
        #region Tests

        [TestMethod]
        public void TestSetDefaultsSetsOperatingCenterFromProductionWorkOrderIfProductionWorkOrderValueIsSet()
        {
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create();
            var expected = pwo.OperatingCenter.Id;
            _viewModel.ProductionWorkOrder = pwo.Id;
            _viewModel.OperatingCenter = null;

            _viewModel.SetDefaults();

            Assert.AreEqual(expected, _viewModel.OperatingCenter);
        }
     
        [TestMethod]
        public void TestSetDefaultsSetsOperatingCenterFromWorkOrderIfWorkOrderValueIsSet()
        {
            var wo = GetEntityFactory<WorkOrder>().Create();
            var expected = wo.OperatingCenter.Id;
            _viewModel.ProductionWorkOrder = null; // PWO has priority and is set by default for the tests, so null it out.
            _viewModel.ProductionWorkOrderDisplay = null;
            _viewModel.WorkOrder = wo.Id;
            _viewModel.OperatingCenter = null;

            _viewModel.SetDefaults();

            Assert.AreEqual(expected, _viewModel.OperatingCenter);
        }

        [TestMethod]
        public void TestSetDefaultsSetsGeneralDateTimeToCurrentDateTime()
        {
            var expected = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expected);
            _viewModel.GeneralDateTime = null;

            _viewModel.SetDefaults();

            Assert.AreEqual(expected, _viewModel.GeneralDateTime);
        }


        #endregion
    }

    [TestClass]
    public class EditConfinedSpaceFormTest : ConfinedSpaceFormViewModelTest<EditConfinedSpaceForm> { }

    [TestClass]
    public class PostCompletionConfinedSpaceFormTest : MapCallMvcInMemoryDatabaseTestBase<ConfinedSpaceForm>
    {
        #region Fields

        private ViewModelTester<PostCompletionConfinedSpaceForm, ConfinedSpaceForm> _vmTester;
        private PostCompletionConfinedSpaceForm _viewModel;
        private ConfinedSpaceForm _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
        }

        private ConfinedSpaceFormAtmosphericTest GetNewAtomosphericTest()
        {
            return new ConfinedSpaceFormAtmosphericTest {
                OxygenPercentageTop = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MAX,
                OxygenPercentageBottom = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MAX,
                OxygenPercentageMiddle = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MAX,
                CarbonMonoxidePartsPerMillionBottom = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.CO_MAX,
                CarbonMonoxidePartsPerMillionMiddle = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.CO_MAX,
                CarbonMonoxidePartsPerMillionTop = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.CO_MAX,
                HydrogenSulfidePartsPerMillionBottom = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.H2S_MAX,
                HydrogenSulfidePartsPerMillionMiddle = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.H2S_MAX,
                HydrogenSulfidePartsPerMillionTop = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.H2S_MAX,
                LowerExplosiveLimitPercentageBottom = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.LEL_MAX,
                LowerExplosiveLimitPercentageMiddle = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.LEL_MAX,
                LowerExplosiveLimitPercentageTop = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.LEL_MAX
            };
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _user = GetFactory<UserFactory>().Create();
            _user.Employee = GetEntityFactory<Employee>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetFactory<ConfinedSpaceFormFactory>().Create();
            _viewModel = _viewModelFactory.Build<PostCompletionConfinedSpaceForm, ConfinedSpaceForm>( _entity);
            _vmTester = new ViewModelTester<PostCompletionConfinedSpaceForm, ConfinedSpaceForm>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.PermitCancellationNote);
        }

        [TestMethod]
        public void MapToEntityMapsEntrants()
        {
            _entity.CanBeControlledByVentilationAlone = false; // need to enable section 5 for the test.
            _viewModel.NewEntrants = new List<CreateConfinedSpaceFormEntrant> { _viewModelFactory.Build<CreateConfinedSpaceFormEntrant>() };
            _vmTester.MapToEntity();
            Assert.AreEqual(1, _entity.Entrants.Count);
        }

        [TestMethod]
        public void MapToEntityMapsCancellationSignature()
        {
            _viewModel.IsPermitCancelledSectionSigned = true;
            _viewModel.PermitCancellationNote = "lol its a note";

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(_viewModel.PermitCancellationNote, _entity.PermitCancellationNote);
            Assert.AreEqual(_viewModel.IsPermitCancelledSectionSigned, _entity.IsPermitCancelledSectionSigned);
        }

        [TestMethod]
        public void TestMapSetsIsCompletedTrueWhenTrue()
        {
            _entity.AtmosphericTests.Add(GetNewAtomosphericTest());
            _entity.ReclassificationSignedBy = new Employee { Id = 108 };

            _viewModel.Map(_entity);

            Assert.IsTrue(_viewModel.IsCompleted);
        }

        #endregion
    }
}
