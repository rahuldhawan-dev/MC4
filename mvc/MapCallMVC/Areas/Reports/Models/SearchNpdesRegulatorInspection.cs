using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchNpdesRegulatorInspection : SearchSet<NpdesRegulatorInspectionReportItem>, ISearchNpdesRegulatorInspectionReport
    {
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [EntityMap, EntityMustExist(typeof(Town))]
        [DropDown("Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center", Area = "")]
        public int? Town { get; set; }

        [DropDown]
        public int? Year { get; set; }

        public DateRange DepartureDateTime { get; set; }
    }
}