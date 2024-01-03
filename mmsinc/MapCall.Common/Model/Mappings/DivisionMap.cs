using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class DivisionMap : ClassMap<Division>
    {
        public DivisionMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable();

            References(x => x.State).Not.Nullable();
        }
    }
}
