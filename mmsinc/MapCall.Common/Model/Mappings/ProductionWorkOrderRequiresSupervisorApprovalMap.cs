using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    /// <summary>
    /// This is a View not a table MC-1220
    /// </summary>
    public class
        ProductionWorkOrderRequiresSupervisorApprovalMap : ClassMap<ProductionWorkOrderRequiresSupervisorApproval>
    {
        public ProductionWorkOrderRequiresSupervisorApprovalMap()
        {
            Table("ProductionWorkOrderRequiresSupervisorApproval");
            ReadOnly();
            CompositeId().KeyReference(x => x.ProductionWorkOrder, "ProductionWorkOrderId");
            References(x => x.ProductionWorkOrder).Not.Nullable().ReadOnly();
            Map(x => x.RequiresSupervisorApproval).Not.Nullable();

            // Need this so when SchemaExport doesn't create a table
            SchemaAction.None();
        }
    }
}
