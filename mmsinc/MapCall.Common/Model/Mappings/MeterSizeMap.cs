using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MeterSizeMap : ClassMap<MeterSize>
    {
        public MeterSizeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("MeterSizeID");

            Map(x => x.Size).Length(MeterSize.SIZE_LENGTH);
        }
    }
}
