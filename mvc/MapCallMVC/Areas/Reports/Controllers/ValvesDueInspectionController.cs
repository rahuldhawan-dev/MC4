using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class ValvesDueInspectionController : ControllerBaseWithPersistence<IValveRepository, Valve, User>
    {
        #region Constructor

        public ValvesDueInspectionController(ControllerBaseWithPersistenceArguments<IValveRepository, Valve, User> args) : base(args) {}

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
        }

        #endregion

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchValvesDueInspectionReport>();
        }

        [HttpGet]
        public ActionResult Index(SearchValvesDueInspectionReport model)
        {
            model.EnablePaging = false;
            return ActionHelper.DoIndex(model, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetValvesDueInspection(model)
            });
        }
    }
}