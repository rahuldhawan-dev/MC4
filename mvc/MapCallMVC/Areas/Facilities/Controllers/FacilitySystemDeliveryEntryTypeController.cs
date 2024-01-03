using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class FacilitySystemDeliveryEntryTypeController : ControllerBaseWithPersistence<FacilitySystemDeliveryEntryType, User>
    {
        #region Constants

        private const RoleModules ROLE = RoleModules.ProductionSystemDeliveryConfiguration;

        #endregion

        #region Constructors

        public FacilitySystemDeliveryEntryTypeController(ControllerBaseWithPersistenceArguments<IRepository<FacilitySystemDeliveryEntryType>, FacilitySystemDeliveryEntryType, User> args) : base(args) { }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, extraFilterP: x => x.IsActive);
                    break;
            }

        }

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditFacilitySystemDeliveryEntryType>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditFacilitySystemDeliveryEntryType model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToRouteWithTabSelected(new { Action="Show", Controller="Facility", Area = "", Id = model.Facility }, "SystemDeliveryTab")
            });
        }

        #endregion  

        #endregion
    }
}
