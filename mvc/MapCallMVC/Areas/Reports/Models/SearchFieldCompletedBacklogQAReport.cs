using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchFieldCompletedBacklogQAReport : SearchSet<FieldCompletedBacklogQAReportItem>, ISearchFieldCompletedBacklogQAReport
    {
        [Required, MultiSelect, EntityMap] 
        public int[] OperatingCenter { get; set; }

        [Required]
        public RequiredDateRange DateCompleted { get; set; }
    }
}