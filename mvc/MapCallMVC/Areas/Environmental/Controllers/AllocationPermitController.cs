using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    [DisplayName("Allocation Groupings")]
    public class AllocationPermitController : ControllerBaseWithPersistence<IRepository<AllocationPermit>, AllocationPermit, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Edit:
                case ControllerAction.Search:
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownData();
                    this.AddDropDownData<EnvironmentalPermit>(d => d.GetAllSorted(x => x.PermitNumber).Where(x => x.EnvironmentalPermitType.Id == EnvironmentalPermitType.Indices.ALLOCATION_PERMIT), d => d.Id, d => d.PermitNumber);
                    break;
            }

            switch (action)
            {
                case ControllerAction.Edit:
                case ControllerAction.Search:
                    this.AddDynamicDropDownData<PublicWaterSupply, PublicWaterSupplyDisplayItem>();
                    break;
                case ControllerAction.New:
                    this.AddDynamicDropDownData<PublicWaterSupply, PublicWaterSupplyDisplayItem>(filter: x => x.Status.ToString() == "Active");
                    this.AddDynamicDropDownData<PublicWaterSupply, PublicWaterSupplyDisplayItem>(filter: x => x.Status.Id == PublicWaterSupplyStatus.Indices.ACTIVE);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.EnvironmentalGeneral)]
        public ActionResult Search(SearchAllocationPermit search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(RoleModules.EnvironmentalGeneral)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(RoleModules.EnvironmentalGeneral)]
        public ActionResult Index(SearchAllocationPermit search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateAllocationPermit(_container));
        }

        [HttpPost, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Add)]
        public ActionResult Create(CreateAllocationPermit model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditAllocationPermit>(id);
        }

        [HttpPost, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Edit)]
        public ActionResult Update(EditAllocationPermit model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public AllocationPermitController(ControllerBaseWithPersistenceArguments<IRepository<AllocationPermit>, AllocationPermit, User> args) : base(args) {}
    }
}
