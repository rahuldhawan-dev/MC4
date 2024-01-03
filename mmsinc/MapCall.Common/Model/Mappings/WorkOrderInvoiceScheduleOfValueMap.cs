using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WorkOrderInvoiceScheduleOfValueMap : ClassMap<WorkOrderInvoiceScheduleOfValue>
    {
        public const string TABLE_NAME = "WorkOrderInvoicesScheduleOfValues";

        public WorkOrderInvoiceScheduleOfValueMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.WorkOrderInvoice).Nullable();
            References(x => x.ScheduleOfValue).Nullable();
            Map(x => x.LaborUnitCost).Nullable();
            Map(x => x.MaterialCost).Nullable();
            Map(x => x.MiscCost).Nullable();
            Map(x => x.Total).Nullable();
            Map(x => x.IsOvertime).Nullable();
            Map(x => x.OtherDescription).Nullable();
            Map(x => x.IncludeWithInvoice).Not.Nullable();
            Map(x => x.IncludeMarkup).Not.Nullable();
        }
    }
}
