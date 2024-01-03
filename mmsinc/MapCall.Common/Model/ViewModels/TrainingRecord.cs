using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public class TrainingTotalHoursReportItem
    {
        public string OperatingCenter { get; set; }
        public bool OSHARequirement { get; set; }
        public string ClassId { get; set; }
        public string TrainingModule { get; set; }
        public string CommonName { get; set; }
        public string Position { get; set; }
        public string PositionGroup { get; set; }
        public int Year { get; set; }
        public int TotalEmployeesAttended { get; set; }
        public int TotalEmployeesScheduled { get; set; }
        public int? TotalHours { get; set; }

        public int TotalHoursDelivered
        {
            get { return (TotalHours ?? 0) * TotalEmployeesAttended; }
        }

        public int TotalHoursScheduled
        {
            get { return (TotalHours ?? 0) * TotalEmployeesScheduled; }
        }
    }

    public interface ISearchTrainingTotalHours : ISearchSet<TrainingTotalHoursReportItem>
    {
        int? Year { get; set; }
    }
}
