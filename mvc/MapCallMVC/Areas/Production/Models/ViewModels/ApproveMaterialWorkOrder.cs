using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels {
    public class ApproveMaterialWorkOrder : ViewModel<ProductionWorkOrder>
    {
        public ApproveMaterialWorkOrder(IContainer container) : base(container) { }

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            entity.MaterialsApprovedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.MaterialsApprovedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            return entity;
        }
    }
}