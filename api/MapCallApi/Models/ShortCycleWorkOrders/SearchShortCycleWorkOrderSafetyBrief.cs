using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models.ShortCycleWorkOrders
{
    public class SearchShortCycleWorkOrderSafetyBrief : SearchSet<ShortCycleWorkOrderSafetyBrief>
    {
        public DateRange DateCompleted { get; set; }
        [SearchAlias("FSR", "EmployeeId")]
        public string FSR { get; set; }
    }
}
