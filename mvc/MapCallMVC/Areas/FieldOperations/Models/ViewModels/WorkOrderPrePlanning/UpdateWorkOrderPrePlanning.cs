using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderPrePlanning
{
    public class UpdateWorkOrderPrePlanning : ViewModel<WorkOrder>
    {
        #region Private Members

        private WorkOrder _displayWorkOrder;

        #endregion

        #region Properties

        [ClientCallback("WorkOrderPrePlanning.validatePlannedCompletionDate", 
            ErrorMessage = CreateWorkOrder.PLANNED_COMPLETION_DATE_ERROR_MESSAGE)]
        [View(WorkOrder.DisplayNames.PLANNED_COMPLETION_DATE)]
        public DateTime? PlannedCompletionDate { get; set; }
        [DoesNotAutoMap]
        public int[] WorkOrderIds { get; set; }

        [DoesNotAutoMap]
        public virtual WorkOrder DisplayWorkOrder
        {
            get
            {
                if (_displayWorkOrder == null)
                {
                    _displayWorkOrder = _container.GetInstance<IRepository<WorkOrder>>().Find(Id);
                }

                return _displayWorkOrder;
            }
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidatePlannedCompletionDate());
        }

        private IEnumerable<ValidationResult> ValidatePlannedCompletionDate()
        {
            var today = _container.GetInstance<IDateTimeProvider>().GetCurrentDate().BeginningOfDay();

            if (DisplayWorkOrder?.Priority?.Id == (int)WorkOrderPriority.Indices.EMERGENCY)
            {
                if (PlannedCompletionDate?.BeginningOfDay() < today)
                {
                    yield return new ValidationResult(CreateWorkOrder.PLANNED_COMPLETION_DATE_ERROR_MESSAGE,
                        new[] { "PlannedCompletionDate" });
                }
            }
            else if (PlannedCompletionDate?.BeginningOfDay() < today.AddDays(2))
            {
                yield return new ValidationResult(
                    CreateWorkOrder.PLANNED_COMPLETION_DATE_ERROR_MESSAGE,
                    new[] { "PlannedCompletionDate" });
            }
        }

        #endregion

        #region Constructors

        public UpdateWorkOrderPrePlanning(IContainer container) : base(container) { }

        #endregion
    }
}
