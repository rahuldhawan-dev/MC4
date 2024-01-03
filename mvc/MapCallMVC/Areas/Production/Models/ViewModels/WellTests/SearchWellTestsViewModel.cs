using System;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.Production.Models.ViewModels.WellTests
{
    public class SearchWellTestsViewModel : SearchSet<WellTest>
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
         DropDown("", "Equipment", "ByFacilityIdForEquipmentTypeOfWell", DependsOn = nameof(Facility), PromptText = "Please select a Facility.")]
        public int? Equipment { get; set; }

        public virtual DateTime? DateOfTest { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Employee)),
         DropDown("", "Employee", "GetByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center.")]
        public int? Employee { get; set; }

        #endregion
    }
}