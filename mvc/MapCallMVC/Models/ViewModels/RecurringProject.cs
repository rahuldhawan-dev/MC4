using MMSINC.Data;
using MMSINC.Metadata;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Models.ViewModels
{
    public class SearchRecurringProjectList : SearchSet<RecurringProject>
    {
        #region Properties

        // Operating Center
        [DropDown]
        public int? OperatingCenter { get; set; }

        // Town
        [DropDown(Area = "", Controller = "Town", Action = "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Town { get; set; }

        [DropDown]
        public int? AssetCategory { get; set; }

        public DateRange ActualInServiceDate { get; set; }

        // Estimated In Service Period
        public DateRange EstimatedInServiceDate { get; set; }

       // Foundation Filing Period
        [DropDown]
        public int? FoundationalFilingPeriod { get; set; }

        [DropDown]
        public int? Status { get; set; }

        public decimal? FinalCriteriaScore { get; set; }
        public decimal? FinalRawScore { get; set; }

        public bool? RequiresScoring { get; set; }

        #endregion
    }
}