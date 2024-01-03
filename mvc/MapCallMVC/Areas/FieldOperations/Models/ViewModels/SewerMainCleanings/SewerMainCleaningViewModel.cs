using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerMainCleanings
{
    public abstract class SewerMainCleaningViewModel : ViewModel<SewerMainCleaning>
    {
        #region Properties

        [DoesNotAutoMap("Display only")]
        public SewerMainCleaning Display
        {
            get
            {
                var repo = _container.GetInstance<ISewerMainCleaningRepository>();
                return repo.Find(Id);
            }
        }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [Required, EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [Required, DateTimePicker]
        public DateTime? Date { get; set; }

        public int? GallonsOfWaterUsed { get; set; }

        [EntityMap, EntityMustExist(typeof(Hydrant))]
        public abstract int? HydrantUsed { get; set; }

        [Required]
        public float? FootageOfMainInspected { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OpeningCondition))]
        public int? Opening1Condition { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OpeningFrameAndCover))]
        public int? Opening1FrameAndCover { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OpeningCondition))]
        public int? Opening2Condition { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OpeningFrameAndCover))]
        public int? Opening2FrameAndCover { get; set; }

        public string TableNotes { get; set; }

        [Required]
        public bool? Overflow { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(CleaningSchedule))]
        public int? CleaningSchedule { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [EntityMap, EntityMustExist(typeof(Street))]
        public int? CrossStreet2 { get; set; }

        [Required, DateTimePicker]
        public DateTime? InspectedDate { get; set; }

        [DropDown("FieldOperations", "SewerOverflow", "ByStreetId", DependsOn = "Street", PromptText = "Select a street above",ErrorText = "No matching overflows on the selected street")]
        [EntityMap, EntityMustExist(typeof(SewerOverflow))]
        [RequiredWhen("Overflow", ComparisonType.EqualTo, true)]
        public int? SewerOverflow { get; set; }

        [DoesNotAutoMap("Set by MapToEntity - for controller to know if it gets sent")]
        public bool SendToSAP { get; set; }

        public bool BlockageFound { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(CauseOfBlockage))]
        [RequiredWhen("BlockageFound", true, FieldOnlyVisibleWhenRequired = true)]
        public int? CauseOfBlockage { get; set; }

        [AutoComplete("FieldOperations", "SewerOpening", "ByPartialSewerOpeningMatchByTown", DependsOn = "Town", DisplayProperty = nameof(SewerOpening.OpeningNumber), PlaceHolder = "Start typing to list Openings")]
        [EntityMap, EntityMustExist(typeof(SewerOpening)), Required]
        public int? Opening1 { get; set; }
        
        [AutoMap(SecondaryPropertyName = nameof(SewerMainCleaning.Opening2IsATerminus))]
        [View("Opening 2 is a Terminus")]
        public bool OpeningTwoIsATerminus { get; set; }

        [AutoComplete("FieldOperations", "SewerOpening", "ByPartialSewerOpeningMatchByTown", DependsOn = "Town", DisplayProperty = nameof(SewerOpening.OpeningNumber), PlaceHolder = "Start typing to list Openings")]
        [EntityMap, EntityMustExist(typeof(SewerOpening))]
        [RequiredWhen(nameof(OpeningTwoIsATerminus), ComparisonType.EqualTo, false)]
        public int? Opening2 { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(SewerMainInspectionType)), DropDown]
        public int? InspectionType { get; set; }
        
        [EntityMap, EntityMustExist(typeof(SewerMainInspectionGrade)), DropDown]
        public int? InspectionGrade { get; set; }

        [EntityMap, EntityMustExist(typeof(Street))]
        public abstract int? Street { get; set; }
        [EntityMap, EntityMustExist(typeof(Street))]
        public abstract int? CrossStreet { get; set; }

        #endregion

        #region Constructors

        public SewerMainCleaningViewModel(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override SewerMainCleaning MapToEntity(SewerMainCleaning entity)
        {
            entity = base.MapToEntity(entity);

            if (InspectionType.HasValue && new[] {
                    SewerMainInspectionType.Indices.MAIN_CLEANING_PM,
                    SewerMainInspectionType.Indices.SMOKE_TEST
                }.Contains(InspectionType.Value))
            {
                entity.InspectionGrade = null;
            }

            if (!BlockageFound)
            {
                entity.CauseOfBlockage = null;
            }

            SendToSAP = (entity.Opening1 != null && entity.Opening1.OperatingCenter.SAPEnabled &&
                         !entity.Opening1.OperatingCenter.IsContractedOperations &&
                         (entity.SAPNotificationNumber == null || entity.SAPNotificationNumber == ""))
                        || (entity.Opening2 != null && entity.Opening2.OperatingCenter.SAPEnabled &&
                            !entity.Opening2.OperatingCenter.IsContractedOperations &&
                            (entity.SAPNotificationNumber == null || entity.SAPNotificationNumber == ""));
            entity.NeedsToSync = true;

            return entity;
        }

        #endregion
    }
}
