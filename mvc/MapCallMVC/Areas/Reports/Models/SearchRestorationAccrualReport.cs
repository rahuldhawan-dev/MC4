using MMSINC.Data;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchRestorationAccrualReport : SearchSet<Restoration>
    {
        #region Properties

        public override bool DefaultSortAscending => true;
        public override string DefaultSortBy => "WorkOrder.Id";

        #region Properties not being searched on but need to exist for aliases

        // Not displayed, but needed for alias.
        [SearchAlias("WorkOrder", "Id", Required = true)]
        public int? WorkOrder { get; set; }

        // Not displayed, but needed for alias
        [SearchAlias("WorkOrder.WorkDescription", "Id", Required = true)]
        public int? WorkDescription { get; set; }
        
        #endregion

        [DropDown]
        [Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [SearchAlias("WorkOrder.WorkDescription", "AccountingType.Id")]
        [DropDown]
        [Required, EntityMap, EntityMustExist(typeof(AccountingType))]
        public int? AccountingType { get; set; }

        [Required]
        [SearchAlias("WorkOrder", "DateCompleted")]
        public RequiredDateRange WorkOrderDateCompleted { get; set; }

        #endregion
    }
}