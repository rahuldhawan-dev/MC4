using System;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits
{
    public class SearchRedTagPermitViewModel : SearchSet<RedTagPermit>
    {
        #region Properties

        [DropDown,
         EntityMap,
         SearchAlias("criteriaOperatingCenter.State", "criteriaState", "Id"),
         EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [EntityMap,
         SearchAlias("criteriaFacility.OperatingCenter", "criteriaOperatingCenter", "Id"),
         EntityMustExist(typeof(OperatingCenter)),
         DropDown("", "OperatingCenter", "ByStateId", DependsOn = nameof(State), PromptText = "Please select a State.")]
        public int? OperatingCenter { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Facility)),
         SearchAlias("criteriaEquipment.Facility", "criteriaFacility", "Id"),
         DropDown("", "Facility", "ByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center.")]
        public int? Facility { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Equipment)),
         SearchAlias("Equipment", "criteriaEquipment", "Id"),
         DropDown("", "Equipment", "ByFacilityIdAndIsEligibleForRedTagPermitEquipmentTypes", DependsOn = nameof(Facility), PromptText = "Please select a Facility.")]
        public int? Equipment { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Employee)),
         DropDown("", "Employee", "ActiveProductionWorkManagementEmployeesByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center.")]
        public int? PersonResponsible { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Employee)),
         DropDown("", "Employee", "ActiveProductionWorkManagementEmployeesByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center.")]
        public int? AuthorizedBy { get; set; }

        #endregion
    }
}
