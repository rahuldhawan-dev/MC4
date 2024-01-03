using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.HumanResources.Models.ViewModels
{
    public class SearchCovidIssue : SearchSet<CovidIssue>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("oc.State", "st", "Id", Required = true)]
        public int? State { get; set; }
        [SearchAlias("e.OperatingCenter", "oc", "IsContractedOperations")]
        public bool? IsContractedOperations { get; set; }
        [MultiSelect("", "OperatingCenter", "ByStateIdOrIsContractedOperationsForHumanResourcesCovid", DependsOn = "State, IsContractedOperations", DependentsRequired = DependentRequirement.None)]
        [SearchAlias("e.OperatingCenter", "oc", "Id", Required = true)]
        public int[] OperatingCenter { get; set; }
        
        [DropDown("", "Employee", "ByOperatingCentersOrStateForHumanResourcesCovid", DependsOn = "State, OperatingCenter", DependentsRequired = DependentRequirement.One)]
        [SearchAlias("Employee", "e", "Id", Required = true)]
        public int? Employee { get; set; }
        [DropDown("", "Employee", "ByOperatingCentersOrStateForHumanResourcesCovid", DependsOn = "State, OperatingCenter", DependentsRequired = DependentRequirement.One)]
        [SearchAlias("e.ReportsTo", "rpt", "Id", Required = true)]
        public int? ReportsTo { get; set; }
        [DropDown("", "Employee", "ByOperatingCentersOrStateForHumanResourcesCovid", DependsOn = "State, OperatingCenter", DependentsRequired = DependentRequirement.One)]
        public int? HumanResourcesManager { get; set; }
        
        [SearchAlias("e.ReportingFacility", "fac", "Id", Required = true)]
        public int? ReportingFacility { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidRequestType))]
        [SearchAlias("RequestType", "reqt", "Id", Required = true)]
        public int? RequestType { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidSubmissionStatus))]
        [SearchAlias("SubmissionStatus", "ss", "Id", Required = true)]
        public int? SubmissionStatus { get; set; }
        public DateRange SubmissionDate { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidOutcomeCategory))]
        [SearchAlias("OutcomeCategory", "cat", "Id", Required = true)]
        public int? OutcomeCategory { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidQuarantineStatus))]
        [SearchAlias("QuarantineStatus", "qs", "Id", Required = true)]
        public int? QuarantineStatus { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ReleaseReason))]
        public int? ReleaseReason { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidAnswerType))]
        public int? WorkExposure { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidAnswerType))]
        public int? AvoidableCloseContact { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidAnswerType))]
        public int? FaceCoveringWorn { get; set; }
        public DateRange StartDate { get; set; }
        public DateRange EstimatedReleaseDate { get; set; }
        public DateRange ReleaseDate { get; set; }
        public bool? HealthDepartmentNotification { get; set; }
    }
}
