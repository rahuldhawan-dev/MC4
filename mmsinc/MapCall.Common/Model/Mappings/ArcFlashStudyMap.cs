using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ArcFlashStudyMap : ClassMap<ArcFlashStudy>
    {
        public const string TABLE_NAME = "ArcFlashStudies",
                            EXPIRING_WITHIN_A_YEAR_SQL =
                                "(CASE WHEN (DateLabelsApplied is not null AND (datediff(day, GetDate(), dateadd(year, 5, DateLabelsApplied))) < 365) THEN 1 ELSE 0 END)",
                            EXPIRING_WITHIN_A_YEAR_SQLITE =
                                "(CASE WHEN (DateLabelsApplied is not null AND (JulianDay(date(DateLabelsApplied,'+5 years')) - JulianDay(date('now')) < 365)) THEN 1 ELSE 0 END)";

        public ArcFlashStudyMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id).GeneratedBy.Identity();

            LazyLoad();

            References(x => x.Facility).Not.Nullable();
            References(x => x.UtilityCompany).Nullable();
            References(x => x.ArcFlashStatus).Nullable();
            References(x => x.PowerPhase).Nullable();
            References(x => x.Voltage).Nullable();
            References(x => x.TransformerKVARating, "UtilityTransformerKVARatingId").Nullable();
            References(x => x.FacilitySize).Nullable();
            References(x => x.FacilityTransformerWiringType).Nullable();
            References(x => x.TypeOfArcFlashAnalysis).Nullable();
            References(x => x.ArcFlashLabelType).Nullable();

            Map(x => x.PowerCompanyDataReceived).Not.Nullable();
            Map(x => x.UtilityCompanyDataReceivedDate).Nullable();
            Map(x => x.AFHAAnalysisPerformed).Nullable();
            Map(x => x.TransformerKVAFieldConfirmed).Not.Nullable();
            Map(x => x.DateLabelsApplied).Nullable();
            Map(x => x.ArcFlashContractor).Nullable().Length(ArcFlashStudy.StringLengths.ARC_FLASH_CONTRACTOR);
            Map(x => x.ArcFlashHazardAnalysisStudyParty)
               .Nullable().Length(ArcFlashStudy.StringLengths.ARC_FLASH_HAZARD_ANALYSIS_STUDY_PARTY);
            Map(x => x.CostToComplete).Nullable();
            Map(x => x.Priority).Length(ArcFlashStudy.StringLengths.PRIORITY).Nullable();
            Map(x => x.UtilityCompanyOther).Length(ArcFlashStudy.StringLengths.UTILITY_COMPANY_OTHER).Nullable();
            Map(x => x.UtilityAccountNumber).Length(ArcFlashStudy.StringLengths.UTILITY_ACCOUNT_NUMBER).Nullable();
            Map(x => x.UtilityMeterNumber).Length(ArcFlashStudy.StringLengths.UTILITY_METER_NUMBER).Nullable();
            Map(x => x.UtilityPoleNumber).Length(ArcFlashStudy.StringLengths.UTILITY_POLE_NUMBER).Nullable();
            Map(x => x.PrimaryVoltageKV).Precision(18).Scale(2).Nullable();
            Map(x => x.TransformerResistancePercentage).Precision(18).Scale(2).Nullable();
            Map(x => x.TransformerReactancePercentage).Precision(18).Scale(2).Nullable();
            Map(x => x.PrimaryFuseSize).Precision(18).Scale(2).Nullable();
            Map(x => x.PrimaryFuseType).Length(Facility.StringLengths.PRIMARY_FUSE_TYPE).Nullable();
            Map(x => x.PrimaryFuseManufacturer).Length(Facility.StringLengths.PRIMARY_FUSE_MANUFACTURER).Nullable();
            Map(x => x.LineToLineFaultAmps).Precision(18).Scale(2).Nullable();
            Map(x => x.LineToLineNeutralFaultAmps).Precision(18).Scale(2).Nullable();
            Map(x => x.ArcFlashNotes).Length(int.MaxValue).Nullable();

            Map(x => x.ExpiringWithinAYear)
               .DbSpecificFormula(EXPIRING_WITHIN_A_YEAR_SQL, EXPIRING_WITHIN_A_YEAR_SQLITE);
        }
    }
}
