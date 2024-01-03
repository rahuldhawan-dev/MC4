using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class EndEmployeeAssignment : ViewModel<EmployeeAssignment>
    {
        #region Constructor

        public EndEmployeeAssignment(IContainer container) : base(container) { }

        #endregion

        #region Properties

        // This is a hidden field used for client-side validation.
        // Also note that this would be secured, except we're using a single form
        // on the page and that would screw things up if there are multiple
        // assignments. We could really use something similar to [Secured] that
        // autopopulates the value directly from the database record during model binding without
        // needing to use tokens.
        [Required, AutoMap(MapDirections.ToViewModel)]
        public DateTime? DateStarted { get; set; }

        [DateTimePicker, CompareTo(nameof(DateStarted), ComparisonType.GreaterThan, TypeCode.DateTime, IgnoreNullValues = true)]
        public DateTime? DateEnded { get; set; }

        [Required]
        [RegularExpression(@"(?:\d*\.\d{1,2}|\d+)$", ErrorMessage = EmployeeAssignment.MUST_BE_TWO_DECIMAL_PLACES)]
        [View("Hours Worked", FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public decimal? HoursWorked { get; set; }

        // This is also a hidden field. Only used by the controller to create a note.
        [Required, EntityMap(MapDirections.ToViewModel), EntityMustExist(typeof(ProductionWorkOrder))]
        public int? ProductionWorkOrder { get; set; }

        [Multiline, Required, DoesNotAutoMap("Value only used by the controller to create a note.")]
        public string Notes { get; set; }

        [DoesNotAutoMap("Not a View Property - set by MapToEntity and Used by Controller")]
        public bool? IsFinalAssignment { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method exists due to the way DateStarted is being passed to this model from the client. It does not
        /// work with using SecureAttribute, so we want to double check that it is correct.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ValidationResult> ValidateDateStartedWasNotTamperedWith()
        {
            var assignment = _container.GetInstance<IRepository<EmployeeAssignment>>().Find(Id);
            if (assignment == null)
            {
                yield break;
            }

            if (assignment.DateStarted.HasValue && DateStarted.HasValue && assignment.DateStarted.Value.Date != DateStarted.Value.Date)
            {
                yield return new ValidationResult("DateStarted is invalid.", new[] { nameof(DateStarted) });
            }
        }

        /// <summary>
        /// This method exists due to the way ProductionWorkOrder is being passed to this model from the client. It does not
        /// work with using SecureAttribute, so we want to double check that it is correct.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ValidationResult> ValidateProductionWorkOrderWasNotTamperedWith()
        {
            var assignment = _container.GetInstance<IRepository<EmployeeAssignment>>().Find(Id);
            if (assignment == null)
            {
                yield break;
            }

            if (assignment.ProductionWorkOrder.Id != ProductionWorkOrder)
            {
                yield return new ValidationResult("ProductionWorkOrder is invalid.", new[] { nameof(ProductionWorkOrder) });
            }
        }

        #endregion

        #region Public Methods

        public override EmployeeAssignment MapToEntity(EmployeeAssignment entity)
        {
            entity.DateEnded = DateEnded ?? _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.HoursWorked = HoursWorked ?? 0M;
            IsFinalAssignment = entity.ProductionWorkOrder?.CurrentAssignments.All(x => x.DateEnded != null);
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateDateStartedWasNotTamperedWith())
                       .Concat(ValidateProductionWorkOrderWasNotTamperedWith());
        }

        #endregion
    }
}