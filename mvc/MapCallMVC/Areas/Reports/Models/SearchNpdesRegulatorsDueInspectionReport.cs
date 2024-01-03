using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.ViewModels;
using MMSINC.Utilities.Excel;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchNpdesRegulatorsDueInspectionReport : SearchSet<NpdesRegulatorsDueInspectionReportItem>, ISearchNpdesRegulatorsDueInspectionReportItem
    {
        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int? OperatingCenter { get; set; }

        public int? OperatingCenterId { get; set; }

        [Required]
        public RequiredDateRange DepartureDateTime { get; set; }

        [EntityMap, EntityMustExist(typeof(Town))]
        [SearchAlias("Town", "town", "Id")]
        public int? Town { get; set; }

        public int? TownId { get; set; }

        [EntityMap, EntityMustExist(typeof(AssetStatus))]
        public int? Status { get; set; }

        public int? StatusId { get; set; }
    }
}