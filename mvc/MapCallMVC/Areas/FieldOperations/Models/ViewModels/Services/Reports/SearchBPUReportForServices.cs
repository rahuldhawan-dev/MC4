using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports
{
    public class SearchBPUReportForServices : SearchSet<BPUReportForServiceReportItem>, ISearchBPUReportForServices
    {
        #region Properties

        [DropDown]
        public virtual int? OperatingCenter { get; set; }

        [Required, DropDown]
        public virtual int Year { get; set; }

        #endregion
    }
}