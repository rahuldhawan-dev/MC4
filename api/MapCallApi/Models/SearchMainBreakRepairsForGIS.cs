using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;

namespace MapCallApi.Models {
    public class SearchMainBreakRepairsForGIS : SearchSet<WorkOrder>, ISearchMainBreakRepairsForGIS
    {
        [Search(CanMap = false)]
        public DateRange DateCompleted { get; set; }
        [Search(CanMap = false)]
        public bool? RecentOrders { get; set; }
    }
}