using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class CreateEmployeeAssignment : IValidatableObject
    {
        #region Properties

        [DoesNotAutoMap("Mapped manually")]
        [Required, DropDown, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        public int[] ProductionWorkOrderIds { get; set; }

        [EntityMap(MapDirections.None), EntityMustExist(typeof(ProductionSkillSet))]
        public int? ProductionSkillSet { get; set; }

        [EntityMustExist(typeof(Employee)), EntityMap, Required]
        [MultiSelect(Area = "", Controller = "Employee", Action = "GetEmployeesForProductionWorkOrderSchedulingByOperatingCenterAndProductionSkillSet", DependsOn = "OperatingCenter, ProductionSkillSet", PromptText = "Please select an operating center above", DependentsRequired = DependentRequirement.One)]
        public int[] AssignedTo { get; set; }

        [Required]
        public DateTime? AssignedFor { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProductionWorkOrderIds == null || !ProductionWorkOrderIds.Any())
            {
                yield return new ValidationResult("At least one order must be selected for assignment.");
            }
        }

        #endregion
    }
}