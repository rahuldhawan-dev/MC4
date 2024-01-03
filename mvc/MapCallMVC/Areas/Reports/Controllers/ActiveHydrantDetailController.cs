using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("BPU Active Hydrant Detail Report")]
    public class ActiveHydrantDetailController : ControllerBaseWithPersistence<IHydrantRepository, Hydrant, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructor

        public ActiveHydrantDetailController(ControllerBaseWithPersistenceArguments<IHydrantRepository, Hydrant, User> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
            this.AddDropDownData<LateralSize>();
            this.AddDropDownData<HydrantSize>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchActiveHydrantDetailReport>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchActiveHydrantDetailReport model)
        {
            model.EnablePaging = false;
            return ActionHelper.DoIndex(model, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetActiveHydrantDetailCounts(model)
            });
        }

        #endregion

    }
}