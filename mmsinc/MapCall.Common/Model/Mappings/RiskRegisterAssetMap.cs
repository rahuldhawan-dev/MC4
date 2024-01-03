using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RiskRegisterAssetMap : ClassMap<RiskRegisterAsset>
    {
        #region Constructors

        public RiskRegisterAssetMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            
            References(x => x.RiskRegisterAssetGroup).Not.Nullable();
            References(x => x.RiskRegisterAssetCategory).Nullable();

            References(x => x.State).Not.Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.PublicWaterSupply).Nullable();
            References(x => x.WasteWaterSystem).Nullable();
            References(x => x.Facility).Nullable();
            References(x => x.Equipment).Nullable();
            References(x => x.Coordinate).Nullable().Cascade.All();
            References(x => x.Employee).Not.Nullable();
            References(x => x.RiskRegisterAssetZone).Column("ZoneId").Nullable();

            Map(x => x.ImpactDescription).Length(RiskRegisterAsset.StringLengths.IMPACT_DESCRIPTION).Not.Nullable();
            Map(x => x.RiskDescription).Length(RiskRegisterAsset.StringLengths.RISK_DESCRIPTION).Not.Nullable();
            // RiskQuandrant is a required field with no nulls. This should not be nullable in the db.
            Map(x => x.RiskQuadrant).Nullable();
            Map(x => x.IdentifiedAt).Not.Nullable();
            Map(x => x.CofMax).Column("COFMax").Not.Nullable();
            Map(x => x.LofMax).Column("LOFMax").Not.Nullable();
            Map(x => x.TotalRiskWeighted).Not.Nullable();
            Map(x => x.RiskRegisterId).Length(RiskRegisterAsset.StringLengths.RISK_REGISTER_ID).Nullable();

            Map(x => x.InterimMitigationMeasuresTaken).Length(RiskRegisterAsset.StringLengths.INTERIM_MITIGATION_MEASURES_TAKEN).Nullable();
            Map(x => x.InterimMitigationMeasuresTakenAt).Nullable();
            Map(x => x.InterimMitigationMeasuresTakenEstimatedCosts).Nullable().Precision(19).Scale(2);

            Map(x => x.FinalMitigationMeasuresTaken).Length(RiskRegisterAsset.StringLengths.FINAL_MITIGATION_MEASURES_TAKEN).Nullable();
            Map(x => x.FinalMitigationMeasuresTakenAt).Nullable();
            Map(x => x.FinalMitigationMeasuresTakenEstimatedCosts).Nullable().Precision(19).Scale(2);

            Map(x => x.CompletionTargetDate).Nullable();
            Map(x => x.CompletionActualDate).Nullable();

            Map(x => x.IsProjectInComprehensivePlanningStudy).Not.Nullable();
            Map(x => x.IsProjectInCapitalPlan).Not.Nullable();
            Map(x => x.RelatedWorkBreakdownStructure).Length(RiskRegisterAsset.StringLengths.RELATED_WORK_BREAKDOWN_STRUCTURE).Nullable();

            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.ActionItems).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }

        #endregion
    }
}
