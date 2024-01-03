namespace MapCallMVC.Areas.SAP.Models.ViewModels
{
    public class AddEquipmentToMaintenancePlan
    {
        public string MaintenancePlan { get; set; }
        public string Equipment { get; set; }
        public string FunctionalLocation { get; set; }
        public int MapCallEquipmentId { get; set; }
        public string MaintenanceItem { get; set; }
    }

    public class RemoveEquipmentFromMaintenancePlan : AddEquipmentToMaintenancePlan
    {
        
    }
}
