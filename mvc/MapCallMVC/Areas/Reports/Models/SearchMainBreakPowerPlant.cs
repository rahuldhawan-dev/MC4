using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchMainBreakPowerPlant : SearchSet<MainBreak>
    {
        #region Properties

        [MultiSelect, SearchAlias("workOrder.OperatingCenter", "Id")]
        public int[] OperatingCenter { get; set; }
        [SearchAlias("WorkOrder", "workOrder", "DateCompleted")]
        public DateRange DateCompleted { get; set; }

        #endregion

    }
}