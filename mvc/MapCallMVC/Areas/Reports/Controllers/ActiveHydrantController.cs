using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    // Original MapCall page only required user to be logged in. No roles required.
    [DisplayName("BPU Active Hydrants Report")]
    public class ActiveHydrantController : ControllerBaseWithPersistence<IHydrantRepository, Hydrant, User>
    {
        #region Constructor

        public ActiveHydrantController(ControllerBaseWithPersistenceArguments<IHydrantRepository, Hydrant, User> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
        }

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchActiveHydrantReport>();
        }

        [HttpGet]
        public ActionResult Index(SearchActiveHydrantReport model)
        {
            return ActionHelper.DoIndex(model, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => { Repository.GetActiveHydrantCounts(model); }
            });
        }

        #endregion
    }
}