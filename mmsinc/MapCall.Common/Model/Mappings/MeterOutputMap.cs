using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MeterOutputMap : ClassMap<MeterOutput>
    {
        public MeterOutputMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("MeterOutputID");

            Map(x => x.Outputs).Not.Nullable().Precision(10);
        }
    }
}
