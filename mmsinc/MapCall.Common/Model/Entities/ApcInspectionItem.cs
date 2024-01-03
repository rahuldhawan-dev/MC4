using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ApcInspectionItem : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes
    {
        #region Constants

        public readonly struct DisplayNames
        {
            public const string FACILITY_AREA = "Facility Area",
                                INSPECTION_RATING = "Inspection Rating";
        }

        #endregion

        #region Properties

        #region Table Column Properties

        public virtual int Id { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string Area { get; set; }

        [Required]
        [StringLength(255)]
        public virtual string Description { get; set; }

        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime DateReported { get; set; }

        //MC-5253 DateRectified is currently hidden from user view. It will be used when Action Items for FacilitiesInspection added - 01/26/2023
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? DateRectified { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)] 
        public virtual DateTime? DateInspected { get; set; }

        public virtual int? Score { get; set; }
        public virtual string Percentage { get; set; }

        [View(DisplayNames.FACILITY_AREA)]
        public virtual IList<FacilityInspectionAreaType> FacilityInspectionAreaTypes { get; set; }

        [View(DisplayNames.INSPECTION_RATING)]
        public virtual FacilityInspectionRatingType FacilityInspectionRatingType { get; set; }

        public virtual IList<FacilityInspectionFormAnswer> FacilityInspectionFormAnswers { get; set; }

        [View(DisplayNames.FACILITY_AREA)]
        public virtual string DisplayFacilityInspectionAreaType => string.Join(", ", FacilityInspectionAreaTypes.Select(x => x.Description).Distinct());

        [DoesNotExport]
        public virtual IOrderedEnumerable<FacilityInspectionFormAnswer> GeneralWorkAreaConditions =>
            FacilityInspectionFormAnswers
               .Where(x => x.FacilityInspectionFormQuestion.Category.Id ==
                           FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS)
               .OrderBy(x =>
                    x.FacilityInspectionFormQuestion.DisplayOrder);

        [DoesNotExport]
        public virtual IOrderedEnumerable<FacilityInspectionFormAnswer> EmergencyResponseFirstAid =>
            FacilityInspectionFormAnswers
               .Where(x => x.FacilityInspectionFormQuestion.Category.Id ==
                           FacilityInspectionFormQuestionCategory.Indices.EMERGENCY_RESPONSE_FIRST_AID)
               .OrderBy(x =>
                    x.FacilityInspectionFormQuestion.DisplayOrder);

        [DoesNotExport]
        public virtual IOrderedEnumerable<FacilityInspectionFormAnswer> Security =>
            FacilityInspectionFormAnswers
               .Where(x => x.FacilityInspectionFormQuestion.Category.Id ==
                           FacilityInspectionFormQuestionCategory.Indices.SECURITY)
               .OrderBy(x =>
                    x.FacilityInspectionFormQuestion.DisplayOrder);

        [DoesNotExport]
        public virtual IOrderedEnumerable<FacilityInspectionFormAnswer> FireSafety =>
            FacilityInspectionFormAnswers
               .Where(x => x.FacilityInspectionFormQuestion.Category.Id ==
                           FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY)
               .OrderBy(x =>
                    x.FacilityInspectionFormQuestion.DisplayOrder);

        [DoesNotExport]
        public virtual IOrderedEnumerable<FacilityInspectionFormAnswer> PersonalProtectiveEquipment =>
            FacilityInspectionFormAnswers
               .Where(x => x.FacilityInspectionFormQuestion.Category.Id ==
                           FacilityInspectionFormQuestionCategory.Indices.PERSONAL_PROTECTIVE_EQUIPMENT)
               .OrderBy(x =>
                    x.FacilityInspectionFormQuestion.DisplayOrder);

        [DoesNotExport]
        public virtual IOrderedEnumerable<FacilityInspectionFormAnswer> ChemicalStorageHazCom =>
            FacilityInspectionFormAnswers
               .Where(x => x.FacilityInspectionFormQuestion.Category.Id ==
                           FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM)
               .OrderBy(x =>
                    x.FacilityInspectionFormQuestion.DisplayOrder);

        [DoesNotExport]
        public virtual IOrderedEnumerable<FacilityInspectionFormAnswer> EquipmentTools =>
            FacilityInspectionFormAnswers
               .Where(x => x.FacilityInspectionFormQuestion.Category.Id ==
                           FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS)
               .OrderBy(x =>
                    x.FacilityInspectionFormQuestion.DisplayOrder);

        [DoesNotExport]
        public virtual IOrderedEnumerable<FacilityInspectionFormAnswer> ConfinedSpace =>
            FacilityInspectionFormAnswers
               .Where(x => x.FacilityInspectionFormQuestion.Category.Id ==
                           FacilityInspectionFormQuestionCategory.Indices.CONFINED_SPACE)
               .OrderBy(x =>
                    x.FacilityInspectionFormQuestion.DisplayOrder);

        [DoesNotExport]
        public virtual IOrderedEnumerable<FacilityInspectionFormAnswer> VehicleMotorizedEquipment =>
            FacilityInspectionFormAnswers
               .Where(x => x.FacilityInspectionFormQuestion.Category.Id ==
                           FacilityInspectionFormQuestionCategory.Indices.VEHICLE_MOTORIZED_EQUIPMENT)
               .OrderBy(x =>
                    x.FacilityInspectionFormQuestion.DisplayOrder);

        [DoesNotExport]
        public virtual IOrderedEnumerable<FacilityInspectionFormAnswer> OshaTraining =>
            FacilityInspectionFormAnswers
               .Where(x => x.FacilityInspectionFormQuestion.Category.Id ==
                           FacilityInspectionFormQuestionCategory.Indices.OSHA_TRAINING)
               .OrderBy(x =>
                    x.FacilityInspectionFormQuestion.DisplayOrder);

        #endregion

        #region References

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual ApcInspectionItemType Type { get; set; }
        public virtual Employee AssignedTo { get; set; }

        public virtual IList<ApcInspectionItemNote> ItemNotes { get; set; }
        public virtual IList<ApcInspectionItemDocument> ItemDocuments { get; set; }

        #endregion

        #region Logical

        public virtual string TableName => nameof(ApcInspectionItem) + "s";

        public virtual IList<IDocumentLink> LinkedDocuments => ItemDocuments.Map(d => (IDocumentLink)d);
        public virtual IList<INoteLink> LinkedNotes => ItemNotes.Map(n => (INoteLink)n);

        #endregion

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        #endregion

        #region Constructors

        public ApcInspectionItem()
        {
            ItemNotes = new List<ApcInspectionItemNote>();
            ItemDocuments = new List<ApcInspectionItemDocument>();
            FacilityInspectionFormAnswers = new List<FacilityInspectionFormAnswer>();
            FacilityInspectionAreaTypes = new List<FacilityInspectionAreaType>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}