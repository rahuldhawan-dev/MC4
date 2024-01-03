using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports
{
    public class SearchServicesCompletedByCategory : SearchSet<ServicesCompletedByCategoryReportItem>, ISearchServicesCompletedByCategory
    {
        #region Properties

        [DropDown, Required]
        public int? OperatingCenter { get; set; }

        public DateRange DateInstalled { get; set; }
        public bool? DeveloperServicesDriven { get; set; }

        #endregion
    }
}