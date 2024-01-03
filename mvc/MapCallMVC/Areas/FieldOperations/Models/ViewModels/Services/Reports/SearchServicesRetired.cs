using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports
{
    public class SearchServicesRetired : SearchSet<ServicesRetiredReportItem>, ISearchServicesRetired
    {
        #region Properties

        [DropDown]
        public virtual int? OperatingCenter { get; set; }

        [DropDown(Area = "", Controller = "Town", Action = "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public virtual int? Town { get; set; }

        [DropDown]
        public virtual int? YearRetired { get; set; }

        #endregion
    }
}