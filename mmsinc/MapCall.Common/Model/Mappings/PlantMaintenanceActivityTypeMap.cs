using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class PlantMaintenanceActivityTypeMap : EntityLookupMap<PlantMaintenanceActivityType>
    {
        public PlantMaintenanceActivityTypeMap()
        {
            Map(x => x.Code).Length(PlantMaintenanceActivityType.StringLengths.CODE).Not.Nullable();

            References(x => x.OrderType).Nullable();
        }
    }
}
