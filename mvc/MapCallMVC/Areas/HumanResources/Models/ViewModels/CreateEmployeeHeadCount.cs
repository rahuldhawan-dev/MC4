using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.HumanResources.Models.ViewModels
{
    public class CreateEmployeeHeadCount : EmployeeHeadCountViewModel
    {
        #region Constructor

        public CreateEmployeeHeadCount(IContainer container) : base(container) { }

        #endregion

        public override EmployeeHeadCount MapToEntity(EmployeeHeadCount entity)
        {
            base.MapToEntity(entity);

            entity.CreatedAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.CreatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;

            return entity;
        }
    }
}