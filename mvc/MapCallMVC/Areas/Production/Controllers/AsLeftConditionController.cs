using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.Controllers;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Production.Controllers
{
    [ActionHelperViewVirtualPathFormat("~/Areas/Production/Views/AsLeftCondition/{0}.cshtml")]
    public class AsLeftConditionController : EntityLookupControllerBase<IRepository<AsLeftCondition>, AsLeftCondition, AsLeftConditionViewModel>
    {
        #region Exposed Methods

        public override RoleModules GetDynamicRoleModuleForAction(string action)
        {
            return RoleModules.ProductionDataAdministration;
        }

        #endregion

        #region Constructor 

        public AsLeftConditionController(ControllerBaseWithPersistenceArguments<IRepository<AsLeftCondition>, AsLeftCondition, User> args) : base(args) { }

        #endregion
    }
}