using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Models.ViewModels
{
    public class SearchLockoutForm : SearchSet<LockoutForm>, IValidatableObject
    {

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("oc.State", "st", "Id", Required = true)]
        public int? State { get; set; }

        [MultiSelect("", "OperatingCenter", "ByStateIdForHealthAndSafetyLockoutForm", DependsOn = "State", DependentsRequired = DependentRequirement.None)]
        [SearchAlias("OperatingCenter", "oc", "Id", Required = true)]
        public int[] OperatingCenter { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterIds", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Facility { get; set; }
        
        // TODO: Change this from text to a lookup.
        [DropDown("", "Employee", "GetByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? OutOfServiceAuthorizedEmployee { get; set; }

        [DropDown(Area = "", Controller = "EquipmentType", Action = "ByFacilityId", DependsOn = "Facility", PromptText = "Select a facility above.")]
        [EntityMap, EntityMustExist(typeof(EquipmentType))]
        public int? EquipmentType { get; set; }

        [DropDown("", "Equipment", "ByFacilityIdAndSometimesEquipmentTypeIdAndProductionWorkOrder", DependsOn = "Facility,EquipmentType", PromptText = "Please select a facility above")]
        public int? Equipment { get; set; }

        [DropDown]
        public int? LockoutReason { get; set; }
        [DropDown]
        public int? IsolationPoint { get; set; }
        public DateRange OutOfServiceDateTime { get; set; }
        public DateRange ReturnedToServiceDateTime { get; set; }

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}