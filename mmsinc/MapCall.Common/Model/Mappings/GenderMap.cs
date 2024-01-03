using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class GenderMap : ClassMap<Gender>
    {
        public GenderMap()
        {
            Id(x => x.Id);

            Map(x => x.Description).Not.Nullable().Unique();
        }
    }
}
