using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderPrePlanning
{
    public class AssignWorkOrderPrePlanning : ViewModel
    {
        [Secured]
        public int OperatingCenter { get; set; }

        public int[] WorkOrderIds { get; set; }

        [DropDown("", "User", "GetByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMustExist(typeof(Employee)), RequiredWhen("ContractorAssignedTo", null)]
        public int? AssignedTo { get; set; }

        [DropDown("Contractors", "Contractor", "ActiveContractorsByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMustExist(typeof(Contractor)), RequiredWhen("AssignedTo", null)]
        public int? ContractorAssignedTo { get; set; }
    }
}
