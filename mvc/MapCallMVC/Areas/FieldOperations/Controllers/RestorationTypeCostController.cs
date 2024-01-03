using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class RestorationTypeCostController : ControllerBaseWithPersistence<RestorationTypeCost, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchRestorationTypeCost search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchRestorationTypeCost search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditRestorationTypeCost>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditRestorationTypeCost model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresAdmin]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateRestorationTypeCost>());
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Create(CreateRestorationTypeCost model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        public RestorationTypeCostController(ControllerBaseWithPersistenceArguments<IRepository<RestorationTypeCost>, RestorationTypeCost, User> args) : base(args) { }
    }
}