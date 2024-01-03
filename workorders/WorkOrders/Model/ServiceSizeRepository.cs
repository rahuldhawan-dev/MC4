using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class ServiceSizeRepository : WorkOrdersRepository<ServiceSize>
    {
        public static IEnumerable<ServiceSize> GetMainBreakSizes()
        {
            return (from m in DataTable
                    where m.Main == true && m.ServiceSizeDescription != null && m.Size > 0m
                    orderby  m.SortOrder
                    select m);
        }

        public static IEnumerable<ServiceSize> SelectAllSorted()
        {
            return (from m in DataTable
                orderby m.Size
                select m);
        }
    }

    public class ServiceMaterialRepository : WorkOrdersRepository<ServiceMaterial>
    {
        public static IEnumerable<ServiceMaterial> SelectAllButUnknownSorted()
        {
            return (from m in DataTable
                    orderby m.Description
                    where m.Description != "UNKNOWN"
                    select m);
        }
    }
}
