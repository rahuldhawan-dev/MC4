using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchInspectionProductivity : SearchSet<InspectionProductivityReportItem>, ISearchInspectionProductivity
    {
        [Required]
        public DateTime? StartDate { get; set; }

        [DropDown]
        public InspectionProductivityWeekSpan? Week { get; set; }

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown("", "User", "GetByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? InspectedBy { get; set; }

        public int GetDays()
        {
            if (Week.HasValue && Week.Value == InspectionProductivityWeekSpan.TwoWeeks)
            {
                return 14;
            }

            // Return seven days by default rather than make Week a required field.
            return 7;
        }
    }
}