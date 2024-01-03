using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class MainBreaksAndServiceLineRepairsByMonthController : ControllerBaseWithPersistence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
                    this.AddDropDownData("YearCompleted", _container.GetInstance<IWorkOrderRepository>().GetDistinctYearsCompleted().OrderBy(x => x) , x => x, x => x);
                    break;
            }
            base.SetLookupData(action);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.FieldServicesWorkManagement)]
        public ActionResult Search(SearchMainBreaksAndServiceLineRepairsByMonth search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesWorkManagement)]
        public ActionResult Index(SearchMainBreaksAndServiceLineRepairsByMonth search)
        {
            // TODO: ActionHelper doesn't know how to handle ISearchSets that use a
            // different model type for the return type.
            var model = Repository.SearchMainBreaksAndServiceLineRepairsReport(search);
            return this.RespondTo(f => {
                f.View(() => View(model));
                f.Excel(() => this.Excel(model));
            });
        }

        #endregion

        public MainBreaksAndServiceLineRepairsByMonthController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args) : base(args) {}
    }
}
