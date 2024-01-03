using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CleaningScheduleMap : ClassMap<CleaningSchedule>
    {
        public CleaningScheduleMap()
        {
            Id(x => x.Id, "CleaningScheduleID");

            Map(x => x.Description, "CleaningSchedule");
        }
    }
}
