using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    //TODO: This is main breaks, not work orders.
    public class MainBreakPowerPlantController : ControllerBaseWithPersistence<IMainBreakRepository, MainBreak, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
                    break;
            }
            base.SetLookupData(action);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.FieldServicesWorkManagement)]
        public ActionResult Search(SearchMainBreakPowerPlant search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesWorkManagement)]
        public ActionResult Index(SearchMainBreakPowerPlant search)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback =  () => Repository.GetPowerPlantMainBreaks(search)
                }));
                f.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.GetPowerPlantMainBreaks(search).Select(x => new {
                        x.WorkOrder.OperatingCenter,
                        x.WorkOrder,
                        x.WorkOrder.DateCompleted,
                        x.WorkOrder.Town,
                        Size = x.ServiceSize,
                        FootageInstalled = x.FootageReplaced,
                        x.ReplacedWith,
                        ExistingMaterial = x.MainBreakMaterial
                    });
                    return this.Excel(results, new MMSINC.Utilities.Excel.ExcelExportSheetArgs {
                        SheetName = "Power Plant Main Break Report",
                        Header = "Power Plant Main Break Report"
                    });
                });
            });
        }

        #endregion

        #region Constructor

        public MainBreakPowerPlantController(ControllerBaseWithPersistenceArguments<IMainBreakRepository, MainBreak, User> args) : base(args) {}

        #endregion

    }
}