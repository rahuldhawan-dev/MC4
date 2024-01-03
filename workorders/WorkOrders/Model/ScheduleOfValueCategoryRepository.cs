using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class ScheduleOfValueCategoryRepository : WorkOrdersRepository<ScheduleOfValueCategory>
    {
        public static int[] LaborCategories = new[] { 21, 22, 23, 24, 25, 26 };
        
        public IEnumerable<ScheduleOfValueCategory> GetScheduleOfValueCategories()
        {
            return from x in DataTable orderby x.Description select x;
        }

        public IEnumerable<ScheduleOfValueCategory> GetScheduleOfValueLaborCategories()
        {
            return from x in DataTable orderby x.Description select x ;
        }
    }
}