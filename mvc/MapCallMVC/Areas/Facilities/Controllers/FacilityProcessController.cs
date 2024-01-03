using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class FacilityProcessController : ControllerBaseWithPersistence<FacilityProcess, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.ProductionFacilities;

        #endregion

        #region Constructor

        public FacilityProcessController(ControllerBaseWithPersistenceArguments<IRepository<FacilityProcess>, FacilityProcess, User> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    this.AddDropDownData<ProcessStage>();
                    break;
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add, extraFilterP: x => x.IsActive);
                    this.AddDropDownData<ProcessStage>();
                    break;
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Read);
                    this.AddDropDownData<ProcessStage>();
                    break;
            }
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchFacilityProcess>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchFacilityProcess search)
        {
            return ActionHelper.DoIndex(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? facilityId = null)
        {
            var model = new FacilityProcessViewModel(_container);
            // If facilityId has sa value then we also need to get the operating center, or else
            // the cascading dropdown won't work correctly.
            if (facilityId.HasValue)
            {
                var facility = _container.GetInstance<IFacilityRepository>().Find(facilityId.Value);

                if (facility == null)
                {
                    return HttpNotFound();
                }

                model.Facility = facilityId;
                model.OperatingCenter = facility.OperatingCenter.Id;
            }

            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(FacilityProcessViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id, bool fromFacilityPage = false)
        {
            // TODO: What is this fromFacilityPage parameter for? It isn't used anywhere.
            return ActionHelper.DoEdit<FacilityProcessViewModel>(id);
        }

        [HttpPost,RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(FacilityProcessViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete,RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult ByFacilityId(int id)
        {
            var processes = Enumerable.Empty<FacilityProcess>();
            var repo = _container.GetInstance<IFacilityRepository>();
            var facility = repo.Find(id);
            if (facility != null)
            {
                processes = facility.FacilityProcesses;
            }

            return new CascadingActionResult(processes, "Description", "Id");
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveFacilityProcessStep(RemoveFacilityProcessStep model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion
    }
}