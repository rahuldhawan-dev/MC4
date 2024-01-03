using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WorkOrderScheduleOfValueMap : ClassMap<WorkOrderScheduleOfValue>
    {
        public const string TABLE_NAME = "WorkOrdersScheduleOfValues";

        public WorkOrderScheduleOfValueMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.WorkOrder).Not.Nullable();
            References(x => x.ScheduleOfValue).Not.Nullable();
            Map(x => x.IsOvertime).Not.Nullable();
            Map(x => x.Total).Not.Nullable();
            Map(x => x.LaborUnitCost).Not.Nullable();
            Map(x => x.OtherDescription).Nullable();
        }
    }
}
