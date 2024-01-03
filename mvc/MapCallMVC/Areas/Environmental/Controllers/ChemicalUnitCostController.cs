using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    public class ChemicalUnitCostController : ControllerBaseWithPersistence<ChemicalUnitCost, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchChemicalUnitCost search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchChemicalUnitCost search)
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
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateChemicalUnitCost>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateChemicalUnitCost model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditChemicalUnitCost>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditChemicalUnitCost model)
        {
            return ActionHelper.DoUpdate(model);
        }
		
        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

		#region Constructors

        public ChemicalUnitCostController(ControllerBaseWithPersistenceArguments<IRepository<ChemicalUnitCost>, ChemicalUnitCost, User> args) : base(args) {}

		#endregion

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult ByChemical(int chemicalId)
        {
            return new CascadingActionResult<ChemicalUnitCost, ChemicalUnitCostDisplayItem>(
                Repository.Where(uc => uc.Chemical.Id == chemicalId));
        }
    }
}