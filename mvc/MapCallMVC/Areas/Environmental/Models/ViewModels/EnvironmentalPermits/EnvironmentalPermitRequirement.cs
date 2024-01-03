using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class EditEnvironmentalPermitRequirement : ViewModel<EnvironmentalPermitRequirement>
    {
        #region Properties

        [EntityMap]
        public int? EnvironmentalPermit { get; set; }

        [DropDown, Required, EntityMap]
        public int? RequirementType { get; set; }

        [Required]
        [StringLength(EnvironmentalPermitRequirement.StringLengths.REQUIREMENT)]
        public string Requirement { get; set; }

        [DropDown, Required, EntityMap]
        public int? ValueUnit { get; set; }

        [DropDown, Required, EntityMap]
        public int? ValueDefinition { get; set; }

        [DropDown, Required, EntityMap]
        public int? TrackingFrequency { get; set; }

        [DropDown, Required, EntityMap]
        public int? ReportingFrequency { get; set; }

        [DataType(DataType.MultilineText)]
        public string ReportingFrequencyDetails { get; set; }

        [DropDown, EntityMap]
        public int? ProcessOwner { get; set; }

        [DropDown, Required, EntityMap]
        public int? ReportingOwner { get; set; }

        [StringLength(EnvironmentalPermitRequirement.StringLengths.REPORT_DATA_STORAGE_LOCATION)]
        public string ReportDataStorageLocation { get; set; }

        [StringLength(EnvironmentalPermitRequirement.StringLengths.REPORT_CREATION_INSTRUCTIONS)]
        public string ReportCreationInstructions { get; set; }

        [DataType(DataType.MultilineText)]
        [RequiredWhen("RequiresRequirements", ComparisonType.EqualTo, true)]
        public string ReportSendTo { get; set; }

        [DropDown, RequiredWhen("ReportSendTo", ComparisonType.NotEqualTo, ""), EntityMap, EntityMustExist(typeof(CommunicationType))]
        public int? CommunicationType { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [RequiredWhen("CommunicationType", ComparisonType.EqualTo, MapCall.Common.Model.Entities.CommunicationType.Indices.EMAIL)]
        [StringLength(EnvironmentalPermitRequirement.StringLengths.COMMUNICATION_EMAIL)]
        public string CommunicationEmail { get; set; }

        [RequiredWhen("CommunicationType", ComparisonType.EqualTo, MapCall.Common.Model.Entities.CommunicationType.Indices.AGENCY_SUBMITTAL_FORM)]
        [StringLength(EnvironmentalPermitRequirement.StringLengths.COMMUNICATION_LINK)]
        public string CommunicationLink { get; set; }

        [DoesNotAutoMap]
        public virtual bool? RequiresRequirements { get; set; }

        #endregion

        #region Constructors

        public EditEnvironmentalPermitRequirement(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override void Map(EnvironmentalPermitRequirement entity)
        {
            base.Map(entity);
            RequiresRequirements = entity.EnvironmentalPermit?.RequiresRequirements;
        }

        #endregion
    }
}