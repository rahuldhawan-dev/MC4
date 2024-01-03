using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports
{
    public class SearchServicesRenewedSummary : SearchSet<ServicesRenewedSummaryReportItem>, ISearchServicesRenewedSummary
    {
        #region Properties

        [DropDown]
        public virtual int? OperatingCenter { get; set; }

        public IntRange Year { get; set; }

        #endregion
    }
}