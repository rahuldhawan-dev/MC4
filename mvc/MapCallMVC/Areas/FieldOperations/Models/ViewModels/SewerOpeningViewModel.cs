using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SewerOpeningViewModel : ViewModel<SewerOpening>
    {
        #region Constants

        private const string ERROR_CRITICAL_NOTES_MUST_BE_NULL = "Critical checkbox must be checked when setting critical notes.";

        #endregion

        #region Properties

        [DropDown("Environmental", "WasteWaterSystem", "ByOperatingCenterAndTown", DependsOn = "OperatingCenter,Town", PromptText = "Select an operating center and town")]
        [EntityMap, EntityMustExist(typeof(WasteWaterSystem))]
        [View(MapCall.Common.Model.Entities.WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public int? WasteWaterSystem { get; set; }

        [Coordinate(AddressCallback = "SewerOpenings.getAddress", IconSet = IconSets.SingleDefaultIcon), EntityMap]
        [Required]
        public int? Coordinate { get; set; }

        [DropDown]
        [Required, EntityMap, EntityMustExist(typeof(AssetStatus))]
        public int? Status { get; set; }

        [DropDown]
        [EntityMap, EntityMustExist(typeof(SewerOpeningMaterial))]
        public int? SewerOpeningMaterial { get; set; }

        // DropDown done in overrides
        [ClientCallback("SewerOpenings.validateFunctionalLocation", ErrorMessage = "The Functional Location field is required.")]
        [DropDown("FieldOperations", "FunctionalLocation", "ActiveByTownIdForSewerOpeningAssetType", DependsOn = "Town", PromptText = "Select a town above")]
        public virtual int? FunctionalLocation { get; set; }

        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [EntityMap, EntityMustExist(typeof(TownSection))]
        public virtual int? TownSection { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(SewerOpeningType))]
        public virtual int? SewerOpeningType { get; set; }
        public DateTime? DateInstalled { get; set; }

        [RequiredWhen(nameof(Status), ComparisonType.EqualToAny, nameof(GetDateRetiredRequiredStatusIds), typeof(SewerOpeningViewModel), ErrorMessage = "DateRetired is required for retired / removed sewer openings.")]
        public DateTime? DateRetired { get; set; }

        [StringLength(18, MinimumLength = 18)]
        public virtual string GeoEFunctionalLocation { get; set; }

        [StringLength(SewerOpening.StringLengths.MAP_PAGE)]
        public string MapPage { get; set; }

        [StringLength(SewerOpening.StringLengths.STREET_NUMBER)]
        public string StreetNumber { get; set; }

        [StringLength(SewerOpening.StringLengths.OLD_NUMBER)]
        public string OldNumber { get; set; }

        [StringLength(SewerOpening.StringLengths.DISTANCE_FROM_CROSS_STREET)]
        public string DistanceFromCrossStreet { get; set; }

        public bool IsEpoxyCoated { get; set; }
        public int? Route { get; set; }
        public int? Stop { get; set; }
        public bool IsDoghouseOpening { get; set; }
        public decimal? DepthToInvert { get; set; }
        public decimal? RimElevation { get; set; }

        [DoesNotAutoMap]
        public bool SendToSAP { get; set; }

        [DoesNotAutoMap]
        public bool SendNotificationOnSave { get; set; }

        [CheckBox] // This isn't a nullable bool so it can be a checkbox
        public bool? Critical { get; set; }

        [ClientCallback("SewerOpenings.validateCriticalNotes", ErrorMessage = ERROR_CRITICAL_NOTES_MUST_BE_NULL)]
        [Multiline, RequiredWhen("Critical", true), StringLength(SewerOpening.StringLengths.CRITICAL_NOTES)]
        public string CriticalNotes { get; set; }

        public virtual int? InspectionFrequency { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public virtual int? InspectionFrequencyUnit { get; set; }

        [StringLength(SewerOpening.StringLengths.OUTFALL_NUMBER)]
        [RequiredWhen(nameof(SewerOpeningType), ComparisonType.EqualTo, MapCall.Common.Model.Entities.SewerOpeningType.Indices.NPDES_REGULATOR, FieldOnlyVisibleWhenRequired = true)]
        public string OutfallNumber { get; set; }

        [StringLength(SewerOpening.StringLengths.LOCATION_DESCRIPTION)]
        [RequiredWhen(nameof(SewerOpeningType), ComparisonType.EqualTo, MapCall.Common.Model.Entities.SewerOpeningType.Indices.NPDES_REGULATOR, FieldOnlyVisibleWhenRequired = true)]
        public string LocationDescription { get; set; }

        [DropDown("", "BodyOfWater", "ByOperatingCenterId", DependsOn = "OperatingCenter", DependentsRequired = DependentRequirement.None)]
        [RequiredWhen(nameof(SewerOpeningType), ComparisonType.EqualTo, MapCall.Common.Model.Entities.SewerOpeningType.Indices.NPDES_REGULATOR, FieldOnlyVisibleWhenRequired = true)]
        [EntityMap, EntityMustExist(typeof(BodyOfWater))]
        public int? BodyOfWater { get; set; }

        #endregion

        #region Constructors

        public SewerOpeningViewModel(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateCriticalNotes()
        {
            if (!Critical.GetValueOrDefault() && !string.IsNullOrWhiteSpace(CriticalNotes))
            {
                // I'd considered just nulling out CriticalNotes instead of validating it, but the user may
                // have forgotten to check the box.
                yield return new ValidationResult(ERROR_CRITICAL_NOTES_MUST_BE_NULL, new[] { "CriticalNotes" });
            }
        }

        public static int[] GetDateRetiredRequiredStatusIds() => AssetStatus.RETIRED_STATUS_IDS;

        #endregion

        #region Exposed Methods

        public override SewerOpening MapToEntity(SewerOpening entity)
        {
            var previousAssetStatus = entity.Status;
            base.MapToEntity(entity);
            SendToSAP = entity.OperatingCenter.CanSyncWithSAP;

            // if existing == Cancelled, Retired, or Removed and existing.Id == viewModel.HydrantStatusId then false
            if (Status.HasValue && previousAssetStatus?.Id == Status.Value)
            {
                if (new[] {
                    AssetStatus.Indices.CANCELLED,
                    AssetStatus.Indices.RETIRED,
                    AssetStatus.Indices.REMOVED
                }.Contains(previousAssetStatus.Id))
                {
                    SendToSAP = false;
                }
            }

            if (entity.DateRetired.HasValue && !AssetStatus.RETIRED_STATUS_IDS.Contains(entity.Status.Id))
            {
                entity.DateRetired = null;
            }

            return entity;
        }



        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateCriticalNotes());
        }

        #endregion
    }
}
