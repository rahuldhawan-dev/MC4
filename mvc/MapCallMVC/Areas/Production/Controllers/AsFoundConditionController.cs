using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.Controllers;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Production.Controllers
{
    [ActionHelperViewVirtualPathFormat("~/Areas/Production/Views/AsFoundCondition/{0}.cshtml")]
    public class AsFoundConditionController : EntityLookupControllerBase<IRepository<AsFoundCondition>, AsFoundCondition, AsFoundConditionViewModel>
    {
        #region Exposed Methods

        public override RoleModules GetDynamicRoleModuleForAction(string action)
        {
            return RoleModules.ProductionDataAdministration;
        }

        #endregion

        #region Constructor 

        public AsFoundConditionController(ControllerBaseWithPersistenceArguments<IRepository<AsFoundCondition>, AsFoundCondition, User> args) : base(args) { }

        #endregion
    }
}