using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using Newtonsoft.Json;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchCompletedWorkOrders : SearchSet<WorkOrder>
    {
        [DropDown]
        [SearchAlias("OperatingCenter", "Id", Required = true)]
        public virtual int? OperatingCenter { get; set; }

        [MultiSelect("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Select an operating center above")]
        [SearchAlias("Town", "Id", Required = true)]
        public int[] Town { get; set; }
        
        public DateRange DateCompleted { get; set; }
        
        // These fields are here to create the joins in the query that's executed so that
        // a query isn't fired off for each value, instead a join is created
        [SearchAlias("Street", "Id", Required = true)]
        public int? Street { get; set; }
        [SearchAlias("WorkDescription", "wd", "Id", Required = true)]
        public int? WorkDescription { get; set; }
        [SearchAlias("AssetType", "Id", Required = true)]
        public int? AssetType { get; set; }
        [SearchAlias("RequestedBy", "Id", Required = true)]
        public int? RequestedBy { get; set; }
        [SearchAlias("CreatedBy", "Id", Required = true)]
        public int? CreatedBy { get; set; }
        [SearchAlias("RequestingEmployee", "Id", Required = true)]
        public int? RequestingEmployee { get; set; }
        [SearchAlias("CompletedBy", "Id", Required = true)]
        public int? CompletedBy { get; set; }
        [SearchAlias("MaterialsApprovedBy", "Id", Required = true)]
        public int? MaterialsApprovedBy { get; set; }
        [SearchAlias("ApprovedBy", "Id", Required = true)]
        public int? ApprovedBy { get; set; }
        [SearchAlias("wd.AccountingType", "Id", Required = true)]
        public int? AccountingType { get; set; }
        [SearchAlias("CurrentAssignment", "ca", "Id", Required = true)]
        public int? CurrentAssignment { get; set; }
        [SearchAlias("ca.Crew", "Id", Required = true)]
        public int? CurrentCrew { get; set; }
    }
}
