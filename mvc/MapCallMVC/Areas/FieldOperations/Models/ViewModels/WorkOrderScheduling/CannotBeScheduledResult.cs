namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderScheduling
{
    public class CannotBeScheduledResult : CanBeScheduledResult
    {
        public override bool CanBeScheduled => false;
        public string Notification { get; }

        public CannotBeScheduledResult(string notificationTemplate, int workOrderId)
        {
            Notification = string.Format(notificationTemplate, workOrderId);
        }
    }
}
