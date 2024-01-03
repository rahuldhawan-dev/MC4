using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports
{
    public class SearchServicesTDPendingServicesKPI : SearchSet<TDPendingServicesKPIReportItem>, ISearchTDPendingServicesKPI
    {
        #region Properties

        [DropDown, Required]
        public int? OperatingCenter { get; set; }

        #endregion
    }
}