using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class ValveRouteController : ControllerBaseWithPersistence<IValveRepository, Valve, User>
    {
        #region Contructors

        public ValveRouteController(ControllerBaseWithPersistenceArguments<IValveRepository, Valve, User> args) : base(args) {}

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
        }

        [HttpGet]
        public ActionResult Search(SearchValveRouteReport search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        public ActionResult Index(SearchValveRouteReport model)
        {
            model.EnablePaging = false;
            var args = new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetRoutes(model)
            };
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(model, args));
                formatter.Excel(() => ActionHelper.DoExcel(model, args));
            });
        }

        #endregion
    }
}