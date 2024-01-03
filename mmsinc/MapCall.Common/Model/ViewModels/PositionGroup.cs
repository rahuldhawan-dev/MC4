using System.ComponentModel;
using System.Globalization;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public class TrainingClassificationSumReportItem
    {
        public string OperatingCenter { get; set; }

        [DisplayName("OSHA Requirement")]
        public bool? OSHARequirement { get; set; }

        public string ClassId { get; set; }
        public string TrainingModule { get; set; }
        public string CommonName { get; set; }
        public string Position { get; set; }
        public string PositionGroup { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        [DisplayName("Month")]
        public string MonthStr
        {
            get { return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month); }
        }

        public int NumberOfEmployees { get; set; }
    }

    public interface ISearchTrainingClassificationSum : ISearchSet<TrainingClassificationSumReportItem>
    {
        int? OperatingCenter { get; set; }
        bool? OSHARequirement { get; set; }
        string ClassId { get; set; }
        int? PositionGroupCommonName { get; set; }
        int? TrainingModule { get; set; }
        int? Id { get; set; }

        [Search(CanMap = false)]
        int? Year { get; set; }
    }
}
