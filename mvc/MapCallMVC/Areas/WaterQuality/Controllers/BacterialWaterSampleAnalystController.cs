using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class BacterialWaterSampleAnalystController : ControllerBaseWithPersistence<IBacterialWaterSampleAnalystRepository, BacterialWaterSampleAnalyst, User>
    {
        #region Consts

        public const RoleModules ROLE = RoleModules.WaterQualityGeneral;

        #endregion

        #region Constructors

        public BacterialWaterSampleAnalystController(ControllerBaseWithPersistenceArguments<IBacterialWaterSampleAnalystRepository, BacterialWaterSampleAnalyst, User> args) : base(args) { }
        
        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.Show:
                    // This is needed for the AddOperatingCenter partial. Automatically populating dropdowns
                    // does not currently work when a partial view is using a different model than the main
                    // view.
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(
                        "OperatingCenters");
                    break;
            }
        }

        #region Show/Index/Search

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Index()
        {
            var results = Repository.GetAll().OrderBy(x => x.Employee == null ? null : x.Employee.LastName).ToList();
            return ActionHelper.DoIndexWithResults(results);
        }

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateBacterialWaterSampleAnalyst(_container));
        }

        [HttpPost]
        [RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Create(CreateBacterialWaterSampleAnalyst model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditBacterialWaterSampleAnalyst>(id);
        }

        [HttpPost]
        [RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Update(EditBacterialWaterSampleAnalyst model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost]
        [RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult AddOperatingCenter(
            AddBacterialWaterSampleAnalystOperatingCenter model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost]
        [RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult RemoveOperatingCenter(
            RemoveBacterialWaterSampleAnalystOperatingCenter model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Destroy

        [HttpDelete]
        [RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Cascades
        
        [HttpGet]
        public ActionResult GetActiveByOperatingCenter(int id)
        {
            var results = Repository.GetAllActiveByOperatingCenter(id);
            return new CascadingActionResult<BacterialWaterSampleAnalyst, BacterialWaterSampleAnalystDisplayItem>(results, "Display", "Id");
        }

        [HttpGet]
        public ActionResult GetByOperatingCenter(int id)
        {
            var results = Repository.GetAllByOperatingCenter(id);
            return new CascadingActionResult<BacterialWaterSampleAnalyst, BacterialWaterSampleAnalystDisplayItem>(results, "Display", "Id");
        }
        
        #endregion

        #endregion
    }
}