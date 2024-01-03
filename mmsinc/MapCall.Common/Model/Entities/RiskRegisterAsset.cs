using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RiskRegisterAsset : IThingWithActionItems, IThingWithNotes, IThingWithDocuments, IThingWithCoordinate, IThingWithState
    {
        #region Constants

        public readonly struct StringLengths
        {
            public const int 
                IMPACT_DESCRIPTION = 255,
                RISK_DESCRIPTION = 255,
                INTERIM_MITIGATION_MEASURES_TAKEN = 150,
                FINAL_MITIGATION_MEASURES_TAKEN = 255,
                RELATED_WORK_BREAKDOWN_STRUCTURE = 25,
                RISK_REGISTER_ID = 25;
        }

        public readonly struct Ranges
        {
            public const int RISK_QUADRANT_LOW = 1,
                             RISK_QUADRANT_HIGH = 25;
        }

        public readonly struct ViewDisplayNames
        {
            public const string
                GROUP = "Asset",
                IMPACT = "COF Max",
                CATEGORY = "Threat/Asset Pair Category",
                PROBABILITY = "LOF Max",
                COORDINATE = "Location",
                EMPLOYEE = "Risk Project Owner",
                IMPACT_DESCRIPTION = "COF Weighted",
                RISK_DESCRIPTION = "Description of Risk",
                IDENTIFIED_AT = "Date Risk Identified",
                INTERIM_MITIGATION_MEASURES_TAKEN = "Description",
                INTERIM_MITIGATION_MEASURES_TAKEN_ESTIMATED_COSTS = "Costs",
                INTERIM_MITIGATION_MEASURES_TAKEN_AT = "Date",
                FINAL_MITIGATION_MEASURES_TAKEN = "Description",
                FINAL_MITIGATION_MEASURES_TAKEN_ESTIMATED_COSTS = "Costs",
                FINAL_MITIGATION_MEASURES_TAKEN_AT = "Date",
                COMPLETION_TARGET_DATE = "Target Completion Date",
                COMPLETION_ACTUAL_DATE = "Actual Completion Date",
                IS_PROJECT_IN_COMPREHENSIVE_PLANNING_STUDY = "Mitigation Project in CPS",
                IS_PROJECT_IN_CAPITAL_PLAN = "Mitigation Project in Capital Plan",
                RELATED_WORK_BREAKDOWN_STRUCTURE = "Related WBS",
                RISK_QUADRANT = "Risk Max",
                RISK_REGISTER_ID = "Risk Register Id",
                RISK_REGISTER_ZONE = "Zone",
                WASTEWATER_SYSTEM = "Wastewater System";
        }

        #endregion

        #region Table Properties

        [DoesNotExport]
        public virtual string TableName => nameof(RiskRegisterAsset) + "s";

        public virtual int Id { get; set; }
        
        [View(ViewDisplayNames.GROUP)]
        public virtual RiskRegisterAssetGroup RiskRegisterAssetGroup { get; set; }

        [View(ViewDisplayNames.IMPACT)]
        public virtual int CofMax { get; set; } 

        [View(ViewDisplayNames.CATEGORY)]
        public virtual RiskRegisterAssetCategory RiskRegisterAssetCategory { get; set; }

        [View(ViewDisplayNames.PROBABILITY)]
        public virtual int LofMax { get; set; } 

        public virtual State State { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }

        public virtual PublicWaterSupply PublicWaterSupply { get; set; }

        [View(WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public virtual WasteWaterSystem WasteWaterSystem { get; set; }

        public virtual Facility Facility { get; set; }

        public virtual Equipment Equipment { get; set; }

        [View(ViewDisplayNames.COORDINATE)]
        public virtual Coordinate Coordinate { get; set; }

        public virtual MapIcon Icon => Coordinate?.Icon;

        public virtual decimal? Latitude => Coordinate?.Latitude;

        public virtual decimal? Longitude => Coordinate?.Longitude;

        [View(ViewDisplayNames.EMPLOYEE)]
        public virtual Employee Employee { get; set; }

        [View(ViewDisplayNames.IMPACT_DESCRIPTION)]
        public virtual string ImpactDescription { get; set; }

        [View(ViewDisplayNames.RISK_DESCRIPTION)]
        public virtual string RiskDescription { get; set; }

        [View(ViewDisplayNames.RISK_QUADRANT)]
        public virtual int? RiskQuadrant { get; set; }

        [View(ViewDisplayNames.IDENTIFIED_AT, FormatStyle.Date)]
        public virtual DateTime IdentifiedAt { get; set; }

        [ExcelExportColumn(UsePropertyName = true),
         View(ViewDisplayNames.INTERIM_MITIGATION_MEASURES_TAKEN)]
        public virtual string InterimMitigationMeasuresTaken { get; set; }

        [ExcelExportColumn(UsePropertyName = true),
         View(ViewDisplayNames.INTERIM_MITIGATION_MEASURES_TAKEN_ESTIMATED_COSTS, FormatStyle.Currency)]
        public virtual decimal? InterimMitigationMeasuresTakenEstimatedCosts { get; set; }

        [ExcelExportColumn(UsePropertyName = true),
         View(ViewDisplayNames.INTERIM_MITIGATION_MEASURES_TAKEN_AT, FormatStyle.Date)]
        public virtual DateTime? InterimMitigationMeasuresTakenAt { get; set; }

        [ExcelExportColumn(UsePropertyName = true),
         View(ViewDisplayNames.FINAL_MITIGATION_MEASURES_TAKEN)]
        public virtual string FinalMitigationMeasuresTaken { get; set; }

        [ExcelExportColumn(UsePropertyName = true),
         View(ViewDisplayNames.FINAL_MITIGATION_MEASURES_TAKEN_ESTIMATED_COSTS, FormatStyle.Currency)]
        public virtual decimal? FinalMitigationMeasuresTakenEstimatedCosts { get; set; }

        [ExcelExportColumn(UsePropertyName = true),
         View(ViewDisplayNames.FINAL_MITIGATION_MEASURES_TAKEN_AT, FormatStyle.Date)]
        public virtual DateTime? FinalMitigationMeasuresTakenAt { get; set; }

        [View(ViewDisplayNames.COMPLETION_TARGET_DATE)]
        public virtual DateTime? CompletionTargetDate { get; set; }

        [View(ViewDisplayNames.COMPLETION_ACTUAL_DATE)]
        public virtual DateTime? CompletionActualDate { get; set; }

        [View(ViewDisplayNames.IS_PROJECT_IN_COMPREHENSIVE_PLANNING_STUDY)]
        public virtual bool IsProjectInComprehensivePlanningStudy { get; set; }

        [View(ViewDisplayNames.IS_PROJECT_IN_CAPITAL_PLAN)]
        public virtual bool IsProjectInCapitalPlan { get; set; }

        [View(ViewDisplayNames.RELATED_WORK_BREAKDOWN_STRUCTURE)]
        public virtual string RelatedWorkBreakdownStructure { get; set; }

        public virtual int TotalRiskWeighted { get; set; }

        [MaxLength(StringLengths.RISK_REGISTER_ID),
         View(ViewDisplayNames.RISK_REGISTER_ID)]
        public virtual string RiskRegisterId { get; set; }

        [View(ViewDisplayNames.RISK_REGISTER_ZONE)]
        public virtual RiskRegisterAssetZone RiskRegisterAssetZone { get; set; }

        public virtual IList<Note<RiskRegisterAsset>> Notes { get; set; }

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public virtual IList<Document<RiskRegisterAsset>> Documents { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<ActionItem<RiskRegisterAsset>> ActionItems { get; set; }

        public virtual IList<IActionItemLink> LinkedActionItems => ActionItems.Cast<IActionItemLink>().ToList();

        #endregion

        #region Logical Properties

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        #endregion

        #region Constructors

        public RiskRegisterAsset()
        {
            ActionItems = new List<ActionItem<RiskRegisterAsset>>();
            Documents = new List<Document<RiskRegisterAsset>>();
            Notes = new List<Note<RiskRegisterAsset>>();
        }

        #endregion
    }
}
