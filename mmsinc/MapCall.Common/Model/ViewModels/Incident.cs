using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public interface ISearchIncidentOSHARecordableSummary : ISearchSet<Incident>
    {
        int[] OperatingCenter { get; set; }
        DateRange IncidentDate { get; set; }
    }
}
