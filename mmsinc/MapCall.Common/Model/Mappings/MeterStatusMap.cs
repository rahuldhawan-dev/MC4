using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MeterStatusMap : ClassMap<MeterStatus>
    {
        public const string TABLE_NAME = "MeterStatuses";

        public MeterStatusMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity().Column("MeterStatusID");
            Map(x => x.Description).Not.Nullable().Unique().Length(MeterStatus.DESCRIPTION_LENGTH);
        }
    }
}
