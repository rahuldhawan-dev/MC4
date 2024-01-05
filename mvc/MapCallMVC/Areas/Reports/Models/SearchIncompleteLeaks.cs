using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchIncompleteLeaks : SearchSet<WorkOrder>
    {
        [DropDown]
        [SearchAlias("OperatingCenter", "Id", Required = true)]
        public virtual int? OperatingCenter { get; set; }
    }
}