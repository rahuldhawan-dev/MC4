using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
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

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderPlanning
{
    public class UpdateWorkOrderPlanning : ViewModel<WorkOrder>
    {
        #region Constructors

        public UpdateWorkOrderPlanning(IContainer container) : base(container) { }

        #endregion

        #region Private Members

        private WorkOrder _displayWorkOrder;

        #endregion
        
        #region Properties

        [AutoMap("TrafficControlRequired")]
        [View("Traffic Control Required")]
        public bool TrafficControlRequiredPlanning { get; set; }
        public int? NumberOfOfficersRequired { get; set; }

        [DoesNotAutoMap]
        public virtual string AdditionalNotes { get; set; }

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

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            base.MapToEntity(entity);
            
            if (!string.IsNullOrEmpty(AdditionalNotes))
            {
                var dtp = _container.GetInstance<IDateTimeProvider>();
                var authServ = _container.GetInstance<IAuthenticationService<User>>();
                entity.AppendNotes(authServ.CurrentUser, dtp.GetCurrentDate(), AdditionalNotes);
            }
            
            return entity;
        }

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
    }
}