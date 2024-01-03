using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchSewerMainCleaningFootageReport : SearchSet<SewerMainCleaningFootageReportItem>, ISearchSewerMainCleaningFootageReport
    {
        #region Properties

        [DropDown, EntityMustExist(typeof(OperatingCenter)), EntityMap]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SewerMainInspectionType))]
        public int? InspectionType { get; set; }

        [Required, DropDown]
        public int? Year { get; set; }

        public int? Month { get; set; }

        #endregion
    }
}
