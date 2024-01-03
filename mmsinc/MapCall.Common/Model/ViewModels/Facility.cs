namespace MapCall.Common.Model.ViewModels
{
    public class ArcFlashCompletionReportItem
    {
        public string OperatingCenter { get; set; }
        public int TotalArcFlashFacilities { get; set; }
        public int NumberCompleted { get; set; }
        public int NumberPending { get; set; }
        public int NumberDeferred { get; set; }
        public int TotalCompletedNotDeferred { get; set; }
    }
}
