using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchAverageCompletionTime : SearchSet<AverageCompletionTime>, ISearchAverageCompletionTime
    {
        #region Properties

        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        #endregion

        public SearchAverageCompletionTime()
        {
            EnablePaging = false;
        }
    }
}
