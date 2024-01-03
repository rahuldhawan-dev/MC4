using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ArcFlashStudy : IEntity
    {
        #region Constants

        public struct StringLengths
        {
            public const int
                ARC_FLASH_HAZARD_ANALYSIS_STUDY_PARTY = 50,
                ARC_FLASH_CONTRACTOR = 50,
                PRIMARY_FUSE_TYPE = 50,
                PRIMARY_FUSE_MANUFACTURER = 100,
                PRIORITY = 25,
                UTILITY_ACCOUNT_NUMBER = 50,
                UTILITY_COMPANY_OTHER = 50,
                UTILITY_METER_NUMBER = 25,
                UTILITY_POLE_NUMBER = 25;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual UtilityCompany UtilityCompany { get; set; }

        [DisplayName("Arc Flash Study Status")]
        public virtual ArcFlashStatus ArcFlashStatus { get; set; }

        public virtual FacilitySize FacilitySize { get; set; }
        public virtual ArcFlashAnalysisType TypeOfArcFlashAnalysis { get; set; }

        [DisplayName("Label Type")]
        public virtual ArcFlashLabelType ArcFlashLabelType { get; set; }

        public virtual UtilityTransformerKVARating TransformerKVARating { get; set; }

        [DisplayName("Secondary (Incoming Service) Voltage (V)")]
        public virtual Voltage Voltage { get; set; }

        [DisplayName("Phase")]
        public virtual PowerPhase PowerPhase { get; set; }

        [DisplayName("Facility Transformer Wiring Configuration")]
        public virtual FacilityTransformerWiringType FacilityTransformerWiringType { get; set; }

        public virtual string Priority { get; set; }

        [DisplayName("Utility Company Data Received")]
        public virtual bool PowerCompanyDataReceived { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? UtilityCompanyDataReceivedDate { get; set; }

        [DisplayName("AFHA Analysis Performed")]
        public virtual bool? AFHAAnalysisPerformed { get; set; }

        public virtual string UtilityCompanyOther { get; set; }

        public virtual string UtilityAccountNumber { get; set; }

        public virtual string UtilityMeterNumber { get; set; }

        public virtual string UtilityPoleNumber { get; set; }

        [DisplayName("Utility Primary Voltage (kV)")]
        public virtual decimal? PrimaryVoltageKV { get; set; }

        [DisplayName("Utility Transformer KVA Field Confirmed?")]
        public virtual bool TransformerKVAFieldConfirmed { get; set; }

        public virtual decimal? TransformerResistancePercentage { get; set; }

        [DisplayName("Utility Transformer Impedance Percentage")]
        public virtual decimal? TransformerReactancePercentage { get; set; }

        [DisplayName("Utility Primary Fuse Size")]
        public virtual decimal? PrimaryFuseSize { get; set; }

        [DisplayName("Utility Primary Fuse Type")]
        public virtual string PrimaryFuseType { get; set; }

        [DisplayName("Utility Primary Fuse Manufacturer")]
        public virtual string PrimaryFuseManufacturer { get; set; }

        [DisplayName("Utility Primary Fuse Available Fault Current")]
        public virtual decimal? LineToLineFaultAmps { get; set; }

        public virtual decimal? LineToLineNeutralFaultAmps { get; set; }

        public virtual string ArcFlashNotes { get; set; }

        [View(FormatStyle.Date)]
        [DisplayName("Last Date Validated")]
        public virtual DateTime? DateLabelsApplied { get; set; }

        [DisplayName("Arc Flash Site Data Collection Party")]
        public virtual string ArcFlashContractor { get; set; }

        public virtual string ArcFlashHazardAnalysisStudyParty { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        [DisplayName("Cost to Complete the Study")]
        public virtual decimal? CostToComplete { get; set; }

        public virtual bool? ExpiringWithinAYear { get; set; }

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        #endregion
    }

    [Serializable]
    public class MostRecentArcFlashStudy : IEntity
    {
        public virtual int Id { get; set; }
        public virtual Facility Facility { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DateLabelsApplied { get; set; }

        public virtual bool ExpiringWithinAYear { get; set; }
    }
}
