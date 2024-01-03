using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using System.Web.Mvc;
using MMSINC.Results;

namespace MapCallMVC.Controllers
{
    public class MaterialController : ControllerBaseWithPersistence<MaterialRepository, Material, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesMaterials;

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchMaterial search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchMaterial search)
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
            return ActionHelper.DoNew(new CreateMaterial(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateMaterial model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditMaterial>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditMaterial model)
        {
            return ActionHelper.DoUpdate(model);
        }
		
        #endregion

        #region FindByPartialPartNumberOrDescription

        [HttpGet]
        public ActionResult FindByPartialPartNumberOrDescription(string partial)
        {
            var results = Repository.FindByPartialPartNumberOrDescription(partial);
            return new AutoCompleteResult(results, "Id", "FullDescription");
        }

        #endregion

        public MaterialController(ControllerBaseWithPersistenceArguments<MaterialRepository, Material, User> args) : base(args) {}
    }
}