using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class EnvironmentalPermitStatusMap : ClassMap<EnvironmentalPermitStatus>
    {
        public EnvironmentalPermitStatusMap()
        {
            Table("EnvironmentalPermitStatuses");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("EnvironmentalPermitStatusID");
            Map(x => x.Description)
               .Column("Description")
               .Not.Nullable()
               .Unique().Length(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.DESCRIPTION);
        }
    }
}
