using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ScheduleTypeMap : ClassMap<ScheduleType>
    {
        public const string TABLE_NAME = "ScheduleType";

        public ScheduleTypeMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "ScheduleTypeId");
            Map(x => x.Description, "ScheduleType");
        }
    }
}
