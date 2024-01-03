using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Configuration;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    /// <summary>
    /// This had to become the base for all the others because it had the set of all
    /// common fields/functionality for all the others.
    /// </summary>
    public abstract class PostCompletionConfinedSpaceFormBase : ViewModel<ConfinedSpaceForm>
    {
        #region Fields

        private ConfinedSpaceForm _original;
        private ProductionWorkOrder _productionWorkOrder;
        private WorkOrder _workOrder;

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public new ConfinedSpaceForm Original
        {
            // need to override this so we can grab it from the repository
            // if we don't have it
            get => _original ?? (_original = _container.GetInstance<IRepository<ConfinedSpaceForm>>().Find(Id));
            protected set => _original = value;
        }

        #region PermitCancellation

        [DoesNotAutoMap("Used for validation and needed in the view")]
        public bool IsPermitCancelledSectionPreviouslySigned => Original?.IsPermitCancelledSectionSigned == true;

        [DoesNotAutoMap, CheckBox]
        public bool? IsPermitCancelledSectionSigned { get; set; }

        [Multiline, StringLength(ConfinedSpaceForm.StringLengths.PERMIT_CANCELLATION_NOTE)]
        [RequiredWhen("IsPermitCancelledSectionSigned", ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        public string PermitCancellationNote { get; set; }

        #endregion

        #region Entrants

        [DoesNotAutoMap]
        public virtual bool HasAtLeastOneEntrant => !Original.IsSection5Enabled || NewEntrants?.Count > 0 ||
                                             Original.Entrants?.Count > 0;

        [DoesNotAutoMap("Done manually in MapToEntity"), RequiredWhen(nameof(HasAtLeastOneEntrant), ComparisonType.EqualTo, false, ErrorMessage = "Employee Assignments are required")]
        public List<CreateConfinedSpaceFormEntrant> NewEntrants { get; set; }
        [DoesNotAutoMap("Done manually in MapToEntity")]
        public List<int> RemovedEntrants { get; set; }


        #endregion

        #region Section 1


        [Secured] // This value comes from the New link on the ProductionWorkOrder/Show page.
        [EntityMap, EntityMustExist(typeof(ProductionWorkOrder))]
        public int? ProductionWorkOrder { get; set; }

        [View("Production Work Order")]
        [DoesNotAutoMap("Display only")]
        public ProductionWorkOrder ProductionWorkOrderDisplay
        {
            get
            {
                // This field is used to display several properties, so make sure to 
                // cache this lookup. 
                if (_productionWorkOrder == null && ProductionWorkOrder.HasValue)
                {
                    _productionWorkOrder = _container.GetInstance<IRepository<ProductionWorkOrder>>()
                                                     .Find(ProductionWorkOrder.GetValueOrDefault());
                }
                return _productionWorkOrder;
            }
            // This setter exists solely for a couple of unit tests because the unit tests
            internal set => _productionWorkOrder = value;
        }
      
        [Secured] // This value comes from the New link on the ProductionWorkOrder/Show page.
        [EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        [View("Work Order")]
        [DoesNotAutoMap("Display only")]
        public WorkOrder WorkOrderDisplay
        {
            get
            {
                // This field is used to display several properties, so make sure to 
                // cache this lookup. 
                if (_workOrder == null && WorkOrder.HasValue)
                {
                    _workOrder = _container.GetInstance<IRepository<WorkOrder>>()
                                .Find(WorkOrder.GetValueOrDefault());
                }
                return _workOrder;
            }
        }

        [View("Short Cycle Work Order")]
        public long? ShortCycleWorkOrderNumber { get; set; }

        [DropDown]
        [EntityMap(MapDirections.None), RequiredWhen(nameof(ShortCycleWorkOrderNumber), ComparisonType.NotEqualTo, null), EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State", PromptText = "Select a state above")]
        [EntityMap, RequiredWhen(nameof(ShortCycleWorkOrderNumber), ComparisonType.NotEqualTo, null), EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required, DateTimePicker, View(FormatStyle.DateTimeWithoutSeconds)]
        public DateTime? GeneralDateTime { get; set; }

        [Required, StringLength(ConfinedSpaceForm.StringLengths.LOCATION_AND_DESCRIPTION, MinimumLength = ConfinedSpaceForm.StringLengths.MIN_LENGTH)]
        public string LocationAndDescriptionOfConfinedSpace { get; set; }

        [Required, StringLength(ConfinedSpaceForm.StringLengths.PURPOSE_OF_ENTRY, MinimumLength = ConfinedSpaceForm.StringLengths.MIN_LENGTH)]
        public string PurposeOfEntry { get; set; }

        #endregion

        #region Section 2

        [DoesNotAutoMap("Display/validation only")]
        public bool IsSection2Completed
        {
            get
            {
                var isCompleted = Original?.IsSection2Completed;
                return isCompleted.GetValueOrDefault();
            }
        }

        [AutoMap(MapDirections.ToViewModel)]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// This must == true in order for tests to be added.
        /// This needs to be a checkbox, but it can't be a required field
        /// because otherwise it will require them to check it.
        /// </summary>
        [DoesNotAutoMap, CheckBox]
        public bool? IsBumpTestConfirmed { get; set; }

        [DoesNotAutoMap("Used for validation and needed in the view")]
        public bool IsBumpTestPreviouslyConfirmed => Original?.IsBumpTestConfirmed == true;

        [DoesNotAutoMap("These only ever come from the client-side and they need to be manually mapped.")]
        public List<CreateConfinedSpaceFormAtmosphericTest> NewAtmosphericTests { get; set; }

        #endregion

        #endregion

        #region Private Methods

        public override void Map(ConfinedSpaceForm entity)
        {
            base.Map(entity);
            Original = entity;
        }

        private void MapEntrantsToEntity(ConfinedSpaceForm entity)
        {
            // Entrants are only able to be added when section 5/entrants
            // section is enabled. If they add entrants and then go back
            // and revert something that disables this section then we want
            // to remove any existing entrants.
            if (!entity.IsSection5Enabled)
            {
                entity.Entrants.Clear();
            }
            else
            {
                foreach (var id in RemovedEntrants ?? Enumerable.Empty<int>())
                {
                    entity.Entrants.RemoveSingle(x => x.Id == id);
                }

                foreach (var entrant in NewEntrants ?? Enumerable.Empty<CreateConfinedSpaceFormEntrant>())
                {
                    var entrantEntity = new ConfinedSpaceFormEntrant();
                    entrant.MapToEntity(entrantEntity);
                    entrantEntity.ConfinedSpaceForm = entity;
                    entity.Entrants.Add(entrantEntity);
                }
            }
        }

        private void MapAtmosphericTestsToEntity(ConfinedSpaceForm entity)
        {
            // NewAtmosphericTests will be null whenever they're creating/editing a record
            // that doesn't include a new atmospheric test.
            // Also, IsSection2Completed may become true after this method completed. This will matter in MC-2665
            if (entity.IsBumpTestConfirmed && NewAtmosphericTests != null)
            {
                foreach (var test in NewAtmosphericTests)
                {
                    var testEntity = new ConfinedSpaceFormAtmosphericTest();
                    test.MapToEntity(testEntity);
                    testEntity.ConfinedSpaceForm = entity;
                    entity.AtmosphericTests.Add(testEntity);
                }
            }
        }

        public override ConfinedSpaceForm MapToEntity(ConfinedSpaceForm entity)
        {
            entity = base.MapToEntity(entity);
            MapAtmosphericTestsToEntity(entity);
            MapEntrantsToEntity(entity);
            return entity;
        }

        #endregion

        public PostCompletionConfinedSpaceFormBase(IContainer container) : base(container) { }
    }

    /// <remarks>
    ///
    /// NOTE: All of the fields with required strings have a min length of 5. This was requested
    /// by the business because they think it will prevent users from writing "N/A". Personally,
    /// I think they'll find some other new useless way to enter a value, but there's your
    /// explanation for why they're there. -Ross 8/25/2020
    ///
    /// If I were to do this all again, I would have made each section an explicit view model
    /// class with a property on this. It would make it a lot easier for dealing with erasing
    /// unintional information when sections gets saved then disabled and saved again. -Ross 9/16/2020
    /// 
    /// </remarks>
    public abstract class ConfinedSpaceFormViewModel : PostCompletionConfinedSpaceFormBase
    {
        #region Consts

        public const string USER_MUST_HAVE_EMPLOYEE_RECORD = "Your user account must have an associated employee record before you can sign this record.";

        #endregion

        #region Properties

        #region Section 1

        [Required, DateTimePicker, View(FormatStyle.DateTimeWithoutSeconds)]
        public DateTime? GeneralDateTime { get; set; }

        [Required, StringLength(ConfinedSpaceForm.StringLengths.LOCATION_AND_DESCRIPTION, MinimumLength = ConfinedSpaceForm.StringLengths.MIN_LENGTH)]
        public string LocationAndDescriptionOfConfinedSpace { get; set; }

        [Required, StringLength(ConfinedSpaceForm.StringLengths.PURPOSE_OF_ENTRY, MinimumLength = ConfinedSpaceForm.StringLengths.MIN_LENGTH)]
        public string PurposeOfEntry { get; set; }

        #endregion

        #region Section 2
        
        // TODO: Another ticket is coming in for this. This needs to filter by gas monitors that 
        // have a valid/passing calibration for a given date. I don't have a ticket number yet. -Ross 8/24/2020
        [DropDown, RequiredWhen(nameof(IsBumpTestConfirmed), true)]
        [EntityMap, EntityMustExist(typeof(GasMonitor))]
        public int? GasMonitor { get; set; }

        #endregion

        #region Section 3

        [DoesNotAutoMap("Used for validation and needed in the view")]
        public bool IsReclassificationSectionPreviouslySigned => Original?.IsReclassificationSectionSigned == true;

        [DoesNotAutoMap, CheckBox]
        public bool? IsReclassificationSectionSigned { get; set; }

        #endregion

        #region Section 4

        public bool? CanBeControlledByVentilationAlone { get; set; }

        [DoesNotAutoMap("Used for validation and needed in the view")]
        public bool IsHazardSectionPreviouslySigned => Original?.IsHazardSectionSigned == true;

        [DoesNotAutoMap, CheckBox, RequiredWhen(nameof(CanBeControlledByVentilationAlone), ComparisonType.NotEqualTo, null)]
        public bool? IsHazardSectionSigned { get; set; }

        #endregion

        #region Section 5

        [DoesNotAutoMap]
        public override bool HasAtLeastOneEntrant => CanBeControlledByVentilationAlone == null ||
                                                     CanBeControlledByVentilationAlone == true ||
                                                     NewEntrants?.Count > 0 || Original?.Entrants?.Count > 0;
        [RequiredWhen(nameof(CanBeControlledByVentilationAlone), false), DateTimePicker]
        public DateTime? PermitBeginsAt { get; set; }
        [RequiredWhen(nameof(CanBeControlledByVentilationAlone), false), DateTimePicker]
        public DateTime? PermitEndsAt { get; set; }

        #region Hazards Checklist

        [DoesNotAutoMap("Done manually")]
        public List<ConfinedSpaceFormHazardViewModel> Hazards { get; set; }

        #endregion

        #region Required Safety Equipment

        public bool? HasWarningSafetyEquipment { get; set; }
        public bool? HasAccessSafetyEquipment { get; set; }
        public bool? HasLightingSafetyEquipment { get; set; }
        public bool? HasVentilationSafetyEquipment { get; set; }
        public bool? HasGFCISafetyEquipment { get; set; }
        public bool? HasOtherSafetyEquipment { get; set; }
        public bool? HasHeadSafetyEquipment { get; set; }
        public bool? HasEyeSafetyEquipment { get; set; }
        public bool? HasRespiratorySafetyEquipment { get; set; }
        public bool? HasHandSafetyEquipment { get; set; }
        public bool? HasFallSafetyEquipment { get; set; }
        public bool? HasFootSafetyEquipment { get; set; }

        [StringLength(ConfinedSpaceForm.StringLengths.HAS_OTHER_SAFETY_EQUIPMENT_NOTES, MinimumLength = ConfinedSpaceForm.StringLengths.MIN_LENGTH)]
        [RequiredWhen(nameof(HasOtherSafetyEquipment), true)]
        public string HasOtherSafetyEquipmentNotes { get; set; }

        #endregion

        #region Method of communication

        [DropDown, EntityMap, EntityMustExist(typeof(ConfinedSpaceFormMethodOfCommunication))]
        public int? MethodOfCommunication { get; set; }

        [StringLength(ConfinedSpaceForm.StringLengths.METHOD_OF_COMMUNICATION_OTHER_NOTES, MinimumLength = ConfinedSpaceForm.StringLengths.MIN_LENGTH)]
        [RequiredWhen(nameof(MethodOfCommunication), ConfinedSpaceFormMethodOfCommunication.Indices.OTHER)]
        public virtual string MethodOfCommunicationOtherNotes { get; set; }

        #endregion

        #region Hot work permit

        public bool? IsHotWorkPermitRequired { get; set; }
        public bool? IsFireWatchRequired { get; set; }

        #endregion

        #region Method of rescue of entrants

        [RequiredWhen(nameof(CanBeControlledByVentilationAlone), false)]
        public bool? HasRetrievalSystem { get; set; }
        [RequiredWhen(nameof(CanBeControlledByVentilationAlone), false)]
        public bool? HasContractRescueService { get; set; }
        [RequiredWhen(nameof(CanBeControlledByVentilationAlone), false)]
        [StringLength(ConfinedSpaceForm.StringLengths.EMERGENCY_RESPONSE_AGENCY, MinimumLength = ConfinedSpaceForm.StringLengths.MIN_LENGTH)]
        public string EmergencyResponseAgency { get; set; }
        [RequiredWhen(nameof(CanBeControlledByVentilationAlone), false)]
        [StringLength(ConfinedSpaceForm.StringLengths.EMERGENCY_RESPONSE_CONTACT, MinimumLength = ConfinedSpaceForm.StringLengths.MIN_LENGTH)]
        public string EmergencyResponseContact { get; set; }

        #endregion

        #region Authorization to begin entry operation

        [DoesNotAutoMap("Used for validation and needed in the view")]
        public bool IsBeginEntrySectionPreviouslySigned => Original?.IsBeginEntrySectionSigned == true;

        [DoesNotAutoMap, CheckBox]
        public bool? IsBeginEntrySectionSigned { get; set; }

        #endregion

        #endregion

        [DoesNotAutoMap]
        public bool? IsFormComplete => Original?.IsCompleted;

        #endregion

        #region Constructor

        public ConfinedSpaceFormViewModel(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private void MapSignaturesToEntity(ConfinedSpaceForm entity)
        {
            // Once a section has a signature, the signature can not be changed. Multiple
            // users will be editing this record and we don't want a previous signature to be
            // overwritten by another user/timestamp.

            void SignSection(Action<Employee, DateTime> signAndDate)
            {
                signAndDate(_container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee,
                    _container.GetInstance<IDateTimeProvider>().GetCurrentDate());
            }

            if (IsBeginEntrySectionSigned == true && entity.BeginEntryAuthorizedBy == null)
            {
                SignSection((emp, date) =>
                {
                    entity.BeginEntryAuthorizedBy = emp;
                    entity.BeginEntryAuthorizedAt = date;
                });
            }

            if (IsBumpTestConfirmed == true && entity.BumpTestConfirmedBy == null)
            {
                SignSection((emp, date) =>
                {
                    entity.BumpTestConfirmedBy = emp;
                    entity.BumpTestConfirmedAt = date;
                });
            }

            if (IsHazardSectionSigned == true && entity.HazardSignedBy == null)
            {
                SignSection((emp, date) =>
                {
                    entity.HazardSignedBy = emp;
                    entity.HazardSignedAt = date;
                });
            }

            if (IsPermitCancelledSectionSigned == true && entity.PermitCancelledBy == null)
            {
                SignSection((emp, date) =>
                {
                    entity.PermitCancelledBy = emp;
                    entity.PermitCancelledAt = date;
                });
            }

            if (IsReclassificationSectionSigned == true && entity.ReclassificationSignedBy == null)
            {
                SignSection((emp, date) =>
                {
                    entity.ReclassificationSignedBy = emp;
                    entity.ReclassificationSignedAt = date;
                });
            }
        }

        private void MapHazardsToEntity(ConfinedSpaceForm entity)
        {
            // Hazards will be null if section 2 is not completed as the tab
            // will not be visible/rendered at all. We don't want to clear out
            // any existing hazards if there's a screw-up somewhere.
            if (Hazards == null)
            {
                return;
            }
            entity.Hazards.Clear();
            foreach (var hazard in Hazards.Where(x => x.IsChecked == true))
            {
                var hazardEntity = new ConfinedSpaceFormHazard
                {
                    ConfinedSpaceForm = entity
                };
                entity.Hazards.Add(hazard.MapToEntity(hazardEntity));
            }
        }

        private void MapAtmosphericTestsToEntity(ConfinedSpaceForm entity)
        {
            // NewAtmosphericTests will be null whenever they're creating/editing a record
            // that doesn't include a new atmospheric test.
            // Also, IsSection2Completed may become true after this method completed. This will matter in MC-2665
            if (entity.IsBumpTestConfirmed && NewAtmosphericTests != null)
            {
                foreach (var test in NewAtmosphericTests)
                {
                    var testEntity = new ConfinedSpaceFormAtmosphericTest();
                    test.MapToEntity(testEntity);
                    testEntity.ConfinedSpaceForm = entity;
                    entity.AtmosphericTests.Add(testEntity);
                }
            }
        }

        // MC-2511 requested that IsFireWatchRequired also be true if the
        // hot work permit is also required. 
        private void SetFireWatchValueIfHotWorksTrue(ConfinedSpaceForm entity)
        {
            if (entity.IsHotWorkPermitRequired == true)
            {
                entity.IsFireWatchRequired = true;
            }
        }
        private IEnumerable<ValidationResult> ValidatePermitHasNotAlreadyBeenCancelled()
        {
            var entity = _container.GetInstance<IRepository<ConfinedSpaceForm>>().Find(Id);
            if (entity == null)
            {
                yield break; // cut out early. This will 404 anyway.
            }
            if (entity.IsPermitCancelledSectionSigned)
            {
                yield return new ValidationResult("This permit has been cancelled and can no longer be edited.");
            }
        }

        private static ValidationResult getUserMustHaveEmployeeRecord(string propertyName)
        {
            return new ValidationResult(USER_MUST_HAVE_EMPLOYEE_RECORD, new[] { propertyName });
        }

        private IEnumerable<ValidationResult> ValidateUserHasEmployeeRecordWhenSigning()
        {
            var userHasEmployee = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee != null;

            if (!userHasEmployee)
            {
                if (NewAtmosphericTests != null && NewAtmosphericTests.Any())
                {
                    yield return getUserMustHaveEmployeeRecord(nameof(NewAtmosphericTests));
                }

                if (IsBeginEntrySectionSigned == true && !IsBeginEntrySectionPreviouslySigned)
                {
                    yield return getUserMustHaveEmployeeRecord(nameof(IsBeginEntrySectionSigned));
                }

                if (IsBumpTestConfirmed == true && !IsBumpTestPreviouslyConfirmed)
                {
                    yield return getUserMustHaveEmployeeRecord(nameof(IsBumpTestConfirmed));
                }

                if (IsHazardSectionSigned == true && !IsHazardSectionPreviouslySigned)
                {
                    yield return getUserMustHaveEmployeeRecord(nameof(IsHazardSectionSigned));
                }

                if (IsPermitCancelledSectionSigned == true && !IsPermitCancelledSectionPreviouslySigned)
                {
                    yield return getUserMustHaveEmployeeRecord(nameof(IsPermitCancelledSectionSigned));
                }

                if (IsReclassificationSectionSigned == true && !IsReclassificationSectionPreviouslySigned)
                {
                    yield return getUserMustHaveEmployeeRecord(nameof(IsReclassificationSectionSigned));
                }
            }
        }

        private void SatisfyConfinedSpaceFormPreRequisiteWhenFormComplete(ConfinedSpaceForm entity)
        {
            if (entity.ProductionWorkOrder != null && entity.IsCompleted && entity.ProductionWorkOrder.ProductionWorkOrderProductionPrerequisites.Any(x => x.ProductionPrerequisite.Id == ProductionPrerequisite.Indices.IS_CONFINED_SPACE))
            {
                var dateTimeProvider = _container.GetInstance<IDateTimeProvider>();

                var existingConfinedSpacePreReq = entity.ProductionWorkOrder.ProductionWorkOrderProductionPrerequisites.Where(x =>
                    x.ProductionPrerequisite.Id == ProductionPrerequisite.Indices.IS_CONFINED_SPACE).FirstOrDefault();

                existingConfinedSpacePreReq.SatisfiedOn = dateTimeProvider.GetCurrentDate();
            }
        }

        private IEnumerable<ValidationResult> ValidateEntrants()
        {
            var seenSoFar = new List<(int Employee, int Type)>();

            foreach (var existing in Original?.Entrants ?? new List<ConfinedSpaceFormEntrant>())
            {
                if (existing.Employee == null)
                {
                    // we only care about employee entrants
                    continue;
                }

                seenSoFar.Add((Employee: existing.Employee.Id, Type: existing.EntrantType.Id));
            }

            foreach (var entrant in NewEntrants ?? new List<CreateConfinedSpaceFormEntrant>())
            {
                if (!entrant.Employee.HasValue)
                {
                    continue;
                }

                var toWatchFor = new List<int> {entrant.EntrantType.Value};

                if (entrant.EntrantType == ConfinedSpaceFormEntrantType.Indices.ENTRANT)
                {
                    toWatchFor.Add(ConfinedSpaceFormEntrantType.Indices.ATTENDANT);
                    toWatchFor.Add(ConfinedSpaceFormEntrantType.Indices.ENTRY_SUPERVISOR);
                }
                else
                {
                    toWatchFor.Add(ConfinedSpaceFormEntrantType.Indices.ENTRANT);
                }

                if (seenSoFar.Any(e =>
                    e.Employee == entrant.Employee && toWatchFor.Contains(e.Type)))
                {
                    yield return new ValidationResult("Invalid combination of employees and entrant types detected.",
                        new[] {"Entrants"});
                }

                seenSoFar.Add((Employee: entrant.Employee.Value, Type: entrant.EntrantType.Value));
            }
        }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();

            // Populate the Hazards list with default unchecked values for every hazard type.
            var viewModelFactory = _container.GetInstance<IViewModelFactory>();
            // ToList() is needed because trying to call GetAll().Select() with viewModelFactory does not work
            // with sql queries.
            var hazardTypes = _container.GetInstance<IRepository<ConfinedSpaceFormHazardType>>().GetAll().ToList();
            Hazards = hazardTypes.Select(x => viewModelFactory.BuildWithOverrides<ConfinedSpaceFormHazardViewModel>(new
            {
                HazardTypeDescription = x.Description,
                HazardType = x.Id,
                IsChecked = false
            })).ToList();
        }

        public override void Map(ConfinedSpaceForm entity)
        {
            if (entity.ShortCycleWorkOrderNumber != null)
            {
                State = entity.OperatingCenter?.State.Id;
            }

            base.Map(entity);

            IsBeginEntrySectionSigned = IsBeginEntrySectionPreviouslySigned;
            IsBumpTestConfirmed = IsBumpTestPreviouslyConfirmed;
            IsHazardSectionSigned = IsHazardSectionPreviouslySigned;
            IsPermitCancelledSectionSigned = IsPermitCancelledSectionPreviouslySigned;
            IsReclassificationSectionSigned = IsReclassificationSectionPreviouslySigned;

            foreach (var hazard in entity.Hazards)
            {
                var hazardVm = Hazards.Single(x => x.HazardType == hazard.HazardType.Id);
                hazardVm.IsChecked = true;
                hazardVm.Notes = hazard.Notes;
            }
        }

        public override ConfinedSpaceForm MapToEntity(ConfinedSpaceForm entity)
        {
            // this needs to happen before the call to base, otherwise the bump
            // test precondition will fail in MapAtmosphericTestsToEntity
            MapSignaturesToEntity(entity);
            var entityMonitor = entity.GasMonitor;

            if (ProductionWorkOrderDisplay != null)
            {
                OperatingCenter = ProductionWorkOrderDisplay.OperatingCenter.Id;
            }
            else if (WorkOrderDisplay != null)
            {
                OperatingCenter = WorkOrderDisplay.OperatingCenter.Id;
            }

            entity = base.MapToEntity(entity);
            SetFireWatchValueIfHotWorksTrue(entity);
            MapHazardsToEntity(entity);
            SatisfyConfinedSpaceFormPreRequisiteWhenFormComplete(entity);
            if (entity.IsBumpTestConfirmed && entity.GasMonitor == null)
            {
                entity.GasMonitor = entityMonitor;
            }
            
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidatePermitHasNotAlreadyBeenCancelled())
                       .Concat(ValidateUserHasEmployeeRecordWhenSigning())
                       .Concat(ValidateEntrants());
        }

        #endregion
    }

    public class CreateConfinedSpaceForm : ConfinedSpaceFormViewModel
    {
        #region Constructor

        public CreateConfinedSpaceForm(IContainer container) : base(container) { }

        #endregion

        public override void SetDefaults()
        {
            base.SetDefaults();
            GeneralDateTime = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();

            // NOTE: The different work order values are set in the ConfinedSpaceFormController.New action.
            if (ProductionWorkOrderDisplay != null)
            {
                OperatingCenter = ProductionWorkOrderDisplay.OperatingCenter.Id;
            }
            else if (WorkOrderDisplay != null)
            {
                OperatingCenter = WorkOrderDisplay.OperatingCenter.Id;
            }
        }
    }

    public class EditConfinedSpaceForm : ConfinedSpaceFormViewModel
    {
        #region Constructor

        public EditConfinedSpaceForm(IContainer container) : base(container) { }

        #endregion
    }

    public class PostCompletionConfinedSpaceForm : PostCompletionConfinedSpaceFormBase
    {
        private void MapCancellationSignaturesToEntity(ConfinedSpaceForm entity)
        {
            void SignSection(Action<Employee, DateTime> signAndDate)
            {
                signAndDate(_container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee,
                    _container.GetInstance<IDateTimeProvider>().GetCurrentDate());
            }

            if (IsPermitCancelledSectionSigned == true && entity.PermitCancelledBy == null)
            {
                SignSection((emp, date) => {
                    entity.PermitCancelledBy = emp;
                    entity.PermitCancelledAt = date;
                });
            }
        }

        public override ConfinedSpaceForm MapToEntity(ConfinedSpaceForm entity)
        {
            entity = base.MapToEntity(entity);
            MapCancellationSignaturesToEntity(entity);
            return entity;
        }

        public PostCompletionConfinedSpaceForm(IContainer container) : base(container) { }
    }
}
