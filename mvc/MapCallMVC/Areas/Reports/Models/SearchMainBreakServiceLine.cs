using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchMainBreak : SearchSet<MainBreakReportItem>, ISearchMainBreakReport
    {
        [DropDown]
        public int? State { get; set; }
        public bool? IsContractedOperations { get; set; }
        [MultiSelect(DependsOn = "State,IsContractedOperations", Area = "", Controller = "OperatingCenter", Action = "ByStateIdAndContracted", DependentsRequired = DependentRequirement.None)]
        public int[] OperatingCenter { get; set; }
        [Required, DropDown]
        public int? Year { get; set; }
    }
}
