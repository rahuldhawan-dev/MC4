using System;
using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Water Usage for Flushing")]
    public class MonthlyFlushingWaterUsageController :  ControllerBaseWithPersistence<IHydrantInspectionRepository, HydrantInspection, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructor
        
        public MonthlyFlushingWaterUsageController(ControllerBaseWithPersistenceArguments<IHydrantInspectionRepository, HydrantInspection, User> args) : base(args) {}
        
        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownData("OperatingCenter");
            }
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchHydrantFlushing() {
                Year = DateTime.Now.Year
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchHydrantFlushing model)
        {
            // Needed for display only. 
            ViewData["FullOperatingCenter"] = _container.GetInstance<IOperatingCenterRepository>().Find(model.OperatingCenter.GetValueOrDefault());
            return ActionHelper.DoIndex(model, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetFlushingReport(model)
            });
        }
        #endregion
    }
}