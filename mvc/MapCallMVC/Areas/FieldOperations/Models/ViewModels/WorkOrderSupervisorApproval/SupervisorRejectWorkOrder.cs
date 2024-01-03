using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderSupervisorApproval
{
    public class SupervisorRejectWorkOrder : ViewModel<WorkOrder>
    {
        #region Properties

        [Required, Multiline]
        [DoesNotAutoMap]
        public string RejectionReason { get; set; }

        #endregion

        #region Constructor

        public SupervisorRejectWorkOrder(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            // No reason to call the base MapToEntity.

            // One a supervisor rejects a work order, it needs to be recompleted 
            // before it can go back to supervisor approval.
            entity.CompletedBy = null;
            entity.DateCompleted = null;

            var dtp = _container.GetInstance<IDateTimeProvider>();
            var authServ = _container.GetInstance<IAuthenticationService<User>>();
            entity.DateRejected = dtp.GetCurrentDate();
            entity.Notes = $"{entity.Notes} {Environment.NewLine} {dtp.GetCurrentDate()} {authServ.CurrentUser.UserName}: {RejectionReason}";

            return entity;
        }

        private IEnumerable<ValidationResult> ValidateWorkOrderIsNotAlreadyApproved()
        {
            var workOrder = _container.GetInstance<IRepository<WorkOrder>>().Find(Id);
            if (workOrder.ApprovedOn.HasValue)
            {
                // Do this as a top-level validation result because the textbox for this is inside a dialog.
                yield return new ValidationResult("Can not reject a work order that has already been approved.");
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateWorkOrderIsNotAlreadyApproved());
        }

        #endregion
    }
}