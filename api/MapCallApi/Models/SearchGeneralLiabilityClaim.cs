using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models
{
    public class SearchGeneralLiabilityClaim : SearchSet<GeneralLiabilityClaim>
    {
        public DateRange IncidentDateTime { get; set; }
    }
}