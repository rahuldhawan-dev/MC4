using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class OperatingCenterAssetTypeRepository : WorkOrdersRepository<OperatingCenterAssetType>
    {
        public static IEnumerable<AssetType> SelectAssetTypesByOperatingCenter(OperatingCenter operatingCenter)
        {
            return DataTable.Where(oc => oc.OperatingCenter == operatingCenter).Select(x=>x.AssetType);
        }
    }
}
