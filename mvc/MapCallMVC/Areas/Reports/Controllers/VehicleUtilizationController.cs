using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Vehicle Utilization Report")]
    public class VehicleUtilizationController : ControllerBaseWithPersistence<IVehicleRepository, Vehicle, User>
    {
        public VehicleUtilizationController(ControllerBaseWithPersistenceArguments<IVehicleRepository, Vehicle, User> args) : base(args) { }

        [HttpGet, RequiresRole(RoleModules.FleetManagementGeneral)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchVehicleUtilizationReport>();
        }

        [HttpGet, RequiresRole(RoleModules.FleetManagementGeneral)]
        public ActionResult Index(SearchVehicleUtilizationReport model)
        {
            model.EnablePaging = false;
            return ActionHelper.DoIndex(model, new MMSINC.Utilities.ActionHelperDoIndexArgs
            {
                SearchOverrideCallback = () => { Repository.SearchVehicleUtilization(model); }
            });
        }
    }
}