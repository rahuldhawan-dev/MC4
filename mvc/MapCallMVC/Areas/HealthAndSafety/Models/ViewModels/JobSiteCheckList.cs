using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NHibernate.Type;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public abstract class BaseJobSiteCheckListViewModel : ViewModel<JobSiteCheckList>
    {
        #region Fields

        private List<int> _protectionTypes = new List<int>();
        protected readonly IViewModelFactory _viewModelFactory;
        private IEnumerable<JobSiteCheckListCrewMembers> _displayCrewMembers;

        #endregion

        #region Properties

        [Multiline, Required, StringLengthNotRequired("ntext field")]
        public string Address { get; set; }

        [JobSiteCheckList.YesNah]
        [RequiredWhen("SafetyBriefComplete", ComparisonType.EqualTo, true, ErrorMessage = "Required"), View(JobSiteCheckList.Display.ALL_EMPLOYEES_WEARING_PPE)]
        public bool? AllEmployeesWearingAppropriatePersonalProtectionEquipment { get; set; }

        [JobSiteCheckList.YesNah, RequiredWhen("HasExcavation", ComparisonType.EqualTo, true, ErrorMessage = "Required"), View(JobSiteCheckList.Display.ALL_MATERIALS_SET_BACK)]
        public bool? AllMaterialsSetBackFromEdgeOfTrenches { get; set; }

        [JobSiteCheckList.YesNah, RequiredWhen("HasExcavation", ComparisonType.EqualTo, true, ErrorMessage = "Required"), View(JobSiteCheckList.Display.SPOTTER_ASSIGNED)]
        public bool? SpotterAssigned { get; set; }

        [JobSiteCheckList.YesNah, RequiredWhen("HasExcavation", ComparisonType.EqualTo, true, ErrorMessage = "Required"), View(JobSiteCheckList.Display.IS_MANUFACTURER_DATA_ONSITE_FOR_SHORING_OR_SHIELDING_EQUIPMENT)]
        public bool? IsManufacturerDataOnSiteForShoringOrShieldingEquipment { get; set; }

        [RequiredWhen("HasExcavation", ComparisonType.EqualTo, true, ErrorMessage = "Required"), View(JobSiteCheckList.Display.IS_THE_EXCAVATION_GUARDED_FROM_ACCIDENTAL_ENTRY)]
        public bool? IsTheExcavationGuardedFromAccidentalEntry { get; set; }

        [RequiredWhen("HasExcavation", ComparisonType.EqualTo, true, ErrorMessage = "Required"), View(JobSiteCheckList.Display.ARE_THERE_VISUAL_SIGNS_OF_POTENTIAL_SOIL_COLLAPSE)]
        public bool? AreThereAnyVisualSignsOfPotentialSoilCollapse { get; set; }

        [RequiredWhen("HasExcavation", ComparisonType.EqualTo, true, ErrorMessage = "Required"), View(JobSiteCheckList.Display.IS_THE_EXCAVATION_SUBJECT_TO_VIBRATION)]
        public bool? IsTheExcavationSubjectToVibration { get; set; }

        [EntityMap]
        [EntityMustExist(typeof(SoilConditionsWithinExcavationType))]
        [DropDown]
        public virtual int? SoilConditionsWithinExcavationType { get; set; }

        [EntityMap]
        [EntityMustExist(typeof(SoilCompositionExcavationType))]
        [DropDown]
        public virtual int? SoilCompositionExcavationType { get; set; }

        [JobSiteCheckList.YesNah, RequiredWhen("SafetyBriefComplete", true, ErrorMessage = "Required"), View(JobSiteCheckList.Display.ALL_STRUCTURES_SUPPORTED)]
        public bool? AllStructuresSupportedOrProtected { get; set; }

        [JobSiteCheckList.YesNah, RequiredWhen("HasExcavation", true, ErrorMessage = "Required"), View(JobSiteCheckList.Display.ARE_EXPOSED_UTILITIES_PROTECTED)]
        public bool? AreExposedUtilitiesProtected { get; set; }

        [View("O2"), RequiredWhen("HasAtmosphereBeenTested", true, ErrorMessage = "Required")]
        public decimal? AtmosphericOxygenLevel { get; set; }

        [View("CO"), RequiredWhen("HasAtmosphereBeenTested", true, ErrorMessage = "Required")]
        public decimal? AtmosphericCarbonMonoxideLevel { get; set; }

        [View("LEL"), RequiredWhen("HasAtmosphereBeenTested", true, ErrorMessage = "Required")]
        public decimal? AtmosphericLowerExplosiveLimit { get; set; }

        [RequiredWhen("SafetyBriefComplete", true, ErrorMessage = "Required")]
        public bool? HasExcavation { get; set; }

        [RequiredWhen("SafetyBriefComplete", true)]
        public DateTime? CheckListDate { get; set; }

        [Multiline, StringLengthNotRequired("ntext field")]
        public virtual string Comments { get; set; }

        [DoesNotAutoMap("Display only")]
        public virtual IEnumerable<JobSiteCheckListComment> DisplayComments
        {
            get
            {
                var original = _container.GetInstance<IJobSiteCheckListRepository>().Find(Id);
                if (original != null)
                {
                    return original.CommentsByDate;
                }
                return Enumerable.Empty<JobSiteCheckListComment>();
            }
        }

        [Required, EntityMap]
        [EntityMustExist(typeof(Employee))]
        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterIdForJobSiteCheckLists", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        public int? CompetentEmployee { get; set; }

        [JobSiteCheckList.YesNah, RequiredWhen("HasTrafficControl", true, ErrorMessage = "Required")]
        [View(JobSiteCheckList.Display.COMPLIES_WITH_STANDARDS)]
        public bool? CompliesWithStandards { get; set; }

        [Required, Coordinate(AddressField="Address"), EntityMap]
        [EntityMustExist(typeof(Coordinate))]
        public int? Coordinate { get; set; }

        [DoesNotAutoMap("Display only")]
        public virtual IEnumerable<JobSiteCheckListCrewMembers> DisplayCrewMembers
        {
            get
            {
                if (_displayCrewMembers == null)
                {
                    var original = _container.GetInstance<IJobSiteCheckListRepository>().Find(Id);
                    _displayCrewMembers = original != null
                        ? original.CrewMembersByDate
                        : Enumerable.Empty<JobSiteCheckListCrewMembers>();
                }

                return _displayCrewMembers;
            }
        }

        [DoesNotAutoMap("Logical; used for validation")]
        public virtual bool HasCrewMembers => DisplayCrewMembers.Any();

        [Multiline, StringLengthNotRequired("ntext field"), RequiredWhen(nameof(HasCrewMembers), false)]
        public virtual string CrewMembers { get; set; }

        public List<CreateJobSiteExcavation> Excavations { get; set; }

        [JobSiteCheckList.YesNah, RequiredIfExcavationsFourFeetOrDeeper, View(JobSiteCheckList.Display.HAS_ATMOSPHERE_BEEN_TESTED)]
        public bool? HasAtmosphereBeenTested { get; set; }

        [ClientCallback("JobSiteCheckList.validateFourFeetDeep", ErrorMessage = "This must be checked when there are excavations five feet or deeper.")]
        public bool HasExcavationOverFourFeetDeep { get; set; }

        public bool HasExcavationFiveFeetOrDeeper { get; set; }

        [JobSiteCheckList.YesNah, RequiredWhen("SafetyBriefComplete", true, ErrorMessage = "Required"), View(JobSiteCheckList.Display.HAS_TRAFFIC_CONTROL)]
        [ClientCallback("JobSiteCheckList.validateHasTrafficControl", ErrorMessage = "At least one traffic control type must be selected.")]
        public bool? HasTrafficControl { get; set; }

        [View(JobSiteCheckList.Display.HAS_BARRICADES)]
        public bool HasBarricadesForTrafficControl { get; set; }
        [View(JobSiteCheckList.Display.HAS_CONES)]
        public bool HasConesForTrafficControl { get; set; }
        [View(JobSiteCheckList.Display.HAS_FLAG_PERSON)]
        public bool HasFlagPersonForTrafficControl { get; set; }
        [View(JobSiteCheckList.Display.HAS_POLICE)]
        public bool HasPoliceForTrafficControl { get; set; }
        [View(JobSiteCheckList.Display.HAS_SIGNS)]
        public bool HasSignsForTrafficControl { get; set; }

        [JobSiteCheckList.YesNah, RequiredWhen("SafetyBriefComplete", true, ErrorMessage = "Required"), View(JobSiteCheckList.Display.IS_MARKOUT_VALID)]
        public bool? IsMarkoutValidForSite { get; set; }

        [JobSiteCheckList.YesNah, RequiredIfExcavationsFourFeetOrDeeper, View(JobSiteCheckList.Display.IS_A_LADDER_IN_PLACE)]
        public bool? IsALadderInPlace { get; set; }

        [JobSiteCheckList.YesNah, RequiredIfExcavationsFourFeetOrDeeper, View(JobSiteCheckList.Display.IS_LADDER_ON_SLOPE)]
        public bool? IsLadderOnSlope { get; set; }

        [JobSiteCheckList.YesNah, RequiredWhen("IsMarkoutValidForSite", true, ErrorMessage = "Required"), View(JobSiteCheckList.Display.IS_MARKOUT_EMERGENCY)]
        public bool? IsEmergencyMarkoutRequest { get; set; }

        // This property's needed for validation only. 
        [Required, MMSINC.Metadata.Secured]
        public bool? IsPressurizedRisksRestrainedFieldRequired { get; set; }

        [JobSiteCheckList.YesNah, RequiredIfExcavationsFiveFeetOrDeeper, View(JobSiteCheckList.Display.IS_SHORING_SYSTEM_USED)]
        public bool? IsShoringSystemUsed { get; set; }

        [JobSiteCheckList.YesNah, RequiredIfExcavationsFiveFeetOrDeeper, View(JobSiteCheckList.Display.IS_SLOPE_ANGLE_NOT_LESS)]
        public bool? IsSlopeAngleNotLessThanOneHalfHorizontalToOneVertical { get; set; }

        [JobSiteCheckList.YesNah, RequiredIfExcavationsFourFeetOrDeeper, View(JobSiteCheckList.Display.LADDER_EXTENDS_ABOVE_GRADE)]
        public bool? LadderExtendsAboveGrade { get; set; }

        [View("Electric"), BoolFormat("Yes", "No", "N/A")]
        public bool? MarkedElectric { get; set; }
        [View("Fuel/Gas"), BoolFormat("Yes", "No", "N/A")]
        public bool? MarkedFuelGas { get; set; }
        [View("Other"), BoolFormat("Yes", "No", "N/A")]
        public bool? MarkedOther { get; set; }
        [View("Sanitary Sewer"), BoolFormat("Yes", "No", "N/A")]
        public bool? MarkedSanitarySewer { get; set; }
        [View("Telephone"), BoolFormat("Yes", "No", "N/A")]
        public bool? MarkedTelephone { get; set; }
        [View("Water"), BoolFormat("Yes", "No", "N/A")]
        public bool? MarkedWater { get; set; }

        [RequiredWhen("IsMarkoutValidForSite", true)]
        [StringLength(JobSiteCheckList.StringLengths.MARKOUT_NUMBER)]
        public string MarkoutNumber { get; set; }

        [RequiredWhen(nameof(PressurizedRiskRestrainedType), "GetNoPressurizedRiskRestrainedType", typeof(BaseJobSiteCheckListViewModel))]
        [DropDown, EntityMap, EntityMustExist(typeof(JobSiteCheckListNoRestraintReasonType))]
        public int? NoRestraintReason { get; set; }

        [Required, EntityMap]
        [EntityMustExist(typeof(OperatingCenter))]
        [DropDown]
        public int? OperatingCenter { get; set; }

        [RequiredWhen(nameof(IsPressurizedRisksRestrainedFieldRequired), true)]
        [DropDown, EntityMap, EntityMustExist(typeof(JobSiteCheckListPressurizedRiskRestrainedType))]
        public int? PressurizedRiskRestrainedType { get; set; }

        [CheckBoxList]
        [View(JobSiteCheckList.Display.PROTECTION_TYPE)]
        public List<int> ProtectionTypes
        {
            get { return _protectionTypes; }
            set { _protectionTypes = value; }
        }

        [DoesNotAutoMap]
        [ClientCallback("JobSiteCheckList.validateProtectionTypes", ErrorMessage = "At least one protection type must be selected.")]
        public string ProtectionTypesClientSideValidationHack { get; set; }

        [RequiredWhen(nameof(PressurizedRiskRestrainedType), "GetYesPressurizedRiskRestrainedType", typeof(BaseJobSiteCheckListViewModel))]
        [DropDown, EntityMap, EntityMustExist(typeof(JobSiteCheckListRestraintMethodType))]
        public int? RestraintMethod { get; set; }

        [JobSiteCheckList.YesNah, RequiredIfExcavationsFiveFeetOrDeeper, View(JobSiteCheckList.Display.SHORING_SYSTEM_EXTENDS)]
        public bool? ShoringSystemSidesExtendAboveBaseOfSlope { get; set; }

        [JobSiteCheckList.YesNah, RequiredIfExcavationsFiveFeetOrDeeper, View(JobSiteCheckList.Display.SHORING_SYSTEM_INSTALLED_TWO_FEET)]
        public bool? ShoringSystemInstalledTwoFeetFromBottomOfTrench { get; set; }

        [EntityMap]
        [EntityMustExist(typeof(Employee))]
        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterIdForJobSiteCheckLists", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        [RequiredWhen("SupervisorSignOffDate", ComparisonType.NotEqualTo, null)]
        public int? SupervisorSignOffEmployee { get; set; }

        [RequiredWhen("SupervisorSignOffEmployee", ComparisonType.NotEqualTo, null)]
        public DateTime? SupervisorSignOffDate { get; set; }

        [JobSiteCheckList.YesNah, RequiredWhen("HasExcavation", true, ErrorMessage = "Required"), View(JobSiteCheckList.Display.WATER_CONTROL_SYSTEM_IN_USE)]
        public bool? WaterControlSystemsInUse { get; set; }

        [StringLength(JobSiteCheckList.StringLengths.WORK_ORDER_ID)]
        [View(JobSiteCheckList.Display.SAP_WORK_ORDER)]
        public string SAPWorkOrderId { get; set; }
        
        [EntityMap, EntityMustExist(typeof(WorkOrder))]
        [View(JobSiteCheckList.Display.MAPCALL_WORKORDER_ID)]
        public int? MapCallWorkOrder { get; set; }

        #region SafetyBrief

        [Required, View("Date/Time"), DateTimePicker]
        public DateTime? SafetyBriefDateTime { get;set; }
        [Required]
        public bool? AnyPotentialWeatherHazards { get; set; }
        [EntityMap, MultiSelect, EntityMustExist(typeof(JobSiteCheckListSafetyBriefWeatherHazardType))]
        [RequiredWhen("AnyPotentialWeatherHazards", ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(JobSiteCheckList.Display.SAFETY_BRIEF_IF_YES)]
        public int[] SafetyBriefWeatherHazardTypes { get; set; }
        [RequiredWhen("AnyPotentialWeatherHazards", ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        public bool? HadConversationAboutWeatherHazard { get; set; }
        [Multiline, StringLength(JobSiteCheckList.StringLengths.NOTES), RequiredWhen("HadConversationAboutWeatherHazard", ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        public string HadConversationAboutWeatherHazardNotes { get; set; }
        [Required]
        public bool? AnyTimeOfDayConstraints { get; set; }
        [EntityMap, MultiSelect, EntityMustExist(typeof(JobSiteCheckListSafetyBriefTimeOfDayConstraintType))]
        [RequiredWhen("AnyTimeOfDayConstraints", ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(JobSiteCheckList.Display.SAFETY_BRIEF_IF_YES)]
        public int[] SafetyBriefTimeOfDayConstraintTypes { get; set; }
        [Required]
        public bool? AnyTrafficHazards { get; set; }
        [EntityMap, MultiSelect, EntityMustExist(typeof(JobSiteCheckListSafetyBriefTrafficHazardType))]
        [RequiredWhen("AnyTrafficHazards", ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(JobSiteCheckList.Display.SAFETY_BRIEF_IF_YES)]
        public int[] SafetyBriefTrafficHazardTypes { get; set; }
        [Required]
        public bool? InvolveConfinedSpace { get; set; }
        [Required]
        public bool? AnyPotentialOverheadHazards { get; set; }
        [EntityMap, MultiSelect, EntityMustExist(typeof(JobSiteCheckListSafetyBriefOverheadHazardType))]
        [RequiredWhen("AnyPotentialOverheadHazards", ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(JobSiteCheckList.Display.SAFETY_BRIEF_IF_YES)]
        public int[] SafetyBriefOverheadHazardTypes { get; set; }
        [Required]
        public bool? AnyUndergroundHazards { get; set; }
        [EntityMap, MultiSelect, EntityMustExist(typeof(JobSiteCheckListSafetyBriefUndergroundHazardType))]
        [RequiredWhen("AnyUndergroundHazards", ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(JobSiteCheckList.Display.SAFETY_BRIEF_IF_YES)]
        public int[] SafetyBriefUndergroundHazardTypes { get; set; }
        [Required]
        public virtual bool? AreThereElectricalHazards { get;set; }
        [CheckBox]
        public virtual bool? HaveYouInspectedSlings { get; set; }
        [EntityMap, MultiSelect, EntityMustExist(typeof(JobSiteCheckListSafetyBriefElectricalHazardType))]
        [RequiredWhen("AreThereElectricalHazards", ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(JobSiteCheckList.Display.SAFETY_BRIEF_IF_YES)]
        public int[] SafetyBriefElectricalHazardTypes { get; set; }
        [CheckBox]
        public bool? WorkingWithACPipe { get;set; }
        [RequiredWhen("WorkingWithACPipe", ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        public bool? CrewMembersTrainedInACPipe { get; set; }
        [Required]
        public bool? HaveEquipmentToDoJobSafely { get;set; }
        [RequiredWhen("HaveEquipmentToDoJobSafely", ComparisonType.EqualTo, false, FieldOnlyVisibleWhenRequired = true), Multiline, StringLength(JobSiteCheckList.StringLengths.NOTES)]
        public string HaveEquipmentToDoJobSafelyNotes { get; set; }
        [Required]
        public bool? ReviewedErgonomicHazards { get;set; }
        [RequiredWhen("ReviewedErgonomicHazards", ComparisonType.EqualTo, false,  FieldOnlyVisibleWhenRequired = true),Multiline, StringLength(JobSiteCheckList.StringLengths.NOTES)]
        public string ReviewedErgonomicHazardsNotes { get; set; }
        [CheckBox, Required]
        public bool? ReviewedLocationOfSafetyEquipment { get;set; }
        [Required]
        public bool? OtherHazardsIdentified { get;set; }
        [RequiredWhen("OtherHazardsIdentified", ComparisonType.EqualTo, true,  FieldOnlyVisibleWhenRequired = true), Multiline, StringLength(JobSiteCheckList.StringLengths.NOTES)]
        public string OtherHazardNotes { get; set; }
        [CheckBox]
        public bool? HadDiscussionAboutHazardsAndPrecautions { get;set;}
        [Multiline, StringLength(JobSiteCheckList.StringLengths.NOTES), RequiredWhen("HadDiscussionAboutHazardsAndPrecautions", ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        public string HadDiscussionAboutHazardsAndPrecautionsNotes { get;set; }
        [CheckBox]
        public bool? CrewMembersRemindedOfStopWorkAuthority { get; set; }
        [CheckBox, ClientCallback("SafetyBrief.validatePpeChecked", ErrorMessage = "At least one PPE type must be checked")]
        public bool? HeadProtection { get; set; }
        [CheckBox]
        public bool? HandProtection { get; set; }
        [CheckBox]
        public bool? ElectricalProtection { get; set; }
        [CheckBox]
        public bool? FootProtection { get; set; }
        [CheckBox]
        public bool? EyeProtection { get; set; }
        [CheckBox]
        public bool? FaceShield { get; set; }
        [CheckBox]
        public bool? SafetyGarment { get; set; }
        [CheckBox]
        public bool? HearingProtection { get; set; }
        [CheckBox]
        public bool? RespiratoryProtection { get; set; }
        [CheckBox]
        public bool? PPEOther { get; set; }
        [Multiline, StringLength(JobSiteCheckList.StringLengths.NOTES), RequiredWhen("PPEOther", ComparisonType.EqualTo, true,  FieldOnlyVisibleWhenRequired = true)]
        public string PPEOtherNotes { get; set; }

        #endregion

        #endregion

        #region LogicalProps

        [DoesNotAutoMap]
        public bool? IsCreate { get; set; }

        [DoesNotAutoMap]
        public bool SafetyBriefComplete
        {
            get { return AnyPotentialWeatherHazards != null && IsCreate == false; }
        }

        #endregion

        #region Constructor

        public BaseJobSiteCheckListViewModel(IContainer container) : base(container)
        {
            Excavations = Excavations ?? new List<CreateJobSiteExcavation>();
            _viewModelFactory = _container.GetInstance<IViewModelFactory>();
        }

        #endregion

        #region Public Methods

        public override void Map(JobSiteCheckList entity)
        {
            base.Map(entity);

            ProtectionTypes.Clear();
            ProtectionTypes.AddRange(entity.ProtectionTypes.Where(x => x != null).Select(x => x.Id));

            Excavations = entity.Excavations.Select(x => _viewModelFactory.Build<CreateJobSiteExcavation, JobSiteExcavation>(x)).ToList();
        }

        public override JobSiteCheckList MapToEntity(JobSiteCheckList entity)
        {
            base.MapToEntity(entity);

            entity.ProtectionTypes.Clear();

            var repo = _container.GetInstance<IRepository<JobSiteExcavationProtectionType>>();
            foreach (var id in ProtectionTypes)
            {
                entity.ProtectionTypes.Add(repo.Find(id));
            }

            MapExcavationsToEntity(entity);
            MapCommentsToEntity(entity);
            MapCrewMembersToEntity(entity);

            return entity;
        }

        private void MapExcavationsToEntity(JobSiteCheckList entity)
        {
            // NOTE: Excavations are not editable. Do not do value mapping
            //       for existing excavations. On the client they *are*
            //       editable, but they're done in such a way that the existing
            //       ones are deleted and replaced with new ones.

            var existingExcavations = entity.Excavations.ToDictionary(x => x.Id, x => x);
            var existingOnViewModel = Excavations.Where(x => x.Id > 0).Select(x => x.Id);
            var existingToRemove = existingExcavations.Keys.Except(existingOnViewModel);

            foreach (var id in existingToRemove)
            {
                entity.Excavations.Remove(existingExcavations[id]);
            }

            var newOnViewModel = Excavations.Where(x => x.Id <= 0);
            foreach (var vm in newOnViewModel)
            {
                var exc = new JobSiteExcavation();
                vm.MapToEntity(exc);
                exc.JobSiteCheckList = entity;
                entity.Excavations.Add(exc);
            }
        }

        private void MapCommentsToEntity(JobSiteCheckList entity)
        {
            if (!string.IsNullOrWhiteSpace(Comments))
            {
                var c = new JobSiteCheckListComment {
                    Comments = Comments,
                    JobSiteCheckList = entity
                };
                entity.Comments.Add(c);
            }
        }

        private void MapCrewMembersToEntity(JobSiteCheckList entity)
        {
            if (!string.IsNullOrWhiteSpace(CrewMembers))
            {
                var c = new JobSiteCheckListCrewMembers {
                    CrewMembers = CrewMembers,
                    JobSiteCheckList = entity
                };
                entity.CrewMembers.Add(c);
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                .Concat(ValidateRecordIsEditable())
                .Concat(ValidateExcavations())
                .Concat(ValidateTrafficControlTypes())
                .Concat(ValidateProtectionTypes())
                .Concat(Validate4FootQuestions());
        }

        private IEnumerable<ValidationResult> ValidateRecordIsEditable()
        {
            // This validation should never actually appear to the user. The Edit
            // action is doing a redirect to the show page in this case. The only
            // way a user would see this is if A) They had the edit page open prior
            // to the supervisor approving it, and then they hit update, or B) They
            // bypassed the edit page and made their own form.

            var original = _container.GetInstance<IJobSiteCheckListRepository>().Find(Id);
            var isAdmin = _container.GetInstance<IAuthenticationService>().CurrentUserIsAdmin;

            // Admins are allowed to edit regardless. If the entity doesn't exist then it's gonna
            // be handled as a 404 elsewhere, or it's a new record.
            if ((original != null && original.IsApprovedBySupervisor) && !isAdmin)
            {
                yield return new ValidationResult("Record can not be edited after it has been approved by a supervisor.");
            }
        }

        private IEnumerable<ValidationResult> ValidateExcavations()
        {
            if (HasExcavation.GetValueOrDefault() == true && (Excavations == null || !Excavations.Any()))
            {
                yield return
                    new ValidationResult("At least one excavation must be added to a job site checklist.",
                        new[] { "Excavations" });
                yield break;
            }

            // We aren't clearing out excavation records if a user creates a record with excavations and then later
            // edits it to no longer require excavations. I have zero clue why they would ever do that intentionally.
            // Because of that, we still want to validate that the excavations are still valid rather than clearing
            // out the excavation records because they'll most likely want those values back once they've spotted their
            // error.

            if (Excavations != null)
            {
                var existingExcavations = Excavations.Where(x => x.Id > 0).ToArray();

                // This check should work for both creating and editing a checklist.
                var repo = _container.GetInstance<MMSINC.Data.NHibernate.IRepository<JobSiteExcavation>>();
                foreach (var e in existingExcavations)
                {
                    var entity = repo.Find(e.Id);
                    if (entity.JobSiteCheckList.Id != Id)
                    {
                        yield return
                            new ValidationResult("An excavation belonging to another job site checklist can not be added to a different checklist.",
                                new[] { "Excavations" });
                    }
                }
            }
        }

        private IEnumerable<ValidationResult> ValidateTrafficControlTypes()
        {
            if (HasTrafficControl.GetValueOrDefault())
            {
                var hasSomeFormOfTrafficControlSelected = (HasBarricadesForTrafficControl ||
                                                           HasConesForTrafficControl ||
                                                           HasFlagPersonForTrafficControl ||
                                                           HasPoliceForTrafficControl ||
                                                           HasSignsForTrafficControl);

                if (!hasSomeFormOfTrafficControlSelected)
                {
                    yield return new ValidationResult("At least one form of traffic control must be selected.", new[] { "HasTrafficControl" });
                }
            }
        }

        private IEnumerable<ValidationResult> ValidateProtectionTypes()
        {
            if (HasExcavationFiveFeetOrDeeper && !ProtectionTypes.Any())
            {
                yield return
                    new ValidationResult("At least one protection type must be selected.", new[] { "ProtectionTypes" });
            }
        }

        private IEnumerable<ValidationResult> Validate4FootQuestions()
        {
            if (HasExcavationFiveFeetOrDeeper && !HasExcavationOverFourFeetDeep)
            {
                yield return new ValidationResult("This must be checked when there are excavations five feet or deeper.", new[] { "HasExcavationOverFourFeetDeep" });
            }
        }

        public static int GetNoPressurizedRiskRestrainedType()
        {
            return JobSiteCheckListPressurizedRiskRestrainedType.Indices.NO;
        }

        public static int GetYesPressurizedRiskRestrainedType()
        {
            return JobSiteCheckListPressurizedRiskRestrainedType.Indices.YES;
        }

        #endregion

        #region Private classes

        private class RequiredIfExcavationsFourFeetOrDeeperAttribute : RequiredWhenAttribute
        {
            public RequiredIfExcavationsFourFeetOrDeeperAttribute()
                : base("HasExcavationOverFourFeetDeep", true)
            {
                ErrorMessage = "Required";
            }
        }

        private class RequiredIfExcavationsFiveFeetOrDeeperAttribute : RequiredWhenAttribute
        {
            public RequiredIfExcavationsFiveFeetOrDeeperAttribute()
                : base("HasExcavationFiveFeetOrDeeper", true)
            {
                ErrorMessage = "Required";
            }
        }

        #endregion
    }

    public class CreateJobSiteCheckList : BaseJobSiteCheckListViewModel
    {
        #region Properties
        
        // These props are only required during creation due to how 
        // they get mapped to the entity.

        [RequiredWhen("SafetyBriefComplete", true)]
        public override string Comments
        {
            get { return base.Comments; }
            set { base.Comments = value; }
        }

        [Required]
        public override string CrewMembers
        {
            get { return base.CrewMembers; }
            set { base.CrewMembers = value; }
        }

        #endregion

        public override JobSiteCheckList MapToEntity(JobSiteCheckList entity)
        {
            entity.CreatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UniqueName;
            return base.MapToEntity(entity);
        }

        public override void Map(JobSiteCheckList entity)
        {
            base.Map(entity);
            IsCreate = true;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            // As of bug-3940, all NEW records require this field to be true.
            // for MC-2465, they want to be able to submit checklist with just safety brief, so setting to false on create
            IsPressurizedRisksRestrainedFieldRequired = false;

            CheckListDate = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            SafetyBriefDateTime = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();

            var workOrder = _container.GetInstance<IWorkOrderRepository>().Find(MapCallWorkOrder.GetValueOrDefault());
            if (workOrder != null)
            {
                OperatingCenter = workOrder.OperatingCenter.Id;
                SAPWorkOrderId = Convert.ToString(workOrder.SAPWorkOrderNumber);
                Address = workOrder.StreetAddress + ", " + workOrder.TownAddress;

                if (workOrder.Latitude.HasValue && workOrder.Longitude.HasValue)
                {
                    var coordinate = new Coordinate
                    {
                        Latitude = Convert.ToDecimal(workOrder.Latitude),
                        Longitude = Convert.ToDecimal(workOrder.Longitude),

                        // This is terrible and I don't like it. -Ross 2/19/2015
                        Icon = _container.GetInstance<IIconSetRepository>().GetDefaultIconSet(_container.GetInstance<IRepository<MapIcon>>()).DefaultIcon
                    };

                    _container.GetInstance<IRepository<Coordinate>>().Save(coordinate);
                    Coordinate = coordinate.Id;
                }

                var curMarkout = workOrder.CurrentMarkout?.Markout;
                if (curMarkout != null)
                {
                    MarkoutNumber = curMarkout.MarkoutNumber;
                }

                // bug 2236: This needs to be left null(rather than false) if it's not an emergency markout.
                if (workOrder.MarkoutRequirement ==
                    _container.GetInstance<IMarkoutRequirementRepository>().GetEmergencyMarkoutRequirement())
                {
                    IsEmergencyMarkoutRequest = true;
                }

                var employee = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee;
                if (employee != null)
                {
                    CompetentEmployee = employee.Id;
                }
            }
        }

        public CreateJobSiteCheckList(IContainer container) : base(container) { }
    }

    public class EditJobSiteCheckList : BaseJobSiteCheckListViewModel
    {
        public override void Map(JobSiteCheckList entity)
        {
            base.Map(entity);
            IsCreate = false;
            IsPressurizedRisksRestrainedFieldRequired = true;

            if (MapCallWorkOrder != null && string.IsNullOrWhiteSpace(MarkoutNumber))
            {
                var workOrder = _container.GetInstance<IWorkOrderRepository>().Find(MapCallWorkOrder.GetValueOrDefault());

                MarkoutNumber = workOrder?.CurrentMarkout?.Markout?.MarkoutNumber ?? MarkoutNumber;
                IsEmergencyMarkoutRequest = workOrder?.MarkoutRequirement?.Id == (int)MarkoutRequirement.Indices.EMERGENCY ? true : IsEmergencyMarkoutRequest;
            }
        }

        public EditJobSiteCheckList(IContainer container) : base(container) { }
    }

    public class SearchJobSiteCheckList : SearchSet<JobSiteCheckList>
    {
        #region Properties

        [View("Job Site Check List ID")]
        public int? EntityId { get; set; }

        [DropDown]
        public int? OperatingCenter { get; set; }

        public DateRange CheckListDate { get; set; }

        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterIdForJobSiteCheckLists", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        public int? CompetentEmployee { get; set; }

        [BoolFormat("Yes", "No")]
        [Search(CanMap = false)]
        public bool? IsSignedOffBySupervisor { get; set; }

        [View(JobSiteCheckList.Display.SAP_WORK_ORDER)]
        public string SAPWorkOrderId { get; set; }

        // This needs a search alias I think?
        [SearchAlias("MapCallWorkOrder", "Id")]
        public int? MapCallWorkOrder { get; set; }

        // Only used in ModifyValues. Not in view.

        public DateRange SupervisorSignOffDate { get; set; }

        public bool? HasExcavation { get; set; }

        #endregion

        #region Exposed Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);
            if (IsSignedOffBySupervisor.HasValue)
            {
                if (IsSignedOffBySupervisor.Value)
                {
                    mapper.MappedProperties[nameof(SupervisorSignOffDate)].Value = SearchMapperSpecialValues.IsNotNull;
                }
                else
                {
                    mapper.MappedProperties[nameof(SupervisorSignOffDate)].Value = SearchMapperSpecialValues.IsNull;
                    mapper.MappedProperties[nameof(HasExcavation)].Value = SearchMapperSpecialValues.IsNotNull;
                }
            }
        }

        #endregion
    }
}