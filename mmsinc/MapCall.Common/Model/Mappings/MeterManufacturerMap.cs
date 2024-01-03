using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MeterManufacturerMap : ClassMap<MeterManufacturer>
    {
        public MeterManufacturerMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("MeterManufacturerID");

            Map(x => x.Description).Not.Nullable().Unique().Length(MeterManufacturer.DESCRIPTION_LENGTH);
        }
    }
}
