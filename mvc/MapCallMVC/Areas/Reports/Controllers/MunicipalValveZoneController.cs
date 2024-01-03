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
    public class MunicipalValveZoneController : ControllerBaseWithPersistence<IMunicipalValveZoneRepository, MunicipalValveZone, User>
    {
        #region Constructor

        public MunicipalValveZoneController(ControllerBaseWithPersistenceArguments<IMunicipalValveZoneRepository, MunicipalValveZone, User> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
        }

        [HttpGet]
        public ActionResult Search(SearchMunicipalValveZoneReport search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        public ActionResult Index(SearchMunicipalValveZoneReport model)
        {
            // disable for all 
            model.EnablePaging = false;
            var args = new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => { Repository.GetMunicipalValveZoneReportItems(model); }
            };

            return this.RespondTo((formatter) =>
            {
                model.EnablePaging = false;
                formatter.View(() => ActionHelper.DoIndex(model, args));
                formatter.Excel(() => ActionHelper.DoExcel(model, args));
            });
        }

        #endregion
    }
}