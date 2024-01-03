using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;
using System;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class EndOfPipeExceedanceViewModel : ViewModel<EndOfPipeExceedance>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [Required]
        public virtual int? State { get; set; }
        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [Required]
        public virtual int? OperatingCenter { get; set; }
        [DropDown("Environmental", "WasteWaterSystem", "ByOperatingCenter", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(WasteWaterSystem))]
        [Required]
        public virtual int? WasteWaterSystem { get; set; }
        [DropDown("", "Facility", "GetByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(Facility))]
        public virtual int? Facility { get; set; }
        [Required]
        public virtual DateTime? EventDate { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(LimitationType))]
        [Required]
        public virtual int? LimitationType { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(EndOfPipeExceedanceType))]
        [Required]
        public virtual int? EndOfPipeExceedanceType { get; set; }
        [RequiredWhen("EndOfPipeExceedanceType", ComparisonType.EqualTo, MapCall.Common.Model.Entities.EndOfPipeExceedanceType.Indices.OTHER, FieldOnlyVisibleWhenRequired = true)]
        [StringLength(EndOfPipeExceedance.StringLengths.EndOfPipeExceedanceTypeOtherReason)]
        public virtual string EndOfPipeExceedanceTypeOtherReason { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(EndOfPipeExceedanceRootCause))]
        [Required]
        public virtual int? EndOfPipeExceedanceRootCause { get; set; }
        [RequiredWhen("EndOfPipeExceedanceRootCause", ComparisonType.EqualTo, MapCall.Common.Model.Entities.EndOfPipeExceedanceRootCause.Indices.OTHER, FieldOnlyVisibleWhenRequired = true)]
        [StringLength(EndOfPipeExceedance.StringLengths.EndOfPipeExceedanceRootCauseOtherReason)]
        public virtual string EndOfPipeExceedanceRootCauseOtherReason { get; set; }
        [Required]
        public virtual bool? ConsentOrder { get; set; }
        [Required]
        public virtual bool? NewAcquisition { get; set; }
        [Required]
        [Multiline]
        public virtual string BriefDescription { get; set; }

        #endregion

        #region Constructors

        public EndOfPipeExceedanceViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateEndOfPipeExceedance : EndOfPipeExceedanceViewModel
    {
        #region Properties

        [DropDown("", "OperatingCenter", "ActiveByStateIdOrAll", DependsOn = nameof(State))]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter; 
            set => base.OperatingCenter = value;
        }

        #endregion

        public CreateEndOfPipeExceedance(IContainer container) : base(container) { }
    }

    public class EditEndOfPipeExceedance : EndOfPipeExceedanceViewModel
    {
        public EditEndOfPipeExceedance(IContainer container) : base(container) { }
    }

    public class SearchEndOfPipeExceedance : SearchSet<EndOfPipeExceedance>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public virtual int? State { get; set; }
        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }
        [DropDown("Environmental", "WasteWaterSystem", "ByOperatingCenter", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(WasteWaterSystem))]
        [View(DisplayName = MapCall.Common.Model.Entities.WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public virtual int? WasteWaterSystem { get; set; }
        [DropDown("", "Facility", "GetByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(Facility))]
        public virtual int? Facility { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(EndOfPipeExceedanceType))]
        public virtual int? EndOfPipeExceedanceType { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(EndOfPipeExceedanceRootCause))]
        public virtual int? EndOfPipeExceedanceRootCause { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(LimitationType))]
        public virtual int? LimitationType { get; set; }
        public virtual DateRange EventDate { get; set; }
        [View(Description = EndOfPipeExceedance.CONSENT_ORDER_DESCRIPTION)]
        public virtual bool? ConsentOrder { get; set; }
        [View(Description = EndOfPipeExceedance.NEW_ACQUISITION_DESCRIPTION)]
        public virtual bool? NewAcquisition { get; set; }
    }
}
