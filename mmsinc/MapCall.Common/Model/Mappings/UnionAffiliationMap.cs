using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class UnionAffiliationMap : ClassMap<UnionAffiliation>
    {
        public UnionAffiliationMap()
        {
            Table(FixTableAndColumnNamesForBug1623.NewTableNames.UNION_AFFILIATIONS);
            Id(x => x.Id);
            Map(x => x.Description).Not.Nullable().Unique();
        }
    }
}
