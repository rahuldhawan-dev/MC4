using System.ComponentModel.DataAnnotations;
using MMSINC.Metadata;

namespace MapCall.Common.Model.ViewModels
{
    public class SearchSapFunctionalLocation
    {
        #region Properties

        [Required]
        public virtual string PlanningPlant { get; set; }

        [StringLength(30), View(Description = "Use * for wildcards")]
        public string FunctionalLocation { get; set; }

        [StringLength(40)]
        public string Description { get; set; }

        [StringLength(2)]
        public string FunctionalLocationCategory { get; set; }

        [StringLength(10)]
        public string SortField { get; set; }

        [StringLength(10)]
        public string TechnicalObjectType { get; set; }

        #endregion
    }
}
