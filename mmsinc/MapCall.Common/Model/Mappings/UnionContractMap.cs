using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class UnionContractMap : ClassMap<UnionContract>
    {
        public UnionContractMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OperatingCenter);
            References(x => x.Local);

            Map(x => x.StartDate);
            Map(x => x.EndDate);
            Map(x => x.PercentIncreaseYr1).Precision(53);
            Map(x => x.PercentIncreaseYr2).Precision(53);
            Map(x => x.PercentIncreaseYr3).Precision(53);
            Map(x => x.PercentIncreaseYr4).Precision(53);
            Map(x => x.PercentIncreaseYr5).Precision(53);
            Map(x => x.PercentIncreaseYr6).Precision(53);
            Map(x => x.NewContractExpirationDate);
            Map(x => x.NewContractEffectiveDate);
            Map(x => x.TermOfContract).Length(50);
            Map(x => x.DateOfMoa).Column("DateOfMOA");
            Map(x => x.CompanyNegotiatingCommittee);
            Map(x => x.UnionNegotiatingCommittee);
            Map(x => x.ContractExtended);
            Map(x => x.ContractExtensionDate);
            Map(x => x.CompanyKeyObjectivesSummary);
            Map(x => x.RatificationVoteFor).Precision(53);
            Map(x => x.RatificationVoteAgainst).Precision(53);
            Map(x => x.TotalBargainingUnitMembers).Precision(53);
            Map(x => x.Notes);
            Map(x => x.Retroactivity);

            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.UnionContractNotes).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Grievances)
               .KeyColumn(FixTableAndColumnNamesForBug1623.NewColumnNames.Grievances.CONTRACT_ID);
            HasMany(x => x.Proposals)
               .KeyColumn(FixUnionContractProposalsTableAndColumnNamesForBug1702.NewColumnNames.CONTRACT_ID);
        }
    }
}
