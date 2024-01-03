using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Controllers
{
    [ActionHelperViewVirtualPathFormat("~/Views/EquipmentLifespan/{0}.cshtml")]
    public class EquipmentLifespanController : EntityLookupControllerBase<IRepository<EquipmentLifespan>, EquipmentLifespan, EquipmentLifespanViewModel>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownData();
                break;
            }
        }

        public override RoleModules GetDynamicRoleModuleForAction(string action)
        {
            // MC-2652 specifically asked for the Edit/Update actions only to be
            // limited by this role. The rest should still allow for DataLookups.
            // I will not be surprised if they ask for this to be changed. -Ross 1/22/2021
            if (action == nameof(New) || action == nameof(Edit) || action == nameof(Update))
            {
                return RoleModules.EngineeringEAMAssetManagement;
            }

            return base.GetDynamicRoleModuleForAction(action);
        }

        #endregion
        
        public EquipmentLifespanController(ControllerBaseWithPersistenceArguments<IRepository<EquipmentLifespan>, EquipmentLifespan, User> args) : base(args) { }
    }
}
