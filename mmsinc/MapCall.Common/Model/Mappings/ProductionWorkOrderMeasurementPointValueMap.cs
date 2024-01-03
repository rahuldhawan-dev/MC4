using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionWorkOrderMeasurementPointValueMap : ClassMap<ProductionWorkOrderMeasurementPointValue>
    {
        public ProductionWorkOrderMeasurementPointValueMap()
        {
            Id(x => x.Id); //.GeneratedBy.Identity();

            Map(x => x.Value).Not.Nullable();
            Map(x => x.MeasurementDocId).Nullable();

            References(x => x.ProductionWorkOrder).Not.Nullable();
            References(x => x.MeasurementPointEquipmentType).Not.Nullable();
            References(x => x.Equipment).Not.Nullable();
        }
    }
}
