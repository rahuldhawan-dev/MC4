using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RecurringProjectMap : ClassMap<RecurringProject>
    {
        #region Constants

        public const string HAS_MAINS_SELECTED =
                                "(CASE WHEN ((SELECT COUNT(1) FROM RecurringProjectMains RPM WHERE RPM.RecurringProjectId = Id) > 0) THEN 1 ELSE 0 END)",
                            REQUIRES_SCORING = "CASE WHEN ((" +
                                               "(SELECT SUM(pdlv.VariableScore) FROM RecurringProjectsPipeDataLookupValues rpppdlv join PipeDataLookupValues pdlv on pdlv.Id = rpppdlv.PipeDataLookupValueID where rpppdlv.RecurringProjectID = Id) " +
                                               "/ " +
                                               "(SELECT Count(pdlv.VariableScore) FROM RecurringProjectsPipeDataLookupValues rpppdlv join PipeDataLookupValues pdlv on pdlv.Id = rpppdlv.PipeDataLookupValueID where rpppdlv.RecurringProjectID = Id AND pdlv.VariableScore > 0))" +
                                               " < 16) THEN 1 ELSE 0 END";

        #endregion

        #region Constructors

        public RecurringProjectMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Town).Nullable();
            References(x => x.RecurringProjectType).Column("ProjectTypeID").Nullable();
            References(x => x.ProposedDiameter).Nullable();
            References(x => x.ProposedPipeMaterial).Nullable();
            References(x => x.AcceleratedAssetInvestmentCategory).Nullable();
            References(x => x.SecondaryAssetInvestmentCategory).Nullable();
            References(x => x.Status).Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.FoundationalFilingPeriod).Nullable();
            References(x => x.AssetCategory).Not.Nullable();
            References(x => x.AssetType).Not.Nullable();
            References(x => x.RegulatoryStatus, "RecurringProjectRegulatoryStatusId").Nullable();
            References(x => x.CreatedBy).Not.Nullable();
            References(x => x.OverrideInfoMasterReason);
            References(x => x.CorrectDiameter).Nullable();
            References(x => x.CorrectMaterial).Nullable();

            References(x => x.Icon).Formula("(CASE WHEN StatusId = 2 THEN 30 " +
                                            " WHEN StatusId = 3 THEN 28" +
                                            " ELSE 29 END)");

            Map(x => x.ProjectTitle).Not.Nullable();
            Map(x => x.ProjectDescription);
            Map(x => x.District).Nullable();
            Map(x => x.OriginationYear);
            Map(x => x.HistoricProjectID);
            Map(x => x.NJAWEstimate).Not.Nullable();
            Map(x => x.ProposedLength);
            Map(x => x.Justification);
            Map(x => x.EstimatedProjectDuration);
            Map(x => x.EstimatedInServiceDate);
            Map(x => x.ActualInServiceDate);
            Map(x => x.TotalInfoMasterScore).Nullable();
            Map(x => x.FinalCriteriaScore);
            Map(x => x.FinalRawScore);
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.WBSNumber).Nullable();
            Map(x => x.OverrideInfoMasterDecision).Nullable();
            Map(x => x.OverrideInfoMasterJustification).Nullable();
            Map(x => x.CPSReferenceId).Nullable();
            Map(x => x.DecadeInstalledOverride).Nullable();
            Map(x => x.ExistingPipeMaterialOverride).Nullable();
            Map(x => x.ExistingDiameterOverride).Nullable();
            Map(x => x.CorrectInstallationDate).Nullable();

            HasManyToMany(x => x.HighCostFactors)
               .Table("RecurringProjectsHighCostFactors")
               .ParentKeyColumn("RecurringProjectId")
               .ChildKeyColumn("HighCostFactorId");
            HasManyToMany(x => x.PipeDataLookupValues)
               .Table("RecurringProjectsPipeDataLookupValues")
               .ParentKeyColumn("RecurringProjectID")
               .ChildKeyColumn("PipeDataLookupValueID")
               .Cascade.All();
            HasManyToMany(x => x.GISDataInaccuracies)
               .Table("RecurringProjectsGISDataInaccuracies")
               .ParentKeyColumn("RecurringProjectID")
               .ChildKeyColumn("GISDataInaccuracyTypeId")
               .Cascade.All();
            HasManyToMany(x => x.MainBreakOrders)
               .Table("RecurringProjectsMainBreakOrders")
               .ParentKeyColumn("RecurringProjectId")
               .ChildKeyColumn("WorkOrderId")
               .Cascade.All();

            HasMany(x => x.ProjectEndorsements)
               .KeyColumn("RecurringProjectID")
               .Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.RecurringProjectsPipeDataLookupValues)
               .KeyColumn("RecurringProjectId")
               .Cascade.AllDeleteOrphan();

            HasMany(x => x.RecurringProjectMains).KeyColumn("RecurringPRojectId").Cascade.AllDeleteOrphan().Inverse();

            Map(x => x.RequiresScoring).Formula(REQUIRES_SCORING).Not.Update().Not.Insert();

            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();

            Map(x => x.HasMainsSelected).Formula(HAS_MAINS_SELECTED).Not.Update().Not.Insert();
        }

        #endregion
    }
}
