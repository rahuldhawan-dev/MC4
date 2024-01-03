using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.ViewModels
{
    public class EnvironmentalNonComplianceEventCreatedNotification
    {
        public EnvironmentalNonComplianceEvent EnvironmentalNonComplianceEvent { get; set; }
        public string CreatedByFullName { get; set; }
        public string RecordUrl { get; set; }
    }
}
