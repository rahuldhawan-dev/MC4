using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class InstitutionalKnowledgeMap : ClassMap<InstitutionalKnowledge>
    {
        public InstitutionalKnowledgeMap()
        {
            Table(FixTableAndColumnNamesForBug1623.NewTableNames.INSTITUTIONAL_KNOWLEDGE);
            Id(x => x.Id);
            Map(x => x.Description).Not.Nullable().Unique();
        }
    }
}
