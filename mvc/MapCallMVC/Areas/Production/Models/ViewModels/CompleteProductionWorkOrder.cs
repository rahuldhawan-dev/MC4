using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class CompleteProductionWorkOrder : ViewModel<ProductionWorkOrder>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(ProductionWorkOrderActionCode))]
        [RequiredWhen("Corrective", true, ErrorMessage = "Required for Corrective orders.")]
        public virtual int? ActionCode { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ProductionWorkOrderFailureCode))]
        [RequiredWhen("Corrective", true, ErrorMessage = "Required for Corrective orders.")]
        public virtual int? FailureCode { get; set; }

        [DoesNotAutoMap]
        public virtual bool Corrective { get; protected set; }

        #endregion

        #region Constructors

        public CompleteProductionWorkOrder(IContainer container) : base(container) { }

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
            entity.DateCompleted = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.CompletedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            if (entity.ProductionWorkDescription?.OrderType?.Id == OrderType.Indices.CORRECTIVE_ACTION_20 &&
                entity.ProductionWorkDescription?.Description == "REHAB/RENEW")
            {
                foreach (var pwEq in entity.Equipments)
                {
                    pwEq.Equipment.ExtendedUsefulLifeWorkOrderId = entity.Id;
                    pwEq.Equipment.LifeExtendedOnDate = entity.DateCompleted;
                    pwEq.Equipment.ExtendedUsefulLifeComment = entity.OrderNotes;
                }
            }
            return entity;
        }

        #endregion
    }
}