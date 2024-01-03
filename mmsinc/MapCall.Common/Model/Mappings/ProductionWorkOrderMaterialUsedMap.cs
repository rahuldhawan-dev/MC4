using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionWorkOrderMaterialUsedMap : ClassMap<ProductionWorkOrderMaterialUsed>
    {
        public const string TABLE_NAME = "ProductionWorkOrderMaterialUsed";

        public ProductionWorkOrderMaterialUsedMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id);
            Map(x => x.NonStockDescription).Nullable();
            References(x => x.ProductionWorkOrder).Not.Nullable();
            References(x => x.Material).Nullable();
            Map(x => x.Quantity).Not.Nullable();
            References(x => x.StockLocation).Nullable();
        }
    }
}
