using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MeterTypeMap : ClassMap<MeterType>
    {
        public MeterTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("MeterTypeID");

            Map(x => x.Description).Not.Nullable().Unique().Length(MeterType.DESCRIPTION_LENGTH);
        }
    }
}
