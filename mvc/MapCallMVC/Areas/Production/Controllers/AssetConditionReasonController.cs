using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Controllers;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Production.Controllers
{
    [ActionHelperViewVirtualPathFormat("~/Areas/Production/Views/AssetConditionReason/{0}.cshtml")]
    public class AssetConditionReasonController : EntityLookupControllerBase<IRepository<AssetConditionReason>, AssetConditionReason, AssetConditionReasonViewModel>
    {
        #region Exposed Methods

        public override RoleModules GetDynamicRoleModuleForAction(string action)
        {
            return RoleModules.ProductionDataAdministration;
        }

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                case ControllerAction.Search:
                    this.AddDropDownData<ConditionType>();
                    break;
            }
        }

        #endregion

        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionDataAdministration;

        #endregion

        #region Constructor

        public AssetConditionReasonController(
            ControllerBaseWithPersistenceArguments<IRepository<AssetConditionReason>, AssetConditionReason, User> args,
            IRepository<ConditionDescription> conditionDescriptionRep) : base(args) {}

        #endregion
    }
}