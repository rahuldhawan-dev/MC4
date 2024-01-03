using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class AllocationCategoryMap : ClassMap<AllocationCategory>
    {
        public AllocationCategoryMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("AllocationCategoryID");

            Map(x => x.Description)
               .Not.Nullable()
               .Unique().Length(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.DESCRIPTION);
        }
    }
}
