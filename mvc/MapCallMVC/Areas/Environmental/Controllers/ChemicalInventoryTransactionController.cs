using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    public class ChemicalInventoryTransactionController : ControllerBaseWithPersistence<ChemicalInventoryTransaction, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchChemicalInventoryTransaction search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchChemicalInventoryTransaction search)
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
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateChemicalInventoryTransaction>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateChemicalInventoryTransaction model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditChemicalInventoryTransaction>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditChemicalInventoryTransaction model)
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

        public ChemicalInventoryTransactionController(ControllerBaseWithPersistenceArguments<IRepository<ChemicalInventoryTransaction>, ChemicalInventoryTransaction, User> args) : base(args) {}

		#endregion
    }
}