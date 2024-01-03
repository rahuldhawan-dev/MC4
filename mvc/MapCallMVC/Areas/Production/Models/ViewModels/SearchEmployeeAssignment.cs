using System;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SearchEmployeeAssignment : SearchSet<EmployeeAssignment>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State)), Search(CanMap = false)]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateIdForProductionWorkManagement", DependsOn = "State", PromptText = "Please select a state above"),
            EntityMustExist(typeof(OperatingCenter)), SearchAlias("ProductionWorkOrder", "pwo", "OperatingCenter.Id"),
            RequiredWhen("State", ComparisonType.NotEqualTo, null, ErrorMessage = "Required with State")]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Employee", "ProductionWorkManagementEmployeesByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMustExist(typeof(Employee))]
        public int? AssignedTo { get; set; }

        public DateRange AssignedFor { get; set; }

        public bool? OrderIsOpen { get; set; }

        #endregion
    }
}