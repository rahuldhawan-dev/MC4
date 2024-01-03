using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class OperatingCenterStockedMaterialController : ControllerBaseWithPersistence<OperatingCenterStockedMaterial, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesMaterials;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
                    break;
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add);
                    this.AddDynamicDropDownData<Material, MaterialDisplayItem>(m => m.Id, m => m.Display, filter: m => m.IsActive);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchOperatingCenterStockedMaterial search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchOperatingCenterStockedMaterial search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateOperatingCenterStockedMaterial(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateOperatingCenterStockedMaterial model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs
            {
                OnSuccess = () => DoRedirectionToAction("Index", null)
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                // NOTE: Redirects to Index because the Delete button exists there.
                // Having said that, the page they were on is lost when they do that.
                OnSuccess = () => RedirectToAction("Index")
            });
        }

        #endregion

        #region StockMaterialsSearchByOperatingCenterId

        [HttpGet]
        public ActionResult StockMaterialSearchByOperatingCenter(string search, int operatingCenterId)
        {
            var materials = Repository.Where(x =>
                x.OperatingCenter != null && x.OperatingCenter.Id == operatingCenterId &&
                x.Material.IsActive && 
                (x.Material.PartNumber.Contains(search) || 
                 x.Material.Description.Contains(search))).Select(x => x.Material).ToList();
            var results = new SelectList(materials, "Id", "FullDescription");
            return Json(new { Result = "OK", Options = results }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public OperatingCenterStockedMaterialController(ControllerBaseWithPersistenceArguments<IRepository<OperatingCenterStockedMaterial>, OperatingCenterStockedMaterial, User> args) : base(args) {}
    }
}