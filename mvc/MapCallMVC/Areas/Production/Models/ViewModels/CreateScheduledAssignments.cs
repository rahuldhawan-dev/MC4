using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class CreateScheduledAssignments : ViewModel<MaintenancePlan>
    {
        #region Properties

        [DoesNotAutoMap("Mapped manually")]
        [Required, DropDown, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [DoesNotAutoMap("Mapped manually"), EntityMustExist(typeof(Employee)), Required]
        [MultiSelect(Area = "", Controller = "Employee", Action = "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above", DependentsRequired = DependentRequirement.One)]
        public int[] AssignedTo { get; set; }

        [Required, DoesNotAutoMap("Mapped manually")]
        public DateTime? AssignedFor { get; set; }

        [DoesNotAutoMap("Mapped manually")]
        public DateTime[] ScheduledDates { get; set; }

        #endregion

        #region Constructors

        public CreateScheduledAssignments(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override MaintenancePlan MapToEntity(MaintenancePlan entity)
        {
            var employees = _container.GetInstance<EmployeeRepository>()
                                      .Where(x => AssignedTo.Contains(x.Id));
            var createdBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee;

            foreach (var scheduledDate in ScheduledDates)
            {
                foreach (var assignedTo in AssignedTo)
                {
                    entity.ScheduledAssignments.Add(new ScheduledAssignment {
                        AssignedFor = AssignedFor.Value,
                        AssignedTo = employees.First(x => x.Id == assignedTo),
                        CreatedBy = createdBy,
                        MaintenancePlan = entity,
                        ScheduledDate = scheduledDate
                    });
                }
            }

            return base.MapToEntity(entity);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateScheduledDateIsSelected());
        }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateScheduledDateIsSelected()
        {
            if (ScheduledDates is null || ScheduledDates.Length == 0)
            {
                yield return new ValidationResult("At least one planned date must be selected for assignment.");
            }
        }

        #endregion
    }
}