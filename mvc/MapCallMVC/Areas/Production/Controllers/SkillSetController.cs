using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class SkillSetController : ControllerBaseWithPersistence<IRepository<SkillSet>, SkillSet, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionDataAdministration;
        
        #endregion

        #region Constructor

        public SkillSetController(ControllerBaseWithPersistenceArguments<IRepository<SkillSet>, SkillSet, User> args) : base(args) {}

        #endregion

        #region Delete

        [HttpDelete]
        [RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchSkillSet search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchSkillSet search)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<SkillSetViewModel>());
        }

        [HttpPost]
        [RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(SkillSetViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<SkillSetViewModel>(id, null, entity => { });
        }

        [HttpPost]
        [RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(SkillSetViewModel model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs());
        }

        #endregion
    }
}