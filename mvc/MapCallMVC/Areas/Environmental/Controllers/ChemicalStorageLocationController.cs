using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using System.Web.Mvc;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using MMSINC;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    public class ChemicalStorageLocationController : ControllerBaseWithPersistence<ChemicalStorageLocation, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalChemicalData;

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchChemicalStorageLocation search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchChemicalStorageLocation search)
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
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateChemicalStorageLocation>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateChemicalStorageLocation model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditChemicalStorageLocation>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditChemicalStorageLocation model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult ByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult(Repository.Where(x => x.OperatingCenter.Id == operatingCenterId), "StorageLocationDescription", "Id");
        }

        #endregion

        #region Constructors

        public ChemicalStorageLocationController(ControllerBaseWithPersistenceArguments<IRepository<ChemicalStorageLocation>, ChemicalStorageLocation, User> args) : base(args) { }

        #endregion
    }
}
