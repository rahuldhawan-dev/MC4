using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MaterialUsedMap : ClassMap<MaterialUsed>
    {
        public const string TABLE_NAME = "MaterialsUsed";

        public MaterialUsedMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity().Column("MaterialsUsedID");

            Map(x => x.Quantity).Not.Nullable();
            Map(x => x.NonStockDescription).Nullable();

            References(x => x.WorkOrder).Not.Nullable();
            References(x => x.Material).Nullable();
            References(x => x.StockLocation).Nullable();

            Map(x => x.Cost)
               .DbSpecificFormula(
                    "(SELECT TOP 1 ocsm.Cost FROM WorkOrders wo INNER JOIN OperatingCenters oc ON wo.OperatingCenterID = oc.OperatingCenterID INNER JOIN OperatingCenterStockedMaterials ocsm ON oc.OperatingCenterID = ocsm.OperatingCenterID WHERE ocsm.MaterialID = MaterialID AND wo.WorkOrderID = WorkOrderID)",
                    "(SELECT ocsm.Cost FROM WorkOrders wo INNER JOIN OperatingCenters oc ON wo.OperatingCenterID = oc.OperatingCenterID INNER JOIN OperatingCenterStockedMaterials ocsm ON oc.OperatingCenterID = ocsm.OperatingCenterID WHERE ocsm.MaterialID = MaterialID AND wo.WorkOrderID = WorkOrderID LIMIT 1)");
        }
    }
}
