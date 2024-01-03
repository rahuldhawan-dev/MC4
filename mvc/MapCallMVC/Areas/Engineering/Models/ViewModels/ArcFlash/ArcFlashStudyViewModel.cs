using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Engineering.Models.ViewModels.ArcFlash
{
    public class ArcFlashStudyViewModel : ViewModel<ArcFlashStudy>
    {
        #region Properties

        #region Entities

        [DropDown("Production", "UtilityCompany", "ByStateId", DependsOn = "State"), EntityMap, EntityMustExist(typeof(UtilityCompany))]
        public int? UtilityCompany { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ArcFlashStatus))]
        public int? ArcFlashStatus { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PowerPhase))]
        [RequiredWhen("ArcFlashLabelType", ComparisonType.NotEqualTo, MapCall.Common.Model.Entities.ArcFlashLabelType.Indices.STANDARDLABEL)]
        public int? PowerPhase { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Voltage))]
        [RequiredWhen("ArcFlashLabelType", ComparisonType.NotEqualTo, MapCall.Common.Model.Entities.ArcFlashLabelType.Indices.STANDARDLABEL)]
        public int? Voltage { get; set; }

        [DropDown("Facilities", "UtilityTransformerKVARating", "ByVoltage", DependsOn = nameof(Voltage), PromptText = "Please select an (Incoming Voltage)")]
        [EntityMap, EntityMustExist(typeof(UtilityTransformerKVARating))]
        [RequiredWhen("ArcFlashLabelType", ComparisonType.NotEqualTo, MapCall.Common.Model.Entities.ArcFlashLabelType.Indices.STANDARDLABEL)]
        public int? TransformerKVARating { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(FacilitySize))]
        public int? FacilitySize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(FacilityTransformerWiringType))]
        public int? FacilityTransformerWiringType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ArcFlashAnalysisType))]
        public int? TypeOfArcFlashAnalysis { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ArcFlashLabelType))]
        public int? ArcFlashLabelType { get; set; }

        #endregion

        #region Regular Properties

        [RequiredWhen("ArcFlashLabelType",ComparisonType.NotEqualTo, MapCall.Common.Model.Entities.ArcFlashLabelType.Indices.STANDARDLABEL)]
        public bool? PowerCompanyDataReceived { get; set; }

        [RequiredWhen("PowerCompanyDataReceived", ComparisonType.EqualTo, true)]
        public DateTime? UtilityCompanyDataReceivedDate { get; set; }

        public bool? AFHAAnalysisPerformed { get; set; }

        [RequiredWhen("ArcFlashStatus", ComparisonType.EqualTo, MapCall.Common.Model.Entities.ArcFlashStatus.Indices.COMPLETED)]
        public bool? TransformerKVAFieldConfirmed { get; set; }

        public DateTime? DateLabelsApplied { get; set; }

        [RequiredWhen("ArcFlashStatus", ComparisonType.EqualTo, MapCall.Common.Model.Entities.ArcFlashStatus.Indices.COMPLETED)]
        [StringLength(Facility.StringLengths.ARC_FLASH_CONTRACTOR)]
        public string ArcFlashContractor { get; set; }

        public string ArcFlashHazardAnalysisStudyParty { get; set; }

        [RequiredWhen("ArcFlashStatus", ComparisonType.EqualTo, MapCall.Common.Model.Entities.ArcFlashStatus.Indices.COMPLETED)]
        public decimal? CostToComplete { get; set; }

        [StringLength(ArcFlashStudy.StringLengths.PRIORITY)]
        public string Priority { get; set; }

        [StringLength(ArcFlashStudy.StringLengths.UTILITY_COMPANY_OTHER)]
        public string UtilityCompanyOther { get; set; }

        [StringLength(ArcFlashStudy.StringLengths.UTILITY_ACCOUNT_NUMBER)]
        public string UtilityAccountNumber { get; set; }

        [StringLength(ArcFlashStudy.StringLengths.UTILITY_METER_NUMBER)]
        public string UtilityMeterNumber { get; set; }

        [StringLength(ArcFlashStudy.StringLengths.UTILITY_POLE_NUMBER)]
        public string UtilityPoleNumber { get; set; }

        public decimal? PrimaryVoltageKV { get; set; }

        public decimal? TransformerResistancePercentage { get; set; }

        public decimal? TransformerReactancePercentage { get; set; }

        public decimal? PrimaryFuseSize { get; set; }

        [StringLength(ArcFlashStudy.StringLengths.PRIMARY_FUSE_TYPE)]
        public string PrimaryFuseType { get; set; }

        [StringLength(ArcFlashStudy.StringLengths.PRIMARY_FUSE_MANUFACTURER)]
        public string PrimaryFuseManufacturer { get; set; }

        public decimal? LineToLineFaultAmps { get; set; }

        public decimal? LineToLineNeutralFaultAmps { get; set; }

        public string ArcFlashNotes { get; set; }

        #endregion

        #endregion

        #region Constructors

        public ArcFlashStudyViewModel(IContainer container) : base(container) { }

        #endregion
    }

}
