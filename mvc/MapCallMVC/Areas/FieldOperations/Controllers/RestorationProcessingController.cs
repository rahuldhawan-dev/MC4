using System;
using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    [DisplayName("Restoration Processing")]
    public class RestorationProcessingController : ControllerBaseWithPersistence<IGeneralWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Constructors

        public RestorationProcessingController(
            ControllerBaseWithPersistenceArguments<IGeneralWorkOrderRepository, WorkOrder, User> args) : base(args) { }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownData();
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchRestorationProcessing search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchRestorationProcessing search)
        {
            return ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs
            {
                SearchOverrideCallback = () => Repository.SearchRestorationOrders(search)
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region Edit

        [RequiresRole(ROLE, RoleActions.Edit)]
        [HttpGet, ActionBarVisible(false)]
        public void Edit(int id)
        {
            // noop: edit action only needed so user can edit/add documents
        }

        #endregion
    }
}