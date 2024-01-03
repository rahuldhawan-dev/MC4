using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchServiceDSIC : SearchSet<ServiceDSICReportItem>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        public DateRange DateInstalled { get; set; }
    }
}