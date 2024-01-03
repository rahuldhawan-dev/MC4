using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.ViewModels
{
    public class EnvironmentalNonComplianceActionItemAssignedNotification
    {
        public EnvironmentalNonComplianceEvent EnvironmentalNonComplianceEvent { get; set; }
        public EnvironmentalNonComplianceEventActionItem EnvironmentalNonComplianceEventActionItem { get; set; }
        public string AssignedToFullName { get; set; }
        public string RecordUrl { get; set; }
        public string HelpUrl { get; set; }
    }
}