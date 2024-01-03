using System;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class UnionController : ControllerBaseWithPersistence<Union, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.HumanResourcesUnion;
        public const string UNION_NOT_FOUND = "Union not found.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<Union>(nameof(SearchUnion.EntityId), f => f.GetAllSorted(x => x.BargainingUnit), f => f.Id, f => f.BargainingUnit);
                break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchUnion search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchUnion search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    RedirectSingleItemToShowView = true
                }));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.HumanResourcesUnion, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateUnion(_container));
        }

        [HttpPost, RequiresRole(RoleModules.HumanResourcesUnion, RoleActions.Add)]
        public ActionResult Create(CreateUnion model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.HumanResourcesUnion, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditUnion>(id);
        }

        [HttpPost, RequiresRole(RoleModules.HumanResourcesUnion, RoleActions.Edit)]
        public ActionResult Update(EditUnion union)
        {
            return ActionHelper.DoUpdate(union);
        }

        #endregion

        public UnionController(ControllerBaseWithPersistenceArguments<IRepository<Union>, Union, User> args) : base(args) {}
    }
}
