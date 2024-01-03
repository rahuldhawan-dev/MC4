using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Production.Controllers
{
    [DisplayName("Scheduling Production Work")]
    public class SchedulingController : ControllerBaseWithPersistence<IProductionWorkOrderRepository, ProductionWorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = ProductionWorkOrderController.ROLE;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddDropDownData<ProductionSkillSet>();
                this.AddDropDownData<OrderType>();
                this.AddDropDownData<MaintenancePlanTaskType>(x => x.Where(y => y.IsActive), x => x.Id, x => x.Description);
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchSchedulingProductionWorkOrder());
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Index(SearchSchedulingProductionWorkOrder search)
        {
            search.EnablePaging = false;
            this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();

            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.SearchForDistinct(search)
                }));
            });
        }

        #endregion

        public SchedulingController(ControllerBaseWithPersistenceArguments<IProductionWorkOrderRepository, ProductionWorkOrder, User> args) : base(args) { }
    }
}