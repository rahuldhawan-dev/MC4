using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchCovidStatusReport : SearchSet<CovidIssue>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("oc.State", "st", "Id", Required = true)]
        public int? State { get; set; }
        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State")]
        [SearchAlias("e.OperatingCenter", "oc", "Id", Required = true)]
        public int? OperatingCenter { get; set; }
        // THIS IS ONLY HERE TO GET THE ALIAS 
        [SearchAlias("Employee", "e", "Id", Required = true)]
        public int? Employee { get; set; }
        public DateRange SubmissionDate { get; set; }
        [SearchAlias("Employee", "e", "EmployeeId")]
        public string EmployeeId { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidOutcomeCategory))]
        [SearchAlias("OutcomeCategory", "cat", "Id", Required = true)]
        public int? OutcomeCategory { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidSubmissionStatus))]
        [SearchAlias("SubmissionStatus", "ss", "Id", Required = true)]
        public int? SubmissionStatus { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidQuarantineStatus))]
        [SearchAlias("QuarantineStatus", "qs", "Id", Required = true)]
        public int? QuarantineStatus { get; set; }
        public DateRange StartDate { get; set; }
        public DateRange ReleaseDate { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(ReleaseReason))]
        public int? ReleaseReason { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidRequestType))]
        public int? RequestType { get; set; }
        public DateRange EstimatedReleaseDate { get; set; }
    }
}
