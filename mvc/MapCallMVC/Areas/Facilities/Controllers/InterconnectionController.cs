using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    [DisplayName("Interconnection Details")]
    public class InterconnectionController : ControllerBaseWithPersistence<IInterconnectionRepository, Interconnection, User>
    {
        #region Constants

        public const RoleModules Role = RoleModules.ProductionInterconnections;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    break;
                case ControllerAction.Edit:
                    // The view model specifically references this twice so it only queries the PWS's once.
                    this.AddDynamicDropDownData<PublicWaterSupply, PublicWaterSupplyDisplayItem>();
                    this.AddOperatingCenterDropDownData();
                    break;
                case ControllerAction.New:
                    // The view model specifically references this twice so it only queries the PWS's once.
                    this.AddDynamicDropDownData<PublicWaterSupply, PublicWaterSupplyDisplayItem>();
                    this.AddOperatingCenterDropDownData(x => x.IsActive);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(Role, RoleActions.Read)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchInterconnection());
        }

        [HttpGet, RequiresRole(Role, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(Role, RoleActions.Read)]
        public ActionResult Index(SearchInterconnection search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(Role, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateInterconnection(_container));
        }

        [HttpPost, RequiresRole(Role, RoleActions.Add)]
        public ActionResult Create(CreateInterconnection model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(Role, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditInterconnection>(id);
        }

        [HttpPost, RequiresRole(Role, RoleActions.Edit)]
        public ActionResult Update(EditInterconnection model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        public InterconnectionController(ControllerBaseWithPersistenceArguments<IInterconnectionRepository, Interconnection, User> args) : base(args) { }
    }
}
