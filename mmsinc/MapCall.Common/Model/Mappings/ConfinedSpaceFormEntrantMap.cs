using System.Web.UI;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ConfinedSpaceFormEntrantMap : ClassMap<ConfinedSpaceFormEntrant>
    {
        public const string TABLE_NAME = "ConfinedSpaceFormEntrants";

        public ConfinedSpaceFormEntrantMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.ConfinedSpaceForm).Not.Nullable();
            References(x => x.EntrantType, "ConfinedSpaceFormEntrantTypeId").Not.Nullable();
            References(x => x.Employee).Nullable();
            Map(x => x.ContractingCompany).Nullable()
                                          .Length(ConfinedSpaceFormEntrant.StringLengths.CONTRACTING_COMPANY);
            Map(x => x.ContractorName).Nullable().Length(ConfinedSpaceFormEntrant.StringLengths.CONTRACTOR_NAME);
        }
    }
}
