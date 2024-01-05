using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchIncompleteLeaks : SearchSet<WorkOrder>, ISearchIncompleteLeaks
    {
        [DropDown]
        [SearchAlias("OperatingCenter", "Id", Required = true)]
        public virtual int? OperatingCenter { get; set; }
    }
}