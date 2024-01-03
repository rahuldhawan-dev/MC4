namespace MapCall.Common.Model.ViewModels
{
    public class ShortCycleWorkOrderStatusUpdateViewModel
    {
        #region Properties

        public virtual string WorkOrderNumber { get; set; }
        public virtual string OperationNumber { get; set; }
        public virtual string AssignmentStart { get; set; }
        public virtual string AssignmentEnd { get; set; }
        public virtual string StatusNumber { get; set; }
        public virtual string StatusNonNumber { get; set; }
        public virtual int? AssignedEngineer { get; set; }
        public virtual string DispatchId { get; set; }
        public virtual string EngineerId { get; set; }
        public virtual string ItemTimeStamp { get; set; }
        public virtual string MapCallStatus { get; set; }
        public virtual string StatusNotes { get; set; }

        #endregion
    }
}
