using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Helpers;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class AssetReliabilityController : ControllerBaseWithPersistence<AssetReliability, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionAssetReliability;

        #endregion

        #region Constructor

        public AssetReliabilityController(ControllerBaseWithPersistenceArguments<IRepository<AssetReliability>, AssetReliability, User> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDynamicDropDownData<Equipment, EquipmentDisplayItem>(filter: x => x.AssetReliabilities.Any());
                    this.AddDynamicDropDownData<Employee, EmployeeDisplayItem>(filter: x => x.User != null && x.User.Roles.Any(y => y.Module.Id == (int)ROLE));
                    this.AddDropDownData<EquipmentLifespan>(keyGetter: x => x.Id, valueGetter: x => x.Description);
                    break;
            }

            this.AddDropDownData<ConfinedSpaceFormEntrantType>("NewEntrants.EntrantType");
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchAssetReliability search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchAssetReliability search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [ActionBarVisible(false)]
        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? productionWorkOrderId = null, int? equipmentId = null)
        {
            if (productionWorkOrderId == null && equipmentId == null)
            {
                return HttpNotFound();
            }

            var vm = ViewModelFactory.Build<CreateAssetReliability>();
            vm.ProductionWorkOrder = productionWorkOrderId;
            vm.Equipment = equipmentId;

            return ActionHelper.DoNew(vm);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateAssetReliability model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditAssetReliability>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditAssetReliability model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Copy

        [HttpGet, RequiresRole(ROLE, RoleActions.Add), Crumb(Action = "New")]
        public ActionResult Copy(int id)
        {
            var entity = Repository.Find(id);
            if (entity == null)
            {
                return DoHttpNotFound($"Could not find system delivery entry with id {id}.");
            }

            var viewModel = ViewModelFactory.Build<CreateAssetReliability>();
            viewModel.ProductionWorkOrder = entity.ProductionWorkOrder.Id;
            viewModel.Equipment = entity.Equipment.Id;

            return ActionHelper.DoNew(viewModel);
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}
