using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Results;

namespace MapCallMVC.Controllers
{
    public class PositionGroupController : ControllerBaseWithPersistence<PositionGroup, User>
    {
        #region Constants

        private const RoleModules ROLE = RoleModules.HumanResourcesPositions;

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchPositionGroup>();
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchPositionGroup model)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(model));
                f.Excel(() => ActionHelper.DoExcel(model));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreatePositionGroup(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreatePositionGroup model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditPositionGroup>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditPositionGroup model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        [HttpGet]
        public ActionResult GetByCommonName(int commonNameId)
        {
            return new CascadingActionResult<PositionGroup, PositionGroupDisplayItem>(Repository.Where(g => g.CommonName.Id == commonNameId));
        }

        public PositionGroupController(ControllerBaseWithPersistenceArguments<IRepository<PositionGroup>, PositionGroup, User> args) : base(args) {}
    }
}
