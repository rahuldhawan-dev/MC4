using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Models.ViewModels
{
    public class SearchMainBreaksAndServiceLineRepairsByMonth : SearchSet<WorkOrder>
    {
        #region Properties

        [MultiSelect]
        public int[] OperatingCenter { get; set; }

        [MultiSelect, Required]
        public int[] YearCompleted { get; set; }

        #endregion
    }
}