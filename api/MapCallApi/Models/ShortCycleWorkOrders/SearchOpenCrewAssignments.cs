using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models.ShortCycleWorkOrders {
    public class SearchOpenCrewAssignments : SearchSet<CrewAssignment>
    {
        public int Hours { get; set; }
    }
}