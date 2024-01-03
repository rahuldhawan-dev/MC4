using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MainSizeMap : ClassMap<MainSize>
    {
        public MainSizeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("MainSizeId");

            Map(x => x.Description).Column("Size");
        }
    }
}
