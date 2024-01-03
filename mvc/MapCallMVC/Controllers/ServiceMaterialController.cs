using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class ServiceMaterialController : EntityLookupControllerBase<IRepository<ServiceMaterial>, ServiceMaterial>
    {
        #region Constructors

        public ServiceMaterialController(ControllerBaseWithPersistenceArguments<IRepository<ServiceMaterial>, ServiceMaterial, User> args) : base(args) {}

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Show)
            {
                this.AddOperatingCenterDropDownData();
            }
        }

        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult ByOperatingCenterId(int id)
        {
            return new CascadingActionResult(Repository.Where(x => x.OperatingCentersServiceMaterials.Any(y => y.OperatingCenter.Id == id)), "Description", "Id");
        }

        [HttpGet]
        public ActionResult ByOperatingCenterIdNewServices(int id)
        {
            return new CascadingActionResult(Repository.Where(x => x.OperatingCentersServiceMaterials.Any(y => y.OperatingCenter.Id == id && y.NewServiceRecord)), "Description", "Id");
        }

        #endregion

        [HttpGet]
        public override ActionResult Index()
        {
            // ActionHelper doesn't have DoIndex overrides with results and args so using DoView. 
            // We also have new stories coming to update add/edit pages so will not be 
            // inheriting from EntityLookupControllerBase and those views need to be defined manually.
            // Once added and inherited from ControllerBaseWithPersistence, we can remove this comment and
            // use ActionHelper.DoIndex(Repository.GetAll().ToList())
            return DoView("~/Views/ServiceMaterial/Index.cshtml", Repository.GetAll().ToList());
        }

        [HttpGet] // TODO: This doesn't require a role?
        public override ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                ViewName = "~/Views/ServiceMaterial/Show.cshtml"
            });
        }

        #endregion
    }
}