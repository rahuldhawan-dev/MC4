using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WorkOrderInvoiceMap : ClassMap<WorkOrderInvoice>
    {
        public WorkOrderInvoiceMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            // TODO: Make this not nullable in the database. It's a required
            // field and none of the values are null.
            Map(x => x.InvoiceDate).Nullable();
            Map(x => x.IncludeMaterials).Not.Nullable();
            Map(x => x.SubmittedDate).Nullable();
            Map(x => x.CanceledDate).Nullable();
            Map(x => x.InvoiceNotes).Nullable();

            References(x => x.ScheduleOfValueType).Not.Nullable();
            References(x => x.WorkOrder).Nullable();
            // FORMULA
            References(x => x.WorkOrderInvoiceStatus)
               .Formula(
                    "(CASE WHEN (CanceledDate is not null) THEN 3 WHEN (SUBMITTEDDATE is not null) THEN 2 ELSE 1 END)");

            HasMany(x => x.WorkOrderInvoicesScheduleOfValues)
               .KeyColumn("WorkOrderInvoiceId")
               .Cascade.AllDeleteOrphan().Inverse();

            HasMany(x => x.WorkOrderInvoiceDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.WorkOrderInvoiceNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
