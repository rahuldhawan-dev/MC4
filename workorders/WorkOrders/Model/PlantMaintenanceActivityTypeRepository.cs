//PlantMaintenanceActivityType = MapCall.Common.Model.Entities.PlantMaintenanceActivityType;
using System.Linq;
using System.Collections.Generic;

namespace WorkOrders.Model
{
    public class PlantMaintenanceActivityTypeRepository : WorkOrdersRepository<PlantMaintenanceActivityType>
    {
        public IEnumerable<PlantMaintenanceActivityType> SelectForWorkOrderInput()
        {
            var validCodes = PlantMaintenanceActivityType.GetOverrideCodes();
            return (from d in DataTable
                    where validCodes.Contains(d.Id)
                    select d);
        }
    }
}