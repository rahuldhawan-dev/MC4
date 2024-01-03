using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderSupervisorApproval
{
    public class SupervisorApproveWorkOrder : ViewModel<WorkOrder>
    {
        #region Fields

        private WorkOrder _original;

        #endregion

        #region Properties

        [AutoMap(MapDirections.ToViewModel)]
        [StringLength(WorkOrder.StringLengths.ACCOUNT_CHARGED)]
        public string AccountCharged { get; set; }

        [DoesNotAutoMap]
        public bool OperatingCenterHasWorkOrderInvoicing
        {
            get { return GetWorkOrderEntity().OperatingCenter.HasWorkOrderInvoicing; }
        }

        // *Not* required, but can only be set when OperatingCenterHasWorkOrderInvoicing == true.
        [AutoMap(MapDirections.ToViewModel)]
        public bool? RequiresInvoice { get; set; }

        #endregion

        #region Constructor

        public SupervisorApproveWorkOrder(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        public WorkOrder GetWorkOrderEntity()
        {
            if (_original == null)
            {
                _original = _container.GetInstance<IRepository<WorkOrder>>().Find(Id);
            }

            return _original;
        }

        #endregion

        #region Public Methods

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            // Do not call base.MapToEntity. All mapping must be done manually for this model.

            entity.ApprovedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            entity.ApprovedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();

            if (OperatingCenterHasWorkOrderInvoicing)
            {
                entity.RequiresInvoice = RequiresInvoice;
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IEnumerable<ValidationResult> validateWorkOrderCanBeApproved()
            {
                var entity = GetWorkOrderEntity();
                if (!entity.CanBeApproved)
                {
                    // NOTE: Everything being returned in here is also what's checked for in the CanBeApproved 
                    // property. Spitting out all of these messages should hopefully reduce the amount of questions
                    // from the business if/when they can't approve an order.
                    if (entity.CancelledAt.HasValue)
                    {
                        yield return new ValidationResult("This work order has been cancelled and can not be approved.");
                    }
                    else if (!entity.DateCompleted.HasValue)
                    {
                        yield return new ValidationResult("This work order must be completed before it can be approved.");
                    }
                    else if (!entity.IsSAPValid)
                    {
                        yield return new ValidationResult("This work order must have an SAP Work Order number before it can be approved.");
                    }
                    else if (entity.HasServiceApprovalIssue)
                    {
                        yield return new ValidationResult("A service must be attached to this work order. That service must have a valid Date Installed value.");
                    }
                    else if (entity.HasInvestigativeWorkDescriptionApprovalIssue)
                    {
                        yield return new ValidationResult("Unable to approve a work order with an investigative work order description.");
                    }
                    else if (entity.HasSAPNotReleased)
                    {
                        yield return new ValidationResult("Unable to approve a work order when SAP has not released or has rejected the release.");
                    }
                    else if (entity.HasAssetTypeError)
                    {
                        yield return new ValidationResult("Unable to approve a work order when its asset type does not match its work description's asset type.");
                    }
                    else
                    {
                        yield return new ValidationResult("This work order can not be approved. A reason could not be provided.");
                    }
                }
                else if (entity.ApprovedOn.HasValue)
                {
                    yield return new ValidationResult("This work order has already been approved.");
                }
            }

            return base.Validate(validationContext).Concat(validateWorkOrderCanBeApproved());
        }

        #endregion
    }
}