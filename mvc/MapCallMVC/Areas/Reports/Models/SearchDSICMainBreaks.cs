using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchDSICMainBreaks : SearchSet<MainBreak>
    {
        #region Properties

        [SearchAlias("WorkOrder", "wo", "Id", Required = true)]
        public int? WorkOrder { get; set; }

        [DropDown]
        [SearchAlias("wo.OperatingCenter", "Id", Required = true)]
        public virtual int? OperatingCenter { get; set; }
      
        public DateRange DateCompleted { get; set; }
        
        public DateRange DateReceived { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Select an operating center above")]
        [SearchAlias("wo.Town", "Id", Required = true)]
        public int? Town { get; set; }

        [DropDown(PromptText = "Select Material")]
        [SearchAlias("MainBreakMaterial", "Id", Required = true)]
        public int? Material { get; set; }

        [SearchAlias("wo.Street", "Id", Required = true)]
        public int? StreetAddress { get; set; }

        [SearchAlias("wo.WorkDescription", "Id", Required = true)]
        public int? WorkDescription { get; set; }

        [SearchAlias("MainFailureType", "Id", Required = true)]
        public int? FailureType { get; set; } 
        
        [SearchAlias("MainBreakSoilCondition", "Id", Required = true)]
        public int? SoilCondition { get; set; }

        #endregion
    }
}