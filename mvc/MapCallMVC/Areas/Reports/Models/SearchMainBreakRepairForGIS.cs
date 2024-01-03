using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchMainBreakRepairsForGIS : SearchSet<WorkOrder>
    {
        public bool? UpdatedMobileGIS { get; set; }
        [DropDown, EntityMustExist(typeof(OperatingCenter)), EntityMap]
        public int? OperatingCenter { get; set; }
        public DateRange DateCompleted { get; set; }
    }
}