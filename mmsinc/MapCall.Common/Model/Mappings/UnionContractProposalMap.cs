using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class UnionContractProposalMap : ClassMap<UnionContractProposal>
    {
        public UnionContractProposalMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.ManagementOrUnion).Column("ManagementorUnionId");
            References(x => x.Contract)
               .Column(FixUnionContractProposalsTableAndColumnNamesForBug1702.NewColumnNames.CONTRACT_ID);
            References(x => x.Prioritization)
               .Column(NormalizeOutUnionContractProposalLookupsForBug1702
                      .NewColumnNames.UnionContractProposals.PRIORITIZATION_ID);
            References(x => x.NegotiationTiming)
               .Column(NormalizeOutUnionContractProposalLookupsForBug1702
                      .NewColumnNames.UnionContractProposals.NEGOTIATION_TIMING_ID);
            References(x => x.Grouping)
               .Column(NormalizeOutUnionContractProposalLookupsForBug1702
                      .NewColumnNames.UnionContractProposals.GROUPING_ID);
            References(x => x.PrintingSequence)
               .Column(NormalizeOutUnionContractProposalLookupsForBug1702
                      .NewColumnNames.UnionContractProposals.PRINTING_SEQUENCE_ID);
            References(x => x.Status)
               .Column(NormalizeOutUnionContractProposalLookupsForBug1702
                      .NewColumnNames.UnionContractProposals.STATUS_ID);
            References(x => x.AffectedDepartment)
               .Column(NormalizeOutUnionContractProposalLookupsForBug1702
                      .NewColumnNames.UnionContractProposals.AFFECTED_DEPARTMENT_ID);
            References(x => x.PrimaryDriverForProposal, "PrimaryDriversForProposalId");

            References(x => x.OperatingCenter)
               .Formula("(Select c.OperatingCenterId FROM UnionContracts c WHERE c.Id = ContractId)").ReadOnly();

            Map(x => x.NegotiationStrategy);
            Map(x => x.Flag).Not.Nullable();
            Map(x => x.StrikeProposal).Not.Nullable();
            Map(x => x.Reviewed).Not.Nullable();
            Map(x => x.CrossReferenceNumber).Length(25);
            Map(x => x.ProposalClosedDate);
            Map(x => x.ProposalDescription);
            Map(x => x.TargetValueOfChange).Precision(18).Scale(2);
            Map(x => x.ValuationAssumptions);
            Map(x => x.ImpactOfChange);
            Map(x => x.ToAchieveBenefitOfChange).Length(255);
            Map(x => x.ImpactOnHealthSafety);
            Map(x => x.ImpactOnManagementsRights);
            Map(x => x.ImpactOnOperationalEfficiency);
            Map(x => x.ImpactOnOvertime);
            Map(x => x.CurrentPageNumber).Precision(10);
            Map(x => x.AnticipatedResponseFromOppositeSide);
            Map(x => x.Notes);
            Map(x => x.Sme).Column("SME").Length(50);
            Map(x => x.ImpactOnAttendance);
            Map(x => x.ImpactOnCustomerService);
            Map(x => x.ImpactOnEconomics);
            Map(x => x.ImpactOnBenefits);
            Map(x => x.ImpactOnStaffingLevels);
            Map(x => x.ImpactOnRegulatoryCompliance);
            Map(x => x.TalkingPoints);
            Map(x => x.ImplementationItems);
            Map(x => x.CostModelNeeded);

            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.UnionContractProposalNotes).KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
