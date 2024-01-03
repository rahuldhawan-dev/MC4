using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchOpenIssuedServicesReport : SearchSet<OpenIssuedServicesReportItem>
    {
        [DropDown, Required, EntityMustExist(typeof(OperatingCenter))]
//        [SearchAlias("OperatingCenter", "Id")]
        public int? OperatingCenter { get; set; }
        [DropDown("", "ServiceRestorationContractor", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above"), Required]
        [Display(Name = "Work Issued To")]
        public int? WorkIssuedTo { get; set; }
        public DateRange DateIssuedToField { get; set; }
    }
}