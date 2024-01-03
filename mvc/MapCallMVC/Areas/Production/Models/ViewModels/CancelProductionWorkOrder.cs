using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels {
    public class CancelProductionWorkOrder : ViewModel<ProductionWorkOrder>
    {
        public CancelProductionWorkOrder(IContainer container) : base(container) { }

        [DropDown, EntityMap, EntityMustExist(typeof(ProductionWorkOrderCancellationReason))]
        [Required]
        public int? CancellationReason { get; set; }

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            base.MapToEntity(entity);
            entity.DateCancelled = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.CancelledBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            return base.MapToEntity(entity);
        }
    }
}