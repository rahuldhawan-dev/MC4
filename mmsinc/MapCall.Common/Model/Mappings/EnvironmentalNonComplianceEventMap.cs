using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EnvironmentalNonComplianceEventMap : ClassMap<EnvironmentalNonComplianceEvent>
    {
        public EnvironmentalNonComplianceEventMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.State).Not.Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.PublicWaterSupply).Column("SystemId").Nullable();
            References(x => x.WasteWaterSystem).Nullable();
            References(x => x.Facility).Nullable();
            References(x => x.IssueType).Column("TypeId").Not.Nullable();
            References(x => x.IssueSubType).Column("SubTypeId").Nullable();
            References(x => x.Responsibility).Not.Nullable();
            References(x => x.FailureType).Nullable();
            References(x => x.IssueStatus).Column("StatusId").Not.Nullable();
            References(x => x.IssuingEntity).Nullable();
            References(x => x.CreatedBy).Column("CreatedById").Nullable().Not.Update();
            References(x => x.CountsAgainstTarget).Column("CountsAgainstTarget").Nullable();
            
            Map(x => x.AwarenessDate).Not.Nullable();
            Map(x => x.EventDate).Not.Nullable();
            Map(x => x.DateFinalized).Nullable();
            Map(x => x.DateOfEnvironmentalLeadershipTeamReview).Nullable();
            Map(x => x.EnforcementDate).Nullable();
            Map(x => x.NameOfThirdParty).Length(EnvironmentalNonComplianceEvent.StringLengths.RESPONSIBILITY_NAME)
                                        .Nullable();
            Map(x => x.SummaryOfEvent).Length(int.MaxValue).Not.Nullable();
            Map(x => x.FailureTypeDescription)
               .Length(EnvironmentalNonComplianceEvent.StringLengths.FAILURE_TYPE_DESCRIPTION)
               .Column("FailureTypeDescription").Nullable();
            Map(x => x.FineAmount).Nullable();
            Map(x => x.NameOfEntity).Length(EnvironmentalNonComplianceEvent.StringLengths.ISSUING_ENTITY_NAME)
                                    .Nullable();
            Map(x => x.IssueYear).Nullable();
            
            Map(x => x.NOVWorkGroupReviewDate).Nullable();
            Map(x => x.ChiefEnvOfficerApprovalDate).Nullable();
            Map(x => x.CreatedAt).Not.Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.ActionItems)
               .KeyColumn("EnvironmentalNonComplianceEventId").Inverse().Cascade.AllDeleteOrphan();
            HasManyToMany(x => x.RootCauses)
               .Table("EnvironmentalNonComplianceEventsEnvironmentalNonComplianceEventRootCauses")
               .ParentKeyColumn("EnvironmentalNonComplianceEventId")
               .ChildKeyColumn("EnvironmentalNonComplianceEventRootCauseId")
               .LazyLoad()
               .Cascade.None();
        }
    }
}
