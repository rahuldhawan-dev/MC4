using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models
{
    public class SearchIncident : SearchSet<Incident>
    {
        public DateRange IncidentDate { get; set; }
        public DateRange UpdatedAt { get; set; }
    }
}