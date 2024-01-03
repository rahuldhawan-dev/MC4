using System.ComponentModel.DataAnnotations;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class BaseSearchValveInspectionsByMonth
    {
        [Required, DropDown]
        public int? OperatingCenter { get; set; }

        [Required, DropDown]
        public int? Year { get; set; }
    }
}