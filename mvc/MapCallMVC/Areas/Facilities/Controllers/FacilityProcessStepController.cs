using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class FacilityProcessStepController : ControllerBaseWithPersistence<FacilityProcessStep, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.ProductionFacilities;

        #endregion

        #region Constructors

        public FacilityProcessStepController(ControllerBaseWithPersistenceArguments<IRepository<FacilityProcessStep>, FacilityProcessStep, User> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDropDownData<FacilityProcessStepSubProcess>();
            this.AddDropDownData<UnitOfMeasure>();

            switch (action)
            {
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add, extraFilterP: x => x.IsActive);
                    break;

                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    break;

                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
                    break;
            }
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchFacilityProcessStep>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchFacilityProcessStep model)
        {
            return ActionHelper.DoIndex(model);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? facilityProcessId)
        {
            var model = new FacilityProcessStepViewModel(_container);
            if (facilityProcessId.HasValue)
            {
                var process = _container.GetInstance<IRepository<FacilityProcess>>().Find(facilityProcessId.Value);

                if (process == null)
                {
                    return HttpNotFound();
                }

                model.FacilityProcess = process.Id;
                model.Facility = process.Facility.Id;
                model.OperatingCenter = process.Facility.OperatingCenter.Id;
            }
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(FacilityProcessStepViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<FacilityProcessStepViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(FacilityProcessStepViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}