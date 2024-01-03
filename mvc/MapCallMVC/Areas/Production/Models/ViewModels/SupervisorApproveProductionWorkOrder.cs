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

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SupervisorApproveProductionWorkOrder : ViewModel<ProductionWorkOrder>
    {
        #region Consts

        public const string CAUSE_CODE_ERROR_MESSAGE = "Required for Corrective orders.";

        #endregion

        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(ProductionWorkOrderCauseCode))]
        [RequiredWhen("Corrective", true, ErrorMessage = CAUSE_CODE_ERROR_MESSAGE)]
        public virtual int? CauseCode { get; set; }

        // This is a hidden field on the clientside.
        [DoesNotAutoMap, Required, Secured(AppliesToAdmins = true)]
        public virtual bool? Corrective { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        public virtual string WBSElement { get; set; }

        //It was requested that this field be added to the screen even though it doesn't have anything backing it at the moment.
        //There is ticket MC-5290 to populate it in the future.
        public virtual string AccountType { get; set; }

        #endregion

        #region Constructors

        public SupervisorApproveProductionWorkOrder(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateWorkOrderCanBeApproved()
        {
            var workOrder = _container.GetInstance<IRepository<ProductionWorkOrder>>().Find(Id);
            if (workOrder == null)
            {
                // Cut out early. Prior validation already handles the existence of the record.
                yield break;
            }

            if (!workOrder.CanBeSupervisorApproved)
            {
                yield return new ValidationResult($"Work Order #{workOrder.Id} can not be supervisor approved.");
            }
        }

        #endregion

        #region Exposed Methods

        public override void Map(ProductionWorkOrder entity)
        {
            base.Map(entity);
            Corrective = entity.ProductionWorkDescription?.OrderType?.Id == OrderType.Indices.CORRECTIVE_ACTION_20;
        }

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            entity = base.MapToEntity(entity);
            entity.ApprovedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.ApprovedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateWorkOrderCanBeApproved());
        }

        #endregion
    }
}