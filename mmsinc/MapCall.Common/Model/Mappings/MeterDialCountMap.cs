using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MeterDialCountMap : ClassMap<MeterDialCount>
    {
        public MeterDialCountMap()
        {
            Id(x => x.Id, "MeterDialCountID").GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Unique().Precision(10);
        }
    }
}
