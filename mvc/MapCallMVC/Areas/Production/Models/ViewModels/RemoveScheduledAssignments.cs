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
using MMSINC.Data.NHibernate;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class RemoveScheduledAssignments : ViewModel<MaintenancePlan>
    {
        #region Properties

        [DoesNotAutoMap("Mapped manually")]
        public int[] SelectedAssignments { get; set; }

        #endregion

        #region Constructors

        public RemoveScheduledAssignments(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override MaintenancePlan MapToEntity(MaintenancePlan entity)
        {
            var assignments = _container.GetInstance<IRepository<ScheduledAssignment>>()
                                        .Where(x => SelectedAssignments.Contains(x.Id));
            
            foreach (var assignment in assignments)
            {
                entity.ScheduledAssignments.Remove(assignment);
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
            if (SelectedAssignments is null || SelectedAssignments.Length == 0)
            {
                yield return new ValidationResult("At least one assignment must be selected for removal.");
            }
        }

        #endregion
    }
}